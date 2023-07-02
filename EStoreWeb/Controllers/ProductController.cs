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
            if (tokenModel.ExpiredTime < DateTime.UtcNow.Ticks && tokenModel.RoleName.Equals("User"))
            {
                return RedirectToAction("ProductUser");
            }
            return View();
        }
        
        public IActionResult ProductUser()
        {
            return View();
        }
    }
}
