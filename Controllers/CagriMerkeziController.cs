﻿using CagriMerkezi2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CagriMerkezi2.Controllers
{
    public class CagriMerkeziController : Controller
    {
        private readonly ICagriMerkeziRepository _cagriMerkeziRepository;
        private readonly ISikayetRepository _sikayetRepository;
        private readonly IBirimRepository _birimRepository;
        private readonly IDepartmanRepository _departmanRepository;
        private readonly ISikayetDurumRepository _sikayetDurumRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;


        public CagriMerkeziController(ICagriMerkeziRepository cagriMerkeziRepository, ISikayetRepository sikayetRepository, 
                    IBirimRepository birimRepository, IWebHostEnvironment webHostEnvironment, IDepartmanRepository departmanRepository,
                    ISikayetDurumRepository sikayetDurumRepository)
        {
            _cagriMerkeziRepository = cagriMerkeziRepository;
            _sikayetRepository = sikayetRepository;
            _birimRepository = birimRepository;
            _webHostEnvironment = webHostEnvironment;
            _departmanRepository = departmanRepository;
            _sikayetDurumRepository = sikayetDurumRepository;

            var result = from cagriMerkezi in _cagriMerkeziRepository.GetAll()
                         join birim in _birimRepository.GetAll() on cagriMerkezi.BirimId equals birim.Id
                         join departman in _departmanRepository.GetAll() on cagriMerkezi.DepId equals departman.Id
                         join sikayetDurum in _sikayetDurumRepository.GetAll() on cagriMerkezi.DurumId equals sikayetDurum.Id
                         select new
                         {
                             cagriMerkeziAdi = cagriMerkezi.Ad,
                             departmanAdi = departman.Ad,
                             birimAdi = birim.Ad,
                             durumAdi = sikayetDurum.Ad,
                         };
            _sikayetDurumRepository = sikayetDurumRepository;
        }


        public IActionResult Index()
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



        public IActionResult GelenSikayet()
        {
            string yetki = HttpContext.Session.GetString("Yetki");
            ViewBag.Yetki = yetki;

            if (HttpContext.Session.GetString("GirisKontrol") == "ok" || yetki == "admin" || yetki == "user")
            {
                List<CagriMerkezi> objCagriList = _cagriMerkeziRepository.GetAll().ToList();
                return View(objCagriList);
            }
            else
            {
                return RedirectToAction("Login", "Kullanici");
            }
        }



        public IActionResult EkleGuncelle(int? id, int? selectedBirimId)
        {
            string yetki = HttpContext.Session.GetString("Yetki");
            ViewBag.Yetki = yetki;

            IEnumerable<SelectListItem> DurumCagriList = _sikayetDurumRepository.GetAll().Select(b => new SelectListItem
            {
                Text = b.Ad,
                Value = b.Id.ToString()
            });
            ViewBag.DurumCagriList = DurumCagriList;

            IEnumerable<SelectListItem> BirimCagriList = _birimRepository.GetAll().Select(b => new SelectListItem
            {
                Text = b.Ad,
                Value = b.Id.ToString()
            });
            ViewBag.BirimCagriList = BirimCagriList;

            // Seçilen birim ID'sini kullanarak sadece o birime ait departmanları al
            if (selectedBirimId.HasValue)
            {
                ViewBag.DepCagriList = _departmanRepository
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
                IEnumerable<SelectListItem> DepCagriList = _departmanRepository.GetAll().Select(d => new SelectListItem
                {
                    Text = d.Ad,
                    Value = d.Id.ToString()
                });
                ViewBag.DepCagriList = DepCagriList;
            }

            if (id == null || id == 0)
            {
                return View();
            }
            else
            {
                CagriMerkezi? cagriVt = _cagriMerkeziRepository.Get(u => u.Id == id);
                if (cagriVt == null)
                {
                    return NotFound();
                }

                return View(cagriVt);
            }

        }


        [HttpGet]
        public IActionResult GetDepartmentsByBirimId(int birimId)
        {
            var depCagriList = _departmanRepository.GetDepartmentsByBirimId(birimId)
                .Select(d => new SelectListItem
                {
                    Text = d.Ad,
                    Value = d.Id.ToString()
                });

            return Json(depCagriList);
        }


        [HttpPost]
        public IActionResult EkleGuncelle(CagriMerkezi? cagriMerkezi, IFormFile? file, int? BirimId, int? DepId, int? DurumId)
        {

            if (!ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string cagriPath = Path.Combine(wwwRootPath, @"img");

                if (file != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(cagriPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    cagriMerkezi.ResimUrl = @"\img\" + file.FileName;
                }

                
                if (cagriMerkezi.Id == 0)
                {
                    cagriMerkezi.BasvuruKodu = GenerateUniqueCode();

                    // Eğer BirimId ve DepId değerleri varsa Sikayet tablosuna ekleyin
                    if (BirimId.HasValue && DepId.HasValue)
                    {
                        Sikayet yeniSikayet = new Sikayet
                        {
                            Ad = cagriMerkezi.Ad,
                            Soyad = cagriMerkezi.Soyad,
                            TC = cagriMerkezi.TC,
                            Adres = cagriMerkezi.Adres,
                            TelNo = cagriMerkezi.TelNo,
                            Aciklama = cagriMerkezi.Aciklama,
                            BasvuruKodu = cagriMerkezi.BasvuruKodu,
                            ResimUrl = cagriMerkezi.ResimUrl,
                            DurumId = DurumId.Value,
                            BirimId = BirimId.Value,
                            DepId = DepId.Value
                        };

                        _sikayetRepository.Ekle(yeniSikayet);
                        _sikayetRepository.Kaydet();

                        //burada şikayet oluşturuldu, başvuru kodu mesajı versin
                        // TempData kullanarak mesajı taşı
                        TempData["Mesaj"] = $"Şikayet oluşturulmuştur. Başvuru kodunuz: {cagriMerkezi.BasvuruKodu}";
                        return RedirectToAction("Index", "Sikayet");
                    }
                    else
                    {
                        // Eğer BirimId ve DepId değerleri yoksa CagriMerkezi tablosuna ekleyin
                        _cagriMerkeziRepository.Ekle(cagriMerkezi);
                        _cagriMerkeziRepository.Kaydet();

                        //burada şikayet oluşturuldu, başvuru kodu mesajı verilip home indexe gitsin
                        // TempData kullanarak mesajı taşı
                        TempData["Mesaj"] = $"Çağrı merkezine şikayet başvurunuz alınmıştır. Başvuru kodunuz: {cagriMerkezi.BasvuruKodu}";
                        return RedirectToAction("Index", "Home");
                    }
                }
                
            }
            if (BirimId.HasValue && DepId.HasValue)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string cagriPath = Path.Combine(wwwRootPath, @"img");

                if (file != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(cagriPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    cagriMerkezi.ResimUrl = @"\img\" + file.FileName;
                }

                Sikayet yeniSikayet = new Sikayet
                {
                    Ad = cagriMerkezi.Ad,
                    Soyad = cagriMerkezi.Soyad,
                    TC = cagriMerkezi.TC,
                    Adres = cagriMerkezi.Adres,
                    TelNo = cagriMerkezi.TelNo,
                    Aciklama = cagriMerkezi.Aciklama,
                    BasvuruKodu = cagriMerkezi.BasvuruKodu,
                    ResimUrl = cagriMerkezi.ResimUrl,
                    DurumId = DurumId.Value,
                    BirimId = BirimId.Value,
                    DepId = DepId.Value
                };

                _sikayetRepository.Ekle(yeniSikayet);
                _sikayetRepository.Kaydet();

                _cagriMerkeziRepository.Detach(cagriMerkezi);

                var silinecekCagriMerkezi = _cagriMerkeziRepository.Get(u => u.Id == cagriMerkezi.Id);

                if (silinecekCagriMerkezi != null)
                {
                    _cagriMerkeziRepository.Sil(silinecekCagriMerkezi);
                    _cagriMerkeziRepository.Kaydet();
                }

                return RedirectToAction("Index", "Sikayet");
            }

            else
            {
                // Eğer BirimId ve DepId değerleri yoksa CagriMerkezi tablosunu güncelleyin
                _cagriMerkeziRepository.Guncelle(cagriMerkezi);
                _cagriMerkeziRepository.Kaydet();
                return RedirectToAction("GelenSikayet", "CagriMerkezi");
            }
                
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
            string yetki = HttpContext.Session.GetString("Yetki");
            ViewBag.Yetki = yetki;

            if (HttpContext.Session.GetString("GirisKontrol") == "ok" || yetki == "admin" || yetki == "user")
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                CagriMerkezi? cagriVt = _cagriMerkeziRepository.Get(u => u.Id == id);
                if (cagriVt == null)
                {
                    return NotFound();
                }
                return View(cagriVt);
            }
            else
            {
                return RedirectToAction("Login", "Kullanici");
            }
        }


        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            CagriMerkezi? cagri = _cagriMerkeziRepository.Get(u => u.Id == id);
            if (cagri == null)
            {
                return NotFound();
            }
            _cagriMerkeziRepository.Sil(cagri);
            _cagriMerkeziRepository.Kaydet();
            return RedirectToAction("GelenSikayet", "CagriMerkezi");
        }



    }
}
