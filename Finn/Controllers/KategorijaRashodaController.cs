using Finn.Data;
using Finn.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finn.Controllers
{
    public class KategorijaRashodaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KategorijaRashodaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var kategorije =
                _context.KategorijeRashoda.ToList();

            return View(kategorije);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(KategorijaRashoda kategorija)
        {
            if (ModelState.IsValid)
            {
                _context.KategorijeRashoda.Add(kategorija);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(kategorija);
        }

        public IActionResult Delete(int id)
        {
            var kategorija =
                _context.KategorijeRashoda.Find(id);

            if (kategorija != null && !kategorija.Zadana)
            {
                _context.KategorijeRashoda.Remove(kategorija);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}