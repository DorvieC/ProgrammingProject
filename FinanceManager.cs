using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System; // Додано для Console

namespace PersonalFinanceTracker
{
    // Допоміжний клас для серіалізації/десеріалізації всіх даних програми
    public class AppData
    {
        public List<Wallet> Wallets { get; set; } = new List<Wallet>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<Currency> Currencies { get; set; } = new List<Currency>();
        public List<IncomeSource> IncomeSources { get; set; } = new List<IncomeSource>();
        public List<ExpenseCategory> ExpenseCategories { get; set; } = new List<ExpenseCategory>();
    }

    public class FinanceManager
    {
        private const string DataFileName = "finance_data.json";

        public List<Wallet> Wallets { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        public List<Currency> Currencies { get; private set; }
        public List<IncomeSource> IncomeSources { get; private set; }
        public List<ExpenseCategory> ExpenseCategories { get; private set; }
        // public CurrencyConverter Converter { get; private set; }

        public FinanceManager()
        {
            // Ініціалізуємо порожні списки спочатку
            Wallets = new List<Wallet>();
            Transactions = new List<Transaction>();
            Currencies = new List<Currency>();
            IncomeSources = new List<IncomeSource>();
            ExpenseCategories = new List<ExpenseCategory>();
            // Converter = new CurrencyConverter(); // Якщо він також потребує збереження/завантаження

            if (!LoadData()) // Якщо завантаження не вдалося або файлу даних не існувало
            {
                // Ініціалізуємо валюти за замовчуванням, якщо дані не завантажено
                InitializeDefaultCurrencies();
                SaveData(); // Зберігаємо початковий стан з валютами за замовчуванням
            }
        }

        private void InitializeDefaultCurrencies()
        {
            // Перевірка, чи валюти вже існують, щоб уникнути дублікатів, якщо цей метод викликається в інших сценаріях
            if (!Currencies.Exists(c => c.Code == "USD"))
                Currencies.Add(new Currency("USD", "Долар США"));
            if (!Currencies.Exists(c => c.Code == "EUR"))
                Currencies.Add(new Currency("EUR", "Євро"));
            if (!Currencies.Exists(c => c.Code == "UAH"))
                Currencies.Add(new Currency("UAH", "Українська гривня"));
        }

        private bool LoadData()
        {
            if (File.Exists(DataFileName))
            {
                try
                {
                    string jsonData = File.ReadAllText(DataFileName);
                    AppData? data = JsonSerializer.Deserialize<AppData>(jsonData);
                    if (data != null)
                    {
                        Wallets = data.Wallets ?? new List<Wallet>();
                        Transactions = data.Transactions ?? new List<Transaction>();
                        Currencies = data.Currencies ?? new List<Currency>();
                        IncomeSources = data.IncomeSources ?? new List<IncomeSource>();
                        ExpenseCategories = data.ExpenseCategories ?? new List<ExpenseCategory>();
                        // Console.WriteLine("Дані успішно завантажено з " + Path.GetFullPath(DataFileName)); // Для відладки
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка завантаження даних: {ex.Message}. Буде створено новий файл даних.");
                    // Можна додати логіку для створення резервної копії пошкодженого файлу
                }
            }
            // Console.WriteLine("Файл даних не знайдено. Буде створено новий."); // Для відладки
            return false;
        }

        public void SaveData()
        {
            try
            {
                AppData data = new AppData
                {
                    Wallets = this.Wallets,
                    Transactions = this.Transactions,
                    Currencies = this.Currencies,
                    IncomeSources = this.IncomeSources,
                    ExpenseCategories = this.ExpenseCategories
                };
                // Використовуємо JsonSerializerOptions для гарного форматування JSON
                string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(DataFileName, jsonData);
                // Console.WriteLine("Дані успішно збережено в " + Path.GetFullPath(DataFileName)); // Для відладки
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження даних: {ex.Message}");
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            return new List<Transaction>(Transactions); // Повертаємо копію, щоб запобігти зовнішній модифікації внутрішнього списку
        }

        // Майбутні методи, що модифікують дані, повинні викликати SaveData()
        // Приклад:
        // public void AddWallet(Wallet wallet)
        // {
        //     Wallets.Add(wallet);
        //     SaveData();
        // }

        // Існуючі методи з інструкції (потрібно буде додати сюди їх реалізації та виклики SaveData())
        // Наприклад, методи, що Ярик та Захар будуть реалізовувати:
        // AddWallet, RemoveWallet, AddIncomeSource, RemoveIncomeSource, AddExpenseCategory, RemoveExpenseCategory, AddTransaction

        // Тут будуть методи для:
        // - Додавання/видалення гаманців (потребують SaveData())
        // - Додавання/видалення джерел доходу/категорій витрат (потребують SaveData())
        // - Реєстрації доходів/витрат (потребують SaveData())
        // - Отримання балансів
        // - Генерації звітів/графіків (текстових)
        // - Обміну валют (потребують SaveData(), якщо змінює баланси)

        public bool ExchangeCurrency(string fromWalletName, string toWalletName, decimal amountToConvert, decimal exchangeRate)
        {
            Wallet fromWallet = Wallets.Find(w => w.Name.Equals(fromWalletName, StringComparison.OrdinalIgnoreCase));
            Wallet toWallet = Wallets.Find(w => w.Name.Equals(toWalletName, StringComparison.OrdinalIgnoreCase));

            if (fromWallet == null)
            {
                Console.WriteLine($"Помилка: Гаманець '{fromWalletName}' не знайдено.");
                return false;
            }
            if (toWallet == null)
            {
                Console.WriteLine($"Помилка: Гаманець '{toWalletName}' не знайдено.");
                return false;
            }

            if (fromWallet.WalletCurrency.Code == toWallet.WalletCurrency.Code)
            {
                Console.WriteLine("Помилка: Гаманці мають однакову валюту. Для переказу між такими гаманцями використовуйте іншу функцію (ще не реалізовано).");
                return false;
            }

            if (amountToConvert <= 0)
            {
                Console.WriteLine("Помилка: Сума для конвертації має бути позитивною.");
                return false;
            }

            if (fromWallet.Balance < amountToConvert)
            {
                Console.WriteLine($"Помилка: Недостатньо коштів у гаманці '{fromWallet.Name}'. Баланс: {fromWallet.Balance} {fromWallet.WalletCurrency.Code}, потрібно: {amountToConvert} {fromWallet.WalletCurrency.Code}.");
                return false;
            }

            decimal convertedAmount;
            try
            {
                convertedAmount = CurrencyConverter.Convert(amountToConvert, exchangeRate);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Помилка конвертації: {ex.ParamName} - {ex.Message}");
                return false;
            }

            if (convertedAmount <= 0) // Додаткова перевірка після конвертації
            {
                Console.WriteLine("Помилка: Конвертована сума не може бути нульовою або від'ємною. Перевірте курс.");
                return false;
            }

            // Створюємо транзакції
            string exchangeDescriptionOut = $"Обмін {amountToConvert} {fromWallet.WalletCurrency.Code} на {convertedAmount:N2} {toWallet.WalletCurrency.Code} (в гаманець {toWallet.Name})";
            Transaction debitTransaction = new Transaction(DateTime.Now, TransactionType.Expense, amountToConvert, fromWallet.WalletCurrency, exchangeDescriptionOut, fromWallet, "Обмін валют");
            Transactions.Add(debitTransaction);
            fromWallet.Balance -= amountToConvert;

            string exchangeDescriptionIn = $"Обмін {amountToConvert} {fromWallet.WalletCurrency.Code} (з гаманця {fromWallet.Name}) на {convertedAmount:N2} {toWallet.WalletCurrency.Code}";
            Transaction creditTransaction = new Transaction(DateTime.Now, TransactionType.Income, convertedAmount, toWallet.WalletCurrency, exchangeDescriptionIn, toWallet, "Обмін валют");
            Transactions.Add(creditTransaction);
            toWallet.Balance += convertedAmount;

            SaveData();
            Console.WriteLine($"Обмін успішно виконано. Списано {amountToConvert} {fromWallet.WalletCurrency.Code} з '{fromWallet.Name}'. Зараховано {convertedAmount:N2} {toWallet.WalletCurrency.Code} до '{toWallet.Name}'.");
            return true;
        }
    }
} 