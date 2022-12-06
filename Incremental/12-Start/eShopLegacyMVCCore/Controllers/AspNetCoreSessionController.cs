using eShopLegacy.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShopLegacyMVCCore.Controllers
{
    public class AspNetCoreSessionController : Controller
    {
        // GET: AspNetCoreSession
        public ActionResult Index()
        {
            var model = ((System.Web.HttpContext)HttpContext).Session["DemoItem"];
            return View(model);
        }

        // POST: AspNetCoreSession
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SessionDemoModel demoModel)
        {
            ((System.Web.HttpContext)HttpContext).Session["DemoItem"] = demoModel;
            return View(demoModel);
        }
    }
}
