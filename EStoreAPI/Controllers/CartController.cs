using BussinessObject.Models;
using DataAccess.DataContext;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly EStoreDbContext _dbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        public CartController(IProductService productService,
            EStoreDbContext eStoreDbContext,
            IHttpContextAccessor contextAccessor)
        {
            _productService = productService;
            _dbContext = eStoreDbContext;
            _contextAccessor = contextAccessor;
        }

        [Authorize(Roles = "User")]
        [HttpPost("add/{productId}")]
        public async Task<IActionResult> AddCart([FromRoute] int productId, [FromBody] List<CartModel> models)
        {
            try
            {
                var product = await _productService.GetById(productId);
                if (product == null)
                {
                    return NotFound();
                }
                if (product.UnitInStock < 1)
                {
                    return Ok(new APIResponeModel() { Code = 400, Message = "Unit stock is not enough!", IsSuccess = false });
                }
                List<CartModel> carts = new List<CartModel>();
                if (models != null && models.Count > 0)
                {
                    carts.AddRange(models);
                }

                var productIds = carts.Select(x => x.ProductId).ToList();
                if (productIds != null && productIds.Any() && productIds.Contains(productId))
                {
                    foreach (var item in carts)
                    {
                        if (item.ProductId == productId)
                        {
                            item.Quantity += 1;
                        }
                    }
                }
                else
                {
                    carts.Add(new CartModel()
                    {
                        ProductId = productId,
                        Quantity = 1,
                        UnitPrice = product.UnitPrice,
                        ProductName = product.Name
                    });
                }
                product.UnitInStock -= 1;
                await _productService.Update(product);
                return Ok(carts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("delete/{productId}")]
        public async Task<IActionResult> DeleteCart([FromRoute] int productId, [FromBody] List<CartModel> carts)
        {
            try
            {
                var product = await _productService.GetById(productId);
                carts = carts.Where(x => x.ProductId != productId).ToList();
                product.UnitInStock -= 1;
                await _dbContext.SaveChangesAsync();
                return Ok(carts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
