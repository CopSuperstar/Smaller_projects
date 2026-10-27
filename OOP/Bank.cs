class Bank
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string message)
            : base(message) { }
    }

    public class Account
    {
        public Account(int i, string? owner, decimal balance)
        {
            this.Balance = balance;
            this.id = i;
            this.Owner = owner;
        }

        public int id { get; private set; }
        public string? Owner { get; private set; }
        public decimal Balance { get; protected set; }
        public event Action<decimal>? BalanceChanged;

        protected void OnBalanceChanged() => BalanceChanged?.Invoke(Balance);

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive");
            this.Balance += amount;
            OnBalanceChanged();
        }

        public virtual void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive");
            if (this.Balance >= amount)
            {
                this.Balance -= amount;
                OnBalanceChanged();
            }
            else
            {
                throw new InsufficientFundsException("Isn't enough money on the account");
            }
        }
    }

    public class CheckingAccount : Account
    {
        private int _overdraftLimit;

        public int OverdraftLimit
        {
            get => _overdraftLimit;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Limit cannot be negative.");
                _overdraftLimit = value;
            }
        }

        public CheckingAccount(int id, string? owner, decimal balance, int overdraftlimit)
            : base(id, owner, balance)
        {
            OverdraftLimit = overdraftlimit;
        }

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive");
            if (this.Balance - amount >= -OverdraftLimit)
            {
                this.Balance -= amount;
                OnBalanceChanged();
            }
            else
            {
                throw new InsufficientFundsException("Isn't enough money on the account");
            }
        }
    }

    public class BankRegistry
    {
        Dictionary<int, Account> accounts = new();

        public void Open(Account account)
        {
            if (!accounts.TryAdd(account.id, account))
                throw new ArgumentException("Account with this id already exists");
        }

        public Account? Find(int id) => accounts.GetValueOrDefault(id);

        public decimal? HighestBalance() => accounts.Values.Max(n => (decimal?)n.Balance);

        public decimal TotalMoneyAccrossAll() => accounts.Values.Sum(n => n.Balance);

        public int AccountsWithNegativeBalance()
        {
            if (accounts.Count == 0)
            {
                throw new Exception("no accounts in the bank");
            }
            return accounts.Values.Count(n => n.Balance < 0);
        }

        public Account? RichestAccount() => accounts.Values.MaxBy(a => a.Balance);
    }

    static void OpenAccount(BankRegistry reg)
    {
        Console.WriteLine("Creating a new Account. Enter an id");
        string? idInput = Console.ReadLine();
        if (!int.TryParse(idInput, out int id))
        {
            Console.WriteLine("id is a number!");
        }
        else
        {
            if (reg.Find(id)?.id == id)
            {
                throw new ArgumentException("Account with this id already exists");
            }
            Console.WriteLine("enter a name of the owner:");
            string? ownerInput = Console.ReadLine();
            Console.WriteLine("enter their balance:");
            string? balanceInput = Console.ReadLine();
            if (!decimal.TryParse(balanceInput, out decimal balance))
            {
                Console.WriteLine("balance is a number!");
            }
            else
            {
                reg.Open(new Account(id, ownerInput, balance));
            }
        }
    }

    static void ShowBalance(BankRegistry reg)
    {
        Console.WriteLine("To see a balance on a specific account, enter id:");

        string? idToCheck = Console.ReadLine();
        if (!int.TryParse(idToCheck, out int _idToCheck))
        {
            Console.WriteLine("not found");
        }
        else
        {
            Console.WriteLine(reg.Find(_idToCheck)?.Balance);
        }
    }

    public static void Run()
    {
        var test = new Account(99, "Test", 0);
        test.BalanceChanged += b => Console.WriteLine($"New balance: {b}");
        test.BalanceChanged += b => Console.WriteLine($"Second listener got {b}");
        test.BalanceChanged += b =>
        {
            if (b < 20)
            {
                Console.WriteLine("Balance is low!");
            }
        };
        test.Deposit(50);
        test.Withdraw(40);
        var chk = new CheckingAccount(100, "Check", 100, 200);
        chk.BalanceChanged += b => Console.WriteLine($"Checking: {b}");
        chk.Withdraw(50);
        Console.WriteLine(chk.Balance);
        string startMessage = """
            Choose an Option to start:

            0.  Quit
            1.  Open an Account
            2.  Show Balance
            3.  highest balance
            4.  the total money across all accounts
            5.  the accounts with a negative balance
            6.  richest account
            """;

        var reg = new BankRegistry();
        // reg.Open(new CheckingAccount(1, "Bob", 78.7m, 100));
        // reg.Open(new CheckingAccount(2, "Tom", -11.8m, 100));
        // reg.Open(new CheckingAccount(3, "Rex", 44.5m, 100));
        // reg.Open(new CheckingAccount(4, "Pal", -67.6m, 100));
        // reg.Open(new CheckingAccount(5, "Bill", 98.9m, 100));
        // reg.Open(new CheckingAccount(6, "Geil", 31.1m, 100));
        while (true)
        {
            Console.WriteLine(startMessage);
            string? baseMenuInput = Console.ReadLine();
            if (!int.TryParse(baseMenuInput, out int _baseMenuInput))
            {
                Console.WriteLine("Enter a number!");
            }
            else
            {
                if (_baseMenuInput == 0)
                {
                    break;
                }
                else if (_baseMenuInput == 1)
                {
                    try
                    {
                        OpenAccount(reg);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (_baseMenuInput == 2)
                {
                    ShowBalance(reg);
                }
                else if (_baseMenuInput == 3)
                {
                    decimal? highest = reg.HighestBalance();
                    if (highest is null)
                    {
                        Console.WriteLine("No accounts yet");
                    }
                    else
                    {
                        Console.WriteLine(highest);
                    }
                }
                else if (_baseMenuInput == 4)
                {
                    Console.WriteLine(reg.TotalMoneyAccrossAll());
                }
                else if (_baseMenuInput == 5)
                {
                    Console.WriteLine(reg.AccountsWithNegativeBalance());
                }
                else if (_baseMenuInput == 6)
                {
                    Account? richest = reg.RichestAccount();
                    if (richest is null)
                    {
                        Console.WriteLine("No accounts yet");
                    }
                    else
                    {
                        Console.WriteLine(richest);
                    }
                }
                else
                {
                    Console.WriteLine("Unknown command");
                }
            }
        }
    }
}
