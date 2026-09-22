using OpenBank.Domain.Enums;
using OpenBank.Domain.Exceptions;

namespace OpenBank.Domain.Entities;

public class PixKey
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public PixKeyType KeyType { get; private set; }
    public string KeyValue { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    // Propriedade de navegação do EF Core
    public Account? Account { get; private set; }

    private PixKey() { }

    public PixKey(Guid accountId, PixKeyType keyType, string keyValue)
    {
        if (accountId == Guid.Empty)
            throw new DomainException("A chave Pix deve estar vinculada a uma conta válida.");

        if (string.IsNullOrWhiteSpace(keyValue))
            throw new DomainException("O valor da chave Pix não pode ser vazio.");

        Id = Guid.NewGuid();
        AccountId = accountId;
        KeyType = keyType;
        KeyValue = keyValue.Trim();
        CreatedAt = DateTime.UtcNow;
    }
}