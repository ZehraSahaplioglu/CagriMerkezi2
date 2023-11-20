using CagriMerkezi2.Utility;
using Microsoft.EntityFrameworkCore;

namespace CagriMerkezi2.Models
{
    public class DepartmanRepository : Repository<Departman>, IDepartmanRepository
    {

        private UygulamaDbContext _uygulamaDbContext;

        public DepartmanRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }

        // Calisan ve Sikayet index sayfaları için filtreleme
        public IEnumerable<Departman> GetDepartmentsByBirimId(int birimId)
        {
            return _uygulamaDbContext.Departmanlar
                .Where(d => d.BirimId == birimId)
                .ToList();
        }

        // Departman index sayfası için filtreleme
        public List<Departman> GetFilteredDep(int birimId)
        {
            var departmanlar = _uygulamaDbContext.Departmanlar.Include(d => d.Birim).ToList();

            // Veritabanından ilgili birime ait sikayetleri çek
            var filteredDep = _uygulamaDbContext.Departmanlar
                .Where(s => s.BirimId == birimId)
                .ToList();

            // Sonuçları döndür
            return filteredDep;
        }

        public void Guncelle(Departman departman)
        {
            _uygulamaDbContext.Update(departman);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
