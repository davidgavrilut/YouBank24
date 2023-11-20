namespace YouBank24.Repository.IRepository;
public interface IRepository<T> where T : class {
    void Add(T entity);
}
