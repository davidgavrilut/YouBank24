using YouBank24.Data;
using YouBank24.Repository.IRepository;

namespace YouBank24.Repository;
public class UnitOfWork : IUnitOfWork {
    private ApplicationDbContext _db;
    public UnitOfWork(ApplicationDbContext db) {
        _db = db;
        Account = new AccountRepository(_db);
    }

    public IAccountRepository Account { get; private set; }

    public void Save() {
        _db.SaveChanges();
    }
}
