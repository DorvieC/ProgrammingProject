namespace PersonalFinanceTracker
{
    public class ExpenseCategory
    {
        public string Name { get; set; }
        // Можна додати інші властивості, якщо потрібно

        public ExpenseCategory(string name)
        {
            Name = name;
        }
    }
} 