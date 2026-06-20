# Database Strategy & Provider Switching

## 📌 Overview

The CIS API supports two persistence providers:

- MySQL (relational)
- MongoDB (document-based)

The provider is selected at runtime via configuration, allowing flexibility without changing application code.

---

## ⚙️ Configuration

The active database is defined using:

```yaml
DatabaseConfig__Provider: "MongoDB"  # or "MySQL"
```

### Connection Strings

```yaml
  # MongoDB
  ConnectionStrings__MongoDB: "mongodb://mongodb:27017"
  
  # MySQL
  ConnectionStrings__CISDB: "server=mysql-db;port=3306;database=sd3db;user=root;password=root"
```

---

## 🔄 Runtime Behavior

At application startup:

- The configured provider is read from `DatabaseConfig`
- The corresponding infrastructure layer is registered:
  - MongoDB → `AddMongoInfrastructureServices`
  - MySQL → `AddMySqlInfrastructureServices`

This enables:

- Same domain and application layers
- Different persistence implementations

---

## 🗄️ MySQL Behavior

- Uses Entity Framework Core
- Schema defined via mappings
- Migrations are applied automatically on startup

### Key characteristics

- Strong consistency
- Structured schema
- Relationships enforced via foreign keys

---

## 🍃 MongoDB Behavior

- Uses collections (`topics`, `ideas`, `votes`)
- No schema enforcement
- Collections are created dynamically

### Key characteristics

- Flexible schema
- High scalability
- Relationships handled via references

---

## 🔁 Data Migration

Switching from MySQL to MongoDB requires explicit data migration.

This project provides a **Python ETL script**:

- Extracts data from MySQL
- Transforms into document model
- Loads into MongoDB using upsert

📄 Details: [migration.md](migration.md)

---

## ⚠️ Important Considerations

- Switching providers does NOT migrate data automatically
- MongoDB does not enforce constraints (handled at application level)
- MySQL and MongoDB can coexist, but only one is active

---

## 🧠 Design Decision

The project uses a **provider-based architecture** to:

- Decouple domain from persistence
- Allow experimentation with NoSQL
- Support gradual migration strategies

---

## 🔮 Future Improvements

- Automated migration in CI/CD
- Incremental (delta) migration
- Dual-write strategy (optional)
- Data consistency monitoring