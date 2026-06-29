using System.ComponentModel.DataAnnotations;

namespace Finn.Models
{
    public class Prihod
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Iznos je obavezan")]
        [Range(1, 100000, ErrorMessage = "Iznos mora biti veći od 0")]
        public decimal Iznos { get; set; }

        [Required(ErrorMessage = "Kategorija je obavezna")]
        public string? Kategorija { get; set; }

        [Required(ErrorMessage = "Datum je obavezan")]
        public DateTime Datum { get; set; }

        [StringLength(200, ErrorMessage = "Opis može imati maksimalno 200 znakova")]
        public string? Opis { get; set; }

        [Required]
        public string TipPrihoda { get; set; } = "Predviđeni";

        public string? UserId { get; set; }
    }
}