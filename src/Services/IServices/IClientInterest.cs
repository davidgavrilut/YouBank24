using YouBank24.Models.ViewModels;

namespace YouBank24.Services.IServices
{
    public interface IClientInterest
    {
        Task<InterestRate> GetInterestRateAsync(string country);
    }
}
