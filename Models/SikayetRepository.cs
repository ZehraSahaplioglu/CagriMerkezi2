using CagriMerkezi2.Utility;
using Microsoft.EntityFrameworkCore;

namespace CagriMerkezi2.Models
{
    public class SikayetRepository : Repository<Sikayet>, ISikayetRepository
    {
        private UygulamaDbContext _uygulamaDbContext;

        public SikayetRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }

        public void Guncelle(Sikayet sikayet)
        {
            var existingEntity = _uygulamaDbContext.Sikayetler.Find(sikayet.Id);

            if (existingEntity != null)
            {
                // Eğer varlık zaten takip ediliyorsa Attach veya Update yapabilirsiniz.
                _uygulamaDbContext.Entry(existingEntity).CurrentValues.SetValues(sikayet);
                // veya
                _uygulamaDbContext.Update(existingEntity);
            }

            //_uygulamaDbContext.Update(sikayet);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
