using BussinessObject.Entities;
using BussinessObject.Models;
using DataAccess.DataContext;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly EStoreDbContext _context;
        private readonly SignInManager<User> _signIn;
        public UserController(IUserService userService,
            EStoreDbContext eStoreDbContext,
            SignInManager<User> signInManager)
        {
            _userService = userService;
            _context = eStoreDbContext;
            _signIn = signInManager;
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
                var userEntity = await _userService.GetUserByEmail(model.Email);
                if (userEntity != null)
                {
                    var checkLockUser = await _signIn.CheckPasswordSignInAsync(userEntity, model.Password, lockoutOnFailure: true);
                    if (checkLockUser.IsLockedOut)
                    {
                        return new APIResponeModel()
                        {
                            Code = 400,
                            Message = "Your account is locked. Please try again after 30 minutes",
                            IsSuccess = false
                        };
                    }
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
        [Authorize(Roles = "Admin")]
        [HttpGet("salereport/{fromDate:datetime}/{toDate:datetime}")]

        public IActionResult GetSaleReport([FromRoute] DateTime fromDate, [FromRoute] DateTime toDate)
        {
            try
            {
                var result = _context.OrderDetails
               .Join(_context.Orders,
                   od => od.OrderId,
                   o => o.OrderId,
                   (od, o) => new { OrderDetail = od, Order = o })
                .Join(
                   _context.Products,
                   od => od.OrderDetail.ProductId,
                   p => p.ProductId,
                   (od, p) => new { od.OrderDetail, od.Order, Product = p }
               )
               .Where(x => x.Order.OrderDate >= fromDate && x.Order.OrderDate <= toDate)
               .GroupBy(
                   x => new
                   {
                       OrderDate = x.Order.OrderDate,
                       ProductName = x.Product.Name,
                       UnitPrice = x.OrderDetail.UnitPrice
                   })
               .Select(g => new
               {
                   OrderDate = g.Key.OrderDate,
                   ProductName = g.Key.ProductName,
                   UnitPrice = g.Key.UnitPrice,
                   Quantity = g.Sum(x => x.OrderDetail.Quantity),
                   Sales = g.Sum(x => x.OrderDetail.Quantity) * g.Key.UnitPrice
               })
               .OrderByDescending(x => x.OrderDate)
               .ThenByDescending(x => x.Sales)
               .ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
