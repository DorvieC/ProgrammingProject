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
            Console.WriteLine("Ласкаво просимо до Менеджера Особистих Фінансів!");
            // Тепер не потрібно передавати менеджер, бо він доступний статично
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
                Console.WriteLine("6. Керування категоріями");
                Console.WriteLine("7. Обмін валют");
                Console.WriteLine("8. Вихід");
                Console.Write("Оберіть опцію: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // TODO: Показати баланс
                        HandleShowBalance(); // Викличемо новий метод
                        break;
                    case "2":
                        // TODO: Додати дохід
                        HandleAddIncome(); // Викличемо новий метод
                        break;
                    case "3":
                        // TODO: Додати витрату
                        HandleAddExpense(); // Викличемо новий метод
                        break;
                    case "4":
                        HandleDisplayAllTransactions(); // Змінено на виклик нового методу
                        break;
                    case "5":
                         // TODO: Керування гаманцями
                        HandleWalletManagement(); // Викличемо новий метод
                        break;
                    case "6":
                        // TODO: Керування категоріями
                        HandleCategoryManagement(); // Викличемо новий метод
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

        // Нові методи-обробники, які будуть реалізовувати Ярик та Захар
        // Вони використовують статичний `manager`

        static void HandleShowBalance()
        {
            // Цю функцію реалізує Ярик
            Console.WriteLine("Функція 'Показати баланс' буде реалізована Яриком.");
        }

        static void HandleAddIncome()
        {
            // Цю функцію реалізує Захар
            Console.WriteLine("Функція 'Додати дохід' буде реалізована Захаром.");
        }

        static void HandleAddExpense()
        {
            // Цю функцію реалізує Захар
            Console.WriteLine("Функція 'Додати витрату' буде реалізована Захаром.");
        }

        static void HandleWalletManagement()
        {
            // Цю функцію реалізує Ярик
            Console.WriteLine("Функція 'Керування гаманцями' буде реалізована Яриком.");
        }

        static void HandleCategoryManagement()
        {
            // Цю функцію реалізує Захар
            Console.WriteLine("Функція 'Керування категоріями' буде реалізована Захаром.");
        }

        // Новий метод для відображення всіх транзакцій
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
                // Переконуємося, що звертаємось до властивостей безпечно, якщо TransactionCurrency або TargetWallet можуть бути null
                // Однак, за логікою FinanceManager, вони не повинні бути null для існуючих транзакцій.
                string currencyCode = t.TransactionCurrency?.Code ?? "N/A";
                string walletName = t.TargetWallet?.Name ?? "N/A";

                Console.WriteLine($"{t.Date:dd.MM.yyyy HH:mm} | {transactionTypeString,-7} | {t.Amount,10:N2} {currencyCode,-3} | Гаманець: {walletName,-15} | Кат./Дж.: {t.CategoryOrSourceName,-20} | Опис: {t.Description}");
            }
            Console.WriteLine("-------------------------");
        }

        static void HandleCurrencyExchange()
        {
            Console.WriteLine("\n--- Обмін валют ---");

            var wallets = manager.Wallets;
            if (wallets == null || wallets.Count < 2)
            {
                Console.WriteLine("Для обміну потрібно щонайменше два гаманці з різними валютами.");
                return;
            }

            Console.WriteLine("Доступні гаманці:");
            foreach (var w in wallets)
            {
                Console.WriteLine($"- {w.Name} ({w.Balance} {w.WalletCurrency.Code})");
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
            if (!decimal.TryParse(amountString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amountToConvert))
            {
                Console.WriteLine("Некоректний формат суми.");
                return;
            }

            Console.Write($"Введіть обмінний курс (скільки {toWallet.WalletCurrency.Code} ви отримуєте за 1 {fromWallet.WalletCurrency.Code}): ");
            string rateString = Console.ReadLine();
            if (!decimal.TryParse(rateString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal exchangeRate))
            {
                Console.WriteLine("Некоректний формат курсу.");
                return;
            }

            if (manager.ExchangeCurrency(fromWalletName, toWalletName, amountToConvert, exchangeRate))
            {
                // Повідомлення про успіх вже виводиться методом ExchangeCurrency
            }
            else
            {
                // Повідомлення про помилку вже виводиться методом ExchangeCurrency
                Console.WriteLine("Операція обміну не вдалася."); // Додаткове загальне повідомлення
            }
        }
    }
} 