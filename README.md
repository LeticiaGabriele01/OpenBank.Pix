# 🏦 OpenBank.Pix — Motor Transacional & Core Banking API

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?style=for-the-badge&logo=postgresql)
![Redis](https://img.shields.io/badge/Redis-Cache-DC382D?style=for-the-badge&logo=redis)
![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?style=for-the-badge&logo=docker)

API de alto desempenho simulando o motor transacional de um sistema bancário e o fluxo de liquidação do **Pix**. O objetivo principal deste projeto é aplicar conceitos avançados de engenharia de software para resolver problemas críticos do setor financeiro: **concorrência violenta, consistência estrita de dados e tolerância a falhas**.

---

## 🎯 Desafios Técnicos Resolvidos

- **Controle de Concorrência e Race Conditions:** Prevenção de gasto duplo e saldo negativo em requisições simultâneas usando **Lock Pessimista (`FOR UPDATE`)** no banco de dados.
- **Idempotência de Requisições:** Interceptação via Middleware customizado no C# com armazenamento em **Redis** para evitar reprocessamento de transações em falhas de rede.
- **Partida Dobrada (Double-Entry Ledger):** Garantia de consistência contábil onde todo débito em uma conta possui um crédito correspondente de igual valor.
- **Arquitetura Híbrida de ORMs:** Uso de **Entity Framework Core** para operações relacionais de escrita (ACID) e **Dapper** para leitura de extratos em altíssima performance.

---

## 🛠️ Tecnologias e Padrões Utilizados

- **Linguagem / Framework:** C# / .NET 8 / Web API
- **Arquitetura:** Clean Architecture & SOLID
- **Banco de Dados:** PostgreSQL 16 (Relacional)
- **Cache & Locks:** Redis
- **Mapeamento de Dados:** Entity Framework Core & Dapper
- **Validação & Injeção:** FluentValidation & Dependency Injection nativa do .NET
- **Testes de Carga:** k6
- **Containerização:** Docker & Docker Compose

---

## 🏗️ Arquitetura do Sistema

```mermaid
sequenceDiagram
    autonumber
    actor Cliente
    participant API as API (.NET 8)
    participant Redis as Redis Cache
    participant DB as PostgreSQL

    Cliente->>API: POST /api/v1/pix/transfers (Header: X-Idempotency-Key)
    API->>Redis: Checa se chave de idempotência existe
    alt Chave encontrada
        Redis-->>API: Retorna resposta em cache
        API-->>Cliente: 200 OK (Transação já processada)
    else Chave nova
        API->>DB: Inicia Transação SQL + Lock Pessimista na Conta (FOR UPDATE)
        DB-->>API: Linha bloqueada com saldo atualizado
        API->>API: Valida saldo e regras de negócio
        API->>DB: Registra Débito/Crédito (Partida Dobrada)
        API->>DB: Commit da Transação SQL
        API->>Redis: Salva chave de idempotência (TTL 24h)
        API-->>Cliente: 201 Created (Pix realizado)
    end
