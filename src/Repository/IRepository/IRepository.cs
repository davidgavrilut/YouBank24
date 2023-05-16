using System.Linq.Expressions;

namespace YouBank24.Repository.IRepository;
public interface IRepository<T> where T : class {
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    T GetFirstOrDefault(Expression<Func<T, bool>> expression);

}
