using Finn.Data;
using Finn.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finn.Controllers
{
    public class KategorijaPrihodaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KategorijaPrihodaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var kategorije =
                _context.KategorijePrihoda.ToList();

            return View(kategorije);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(KategorijaPrihoda kategorija)
        {
            if (ModelState.IsValid)
            {
                _context.KategorijePrihoda.Add(kategorija);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(kategorija);
        }

        public IActionResult Delete(int id)
        {
            var kategorija =
                _context.KategorijePrihoda.Find(id);

            if (kategorija != null && !kategorija.Zadana)
            {
                _context.KategorijePrihoda.Remove(kategorija);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}