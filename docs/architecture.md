# 🏛️ Clean Architecture & System Layers

This document outlines the architectural boundaries and layer dependencies of the **OpenBank.Pix** core engine.

## 📐 Layer Dependency Diagram

```mermaid
graph TD
    API[OpenBank.API] --> App[OpenBank.Application]
    App --> Domain[OpenBank.Domain]
    Infra[OpenBank.Infrastructure] --> App
    Infra --> Domain