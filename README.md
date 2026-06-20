# 💡 Idea Voting Platform (Monorepo)

> **A highly resilient, distributed backend ecosystem for Crowdsourced Ideation, featuring polyglot microservices, Clean Architecture, runtime database provider toggling, and automated NoSQL data migration.**

---

## 🛠️ Technology Stack

The ecosystem is distributed across polyglot microservices that integrate seamlessly via an internal Docker network:

* **Identity Service (Users API):** Java 21, Spring Boot 3, Spring Security, Maven, JUnit 5, JaCoCo.
* **Core Ideation Service (CIS API):** C#, .NET 9, ASP.NET Core Web API, Entity Framework Core, MongoDB Driver, xUnit, Moq, Coverlet.
* **Persistence Layers:** MySQL 8 (Relational) & MongoDB (NoSQL).
* **Infrastructure & Automation:** Docker & Docker Compose, Python 3 (ETL Migration Scripts).
* **Security:** Stateless JWT Authentication.

---

## 📂 Monorepo Structure

```text
idea-voting-platform/
│
├── cis-api/                 # Core Ideation REST API (.NET 9)
│   ├── src/                 # Clean Architecture Layers (Core, Infra, Presentation)
│   └── tests/               # Unit and Integration Tests (xUnit)
│
├── users-api/               # Identity REST API (Java Spring Boot)
│   ├── src/main/            # Main application source code
│   └── src/test/            # Unit Tests (JUnit 5)
│
├── scripts/                 # Python ETL Automation Scripts (MySQL -> MongoDB Migration)
└── README.md                # This centralized documentation
```

---

## 🧱 Architecture Overview

Both APIs strictly adhere to the separation of concerns principle through layered architectures, completely isolating core business rules from infrastructure and database implementations:

### CIS API Structure (C#)
* **Core:** Contains the *Domain* (Entities and Contracts) and *Application* (Use Cases and Services) layers, fully pure and framework-agnostic.
* **Infrastructure:** Implementations for authentication, HTTP adapters, Inversion of Control (IoC), MySQL (EF Core), and MongoDB repositories.
* **Presentation (RestApi):** RESTful Controllers that expose and consume data via DTOs.

### Users API Structure (Java)
* **Core:** Contains the domain and application services (`core/domain`, `core/application`), defining abstract repository contracts.
* **Infrastructure:** Concrete persistence implementations (`infrastructure/mysql`, `infrastructure/mongodb`) and JWT security (`infrastructure/security`).
* **Presentation:** Controllers (`presentation/controllers`), object mappers (`presentation/mappers`), and global exception handling.

---

## 🔌 Runtime Database Provider Toggling

A major architectural highlight of this ecosystem is the ability to seamlessly switch database engines (**MySQL <-> MongoDB**) via configuration files, without modifying a single line of business logic.

### 1. Users API Configuration (Java)
Switching is handled via **Spring Profiles** in the `users-api/src/main/resources/application.properties` file. Change the following property:

```properties
# To use MongoDB:
spring.profiles.active=mongodb

# To use MySQL:
spring.profiles.active=mysql
```

### 2. CIS API Configuration (C#)
Switching is handled via environment variables or JSON structure in the .NET configuration file (`appsettings.json`):

```json
{
  "DatabaseConfig": {
    "Provider": "MongoDB" // Valid options: "MongoDB" or "MySQL"
  }
}
```

*Note: Switching providers requires restarting the containers/services to correctly inject the corresponding dependencies.*

---

## 🐳 Environment & Infrastructure (Docker)

Microservices communicate using internal DNS service names within an isolated network.

### 🌐 Shared Network Creation
The Docker virtual network must exist prior to spinning up the ecosystem. Ensure it is created by running:

```bash
docker network create cis-network
```

### 🚀 Starting the Ecosystem
To build the custom images and start the entire physical infrastructure (APIs and Databases) in the background:

```bash
docker compose up --build -d
```

