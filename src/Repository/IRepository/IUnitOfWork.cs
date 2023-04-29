namespace YouBank24.Repository.IRepository; 
public interface IUnitOfWork {
    IAccountRepository Account { get; }
    void Save();
}
