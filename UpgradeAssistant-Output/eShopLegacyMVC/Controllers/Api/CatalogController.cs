using System.Web.Mvc;

namespace eShopLegacyMVC.Controllers.Api
{
    [Route("api")]
    public class CatalogController2 : Microsoft.AspNetCore.Mvc.Controller
    {
        [HttpGet]
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return Json(new { Message = "Hello World!" });
        }
    }
}
