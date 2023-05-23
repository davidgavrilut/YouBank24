namespace YouBank24.Repository.IRepository; 
public interface IUnitOfWork {
    IAccountRepository Account { get; }
    IApplicationUserRepository ApplicationUser { get; }
    ITransactionRepository Transaction { get; }
    void Save();
}
