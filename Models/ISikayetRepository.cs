namespace CagriMerkezi2.Models
{
    public interface ISikayetRepository : IRepository<Sikayet>
    {
        
        void Guncelle(Sikayet sikayet);

        void Kaydet();
    }
}
