using YouBank24.Models;

namespace YouBank24.Repository.IRepository {
    public interface ITransactionRepository : IRepository<Transaction> {
        Transaction GetTransactionById(string transactionId);
        IEnumerable<Transaction> GetAllTransactionsByAccountId(string accountId);
        IEnumerable<Transaction> GetAllTransactionsByReceiverUserId(string receiverUserId);
    }
}
