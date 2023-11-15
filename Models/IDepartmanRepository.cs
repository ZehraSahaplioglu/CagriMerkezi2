using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CagriMerkezi2.Models
{
    public interface IDepartmanRepository : IRepository<Departman>
    {
        IEnumerable<Departman> GetDepartmentsByBirim(int value);
        void Guncelle(Departman departman);

        void Kaydet();
    }
}
