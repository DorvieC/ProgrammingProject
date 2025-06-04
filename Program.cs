using System;

namespace PersonalFinanceTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ласкаво просимо до Менеджера Особистих Фінансів!");
            // Тут буде основне меню та логіка програми
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
                        Console.WriteLine("Функція 'Показати баланс' ще не реалізована.");
                        break;
                    case "2":
                        // TODO: Додати дохід
                        Console.WriteLine("Функція 'Додати дохід' ще не реалізована.");
                        break;
                    case "3":
                        // TODO: Додати витрату
                        Console.WriteLine("Функція 'Додати витрату' ще не реалізована.");
                        break;
                    case "4":
                        // TODO: Показати транзакції
                        Console.WriteLine("Функція 'Показати транзакції' ще не реалізована.");
                        break;
                    case "5":
                         // TODO: Керування гаманцями
                        Console.WriteLine("Функція 'Керування гаманцями' ще не реалізована.");
                        break;
                    case "6":
                        // TODO: Керування категоріями
                        Console.WriteLine("Функція 'Керування категоріями' ще не реалізована.");
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
    }
} 