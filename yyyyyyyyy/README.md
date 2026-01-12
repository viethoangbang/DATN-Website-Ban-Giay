# 🚀 Database Setup Scripts

Scripts tự động để setup SQL Server Docker và scaffold EF Core models.

## 📋 Prerequisites

- Docker Desktop đã cài đặt và đang chạy
- .NET SDK (để chạy EF Core scaffolding)
- Backend project có cài các packages:
  ```bash
  dotnet add package Microsoft.EntityFrameworkCore.SqlServer
  dotnet add package Microsoft.EntityFrameworkCore.Design
  ```

## 🎯 Quick Start

### Cách 1: Chạy tất cả tự động

```bash
cd yyyyyyyyy
./run-all.sh
```

### Cách 2: Chạy từng bước

```bash
# Bước 1: Start Docker SQL Server
./01-start-docker.sh

# Bước 2: Tạo database và chạy schema
./02-create-database.sh

# Bước 3: Scaffold EF Core models
./03-scaffold-efcore.sh
```

## 📁 Scripts

- `01-start-docker.sh` - Start SQL Server container
- `02-create-database.sh` - Tạo database SneakerShop và chạy schema
- `03-scaffold-efcore.sh` - Scaffold models vào Backend/Data/Models
- `run-all.sh` - Chạy tất cả 3 scripts trên
- `scheme.sql` - Database schema

## 🔑 Connection Info

- **Server**: `localhost,1433`
- **Database**: `SneakerShop`
- **User**: `SA`
- **Password**: `@AAAaaa123123`
- **Connection String**: 
  ```
  Server=localhost,1433;Database=SneakerShop;User Id=SA;Password=@AAAaaa123123;TrustServerCertificate=True
  ```

## 📊 Database Schema

### Tables (11 tables):
1. **Account** - Người dùng
2. **Role** - Vai trò
3. **AccountRole** - Phân quyền
4. **Brand** - Thương hiệu (NEW - tách riêng)
5. **Category** - Danh mục sản phẩm
6. **Product** - Sản phẩm
7. **ProductVariant** - Biến thể (màu, size, giá)
8. **Address** - Địa chỉ giao hàng
9. **Order** - Đơn hàng
10. **OrderDetail** - Chi tiết đơn hàng
11. **OrderStatusHistory** - Lịch sử trạng thái đơn hàng

### Key Changes từ schema cũ:
- ✅ Tách **Brand** thành bảng riêng
- ❌ Xóa Cart, CartDetail (không cần persistent cart)
- ❌ Xóa Voucher, Discount (quá phức tạp)
- ❌ Xóa Payment, Shipment (implement sau)
- ❌ Xóa Image, BrandBanner (dùng URL string)
- ✅ Đơn giản hóa ProductDetail → ProductVariant
- ✅ Thêm indexes cho performance
- ✅ Thêm initial data (roles, brands, categories)

## 🛠️ Useful Commands

### Docker
```bash
# Stop SQL Server
docker stop server_sql

# Start SQL Server
docker start server_sql

# Remove container
docker stop server_sql && docker rm server_sql

# Access SQL Server CLI
docker exec -it server_sql /opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "@AAAaaa123123" -C
```

### Database
```bash
# Connect to database
USE SneakerShop;
GO

# List all tables
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';
GO

# Drop database (nếu cần reset)
DROP DATABASE SneakerShop;
GO
```

### EF Core
```bash
# Re-scaffold models
cd Backend/Data
dotnet ef dbcontext scaffold "Server=localhost,1433;Database=SneakerShop;User Id=SA;Password=@AAAaaa123123;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models --context SneakerShopContext --force
```

## 📝 Notes

- Container name: `server_sql`
- Port: `1433`
- SQL Server version: `2022-latest`
- Data Project: `Backend/Data/`
- Models output: `Backend/Data/Models/`
- Context output: `Backend/Data/SneakerShopContext.cs`
