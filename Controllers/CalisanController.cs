using CagriMerkezi2.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace CagriMerkezi2.Controllers
{
    public class CalisanController : Controller
    {
        private readonly ICalisanRepository _calisanRepository;
        private readonly IBirimRepository _birimRepository;
        private readonly IDepartmanRepository _departmanRepository;

        public CalisanController(ICalisanRepository calisanRepository,IBirimRepository birimRepository, IDepartmanRepository departmanRepository)
        {
            _calisanRepository = calisanRepository;
            _birimRepository = birimRepository;
            _departmanRepository = departmanRepository;

            var result = from calisan in _calisanRepository.GetAll()
                         join birim in _birimRepository.GetAll() on calisan.BirimId equals birim.Id
                         join departman in _departmanRepository.GetAll() on calisan.DepId equals departman.Id
                         select new
                         {
                             calisanAdi = calisan.Ad,
                             departmanAdi = departman.Ad,
                             birimAdi = birim.Ad,
                         };

        }


        public IActionResult Index()
        {
            ViewBag.BrCalisanList = _birimRepository.GetAll()
        .Select(b => new SelectListItem
        {
            Text = b.Ad,
            Value = b.Id.ToString()
        }).ToList();

            List<Calisan> objCalisanList = _calisanRepository.GetAll(includeProps: "Departman").ToList();
            return View(objCalisanList);
        }


        public IActionResult YetkiVer(int id)
        {
            return RedirectToAction("YetkiVer", "Kullanici", new { calisanid = id });
        }


        [HttpGet]
        public IActionResult GetFilteredCalisanlar(int birimId)
        {
            List<Calisan> filteredCalisanList;

            if (birimId > 0)
            {
                filteredCalisanList = _calisanRepository.GetFilteredCalisanlar(birimId);
            }
            else
            {
                filteredCalisanList = _calisanRepository.GetAll(includeProps: "Departman").ToList();
            }

            return PartialView("_CalisanListPartial", filteredCalisanList);
        }

        public IActionResult EkleGuncelle(int? id, int? selectedBirimId)
        {
            IEnumerable<SelectListItem> BirimCalisanList = _birimRepository.GetAll().Select(b => new SelectListItem
            {
                Text = b.Ad,
                Value = b.Id.ToString()
            });
            ViewBag.BirimCalisanList = BirimCalisanList;

            // Seçilen birim ID'sini kullanarak sadece o birime ait departmanları al
            if (selectedBirimId.HasValue)
            {
                ViewBag.DepCalisanList = _departmanRepository
                    .GetDepartmentsByBirimId(selectedBirimId.Value)
                    .Select(d => new SelectListItem
                    {
                        Text = d.Ad,
                        Value = d.Id.ToString()
                    });
            }
            else
            {
                // Eğer bir birim seçilmemişse, tüm departmanları getir
                IEnumerable<SelectListItem> DepCalisanList = _departmanRepository.GetAll().Select(d => new SelectListItem
                {
                    Text = d.Ad,
                    Value = d.Id.ToString()
                });
                ViewBag.DepCalisanList = DepCalisanList;
            }

            if (id == null || id == 0)
            {
                return View();
            }
            else
            {
                Calisan? calisanVt = _calisanRepository.Get(u => u.Id == id);
                if (calisanVt == null)
                {
                    return NotFound();
                }

                return View(calisanVt);
            }
        }

        [HttpGet]
        public IActionResult GetDepartmentsByBirimId(int birimId)
        {
            var depCalisanList = _departmanRepository.GetDepartmentsByBirimId(birimId)
                .Select(d => new SelectListItem
                {
                    Text = d.Ad,
                    Value = d.Id.ToString()
                });

            return Json(depCalisanList);
        }



        [HttpPost]
        public IActionResult EkleGuncelle(Calisan calisan)
        {
            if (!ModelState.IsValid)
            {

                if (calisan.Id == 0)
                {
                    _calisanRepository.Ekle(calisan);
                }
                else
                {
                    _calisanRepository.Guncelle(calisan);
                }
                _calisanRepository.Kaydet();
                return RedirectToAction("Index", "Calisan");

            }
            return View();
        }


        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Calisan? calisanVt = _calisanRepository.Get(u => u.Id == id);
            if (calisanVt == null)
            {
                return NotFound();
            }
            return View(calisanVt);
        }

        [HttpPost, ActionName("Sil")]

        public IActionResult SilPOST(int? id)
        {
            Calisan? calisan = _calisanRepository.Get(u => u.Id == id);
            if (calisan == null)
            {
                return NotFound();
            }
            _calisanRepository.Sil(calisan);
            _calisanRepository.Kaydet();
            return RedirectToAction("Index", "Calisan");
        }


    }
}
