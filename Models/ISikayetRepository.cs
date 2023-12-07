
namespace CagriMerkezi2.Models
{
    public interface ISikayetRepository : IRepository<Sikayet>
    {
        // filtreleme işleminde ihtimallere göre
        List<Sikayet> GetFilteredSikayetlerByBirimAndDurum(int birimId, int durumId);

        // birime göre filtreleme işlemi
        List<Sikayet> GetFilteredBirim(int birimId);

        // duruma göre filtreleme işlemi
        List<Sikayet> GetFilteredDurum(int durumId);
        void Guncelle(Sikayet sikayet);

        void Kaydet();

        // başvuru kodu için
        Sikayet GetByBasvuruKodu(string kod);

        
    }
}
