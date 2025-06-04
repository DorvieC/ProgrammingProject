namespace PersonalFinanceTracker
{
    public class Currency
    {
        public string Code { get; set; } // напр., "USD"
        public string Name { get; set; } // напр., "US Dollar"
        // Можна додати обмінний курс відносно базової валюти, якщо потрібно
        // public decimal ExchangeRateToPrimary { get; set; } // Обмінний курс до основної валюти

        public Currency(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
} 