#!/bin/bash

echo "==================================="
echo "Starting SQL Server Docker Container"
echo "==================================="

# Stop and remove existing container if exists
docker stop server_sql 2>/dev/null
docker rm server_sql 2>/dev/null

# Start SQL Server container
docker run -e "ACCEPT_EULA=Y" \
    -e "SA_PASSWORD=@AAAaaa123123" \
    -e "MSSQL_PID=Developer" \
    -p 1433:1433 \
    --name server_sql \
    -d mcr.microsoft.com/mssql/server:2022-latest

if [ $? -eq 0 ]; then
    echo "✅ SQL Server container started successfully!"
    echo ""
    echo "Waiting for SQL Server to be ready..."
    sleep 20
    
    # Check if SQL Server is ready
    docker exec server_sql /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U SA -P "@AAAaaa123123" -C \
        -Q "SELECT @@VERSION" > /dev/null 2>&1
    
    if [ $? -eq 0 ]; then
        echo "✅ SQL Server is ready!"
        echo ""
        echo "Connection Info:"
        echo "  Server: localhost,1433"
        echo "  User: SA"
        echo "  Password: @AAAaaa123123"
        echo ""
        echo "Run next: ./02-create-database.sh"
    else
        echo "⚠️  SQL Server is starting... please wait a few more seconds"
        echo "Then run: ./02-create-database.sh"
    fi
else
    echo "❌ Failed to start SQL Server container"
    exit 1
fi
