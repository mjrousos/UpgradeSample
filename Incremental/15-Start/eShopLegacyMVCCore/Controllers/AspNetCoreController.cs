using Microsoft.AspNetCore.Mvc;

namespace eShopLegacyMVCCore.Controllers
{
    public class AspNetCoreController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Hello world, from ASP.NET Core!");
        }
    }
}
