using System.Globalization;
using System;
using System.Linq; // Додано для перевірки наявності транзакцій
using System.Globalization; // Для CultureInfo.InvariantCulture при парсингу decimal

namespace PersonalFinanceTracker
{
    class Program
    {
        // Створюємо єдиний екземпляр FinanceManager для всього додатку
        private static FinanceManager manager = new FinanceManager();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Для коректного відображення українських символів
            Console.InputEncoding = System.Text.Encoding.UTF8;  // Для коректного вводу українських символів
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
                Console.WriteLine("6. Керування джерелами та категоріями"); // Оновлено
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
                        HandleDisplayAllTransactions(); // Оновлено
                        break;
                    case "5":
                        HandleWalletManagement();
                        break;
                    case "6":
                        HandleSourcesAndCategoriesManagement(); // Оновлено
                        break;
                    case "7":
                        HandleCurrencyExchange(); // Оновлено
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
            // Цей метод, ймовірно, реалізований або буде реалізований Яриком.
            // Якщо його реалізація вже є в FinanceManager, викликаємо її:
            manager.DisplayAllWalletsBalances();
            // Або залишити заглушку, якщо реалізації Яріка ще немає в цій гілці:
            // Console.WriteLine("Функція 'Показати баланс' буде реалізована Яриком.");
        }

        static void HandleAddIncome()
        {
            Console.WriteLine("\n--- Додавання доходу ---");

            if (!manager.Wallets.Any()) // Використовуємо manager.Wallets напряму, якщо GetWallets() повертає копію
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
            foreach (var w in manager.Wallets) // Тут також можна manager.Wallets
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
            manager.DisplayIncomeSources();
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
            manager.DisplayExpenseCategories();
            Console.Write("Введіть назву категорії витрат: ");
            string categoryName = Console.ReadLine();

            Console.Write("Введіть опис (необов'язково): ");
            string description = Console.ReadLine();

            manager.AddExpenseTransaction(walletName, amount, categoryName, description);
        }

        static void HandleWalletManagement()
        {
            // Цей метод, ймовірно, реалізований або буде реалізований Яриком.
            // Якщо його реалізація вже є:
            // manager.SomeWalletManagementMethodFromYarik(); // Приклад
            Console.WriteLine("Функція \'Керування гаманцями\' буде реалізована Яриком."); // Заглушка
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

        static void HandleDisplayAllTransactions()
        {
            Console.WriteLine("\n--- Список транзакцій ---");
            var transactions = manager.GetAllTransactions();

            if (transactions == null!|| transactions.Any())
            {
                Console.WriteLine("Транзакцій ще немає.");
                return;
            }

            foreach (var t in transactions)
            {
                string transactionTypeString = t.Type == TransactionType.Income ? "Дохід" : "Витрата";
                string currencyCode = t.TransactionCurrency?.Code ?? "N/A";
                string walletName = t.TargetWallet?.Name ?? "N/A";

                Console.WriteLine($"{t.Date:dd.MM.yyyy HH:mm} | {transactionTypeString,-7} | {t.Amount,10:N2} {currencyCode,-3} | Гаманець: {walletName,-15} | Кат./Дж.: {t.CategoryOrSource,-20} | Опис: {t.Description}");
            }
            Console.WriteLine("-------------------------");
        }

        static void HandleCurrencyExchange()
        {
            Console.WriteLine("\n--- Обмін валют ---");

            var wallets = manager.GetWallets(); // Використовуємо GetWallets() для отримання копії
            if (wallets == null || wallets.Count < 2)
            {
                Console.WriteLine("Для обміну потрібно щонайменше два гаманці з різними валютами.");
                return;
            }

            Console.WriteLine("Доступні гаманці:");
            foreach (var w in wallets)
            {
                Console.WriteLine($"- {w.Name} ({w.Balance:N2} {w.WalletCurrency.Code})"); // Баланс з форматуванням
            }

            Console.Write("Введіть назву гаманця, з якого обмінюєте: ");
            string fromWalletName = Console.ReadLine();
            Wallet fromWallet = wallets.Find(w => w.Name.Equals(fromWalletName, StringComparison.OrdinalIgnoreCase));
            if (fromWallet == null)
            {
                Console.WriteLine($"Гаманець '{fromWalletName}' не знайдено.");
                return;
            }

            Console.Write("Введіть назву гаманця, на який обмінюєте: ");
            string toWalletName = Console.ReadLine();
            Wallet toWallet = wallets.Find(w => w.Name.Equals(toWalletName, StringComparison.OrdinalIgnoreCase));
            if (toWallet == null)
            {
                Console.WriteLine($"Гаманець '{toWalletName}' не знайдено.");
                return;
            }

            if (fromWalletName.Equals(toWalletName, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Помилка: Вихідний та цільовий гаманці не можуть бути однаковими для обміну.");
                return;
            }

            if (fromWallet.WalletCurrency.Code == toWallet.WalletCurrency.Code)
            {
                Console.WriteLine("Помилка: Валюти гаманців однакові. Обмін неможливий або не має сенсу.");
                return;
            }

            
            Console.Write($"Введіть суму для обміну в {fromWallet.WalletCurrency.Code}: ");
            string amountString = Console.ReadLine();
            if (!decimal.TryParse(amountString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amountToConvert) || amountToConvert <= 0) // Додано перевірку <= 0
            {
                Console.WriteLine("Некоректний формат суми або сума не є позитивною.");
                return;
            }

            Console.Write($"Введіть обмінний курс (скільки {toWallet.WalletCurrency.Code} ви отримуєте за 1 {fromWallet.WalletCurrency.Code}): ");
            string rateString = Console.ReadLine();
            if (!decimal.TryParse(rateString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal exchangeRate) || exchangeRate <= 0) // Додано перевірку <= 0
            {
                Console.WriteLine("Некоректний формат курсу або курс не є позитивним.");
                return;
            }

            manager.ExchangeCurrency(fromWalletName, toWalletName, amountToConvert, exchangeRate);
            // Повідомлення про успіх/невдачу виводяться з FinanceManager
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
    }
}