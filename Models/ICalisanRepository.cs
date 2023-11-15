namespace CagriMerkezi2.Models
{
    public interface ICalisanRepository : IRepository<Calisan> 
    {

        void Guncelle(Calisan calisan);

        void Kaydet();
    }
}
