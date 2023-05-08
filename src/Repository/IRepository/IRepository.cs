using System.Linq.Expressions;

namespace YouBank24.Repository.IRepository;
public interface IRepository<T> where T : class {
    T GetFirstOrDefault(Expression<Func<T, bool>> expression);
}
