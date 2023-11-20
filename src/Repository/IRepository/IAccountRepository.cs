using YouBank24.Models;

namespace YouBank24.Repository.IRepository; 
public interface IAccountRepository : IRepository<Account> {
    Account GetAccountById(string accountId);
    Account GetAccountByUserId(string userId);
    void Update(string applicationUserId, float amount);
    string GenerateCardNumber();
    string GenerateCVV();
    string GenerateIBAN();
    string GenerateExpirationDate();
}
