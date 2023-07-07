using BussinessObject.Models;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        public OrderController(IOrderService orderService,
            IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var order = await _orderService.GetAllOrders();
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("orderhistory/{userId}")]
        public async Task<IActionResult> GetOrderHistory([FromRoute] string userId)
        {
            try
            {
                var result = await _orderService.GetOrdersHistoryByUser(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Authorize(Roles = "User")]                
        [HttpPost("add/{userId}")]
        public async Task<IActionResult> AddOrder([FromRoute] string userId, [FromBody] OrderAddModel model)
        {
            try
            {
                var result = await _orderService.AddOrder(model, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
