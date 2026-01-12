CREATE DATABASE ShopBGiay;
GO
USE ShopBGiay;
GO

-----------------------------
-- 1. ROLE
-----------------------------
CREATE TABLE Role (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) UNIQUE,   -- admin, employee, customer
    Description VARCHAR(255)
);

INSERT INTO Role (Name, Description)
VALUES ('admin','Quản trị hệ thống'),
       ('employee','Nhân viên bán hàng'),
       ('customer','Khách hàng');

-----------------------------
-- 2. ACCOUNT (thay customer + employee)
-----------------------------
CREATE TABLE Account (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Email VARCHAR(255) UNIQUE,
    Password VARCHAR(255),
    PhoneNumber VARCHAR(30),
    FullName NVARCHAR(255),
    Sex NVARCHAR(10),
    BirthYear INT,
    Avatar_Url VARCHAR(1000),
    Status VARCHAR(50),

    CreateBy VARCHAR(100),
    CreateDate DATETIME,
    UpdateBy VARCHAR(100),
    UpdateDate DATETIME
);

-----------------------------
-- 3. ACCOUNT - ROLE (nhiều role)
-----------------------------
CREATE TABLE AccountRole (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    AccountID INT,
    RoleID INT,

    FOREIGN KEY (AccountID) REFERENCES Account(ID),
    FOREIGN KEY (RoleID) REFERENCES Role(ID)
);

-----------------------------
-- 4. ADDRESS
-----------------------------
CREATE TABLE Address (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ProvinceName NVARCHAR(100),
    DistrictName NVARCHAR(100),
    WardName NVARCHAR(100),
    ProvinceId INT,
    DistrictId INT,
    WardCode VARCHAR(20),
    Status NVARCHAR(50),

    ReceiverName NVARCHAR(100),
    ReceiverPhone VARCHAR(30),
    ReceiverEmail VARCHAR(100),

    AccountID INT,
    FOREIGN KEY (AccountID) REFERENCES Account(ID)
);

-----------------------------
-- 5. CATEGORY
-----------------------------
CREATE TABLE Category (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255),
    Description NVARCHAR(1000),
    Status NVARCHAR(50),
    Condition NVARCHAR(100),

    CreateBy NVARCHAR(100),
    CreateDate DATETIME,
    UpdateBy NVARCHAR(100),
    UpdateDate DATETIME
);

-----------------------------
-- 6. PRODUCT
-----------------------------
CREATE TABLE Product (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255),
    Code NVARCHAR(100),
    Brand NVARCHAR(100),
    Description NVARCHAR(1000),
    Status VARCHAR(50),  -- Badge: New, Hot, Sell Well, Best Seller, ...
    IsActive BIT DEFAULT 1,
    Weight INT DEFAULT 1000,  -- Trọng lượng sản phẩm (gram)

    CategoryID INT,
    FOREIGN KEY (CategoryID) REFERENCES Category(ID),

    CreateBy NVARCHAR(100),
    CreateDate DATETIME,
    UpdateBy VARCHAR(100),
    UpdateDate DATETIME
);

-----------------------------
-- 7. COLOR
-----------------------------
CREATE TABLE Color (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50),
    Status VARCHAR(50),
    CreateDate DATETIME,
    UpdateBy VARCHAR(100),
    UpdateDate DATETIME
);

-----------------------------
-- 8. SIZE
-----------------------------
CREATE TABLE Size (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    SizeName VARCHAR(50)
);

-----------------------------
-- 9. IMAGE
-----------------------------
CREATE TABLE Image (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Url NVARCHAR(1000),
    Type NVARCHAR(50),
    Status NVARCHAR(50)
);

-----------------------------
-- 10. PRODUCT DETAIL
-----------------------------
CREATE TABLE ProductDetail (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Price DECIMAL(15,2),
    Quantity INT,

    ImageID INT,
    ColorID INT,
    SizeID INT,
    ProductID INT,

    CreateDate DATETIME,

    FOREIGN KEY (ImageID) REFERENCES Image(ID),
    FOREIGN KEY (ColorID) REFERENCES Color(ID),
    FOREIGN KEY (SizeID) REFERENCES Size(ID),
    FOREIGN KEY (ProductID) REFERENCES Product(ID)
);

-----------------------------
-- 11. CART
-----------------------------
CREATE TABLE Cart (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    CreateDate DATETIME,
    EndDate DATETIME,
    AccountID INT,

    FOREIGN KEY (AccountID) REFERENCES Account(ID)
);

