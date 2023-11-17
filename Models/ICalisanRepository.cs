namespace CagriMerkezi2.Models
{
    public interface ICalisanRepository : IRepository<Calisan> 
    {
        List<Calisan> GetFilteredCalisanlar(int birimId);

        void Guncelle(Calisan calisan);

        void Kaydet();
    }
}
