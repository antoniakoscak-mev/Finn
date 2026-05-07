using Finn.Data;
using Finn.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finn.Controllers
{
    public class TroskoviController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TroskoviController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var sviTroskovi = _context.Troskovi.ToList();

            return View(sviTroskovi);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Trosak noviTrosak)
        {
            if (ModelState.IsValid)
            {
                _context.Troskovi.Add(noviTrosak);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(noviTrosak);
        }

        public IActionResult Delete(int id)
        {
            var trosakZaBrisanje =
                _context.Troskovi.Find(id);

            if (trosakZaBrisanje != null)
            {
                _context.Troskovi.Remove(trosakZaBrisanje);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}