-----------------------------
-- 12. CART DETAIL
-----------------------------
CREATE TABLE CartDetail (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Quantity INT,
    CartID INT,
    ProductDetailID INT,

    FOREIGN KEY (CartID) REFERENCES Cart(ID),
    FOREIGN KEY (ProductDetailID) REFERENCES ProductDetail(ID)
);

-----------------------------
-- 13. ORDER
-----------------------------
CREATE TABLE [Order] (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Description TEXT,
    Total DECIMAL(15,2),
    CreateBy NVARCHAR(100),
    CreateDate DATETIME,
    DeliveryDate DATETIME,
    UpdateDate DATETIME,
    Status NVARCHAR(50),

    CustomerID INT,
    EmployeeID INT,
    AddressDeliveryID INT,

    ReceiverName NVARCHAR(100),
    ReceiverPhone VARCHAR(30),
    ReceiverAddress NVARCHAR(255),
    Code NVARCHAR(100),
    
    -- Pricing fields
    ProductTotal NVARCHAR(100),  -- Tổng tiền sản phẩm (chưa discount)
    ProductDiscount DECIMAL(15,2),  -- Discount từ bảng Discount
    ShippingFee DECIMAL(15,2),
    VoucherDiscount DECIMAL(15,2),
    VoucherCode NVARCHAR(100),
    
    -- Payment & Shipping
    PaymentMethod NVARCHAR(50),  -- VNPay, COD
    PaymentStatus VARCHAR(50),  -- Pending, Paid, Failed, Refunded
    ShippingStatus NVARCHAR(50),  -- Pending, Picking, Delivering, Delivered, Cancelled
    
    -- Delivery
    EstimatedDeliveryDate DATETIME,
    ActualWeight INT,
    Note TEXT,

    FOREIGN KEY (CustomerID) REFERENCES Account(ID),
    FOREIGN KEY (EmployeeID) REFERENCES Account(ID),
    FOREIGN KEY (AddressDeliveryID) REFERENCES Address(ID)
);

-----------------------------
-- 14. ORDER DETAIL
-----------------------------
CREATE TABLE OrderDetail (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Quantity INT,
    ProductName NVARCHAR(255),
    Price DECIMAL(15,2),  -- Giá cuối cùng (sau discount) - backward compatible
    OriginalPrice DECIMAL(15,2),  -- Giá gốc từ ProductDetail.Price
    FinalPrice DECIMAL(15,2),  -- Giá sau discount (same as Price)
    DiscountType NVARCHAR(50),  -- Percentage, Fixed
    DiscountValue DECIMAL(15,2),
    ProductDetailID INT,
    OrderID INT,
    CreateDate DATETIME,

    FOREIGN KEY (ProductDetailID) REFERENCES ProductDetail(ID),
    FOREIGN KEY (OrderID) REFERENCES [Order](ID)
);

-----------------------------
-- 15. VOUCHER
-----------------------------
CREATE TABLE Voucher (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Code VARCHAR(100),
    Name NVARCHAR(255),
    Type VARCHAR(50),
    Discount DECIMAL(15,2),
    MaxDiscount DECIMAL(15,2),
    Quantity INT,
    Description NVARCHAR(1000),
    Status VARCHAR(50),
    StartDate DATETIME,
    EndDate DATETIME,

    CreateBy VARCHAR(100),
    CreateDate DATETIME,
    UpdateBy VARCHAR(100),
    UpdateDate DATETIME,

    CategoryID INT,
    FOREIGN KEY (CategoryID) REFERENCES Category(ID)
);

-----------------------------
-- 16. PRODUCT DETAIL IMAGE (Many-to-Many: ProductDetail - Image)
-----------------------------
CREATE TABLE ProductDetailImage (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ProductDetailID INT NOT NULL,
    ImageID INT NOT NULL,
    DisplayOrder INT DEFAULT 0,
    CreateDate DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (ProductDetailID) REFERENCES ProductDetail(ID) ON DELETE CASCADE,
    FOREIGN KEY (ImageID) REFERENCES Image(ID) ON DELETE CASCADE,
    
    -- Prevent duplicate combinations
    UNIQUE(ProductDetailID, ImageID)
);

-----------------------------
-- 17. BRAND BANNER
-----------------------------
CREATE TABLE BrandBanner (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Brand VARCHAR(100) NOT NULL,
    ImageID INT NOT NULL,
    DisplayOrder INT,
    Status VARCHAR(50),
    CreateBy VARCHAR(100),
    CreateDate DATETIME,
    UpdateBy VARCHAR(100),
    UpdateDate DATETIME,

    FOREIGN KEY (ImageID) REFERENCES Image(ID) ON DELETE CASCADE
);

