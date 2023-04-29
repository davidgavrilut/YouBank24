using YouBank24.Models;

namespace YouBank24.Repository.IRepository; 
public interface IAccountRepository : IRepository<Account> {
    void Update(Account account);
}
