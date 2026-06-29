using Finn.Models;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.IO.Font;
using iText.Kernel.Font;
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

            

                    // NASLOV
                    Paragraph naslov =
                        new Paragraph("FINN")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(28)
                        .SetBold();

                    dokument.Add(naslov);

                    Paragraph podnaslov =
                        new Paragraph("Financijski izvještaj")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(14)
                        .SetFontColor(ColorConstants.GRAY)
                        .SetMarginBottom(20);

                    dokument.Add(podnaslov);

                    decimal ukupniTroskovi =
                        troskovi.Sum(t => t.Iznos);

                    decimal ukupniPrihodi =
                        prihodi.Sum(p => p.Iznos);

                    decimal stanje =
                        ukupniPrihodi - ukupniTroskovi;

                    //troškovi

                    Paragraph naslovTroskovi =
                        new Paragraph("Troškovi")
                        .SetFontSize(18)
                        .SetBold()
                        .SetMarginBottom(10);

                    dokument.Add(naslovTroskovi);

                    Table tablicaTroskovi =
                        new Table(UnitValue.CreatePercentArray(new float[] { 2, 3, 2, 4 }))
                        .UseAllAvailableWidth();

                    // 

                    tablicaTroskovi.AddHeaderCell(
                        new Cell().Add(new Paragraph("Iznos").SetBold())
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                    tablicaTroskovi.AddHeaderCell(
                        new Cell().Add(new Paragraph("Kategorija").SetBold())
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                    tablicaTroskovi.AddHeaderCell(
                        new Cell().Add(new Paragraph("Datum").SetBold())
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                    tablicaTroskovi.AddHeaderCell(
                        new Cell().Add(new Paragraph("Opis").SetBold())
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                    foreach (var trosak in troskovi)
                    {
                        tablicaTroskovi.AddCell(
                            new Cell().Add(
                                new Paragraph($"{trosak.Iznos:N2} €")));

                        tablicaTroskovi.AddCell(
                            new Cell().Add(
                                new Paragraph(trosak.Kategorija ?? "-")));

                        tablicaTroskovi.AddCell(
                            new Cell().Add(
                                new Paragraph(
                                    trosak.Datum.ToString("dd.MM.yyyy."))));

                        tablicaTroskovi.AddCell(
                            new Cell().Add(
                                new Paragraph(trosak.Opis ?? "-")));
                    }

                    dokument.Add(tablicaTroskovi);

                    dokument.Add(new Paragraph(" "));

                  //prihodi

                    Paragraph naslovPrihodi =
                        new Paragraph("Prihodi")
                        .SetFontSize(18)
                        .SetBold()
                        .SetMarginBottom(10);

                    dokument.Add(naslovPrihodi);

                    Table tablicaPrihodi =
                        new Table(UnitValue.CreatePercentArray(new float[] { 2, 3, 2, 4 }))
                        .UseAllAvailableWidth();

                

                    tablicaPrihodi.AddHeaderCell(
                        new Cell().Add(new Paragraph("Iznos").SetBold())
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                    tablicaPrihodi.AddHeaderCell(
                        new Cell().Add(new Paragraph("Kategorija").SetBold())
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                    tablicaPrihodi.AddHeaderCell(
                        new Cell().Add(new Paragraph("Datum").SetBold())
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                    tablicaPrihodi.AddHeaderCell(
                        new Cell().Add(new Paragraph("Opis").SetBold())
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                    foreach (var prihod in prihodi)
                    {
                        tablicaPrihodi.AddCell(
                            new Cell().Add(
                                new Paragraph($"{prihod.Iznos:N2} €")));

                        tablicaPrihodi.AddCell(
                            new Cell().Add(
                                new Paragraph(prihod.Kategorija ?? "-")));

                        tablicaPrihodi.AddCell(
                            new Cell().Add(
                                new Paragraph(
                                    prihod.Datum.ToString("dd.MM.yyyy."))));

                        tablicaPrihodi.AddCell(
                            new Cell().Add(
                                new Paragraph(prihod.Opis ?? "-")));
                    }

                    dokument.Add(tablicaPrihodi);

                    dokument.Add(new Paragraph(" "));

                    //Sažetak

                    Paragraph sazetakNaslov =
                        new Paragraph("Sažetak")
                        .SetFontSize(18)
                        .SetBold()
                        .SetMarginBottom(10);

                    dokument.Add(sazetakNaslov);

                    Table tablicaStanje =
                        new Table(2)
                        .UseAllAvailableWidth();

                    tablicaStanje.AddCell("Ukupni prihodi");
                    tablicaStanje.AddCell($"{ukupniPrihodi:N2} €");

                    tablicaStanje.AddCell("Ukupni troškovi");
                    tablicaStanje.AddCell($"{ukupniTroskovi:N2} €");

                    tablicaStanje.AddCell(
                        new Cell().Add(
                            new Paragraph("Stanje računa").SetBold()));

                    tablicaStanje.AddCell(
                        new Cell().Add(
                            new Paragraph($"{stanje:0.00} €").SetBold()));

                    dokument.Add(tablicaStanje);

                    dokument.Close();
                }

                Podaci = memoryStream.ToArray();
            }
        }
    }
}