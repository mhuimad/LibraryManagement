# LibraryManagement

API de gestion des emprunts d'une bibliothèque municipale (catalogue, emprunts, retours, pénalités de retard).

## Architecture

Clean Architecture / DDD, 3 couches :

- **`LibraryManagement.Domain`** : agrégats (`Book`, `Member`, `Loan`), value objects, commands/queries MediatR, sans dépendance à un autre projet.
- **`LibraryManagement.Infrastructure`** : repositories Dapper, `UnitOfWork`, pipeline behaviors MediatR (validation, transaction).
- **`LibraryManagement.Api`** : controllers, DI, middleware de gestion d'erreurs.

Flux : `Controller → MediatR → ValidationBehavior → TransactionBehavior (commands only) → Handler`.

Base de données SQL Server gérée en projet SSDT (`database/`), déployée en DACPAC.

## Lancer l'API

Prérequis : Docker.

```bash
docker compose up --build
```

L'API est disponible sur `http://localhost:8080`, Swagger UI sur `http://localhost:8080/swagger`.

## Tests

```bash
dotnet test tests/LibraryManagement.DomainUnitTests
dotnet test tests/LibraryManagement.IntegrationTests
```

Les tests d'intégration nécessitent Docker (Testcontainers démarre un SQL Server réel).
