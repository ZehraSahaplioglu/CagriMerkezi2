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

        public IEnumerable<Departman> GetDepartmentsByBirim(int value)
        {
            throw new NotImplementedException();
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
