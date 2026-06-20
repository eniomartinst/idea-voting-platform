"""
CIS API ETL Migration Script (MySQL to MongoDB)

Entities:
- Topics
- Ideas
- Votes

Features:
- Idempotent (upsert)
- Batch processing
- Dry-run mode
- Validation
- Preserves IDs
"""

import mysql.connector
from pymongo import MongoClient
import argparse

# =========================
# CONFIG
# =========================

MYSQL_CONFIG = {
  "host": "localhost",
  "user": "root",
  "password": "root",
  "database": "sd3db"
}

MONGO_URI = "mongodb://localhost:27017/"
MONGO_DB = "sd3_cis_db"

BATCH_SIZE = 100


# =========================
# CONNECTIONS
# =========================

def get_mysql_connection():
  return mysql.connector.connect(**MYSQL_CONFIG)


def get_mongo_db():
  client = MongoClient(MONGO_URI)
  return client[MONGO_DB]


# =========================
# GENERIC LOAD
# =========================

def load_batch(collection, batch, dry_run=False):
  if dry_run:
    print(f"[DRY-RUN] Would process {len(batch)} docs in {collection.name}")
    return

  for doc in batch:
    collection.update_one(
      {"_id": doc["_id"]},
      {"$set": doc},
      upsert=True
    )

# =========================
# TOPICS
# =========================

def migrate_topics(db, cursor, dry_run):
  print("\n>>> Migrating TOPICS...")

  collection = db["topics"]
  cursor.execute("SELECT id, title, description, created_at, user_id FROM topics")

  batch = []
  total = 0

  for row in cursor:
    doc = {
      "_id": row["id"],
      "title": row["title"],
      "description": row["description"],
      "createdAt": row["created_at"],
      "userId": row["user_id"]
    }

    batch.append(doc)

    if len(batch) >= BATCH_SIZE:
      load_batch(collection, batch, dry_run)
      total += len(batch)
      batch = []

  if batch:
    load_batch(collection, batch, dry_run)
    total += len(batch)

  print(f">>> TOPICS done: {total}")


# =========================
# IDEAS
# =========================

def migrate_ideas(db, cursor, dry_run):
  print("\n>>> Migrating IDEAS...")

  collection = db["ideas"]
  cursor.execute("""
                 SELECT id, content, votes_count, created_at, topic_id, user_id
                 FROM ideas
                 """)

  batch = []
  total = 0

  for row in cursor:
    doc = {
      "_id": row["id"],
      "content": row["content"],
      "votesCount": row["votes_count"],  # cache
      "createdAt": row["created_at"],
      "topicId": row["topic_id"],
      "userId": row["user_id"]
    }

    batch.append(doc)

    if len(batch) >= BATCH_SIZE:
      load_batch(collection, batch, dry_run)
      total += len(batch)
      batch = []

  if batch:
    load_batch(collection, batch, dry_run)
    total += len(batch)

  print(f">>> IDEAS done: {total}")


# =========================
# VOTES
# =========================

def migrate_votes(db, cursor, dry_run):
  print("\n>>> Migrating VOTES...")

  collection = db["votes"]
  cursor.execute("SELECT idea_id, user_id, created_at FROM votes")

  batch = []
  total = 0

  for row in cursor:
    doc = {
      "_id": f"{row['idea_id']}:{row['user_id']}",
      "ideaId": row["idea_id"],
      "userId": row["user_id"],
      "createdAt": row["created_at"]
    }

    batch.append(doc)

    if len(batch) >= BATCH_SIZE:
      load_batch(collection, batch, dry_run)
      total += len(batch)
      batch = []

  if batch:
    load_batch(collection, batch, dry_run)
    total += len(batch)

  print(f">>> VOTES done: {total}")


# =========================
# VALIDATION
# =========================

def validate(db, cursor):
  print("\n>>> VALIDATION")

  entities = [
    ("topics", "topics"),
    ("ideas", "ideas"),
    ("votes", "votes")
  ]

  for sql_table, mongo_collection in entities:
    cursor.execute(f"SELECT COUNT(*) AS count FROM {sql_table}")
    mysql_count = cursor.fetchone()["count"]

    mongo_count = db[mongo_collection].count_documents({})

    print(f"{sql_table}: MySQL={mysql_count} | Mongo={mongo_count}")

    if mongo_count < mysql_count:
      raise Exception(f"[ERROR] Missing data in {sql_table}!")

    elif mongo_count > mysql_count:
      print(f"[WARNING] Extra documents detected in {sql_table} (+{mongo_count - mysql_count})")

print(">>> VALIDATION COMPLETED")


# =========================
# MAIN
# =========================

def main(dry_run=False, validate_flag=False):
  mysql_conn = get_mysql_connection()
  cursor = mysql_conn.cursor(dictionary=True)

  db = get_mongo_db()

  print(">>> STARTING CIS MIGRATION")

  migrate_topics(db, cursor, dry_run)
  migrate_ideas(db, cursor, dry_run)
  migrate_votes(db, cursor, dry_run)

  if validate_flag:
    validate(db, cursor)

  cursor.close()
  mysql_conn.close()

  print("\n>>> MIGRATION FINISHED")


if __name__ == "__main__":
  parser = argparse.ArgumentParser()
  parser.add_argument("--dry-run", action="store_true")
  parser.add_argument("--validate", action="store_true")

  args = parser.parse_args()

  main(dry_run=args.dry_run, validate_flag=args.validate)