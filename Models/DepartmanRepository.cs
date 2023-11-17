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
            return _uygulamaDbContext.Departmanlar
            .Where(d => d.BirimId == birimId)
            .ToList();
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
