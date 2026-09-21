# 💡 Idea Voting Platform (Monorepo)

> **A highly resilient, distributed backend ecosystem and modern web application for Crowdsourced Ideation, featuring polyglot microservices, Clean Architecture, runtime database provider toggling, and automated NoSQL data migration.**

---

## 🛠️ Technology Stack

The ecosystem is distributed across polyglot microservices and a responsive single-page web frontend:

* **Frontend Web Application:** Angular (Standalone Components), TypeScript, RxJS, Reactive Forms, SCSS (Custom High-Contrast UI).
* **Identity Service (Users API):** Java 21, Spring Boot 3, Spring Security, Maven, JUnit 5, JaCoCo.
* **Core Ideation Service (CIS API):** C#, .NET 9, ASP.NET Core Web API, Entity Framework Core, MongoDB Driver, xUnit, Moq, Coverlet.
* **Persistence Layers:** MySQL 8 (Relational) & MongoDB (NoSQL).
* **Infrastructure & Automation:** Docker & Docker Compose, Python 3 (ETL Migration Scripts).
* **Security:** Stateless JWT Authentication with automated Angular HttpInterceptors.

---

## 📂 Monorepo Structure

```text
idea-voting-platform/
│
├── web-app/                 # Frontend Web Application (Angular)
│   ├── src/app/core/        # HTTP Services, JWT Interceptors, and strict DTO Models
│   └── src/app/features/    # Standalone UI Components (Auth, Dashboard, Idea Details)
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

Both APIs strictly adhere to the separation of concerns principle through layered architectures, while the frontend consumes them via strongly typed models:

### Web App Structure (Angular)
* **Features:** Modular, standalone components separated by business domains (`auth`, `ideas`). Features reactive forms and isolated state management.
* **Core:** Centralized HTTP communication mapped exactly to backend DTOs. Secures routes via a functional HTTP Interceptor, automatically injecting the JWT token into every outgoing request to protected APIs.

### CIS API Structure (C#)
* **Core:** Contains the *Domain* (Entities and Contracts) and *Application* (Use Cases and Services) layers following the CQRS pattern, fully pure and framework-agnostic.
* **Infrastructure:** Implementations for HTTP adapters, Inversion of Control (IoC), MySQL (EF Core), and MongoDB repositories.
* **Presentation (RestApi):** RESTful Controllers that expose and consume data via DTOs.

### Users API Structure (Java)
* **Core:** Contains the domain and application services, defining abstract repository contracts.
* **Infrastructure:** Concrete persistence implementations and JWT security logic.
* **Presentation:** Controllers, object mappers, and global exception handling.

---

## 🔌 Runtime Database Provider Toggling

A major architectural highlight of this ecosystem is the ability to seamlessly switch database engines (**MySQL <-> MongoDB**) via configuration files, without modifying a single line of business logic.

### 1. Users API Configuration (Java)
Switching is handled via **Spring Profiles** in the `users-api/src/main/resources/application.properties` file:
```properties
spring.profiles.active=mongodb # Or 'mysql'
```

### 2. CIS API Configuration (C#)
Switching is handled via environment variables or JSON structure in `appsettings.json`:
```json
{
  "DatabaseConfig": {
    "Provider": "MongoDB" // Valid options: "MongoDB" or "MySQL"
  }
}
```

---

## 🐳 Environment & Infrastructure (Docker)

Microservices communicate using internal DNS service names within an isolated network.

### 🌐 Shared Network Creation
The Docker virtual network must exist prior to spinning up the ecosystem:
```bash
docker network create cis-network
```

### 🚀 Starting the Ecosystem
To build the custom images and start the physical infrastructure (APIs and Databases) in the background:
```bash
docker compose up --build -d
```

### 🗄️ Database Mapping Strategy
* **Legacy MySQL:** Shared network instance (`sd3db`).
* **Modernized MongoDB:** Shared instance with logical database separation per service (`sd3_users_db` and `sd3_cis_db`).

---

## 🌐 Unified API Endpoints Matrix

### 👥 Users API (Identity Provider) - Local Port: `8001`
* `POST /api/v1/auth/login` - Authenticate and generate JWT
* `POST /api/v1/users` - Register new platform users
* *(Standard GET, PATCH, DELETE operations for user management)*

### 💡 CIS API (Core Ideation & Voting) - Local Port: `8005`
* `POST & GET /api/v1/topics` - Manage discussion topics
* `POST & GET /api/v1/topics/{topicId}/ideas` - Manage ideas linked to specific topics
* `POST /api/v1/ideas/{ideaId}/vote` - Register an upvote
* `POST /api/v1/ideas/{ideaId}/unvote` - Withdraw a vote

---

## 🔐 Cross-Service Authentication Flow
1. The Angular client submits credentials to `POST :8001/api/v1/auth/login`.
2. The **Users API** validates and responds with a cryptographic JWT token.
3. The Angular `authInterceptor` captures this token from `localStorage` and securely injects it as a `Bearer` header into all subsequent requests targeting the **CIS API (:8005)**.

---

## 🔄 Data Migration Strategy (Python ETL)
Legacy data residing in MySQL can be seamlessly migrated to MongoDB using the Python ETL scripts:
1. **Extract:** Optimized cursor-based reading from MySQL.
2. **Transform:** Structural conversion from relational to NoSQL document formats.
3. **Load:** Organized batch insertion using Upsert operations, making the scripts completely **Idempotent**.

---

## 👥 Core Team & Contributors
This ecosystem is architected, developed, and maintained by:
* **Ênio Martins** - Full Stack Development & Architecture
* **Arthur Pereira** - Backend & Integration
* **Christopher Allan** - Backend Engineering
* **Davi** - Core System Architecture
* **Marcos La Santrer** - Database & Data Flow
* **Pedro** - Infrastructure & DevOps
* **Vinicius** - Security Implementation

---

## 🧪 Useful Commands & Testing

**Start Angular Frontend:**
```bash
cd web-app
ng serve -o
```

**Run Unit Tests (Java / Maven):**
```bash
cd users-api
mvn test
```

**Run Unit Tests and Coverage (.NET / C#):**
```bash
cd cis-api
dotnet test
```

## 👨‍💻 Autor

**Ênio Martins**
*Full Stack Developer*

* [GitHub](https://github.com/eniomartinst)