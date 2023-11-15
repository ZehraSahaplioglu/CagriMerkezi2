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
        public readonly IWebHostEnvironment _webHostEnvironment;


        public SikayetController (ISikayetRepository sikayetRepository, IBirimRepository birimRepository, IWebHostEnvironment webHostEnvironment, IDepartmanRepository departmanRepository)
        {
            _sikayetRepository = sikayetRepository;
            _birimRepository = birimRepository;
            _webHostEnvironment = webHostEnvironment;
            _departmanRepository = departmanRepository;

            var result = from sikayet in _sikayetRepository.GetAll()
                         join birim in _birimRepository.GetAll() on sikayet.BirimId equals birim.Id
                         join departman in _departmanRepository.GetAll() on sikayet.DepId equals departman.Id
                         select new
                         {
                             sikayetAdi = sikayet.Ad,
                             departmanAdi = departman.Ad,
                             birimAdi = birim.Ad,
                         };

        }

        public IActionResult Index()
        {
            List<Sikayet> objSikayetList = _sikayetRepository.GetAll(includeProps: "Departman").ToList();
            return View(objSikayetList);
        }

        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> BirimSikayetList = _birimRepository.GetAll().Select(b => new SelectListItem
            {
                Text = b.Ad,
                Value = b.Id.ToString()
            });
            ViewBag.BirimSikayetList = BirimSikayetList;

            IEnumerable<SelectListItem> DepSikayetList = _departmanRepository.GetAll().Select(d => new SelectListItem
            {
                Text = d.Ad,
                Value = d.Id.ToString()
            });
            ViewBag.DepSikayetList = DepSikayetList;

            if (id == null || id == 0)
            {
                return View();
            }
            else
            {
                Sikayet? sikayetVt = _sikayetRepository.Get(u  => u.Id == id);
                if(sikayetVt == null)
                {
                    return NotFound();
                }
                
                return View(sikayetVt);
            }
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

                if(sikayet.Id == 0)
                {
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
