using BussinessObject.Models;
using DataAccess.Services;
using EStoreWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EStoreWeb.Controllers
{
    public class ProductController : Controller
    {
        [BindProperty]
        public ProductAddModel AddModel { get; set; }
        private readonly ILogger<ProductController> _logger;
        private readonly ICommonService _commonService;
        public ProductController(ILogger<ProductController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }
        public IActionResult Index()
        {
            var tokenModel = _commonService.GetTokenData().GetAwaiter().GetResult();
            if (tokenModel == null)
            {
                return Redirect("../Home/Index");
            }
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
        public IActionResult ProductUser()
        {
            return View();
        }
    }
}
