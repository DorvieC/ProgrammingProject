namespace PersonalFinanceTracker
{
    public static class CurrencyConverter
    {
        public static decimal Convert(decimal amount, decimal exchangeRate)
        {
            if (exchangeRate <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(exchangeRate), "Обмінний курс має бути позитивним числом.");
            }
            return amount * exchangeRate;
        }
    }
}