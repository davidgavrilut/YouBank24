using YouBank24.Data;
using YouBank24.Repository.IRepository;

namespace YouBank24.Repository;
public class UnitOfWork : IUnitOfWork {
    private readonly ApplicationDbContext _db;
    public IAccountRepository Account { get; private set; }
    public IApplicationUserRepository ApplicationUser { get; private set; }
    public ITransactionRepository Transaction { get; private set; }
    
    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Account = new AccountRepository(_db);
        ApplicationUser = new ApplicationUserRepository(_db);
        Transaction = new TransactionRepository(_db);
    }

    public void Save() {
        _db.SaveChanges();
    }
}
