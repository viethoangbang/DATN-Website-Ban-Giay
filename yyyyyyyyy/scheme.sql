-- ===================================
-- CLEAN DATABASE SCHEMA - SNEAKER SHOP
-- ===================================

-- 1. ACCOUNT & ROLE MANAGEMENT
CREATE TABLE dbo.Account (
    ID INT IDENTITY(1,1) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    [Password] VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(30) NULL,
    FullName NVARCHAR(255) NOT NULL,
    AvatarUrl VARCHAR(1000) NULL,
    Status VARCHAR(50) NOT NULL DEFAULT 'Active',
    CreateDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdateDate DATETIME NULL,

    CONSTRAINT PK_Account PRIMARY KEY (ID),
    CONSTRAINT UQ_Account_Email UNIQUE (Email)
);

CREATE TABLE dbo.[Role] (
    ID INT IDENTITY(1,1) NOT NULL,
    Name VARCHAR(50) NOT NULL,
    Description NVARCHAR(255) NULL,

    CONSTRAINT PK_Role PRIMARY KEY (ID),
    CONSTRAINT UQ_Role_Name UNIQUE (Name)
);

CREATE TABLE dbo.AccountRole (
    ID INT IDENTITY(1,1) NOT NULL,
    AccountID INT NOT NULL,
    RoleID INT NOT NULL,

    CONSTRAINT PK_AccountRole PRIMARY KEY (ID),
    CONSTRAINT UQ_AccountRole UNIQUE (AccountID, RoleID),
    CONSTRAINT FK_AccountRole_Account 
        FOREIGN KEY (AccountID) REFERENCES dbo.Account(ID) ON DELETE CASCADE,
    CONSTRAINT FK_AccountRole_Role
        FOREIGN KEY (RoleID) REFERENCES dbo.[Role](ID) ON DELETE CASCADE
);

-- 2. BRAND (NEW TABLE)
CREATE TABLE dbo.Brand (
    ID INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    LogoUrl VARCHAR(1000) NULL,

    CONSTRAINT PK_Brand PRIMARY KEY (ID),
    CONSTRAINT UQ_Brand_Name UNIQUE (Name)
);

-- 3. CATEGORY
CREATE TABLE dbo.Category (
    ID INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(500) NULL,

    CONSTRAINT PK_Category PRIMARY KEY (ID),
    CONSTRAINT UQ_Category_Name UNIQUE (Name)
);

-- 4. IMAGE
CREATE TABLE dbo.[Image] (
    ID INT IDENTITY(1,1) NOT NULL,
    Url VARCHAR(1000) NOT NULL,

    CONSTRAINT PK_Image PRIMARY KEY (ID)
);

-- 5. PRODUCT MANAGEMENT
CREATE TABLE dbo.Product (
    ID INT IDENTITY(1,1) NOT NULL,
    Code VARCHAR(100) NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(1000) NULL,
    BrandID INT NOT NULL,
    CategoryID INT NOT NULL,
    Status VARCHAR(50) NOT NULL DEFAULT 'Active',
    CreateDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdateDate DATETIME NULL,

    CONSTRAINT PK_Product PRIMARY KEY (ID),
    CONSTRAINT UQ_Product_Code UNIQUE (Code),
    CONSTRAINT FK_Product_Brand 
        FOREIGN KEY (BrandID) REFERENCES dbo.Brand(ID),
    CONSTRAINT FK_Product_Category 
        FOREIGN KEY (CategoryID) REFERENCES dbo.Category(ID)
);

CREATE TABLE dbo.ProductVariant (
    ID INT IDENTITY(1,1) NOT NULL,
    ProductID INT NOT NULL,
    Color NVARCHAR(50) NOT NULL,
    Size VARCHAR(10) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL DEFAULT 0,
    Status VARCHAR(50) NOT NULL DEFAULT 'Active',
    CreateDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdateDate DATETIME NULL,

    CONSTRAINT PK_ProductVariant PRIMARY KEY (ID),
    CONSTRAINT UQ_ProductVariant UNIQUE (ProductID, Color, Size),
    CONSTRAINT FK_ProductVariant_Product 
        FOREIGN KEY (ProductID) REFERENCES dbo.Product(ID) ON DELETE CASCADE,
    CONSTRAINT CK_ProductVariant_Price CHECK (Price >= 0),
    CONSTRAINT CK_ProductVariant_Quantity CHECK (Quantity >= 0)
);

