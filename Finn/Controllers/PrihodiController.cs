using Finn.Data;
using Finn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Finn.Controllers
{
    [Authorize]
    public class PrihodiController : Controller
    {
        private readonly ApplicationDbContext
            _context;

        public PrihodiController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(
            string? period,
            string? kategorija,
            DateTime? datumOd,
            DateTime? datumDo)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var prihodi =
                _context.Prihodi
                .Where(x => x.UserId == userId)
                .AsQueryable();

            

            if (datumOd.HasValue || datumDo.HasValue)
            {
                period = null;
            }

            // KATEGORIJA

            if (!string.IsNullOrEmpty(kategorija))
            {
                prihodi =
                    prihodi.Where(x =>
                        x.Kategorija.Contains(kategorija));
            }

            // DATUMI

            if (datumOd.HasValue)
            {
                prihodi =
                    prihodi.Where(x =>
                        x.Datum >= datumOd.Value);
            }

            if (datumDo.HasValue)
            {
                prihodi =
                    prihodi.Where(x =>
                        x.Datum <= datumDo.Value);
            }

            // QUICK FILTERI

            if (!string.IsNullOrEmpty(period))
            {
                if (period == "ThisMonth")
                {
                    prihodi =
                        prihodi.Where(x =>
                            x.Datum.Month ==
                            DateTime.Now.Month
                            &&
                            x.Datum.Year ==
                            DateTime.Now.Year);
                }

                if (period == "LastMonth")
                {
                    var prosliMjesec =
                        DateTime.Now.AddMonths(-1);

                    prihodi =
                        prihodi.Where(x =>
                            x.Datum.Month ==
                            prosliMjesec.Month
                            &&
                            x.Datum.Year ==
                            prosliMjesec.Year);
                }

                if (period == "ThisYear")
                {
                    prihodi =
                        prihodi.Where(x =>
                            x.Datum.Year ==
                            DateTime.Now.Year);
                }

                if (period == "LastYear")
                {
                    prihodi =
                        prihodi.Where(x =>
                            x.Datum.Year ==
                            DateTime.Now.Year - 1);
                }
            }

            var settings =
            _context.UserSettings
            .FirstOrDefault(x =>
                x.UserId == userId);

            ViewBag.Kategorije =
                settings?.TipRacuna == "Poslovni"
                ? Kategorije.PoslovniPrihodi
                : Kategorije.OsobniPrihodi;

            ViewBag.DatumOd =
                datumOd?.ToString("yyyy-MM-dd");

            ViewBag.DatumDo =
                datumDo?.ToString("yyyy-MM-dd");

            ViewBag.AktivniPeriod =
                period;

            ViewBag.AktivnaKategorija =
                kategorija;

            // FILTER PORUKA

            string filterPoruka = "";

            if (!string.IsNullOrEmpty(period))
            {
                if (period == "ThisMonth")
                {
                    filterPoruka =
                        "Ovaj mjesec";
                }

                if (period == "LastMonth")
                {
                    filterPoruka =
                        "Prošli mjesec";
                }

                if (period == "ThisYear")
                {
                    filterPoruka =
                        "Ova godina";
                }

                if (period == "LastYear")
                {
                    filterPoruka =
                        "Prošla godina";
                }
            }

            if (datumOd.HasValue || datumDo.HasValue)
            {
                filterPoruka =
                    "Interval: ";

                if (datumOd.HasValue)
                {
                    filterPoruka +=
                        datumOd.Value
                        .ToString("dd/MM/yyyy");
                }

                filterPoruka +=
                    " - ";

                if (datumDo.HasValue)
                {
                    filterPoruka +=
                        datumDo.Value
                        .ToString("dd/MM/yyyy");
                }
            }

            if (string.IsNullOrEmpty(period)
                &&
                !datumOd.HasValue
                &&
                !datumDo.HasValue)
            {
                filterPoruka =
                    "Svi periodi";
            }

            if (!string.IsNullOrEmpty(kategorija))
            {
                filterPoruka +=
                    " • Kategorija: " +
                    kategorija;
            }

            ViewBag.FilterPoruka =
                filterPoruka;

            return View(
                prihodi
                .OrderByDescending(x => x.Datum)
                .ToList());
        }

        public IActionResult Create()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var settings =
                _context.UserSettings
                .FirstOrDefault(x =>
                    x.UserId == userId);

            ViewBag.Kategorije =
                settings?.TipRacuna == "Poslovni"
                ? Kategorije.PoslovniPrihodi
                : Kategorije.OsobniPrihodi;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Prihod prihod)
        {
            if (!ModelState.IsValid)
            {
                var settings =
                _context.UserSettings
                .FirstOrDefault(x =>
                   x.UserId == prihod.UserId);

                ViewBag.Kategorije =
                    settings?.TipRacuna == "Poslovni"
                    ? Kategorije.PoslovniPrihodi
                    : Kategorije.OsobniPrihodi;

                return View(prihod);
            }

            prihod.UserId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            _context.Prihodi.Add(prihod);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var prihod =
                _context.Prihodi
                .FirstOrDefault(x =>
                    x.Id == id
                    &&
                    x.UserId == userId);

            var settings =
            _context.UserSettings
            .FirstOrDefault(x =>
                 x.UserId == userId);
    
            ViewBag.Kategorije =
                settings?.TipRacuna == "Poslovni"
                ? Kategorije.PoslovniPrihodi
                : Kategorije.OsobniPrihodi;

            return View(prihod);
        }

        [HttpPost]
        public IActionResult Edit(Prihod prihod)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije =
                    _context.KategorijePrihoda.ToList();

                return View(prihod);
            }

            prihod.UserId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            _context.Prihodi.Update(prihod);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var prihod =
                _context.Prihodi
                .FirstOrDefault(x =>
                    x.Id == id
                    &&
                    x.UserId == userId);

            if (prihod != null)
            {
                _context.Prihodi.Remove(prihod);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}