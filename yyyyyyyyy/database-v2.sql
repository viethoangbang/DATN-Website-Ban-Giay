CREATE TABLE Account (
    ID INT IDENTITY PRIMARY KEY,
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    FullName NVARCHAR(255),
    Phone VARCHAR(30),
    Status VARCHAR(30),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME
);
CREATE TABLE Role (
    ID INT IDENTITY PRIMARY KEY,
    Name VARCHAR(50) UNIQUE NOT NULL
);
CREATE TABLE AccountRole (
    AccountID INT,
    RoleID INT,
    PRIMARY KEY (AccountID, RoleID),
    FOREIGN KEY (AccountID) REFERENCES Account(ID),
    FOREIGN KEY (RoleID) REFERENCES Role(ID)
);
CREATE TABLE Category (
    ID INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Status VARCHAR(30),
    CreatedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE Product (
    ID INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Code VARCHAR(100),
    BrandID INT,
    CategoryID INT,
    Status VARCHAR(30),
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (BrandID) REFERENCES Brand(ID),
    FOREIGN KEY (CategoryID) REFERENCES Category(ID)
);
CREATE TABLE Brand (
    ID INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    LogoUrl VARCHAR(500),
    Status VARCHAR(30),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Color (
    ID INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);
CREATE TABLE Size (
    ID INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);
CREATE TABLE Image (
    ID INT IDENTITY PRIMARY KEY,
    Url VARCHAR(1000) NOT NULL
);
CREATE TABLE ProductVariant (
    ID INT IDENTITY PRIMARY KEY,
    ProductID INT NOT NULL,
    ColorID INT,
    SizeID INT,
    Price DECIMAL(15,2) NOT NULL,
    Quantity INT NOT NULL,

    FOREIGN KEY (ProductID) REFERENCES Product(ID),
    FOREIGN KEY (ColorID) REFERENCES Color(ID),
    FOREIGN KEY (SizeID) REFERENCES Size(ID)
);
CREATE TABLE ProductVariantImage (
    ProductVariantID INT,
    ImageID INT,
    PRIMARY KEY (ProductVariantID, ImageID),
    FOREIGN KEY (ProductVariantID) REFERENCES ProductVariant(ID),
    FOREIGN KEY (ImageID) REFERENCES Image(ID)
);
CREATE TABLE Cart (
    ID INT IDENTITY PRIMARY KEY,
    AccountID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (AccountID) REFERENCES Account(ID)
);
CREATE TABLE CartItem (
    CartID INT,
    ProductVariantID INT,
    Quantity INT NOT NULL,

    PRIMARY KEY (CartID, ProductVariantID),
    FOREIGN KEY (CartID) REFERENCES Cart(ID),
    FOREIGN KEY (ProductVariantID) REFERENCES ProductVariant(ID)
);
CREATE TABLE Address (
    ID INT IDENTITY PRIMARY KEY,
    AccountID INT,
    FullAddress NVARCHAR(500),
    ReceiverName NVARCHAR(100),
    ReceiverPhone VARCHAR(30),

    FOREIGN KEY (AccountID) REFERENCES Account(ID)
);
CREATE TABLE [Order] (
    ID INT IDENTITY PRIMARY KEY,
    Code VARCHAR(100),
    CustomerID INT,
    AddressID INT,
    Status VARCHAR(30),
    Total DECIMAL(15,2),
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (CustomerID) REFERENCES Account(ID),
    FOREIGN KEY (AddressID) REFERENCES Address(ID)
);
CREATE TABLE OrderItem (
    OrderID INT,
    ProductVariantID INT,
    Price DECIMAL(15,2),
    Quantity INT,

    PRIMARY KEY (OrderID, ProductVariantID),
    FOREIGN KEY (OrderID) REFERENCES [Order](ID),
    FOREIGN KEY (ProductVariantID) REFERENCES ProductVariant(ID)
);
CREATE TABLE Payment (
    ID INT IDENTITY PRIMARY KEY,
    OrderID INT,
    Method VARCHAR(50),
    Amount DECIMAL(15,2),
    Status VARCHAR(30),
    PaidAt DATETIME,

    FOREIGN KEY (OrderID) REFERENCES [Order](ID)
);
CREATE TABLE Voucher (
    ID INT IDENTITY PRIMARY KEY,
    Code VARCHAR(50) UNIQUE,
    DiscountValue DECIMAL(15,2),
    StartDate DATETIME,
    EndDate DATETIME
);
CREATE TABLE OrderVoucher (
    OrderID INT,
    VoucherID INT,
    PRIMARY KEY (OrderID, VoucherID),
    FOREIGN KEY (OrderID) REFERENCES [Order](ID),
    FOREIGN KEY (VoucherID) REFERENCES Voucher(ID)
);
CREATE TABLE Warehouse (
    ID INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,

    DistrictID INT NOT NULL,
    WardCode VARCHAR(20) NOT NULL,
    AddressText NVARCHAR(255),

    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE Shipment (
    ID INT IDENTITY PRIMARY KEY,

    OrderID INT NOT NULL,
    WarehouseID INT NOT NULL,

    ToDistrictID INT NOT NULL,
    ToWardCode VARCHAR(20) NOT NULL,

    Weight INT NOT NULL,
    Length INT NOT NULL,
    Width  INT NOT NULL,
    Height INT NOT NULL,

    ServiceID INT,
    ShippingFee DECIMAL(15,2),

    GHNResponse NVARCHAR(MAX), -- raw json
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (OrderID) REFERENCES [Order](ID),
    FOREIGN KEY (WarehouseID) REFERENCES Warehouse(ID)
);
