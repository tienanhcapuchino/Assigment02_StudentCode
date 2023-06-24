using DataAccess.Models;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EStoreWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
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
            string url = $"http://localhost:5063/api/Product/{id}";
            HttpResponseMessage respone = CommonService.GetDataAPI(url, MethodAPI.DELETE);
            if (respone.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        public IActionResult Add (ProductAddModel model)
        {
            string url = "http://localhost:5063/api/Product/add";
            string jsonData = JsonConvert.SerializeObject(model);
            HttpResponseMessage respone = CommonService.GetDataAPI(url, MethodAPI.POST, jsonData);
            if (respone.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Cannot add!");
        }
    }
}
