
namespace CagriMerkezi2.Models
{
    public interface ISikayetRepository : IRepository<Sikayet>
    {
        // birime göre filtreleme işlemi
        List<Sikayet> GetFilteredSikayetler(int birimId);

        // duruma göre filtreleme işlemi
        List<Sikayet> GetFilteredDurum(int durumId);
        void Guncelle(Sikayet sikayet);

        void Kaydet();

        // başvuru kodu için
        Sikayet GetByBasvuruKodu(string kod);
    }
}
