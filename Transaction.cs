using System;

namespace PersonalFinanceTracker
{
    public enum TransactionType
    {
        Income,
        Expense
    }

    public class Transaction
    {
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public Currency TransactionCurrency { get; set; }
        public string Description { get; set; }
        public Wallet TargetWallet { get; set; }
        // Для витрат можна додати категорію, для доходів - джерело
        public string CategoryOrSource { get; set; }


        public Transaction(DateTime date, TransactionType type, decimal amount, Currency currency, string description, Wallet wallet, string categoryOrSource)
        {
            Date = date;
            Type = type;
            Amount = amount; // Сума завжди позитивна, тип визначає дохід/витрату
            TransactionCurrency = currency;
            Description = description;
            TargetWallet = wallet;
            CategoryOrSource = categoryOrSource;
        }
    }
} 