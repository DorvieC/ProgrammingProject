using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

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
        private const string DataFileName = "finance_data.json"; // Назва файлу для збереження даних

        public List<Wallet> Wallets { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        public List<Currency> Currencies { get; private set; }
        public List<IncomeSource> IncomeSources { get; private set; }
        public List<ExpenseCategory> ExpenseCategories { get; private set; }
        // public CurrencyConverter Converter { get; private set; } // Якщо потрібен, можна розкоментувати

        public FinanceManager()
        {
            // Ініціалізуємо порожні списки спочатку
            Wallets = new List<Wallet>();
            Transactions = new List<Transaction>();
            Currencies = new List<Currency>();
            IncomeSources = new List<IncomeSource>();
            ExpenseCategories = new List<ExpenseCategory>();

            if (!LoadData()) // Якщо завантаження не вдалося або файлу даних не існувало
            {
                // Ініціалізуємо валюти за замовчуванням, якщо дані не завантажено
                InitializeDefaultCurrencies();
                SaveData(); // Зберігаємо початковий стан з валютами за замовчуванням
            }
        }

        // --- Методи Яріка для гаманців ---
        public void AddWallet(string name, string currencyCode, decimal initialBalance)
        {
            Currency currency = Currencies.Find(c => c.Code.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
            if (currency == null)
            {
                Console.WriteLine($"Помилка: Валюта з кодом '{currencyCode}' не знайдена. Спочатку переконайтеся, що валюта існує.");
                Console.WriteLine("Доступні валюти на даний момент:");
                foreach (var c in Currencies)
                {
                    Console.WriteLine($"- {c.Code} ({c.Name})");
                }
                return;
            }
            if (Wallets.Exists(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Помилка: Гаманець з назвою '{name}' вже існує.");
                return;
            }
            Wallets.Add(new Wallet(name, currency, initialBalance));
            Console.WriteLine($"Гаманець '{name}' ({currency.Code}) успішно додано з балансом {initialBalance:N2} {currency.Code}.");
            SaveData(); // Важливо: Зберегти зміни
        }

        public void RemoveWallet(string name)
        {
            Wallet walletToRemove = Wallets.Find(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (walletToRemove == null)
            {
                Console.WriteLine($"Помилка: Гаманець з назвою '{name}' не знайдено.");
                return;
            }

            // Додаткове підтвердження перед видаленням
            Console.Write($"Ви впевнені, що хочете видалити гаманець '{name}' з балансом {walletToRemove.Balance:N2} {walletToRemove.WalletCurrency.Code}? (так/ні): ");
            string confirmation = Console.ReadLine().Trim().ToLower();
            if (confirmation == "так")
            {
                Wallets.Remove(walletToRemove);
                Console.WriteLine($"Гаманець '{name}' успішно видалено.");
                SaveData(); // Важливо: Зберегти зміни
            }
            else
            {
                Console.WriteLine("Видалення гаманця скасовано.");
            }
        }

        public List<Wallet> GetWallets()
        {
            return new List<Wallet>(Wallets); // Повертаємо копію для безпеки
        }

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
                Console.WriteLine($"{wallet.Name}: {wallet.Balance:N2} {wallet.WalletCurrency.Code}");
            }
            Console.WriteLine("--------------------------");
        }
        
        // --- Методи Захара: Керування Джерелами Доходів ---
        public bool AddIncomeSource(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Помилка: Назва джерела доходу не може бути порожньою.");
                return false;
            }
            if (IncomeSources.Exists(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Помилка: Джерело доходу з назвою '{name}' вже існує.");
                return false;
            }
            IncomeSources.Add(new IncomeSource(name));
            Console.WriteLine($"Джерело доходу '{name}' успішно додано.");
            SaveData();
            return true;
        }

        public bool RemoveIncomeSource(string name)
        {
            IncomeSource sourceToRemove = IncomeSources.Find(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (sourceToRemove == null)
            {
                Console.WriteLine($"Помилка: Джерело доходу з назвою '{name}' не знайдено.");
                return false;
            }
            // TODO: Розглянути перевірку, чи використовується джерело в транзакціях перед видаленням
            IncomeSources.Remove(sourceToRemove);
            Console.WriteLine($"Джерело доходу '{name}' успішно видалено.");
            SaveData();
            return true;
        }

        public List<IncomeSource> GetIncomeSources()
        {
            return new List<IncomeSource>(IncomeSources);
        }

        public void DisplayIncomeSources()
        {
            if (!IncomeSources.Any())
            {
                Console.WriteLine("Список джерел доходів порожній.");
                return;
            }
            Console.WriteLine("\n--- Джерела доходів ---");
            foreach (var source in IncomeSources)
            {
                Console.WriteLine($"- {source.Name}");
            }
            Console.WriteLine("-----------------------");
        }

        // --- Методи Захара: Керування Категоріями Витрат ---
        public bool AddExpenseCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Помилка: Назва категорії витрат не може бути порожньою.");
                return false;
            }
            if (ExpenseCategories.Exists(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Помилка: Категорія витрат з назвою '{name}' вже існує.");
                return false;
            }
            ExpenseCategories.Add(new ExpenseCategory(name));
            Console.WriteLine($"Категорія витрат '{name}' успішно додана.");
            SaveData();
            return true;
        }

        public bool RemoveExpenseCategory(string name)
        {
            ExpenseCategory categoryToRemove = ExpenseCategories.Find(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (categoryToRemove == null)
            {
                Console.WriteLine($"Помилка: Категорія витрат з назвою '{name}' не знайдено.");
                return false;
            }
            // TODO: Розглянути перевірку, чи використовується категорія в транзакціях перед видаленням
            ExpenseCategories.Remove(categoryToRemove);
            Console.WriteLine($"Категорія витрат '{name}' успішно видалена.");
            SaveData();
            return true;
        }
        
        public List<ExpenseCategory> GetExpenseCategories()
        {
            return new List<ExpenseCategory>(ExpenseCategories);
        }

        public void DisplayExpenseCategories()
        {
            if (!ExpenseCategories.Any())
            {
                Console.WriteLine("Список категорій витрат порожній.");
                return;
            }
            Console.WriteLine("\n--- Категорії витрат ---");
            foreach (var category in ExpenseCategories)
            {
                Console.WriteLine($"- {category.Name}");
            }
            Console.WriteLine("------------------------");
        }

        // --- Методи Захара: Додавання Транзакцій Доходів/Витрат ---
        public bool AddIncomeTransaction(string walletName, decimal amount, string sourceName, string description)
        {
            Wallet targetWallet = Wallets.Find(w => w.Name.Equals(walletName, StringComparison.OrdinalIgnoreCase));
            if (targetWallet == null)
            {
                Console.WriteLine($"Помилка: Гаманець '{walletName}' не знайдено.");
                return false;
            }
            IncomeSource source = IncomeSources.Find(s => s.Name.Equals(sourceName, StringComparison.OrdinalIgnoreCase));
            if (source == null)
            {
                Console.WriteLine($"Помилка: Джерело доходу '{sourceName}' не знайдено. Спочатку додайте джерело.");
                return false;
            }
            if (amount <= 0)
            {
                Console.WriteLine("Помилка: Сума доходу має бути позитивною.");
                return false;
            }

            targetWallet.Balance += amount;
            Transaction incomeTransaction = new Transaction(
                DateTime.Now,
                TransactionType.Income,
                amount,
                targetWallet.WalletCurrency,
                description,
                targetWallet,
                sourceName 
            );
            Transactions.Add(incomeTransaction);
            Console.WriteLine($"Дохід {amount:N2} {targetWallet.WalletCurrency.Code} успішно додано до гаманця '{walletName}' від '{sourceName}'.");
            SaveData();
            return true;
        }

        public bool AddExpenseTransaction(string walletName, decimal amount, string categoryName, string description)
        {
            Wallet targetWallet = Wallets.Find(w => w.Name.Equals(walletName, StringComparison.OrdinalIgnoreCase));
            if (targetWallet == null)
            {
                Console.WriteLine($"Помилка: Гаманець '{walletName}' не знайдено.");
                return false;
            }
            ExpenseCategory category = ExpenseCategories.Find(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
            if (category == null)
            {
                Console.WriteLine($"Помилка: Категорія витрат '{categoryName}' не знайдена. Спочатку додайте категорію.");
                return false;
            }
            if (amount <= 0)
            {
                Console.WriteLine("Помилка: Сума витрати має бути позитивною.");
                return false;
            }
            if (targetWallet.Balance < amount)
            {
                Console.WriteLine($"Помилка: Недостатньо коштів у гаманці '{walletName}'. Поточний баланс: {targetWallet.Balance:N2} {targetWallet.WalletCurrency.Code}.");
                return false;
            }

            targetWallet.Balance -= amount;
            Transaction expenseTransaction = new Transaction(
                DateTime.Now,
                TransactionType.Expense,
                amount,
                targetWallet.WalletCurrency,
                description,
                targetWallet,
                categoryName 
            );
            Transactions.Add(expenseTransaction);
            Console.WriteLine($"Витрату {amount:N2} {targetWallet.WalletCurrency.Code} успішно списано з гаманця '{walletName}' по категорії '{categoryName}'.");
            SaveData();
            return true;
        }

        // --- Методи Давида (з main) ---
        private void InitializeDefaultCurrencies()
        {
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
                    AppData? data = JsonSerializer.Deserialize<AppData>(jsonData); // Додано ? для nullable reference type
                    if (data != null)
                    {
                        Wallets = data.Wallets ?? new List<Wallet>();
                        Transactions = data.Transactions ?? new List<Transaction>();
                        Currencies = data.Currencies ?? new List<Currency>();
                        IncomeSources = data.IncomeSources ?? new List<IncomeSource>();
                        ExpenseCategories = data.ExpenseCategories ?? new List<ExpenseCategory>();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка завантаження даних: {ex.Message}. Буде створено новий файл даних.");
                }
            }
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
                string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(DataFileName, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження даних: {ex.Message}");
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            return new List<Transaction>(Transactions); // Повертаємо копію
        }

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
                Console.WriteLine("Помилка: Гаманці мають однакову валюту.");
                return false;
            }
            if (amountToConvert <= 0)
            {
                Console.WriteLine("Помилка: Сума для конвертації має бути позитивною.");
                return false;
            }
            if (fromWallet.Balance < amountToConvert)
            {
                Console.WriteLine($"Помилка: Недостатньо коштів у гаманці '{fromWallet.Name}'.");
                return false;
            }

            decimal convertedAmount;
            try
            {
                // Припускаємо, що CurrencyConverter.Convert статичний
                convertedAmount = CurrencyConverter.Convert(amountToConvert, exchangeRate);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Помилка конвертації: {ex.Message}");
                return false;
            }
            if (convertedAmount <= 0) // Додано перевірку, що результат конвертації позитивний
            {
                Console.WriteLine("Помилка: Конвертована сума не може бути нульовою або від'ємною. Перевірте курс.");
                return false;
            }

            string exchangeDescriptionOut = $"Обмін {amountToConvert:N2} {fromWallet.WalletCurrency.Code} на {convertedAmount:N2} {toWallet.WalletCurrency.Code} (в гаманець {toWallet.Name})";
            Transaction debitTransaction = new Transaction(DateTime.Now, TransactionType.Expense, amountToConvert, fromWallet.WalletCurrency, exchangeDescriptionOut, fromWallet, "Обмін валют");
            Transactions.Add(debitTransaction);
            fromWallet.Balance -= amountToConvert;

            string exchangeDescriptionIn = $"Обмін {amountToConvert:N2} {fromWallet.WalletCurrency.Code} (з гаманця {fromWallet.Name}) на {convertedAmount:N2} {toWallet.WalletCurrency.Code}";
            Transaction creditTransaction = new Transaction(DateTime.Now, TransactionType.Income, convertedAmount, toWallet.WalletCurrency, exchangeDescriptionIn, toWallet, "Обмін валют");
            Transactions.Add(creditTransaction);
            toWallet.Balance += convertedAmount;

            SaveData();
            Console.WriteLine($"Обмін успішно виконано. Списано {amountToConvert:N2} {fromWallet.WalletCurrency.Code} з '{fromWallet.Name}'. Зараховано {convertedAmount:N2} {toWallet.WalletCurrency.Code} до '{toWallet.Name}'.");
            return true;
        }
    }
}