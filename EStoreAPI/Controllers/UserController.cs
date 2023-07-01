using BussinessObject.Models;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<APIResponeModel> Register([FromBody] UserRegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                         .Select(e => e.ErrorMessage)
                                         .ToList();
                    return new APIResponeModel()
                    {
                        Code = 400,
                        Data = errors,
                        IsSuccess = false,
                        Message = string.Join(";", errors)
                    };
                }
                var result = await _userService.Register(model);
                return result;
            }
            catch (Exception ex)
            {
                return new APIResponeModel()
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    Data = ex,
                    IsSuccess = false
                };
            }
        }
        [HttpPost("login")]
        public async Task<APIResponeModel> Login([FromBody] UserLoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                         .Select(e => e.ErrorMessage)
                                         .ToList();
                    return new APIResponeModel()
                    {
                        Code = 400,
                        Data = errors,
                        IsSuccess = false,
                        Message = string.Join(";", errors)
                    };
                }
                var result = await _userService.Login(model);
                return result;
            }
            catch (Exception ex)
            {
                return new APIResponeModel()
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    Data = ex,
                    IsSuccess = false
                };
            }
        }

        [HttpPut("profile/{userId}")]
        public async Task<APIResponeModel> UpdateProfile([FromRoute] string userId, [FromBody] UserProfileModel model)
        {
            try
            {
                var result = await _userService.UpdateProfile(userId, model);
                return result;
            }
            catch (Exception ex)
            {
                return new APIResponeModel()
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = ex,
                    IsSuccess = false
                };
            }
        }
        [Authorize(Roles = "User")]
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetUserProfile([FromRoute] string userId)
        {
            try
            {
                var user = await _userService.GetUserProfile(userId);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
