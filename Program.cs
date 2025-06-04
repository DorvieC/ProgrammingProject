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
    }
} 