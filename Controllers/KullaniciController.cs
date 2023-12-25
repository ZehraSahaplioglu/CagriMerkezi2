using CagriMerkezi2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CagriMerkezi2.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly ICalisanRepository _calisanRepository;

        public KullaniciController(IKullaniciRepository kullaniciRepository ,ICalisanRepository calisanRepository)
        {
            _kullaniciRepository = kullaniciRepository;
            _calisanRepository = calisanRepository;

            var result = from Kullanici in _kullaniciRepository.GetAll()
                         join calisan in _calisanRepository.GetAll() on Kullanici.CalisanId equals calisan.Id
                         select new
                         {
                             KullaniciAdi = Kullanici.Ad,
                             calisanAdi = calisan.Ad,
                         };

        }

        public IActionResult Index()
        {
            ViewBag.KulCalisanList = _calisanRepository.GetAll()
        .Select(b => new SelectListItem
        {
            Text = b.Ad,
            Value = b.Id.ToString()
        }).ToList();

            List<Kullanici> objKullaniciList = _kullaniciRepository.GetAll(includeProps: "Calisan").ToList();
            return View(objKullaniciList);
        }

        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> CalisanList = _calisanRepository.GetAll().Select(i => new SelectListItem
            {
                Text = i.Ad,
                Value = i.Id.ToString()
            });
            ViewBag.CalisanList = CalisanList;

            if (id == null || id == 0)
            {
                return View();
            }
            else
            {
                Kullanici? kullaniciVt = _kullaniciRepository.Get(u => u.Id == id);
                if (kullaniciVt == null)
                {
                    return NotFound();
                }
                return View(kullaniciVt);
            }
        }

        [HttpPost]
        public IActionResult EkleGuncelle(Kullanici kullanici)
        {
            if (!ModelState.IsValid)
            {
                if (kullanici.Id == 0)
                {
                    _kullaniciRepository.Ekle(kullanici);
                }
                else
                {
                    _kullaniciRepository.Guncelle(kullanici);
                }

                _kullaniciRepository.Kaydet();
                return RedirectToAction("Index", "Kullanici");
            }

            return View();
        }

        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kullanici? kullaniciVt = _kullaniciRepository.Get(u => u.Id == id);
            if (kullaniciVt == null)
            {
                return NotFound();
            }
            return View(kullaniciVt);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            Kullanici? kullanici = _kullaniciRepository.Get(u => u.Id == id);
            if (kullanici == null)
            {
                return NotFound();
            }
            _kullaniciRepository.Sil(kullanici);
            _kullaniciRepository.Kaydet();
            return RedirectToAction("Index", "Kullanici");
        }

    }
}
