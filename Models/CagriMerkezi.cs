using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CagriMerkezi2.Models
{
    public class CagriMerkezi
    {

        [Key]
        public int Id { get; set; }

        
        public string Ad { get; set; }

        
        public string Soyad { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "Geçerli bir TC Kimlik Numarası giriniz.")]
        public string TC { get; set; }

        
        public string Adres { get; set; }

        
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string TelNo { get; set; }

        public string Aciklama { get; set; }

        
        public string? ResimUrl { get; set; }

        public int? BirimId { get; set; }
        public Birim? Birim { get; set; }

        public int? DepId { get; set; }
        public Departman? Departman { get; set; }

        public int? DurumId { get; set; }
        public SikayetDurum? SikayetDurum { get; set; }

    }
}
