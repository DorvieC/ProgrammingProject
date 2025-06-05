using System;
using System.Globalization;
using System.Linq;

namespace PersonalFinanceTracker
{
    class Program
    {
        private static FinanceManager manager = new FinanceManager();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Ласкаво просимо до Менеджера Особистих Фінансів!");
            ShowMainMenu();
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\nГоловне меню:");
                Console.WriteLine("1. Показати баланс");
                Console.WriteLine("2. Додати дохід");
                Console.WriteLine("3. Додати витрату");
                Console.WriteLine("4. Показати транзакції");
                Console.WriteLine("5. Керування гаманцями");
                Console.WriteLine("6. Керування джерелами та категоріями");
                Console.WriteLine("7. Обмін валют");
                Console.WriteLine("8. Вихід");
                Console.Write("Оберіть опцію: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        HandleShowBalance();
                        break;
                    case "2":
                        HandleAddIncome();
                        break;
                    case "3":
                        HandleAddExpense();
                        break;
                    case "4":
                        HandleDisplayAllTransactions(); // Змінено на виклик нового методу
                        break;
                    case "5":
                        HandleWalletManagement();
                        break;
                    case "6":
                        HandleSourcesAndCategoriesManagement();
                        break;
                    case "7":
                        HandleCurrencyExchange(); // Змінено на виклик нового методу
                        break;
                    case "8":
                        Console.WriteLine("До побачення!");
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Будь ласка, спробуйте знову.");
                        break;
                }
            }
        }

        static void HandleShowBalance()
        {
            manager.DisplayAllWalletsBalances();
        }

        static void HandleAddIncome()
        {
            Console.WriteLine("\n--- Додавання доходу ---");

            if (!manager.Wallets.Any())
            {
                Console.WriteLine("Спочатку потрібно додати гаманець.");
                return;
            }

            var incomeSources = manager.GetIncomeSources();
            if (!incomeSources.Any())
            {
                Console.WriteLine("Спочатку потрібно додати джерело доходу в меню 'Керування джерелами та категоріями'.");
                return;
            }
            
            Console.WriteLine("Доступні гаманці:");
            foreach (var w in manager.Wallets)
            {
                Console.WriteLine($"- {w.Name} ({w.Balance:N2} {w.WalletCurrency.Code})");
            }
            Console.Write("Введіть назву гаманця для зарахування: ");
            string walletName = Console.ReadLine();

            Console.Write($"Введіть суму доходу: ");
            string amountString = Console.ReadLine();
            if (!decimal.TryParse(amountString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Некоректна сума. Сума має бути позитивним числом.");
                return;
            }

            Console.WriteLine("Доступні джерела доходу:");
            manager.DisplayIncomeSources(); // Метод з FinanceManager покаже форматований список
            Console.Write("Введіть назву джерела доходу: ");
            string sourceName = Console.ReadLine();

            Console.Write("Введіть опис (необов'язково): ");
            string description = Console.ReadLine();

            manager.AddIncomeTransaction(walletName, amount, sourceName, description);
        }

        static void HandleAddExpense()
        {
            Console.WriteLine("\n--- Додавання витрати ---");

            if (!manager.Wallets.Any())
            {
                Console.WriteLine("Спочатку потрібно додати гаманець.");
                return;
            }

            var expenseCategories = manager.GetExpenseCategories();
            if (!expenseCategories.Any())
            {
                Console.WriteLine("Спочатку потрібно додати категорію витрат в меню 'Керування джерелами та категоріями'.");
                return;
            }

            Console.WriteLine("Доступні гаманці:");
            foreach (var w in manager.Wallets)
            {
                Console.WriteLine($"- {w.Name} ({w.Balance:N2} {w.WalletCurrency.Code})");
            }
            Console.Write("Введіть назву гаманця для списання: ");
            string walletName = Console.ReadLine();

            Console.Write($"Введіть суму витрати: ");
            string amountString = Console.ReadLine();
            if (!decimal.TryParse(amountString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal amount)  || amount <= 0)
            {
                Console.WriteLine("Некоректна сума. Сума має бути позитивним числом.");
                return;
            }

            Console.WriteLine("Доступні категорії витрат:");
            manager.DisplayExpenseCategories(); // Метод з FinanceManager покаже форматований список
            Console.Write("Введіть назву категорії витрат: ");
            string categoryName = Console.ReadLine();

            Console.Write("Введіть опис (необов'язково): ");
            string description = Console.ReadLine();

            manager.AddExpenseTransaction(walletName, amount, categoryName, description);
        }

        static void HandleWalletManagement()
        {
            Console.WriteLine("\n--- Керування гаманцями ---");
            Console.WriteLine("1. Додати новий гаманець");
            Console.WriteLine("2. Переглянути список гаманців");
            Console.WriteLine("3. Видалити гаманець");
            Console.WriteLine("0. Повернутися до головного меню");
            Console.Write("Оберіть опцію: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введіть назву гаманця: ");
                    string nameAdd = Console.ReadLine();

                    Console.WriteLine("Доступні валюти для гаманця:");
                    if (manager.Currencies.Count == 0)
                    {
                        Console.WriteLine("У системі ще немає валют. Спочатку вони мають бути завантажені або додані.");
                    }
                    else
                    {
                        foreach (var currency in manager.Currencies)
                        {
                            Console.WriteLine($"- {currency.Code} ({currency.Name})");
                        }
                    }
                    Console.Write("Введіть код валюти гаманця (напр., USD): ");
                    string currencyCodeAdd = Console.ReadLine().ToUpper();

                    Console.Write("Введіть початковий баланс: ");
                    if (decimal.TryParse(Console.ReadLine(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal balanceAdd))
                    {
                        manager.AddWallet(nameAdd, currencyCodeAdd, balanceAdd);
                    }
                    else
                    {
                        Console.WriteLine("Некоректний формат балансу. Будь ласка, використовуйте крапку як десятковий розділювач (напр., 100.50).");
                    }
                    break;
                case "2":
                    var walletsView = manager.GetWallets();
                    if (walletsView.Count == 0)
                    {
                        Console.WriteLine("Список гаманців порожній.");
                    }
                    else
                    {
                        Console.WriteLine("\n--- Ваші гаманці ---");
                        foreach (var wallet in walletsView)
                        {
                            Console.WriteLine($"- {wallet.Name} ({wallet.Balance:N2} {wallet.WalletCurrency.Code})");
                        }
                    }
                    break;
                case "3":
                    Console.Write("Введіть назву гаманця для видалення: ");
                    string nameRemove = Console.ReadLine();
                    manager.RemoveWallet(nameRemove);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }
        
        static void HandleSourcesAndCategoriesManagement()
        {
            while (true)
            {
                Console.WriteLine("\n--- Керування джерелами та категоріями ---");
                Console.WriteLine("1. Керування джерелами доходу");
                Console.WriteLine("2. Керування категоріями витрат");
                Console.WriteLine("0. Повернутися до головного меню");
                Console.Write("Оберіть опцію: ");
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        HandleIncomeSourceManagementSubMenu();
                        break;
                    case "2":
                        HandleExpenseCategoryManagementSubMenu();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Будь ласка, спробуйте знову.");
                        break;
                }
            }
        }

        static void HandleIncomeSourceManagementSubMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Керування джерелами доходу ---");
                Console.WriteLine("1. Додати джерело доходу");
                Console.WriteLine("2. Переглянути джерела доходу");
                Console.WriteLine("3. Видалити джерело доходу");
                Console.WriteLine("0. Повернутися");
                Console.Write("Оберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введіть назву нового джерела доходу: ");
                        string sourceNameToAdd = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(sourceNameToAdd))
                        {
                            manager.AddIncomeSource(sourceNameToAdd);
                        }
                        else
                        {
                            Console.WriteLine("Назва джерела доходу не може бути порожньою.");
                        }
                        break;
                    case "2":
                        manager.DisplayIncomeSources();
                        break;
                    case "3":
                        Console.Write("Введіть назву джерела доходу для видалення: ");
                        string sourceNameToRemove = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(sourceNameToRemove))
                        {
                            manager.RemoveIncomeSource(sourceNameToRemove);
                        }
                        else
                        {
                            Console.WriteLine("Назва джерела доходу не може бути порожньою.");
                        }
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Будь ласка, спробуйте знову.");
                        break;
                }
            }
        }

        static void HandleExpenseCategoryManagementSubMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Керування категоріями витрат ---");
                Console.WriteLine("1. Додати категорію витрат");
                Console.WriteLine("2. Переглянути категорії витрат");
                Console.WriteLine("3. Видалити категорію витрат");
                Console.WriteLine("0. Повернутися");
                Console.Write("Оберіть опцію: ");
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        Console.Write("Введіть назву нової категорії витрат: ");
                        string categoryNameToAdd = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(categoryNameToAdd))
                        {
                            manager.AddExpenseCategory(categoryNameToAdd);
                        }
                        else
                        {
                            Console.WriteLine("Назва категорії витрат не може бути порожньою.");
                        }
                        break;
                    case "2":
                        manager.DisplayExpenseCategories();
                        break;
                    case "3":
                        Console.Write("Введіть назву категорії витрат для видалення: ");
                        string categoryNameToRemove = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(categoryNameToRemove))
                        {
                            manager.RemoveExpenseCategory(categoryNameToRemove);
                        }
                        else
                        {
                            Console.WriteLine("Назва категорії витрат не може бути порожньою.");
                        }
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Будь ласка, спробуйте знову.");
                        break;
                }
            }
        }
        
        static void HandleDisplayAllTransactions()
        {
            Console.WriteLine("\n--- Список транзакцій ---");
            var transactions = manager.GetAllTransactions();

            if (transactions == null || !transactions.Any())
            {
                Console.WriteLine("Транзакцій ще немає.");
                return;
            }

            foreach (var t in transactions)
            {
                string transactionTypeString = t.Type == TransactionType.Income ? "Дохід" : "Витрата";
                string currencyCode = t.TransactionCurrency?.Code ?? "N/A";
                string walletName = t.TargetWallet?.Name ?? "N/A";
                // Використовуємо CategoryOrSource, як у Transaction.cs
                Console.WriteLine($"{t.Date:dd.MM.yyyy HH:mm} | {transactionTypeString,-7} | {t.Amount,10:N2} {currencyCode,-3} | Гаманець: {walletName,-15} | Кат./Дж.: {t.CategoryOrSource,-20} | Опис: {t.Description}");
            }
            Console.WriteLine("-------------------------");
        }

        static void HandleCurrencyExchange()
        {
            Console.WriteLine("\n--- Обмін валют ---");

            var wallets = manager.GetWallets(); 
            if (wallets == null || wallets.Count < 2)
            {
                Console.WriteLine("Для обміну потрібно щонайменше два гаманці з різними валютами.");
                return;
            }

            Console.WriteLine("Доступні гаманці:");
            foreach (var w in wallets)
            {
                Console.WriteLine($"- {w.Name} ({w.Balance:N2} {w.WalletCurrency.Code})");
            }

            Console.Write("Введіть назву гаманця, з якого обмінюєте: ");
            string fromWalletName = Console.ReadLine();
            // Wallet fromWallet = wallets.Find(w => w.Name.Equals(fromWalletName, StringComparison.OrdinalIgnoreCase)); // Не використовується далі

            Console.Write("Введіть назву гаманця, на який обмінюєте: ");
            string toWalletName = Console.ReadLine();
            // Wallet toWallet = wallets.Find(w => w.Name.Equals(toWalletName, StringComparison.OrdinalIgnoreCase)); // Не використовується далі

            Console.Write($"Введіть суму для обміну: "); // Валюта буде визначена з fromWallet у FinanceManager
            string amountString = Console.ReadLine();
            if (!decimal.TryParse(amountString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amountToConvert) || amountToConvert <= 0)
            {
                Console.WriteLine("Некоректний формат суми або сума не є позитивною.");
                return;
            }

            Console.Write($"Введіть обмінний курс: "); // Детальніше про курс запитується у FinanceManager або він береться з API в майбутньому
            string rateString = Console.ReadLine();
            if (!decimal.TryParse(rateString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal exchangeRate) || exchangeRate <= 0)
            {
                Console.WriteLine("Некоректний формат курсу або курс не є позитивним.");
                return;
            }

            manager.ExchangeCurrency(fromWalletName, toWalletName, amountToConvert, exchangeRate);
        }
    }
}