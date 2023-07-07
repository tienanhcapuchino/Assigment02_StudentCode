using EStoreWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace EStoreWeb.Controllers
{
    public class SaleReportController : Controller
    {
        private readonly ICommonService _commonService;
        public string TokenData { get; set; }
        public SaleReportController(ICommonService commonService)
        {
            _commonService = commonService;
            TokenData = _commonService.GetToken();
        }
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(TokenData))
            {
				return Redirect("../../Home/Index");
			}
            return View();
        }
    }
}
