using CagriMerkezi2.Utility;

namespace CagriMerkezi2.Models
{
    public class BirimRepository : Repository<Birim>, IBirimRepository
    {
        private UygulamaDbContext _uygulamaDbContext;

        public BirimRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }

        public void Guncelle(Birim birim)
        {
            _uygulamaDbContext.Update(birim);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
