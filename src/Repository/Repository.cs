using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YouBank24.Data;
using YouBank24.Models;
using YouBank24.Repository.IRepository;

namespace YouBank24.Repository; 
public class Repository<T> : IRepository<T> where T : class {
    private readonly ApplicationDbContext _db;
    public Repository(ApplicationDbContext db) {
        _db = db;
    }

    public void Add(T entity) {
        _db.Set<T>().Add(entity);
    }
}
