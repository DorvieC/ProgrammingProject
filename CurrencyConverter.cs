namespace PersonalFinanceTracker
{
    // Зробимо клас статичним, оскільки він міститиме лише статичні методи
    public static class CurrencyConverter
    {
        // Метод для простої конвертації на основі наданого курсу
        // exchangeRate - це скільки одиниць ЦІЛЬОВОЇ валюти ви отримуєте за одну одиницю ВИХІДНОЇ валюти
        public static decimal Convert(decimal amount, decimal exchangeRate)
        {
            if (exchangeRate <= 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(exchangeRate), "Обмінний курс має бути позитивним числом.");
            }
            return amount * exchangeRate;
        }

        // Майбутні методи для отримання курсів з API або іншого джерела могли б бути тут
        // public static decimal GetExchangeRate(string fromCurrencyCode, string toCurrencyCode)
        // {
        //     // ... логіка отримання курсу ...
        //     return 1.0m; // Заглушка
        // }
    }
}