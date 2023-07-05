using BussinessObject.Models;
using EStoreWeb.Routes;
using EStoreWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EStoreWeb.Controllers
{
    public class CartController : Controller
    {
        private ICommonService _commonService;
        private IHttpContextAccessor _contextAccessor;
        private string TokenData { get; set; }
        public CartController(ICommonService commonService,
            IHttpContextAccessor contextAccessor)
        {
            _commonService = commonService;
            _contextAccessor = contextAccessor;
            TokenData = _commonService.GetToken();
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(TokenData))
            {
                return Redirect("../../Home/Index");
            }
            Request.Cookies.TryGetValue("cart", out string cartValue);
            if (TempData["SuccessAddCart"] != null)
            {
                ViewBag.DeleteSuccess = TempData["SuccessAddCart"] as string;
            }
                if (!string.IsNullOrEmpty(cartValue))
            {
                List<CartModel> carts = JsonConvert.DeserializeObject<List<CartModel>>(cartValue);
                return View(carts);
            }
            return View();
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            HttpResponseMessage respone = new HttpResponseMessage();
            var cookies = Request.Cookies["cart"];
            if (!string.IsNullOrEmpty(cookies))
            {
                respone = DataAccess.Services.CommonService.GetDataAPI($"{RoutesManager.AddToCart}/{id}", MethodAPI.POST, TokenData, cookies);
            }
            else
            {
                respone = DataAccess.Services.CommonService.GetDataAPI($"{RoutesManager.AddToCart}/{id}", MethodAPI.POST, TokenData, JsonConvert.SerializeObject(new List<CartModel>()));
            }
            if (respone.IsSuccessStatusCode)
            {
                var datas = await respone.Content.ReadAsStringAsync();
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                };
                Response.Cookies.Append("cart", datas, cookieOptions);
                TempData["SuccessAddCart"] = "Add product to cart success";
                return Redirect("../../Product/ProductUser");
            }
            return BadRequest();
        }

        public async Task<IActionResult> DeleteCart(int productId)
        {
            var cookies = Request.Cookies["cart"];
            HttpResponseMessage respone = DataAccess.Services.CommonService.GetDataAPI($"{RoutesManager.DeleteCart}/{productId}", MethodAPI.POST, TokenData, cookies);
            if (respone.IsSuccessStatusCode)
            {
                var datas = await respone.Content.ReadAsStringAsync();
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                };
                Response.Cookies.Append("cart", datas, cookieOptions);
                TempData["SuccessDeleteCart"] = "Delete product to cart success";
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAllCart()
        {
            Response.Cookies.Delete("cart");
            return RedirectToAction("Index");
        }

    }
}
