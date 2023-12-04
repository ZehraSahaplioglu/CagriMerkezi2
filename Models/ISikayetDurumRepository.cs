namespace CagriMerkezi2.Models
{
    public interface ISikayetDurumRepository : IRepository<SikayetDurum>
    {
        void Guncelle(SikayetDurum sikayetDurum);

        void Kaydet();
    }
}
