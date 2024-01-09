using Microsoft.AspNetCore.Mvc;

namespace CagriMerkezi2.Controllers
{
    public class GirisController : Controller
    {

        private readonly ILogger<GirisController> _logger;

        public GirisController(ILogger<GirisController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            string yetki = HttpContext.Session.GetString("Yetki");
            ViewBag.Yetki = yetki;

            if (HttpContext.Session.GetString("GirisKontrol") == "ok" || yetki == "admin" || yetki == "user")
            {
                
                ViewBag.Yetki = yetki;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Kullanici");
            }
        }
        
    }
}
