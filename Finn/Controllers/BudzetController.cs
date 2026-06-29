using Finn.Data;
using Finn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Finn.Controllers
{
    [Authorize]
    public class BudzetController : Controller
    {
        private readonly ApplicationDbContext
            _context;

        public BudzetController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var budzeti =
                _context.Budzeti
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .ToList();

            foreach (var budzet in budzeti)
            {
                budzet.Potroseno =
                    _context.Troskovi
                    .Where(x =>
                        x.UserId == userId
                        &&
                        x.Datum >= budzet.DatumOd
                        &&
                        x.Datum <= budzet.DatumDo)
                    .Sum(x => (decimal?)x.Iznos) ?? 0;

                budzet.Preostalo =
                    budzet.Iznos - budzet.Potroseno;
            }

            return View(budzeti);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Budzet budzet)
        {
            if (!ModelState.IsValid)
            {
                return View(budzet);
            }

            if (budzet.DatumOd > budzet.DatumDo)
            {
                ModelState.AddModelError(
                    "",
                    "Datum početka ne može biti veći od datuma završetka.");

                return View(budzet);
            }

            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            // PROVJERA POSTOJI LI VEĆ BUDŽET

            var postojiBudzet =
                _context.Budzeti
                .FirstOrDefault(x =>
                    x.UserId == userId
                    &&
                    x.DatumOd == budzet.DatumOd
                    &&
                    x.DatumDo == budzet.DatumDo);

            if (postojiBudzet != null)
            {
                TempData["Greska"] =
                    $"Već postoji budžet '{postojiBudzet.Naziv}' za ovo razdoblje. Možete ga urediti.";

                return View(budzet);
            }

            budzet.UserId = userId;

            _context.Budzeti.Add(budzet);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var userId =User.FindFirstValue(ClaimTypes.NameIdentifier);

            var budzet =
                _context.Budzeti
                .FirstOrDefault(x =>
                    x.Id == id
                    &&
                    x.UserId == userId);

            return View(budzet);
        }

        [HttpPost]
        public IActionResult Edit(Budzet budzet)


        {

            if (!ModelState.IsValid)
            {
                return View(budzet);
            }

            if (budzet.DatumOd > budzet.DatumDo)
            {
                ModelState.AddModelError(
                    "",
                    "Datum početka ne može biti veći od datuma završetka.");

                return View(budzet);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var postojiBudzet =
                _context.Budzeti
                .FirstOrDefault(x =>
                    x.UserId == userId
                    &&
                    x.Id != budzet.Id
                    &&
                    x.DatumOd == budzet.DatumOd
                    &&
                    x.DatumDo == budzet.DatumDo);

            if (postojiBudzet != null)
            {
                TempData["Greska"] =
                $"Već postoji budžet '{postojiBudzet.Naziv}' za odabrano razdoblje. Ako želite promijeniti budžet, otvorite opciju Uredi.";

                return View(budzet);
            }

            budzet.UserId = userId;

            _context.Budzeti.Update(budzet);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var budzet =
                _context.Budzeti
                .FirstOrDefault(x =>
                    x.Id == id
                    &&
                    x.UserId == userId);

            if (budzet != null)
            {
                _context.Budzeti.Remove(budzet);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}