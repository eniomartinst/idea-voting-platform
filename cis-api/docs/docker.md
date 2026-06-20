# Docker Environment & Setup

## 📦 Overview

The system runs as a **microservices architecture** composed of:

- `cis-api` → C# / ASP.NET Core
- `users-api` → Spring Boot
- `mysql-db` → shared relational database
- `mongodb` → shared MongoDB instance (isolated databases per service)

All services communicate through a shared Docker network.

---

## 🌐 Shared Network

Before starting the environment, create the external network:

```bash
docker network create cis-network
```

Verify:

```bash
docker network ls
```

---

## 🗄️ Database Architecture

### 🐬 MySQL (Shared Database)

- Database: `sd3db`
- Used by:
  - `users-api`
  - `cis-api`

**Characteristics:**

- Shared schema
- Supports relational features
- Used for legacy/structured data

---

### 🍃 MongoDB (Shared Instance, Isolated Databases)

- Single container: `mongodb_ds3`
- Logical separation:

| Service   | Database       |
|-----------|----------------|
| users-api | `sd3_users_db` |
| cis-api   | `sd3_cis_db`   |

**Characteristics:**

- Same Mongo instance
- Data isolation per service
- Lower infrastructure overhead

---

## 🧱 Docker Compose (Users API + Databases)

```yaml
services:
  users-api:
    build: .
    container_name: users-api
    restart: always
    ports:
      - "8001:8001"
    depends_on:
      mysql-db:
        condition: service_healthy
      mongodb:
        condition: service_started
    environment:
      SPRING_DATASOURCE_URL: jdbc:mysql://mysql-db:3306/sd3db
      SPRING_DATASOURCE_USERNAME: root
      SPRING_DATASOURCE_PASSWORD: root
      SPRING_DATA_MONGODB_URI: mongodb://mongodb:27017/sd3_users_db
    networks:
      - cis-network

  mysql-db:
    image: mysql:8.0
    container_name: mysql-db
    restart: always
    environment:
      MYSQL_ROOT_PASSWORD: root
      MYSQL_DATABASE: sd3db
    ports:
      - "3306:3306"
    volumes:
      - ./init.sql:/docker-entrypoint-initdb.d/init.sql
    networks:
      - cis-network
    healthcheck:
      test: ["CMD","mysqladmin","ping","-h","localhost"]
      timeout: 20s
      retries: 10

  mongodb:
    image: mongo:latest
    container_name: mongodb_ds3
    restart: always
    environment:
      MONGO_INITDB_DATABASE: sd3_users_db
    ports:
      - "27017:27017"
    volumes:
      - mongo_data:/data/db
    networks:
      - cis-network

networks:
  cis-network:
    external: true

volumes:
  mongo_data:
```

---

## ⚙️ CIS API Integration

The `cis-api` **does not provision databases**, it only connects to them:

```yaml
services:
  cis-api:
    build: .
    container_name: cis-api
    ports:
      - "8005:8005"
    environment:
      DatabaseConfig__Provider: "MongoDB"
      ConnectionStrings__CISDB: "server=mysql-db;port=3306;database=sd3db;user=root;password=root"
      ConnectionStrings__MongoDB: "mongodb://mongodb:27017"
    networks:
      - cis-network
```

---

## 🔌 Connection Strategy

### MySQL

```
server=mysql-db;port=3306;database=sd3db;user=root;password=root
```

### MongoDB

```
mongodb://mongodb:27017
```

Database usage is defined at application level:

- `users-api` → `sd3_users_db`
- `cis-api` → `sd3_cis_db`

---

## ⚠️ MongoDB Initialization (CIS API)

MongoDB creates databases lazily, so the CIS database must be initialized manually.

After starting containers:

```bash
docker exec -it mongodb_ds3 mongosh
```

```bash
use sd3_cis_db
db.createCollection("init_collection")
```

- Required only once 
- Enables the database to be recognized

Collections (`topics`, `ideas`, `votes`) are created later by:

- API usage
- ETL migration

---

## 🔄 Data Persistence

### MySQL

- Initialized via `init.sql`
- Persistent across restarts

### MongoDB

- Uses named volume:

```yaml
volumes:
  mongo_data:
```

- Prevents data loss

---

## 🔁 Startup Order

Recommended order:

1. `mysql-db`
2. `mongodb`
3. `users-api`
4. `cis-api`

---

## 🚀 Running the Environment

Start:

```bash
docker compose up --build
```

Stop:

```bash
docker compose down
```

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

Select the CIS database:

```bash
use sd3_cis_db
show collections
```

---

## 🧠 Key Design Decisions

| Concern        | Decision             | Reason                          |
|----------------|----------------------|---------------------------------|
| MySQL          | Shared               | Maintain relational consistency |
| MongoDB        | Isolated per service | Avoid coupling                  |
| Mongo instance | Single container     | Reduce infra cost               |
| Network        | Shared               | Simplify communication          |
| Mongo init     | Manual               | Mongo lazy creation behavior    |

---

## ⚠️ Important Notes

- MySQL is **shared across services**
- MongoDB is **logically isolated per API**
- `cis-api` depends on external database containers
- MongoDB does **not enforce schema**
- ETL is required for data migration