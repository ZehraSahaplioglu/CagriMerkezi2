
namespace CagriMerkezi2.Models
{
    public interface ISikayetRepository : IRepository<Sikayet>
    {
        List<Sikayet> GetFilteredSikayetler(int birimId);
        void Guncelle(Sikayet sikayet);

        void Kaydet();
    }
}
