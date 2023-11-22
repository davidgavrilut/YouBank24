using System.Security.Claims;
using YouBank24.Models;
using YouBank24.Models.ViewModels;

namespace YouBank24.Services.IServices
{
    public interface ITransactionService
    {
        NewTransactionCreatedModel CreateTransaction(Transaction transaction, string email, Claim claim);
    } 
}
