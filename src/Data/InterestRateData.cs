namespace YouBank24.Data
{
    public class InterestRateData
    {
        public List<InterestRate> central_bank_rates { get; set; }
    }

    public class InterestRate
    {
        public string central_bank { get; set; }
        public string country { get; set; }
        public double rate_pct { get; set; }
        public string last_updated { get; set; }
    }
}
