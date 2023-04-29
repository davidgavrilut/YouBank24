using YouBank24.Data;
using YouBank24.Models;
using YouBank24.Repository.IRepository;

namespace YouBank24.Repository; 
public class AccountRepository : Repository<Account>, IAccountRepository {
    private ApplicationDbContext _db;
    public AccountRepository(ApplicationDbContext db) : base(db) {
        _db = db;
    }

    public void Update(Account account) {
        _db.Accounts.Update(account);
    }
}
