using YouBank24.Models;

namespace YouBank24.Repository.IRepository; 
public interface IAccountRepository : IRepository<Account> {
    void Add(Account account);
    void Remove(Account account);
    void Update(Account account);
    string GenerateCardNumber();
    string GenerateCVV();
    string GenerateIBAN();
    string GenerateExpirationDate();
}
