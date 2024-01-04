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
            return View();
        }
    }
}
