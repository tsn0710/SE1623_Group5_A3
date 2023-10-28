using Microsoft.AspNetCore.Mvc;

namespace Estore2Client.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Detail(int id)
        {
            ViewData["orderId"] = id;
            return View();
        }
        public IActionResult Delete(int id)
        {
            ViewData["orderId"] = id;
            return View();
        }
    }
}
