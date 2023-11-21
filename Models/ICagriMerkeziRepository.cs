namespace CagriMerkezi2.Models
{
    public interface ICagriMerkeziRepository : IRepository<CagriMerkezi>
    {
        void Guncelle(CagriMerkezi cagriMerkezi);

        void Kaydet();
    }
}