### 🗄️ Database Mapping Strategy

* **Legacy MySQL:** Shared network instance. Single database created: `sd3db` (Used by both `users-api` and `cis-api` via specific tables).
* **Modernized MongoDB:** Shared instance with logical database separation per service:
    * `sd3_users_db` -> Exclusive to Users API.
    * `sd3_cis_db` -> Exclusive to CIS API.

---

## 🌐 Unified API Endpoints Matrix

### 👥 Users API (Identity Provider) - Local Port: `8001`

| Method | Endpoint | Description | Status |
| :--- | :--- | :--- | :--- |
| **GET** | `/api/v1/health` | API health check and integrity validation | Implemented |
| **POST** | `/api/v1/auth/login` | Credential authentication and JWT Token generation | Implemented |
| **POST** | `/api/v1/users` | Register new platform users | Implemented |
| **GET** | `/api/v1/users` | Complete list of registered users | Implemented |
| **GET** | `/api/v1/users/{id}` | Retrieve specific user profile by ID | Implemented |
| **PATCH**| `/api/v1/users/{id}` | Partial user data update (name, password) | Implemented |
| **DELETE**| `/api/v1/users/{id}` | Logical/physical user account removal | Implemented |

### 💡 CIS API (Core Ideation & Voting) - Local Port: `8005`

| Method | Endpoint | Description | Status |
| :--- | :--- | :--- | :--- |
| **POST** | `/api/v1/topics` | Create new general discussion topics | Implemented |
| **GET** | `/api/v1/topics` | List and retrieve active topics | Implemented |
| **POST** | `/api/v1/topics/{topicId}/ideas` | Submit new ideas associated with a topic | Implemented |
| **GET** | `/api/v1/topics/{topicId}/ideas` | List ideas linked to a specific topic | Implemented |
| **POST** | `/api/v1/ideas/{ideaId}/vote` | Register a single vote on a specific idea (Upvote) | Implemented |
| **POST** | `/api/v1/ideas/{ideaId}/unvote` | Withdraw/cancel a previously cast vote | Implemented |

---

## 🔐 Cross-Service Authentication Flow
1. The client consumes the `POST :8001/api/v1/auth/login` endpoint sending credentials.
2. The **Users API** validates and responds with a cryptographic JWT token.
3. To consume the protected routes of the **CIS API (:8005)**, the client must inject the token into the HTTP request headers using the Bearer authentication format:
    * **Key:** `Authorization`
    * **Value:** `Bearer <your_jwt_token>`

---

## 🔄 Data Migration Strategy (Python ETL)

When switching API providers to MongoDB, legacy data residing in MySQL must be migrated. The project features Python-based scripts structured under the **ETL** engineering pattern:

1.  **Extract:** Optimized cursor-based reading of relational rows from MySQL.
2.  **Transform:** Dynamic structural conversion from tabular relational data to NoSQL document formats (Dictionaries/JSON), remapping foreign keys and preserving referential integrity through global identifiers (`userId`, `topicId`, `ideaId`).
3.  **Load:** Organized batch insertion (*Batch Size: 100*) using **Upsert** operations, making the scripts completely **Idempotent** (they can be re-executed without the risk of duplicating data).

The scripts fully support simulations (`--dry-run`) and automated post-migration mathematical checks (`--validate`), comparing the total row count from the source database with the amount of documents generated in MongoDB.

---

## 🧪 Useful Commands & Testing

### Run Unit Tests (Java / Maven)
```bash
cd users-api
mvn test
```

### Run Unit Tests and Coverage (.NET / C#)
```bash
cd cis-api
dotnet test
```

### Direct CLI Inspection inside Containers

**MySQL Console Access:**
```bash
docker exec -it mysql-db mysql -u root -p'root' sd3db
```

**MongoDB Shell (Mongosh) Access:**
```bash
docker exec -it mongodb_ds3 mongosh sd3_cis_db
```