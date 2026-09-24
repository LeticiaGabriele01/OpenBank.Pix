# 🔄 Pix Transfer Execution Flow

Detailed transactional sequence for executing a Pix transfer, featuring concurrency control via **Pessimistic Locking (`FOR UPDATE`)** and **Idempotency**.

## 📊 Sequence Diagram

```mermaid
sequenceDiagram
    autonumber
    actor Client as API Client / Controller
    participant Redis as Redis Cache
    participant App as PixService (Application)
    participant RepoAccount as IAccountRepository
    participant DB as PostgreSQL (DB Lock)

    Client->>Redis: Check Idempotency Key (X-Idempotency-Key)
    alt Key Exists
        Redis-->>Client: Return Cached Response (200 OK)
    else New Key
        Client->>App: Execute Pix Transfer (SendPixDTO)
        App->>DB: Begin Transaction + Pessimistic Lock (FOR UPDATE)
        DB-->>App: Lock Acquired
        App->>App: Validate Domain Rules (Debit / Credit)
        App->>DB: Write Ledger Records (Double-Entry)
        App->>DB: Commit Transaction
        App->>Redis: Store Idempotency Key (TTL 24h)
        App-->>Client: Return TransactionResultDTO (201 Created)
    end