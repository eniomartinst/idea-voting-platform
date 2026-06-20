# CIS API (C# + ASP.NET Core)

## 📌 Description

This project provides a **CIS (Crowdsourced Ideation Solution) API** built with **C# and ASP.NET Core**, following clean architecture principles and best practices.

It manages core features such as **topics, ideas, and voting**, serving as the foundation for a scalable and modular backend system.

The goal is to maintain a **standardized architecture**, ensuring separation of concerns and long-term maintainability across services.

---

## 🧱 Architecture Overview

```text
src/
├── Core
│   ├── Application
│   └── Domain
├── Infrastructure
│   ├── Auth
│   ├── Http
│   ├── IoC
│   ├── MongoDB
│   └── MySQL
├── Presentation
│   └── RestApi
└── Server

tests/
├── Core.Application
├── Core.Domain
├── Infrastructure.MySQL
└── Presentation.RestApi
```

---

## ⚙️ Technologies

- .NET 9 / C#
- ASP.NET Core Web API
- Entity Framework Core (MySQL)
- MongoDB Driver
- Docker & Docker Compose

---

## 🐳 Environment Setup

### ⚠️ Prerequisite

Ensure the [Users API](https://gitlab.com/jala-university1/cohort-5/PT.CO.CSSD-232.GA.T1.26.M2/SA/group-1/users-api) is running.

### 🌐 Shared Docker Network

All services communicate through the shared Docker network: `cis-network`

> ⚠️ This network must exist before running the containers.

To **verify** that the network exists:

```bash
docker network ls

docker network inspect cis-network
```

If the network does not exist, **create** it:

```bash
docker network create cis-network
```

📄 Details: [docs/docker.md](docs/docker.md)

---

## 🚀 Running the Project

### 🐳 Docker (recommended)

```bash
docker compose up --build
```

```bash
docker compose up
```

```bash
docker compose down
```

---

## 🗄️ Database

- MySQL → `sd3db`
- MongoDB → `sd3_cis_db`

---

## 🔌 Database Provider Switch

The API supports **runtime switching between MySQL and MongoDB**:

```yaml
DatabaseConfig__Provider: "MongoDB" # or "MySQL"
```

### Behavior

- **MySQL**
  - EF Core + migrations (auto on startup)
- **MongoDB**
  - Document model (no schema migrations)
  - Requires ETL for existing data

### Example Config

```yaml
# MongoDB
DatabaseConfig__Provider: "MongoDB"
ConnectionStrings__MongoDB: "mongodb://mongodb:27017"

# MySQL
DatabaseConfig__Provider: "MySQL"
ConnectionStrings__CISDB: "server=mysql-db;port=3306;database=sd3db;user=root;password=root"
```

⚠️ Switching providers does **not migrate data automatically**

📄 Details: [docs/database.md](docs/database.md)

---

## 🔄 Data Migration

This project uses a **Python-based ETL script** to migrate data from MySQL to MongoDB.

- Covers: Topics, Ideas, Votes
- Idempotent (safe re-execution)
- Supports dry-run and validation

📄 Details: [docs/migration.md](docs/migration.md)

---

## 🗄️ Accessing Databases (Optional)

You can access both databases directly inside their containers for debugging, inspection, or manual queries.

---

### 🐬 MySQL Access

Connect to the MySQL container:

```bash
docker exec -it mysql-db mysql -u root -p
```

Password: `root`

Select the database:

```bash
USE sd3db;
```

---

### 🍃 MongoDB Access

Connect to the MongoDB container:

```bash
docker exec -it mongodb_ds3 mongosh
```

Select and initialize the CIS database:

```bash
use sd3_cis_db
show collections
```

---

### ⚠️ Notes

- MySQL database (`sd3db`) is **shared between `users-api` and `cis-api`**
- MongoDB uses **separate databases per service**:
  - `sd3_users_db`
  - `sd3_cis_db`
- MongoDB databases only appear after:
  - manual initialization (`init_collection`) and first write operation

---

## 🔄 Migrations (MySQL only)

The project uses **Entity Framework Core migrations**.

### Apply migrations

```
dotnet ef migrations add MigrationName --project src/Infrastructure/MySQL --startup-project src/Server
```


- Migrations are required only when changing database structure
- The database is automatically updated on application startup

---

## 🌐 API Endpoints

### Topics

| Method | Endpoint                            | Description  | Status      |
|--------|-------------------------------------|--------------|-------------|
| POST   | http://localhost:8005/api/v1/topics | Create topic | Implemented |
| GET    | http://localhost:8005/api/v1/topics | List topics  | Implemented |

### Ideas

| Method | Endpoint                                            | Description | Status      |
|--------|-----------------------------------------------------|-------------|-------------|
| POST   | http://localhost:8005/api/v1/topics/{topicId}/ideas | Create idea | Implemented |
| GET    | http://localhost:8005/api/v1/topics/{topicId}/ideas | List ideas  | Implemented |

### Votes

| Method | Endpoint                                           | Description  | Status      |
|--------|----------------------------------------------------|--------------|-------------|
| POST   | http://localhost:8005/api/v1/ideas/{ideaId}/vote   | Vote idea    | Implemented |
| POST   | http://localhost:8005/api/v1/ideas/{ideaId}/unvote | Unvote ideas | Implemented |

---

## 🧪 Tests

```bash
dotnet test
```

---

## 🛠️ Development Commands

Useful commands for development and project maintenance:

| Command                              | Description                                    |
|--------------------------------------|------------------------------------------------|
| `dotnet clean`                       | Clean build artifacts (`bin/` and `obj/`)      |
| `dotnet restore`                     | Restore project dependencies                   |
| `dotnet build`                       | Build the solution                             |
| `dotnet sln add **/*.csproj`         | Add all projects to the solution (**Linux**)   |
| `dotnet sln add (ls -r **/*.csproj)` | Add all projects to the solution (**Windows**) |
| `dotnet format`                      | Format code according to .NET standards        |

---

## 🧩 Services Integration

This API is part of a microservices ecosystem:

- `cis-api` → Core system (this repo)
- `users-api` → User management service
- `mysql-db` → Shared relational database (`sd3db`)
- `mongodb` → Shared instance with isolated databases per service

User data is not owned by CIS API — only referenced via `userId`.

📄 Details: [docs/integration.md](docs/integration.md)

---

## 📚 Documentation

| Topic                                  | Description                                                                          |
|----------------------------------------|--------------------------------------------------------------------------------------|
| [**Database**](docs/database.md)       | Database architecture, provider switch (MySQL <-> MongoDB), and persistence strategy |
| [**Docker**](docs/docker)              | Environment setup, containers, networking, and infrastructure                        |
| [**Integration**](docs/integration.md) | Service boundaries, authentication, and API communication                            |
| [**Migration**](docs/migration.md)     | ETL process, data migration, validation, and execution                               |