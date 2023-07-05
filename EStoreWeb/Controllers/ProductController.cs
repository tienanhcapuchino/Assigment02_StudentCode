using BussinessObject.Models;
using DataAccess.Services;
using EStoreWeb.Routes;
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
        
        public async Task<IActionResult> ProductUser()
        {
            var token = _commonService.GetToken();
            HttpResponseMessage respone = DataAccess.Services.CommonService.GetDataAPI(RoutesManager.GetAllProduct, MethodAPI.GET, token);
            if (respone.IsSuccessStatusCode)
            {
                if (TempData["SuccessAddCart"] != null)
                {
                    ViewBag.AddSuccess = TempData["SuccessAddCart"] as string;
                }
                var dataResult = await respone.Content.ReadAsStringAsync();
                var resultList = JsonConvert.DeserializeObject<List<ProductVM>>(dataResult);
                return View(resultList);
            }
            return BadRequest();
        }
    }
}
