using CagriMerkezi2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CagriMerkezi2.Controllers
{
    public class SikayetDurumController : Controller
    {

        private readonly ISikayetDurumRepository _sikayetDurumRepository;

        public SikayetDurumController(ISikayetDurumRepository context)
        {
            _sikayetDurumRepository = context;
        }


        public IActionResult Index()
        {
            string yetki = HttpContext.Session.GetString("Yetki");
            ViewBag.Yetki = yetki;

            if (HttpContext.Session.GetString("GirisKontrol") == "ok" || yetki == "admin" || yetki == "user")
            {
                List<SikayetDurum> objDurumList = _sikayetDurumRepository.GetAll().ToList();
                return View(objDurumList);
            }
            else
            {
                return RedirectToAction("Login", "Kullanici");
            }
        }


        public IActionResult Ekle()
        {
            string yetki = HttpContext.Session.GetString("Yetki");
            ViewBag.Yetki = yetki;

            if (HttpContext.Session.GetString("GirisKontrol") == "ok" || yetki == "admin" || yetki == "user")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Kullanici");
            }
        }


        [HttpPost]
        public IActionResult Ekle(SikayetDurum sikayetDurum)
        {
            if (!ModelState.IsValid)
            {
                _sikayetDurumRepository.Ekle(sikayetDurum);
                _sikayetDurumRepository.Kaydet();
                return RedirectToAction("Index", "SikayetDurum");
            }
            return View();
        }


        public IActionResult Guncelle(int? id)
        {
            string yetki = HttpContext.Session.GetString("Yetki");
            ViewBag.Yetki = yetki;

            if (HttpContext.Session.GetString("GirisKontrol") == "ok" || yetki == "admin" || yetki == "user")
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                SikayetDurum? sikayetDurumVt = _sikayetDurumRepository.Get(u => u.Id == id);
                if (sikayetDurumVt == null)
                {
                    return NotFound();
                }
                return View(sikayetDurumVt);
            }
            else
            {
                return RedirectToAction("Login", "Kullanici");
            }
        }


        [HttpPost]
        public IActionResult Guncelle(SikayetDurum sikayetDurum)
        {
            if (!ModelState.IsValid)
            {
                _sikayetDurumRepository.Guncelle(sikayetDurum);
                _sikayetDurumRepository.Kaydet();
                return RedirectToAction("Index", "SikayetDurum");
            }
            return View();
        }


        public IActionResult Sil(int? id)
        {
            string yetki = HttpContext.Session.GetString("Yetki");
            ViewBag.Yetki = yetki;

            if (HttpContext.Session.GetString("GirisKontrol") == "ok" || yetki == "admin" || yetki == "user")
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                SikayetDurum? sikayetDurumVt = _sikayetDurumRepository.Get(u => u.Id == id);
                if (sikayetDurumVt == null)
                {
                    return NotFound();
                }
                return View(sikayetDurumVt);
            }
            else
            {
                return RedirectToAction("Login", "Kullanici");
            }
        }


        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            SikayetDurum? sikayetDurum = _sikayetDurumRepository.Get(u => u.Id == id);
            if (sikayetDurum == null)
            {
                return NotFound();
            }
            _sikayetDurumRepository.Sil(sikayetDurum);
            _sikayetDurumRepository.Kaydet();
            return RedirectToAction("Index", "SikayetDurum");
        }

    }
}