CREATE TABLE dbo.ProductVariantImage (
    ID INT IDENTITY(1,1) NOT NULL,
    ProductVariantID INT NOT NULL,
    ImageID INT NOT NULL,
    DisplayOrder INT NOT NULL DEFAULT 0,
    IsMain BIT NOT NULL DEFAULT 0,
    CreateDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_ProductVariantImage PRIMARY KEY (ID),
    CONSTRAINT UQ_ProductVariantImage UNIQUE (ProductVariantID, ImageID),
    CONSTRAINT FK_ProductVariantImage_ProductVariant 
        FOREIGN KEY (ProductVariantID) REFERENCES dbo.ProductVariant(ID) ON DELETE CASCADE,
    CONSTRAINT FK_ProductVariantImage_Image 
        FOREIGN KEY (ImageID) REFERENCES dbo.[Image](ID) ON DELETE CASCADE
);

-- 6. ADDRESS
CREATE TABLE dbo.Address (
    ID INT IDENTITY(1,1) NOT NULL,
    AccountID INT NOT NULL,
    ReceiverName NVARCHAR(100) NOT NULL,
    ReceiverPhone VARCHAR(30) NOT NULL,
    Province NVARCHAR(100) NOT NULL,
    District NVARCHAR(100) NOT NULL,
    Ward NVARCHAR(100) NOT NULL,
    StreetAddress NVARCHAR(255) NOT NULL,
    IsDefault BIT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Address PRIMARY KEY (ID),
    CONSTRAINT FK_Address_Account 
        FOREIGN KEY (AccountID) REFERENCES dbo.Account(ID) ON DELETE CASCADE
);

-- 7. CART (SHOPPING CART)
CREATE TABLE dbo.Cart (
    ID INT IDENTITY(1,1) NOT NULL,
    AccountID INT NOT NULL,
    CreateDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_Cart PRIMARY KEY (ID),
    CONSTRAINT FK_Cart_Account 
        FOREIGN KEY (AccountID) REFERENCES dbo.Account(ID) ON DELETE CASCADE
);

CREATE TABLE dbo.CartDetail (
    ID INT IDENTITY(1,1) NOT NULL,
    CartID INT NOT NULL,
    ProductVariantID INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    CreateDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_CartDetail PRIMARY KEY (ID),
    CONSTRAINT UQ_CartDetail UNIQUE (CartID, ProductVariantID),
    CONSTRAINT FK_CartDetail_Cart 
        FOREIGN KEY (CartID) REFERENCES dbo.Cart(ID) ON DELETE CASCADE,
    CONSTRAINT FK_CartDetail_ProductVariant 
        FOREIGN KEY (ProductVariantID) REFERENCES dbo.ProductVariant(ID),
    CONSTRAINT CK_CartDetail_Quantity CHECK (Quantity > 0)
);

-- 8. ORDER MANAGEMENT
CREATE TABLE dbo.[Order] (
    ID INT IDENTITY(1,1) NOT NULL,
    Code VARCHAR(100) NOT NULL,
    CustomerID INT NOT NULL,
    
    -- Delivery Info
    ReceiverName NVARCHAR(100) NOT NULL,
    ReceiverPhone VARCHAR(30) NOT NULL,
    DeliveryAddress NVARCHAR(500) NOT NULL,
    
    -- Order Info
    Note NVARCHAR(1000) NULL,
    Status VARCHAR(50) NOT NULL DEFAULT 'Pending',
    PaymentMethod VARCHAR(50) NOT NULL,
    PaymentStatus VARCHAR(50) NOT NULL DEFAULT 'Unpaid',
    
    -- Money
    SubTotal DECIMAL(18,2) NOT NULL,
    ShippingFee DECIMAL(18,2) NOT NULL DEFAULT 0,
    Total DECIMAL(18,2) NOT NULL,
    
    -- Dates
    CreateDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdateDate DATETIME NULL,
    CompletedDate DATETIME NULL,

    CONSTRAINT PK_Order PRIMARY KEY (ID),
    CONSTRAINT UQ_Order_Code UNIQUE (Code),
    CONSTRAINT FK_Order_Customer 
        FOREIGN KEY (CustomerID) REFERENCES dbo.Account(ID),
    CONSTRAINT CK_Order_Total CHECK (Total >= 0)
);

