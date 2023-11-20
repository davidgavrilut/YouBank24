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
            IEnumerable<Transaction> transactions = _db.Transactions.Where(t => t.AccountId == accountId);
            if (transactions.Count() == 0) {
                throw new ArgumentNullException(nameof(accountId));
            }
            return transactions.ToList();
        }
        public IEnumerable<Transaction> GetAllTransactionsByReceiverUserId(string receiverUserId)
        {
            IEnumerable<Transaction> transactions = _db.Transactions.Where(t =>  t.ReceiverUserId == receiverUserId);
            if (transactions.Count() == 0)
            {
                throw new ArgumentNullException(nameof(receiverUserId));
            }
            return transactions.ToList();
        }

    }
}
