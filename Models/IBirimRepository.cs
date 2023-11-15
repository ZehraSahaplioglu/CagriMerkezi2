namespace CagriMerkezi2.Models
{
    public interface IBirimRepository : IRepository<Birim>
    {
        void Guncelle(Birim birim);
        void Kaydet();

    }
}
