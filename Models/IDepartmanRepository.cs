using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CagriMerkezi2.Models
{
    public interface IDepartmanRepository : IRepository<Departman>
    {
        IEnumerable<Departman> GetDepartmentsByBirimId(int birimId);
        void Guncelle(Departman departman);

        void Kaydet();
    }
}
