using BussinessObject.Models;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EStoreWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        [BindProperty]
        public ProductAddModel AddModel { get; set; }
        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Delete(int id)
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }
        public IActionResult OnAdd(ProductAddModel productModel)
        {
            productModel = AddModel;
            return View();
        }
    }
}
