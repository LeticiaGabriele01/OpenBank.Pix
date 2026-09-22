using OpenBank.Domain.Enums;
using OpenBank.Domain.Exceptions;

namespace OpenBank.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid SourceAccountId { get; private set; }
    public Guid DestinationAccountId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionStatus Status { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string FailureReason { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private Transaction() { }

    public Transaction(Guid sourceAccountId, Guid destinationAccountId, decimal amount, string description = "")
    {
        if (sourceAccountId == Guid.Empty || destinationAccountId == Guid.Empty)
            throw new DomainException("Contas de origem e destino são obrigatórias.");

        if (sourceAccountId == destinationAccountId)
            throw new DomainException("A conta de origem e destino não podem ser as mesmas.");

        if (amount <= 0)
            throw new DomainException("O valor da transação deve ser maior que zero.");

        Id = Guid.NewGuid();
        SourceAccountId = sourceAccountId;
        DestinationAccountId = destinationAccountId;
        Amount = amount;
        Status = TransactionStatus.Pending;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        if (Status != TransactionStatus.Pending)
            throw new DomainException("Apenas transações pendentes podem ser concluídas.");

        Status = TransactionStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed(string reason)
    {
        if (Status != TransactionStatus.Pending)
            throw new DomainException("Apenas transações pendentes podem falhar.");

        Status = TransactionStatus.Failed;
        FailureReason = reason;
        CompletedAt = DateTime.UtcNow;
    }
}