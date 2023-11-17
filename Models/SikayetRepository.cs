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

        public List<Sikayet> GetFilteredSikayetler(int birimId)
        {
            // Veritabanından ilgili birime ait sikayetleri çek
            var filteredSikayetler = _uygulamaDbContext.Sikayetler
                .Where(s => s.BirimId == birimId)
                .ToList();

            // Filtreleme işlemleri buraya eklenebilir
            // Örneğin, başka bir koşula göre filtreleme yapılabilir

            // Sonuçları döndür
            return filteredSikayetler;
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
