#!/bin/bash

echo "========================================="
echo "🚀 DATN-SneakerPoly Database Setup"
echo "========================================="
echo ""

# Change to script directory
cd "$(dirname "$0")"

# Make scripts executable
chmod +x 01-start-docker.sh
chmod +x 02-create-database.sh
chmod +x 03-scaffold-efcore.sh

# Run all steps
echo "Step 1/3: Starting Docker SQL Server..."
./01-start-docker.sh
if [ $? -ne 0 ]; then
    echo "❌ Failed at step 1"
    exit 1
fi

echo ""
echo "Step 2/3: Creating Database and Schema..."
./02-create-database.sh
if [ $? -ne 0 ]; then
    echo "❌ Failed at step 2"
    exit 1
fi

echo ""
echo "Step 3/3: Scaffolding EF Core Models..."
./03-scaffold-efcore.sh
if [ $? -ne 0 ]; then
    echo "❌ Failed at step 3"
    exit 1
fi

echo ""
echo "========================================="
echo "✅ Setup completed successfully!"
echo "========================================="
echo ""
echo "Database Info:"
echo "  Server: localhost,1433"
echo "  Database: SneakerShop"
echo "  User: SA"
echo "  Password: @AAAaaa123123"
echo ""
echo "Connection String:"
echo "  Server=localhost,1433;Database=SneakerShop;User Id=SA;Password=@AAAaaa123123;TrustServerCertificate=True"
echo ""
