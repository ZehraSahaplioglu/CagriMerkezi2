namespace CagriMerkezi2.Models
{
    public interface IKullaniciRepository : IRepository<Kullanici>
    {
        void Guncelle(Kullanici kullanici);

        void Kaydet();
    }
}
