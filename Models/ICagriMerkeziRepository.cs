namespace CagriMerkezi2.Models
{
    public interface ICagriMerkeziRepository : IRepository<CagriMerkezi>
    {
        void Detach(CagriMerkezi cagriMerkezi);
        void Guncelle(CagriMerkezi cagriMerkezi);

        void Kaydet();
    }
}
