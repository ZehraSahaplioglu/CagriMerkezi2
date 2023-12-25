using System.ComponentModel.DataAnnotations;

namespace CagriMerkezi2.Models
{
    public class Kullanici
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

        [Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Telefon numaranızın son 4 hanesini giriniz.")]
        public string Sifre { get; set; }

        [Required]
        public string Yetki {  get; set; }

        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; }
    }
}
