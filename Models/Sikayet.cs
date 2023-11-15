using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CagriMerkezi2.Models
{
    public class Sikayet
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Ad { get; set; }

        [Required]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "TC Kimlik Numarası boş bırakılamaz.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Geçerli bir TC Kimlik Numarası giriniz.")]
        public string TC { get; set; }

        [Required]
        public string Adres { get; set; }

        [Required(ErrorMessage = "Telefon numarası boş bırakılamaz.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string TelNo { get; set; }

        [Required]
        public string Aciklama { get; set; }

        [ValidateNever]
        public string ResimUrl { get; set; }

        public int BirimId { get; set; }
        public Birim Birim { get; set; }

        public int DepId { get; set; }
        public Departman Departman { get; set; }

    }
}
