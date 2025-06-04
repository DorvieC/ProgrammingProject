using System.Collections.Generic;

namespace PersonalFinanceTracker
{
    public class FinanceManager
    {
        public List<Wallet> Wallets { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        public List<Currency> Currencies { get; private set; }
        public List<IncomeSource> IncomeSources { get; private set; }
        public List<ExpenseCategory> ExpenseCategories { get; private set; }
        // public CurrencyConverter Converter { get; private set; }


        public FinanceManager()
        {
            Wallets = new List<Wallet>();
            Transactions = new List<Transaction>();
            Currencies = new List<Currency>();
            IncomeSources = new List<IncomeSource>();
            ExpenseCategories = new List<ExpenseCategory>();
            // Converter = new CurrencyConverter();

            // TODO: Ініціалізувати початкові дані, якщо потрібно (наприклад, валюти за замовчуванням)
        }

        // Тут будуть методи для:
        // - Додавання/видалення гаманців
        // - Додавання/видалення джерел доходу/категорій витрат
        // - Реєстрації доходів/витрат
        // - Отримання балансів
        // - Генерації звітів/графіків (текстових)
        // - Обміну валют
    }
} 