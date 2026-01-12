#!/bin/bash

echo "==================================="
echo "Creating Database and Running Schema"
echo "==================================="

DB_NAME="SneakerShop"
SA_PASSWORD="@AAAaaa123123"

# Check if SQL Server is running
if ! docker ps | grep -q server_sql; then
    echo "❌ SQL Server container is not running!"
    echo "Run: ./01-start-docker.sh first"
    exit 1
fi

echo "Creating database: $DB_NAME"

# Create database
docker exec server_sql /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U SA -P "$SA_PASSWORD" -C \
    -Q "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '$DB_NAME') CREATE DATABASE $DB_NAME"

if [ $? -eq 0 ]; then
    echo "✅ Database created successfully!"
else
    echo "❌ Failed to create database"
    exit 1
fi

echo ""
echo "Running schema script..."

# Copy schema file to container
docker cp scheme.sql server_sql:/tmp/scheme.sql

# Run schema script
docker exec server_sql /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U SA -P "$SA_PASSWORD" -C \
    -d "$DB_NAME" \
    -i /tmp/scheme.sql

if [ $? -eq 0 ]; then
    echo "✅ Schema applied successfully!"
    echo ""
    echo "Checking tables..."
    docker exec server_sql /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U SA -P "$SA_PASSWORD" -C \
        -d "$DB_NAME" \
        -Q "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME"
    echo ""
    echo "Run next: ./03-scaffold-efcore.sh"
else
    echo "❌ Failed to apply schema"
    exit 1
fi
