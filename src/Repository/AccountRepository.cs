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

        // Generate first 15 digits randomly
        for (int i = 0; i < 15; i++) {
            cardNumber += rand.Next(10).ToString();
        }
        // Use Luhn algorithm to calculate the last digit
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
        // Add last digit to card number
        cardNumber += lastDigit;
        return cardNumber;
    }

    public string GenerateCVV() {
        string cvv = "";
        // Generate 3 random digits
        for (int i = 0; i < 3; i++) {
            cvv += rand.Next(10).ToString();
        }
        return cvv;
    }

    public string GenerateIBAN() {
        string iban = "AA52YBNK";
        // Generate 2 random capital letters
        for (int i = 0; i < 2; i++) {
            char letter = (char)rand.Next('A', 'Z' + 1);
            iban += letter;
        }
        // Generate 14 random digits
        for (int i = 0; i < 14; i++) {
            iban += rand.Next(10).ToString();
        }
        return iban;
    }

    public string GenerateExpirationDate() {
        // Get the last 2 digits of the current year
        int year = DateTime.Now.Year % 100;
        // Add a random value between 3 and 5 to get a future year
        year += rand.Next(3, 6);             
        if (year > 99) {
            year -= 100;
        }
        // Generate a future month between 1 and 12
        int month = rand.Next(1, 13);
        if (year == DateTime.Now.Year % 100 && month < DateTime.Now.Month) {
            // If the generated month is in the past of the current year, add 1 year
            year += 1;
        }
        // Format the expiration date string as "MM/YY"
        string monthStr = month.ToString().PadLeft(2, '0');
        string yearStr = year.ToString().PadLeft(2, '0');

        return $"{monthStr}/{yearStr}";
    }
}
