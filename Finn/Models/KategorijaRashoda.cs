using System.ComponentModel.DataAnnotations;

namespace Finn.Models
{
    public class KategorijaRashoda
    {
        public int Id { get; set; }

        [Required]
        public string Naziv { get; set; }

        public string? Boja { get; set; }

        public bool Zadana { get; set; }
    }
}