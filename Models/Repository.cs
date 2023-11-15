using CagriMerkezi2.Utility;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CagriMerkezi2.Models
{
    public class Repository<T> : IRepository<T> where T : class   // : implement anlamında
    {

        private readonly UygulamaDbContext _uygulamaDbContext;
        internal DbSet<T> dbSet;  // dbSet = _uygulamaDbContext.Birimler gibidir

        public Repository(UygulamaDbContext uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
            this.dbSet = _uygulamaDbContext.Set<T>();
            //_uygulamaDbContext.Departmanlar.Include(k => k.Birim).Include(k => k.BirimId);
            //_uygulamaDbContext.Sikayetler.Include(s => s.Departman).Include(s => s.DepId);
            // Include yapısı foreign key yapısı için çok kullanışlı
        }
        
        public void Ekle(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filtre, string? includeProps = null)  // veritabanından data getirme çekme
        {
            IQueryable<T> sorgu = dbSet;
            sorgu = sorgu.Where(filtre);

            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var includeProp in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    //foreignkey'leri çekme işlemi
                    sorgu = sorgu.Include(includeProp);
                   
                }
            }

            return sorgu.FirstOrDefault();  // sadece bir tane döndürmesi gerekiyor bu fonksiyonun
        }

        public IEnumerable<T> GetAll(string? includeProps = null)
        {
            IQueryable<T> sorgu = dbSet;

            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var includeProp in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    //foreignkey'leri çekme işlemi
                    sorgu = sorgu.Include(includeProp);
                }
            }

            return sorgu.ToList();
        }

        public void Sil(T entity)
        {
            dbSet.Remove(entity);  // tek bir kaydı siler
        }

        public void SilAralik(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
