db = db.getSiblingDB('sd3_cis_db');
db.createCollection('init_collection');

db = db.getSiblingDB('sd3_users_db');
db.createCollection('init_collection');
