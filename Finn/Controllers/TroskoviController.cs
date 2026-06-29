using Finn.Data;
using Finn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Finn.Controllers
{
    [Authorize]
    public class TroskoviController : Controller
    {
        private readonly ApplicationDbContext
            _context;

        public TroskoviController(
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

            var troskovi =
                _context.Troskovi
                .Where(x => x.UserId == userId)
                .AsQueryable();

            // RUČNI INTERVAL GASI QUICK FILTERE

            if (datumOd.HasValue || datumDo.HasValue)
            {
                period = null;
            }

            // KATEGORIJA

            if (!string.IsNullOrEmpty(kategorija))
            {
                troskovi =
                    troskovi.Where(x =>
                        x.Kategorija.Contains(kategorija));
            }

            // DATUMI

            if (datumOd.HasValue)
            {
                troskovi =
                    troskovi.Where(x =>
                        x.Datum >= datumOd.Value);
            }

            if (datumDo.HasValue)
            {
                troskovi =
                    troskovi.Where(x =>
                        x.Datum <= datumDo.Value);
            }

            // QUICK FILTERI

            if (!string.IsNullOrEmpty(period))
            {
                if (period == "ThisMonth")
                {
                    troskovi =
                        troskovi.Where(x =>
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

                    troskovi =
                        troskovi.Where(x =>
                            x.Datum.Month ==
                            prosliMjesec.Month
                            &&
                            x.Datum.Year ==
                            prosliMjesec.Year);
                }

                if (period == "ThisYear")
                {
                    troskovi =
                        troskovi.Where(x =>
                            x.Datum.Year ==
                            DateTime.Now.Year);
                }

                if (period == "LastYear")
                {
                    troskovi =
                        troskovi.Where(x =>
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
                ? Kategorije.PoslovniRashodi
                : Kategorije.OsobniRashodi;

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
                troskovi
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
                ? Kategorije.PoslovniRashodi
                : Kategorije.OsobniRashodi;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Trosak trosak)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije =
                    _context.KategorijeRashoda.ToList();

                return View(trosak);
            }

            trosak.UserId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            _context.Troskovi.Add(trosak);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var trosak =
                _context.Troskovi
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
                ? Kategorije.PoslovniRashodi
                : Kategorije.OsobniRashodi;

            return View(trosak);
        }

        [HttpPost]
        public IActionResult Edit(Trosak trosak)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije =
                    _context.KategorijeRashoda.ToList();

                return View(trosak);
            }

            trosak.UserId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            _context.Troskovi.Update(trosak);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var trosak =
                _context.Troskovi
                .FirstOrDefault(x =>
                    x.Id == id
                    &&
                    x.UserId == userId);

            if (trosak != null)
            {
                _context.Troskovi.Remove(trosak);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}