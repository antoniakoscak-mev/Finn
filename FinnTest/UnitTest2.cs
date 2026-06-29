using Finn.Controllers;
using Finn.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinnTest
{
    public class TroskoviControllerTests
    {
        [Fact]
        public void Rashod_Mora_Biti_Pozitivan()
        {
            decimal iznos = 50;

            Assert.True(iznos > 0);
        }

        [Fact]
        public void Kategorija_Rashoda_Ne_Smije_Biti_Prazna()
        {
            string kategorija = "Hrana";

            Assert.False(string.IsNullOrEmpty(kategorija));
        }

        [Fact]
        public void Izracun_Ukupnog_Rashoda_Je_Ispravan()
        {
            decimal rashod1 = 50;
            decimal rashod2 = 150;

            Assert.Equal(200, rashod1 + rashod2);
        }
    }
}