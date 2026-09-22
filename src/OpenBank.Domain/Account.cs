using OpenBank.Domain.Exceptions;

namespace OpenBank.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public string AccountNumber { get; private set; } = string.Empty;
    public string Agency { get; private set; } = string.Empty;
    public string OwnerName { get; private set; } = string.Empty;
    public string DocumentNumber { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public int Version { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Account() { }

    public Account(string accountNumber, string agency, string ownerName, string documentNumber, decimal initialBalance = 0)
    {
        if (initialBalance < 0)
            throw new DomainException("O saldo inicial não pode ser negativo.");

        Id = Guid.NewGuid();
        AccountNumber = accountNumber;
        Agency = agency;
        OwnerName = ownerName;
        DocumentNumber = documentNumber;
        Balance = initialBalance;
        Version = 1;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Credit(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("O valor do crédito deve ser maior que zero.");

        Balance += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Debit(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("O valor do débito deve ser maior que zero.");

        if (Balance < amount)
            throw new DomainException("Saldo insuficiente para realizar a transação.");

        Balance -= amount;
        UpdatedAt = DateTime.UtcNow;
    }
}