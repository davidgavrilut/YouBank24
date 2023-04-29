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

    public T GetFirstOrDefault(Expression<Func<T, bool>> expression) {
        IQueryable<T> query = dbSet.Where(expression);
        return query.FirstOrDefault();
    }

    public void Add(T entity) {
        dbSet.Add(entity);
    }

    public void Remove(T entity) {
        dbSet.Remove(entity);
    }
}
