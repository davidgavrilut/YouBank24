using YouBank24.Models;

namespace YouBank24.Repository.IRepository; 
public interface IAccountRepository : IRepository<Account> {
    void Update(string applicationUserId, float amount);
    string GenerateCardNumber();
    string GenerateCVV();
    string GenerateIBAN();
    string GenerateExpirationDate();
}
