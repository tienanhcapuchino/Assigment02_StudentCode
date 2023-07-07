using BussinessObject.Entities;
using BussinessObject.Models;
using EStoreWeb.Routes;
using EStoreWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EStoreWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICommonService _commonService;
        public string? TokenData { get; set; }
        public OrderController(ICommonService commonService)
        {
            _commonService = commonService;
            TokenData = _commonService.GetToken();
        }
        public IActionResult Index()
        {
            var tokenModel = _commonService.GetTokenData().GetAwaiter().GetResult();
            if (tokenModel == null)
            {
                return Redirect("../Home/Index");
            }
            if (!tokenModel.RoleName.Equals("Admin"))
            {
                return Forbid();
            }
            return View();
        }
        public IActionResult CheckOut()
        {
            if (string.IsNullOrEmpty(TokenData))
            {
                return Redirect("../../Home/Index");
            }
            var cookies = Request.Cookies["cart"];
            List<CartModel> cartModels = new List<CartModel>();
            if (!string.IsNullOrEmpty(cookies))
            {
                cartModels.AddRange(JsonConvert.DeserializeObject<List<CartModel>>(cookies));
            }
            double total = 0;
            if (cartModels.Any())
            {
                foreach (var item in cartModels)
                {
                    total += (item.Quantity * item.UnitPrice);
                    ViewBag.Total = total;
                }
            }
            return View(cartModels);
        }

        public async Task<IActionResult> OrderHistory()
        {
            if (string.IsNullOrEmpty(TokenData))
            {
                return Redirect("../../Home/Index");
            }
            var currentUser = await _commonService.GetTokenData();
            var userId = currentUser.UserId;
            HttpResponseMessage respone = DataAccess.Services.CommonService.GetDataAPI($"{RoutesManager.OrderHistory}/{userId}", MethodAPI.GET, TokenData);
            if (respone.IsSuccessStatusCode)
            {
                var dataResult = await respone.Content.ReadAsStringAsync();
                List<Order> or = JsonConvert.DeserializeObject<List<Order>>(dataResult);
                return View(or);
            }
            return BadRequest();
        }
        public async Task<IActionResult> MakeOrder()
        {
            if (string.IsNullOrEmpty(TokenData))
            {
                return Redirect("../../Home/Index");
            }
            var cookies = Request.Cookies["cart"];
            List<CartModel> cartModels = new List<CartModel>();
            if (!string.IsNullOrEmpty(cookies))
            {
                cartModels.AddRange(JsonConvert.DeserializeObject<List<CartModel>>(cookies));
            }
            OrderAddModel model = new OrderAddModel();
            model.Freight = 0;
            model.OrderDate = DateTime.Now;
            model.ShipDate = DateTime.Now.AddDays(1);
            model.RequiredDate = DateTime.Now.AddDays(4);
            model.OrderDetails = new List<OrderDetailVM>();
            foreach (var item in cartModels)
            {
                OrderDetailVM orderDetail = new OrderDetailVM()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };
                model.OrderDetails.Add(orderDetail);
            }
            string jsonData = JsonConvert.SerializeObject(model);
            var currentUser = await _commonService.GetTokenData();
            var userId = currentUser.UserId;
            HttpResponseMessage respone = DataAccess.Services.CommonService.GetDataAPI($"{RoutesManager.AddOrder}/{userId}", MethodAPI.POST, TokenData, jsonData);
            if (respone.IsSuccessStatusCode)
            {
                ViewBag.CheckSuccess = "Check out successfully!";
                return Redirect("../../Cart/Index");
            }
            else
            {
                return RedirectToAction("CheckOut");
            }
        }
    }
}
