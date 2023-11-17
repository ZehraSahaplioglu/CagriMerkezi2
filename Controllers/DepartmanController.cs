using CagriMerkezi2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CagriMerkezi2.Controllers
{
    public class DepartmanController : Controller
    {

        private readonly IDepartmanRepository _departmanRepository;
        private readonly IBirimRepository _birimRepository;

        public DepartmanController(IDepartmanRepository departmanRepository, IBirimRepository birimRepository)
        {
            _departmanRepository = departmanRepository;
            _birimRepository = birimRepository;
        }


        
        public IActionResult Index()
        {
            ViewBag.BrDepList = _birimRepository.GetAll()
        .Select(b => new SelectListItem
        {
            Text = b.Ad,
            Value = b.Id.ToString()
        }).ToList();

            List<Departman> objDepartmanList = _departmanRepository.GetAll(includeProps:"Birim").ToList();
            return View(objDepartmanList);
        }

        [HttpGet]
        public IActionResult GetFilteredDep(int birimId)
        {
            List<Departman> filteredDepList;

            if (birimId > 0)
            {
                filteredDepList = _departmanRepository.GetFilteredDep(birimId);
            }
            else
            {
                filteredDepList = _departmanRepository.GetAll(includeProps: "Departman").ToList();
            }

            return PartialView("_DepartmanListPartial", filteredDepList);
        }



        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> BirimList = _birimRepository.GetAll().Select(i => new SelectListItem
            {
                Text = i.Ad,
                Value = i.Id.ToString()
            });
            ViewBag.BirimList = BirimList;

            if (id == null || id == 0)
            {
                return View();
            }
            else
            {
                Departman? departmanVt = _departmanRepository.Get(u => u.Id == id);
                if (departmanVt == null)
                {
                    return NotFound();
                }
                return View(departmanVt);
            }
        }

        [HttpPost]
        public IActionResult EkleGuncelle(Departman departman)
        {
            if (!ModelState.IsValid)
            {
                if (departman.Id == 0)
                {
                    _departmanRepository.Ekle(departman);
                }
                else
                {
                    _departmanRepository.Guncelle(departman);
                }

                _departmanRepository.Kaydet();
                return RedirectToAction("Index", "Departman");
            }

            return View();
        }

        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Departman? departmanVt = _departmanRepository.Get(u =>u.Id == id);
            if(departmanVt == null)
            {
                return NotFound();
            }
            return View(departmanVt);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            Departman? departman = _departmanRepository.Get(u =>u.Id == id);
            if (departman == null)
            {
                return NotFound();
            }
            _departmanRepository.Sil(departman);
            _departmanRepository.Kaydet();
            return RedirectToAction("Index", "Departman");
        }

    }
}
