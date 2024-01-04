using CagriMerkezi2.Models;
using CagriMerkezi2.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace CagriMerkezi2.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly ICalisanRepository _calisanRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public KullaniciController(IKullaniciRepository kullaniciRepository ,ICalisanRepository calisanRepository, UserManager<IdentityUser> userManager)
        {
            _kullaniciRepository = kullaniciRepository;
            _calisanRepository = calisanRepository;
            _userManager = userManager;

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


        public IActionResult YetkiVer(int calisanid)
        {
            var calisan = _calisanRepository.Get(u => u.Id == calisanid);
            if (calisan == null)
            {
                return NotFound();
            }

            // Mevcut kullanıcıyı CalisanId'ye göre bul
            var mevcutKullanici = _kullaniciRepository.Get(u => u.CalisanId == calisanid);

            // Yeni kullanıcı oluştur
            mevcutKullanici = new Kullanici
            {
                Ad = calisan.Ad,
                Soyad = calisan.Soyad,
                TC = calisan.TC,
                CalisanId = calisan.Id,
                // Diğer alanları da burada doldurabilirsiniz
            };

            return View(mevcutKullanici); // Burada modeli view'a geçiriyoruz
        }

        [HttpPost]
        public IActionResult YetkiVer(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                _kullaniciRepository.Ekle(kullanici);
                _kullaniciRepository.Kaydet();
                return RedirectToAction("Index", "Kullanici");
            }

            return View(kullanici);
        }


        public IActionResult Guncelle(int? id)
        {
            IEnumerable<SelectListItem> CalisanList = _calisanRepository.GetAll().Select(i => new SelectListItem
            {
                Text = i.Ad,
                Value = i.Id.ToString()
            });
            ViewBag.CalisanList = CalisanList;

            var roller = new List<SelectListItem>
            {
                new SelectListItem { Value = KullaniciRolleri.Role_Admin, Text = "Admin" },
                new SelectListItem { Value = KullaniciRolleri.Role_Calisan, Text = "Calisan" }
            };

            ViewBag.Roller = roller;

            
            Kullanici? kullaniciVt = _kullaniciRepository.Get(u => u.Id == id);
            if (kullaniciVt == null)
            {
                return NotFound();
            }
            return View(kullaniciVt);
            
        }

        [HttpPost]
        public IActionResult Guncelle(Kullanici kullanici)
        {
            if (!ModelState.IsValid)
            {
                _kullaniciRepository.Guncelle(kullanici);
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


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Kullanici model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kullanıcı doğrulama işlemi
            var user = await _userManager.FindByNameAsync(model.TC); // TC ile kullanıcı bulma
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Sifre))
            {
                // Kullanıcıyı oturuma dahil et
                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "TC Kimlik Numarası veya şifre hatalı");
            return View(model);
        }


    }
}
