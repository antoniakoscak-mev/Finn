using Finn.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Finn.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext
            _context;

        public HomeController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(
            string? period,
            DateTime? datumOd,
            DateTime? datumDo,
            string? kategorija)
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

            // RUČNI INTERVAL GASI QUICK FILTERE

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

                troskovi =
                    troskovi.Where(x =>
                        x.Kategorija.Contains(kategorija));
            }

            // DATUMI

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

                    prihodi =
                        prihodi.Where(x =>
                            x.Datum.Month ==
                            prosliMjesec.Month
                            &&
                            x.Datum.Year ==
                            prosliMjesec.Year);

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

            // LISTE

            var prihodiLista =
                prihodi
                .OrderByDescending(x => x.Datum)
                .Take(5)
                .ToList();

            var troskoviLista =
                troskovi
                .OrderByDescending(x => x.Datum)
                .Take(5)
                .ToList();

            ViewBag.Prihodi =
                prihodiLista;

            ViewBag.Troskovi =
                troskoviLista;

            // SAŽETAK

            ViewBag.UkupniPrihodi =
                prihodi.Sum(x => x.Iznos);

            ViewBag.UkupniRashodi =
                troskovi.Sum(x => x.Iznos);

            ViewBag.Stanje =
                ViewBag.UkupniPrihodi -
                ViewBag.UkupniRashodi;

            // KATEGORIJE

            var prihodKartice =
    prihodi
    .GroupBy(x => x.Kategorija)
    .Select(x => new
    {
        Naziv = x.Key,

        Boja =
            x.Key == "Plaća" ? "#2563eb" :
            x.Key == "Bonus" ? "#16a34a" :
            x.Key == "Investicije" ? "#7c3aed" :
            x.Key == "Poklon" ? "#ec4899" :
            x.Key == "Prodaja" ? "#2563eb" :
            x.Key == "Usluge" ? "#16a34a" :
            x.Key == "Najam" ? "#f59e0b" :
            "#64748b",

        Boja20 =
            x.Key == "Plaća" ? "#dbeafe" :
            x.Key == "Bonus" ? "#dcfce7" :
            x.Key == "Investicije" ? "#ede9fe" :
            x.Key == "Poklon" ? "#fce7f3" :
            x.Key == "Prodaja" ? "#dbeafe" :
            x.Key == "Usluge" ? "#dcfce7" :
            x.Key == "Najam" ? "#fef3c7" :
            "#f1f5f9",

        Iznos = x.Sum(p => p.Iznos),

        Ikona =
            x.Key == "Plaća" ? "💼" :
            x.Key == "Bonus" ? "🎁" :
            x.Key == "Investicije" ? "📈" :
            x.Key == "Poklon" ? "🎉" :
            x.Key == "Prodaja" ? "🛒" :
            x.Key == "Usluge" ? "🤝" :
            x.Key == "Najam" ? "🏢" :
            "💰"
    })
    .ToList();

            var rashodKartice =
     troskovi
     .GroupBy(x => x.Kategorija)
     .Select(x => new
     {
         Naziv = x.Key,

         Boja =
             x.Key == "Hrana" ? "#f97316" :
             x.Key == "Stanovanje" ? "#2563eb" :
             x.Key == "Prijevoz" ? "#14b8a6" :
             x.Key == "Shopping" ? "#ec4899" :
             x.Key == "Zdravlje" ? "#ef4444" :
             x.Key == "Putovanja" ? "#8b5cf6" :
             x.Key == "Marketing" ? "#f59e0b" :
             x.Key == "Softver" ? "#6366f1" :
             x.Key == "Plaće" ? "#16a34a" :
             x.Key == "Porezi" ? "#dc2626" :
             x.Key == "Oprema" ? "#475569" :
             x.Key == "Najam prostora" ? "#0891b2" :
             "#64748b",

         Boja20 =
             x.Key == "Hrana" ? "#ffedd5" :
             x.Key == "Stanovanje" ? "#dbeafe" :
             x.Key == "Prijevoz" ? "#ccfbf1" :
             x.Key == "Shopping" ? "#fce7f3" :
             x.Key == "Zdravlje" ? "#fee2e2" :
             x.Key == "Putovanja" ? "#ede9fe" :
             x.Key == "Marketing" ? "#fef3c7" :
             x.Key == "Softver" ? "#e0e7ff" :
             x.Key == "Plaće" ? "#dcfce7" :
             x.Key == "Porezi" ? "#fee2e2" :
             x.Key == "Oprema" ? "#e2e8f0" :
             x.Key == "Najam prostora" ? "#cffafe" :
             "#f1f5f9",

         Iznos = x.Sum(t => t.Iznos),

         Ikona =
             x.Key == "Hrana" ? "🍔" :
             x.Key == "Stanovanje" ? "🏠" :
             x.Key == "Prijevoz" ? "🚗" :
             x.Key == "Shopping" ? "🛍️" :
             x.Key == "Zdravlje" ? "❤️" :
             x.Key == "Putovanja" ? "✈️" :
             x.Key == "Marketing" ? "📢" :
             x.Key == "Softver" ? "💻" :
             x.Key == "Plaće" ? "👥" :
             x.Key == "Porezi" ? "🏛️" :
             x.Key == "Oprema" ? "🖨️" :
             x.Key == "Najam prostora" ? "🏢" :
             "💸"
     })
     .ToList();

            ViewBag.PrihodKartice =
                prihodKartice;

            ViewBag.RashodKartice =
                rashodKartice;

            // FILTERI

            ViewBag.DatumOd =
                datumOd?.ToString("yyyy-MM-dd");

            ViewBag.DatumDo =
                datumDo?.ToString("yyyy-MM-dd");

            ViewBag.Kategorije =
                prihodi
                .Select(x => x.Kategorija)
                .Concat(
                    troskovi.Select(x => x.Kategorija))
                .Distinct()
                .ToList();

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

            return View();
        }

        public IActionResult Pdf()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var troskovi =
                _context.Troskovi
                .Where(x => x.UserId == userId)
                .ToList();

            var prihodi =
                _context.Prihodi
                .Where(x => x.UserId == userId)
                .ToList();

            Finn.Pdf.IspisFinancija pdf =
                new Finn.Pdf.IspisFinancija(
                    troskovi,
                    prihodi);

            return File(
                pdf.Podaci,
                "application/pdf",
                "FinancijskoIzvjesce.pdf");
        }
    }
}