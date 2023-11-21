using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CagriMerkezi2.Models
{
    public class Departman
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Ad { get; set; }


        public int BirimId { get; set; }
        public Birim Birim { get; set; }

        public ICollection<Sikayet> SikayetList { get; set;}

        public ICollection<Calisan> CalisanList { get; set; }

        public ICollection<CagriMerkezi> CagriList { get; set; }
    }
}