-----------------------------
-- 18. DISCOUNT
-----------------------------
CREATE TABLE Discount (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    DiscountType VARCHAR(50) NOT NULL,  -- "Percentage" or "Fixed"
    DiscountValue DECIMAL(15,2) NOT NULL,  -- Percentage (0-100) or Fixed amount
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    Status VARCHAR(50),  -- "Active" or "Inactive"
    CreateBy VARCHAR(100),
    CreateDate DATETIME,
    UpdateBy VARCHAR(100),
    UpdateDate DATETIME,

    FOREIGN KEY (ProductID) REFERENCES Product(ID) ON DELETE CASCADE
);

-----------------------------
-- 19. PAYMENT
-----------------------------
CREATE TABLE Payment (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    
    -- VNPay fields
    TransactionNo VARCHAR(100),
    BankCode VARCHAR(50),
    CardType VARCHAR(50),
    PaymentMethod VARCHAR(50),
    Amount DECIMAL(15,2),
    
    -- Status
    Status VARCHAR(50),
    
    -- VNPay response
    ResponseCode VARCHAR(10),
    ResponseMessage VARCHAR(255),
    SecureHash VARCHAR(500),
    
    -- Timestamps
    CreateDate DATETIME,
    UpdateDate DATETIME,
    PaidDate DATETIME,

    FOREIGN KEY (OrderID) REFERENCES [Order](ID) ON DELETE CASCADE
);

-----------------------------
-- 20. SHIPMENT
-----------------------------
CREATE TABLE Shipment (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    
    -- GHN/GHTK fields
    ShipmentCode VARCHAR(100),
    PartnerCode VARCHAR(50),
    
    -- Pick address
    PickAddress NVARCHAR(500),
    PickProvince NVARCHAR(100),
    PickDistrict NVARCHAR(100),
    PickWard NVARCHAR(100),
    PickTel VARCHAR(30),
    PickName NVARCHAR(100),
    
    -- Delivery address
    DeliveryAddress NVARCHAR(500),
    DeliveryProvince NVARCHAR(100),
    DeliveryDistrict NVARCHAR(100),
    DeliveryWard NVARCHAR(100),
    DeliveryTel VARCHAR(30),
    DeliveryName NVARCHAR(100),
    
    -- Package info
    Weight INT,
    Value DECIMAL(15,2),
    ShippingFee DECIMAL(15,2),
    InsuranceFee DECIMAL(15,2),
    
    -- Status
    Status VARCHAR(50),
    StatusText NVARCHAR(255),
    
    -- Timestamps
    CreateDate DATETIME,
    UpdateDate DATETIME,
    PickedDate DATETIME,
    DeliveredDate DATETIME,
    EstimatedDeliveryDate DATETIME,
    
    Note TEXT,

    FOREIGN KEY (OrderID) REFERENCES [Order](ID) ON DELETE CASCADE
);

-----------------------------
-- 21. ORDER STATUS HISTORY
-----------------------------
CREATE TABLE OrderStatusHistory (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    FromStatus VARCHAR(50),
    ToStatus VARCHAR(50),
    Note TEXT,
    CreatedBy VARCHAR(100),
    CreateDate DATETIME,

    FOREIGN KEY (OrderID) REFERENCES [Order](ID) ON DELETE CASCADE
);

-- Create indexes for better query performance
CREATE INDEX IX_ProductDetailImage_ProductDetailID ON ProductDetailImage(ProductDetailID);
CREATE INDEX IX_ProductDetailImage_ImageID ON ProductDetailImage(ImageID);
CREATE INDEX IX_Product_CategoryID ON Product(CategoryID);
CREATE INDEX IX_ProductDetail_ProductID ON ProductDetail(ProductID);
CREATE INDEX IX_Order_CustomerID ON [Order](CustomerID);
CREATE INDEX IX_Order_Status ON [Order](Status);
CREATE INDEX IX_OrderDetail_OrderID ON OrderDetail(OrderID);
CREATE INDEX IX_Cart_AccountID ON Cart(AccountID);
CREATE INDEX IX_Address_AccountID ON Address(AccountID);
CREATE INDEX IX_Discount_ProductID ON Discount(ProductID);
CREATE INDEX IX_Payment_OrderID ON Payment(OrderID);
CREATE INDEX IX_Shipment_OrderID ON Shipment(OrderID);
CREATE INDEX IX_OrderStatusHistory_OrderID ON OrderStatusHistory(OrderID);
