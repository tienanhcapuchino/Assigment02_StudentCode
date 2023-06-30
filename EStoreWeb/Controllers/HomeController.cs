using BussinessObject.Models;
using DataAccess.Services;
using EStoreWeb.Models;
using EStoreWeb.Routes;
using EStoreWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EStoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICommonService _commonService;

        public HomeController(ILogger<HomeController> logger,
            ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }
        [BindProperty]
        public UserLoginModel LoginModel { get; set; }
        [BindProperty]
        public UserRegisterModel RegisterModel { get; set; }
        public async Task<IActionResult> Index()
        {
            var tokenModel = await _commonService.GetTokenData();
            if (tokenModel != null)
            {
                if (tokenModel.ExpiredTime < DateTime.Now.Ticks)
                {
                    return Redirect("../Product/Index");
                }
            }
            if (TempData["ErrorMessage"] != null)
            {
                string errorMessage = TempData["ErrorMessage"] as string;
                ViewBag.ErrorMessage = errorMessage;
            }
            if (TempData["RegisterFail"] != null)
            {
                string registerFailed = TempData["RegisterFail"] as string;
                ViewBag.RegisterFailed = registerFailed;
            }
            if (TempData["SuccessRegister"] != null)
            {
                string successRegister = TempData["SuccessRegister"] as string;
                ViewBag.SuccessRegister = successRegister;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View("/Views/Home/Register.cshtml");
        }

        public async Task<IActionResult> Login(UserLoginModel model)
        {
            try
            {
                model = LoginModel;
                string jsonData = JsonConvert.SerializeObject(model);
                HttpResponseMessage respone = DataAccess.Services.CommonService.GetDataAPI(RoutesManager.Login, MethodAPI.POST, jsonData);
                if (respone.IsSuccessStatusCode)
                {
                    var dataResult = await respone.Content.ReadAsStringAsync();
                    APIResponeModel result = JsonConvert.DeserializeObject<APIResponeModel>(dataResult);
                    if (result.Code == 200)
                    {
                        Response.Cookies.Append("token", result.Data.ToString());
                        if (model.Email.Equals("admin@estore.com"))
                            return Redirect("../Product/Index");
                        return Redirect("../Product/ProductUser");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = result.Message;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    var dataError = await respone.Content.ReadAsStringAsync();
                    APIResponeModel resultError = JsonConvert.DeserializeObject<APIResponeModel>(dataError);
                    TempData["ErrorMessage"] = resultError.Message;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> DoRegister(UserRegisterModel model)
        {
            model = RegisterModel;
            string jsonData = JsonConvert.SerializeObject(model);

            HttpResponseMessage respone = DataAccess.Services.CommonService.GetDataAPI(RoutesManager.Register, MethodAPI.POST, jsonData);
            if (respone.IsSuccessStatusCode)
            {
                var dataResult = await respone.Content.ReadAsStringAsync();
                APIResponeModel result = JsonConvert.DeserializeObject<APIResponeModel>(dataResult);
                if (result.Code == 200)
                {
                    TempData["SuccessRegister"] = "Register success! Now you can login!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["RegisterFail"] = result.Message;
                    return RedirectToAction("Register");
                }
            }
            else
            {
                var dataError = await respone.Content.ReadAsStringAsync();
                APIResponeModel resultError = JsonConvert.DeserializeObject<APIResponeModel>(dataError);
                TempData["RegisterFail"] = resultError.Message;
                return RedirectToAction("Register");
            }
        }

        public IActionResult Logout()
        {
            _commonService.Logout();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}