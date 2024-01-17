using YouBank24.Data;
using YouBank24.Models;
using YouBank24.Repository.IRepository;

namespace YouBank24.Repository {
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository {
        private ApplicationDbContext _db;
        public TransactionRepository(ApplicationDbContext db) : base(db) {
            _db = db;
        }

        public Transaction GetTransactionById(string transactionId)
        {
            var transaction = _db.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transactionId));
            }
            return transaction;
        }

        public IEnumerable<Transaction> GetAllTransactionsByAccountId(string accountId)
        {
            if (accountId == null) {
                throw new ArgumentNullException(nameof(accountId));
            }
            IEnumerable<Transaction> transactions = _db.Transactions.Where(t => t.AccountId == accountId);
            return transactions.ToList();
        }
        public IEnumerable<Transaction> GetAllTransactionsByReceiverUserId(string receiverUserId)
        {
            if (receiverUserId == null)
            {
                throw new ArgumentNullException(nameof(receiverUserId));
            }
            IEnumerable<Transaction> transactions = _db.Transactions.Where(t =>  t.ReceiverUserId == receiverUserId);
            return transactions.ToList();
        }

    }
}
