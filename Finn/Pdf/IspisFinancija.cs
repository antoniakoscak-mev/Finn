using Finn.Models;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Finn.Pdf
{
    public class IspisFinancija
    {
        public byte[] Podaci { get; set; }

        public IspisFinancija(
            List<Trosak> troskovi,
            List<Prihod> prihodi)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer =
                    new PdfWriter(memoryStream);

                writer.SetCloseStream(false);

                using (PdfDocument pdfDokument =
                    new PdfDocument(writer))
                {
                    Document dokument =
                        new Document(pdfDokument);

                    Paragraph naslov =
                        new Paragraph("Financijski izvještaj")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(20);

                    dokument.Add(naslov);

                    dokument.Add(new Paragraph(" "));

                    decimal ukupniTroskovi =
                        troskovi.Sum(t => t.Iznos);

                    decimal ukupniPrihodi =
                        prihodi.Sum(p => p.Iznos);

                    decimal stanje =
                        ukupniPrihodi - ukupniTroskovi;



                    // TROŠKOVI

                    dokument.Add(
                        new Paragraph("Troškovi")
                        .SetFontSize(16));

                    Table tablicaTroskovi =
                        new Table(4, true);

                    tablicaTroskovi.AddHeaderCell("Iznos");
                    tablicaTroskovi.AddHeaderCell("Kategorija");
                    tablicaTroskovi.AddHeaderCell("Datum");
                    tablicaTroskovi.AddHeaderCell("Opis");

                    foreach (var trosak in troskovi)
                    {
                        tablicaTroskovi.AddCell(
                            trosak.Iznos.ToString());

                        tablicaTroskovi.AddCell(
                            trosak.Kategorija);

                        tablicaTroskovi.AddCell(
                            trosak.Datum.ToShortDateString());

                        tablicaTroskovi.AddCell(
                            trosak.Opis);
                    }

                    dokument.Add(tablicaTroskovi);

                    dokument.Add(new Paragraph(" "));


                    // PRIHODI

                    dokument.Add(
                        new Paragraph("Prihodi")
                        .SetFontSize(16));

                    Table tablicaPrihodi =
                        new Table(4, true);

                    tablicaPrihodi.AddHeaderCell("Iznos");
                    tablicaPrihodi.AddHeaderCell("Kategorija");
                    tablicaPrihodi.AddHeaderCell("Datum");
                    tablicaPrihodi.AddHeaderCell("Opis");

                    foreach (var prihod in prihodi)
                    {
                        tablicaPrihodi.AddCell(
                            prihod.Iznos.ToString());

                        tablicaPrihodi.AddCell(
                            prihod.Kategorija);

                        tablicaPrihodi.AddCell(
                            prihod.Datum.ToShortDateString());

                        tablicaPrihodi.AddCell(
                            prihod.Opis);
                    }

                    dokument.Add(tablicaPrihodi);

                    dokument.Add(new Paragraph(" "));


                    // STANJE

                    dokument.Add(
                        new Paragraph(
                            $"Ukupni prihodi: {ukupniPrihodi} €"));

                    dokument.Add(
                        new Paragraph(
                            $"Ukupni troškovi: {ukupniTroskovi} €"));

                    dokument.Add(
                        new Paragraph(
                            $"Stanje računa: {stanje} €")
                        .SetFontSize(16));

                    dokument.Close();
                }

                Podaci = memoryStream.ToArray();
            }
        }
    }
}