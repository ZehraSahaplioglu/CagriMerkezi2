using CagriMerkezi2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CagriMerkezi2.Controllers
{
    public class BirimController : Controller
    {

        private readonly IBirimRepository _birimRepository;

        public BirimController (IBirimRepository context)
        {
            _birimRepository = context;
        }

        public IActionResult Index()
        {
            List<Birim> objBirimList = _birimRepository.GetAll().ToList();
            return View(objBirimList);
        }


        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Birim birim)
        {
            if (!ModelState.IsValid)
            {
                _birimRepository.Ekle(birim);
                _birimRepository.Kaydet();
                return RedirectToAction("Index", "Birim");
            }
            return View();
        }

        public IActionResult Guncelle(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            Birim? birimVt = _birimRepository.Get(u=>u.Id == id);
            if (birimVt == null)
            {
                return NotFound();
            }
            return View(birimVt);
        }


        [HttpPost]
        public IActionResult Guncelle(Birim birim)
        {
            if (!ModelState.IsValid)
            {
                _birimRepository.Guncelle(birim);
                _birimRepository.Kaydet();
                return RedirectToAction("Index", "Birim");
            }
            return View();
        }

        public IActionResult Sil(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Birim? birimVt = _birimRepository.Get(u => u.Id == id);
            if(birimVt == null)
            {
                return NotFound();
            }
            return View(birimVt);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            Birim? birim = _birimRepository.Get(u => u.Id==id);
            if (birim == null)
            {
                return NotFound();
            }
            _birimRepository.Sil(birim);
            _birimRepository.Kaydet();
            return RedirectToAction("Index", "Birim");
        }

    }
}
