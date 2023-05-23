using YouBank24.Data;
using YouBank24.Models;
using YouBank24.Repository.IRepository;

namespace YouBank24.Repository {
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository {
        private ApplicationDbContext _db;
        public TransactionRepository(ApplicationDbContext db) : base(db) {
            _db = db;
        }
    }
}
