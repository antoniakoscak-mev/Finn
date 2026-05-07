using Finn.Data;
using Finn.Pdf;
using Microsoft.AspNetCore.Mvc;

namespace Finn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var troskovi = _context.Troskovi.ToList();

            var prihodi = _context.Prihodi.ToList();

            decimal ukupniTroskovi =
                troskovi.Sum(t => t.Iznos);

            decimal ukupniPrihodi =
                prihodi.Sum(p => p.Iznos);

            decimal stanje =
                ukupniPrihodi - ukupniTroskovi;

            ViewBag.UkupniPrihodi =
                ukupniPrihodi;

            ViewBag.UkupniTroskovi =
                ukupniTroskovi;

            ViewBag.Stanje =
                stanje;

            ViewBag.Troskovi =
                troskovi;

            ViewBag.Prihodi =
                prihodi;

            return View();
        }

        public IActionResult PdfIzvjestaj()
        {
            var troskovi = _context.Troskovi.ToList();

            var prihodi = _context.Prihodi.ToList();

            IspisFinancija ispis =
                new IspisFinancija(troskovi, prihodi);

            return File(
                ispis.Podaci,
                "application/pdf",
                "financijski_izvjestaj.pdf");
        }
    }
}