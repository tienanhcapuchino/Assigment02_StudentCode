using AutoMapper;
using BussinessObject.Entities;
using BussinessObject.Models;
using DataAccess.DataContext;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AdminAccount _admin;
        private readonly JWTSetting _jwtSetting;
        private readonly EStoreDbContext _dbContext;
        private readonly IMapper _map;
        public UserService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptionsMonitor<AdminAccount> admin,
            IOptionsMonitor<JWTSetting> jwt,
            EStoreDbContext eStoreDb,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _admin = admin.CurrentValue;
            _jwtSetting = jwt.CurrentValue;
            _dbContext = eStoreDb;
            _map = mapper;
        }

        public async Task<APIResponeModel> Login(UserLoginModel model)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(_jwtSetting.Key);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var result = await ValidLogin(model);
            if (!result.IsSuccess)
            {
                return result;
            }
            var claims = await GetClaimsUsers(model);
            var tokenDecription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(3),
                Issuer = _jwtSetting.Issuer,
                Audience = _jwtSetting.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDecription);
            string accessToken = jwtTokenHandler.WriteToken(token);
            if (!result.IsSuccess) return result;
            result.Data = accessToken;
            return result;
        }

        public async Task<APIResponeModel> Register(UserRegisterModel model)
        {
            APIResponeModel result = new APIResponeModel()
            {
                Code = 200,
                IsSuccess = true,
                Message = "OK",
                Data = model
            };
            var userExistMail = await _userManager.FindByEmailAsync(model.Email);
            var userExistName = await _userManager.FindByNameAsync(model.Username);
            if (userExistMail != null || userExistName != null)
            {
                return new APIResponeModel()
                {
                    Code = 400,
                    Message = "User has been already existed!",
                    IsSuccess = false
                };
            }
            var user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var resultCreateUser = await _userManager.CreateAsync(user, model.Password);

            if (!resultCreateUser.Succeeded)
            {
                return new APIResponeModel()
                {
                    Code = 400,
                    Message = "Error when create user",
                    IsSuccess = false
                };
            }
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await _roleManager.RoleExistsAsync("User"))
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            return result;
        }

        private async Task<List<Claim>> GetClaimsUsers(UserLoginModel model)
        {
            List<Claim> result;
            if (model.Email.Equals(_admin.Username))
            {
                result = new List<Claim>()
                {
                    new (ClaimTypes.Email, _admin.Username),
                    new (ClaimTypes.Name, "Admin"),
                    new (ClaimTypes.Role, "Admin"),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                return result;
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            //var userRoles = await _userManager.GetRolesAsync(user);
            result = new List<Claim>()
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new("UserId", user.Id),
                new(ClaimTypes.Role, "User"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            //result.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
            return result;
        }

        private async Task<APIResponeModel> ValidLogin(UserLoginModel user)
        {
            var result = new APIResponeModel()
            {
                Code = 200,
                Message = "Ok",
                IsSuccess = true,
            };
            if (user.Email.Equals(_admin.Username))
            {
                if (!user.Password.Equals(_admin.Password))
                {
                    return new APIResponeModel
                    {
                        Code = 400,
                        IsSuccess = false,
                        Message = "Username or password is incorrect!",
                    };
                }
                else
                {
                    return result;
                }
            }
            var userIdentity = await _userManager.FindByEmailAsync(user.Email);
            if (userIdentity == null || !await _userManager.CheckPasswordAsync(userIdentity, user.Password))
            {
                return new APIResponeModel
                {
                    Code = 400,
                    IsSuccess = false,
                    Message = "Username or password is incorrect!",
                };
            }
            return result;
        }

        public async Task<APIResponeModel> UpdateProfile(string userId, UserProfileModel model)
        {
            APIResponeModel result = new APIResponeModel()
            {
                Code = 200,
                Message = "OK",
                Data = model,
                IsSuccess = true
            };
            var userEntity = await _userManager.FindByIdAsync(userId);
            if (userEntity == null)
            {
                return new APIResponeModel()
                {
                    Code = 404,
                    Message = $"cannot find user with userId: {userId}",
                    IsSuccess = false,
                    Data = model
                };
            }
            var userEmails = await _dbContext.Users.Where(x => x.Id.Equals(userId) && !x.Email.Equals(userEntity.Email) && x.Email.Equals(model.Email)).ToListAsync();
            if (userEmails != null && userEmails.Any())
                return new APIResponeModel()
                {
                    Code = 400,
                    Message = "Email has been already existed!",
                    Data = model,
                    IsSuccess = false,
                };
            //var userUpdate = _map.Map<User>(model);
            userEntity.Email = model.Email;
            userEntity.UserName = model.Username;
            userEntity.PhoneNumber = model.PhoneNumber;
            var resUpdate = await _userManager.UpdateAsync(userEntity);
            if (!resUpdate.Succeeded)
            {
                return new APIResponeModel()
                {
                    Code = 400,
                    Message = "update failed",
                    Data = resUpdate.Errors.ToList(),
                    IsSuccess = false,
                };
            }
            return result;
        }
        public async Task<UserProfileModel> GetUserProfile(string id)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            if (userEntity == null) return null;
            var result = _map.Map<UserProfileModel>(userEntity);
            return result;
        }
    }
}
