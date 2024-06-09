using ActivityFinder.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace ActivityFinder.Server.Models
{
    public class GenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context) 
        { 
            _context = context;
        }

        public virtual T? FindByKey(object key)
        {
            return _context.Set<T>().Find(key);
        }

        public virtual void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public virtual IQueryable<T> GetAll() 
        {
            return _context.Set<T>();
                //.AsNoTracking();
        }
    }
}
