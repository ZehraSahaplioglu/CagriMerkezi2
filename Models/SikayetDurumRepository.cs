using CagriMerkezi2.Utility;

namespace CagriMerkezi2.Models
{
    public class SikayetDurumRepository : Repository<SikayetDurum>, ISikayetDurumRepository
    {
        private UygulamaDbContext _uygulamaDbContext;

        public SikayetDurumRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }

        public void Guncelle(SikayetDurum sikayetDurum)
        {
            var existingEntity = _uygulamaDbContext.SikayetDurums.Find(sikayetDurum.Id);

            if (existingEntity != null)
            {
                // Eğer varlık zaten takip ediliyorsa Attach veya Update yapabilirsiniz.
                _uygulamaDbContext.Entry(existingEntity).CurrentValues.SetValues(sikayetDurum);
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
