namespace PersonalFinanceTracker
{
    public class Wallet
    {
        public string Name { get; set; }
        public Currency WalletCurrency { get; set; }
        public decimal Balance { get; set; }

        public Wallet(string name, Currency currency, decimal initialBalance = 0)
        {
            Name = name;
            WalletCurrency = currency;
            Balance = initialBalance;
        }
    }
} 