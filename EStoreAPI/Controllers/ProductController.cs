using DataAccess.Interfaces;
using BussinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [Authorize]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var result = await _productService.GetAllProducts();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                return Ok(await _productService.DeleteProduct(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] ProductAddModel model)
        {
            try
            {
                return Ok(await _productService.AddProduct(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update/{productId}")]
        public async Task<IActionResult> Update([FromRoute] int productId, [FromBody] ProductUpdateModel model)
        {
            try
            {
                bool result = await _productService.UpdateProduct(productId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("searchByName")]
        public async Task<IActionResult> SearchByName([FromQuery] string? name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    var products = await _productService.GetAllProducts();
                    return Ok(products);
                }
                var result = await _productService.SearchByName(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("searchByPrice")]
        public async Task<IActionResult> SearchByPrice([FromQuery] double? priceFrom, [FromQuery] double? priceTo)
        {
            try
            {
                var result = await _productService.SearchByPrice(priceFrom, priceTo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
