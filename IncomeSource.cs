namespace PersonalFinanceTracker
{
    public class IncomeSource
    {
        public string Name { get; set; }
        // Можна додати інші властивості, якщо потрібно

        public IncomeSource(string name)
        {
            Name = name;
        }
    }
} 