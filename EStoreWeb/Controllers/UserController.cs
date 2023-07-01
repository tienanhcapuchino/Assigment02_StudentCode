using BussinessObject.Models;
using EStoreWeb.Routes;
using EStoreWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EStoreWeb.Controllers
{
    public class UserController : Controller
    {
        [BindProperty]
        public UserProfileModel Profile { get; set; }
        private readonly ICommonService _commonService;
        public UserController(ICommonService commonService)
        {
            _commonService = commonService;
        }
        public async Task<IActionResult> Index(string id)
        {
            var token = _commonService.GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("../Home/Index");
            }
            var respone = DataAccess.Services.CommonService.GetDataAPI($"{RoutesManager.UserProfile}/{id}", MethodAPI.GET, token);
            if (respone.IsSuccessStatusCode)
            {
                var dataResult = await respone.Content.ReadAsStringAsync();
                var profile = JsonConvert.DeserializeObject<UserProfileModel>(dataResult);
                return View(profile);
            }
            if (respone.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }
            return BadRequest();
        }

        public async Task<IActionResult> Update()
        {
            var token = _commonService.GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return Redirect("../Home/Index");
            }
            string jsonData = JsonConvert.SerializeObject(Profile);
            var respone = DataAccess.Services.CommonService.GetDataAPI($"{RoutesManager.UserProfile}/{Profile.Id}", MethodAPI.PUT, token, jsonData);
            if (respone.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }
            if (respone.IsSuccessStatusCode)
            {
                var dataResult = await respone.Content.ReadAsStringAsync();
                APIResponeModel result = JsonConvert.DeserializeObject<APIResponeModel>(dataResult);
                if (result.IsSuccess)
                {
                    ViewBag.Success = "Update successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = result.Message; 
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
