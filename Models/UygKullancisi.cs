using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CagriMerkezi2.Models
{
    public class UygKullancisi : IdentityUser
    {
        [Required]
        public int TC { get; set; }

        public string? AdSoyad { get; set; }
    }
}
