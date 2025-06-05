using System;

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
                        // TODO: Показати транзакції
                        Console.WriteLine("Функція 'Показати транзакції' ще не реалізована.");
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
                        // TODO: Обмін валют
                        Console.WriteLine("Функція 'Обмін валют' ще не реалізована.");
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
            manager.DisplayAllWalletsBalances();
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
                        // Насправді, валюти за замовчуванням завантажуються при старті, якщо немає файлу даних.
                        // Цей блок може бути не потрібен, якщо валюти завжди є.
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
                    // Використовуємо CultureInfo.InvariantCulture для коректного парсингу decimal
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
                            Console.WriteLine($"- {wallet.Name} ({wallet.Balance:N2} {wallet.WalletCurrency.Code})"); // Форматування N2
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


        static void HandleCategoryManagement()
        {
            // Цю функцію реалізує Захар
            Console.WriteLine("Функція 'Керування категоріями' буде реалізована Захаром.");
        }
    }
} 