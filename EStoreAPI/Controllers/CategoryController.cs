using BussinessObject.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("getall")]
        public async Task<List<Category>> Get()
        {
            try
            {
                var result = await _categoryService.GetAll();
                return result;
            }
            catch (Exception ex)
            {
                return new List<Category>();
            }
        }
    }
}
