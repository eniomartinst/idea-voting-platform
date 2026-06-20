import argparse
import sys
import mysql.connector
from pymongo import MongoClient

MYSQL_CONFIG = {
    "host": "localhost",
    "user": "root",
    "password": "root",
    "database": "sd3db"
}

MONGO_URI = "mongodb://localhost:27017/"
MONGO_DB = "sd3_users_db"
COLLECTION_NAME = "users"
BATCH_SIZE = 100

def get_mysql_connection():
    try:
        return mysql.connector.connect(**MYSQL_CONFIG)
    except mysql.connector.Error as err:
        print(f"Error connecting to MySQL: {err}")
        sys.exit(1)

def get_mongo_collection():
    try:
        client = MongoClient(MONGO_URI)
        db = client[MONGO_DB]
        return db[COLLECTION_NAME]
    except Exception as err:
        print(f"Error connecting to MongoDB: {err}")
        sys.exit(1)

def extract_users(cursor):
    cursor.execute("SELECT * FROM users")
    for row in cursor:
        yield row

def transform_user(row: dict) -> dict:
    return {
        "_id": row["id"],
        "name": row.get("name"),
        "login": row.get("login"),
        "password": row.get("password")
    }

def migrate(dry_run=False):
    mysql_conn = get_mysql_connection()
    cursor = mysql_conn.cursor(dictionary=True)
    mongo_collection = get_mongo_collection()

    print(f"Starting migration to MongoDB (dry-run: {dry_run})...")
    
    count = 0
    for row in extract_users(cursor):
        doc = transform_user(row)
        
        if not dry_run:
            mongo_collection.update_one(
                {"_id": doc["_id"]},
                {"$set": doc},
                upsert=True
            )
        count += 1
        
        if count % BATCH_SIZE == 0:
            print(f"Processed {count} records...")
            
    print(f"Migration finished. Total records processed: {count}")
    
    cursor.close()
    mysql_conn.close()

def validate():
    mysql_conn = get_mysql_connection()
    cursor = mysql_conn.cursor()
    mongo_collection = get_mongo_collection()
    
    print("Validating migration...")
    
    cursor.execute("SELECT COUNT(*) FROM users")
    mysql_count = cursor.fetchone()[0]
    
    mongo_count = mongo_collection.count_documents({})
    
    print(f"MySQL count: {mysql_count}")
    print(f"MongoDB count: {mongo_count}")
    
    if mysql_count == mongo_count:
        print("Success: Data counts match!")
        sys.exit(0)
    else:
        print("Error: Data mismatch detected!")
        sys.exit(1)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Migrate user data from MySQL to MongoDB")
    parser.add_argument("--dry-run", action="store_true", help="Simulate the migration without writing to MongoDB")
    parser.add_argument("--validate", action="store_true", help="Validate data counts between MySQL and MongoDB")
    
    args = parser.parse_args()
    
    if args.validate:
        validate()
    else:
        migrate(dry_run=args.dry_run)
