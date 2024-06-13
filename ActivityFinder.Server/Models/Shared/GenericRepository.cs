using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    interface IGenericRepository<T>
    {
        IQueryable<T> GetAll();
        T? FindByKey(object key);
        void Add(T entity);
        void SaveChanges();
        IQueryable<T> GetDataForPage(IQueryable<T> query, int pageNumber, int size);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
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

        public IQueryable<T> GetDataForPage(IQueryable<T> query, int pageNumber, int size)
        {
            if (size < 1 || pageNumber < 1)
                throw new ArgumentException("pageNumber and size must be greater than 0");

            return query.Skip((pageNumber - 1) * size).Take(size);
        }
    }
}
