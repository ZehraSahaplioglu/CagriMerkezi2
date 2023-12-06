using CagriMerkezi2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CagriMerkezi2.Controllers
{
    public class SikayetController : Controller
    {

        private readonly ISikayetRepository _sikayetRepository;
        private readonly IBirimRepository _birimRepository;
        private readonly IDepartmanRepository _departmanRepository;
        private readonly ISikayetDurumRepository _sikayetDurumRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;


        public SikayetController (ISikayetRepository sikayetRepository, IBirimRepository birimRepository, 
            IWebHostEnvironment webHostEnvironment, IDepartmanRepository departmanRepository, ISikayetDurumRepository sikayetDurumRepository)
        {
            _sikayetRepository = sikayetRepository;
            _birimRepository = birimRepository;
            _webHostEnvironment = webHostEnvironment;
            _departmanRepository = departmanRepository;
            _sikayetDurumRepository = sikayetDurumRepository;

            var result = from sikayet in _sikayetRepository.GetAll()
                         join birim in _birimRepository.GetAll() on sikayet.BirimId equals birim.Id
                         join departman in _departmanRepository.GetAll() on sikayet.DepId equals departman.Id
                         join sikayetDurum in _sikayetDurumRepository.GetAll() on sikayet.DurumId equals sikayetDurum.Id
                         select new
                         {
                             sikayetAdi = sikayet.Ad,
                             departmanAdi = departman.Ad,
                             birimAdi = birim.Ad,
                             durumAdi = sikayetDurum.Ad,
                         };

        }

        public IActionResult Index()
        {
            ViewBag.BrSikayetList = _birimRepository.GetAll()
        .Select(b => new SelectListItem
        {
            Text = b.Ad,
            Value = b.Id.ToString()
        }).ToList();

            ViewBag.DrmSikayetList = _sikayetDurumRepository.GetAll()
        .Select(b => new SelectListItem
        {
            Text = b.Ad,
            Value = b.Id.ToString()
        }).ToList();

            List<Sikayet> objSikayetList = _sikayetRepository.GetAll(includeProps: "Departman").ToList();
            return View(objSikayetList);
        }

        public IActionResult BasvuruSorgula(string kod) 
        {
            if (ModelState.IsValid) {

                Sikayet sikayet = _sikayetRepository.GetByBasvuruKodu(kod);

                // Şikayet bulunamadıysa hata sayfasına yönlendir
                if (sikayet == null)
                {
                    return View("Error");
                }

                // Şikayetin durumunu al
                SikayetDurum sikayetDurum = sikayet.SikayetDurum;

                // Durumu görüntüleyen sayfayı döndür
                return View(sikayetDurum);
            }
            return View();
        }


        // arama motorunda birime göre filtreleme işlemi yapar
        [HttpGet]
        public IActionResult GetFilteredSikayetler(int birimId)
        {
            List<Sikayet> filteredSikayetList;

            if (birimId > 0)
            {
                filteredSikayetList = _sikayetRepository.GetFilteredSikayetler(birimId);
            }
            else
            {
                filteredSikayetList = _sikayetRepository.GetAll(includeProps: "Departman").ToList();
            }

            return PartialView("_SikayetListPartial", filteredSikayetList);
        }

        // arama motorunda duruma göre filtreleme işlemi yapar
        [HttpGet]
        public IActionResult GetFilteredDurum(int durumId)
        {
            List<Sikayet> filteredSikayetDrmList;

            if (durumId > 0)
            {
                filteredSikayetDrmList = _sikayetRepository.GetFilteredDurum(durumId);
            }
            else
            {
                filteredSikayetDrmList = _sikayetRepository.GetAll(includeProps: "Departman").ToList();
            }

            return PartialView("_SikayetListPartial", filteredSikayetDrmList);
        }

        public IActionResult EkleGuncelle(int? id, int? selectedBirimId)
        {

            IEnumerable<SelectListItem> DurumSikayetList = _sikayetDurumRepository.GetAll().Select(b => new SelectListItem
            {
                Text = b.Ad,
                Value = b.Id.ToString()
            });
            ViewBag.DurumSikayetList = DurumSikayetList;

            IEnumerable<SelectListItem> BirimSikayetList = _birimRepository.GetAll().Select(b => new SelectListItem
            {
                Text = b.Ad,
                Value = b.Id.ToString()
            });
            ViewBag.BirimSikayetList = BirimSikayetList;

            // Seçilen birim ID'sini kullanarak sadece o birime ait departmanları al
            if (selectedBirimId.HasValue)
            {
                ViewBag.DepSikayetList = _departmanRepository
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
                IEnumerable<SelectListItem> DepSikayetList = _departmanRepository.GetAll().Select(d => new SelectListItem
                {
                    Text = d.Ad,
                    Value = d.Id.ToString()
                });
                ViewBag.DepSikayetList = DepSikayetList;
            }

            if (id == null || id == 0)
            {
                return View();
            }
            else
            {
                Sikayet? sikayetVt = _sikayetRepository.Get(u => u.Id == id);
                if (sikayetVt == null)
                {
                    return NotFound();
                }

                return View(sikayetVt);
            }
        }

        // seçilen birime göre departmanı getirir
        [HttpGet]
        public IActionResult GetDepartmentsByBirimId(int birimId)
        {
            var depSikayetList = _departmanRepository.GetDepartmentsByBirimId(birimId)
                .Select(d => new SelectListItem
                {
                    Text = d.Ad,
                    Value = d.Id.ToString()
                });

            return Json(depSikayetList);
        }

        [HttpPost]
        public IActionResult EkleGuncelle(Sikayet sikayet, IFormFile? file)
        {
            if(!ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string sikayetPath = Path.Combine(wwwRootPath, @"img");

                if(file != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(sikayetPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    sikayet.ResimUrl = @"\img\" + file.FileName;
                }

                if (sikayet.Id == 0)
                {
                    sikayet.BasvuruKodu = GenerateUniqueCode();
                    _sikayetRepository.Ekle(sikayet);
                }
                else
                {
                    _sikayetRepository.Guncelle(sikayet);
                }
                _sikayetRepository.Kaydet();
                return RedirectToAction("Index", "Sikayet");

            }
            return View();
        }

        // başvuru sorgulada kullanılacak olan uiq kod oluşturulması
        private string GenerateUniqueCode()
        {
            Guid uniqueGuid = Guid.NewGuid();
            string uniqueCode = uniqueGuid.ToString("N").Substring(0, 10);
            return uniqueCode;
        }

        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Sikayet? sikayetVt = _sikayetRepository.Get(u => u.Id == id);
            if(sikayetVt == null)
            {
                return NotFound();
            }
            return View(sikayetVt);
        }

        [HttpPost, ActionName("Sil")]

        public IActionResult SilPOST(int? id)
        {
            Sikayet? sikayet = _sikayetRepository.Get(u =>u.Id == id);
            if(sikayet == null)
            {
                return NotFound();
            }
            _sikayetRepository.Sil(sikayet);
            _sikayetRepository.Kaydet();
            return RedirectToAction("Index", "Sikayet");
        }

    }
}
