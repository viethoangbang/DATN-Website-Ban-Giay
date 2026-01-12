#!/bin/bash

echo "==================================="
echo "Creating Admin Account"
echo "==================================="

DB_NAME="SneakerShop"
SA_PASSWORD="@AAAaaa123123"

# BCrypt hash for "Admin123" (cost factor 11)
# This is a valid BCrypt hash, salt is randomly generated each time
HASHED_PASSWORD='$2a$11$0RfN1ymHQSs./TR6srdQAeuGo4D54shpb7yVvH6.AG1X.LOy.RwBi'

echo "Email: admin@sneakerpoly.com"
echo "Password: Admin123"
echo ""

# Check if SQL Server is running
if ! docker ps | grep -q server_sql; then
    echo "❌ SQL Server container is not running!"
    exit 1
fi

# Create admin account
docker exec server_sql /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U SA -P "$SA_PASSWORD" -C \
    -d "$DB_NAME" \
    -Q "
-- Insert Admin account
DECLARE @AdminEmail VARCHAR(255) = 'admin@sneakerpoly.com';
DECLARE @AdminPassword VARCHAR(255) = '$HASHED_PASSWORD';
DECLARE @AccountID INT;
DECLARE @AdminRoleID INT;

-- Check if admin already exists
IF NOT EXISTS (SELECT 1 FROM Account WHERE Email = @AdminEmail)
BEGIN
    -- Insert account
    INSERT INTO Account (Email, [Password], FullName, PhoneNumber, Status, CreateDate)
    VALUES (@AdminEmail, @AdminPassword, N'Administrator', '0123456789', 'Active', GETDATE());
    
    SET @AccountID = SCOPE_IDENTITY();
    
    -- Get Admin role ID
    SELECT @AdminRoleID = ID FROM [Role] WHERE Name = 'Admin';
    
    -- Assign Admin role
    IF @AdminRoleID IS NOT NULL
    BEGIN
        INSERT INTO AccountRole (AccountID, RoleID)
        VALUES (@AccountID, @AdminRoleID);
        
        SELECT 'Admin account created successfully!' AS Result;
        SELECT * FROM Account WHERE ID = @AccountID;
    END
    ELSE
    BEGIN
        SELECT 'Error: Admin role not found!' AS Result;
    END
END
ELSE
BEGIN
    SELECT 'Admin account already exists!' AS Result;
    SELECT * FROM Account WHERE Email = @AdminEmail;
END
"

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Done!"
    echo ""
    echo "Login credentials:"
    echo "  Email: admin@sneakerpoly.com"
    echo "  Password: Admin123"
else
    echo "❌ Failed to create admin account"
    exit 1
fi
