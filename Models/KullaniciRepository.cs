using CagriMerkezi2.Utility;

namespace CagriMerkezi2.Models
{
    public class KullaniciRepository : Repository<Kullanici>, IKullaniciRepository
    {
        private UygulamaDbContext _uygulamaDbContext;

        public KullaniciRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }

        public void Guncelle(Kullanici kullanici)
        {
            var existingEntity = _uygulamaDbContext.Kullanicilar.Find(kullanici.Id);

            if (existingEntity != null)
            {
                // Eğer varlık zaten takip ediliyorsa Attach veya Update yapabilirsiniz.
                _uygulamaDbContext.Entry(existingEntity).CurrentValues.SetValues(kullanici);
                // veya
                _uygulamaDbContext.Update(existingEntity);
            }

        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
