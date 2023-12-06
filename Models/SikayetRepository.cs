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

        // birime göre filtreleme işlemi
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

        // duruma göre filtreleme işlemi
        public List<Sikayet> GetFilteredDurum(int durumId)
        {
            // Veritabanından ilgili birime ait sikayetleri çek
            var filteredSikayetDrm = _uygulamaDbContext.Sikayetler
                .Where(s => s.DurumId == durumId)
                .ToList();

            // Filtreleme işlemleri buraya eklenebilir
            // Örneğin, başka bir koşula göre filtreleme yapılabilir

            // Sonuçları döndür
            return filteredSikayetDrm;
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

        // başvuru kodu için
        public Sikayet GetByBasvuruKodu(string kod)
        {
            // Başvuru koduyla eşleşen şikayeti bul
            var sikayet = _uygulamaDbContext.Sikayetler
                .Include(s => s.SikayetDurum) // İlişkili durumu yüklemek için Include kullanabilirsiniz
                .FirstOrDefault(s => s.BasvuruKodu == kod);

            // Eğer başvuru koduyla eşleşen şikayet bulunamazsa null dönebilirsiniz.
            return sikayet;
        }
    }
}
