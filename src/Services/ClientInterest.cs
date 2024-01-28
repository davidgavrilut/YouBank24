using System.Net.Http;
using System.Text.Json;
using YouBank24.Models;
using YouBank24.Models.ViewModels;
using YouBank24.Services.IServices;

namespace YouBank24.Services
{
    public class ClientInterest : IClientInterest
    {
        private readonly HttpClient _httpClient;
        private readonly ClientInterestConnectionOptions _clientInterestConnection;

        public ClientInterest(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _clientInterestConnection = configuration.GetSection("APIConnections").GetSection("ClientInterest").Get<ClientInterestConnectionOptions>();
        }
        public async Task<InterestRate> GetInterestRateAsync(string country)
        {
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_clientInterestConnection.Uri),
                Headers =
                {
                    { "X-RapidAPI-Key", _clientInterestConnection.X_RapidAPI_Key },
                    { "X-RapidAPI-Host", _clientInterestConnection.X_RapidAPI_Host },
                },
            };
            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var interestRateData = await JsonSerializer.DeserializeAsync<InterestRateData>(stream, options);
            var filteredRatesData = interestRateData?.central_bank_rates
                .Where(r => r.country == country)
                .FirstOrDefault();
            if (filteredRatesData != null)
            {
                return new InterestRate
                {
                    central_bank = filteredRatesData.central_bank,
                    country = filteredRatesData.country,
                    rate_pct = filteredRatesData.rate_pct,
                    last_updated = filteredRatesData.last_updated
                };
            }
            throw new BadHttpRequestException("No data was found for the specified country");
        }
    }
}