CREATE TABLE dbo.OrderDetail (
    ID INT IDENTITY(1,1) NOT NULL,
    OrderID INT NOT NULL,
    ProductVariantID INT NOT NULL,
    
    ProductName NVARCHAR(255) NOT NULL,
    Color NVARCHAR(50) NOT NULL,
    Size VARCHAR(10) NOT NULL,
    
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Total DECIMAL(18,2) NOT NULL,
    
    CreateDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_OrderDetail PRIMARY KEY (ID),
    CONSTRAINT FK_OrderDetail_Order 
        FOREIGN KEY (OrderID) REFERENCES dbo.[Order](ID) ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetail_ProductVariant 
        FOREIGN KEY (ProductVariantID) REFERENCES dbo.ProductVariant(ID),
    CONSTRAINT CK_OrderDetail_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_OrderDetail_Price CHECK (Price >= 0)
);

-- 9. ORDER STATUS HISTORY (for tracking)
CREATE TABLE dbo.OrderStatusHistory (
    ID INT IDENTITY(1,1) NOT NULL,
    OrderID INT NOT NULL,
    FromStatus VARCHAR(50) NULL,
    ToStatus VARCHAR(50) NOT NULL,
    Note NVARCHAR(500) NULL,
    CreatedBy VARCHAR(100) NULL,
    CreateDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_OrderStatusHistory PRIMARY KEY (ID),
    CONSTRAINT FK_OrderStatusHistory_Order 
        FOREIGN KEY (OrderID) REFERENCES dbo.[Order](ID) ON DELETE CASCADE
);

-- 10. VOUCHER
CREATE TABLE dbo.Voucher (
    ID INT IDENTITY(1,1) NOT NULL,
    Code VARCHAR(50) NOT NULL,
    Description NVARCHAR(1000) NULL,
    
    -- Discount Values (can use both or either)
    PercentDiscount DECIMAL(5,2) NULL, -- 0-100%
    FixedDiscount DECIMAL(18,2) NULL, -- Fixed amount
    Quantity INT NOT NULL,
    
    -- Conditions
    MinOrderAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    CategoryID INT NULL, -- Apply to specific category
    ProductID INT NULL, -- Apply to specific product
    Status VARCHAR(50) NOT NULL DEFAULT 'Active',
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    
    CreateDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdateDate DATETIME NULL,

    CONSTRAINT PK_Voucher PRIMARY KEY (ID),
    CONSTRAINT UQ_Voucher_Code UNIQUE (Code),
    CONSTRAINT FK_Voucher_Category 
        FOREIGN KEY (CategoryID) REFERENCES dbo.Category(ID),
    CONSTRAINT FK_Voucher_Product 
        FOREIGN KEY (ProductID) REFERENCES dbo.Product(ID),
    CONSTRAINT CK_Voucher_Dates 
        CHECK (EndDate >= StartDate),
    CONSTRAINT CK_Voucher_Quantity 
        CHECK (Quantity > 0),
    CONSTRAINT CK_Voucher_PercentDiscount 
        CHECK (PercentDiscount IS NULL OR (PercentDiscount >= 0 AND PercentDiscount <= 100)),
    CONSTRAINT CK_Voucher_FixedDiscount 
        CHECK (FixedDiscount IS NULL OR FixedDiscount >= 0),
    CONSTRAINT CK_Voucher_HasDiscount 
        CHECK (PercentDiscount IS NOT NULL OR FixedDiscount IS NOT NULL)
);

-- ===================================
-- INDEXES FOR PERFORMANCE
-- ===================================

-- Account
CREATE INDEX IX_Account_Email ON dbo.Account(Email);
CREATE INDEX IX_Account_Status ON dbo.Account(Status);

-- Product
CREATE INDEX IX_Product_BrandID ON dbo.Product(BrandID);
CREATE INDEX IX_Product_CategoryID ON dbo.Product(CategoryID);
CREATE INDEX IX_Product_Status ON dbo.Product(Status);

