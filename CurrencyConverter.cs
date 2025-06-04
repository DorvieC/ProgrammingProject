namespace PersonalFinanceTracker
{
    public static class CurrencyConverter // Зробимо клас статичним, оскільки він міститиме лише статичні методи
    {
        // Метод для простої конвертації на основі наданого курсу
        // exchangeRate - це скільки одиниць ЦІЛЬОВОЇ валюти ви отримуєте за одну одиницю ВИХІДНОЇ валюти
        public static decimal Convert(decimal amount, decimal exchangeRate)
        {
            if (exchangeRate <= 0)
            {
                // Обробка помилки або повернення вихідної суми, якщо курс некоректний
                // Для простоти, зараз повернемо 0, але в реальному додатку потрібна краща обробка
                // Або можна кидати виняток, який буде оброблено вище
                System.Console.WriteLine("Помилка: Обмінний курс має бути позитивним числом.");
                // Повернемо суму без змін, щоб уникнути несподіваних 0, або потрібно вирішити, як обробляти цю помилку
                // Можливо, краще кинути ArgumentOutOfRangeException
                throw new System.ArgumentOutOfRangeException(nameof(exchangeRate), "Обмінний курс має бути позитивним.");
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