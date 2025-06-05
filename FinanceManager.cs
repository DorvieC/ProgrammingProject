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

        // Додає новий гаманець
        public void AddWallet(string name, string currencyCode, decimal initialBalance)
        {
            // Перевірка, чи існує валюта з таким кодом
            Currency currency = Currencies.Find(c => c.Code.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
            if (currency == null)
            {
                Console.WriteLine($"Помилка: Валюта з кодом '{currencyCode}' не знайдена. Спочатку переконайтеся, що валюта існує, або додайте її (функціонал додавання валют буде пізніше).");
                Console.WriteLine("Доступні валюти на даний момент:");
                foreach (var c in Currencies)
                {
                    Console.WriteLine($"- {c.Code} ({c.Name})");
                }
                return;
            }
            // Перевірка, чи не існує гаманець з такою ж назвою (без урахування регістру)
            if (Wallets.Exists(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Помилка: Гаманець з назвою '{name}' вже існує.");
                return;
            }
            Wallets.Add(new Wallet(name, currency, initialBalance));
            Console.WriteLine($"Гаманець '{name}' ({currency.Code}) успішно додано з балансом {initialBalance} {currency.Code}.");
            SaveData(); // !!! ВАЖЛИВО: Зберегти зміни після додавання гаманця !!!
        }

        // Видаляє гаманець за назвою
        public void RemoveWallet(string name)
        {
            Wallet walletToRemove = Wallets.Find(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (walletToRemove == null)
            {
                Console.WriteLine($"Помилка: Гаманець з назвою '{name}' не знайдено.");
                return;
            }

            // ПОПЕРЕДЖЕННЯ: На даному етапі ми просто видаляємо гаманець.
            // У майбутньому потрібно додати перевірку, чи є транзакції, пов'язані з цим гаманцем.
            // Якщо є, можливо, варто заборонити видалення або запропонувати альтернативу (наприклад, архівувати гаманець).
            // Також, якщо є транзакції, пов'язані з гаманцем, їх теж потрібно видалити або обробити.
            // Поки що для простоти видаляємо лише гаманець.

            // Додаткове підтвердження перед видаленням
            Console.Write($"Ви впевнені, що хочете видалити гаманець '{name}' з балансом {walletToRemove.Balance} {walletToRemove.WalletCurrency.Code}? (так/ні): ");
            string confirmation = Console.ReadLine().Trim().ToLower();

            if (confirmation == "так")
            {
                Wallets.Remove(walletToRemove);
                Console.WriteLine($"Гаманець '{name}' успішно видалено.");
                SaveData(); // !!! ВАЖЛИВО: Зберегти зміни після видалення гаманця !!!
            }
            else
            {
                Console.WriteLine("Видалення гаманця скасовано.");
            }
        }

        // Повертає список усіх гаманців
        public List<Wallet> GetWallets()
        {
            return Wallets; // Повертає посилання на внутрішній список. Для більшої безпеки можна повертати копію: return new List<Wallet>(Wallets);
        }

        // Виводить баланс по кожному гаманцю
        public void DisplayAllWalletsBalances()
        {
            if (Wallets.Count == 0)
            {
                Console.WriteLine("У вас ще немає гаманців.");
                return;
            }
            Console.WriteLine("\n--- Баланс по гаманцях ---");
            foreach (var wallet in Wallets)
            {
                Console.WriteLine($"{wallet.Name}: {wallet.Balance:N2} {wallet.WalletCurrency.Code}"); // Додано форматування N2 для балансу
            }
            Console.WriteLine("--------------------------");
        }
    }
} 