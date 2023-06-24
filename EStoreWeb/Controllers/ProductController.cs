using DataAccess.Models;
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
                HttpClient client = new HttpClient();
                HttpResponseMessage respone = client.GetAsync(url).GetAwaiter().GetResult();
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
    }
}