-- ProductVariant
CREATE INDEX IX_ProductVariant_ProductID ON dbo.ProductVariant(ProductID);
CREATE INDEX IX_ProductVariant_Status ON dbo.ProductVariant(Status);

-- ProductVariantImage
CREATE INDEX IX_ProductVariantImage_ProductVariantID ON dbo.ProductVariantImage(ProductVariantID);
CREATE INDEX IX_ProductVariantImage_ImageID ON dbo.ProductVariantImage(ImageID);
CREATE INDEX IX_ProductVariantImage_IsMain ON dbo.ProductVariantImage(IsMain);

-- Order
CREATE INDEX IX_Order_CustomerID ON dbo.[Order](CustomerID);
CREATE INDEX IX_Order_Status ON dbo.[Order](Status);
CREATE INDEX IX_Order_CreateDate ON dbo.[Order](CreateDate);

-- OrderDetail
CREATE INDEX IX_OrderDetail_OrderID ON dbo.OrderDetail(OrderID);

-- Address
CREATE INDEX IX_Address_AccountID ON dbo.Address(AccountID);

-- Cart
CREATE INDEX IX_Cart_AccountID ON dbo.Cart(AccountID);

-- CartDetail
CREATE INDEX IX_CartDetail_CartID ON dbo.CartDetail(CartID);
CREATE INDEX IX_CartDetail_ProductVariantID ON dbo.CartDetail(ProductVariantID);

-- Voucher
CREATE INDEX IX_Voucher_Code ON dbo.Voucher(Code);
CREATE INDEX IX_Voucher_Status ON dbo.Voucher(Status);
CREATE INDEX IX_Voucher_CategoryID ON dbo.Voucher(CategoryID);
CREATE INDEX IX_Voucher_ProductID ON dbo.Voucher(ProductID);
CREATE INDEX IX_Voucher_Dates ON dbo.Voucher(StartDate, EndDate);

-- ===================================
-- INITIAL DATA
-- ===================================

-- Insert default roles
INSERT INTO dbo.[Role] (Name, Description) VALUES 
('Admin', N'Quản trị viên hệ thống'),
('Manager', N'Quản lý cửa hàng'),
('Staff', N'Nhân viên bán hàng'),
('Customer', N'Khách hàng');

-- Insert sample brands
INSERT INTO dbo.Brand (Name, Description) VALUES 
(N'Nike', N'Thương hiệu thể thao hàng đầu thế giới'),
(N'Adidas', N'Thương hiệu thể thao Đức'),
(N'Puma', N'Thương hiệu thể thao quốc tế'),
(N'New Balance', N'Giày thể thao Mỹ'),
(N'Vans', N'Giày sneaker street style');

-- Insert sample categories
INSERT INTO dbo.Category (Name, Description) VALUES 
(N'Giày thể thao', N'Giày thể thao, running'),
(N'Giày lifestyle', N'Giày đi hàng ngày'),
(N'Giày cao cổ', N'Giày sneaker cao cổ'),
(N'Giày slip-on', N'Giày không dây');

-- Insert sample vouchers
INSERT INTO dbo.Voucher (Code, Description, PercentDiscount, FixedDiscount, Quantity, MinOrderAmount, CategoryID, ProductID, Status, StartDate, EndDate) VALUES 
(N'WELCOME10', N'Giảm 10% cho đơn hàng đầu tiên', 10, NULL, 100, 0, NULL, NULL, 'Active', GETDATE(), DATEADD(MONTH, 6, GETDATE())),
(N'SUMMER50K', N'Giảm 50k cho đơn từ 500k', NULL, 50000, 50, 500000, NULL, NULL, 'Active', GETDATE(), DATEADD(MONTH, 3, GETDATE())),
(N'FREESHIP', N'Miễn phí ship cho đơn từ 300k', NULL, 30000, 200, 300000, NULL, NULL, 'Active', GETDATE(), DATEADD(MONTH, 12, GETDATE())),
(N'SPORT20', N'Giảm 20% cho giày thể thao', 20, NULL, 30, 0, 1, NULL, 'Active', GETDATE(), DATEADD(MONTH, 2, GETDATE()));
