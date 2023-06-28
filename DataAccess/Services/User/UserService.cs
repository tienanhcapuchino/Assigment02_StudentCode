﻿using BussinessObject.Entities;
using BussinessObject.Models;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccess.Services
{
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AdminAccount _admin;
        private readonly JWTSetting _jwtSetting;
        public UserService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            IOptionsMonitor<AdminAccount> admin,
            IOptionsMonitor<JWTSetting> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _admin = admin.CurrentValue;
            _jwtSetting = jwt.CurrentValue;
        }

        public async Task<APIResponeModel> Login(UserLoginModel model)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(_jwtSetting.Key);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
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
            var result = await ValidLogin(model);
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
            var userRoles = await _userManager.GetRolesAsync(user);
            result = new List<Claim>()
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new("UserId", user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            result.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
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
    }
}