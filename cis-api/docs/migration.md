# Data Migration (MySQL to MongoDB)

## 📌 Overview

This document describes the strategy used to migrate data from **MySQL (relational)** to **MongoDB (document-based)** using an ETL approach.

The migration ensures:

- Data integrity
- ID preservation
- Safe re-execution (idempotency)
- Compatibility with the current application

---

## 🎯 Scope

The migration covers the following entities:

- Topics
- Ideas
- Votes

> Users are managed by `users-api` and are **not migrated**.

---

## 🧠 Migration Strategy Decision

The migration is implemented using a **script-based ETL approach (Python)**.

### Why this approach?

- Full control over transformation logic
- No impact on application layers
- Safe to execute outside the API lifecycle
- High reproducibility
- Strong ecosystem support

### Alternatives considered

| Approach            | Reason for rejection                        |
|---------------------|---------------------------------------------|
| API-based migration | Higher risk, tightly coupled to application |
| GUI tools           | Low flexibility and poor reproducibility    |

---

## ⚙️ ETL Strategy

The migration follows the standard ETL pipeline:

### Extract

- Data is queried from MySQL
- Uses streaming via cursor (`dictionary=True`)
- Avoids loading all data into memory

### Transform

- Converts relational rows into MongoDB documents
- Preserves original IDs
- Normalizes field names and structure

### Load

- Inserts data into MongoDB
- Uses `upsert` to avoid duplication
- Supports safe re-execution

---

## 🔁 Idempotent Migration

The migration is designed to be **idempotent**:

- Uses `update_one(..., upsert=True)`
- `_id` is derived from the original SQL ID
- Can be executed multiple times safely

### Benefits

- No duplicate data
- Safe retries
- Supports CI/CD pipelines
- Allows partial migration recovery

---

## 🧩 Data Modeling Strategy

MongoDB collections follow a **reference-based model**:

- `topics`, `ideas`, `votes` are stored separately
- Relationships are maintained via IDs
- No document embedding is used

### Why?

- Avoid large document rewrites
- Improve write performance
- Better scalability (especially for votes)

### Special Case: Votes

Votes use a **composite `_id`**:

```
_id = "ideaId:userId"
```

- Guarantees uniqueness
- Prevents duplicate votes

---

## 🚀 Usage

### Dry-run (simulation)

```bash
python scripts/migrate_cis.py --dry-run
```

- Does not write to MongoDB
- Useful for validation and CI

---

### Execute migration

```bash
python scripts/migrate_cis.py
```

---

### Validate data

```bash
python scripts/migrate_cis.py --validate
```

---

## ✅ Validation Strategy

Validation compares MySQL and MongoDB data:

- Fails if MongoDB is missing records
- Warns if extra records exist
- Passes if all MySQL data is present

### Example output

```
topics: MySQL=4 | Mongo=7
[WARNING] Extra documents detected in topics (+3)
```

### Purpose

- Detect data loss
- Ensure migration completeness
- Allow coexistence with pre-existing Mongo data

---

## ⚠️ MongoDB Behavior

MongoDB uses **lazy database creation**:

- Databases and collections are created only when data is inserted

### Implications

- Empty databases may not appear
- Manual initialization may be required

Example:

```bash
docker exec -it mongodb_ds3 mongosh
```

```bash
use sd3_cis_db
db.createCollection("init_collection")
```

Collections (`topics`, `ideas`, `votes`) will be created later:

- via API usage
- or via ETL migration

---

## ⚠️ Important Notes

- Migration uses **upsert** (no deletions)
- Existing MongoDB data is preserved
- Script is safe to run multiple times
- Data consistency depends on source (MySQL) integrity

---

## 🔮 Future Improvements

- Incremental migration (delta sync)
- Rollback strategy
- CI/CD automation (replace `exit 0`)
- Data consistency checks beyond counts

---

## 💡 Key Insight

> Data migration is not just data transfer.

It requires:

- Structural transformation
- Performance considerations
- Consistency guarantees

The ETL approach enables a controlled evolution from a **relational model** to a **document-based model** while maintaining system reliability.