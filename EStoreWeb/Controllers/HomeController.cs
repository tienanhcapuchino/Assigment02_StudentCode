using BussinessObject.Models;
using DataAccess.Services;
using EStoreWeb.Models;
using EStoreWeb.Routes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace EStoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [BindProperty]
        public UserLoginModel LoginModel { get; set; }
        [BindProperty]
        public UserRegisterModel RegisterModel { get; set; }
        public async Task<IActionResult> Index()
        {
            if (TempData["ErrorMessage"] != null)
            {
                string errorMessage = TempData["ErrorMessage"] as string;
                ViewBag.ErrorMessage = errorMessage;
            }
            if (TempData["LoginModel"] != null)
            {
                LoginModel = JsonConvert.DeserializeObject<UserLoginModel>(TempData["LoginModel"].ToString());
                return View(LoginModel);
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
        public async Task<IActionResult> DoRegister(UserRegisterModel model)
        {
            model = RegisterModel;
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
                TempData["ErrorMessage"] = string.Join("; ", errors);
                //TempData["LoginModel"] = LoginModel;
                return RedirectToAction("Register");
            }
            string jsonData = JsonConvert.SerializeObject(model);

            HttpResponseMessage respone = CommonService.GetDataAPI(RoutesManager.Register, MethodAPI.POST, jsonData);
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
                    TempData["ErrorMessage"] = result.Message;
                    return RedirectToAction("Register");
                }
            }
            else
            {
                var dataError = await respone.Content.ReadAsStringAsync();
                APIResponeModel resultError = JsonConvert.DeserializeObject<APIResponeModel>(dataError);
                //TempData["LoginModel"] = LoginModel;
                TempData["ErrorMessage"] = resultError.Message;
                return RedirectToAction("Register");
            }
        }
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            try
            {
                model = LoginModel;
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
                    TempData["ErrorMessage"] = string.Join("; ", errors);
                    TempData["LoginModel"] = LoginModel;
                    return RedirectToAction("Index");
                }
                string jsonData = JsonConvert.SerializeObject(model);
                HttpResponseMessage respone = CommonService.GetDataAPI(RoutesManager.Login, MethodAPI.POST, jsonData);
                if (respone.IsSuccessStatusCode)
                {
                    var dataResult = await respone.Content.ReadAsStringAsync();
                    APIResponeModel result = JsonConvert.DeserializeObject<APIResponeModel>(dataResult);
                    if (result.Code == 200)
                    {
                        Response.Cookies.Append("token", result.Data.ToString());
                        return View("/Product/Index");
                    }
                    else
                    {
                        TempData["LoginModel"] = LoginModel;
                        TempData["ErrorMessage"] = result.Message;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    var dataError = await respone.Content.ReadAsStringAsync();
                    APIResponeModel resultError = JsonConvert.DeserializeObject<APIResponeModel>(dataError);
                    TempData["LoginModel"] = LoginModel;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}