using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YouBank24.Data;
using YouBank24.Repository.IRepository;

namespace YouBank24.Repository; 
public class Repository<T> : IRepository<T> where T : class {
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;
    public Repository(ApplicationDbContext db) {
        _db = db;
        dbSet = db.Set<T>();
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? expression = null, string? includeProperties = null) {
        IQueryable<T> query = dbSet;
        if (expression != null) {
            query = query.Where(expression);
        }
        if (includeProperties != null) {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                query = query.Include(includeProperty);
            }
        }
        return query.ToList();
    }

    public T GetFirstOrDefault(Expression<Func<T, bool>> expression) {
        IQueryable<T> query = dbSet.Where(expression);
        return query.FirstOrDefault() ?? throw new InvalidOperationException("No results found.");
    }
}
