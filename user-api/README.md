# Users API (Java + Spring Boot)

## 📌 Description

This project provides a **Users API** built with **Java and Spring Boot**, following **Clean Architecture** principles and best practices.

It manages core features such as **user registration, authentication (JWT), and profile management**, serving as the foundational identity service for the CIS (Crowdsourced Ideation Solution) ecosystem.

The goal is to maintain a standardized, scalable architecture, ensuring separation of concerns and seamless integration with multiple persistence layers (MySQL and MongoDB).

---

## 🚀 Technologies

- Java 21
- Spring Boot 3
- Maven
- MySQL 8 (Relational Database)
- MongoDB (NoSQL Database)
- JWT (JSON Web Tokens)
- Docker & Docker Compose

---

## 🧱 Architecture Overview

The project follows a strict layered architecture, enabling the implementation of multiple database infrastructures without affecting domain rules:

    src/main/java/jalau/usersapi
    ├── core
    │   ├── application        # Application services (Use cases & Orchestration)
    │   ├── domain             # Domain entities and Repository/Service Interfaces
    │   └── exception          # Core domain exceptions
    ├── infrastructure
    │   ├── mongodb            # MongoDB Data Persistence Implementation
    │   ├── mysql              # MySQL Data Persistence Implementation
    │   └── security           # JWT Token Provider implementation
    └── presentation
        ├── controllers        # REST API Controllers
        ├── dtos               # Data Transfer Objects (Request/Response)
        ├── exceptions         # Global Exception Handler (Middleware)
        └── mappers            # Object mapping logic

---

## 🗄️ Database Strategy & Toggling (MySQL vs MongoDB)

A key architectural feature of this API is its ability to switch between a Relational Database (MySQL) and a Document Database (MongoDB) seamlessly, utilizing **Spring Profiles** and **Dependency Injection**.

Both databases run simultaneously within the `cis-network` via Docker.

### How to switch databases:

You can switch the active database at any time without changing any business logic.
Simply open `src/main/resources/application.properties` and change the `spring.profiles.active` property:

### Here you decide which database to run on: Input --- mysql --- or --- mongodb ---
**To use MongoDB:**
spring.profiles.active=mongodb

**To use MySQL:**
spring.profiles.active=mysql

*Note: After changing the profile, restart the Docker container (`docker compose up --build`) for the changes to take effect.*

---

## 🐳 Docker & Environment Setup

The project uses **Docker** to facilitate the setup of the API and both databases.

### 🌐 Shared Docker Network
This API shares a Docker network with the CIS API to allow secure internal communication.
Ensure the network exists before running the containers:

    docker network create cis-network

### 🚀 Running the Project

First execution (builds the image):
docker compose up --build

Standard execution:
docker compose up

To stop containers:
docker compose down

The application will be available at: `http://localhost:8001`

---

## 🌐 API Endpoints

### Health
| Method | Endpoint | Description | Status |
|--------|----------|-------------|--------|
| GET | `http://localhost:8001/api/v1/health` | API Health check | Implemented |

### Authentication
| Method | Endpoint | Description | Status |
|--------|----------|-------------|--------|
| POST | `http://localhost:8001/api/v1/auth/login` | Authenticate user and generate JWT token | Implemented |

### Users Management
| Method | Endpoint | Description | Status |
|--------|----------|-------------|--------|
| POST | `http://localhost:8001/api/v1/users` | Create a new user | Implemented |
| GET | `http://localhost:8001/api/v1/users` | List all users | Implemented |
| GET | `http://localhost:8001/api/v1/users/{id}` | Get user by ID | Implemented |
| PATCH | `http://localhost:8001/api/v1/users/{id}` | Update user (Partial update: name, password) | Implemented |
| DELETE | `http://localhost:8001/api/v1/users/{id}` | Delete a user | Implemented |

---

## 🧪 Testing the API

### Authentication Workflow (Postman)
1. Send a `POST` request to `/api/v1/auth/login` with your credentials (`login` and `password`).
2. Copy the generated `token` from the response.
3. For protected routes (like CIS API endpoints), add the token to the **Headers**:
   - **Key:** `Authorization`
   - **Value:** `Bearer <your_token>`

---

## 📌 Current Project Status

- [x] Clean Architecture implemented
- [x] Docker & `cis-network` integration
- [x] Global Exception Handler implemented
- [x] JWT Authentication & Security
- [x] CRUD operations fully functional
- [x] MongoDB Infrastructure integrated alongside MySQL