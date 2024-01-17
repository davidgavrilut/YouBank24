using YouBank24.Data;
using YouBank24.Models;
using YouBank24.Repository.IRepository;

namespace YouBank24.Repository; 
public class AccountRepository : Repository<Account>, IAccountRepository {
    private ApplicationDbContext _db;
    private readonly Random rand;
    public AccountRepository(ApplicationDbContext db) : base(db) {
        _db = db;
        rand = new Random();
    }

    public Account GetAccountById(string accountId)
    {
        var account = _db.Accounts.FirstOrDefault(a => a.AccountId == accountId);
        if (account == null)
        {
            throw new ArgumentNullException(nameof(accountId));
        }
        return account;
    }

    public Account GetAccountByUserId(string userId)
    {
        var account = _db.Accounts.FirstOrDefault(a => a.ApplicationUserId == userId);
        if (account == null) {
            throw new ArgumentNullException(nameof(userId));
        }  
        return account; 
    }

    public void UpdateBalance(string applicationUserId, float amount) {
        var objFromDb = _db.Accounts.FirstOrDefault(u => u.ApplicationUserId == applicationUserId);
        if (objFromDb == null) {
            throw new NullReferenceException(nameof(objFromDb));         
        }
        objFromDb.Balance += amount;
    }

    public string GenerateCardNumber() {
        string cardNumber = "";


        for (int i = 0; i < 15; i++) {
            cardNumber += rand.Next(10).ToString();
        }

        int sum = 0;
        for (int i = 0; i < 15; i++) {
            int digit = int.Parse(cardNumber[i].ToString());
            if (i % 2 == 0) {
                digit *= 2;
                if (digit > 9) digit -= 9;
            }
            sum += digit;
        }
        int lastDigit = (10 - (sum % 10)) % 10;

        cardNumber += lastDigit;
        return cardNumber;
    }

    public string GenerateCVV() {
        string cvv = "";

        for (int i = 0; i < 3; i++) {
            cvv += rand.Next(10).ToString();
        }
        return cvv;
    }

    public string GenerateIBAN() {
        string iban = "AA52YBNK";
        for (int i = 0; i < 2; i++) {
            char letter = (char)rand.Next('A', 'Z' + 1);
            iban += letter;
        }

        for (int i = 0; i < 14; i++) {
            iban += rand.Next(10).ToString();
        }
        return iban;
    }

    public string GenerateExpirationDate() {
        int year = DateTime.Now.Year % 100;

        year += rand.Next(3, 6);             
        if (year > 99) {
            year -= 100;
        }

        int month = rand.Next(1, 13);
        if (year == DateTime.Now.Year % 100 && month < DateTime.Now.Month) {
            year += 1;
        }

        string monthStr = month.ToString().PadLeft(2, '0');
        string yearStr = year.ToString().PadLeft(2, '0');

        return $"{monthStr}/{yearStr}";
    }
}
