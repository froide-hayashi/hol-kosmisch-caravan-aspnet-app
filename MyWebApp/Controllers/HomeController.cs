using MyWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace MyWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDistributedCache cache;

        public HomeController(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public ActionResult Index()
        {
            ViewBag.Message = cache.GetString("message");
            return View();
        }

        [HttpPost]
        public ActionResult Index(MyForm item)
        {
            if (!string.IsNullOrWhiteSpace(item?.Message))
            {
                cache.SetString("message", item.Message);
            }
            return RedirectToAction("Index");
        }
    }
}