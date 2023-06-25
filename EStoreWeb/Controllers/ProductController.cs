using DataAccess.Entities;
using DataAccess.Models;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EStoreWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        //[BindProperty]
        //public ProductAddModel AddModel { get; set; }
        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                string url = "http://localhost:5063/api/Product/getall";
                HttpResponseMessage respone = CommonService.GetDataAPI(url, MethodAPI.GET);
                List<ProductVM> products = new List<ProductVM>();
                if (respone.IsSuccessStatusCode)
                {
                    string data = respone.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    products = JsonConvert.DeserializeObject<List<ProductVM>>(data);
                }
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError("error when get all product!", ex);
                throw;
            }
        }
        public IActionResult Delete(int id)
        {
            try
            {
                string url = $"http://localhost:5063/api/Product/{id}";
                HttpResponseMessage respone = CommonService.GetDataAPI(url, MethodAPI.DELETE);
                if (respone.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        public IActionResult Add()
        {
            try
            {
                string url = "http://localhost:5063/api/Category/getall";
                HttpResponseMessage respone = CommonService.GetDataAPI(url, MethodAPI.GET);
                if (respone.IsSuccessStatusCode)
                {
                    var datas = respone.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    List <Category> categories = JsonConvert.DeserializeObject<List<Category>>(datas);
                    return View("/Views/Product/Add.cshtml", categories);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        public IActionResult OnAdd(ProductAddModel productModel)
        {
            string url = "http://localhost:5063/api/Product/add";
            string jsonData = JsonConvert.SerializeObject(productModel);
            HttpResponseMessage respone = CommonService.GetDataAPI(url, MethodAPI.POST, jsonData);
            if (respone.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Cannot add!");
        }
    }
}
