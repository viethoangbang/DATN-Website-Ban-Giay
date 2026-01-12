create database SneakerPoly
go 
use SnekerPoly

create table Account
(
    Id int identity(1,1),   
    FullName nvarchar(1000) not null,
    Email varchar(max) not null,
    HashPassword varchar(1000),
    Sex bit,
    PhoneNumber varchar(50),
    IsActive bit DEFAULT 0,
    AvatarUrl varchar(max),

    CreateAt datetime DEFAULT getdate(),
    CreateById int null,

    UpdateAt datetime null,
    UpdateById int null,

    CONSTRAINT PK_Account primary key (Id),
    constraint UQ_Email unique (Email),
    CONSTRAINT FK_CreateBy_Account
        FOREIGN KEY (CreateBy) REFERENCES Account(Id)
    CONSTRAINT FK_UpdateBy_Account
        FOREIGN KEY (UpdateBy) REFERENCES Account(Id)
)

create table Role(
    Id int identity(1,1),
    Name nvarchar(100),
    constraint PK_Role primary key (Id),
    constraint UQ_Name unique(Name)
)

create table AccountRole(
    Id int identity(1,1),
    AccountId int,
    RoleId int,
    
    CONSTRAINT PK_AccountRole PRIMARY KEY (Id),
    CONSTRAINT FK_AccountId_Account
        FOREIGN KEY AccountId REFERENCES Account(Id),
    CONSTRAINT FK_RoleId_Role
        FOREIGN KEY RoleId REFERENCES Role(Id),
)

CREATE TABLE Category(
    Id int identity(1,1),
    Name nvarchar(1000),

    CONSTRAINT PK_Category PRIMARY KEY (Id),
    CONSTRAINT UQ_Name UNIQUE (Name)
)

CREATE TABLE Color(
    Id int identity(1,1),
    Name nvarchar(1000),

    CONSTRAINT PK_Color PRIMARY KEY (Id),
    CONSTRAINT UQ_Name UNIQUE (Name)
)

CREATE TABLE Size(
    Id int identity(1,1),
    SizeNumber int,

    CONSTRAINT PK_Size PRIMARY KEY (Id),
    CONSTRAINT UQ_SizeNumber UNIQUE (SizeNumber)
)

CREATE TABLE Image(
    Id int identity(1,1),
    Url varchar(max),
    CONSTRAINT PK_Image PRIMARY KEY (Id)
)

CREATE TABLE AccountAddress(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AccountId INT NOT NULL,
    WardId INT NOT NULL, -- Chỉ lưu ID, không lưu tên
    
    FullName NVARCHAR(255),
    DeliveryPhoneNumber NVARCHAR(20),
    IsDefault BIT DEFAULT 0,
    AddressType NVARCHAR(50),
    
    FOREIGN KEY (WardId) REFERENCES Wards(WardId)
    FOREIGN KEY (AccountId) REFERENCES Account(Id)
)

CREATE TABLE Provinces (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProvinceId INT NOT NULL,
    ProvinceName NVARCHAR(1000) NOT NULL,
    UNIQUE(ProvinceId)
);

CREATE TABLE Districts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DistrictId INT NOT NULL, 
    DistrictName NVARCHAR(1000) NOT NULL,
    ProvinceId INT NOT NULL,
    FOREIGN KEY (ProvinceId) REFERENCES Provinces(ProvinceId),
    UNIQUE(DistrictId)
);

CREATE TABLE Wards (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    WardId INT NOT NULL, 
    WardName NVARCHAR(1000) NOT NULL,
    DistrictId INT NOT NULL, 
    FOREIGN KEY (DistrictId) REFERENCES Districts(DistrictId),
    UNIQUE(WardId)
);

CREATE TABLE Cart(
    Id int identity(1,1) PRIMARY KEY,
    AcountId int not null,
    CONSTRAINT FK_AccountId
        FOREIGN KEY (AccountId) REFERENCES Account(Id)
)