using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finn.Models
{
    public class Budzet
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv budžeta je obavezan")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Iznos je obavezan")]
        [Range(1, 100000)]
        public decimal Iznos { get; set; }

        [Required(ErrorMessage = "Datum početka je obavezan")]
        public DateTime DatumOd { get; set; }

        [Required(ErrorMessage = "Datum završetka je obavezan")]
        public DateTime DatumDo { get; set; }

        public string? UserId { get; set; }

        [NotMapped]
        public decimal Potroseno { get; set; }

        [NotMapped]
        public decimal Preostalo { get; set; }
    }
}