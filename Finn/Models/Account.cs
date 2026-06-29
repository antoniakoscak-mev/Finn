using Microsoft.AspNetCore.Identity;

namespace Finn.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string Naziv { get; set; } = "";

        public string Tip { get; set; } = "";

        public decimal PocetnoStanje { get; set; }

        public string UserId { get; set; } = "";

        public IdentityUser? User { get; set; }
    }
}