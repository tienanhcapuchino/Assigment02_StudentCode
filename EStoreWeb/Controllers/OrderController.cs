using EStoreWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace EStoreWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICommonService _commonService;
        public OrderController(ICommonService commonService)
        {
            _commonService = commonService;
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
    }
}
