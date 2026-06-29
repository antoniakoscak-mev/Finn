using Finn.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Globalization;

namespace Finn.Controllers
{
    [Authorize]
    public class AnalitikaController : Controller
    {
        private readonly ApplicationDbContext
            _context;

        public AnalitikaController(
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

            var troskovi =
                _context.Troskovi
                .Where(x => x.UserId == userId)
                .AsQueryable();

            if (datumOd.HasValue || datumDo.HasValue)
            {
                period = null;
            }

            if (datumOd.HasValue)
            {
                prihodi =
                    prihodi.Where(x =>
                        x.Datum >= datumOd.Value);

                troskovi =
                    troskovi.Where(x =>
                        x.Datum >= datumOd.Value);
            }

            if (datumDo.HasValue)
            {
                prihodi =
                    prihodi.Where(x =>
                        x.Datum <= datumDo.Value);

                troskovi =
                    troskovi.Where(x =>
                        x.Datum <= datumDo.Value);
            }

            if (!string.IsNullOrEmpty(kategorija))
            {
                prihodi =
                    prihodi.Where(x =>
                        x.Kategorija.Contains(kategorija));

                troskovi =
                    troskovi.Where(x =>
                        x.Kategorija.Contains(kategorija));
            }

           

            if (!string.IsNullOrEmpty(period))
            {
                if (period == "ThisMonth")
                {
                    prihodi =
                        prihodi.Where(x =>
                            x.Datum.Month == DateTime.Now.Month
                            &&
                            x.Datum.Year == DateTime.Now.Year);

                    troskovi =
                        troskovi.Where(x =>
                            x.Datum.Month == DateTime.Now.Month
                            &&
                            x.Datum.Year == DateTime.Now.Year);
                }

                if (period == "LastMonth")
                {
                    var prosliMjesec =
                        DateTime.Now.AddMonths(-1);

                    prihodi =
                        prihodi.Where(x =>
                            x.Datum.Month == prosliMjesec.Month
                            &&
                            x.Datum.Year == prosliMjesec.Year);

                    troskovi =
                        troskovi.Where(x =>
                            x.Datum.Month == prosliMjesec.Month
                            &&
                            x.Datum.Year == prosliMjesec.Year);
                }

                if (period == "ThisYear")
                {
                    prihodi =
                        prihodi.Where(x =>
                            x.Datum.Year ==
                            DateTime.Now.Year);

                    troskovi =
                        troskovi.Where(x =>
                            x.Datum.Year ==
                            DateTime.Now.Year);
                }

                if (period == "LastYear")
                {
                    prihodi =
                        prihodi.Where(x =>
                            x.Datum.Year ==
                            DateTime.Now.Year - 1);

                    troskovi =
                        troskovi.Where(x =>
                            x.Datum.Year ==
                            DateTime.Now.Year - 1);
                }
            }

            var prihodiLista =
                prihodi.ToList();

            var troskoviLista =
                troskovi.ToList();

            ViewBag.Prihodi =
                prihodiLista;

            ViewBag.Troskovi =
                troskoviLista;

            ViewBag.UkupniPrihodi =
                prihodiLista.Sum(x => x.Iznos);

            ViewBag.UkupniRashodi =
                troskoviLista.Sum(x => x.Iznos);

            ViewBag.Stanje =
                ViewBag.UkupniPrihodi -
                ViewBag.UkupniRashodi;

            ViewBag.PrihodiGraf =
                prihodiLista
                .GroupBy(x => x.Kategorija)
                .Select(x => new
                {
                    kategorija = x.Key,
                    iznos = x.Sum(y => y.Iznos)
                })
                .ToList();

            ViewBag.TroskoviGraf =
                troskoviLista
                .GroupBy(x => x.Kategorija)
                .Select(x => new
                {
                    kategorija = x.Key,
                    iznos = x.Sum(y => y.Iznos)
                })
                .ToList();

                 ViewBag.PrihodiMjeseci =
            prihodiLista
            .GroupBy(x => x.Datum.Month)
            .Select(x => new
            {
                 mjesec =
            new DateTime(2024, x.Key, 1)
            .ToString(
              "MMM",
              new CultureInfo("hr-HR")),

                iznos =
                  x.Sum(y => y.Iznos)
             })
             .ToList();

                 ViewBag.RashodiMjeseci =
            troskoviLista
            .GroupBy(x => x.Datum.Month)
            .Select(x => new
            {
                mjesec =
            new DateTime(2024, x.Key, 1)
            .ToString(
                "MMM",
                new CultureInfo("hr-HR")),

                iznos =
                    x.Sum(y => y.Iznos)
            })
            .ToList();

            ViewBag.Kategorije =
                prihodiLista
                .Select(x => x.Kategorija)
                .Concat(
                    troskoviLista.Select(x => x.Kategorija))
                .Distinct()
                .ToList();


            ViewBag.AktivniPeriod = period;

            ViewBag.AktivnaKategorija = kategorija;

            ViewBag.DatumOd = datumOd?.ToString("yyyy-MM-dd");

            ViewBag.DatumDo = datumDo?.ToString("yyyy-MM-dd");

            return View();
        }
    }
}