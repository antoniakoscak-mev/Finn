using Finn.Data;
using Finn.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finn.Controllers
{
    public class PrihodiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrihodiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var sviPrihodi = _context.Prihodi.ToList();

            return View(sviPrihodi);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Prihod noviPrihod)
        {
            if (ModelState.IsValid)
            {
                _context.Prihodi.Add(noviPrihod);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(noviPrihod);
        }

        public IActionResult Delete(int id)
        {
            var prihodZaBrisanje =
                _context.Prihodi.Find(id);

            if (prihodZaBrisanje != null)
            {
                _context.Prihodi.Remove(prihodZaBrisanje);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}