# User Data Ownership & Integration

## 🧩 Responsibility Boundaries

The system follows a **clear separation of ownership between services**:

| Service     | Responsibility                              |
|-------------|---------------------------------------------|
| `users-api` | Owns and manages **user data**              |
| `cis-api`   | Consumes user information (does NOT own it) |

---

## 🐬 MySQL Behavior (Shared Database)

Although both APIs use the same MySQL database (`sd3db`):

- `cis-api` **does NOT manage or modify** the `users` table
- No foreign key enforcement at database level
- `userId` is treated as a **reference only**

> This avoids tight coupling despite shared storage

---

## 🔐 Authentication Flow

1. User authenticates via `users-api`
2. A **JWT token** is generated
3. The client sends the token to `cis-api`
4. `cis-api`:
   - Validates the token
   - Extracts `userId` from claims

> No direct database lookup required for authentication

---

## 🔎 User Data Enrichment

When additional user data is needed:

- `cis-api` performs a **GET request** to `users-api`

Example:

```
GET /api/v1/users/{userId}
```

**Ensures**:

- Data consistency
- Single source of truth
- Decoupled services

---

## 🧠 Design Decisions

| Concern        | Decision            | Reason                      |
|----------------|---------------------|-----------------------------|
| User ownership | users-api only      | Single source of truth      |
| CIS access     | via HTTP (GET)      | Loose coupling              |
| Auth           | JWT-based           | Stateless + scalable        |
| Shared DB risk | Avoid direct access | Prevent hidden dependencies |

---

### ⚠️ Important Notes

- Even with a **shared MySQL database**, services remain **logically independent**
- Direct queries to `users` table from `cis-api` are **forbidden by design**
- MongoDB maintains the same principle:
  - `sd3_users_db` → users-api
  - `sd3_cis_db` → cis-api

---

## 💡 Key Insight

> Sharing a database does NOT mean sharing ownership.

The system enforces **logical boundaries at application level**, ensuring scalability and maintainability.