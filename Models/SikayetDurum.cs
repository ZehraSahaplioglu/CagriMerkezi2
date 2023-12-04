using System.ComponentModel.DataAnnotations;

namespace CagriMerkezi2.Models
{
    public class SikayetDurum
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Ad { get; set; }

        public ICollection<Sikayet> SikayetList { get; set; }

        public ICollection<CagriMerkezi> CagriMerkeziList { get; set; }

    }
}
