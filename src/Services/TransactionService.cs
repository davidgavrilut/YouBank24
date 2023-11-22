using System.Security.Claims;
using YouBank24.Models;
using YouBank24.Models.ViewModels;
using YouBank24.Repository.IRepository;
using YouBank24.Services.IServices;

namespace YouBank24.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransactionService(IUnitOfWork unitOfWork) { 
            _unitOfWork = unitOfWork;
        }
        public NewTransactionCreatedModel CreateTransaction(Transaction transaction, string email, Claim claim)
        {
            transaction.TransactionId = Guid.NewGuid().ToString();
            transaction.TransactionType = StaticDetails.TransactionTypeInstantMoney;
            transaction.TransactionTimestamp = DateTime.Now;
            var receiverUser = _unitOfWork.ApplicationUser.GetUserByEmail(email);
            transaction.ReceiverUserId = receiverUser.Id;
            transaction.AccountId = _unitOfWork.Account.GetAccountByUserId(claim.Value).AccountId;
            var senderUserId = _unitOfWork.Account.GetAccountById(transaction.AccountId).ApplicationUserId;
            var senderUser = _unitOfWork.ApplicationUser.GetUserById(senderUserId);
            return new NewTransactionCreatedModel()
            {
                Transaction = transaction,
                ReceiverUser = receiverUser,
                SenderUser = senderUser
            };
        }
    }
}
