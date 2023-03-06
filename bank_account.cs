using System;
using System.Collections.Generic;

public class TransactionHistory
{
	Dictionary<int,String> transactions = new Dictionary<int, String>();
	
	public TransactionHistory(){}
	
	public void saveTransaction(int accountNumber, String transactionInformation)
	{
		if (transactions.ContainsKey(accountNumber) == true)
		{
			String history = transactions[accountNumber];
			transactions[accountNumber] = history + "\n" + transactionInformation;
		}
		else{ transactions.Add(accountNumber,transactionInformation); }
	}
	
	public void ReturnTransactions(Accounts account)
	{
		int accountNumber = 0;
		BankAccount userAccount = null;

    Console.WriteLine("Enter an account number");
		accountNumber = Int32.Parse(Console.ReadLine());
		userAccount = account.ReturnBankAccount(accountNumber);
		
		if (userAccount == null)
		{
		  Console.WriteLine("There is no account associated with this account number");
		  TransactionHistory returnTransactionsRecurive = new TransactionHistory();
		  returnTransactionsRecurive.ReturnTransactions(account);
		}

		foreach(KeyValuePair<int,String> i in transactions)
		{
			if (i.Key == accountNumber)
			{
				Console.WriteLine(accountNumber + " transaction history: \n");
				Console.WriteLine(i.Value + "\n\n");	
			}
		}	
	}
}

public class Transaction
{
	BankAccount userAccount;
	public Transaction(String option,Accounts account, TransactionHistory transaction)
	{
		int accountNumber = 0;
		int amount = 0;
    BankAccount userAccount = null;

    Console.WriteLine("Enter an  account number");
		accountNumber = Int32.Parse(Console.ReadLine());
		userAccount = account.ReturnBankAccount(accountNumber);
		
		if (userAccount == null)
		{
		  Console.WriteLine("There is no account associated with this account number");
		  NewAccount newAccountRecursive = new NewAccount(account, transaction);
		}
				
		if (option == "2")
		{
			Console.WriteLine("Enter withdrawal amount");			
			amount = Int32.Parse(Console.ReadLine());
    
		while (userAccount(amount) == false)
		{
				Console.WriteLine("You have insufficient balance for this transaction");
				accountNumber = Int32.Parse(Console.ReadLine());
		}
			transaction.saveTransaction(accountNumber,amount+" Withdrawn");
		}
		else{
			Console.WriteLine("Enter deposit amount");
			amount = Int32.Parse(Console.ReadLine());
			while (userAccount.Deposit(amount) == false)
			{
				Console.WriteLine("Invalid deposit amount. Amount must be a numerical digit greater than 0");
				accountNumber = Int32.Parse(Console.ReadLine());
			}
			transaction.saveTransaction(accountNumber,amount+" Deposited");
		}
		Console.WriteLine("Your new balance is: {0}", userAccount._accountBalance + "\n");
	}
}

public class NewAccount
{
	public NewAccount(Accounts account, TransactionHistory newTransaction)
	{
		int initialBalance = 0;
		int accountNumber = 0;
    BankAccount existingAccount = null;
    BankAccount newAccount = null;

   	Console.WriteLine("Enter an  account number");
		accountNumber = Int32.Parse(Console.ReadLine());
		existingAccount = account.ReturnBankAccount(accountNumber);
		
		if (existingAccount != null)
		{
		  Console.WriteLine("This account number is already in use");
		  NewAccount newAccountRecursive = new NewAccount(account, newTransaction);
		}

		Console.WriteLine("Enter your initial balance");
		initialBalance = Int32.Parse(Console.ReadLine());
		newAccount = new BankAccount(accountNumber,initialBalance);
		account.userAccounts.Add(newAccount);
		newTransaction.saveTransaction(accountNumber,"New account created");
		Console.WriteLine("\n\n{0} account has been created with {1} initial balance\n\n", newAccount._accountNumber, newAccount._accountBalance);	
	}
}
public class Accounts
{
	public List<BankAccount> userAccounts = new List<BankAccount>();
	public Accounts(){}
	
	public BankAccount ReturnBankAccount(int accountNumber)
	{
		foreach(BankAccount i in userAccounts)
		{
			if (i._accountNumber == accountNumber)
			{
				return i;
			}	
		}
		return null;
	}
}

public class AccountBalance
{
	public AccountBalance(Accounts account)
	{
		int accountNumber = 0;
    BankAccount userAccount = null;

   	Console.WriteLine("Enter an  account number");
		accountNumber = Int32.Parse(Console.ReadLine());
		userAccount = account.ReturnBankAccount(accountNumber);
		
		if (userAccount == null)
		{
		  Console.WriteLine("There is no account associated with this account Number");
		  AccountBalance accountBalanceRecursive = new AccountBalance(account);
		}
		Console.WriteLine("Account Balance is: " + userAccount._accountBalance);
	}
}

public class BankAccount
{
	public int _accountNumber {get; set;}
	public int _accountBalance {get; set;}
	
	
	public BankAccount(int accountNumber, int accountBalance)
	{
		_accountNumber = accountNumber;
		_accountBalance = accountBalance;
	}
	
	public Boolean Withdraw(int amount)
	{
		if (_accountBalance - amount >= 0)
		{		
			_accountBalance -= amount;
			return true;
		}
		return false;
	}
	
	public Boolean Deposit(int amount)
	{
		if (amount > 0)
		{
			_accountBalance += amount;
			return true;
		}
		return false;
	}
}

public class Program
{
	public static void Main()
	{
		String option = "";
		Accounts account = new Accounts();
		TransactionHistory transactions = new TransactionHistory();
		Transaction newTransaction;
		while(option != "exit")
		{
			Console.WriteLine("Welcome to the Bank, check the menu below to continue\n 1 - Create a new Account \n 2 - Make a withdrawal \n 3 - Make a deposit \n 4 - Check your balance \n 5 - Transaction History \n 6 - List of accounts \n Write exit to quit the program");
			option = Console.ReadLine();
			
			switch(option)
			{
				case "1":
					NewAccount userAccount = new NewAccount(account,transactions);
					break;
				case "2":
					newTransaction = new Transaction("2",account,transactions);
					break;
				case "3":
					newTransaction = new Transaction("3",account,transactions);
					break;
				case "4":
					AccountBalance balance = new AccountBalance(account);
					break;
				case "5":
					transactions.ReturnTransactions(account);
					break;
				case "6":
					Console.WriteLine("List of accounts\n");
					foreach (BankAccount i in account.userAccounts)
					{
						Console.WriteLine(i._accountNumber);
					}
					break;
			}
		}
	}
}