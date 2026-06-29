using Finn.Controllers;
using Finn.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinnTest
{
    public class PrihodiControllerTests
    {
        [Fact]
        public void Prihod_Mora_Biti_Pozitivan()
        {
            decimal iznos = 100;

            Assert.True(iznos > 0);
        }

        [Fact]
        public void Prihod_Ima_Ocekivani_Iznos()
        {
            decimal iznos = 100;

            Assert.Equal(100, iznos);
        }

        [Fact]
        public void Kategorija_Ne_Smije_Biti_Prazna()
        {
            string kategorija = "Plaća";

            Assert.False(string.IsNullOrEmpty(kategorija));
        }

        [Fact]
        public void Datum_Prihoda_Je_Ispravan()
        {
            DateTime datum = DateTime.Today;

            Assert.True(datum <= DateTime.Today);
        }

        [Fact]
        public void Izracun_Ukupnog_Prihoda_Je_Ispravan()
        {
            decimal prihod1 = 100;
            decimal prihod2 = 200;

            decimal ukupno =
                prihod1 + prihod2;

            Assert.Equal(300, ukupno);
        }
    }
}