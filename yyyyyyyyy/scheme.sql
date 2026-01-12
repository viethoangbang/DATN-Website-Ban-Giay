-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- ShopBGiay.dbo.Account definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.Account;

CREATE TABLE ShopBGiay.dbo.Account ( ID int IDENTITY(1,1) NOT NULL, Email varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Password varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, PhoneNumber varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, FullName nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Sex varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, BirthYear int NULL, Status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreateBy varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreateDate datetime NULL, UpdateBy varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, UpdateDate datetime NULL, Avatar_Url varchar(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CONSTRAINT PK__Account__3214EC27240D830D PRIMARY KEY (ID), CONSTRAINT UQ__Account__A9D1053449DC465B UNIQUE (Email));


-- ShopBGiay.dbo.Category definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.Category;

CREATE TABLE ShopBGiay.dbo.Category ( ID int IDENTITY(1,1) NOT NULL, Name nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Description nvarchar(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, [Condition] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreateBy varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreateDate datetime NULL, UpdateBy varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, UpdateDate datetime NULL, CONSTRAINT PK__Category__3214EC27EA89B713 PRIMARY KEY (ID));


-- ShopBGiay.dbo.Color definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.Color;

CREATE TABLE ShopBGiay.dbo.Color ( ID int IDENTITY(1,1) NOT NULL, Name varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreateDate datetime NULL, UpdateBy varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, UpdateDate datetime NULL, CONSTRAINT PK__Color__3214EC274E709633 PRIMARY KEY (ID));


-- ShopBGiay.dbo.[Image] definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.[Image];

CREATE TABLE ShopBGiay.dbo.[Image] ( ID int IDENTITY(1,1) NOT NULL, Url varchar(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, [Type] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CONSTRAINT PK__Image__3214EC27555A91A8 PRIMARY KEY (ID));


-- ShopBGiay.dbo.[Role] definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.[Role];

CREATE TABLE ShopBGiay.dbo.[Role] ( ID int IDENTITY(1,1) NOT NULL, Name varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Description varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CONSTRAINT PK__Role__3214EC271958A971 PRIMARY KEY (ID), CONSTRAINT UQ__Role__737584F6B9511F40 UNIQUE (Name));


-- ShopBGiay.dbo.[Size] definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.[Size];

CREATE TABLE ShopBGiay.dbo.[Size] ( ID int IDENTITY(1,1) NOT NULL, SizeName varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Quantity int NULL, CONSTRAINT PK__Size__3214EC271C999E6C PRIMARY KEY (ID));


-- ShopBGiay.dbo.AccountRole definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.AccountRole;

CREATE TABLE ShopBGiay.dbo.AccountRole ( ID int IDENTITY(1,1) NOT NULL, AccountID int NULL, RoleID int NULL, CONSTRAINT PK__AccountR__3214EC27FA5C381B PRIMARY KEY (ID), CONSTRAINT FK__AccountRo__Accou__3E52440B FOREIGN KEY (AccountID) REFERENCES ShopBGiay.dbo.Account(ID), CONSTRAINT FK__AccountRo__RoleI__3F466844 FOREIGN KEY (RoleID) REFERENCES ShopBGiay.dbo.[Role](ID));


-- ShopBGiay.dbo.Address definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.Address;

CREATE TABLE ShopBGiay.dbo.Address ( ID int IDENTITY(1,1) NOT NULL, ProvinceName nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, DistrictName nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, WardName nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, ReceiverName nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, ReceiverPhone nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, ReceiverEmail nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, AccountID int NULL, ProvinceId int NULL, DistrictId int NULL, WardCode varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CONSTRAINT PK__Address__3214EC2732AEAD62 PRIMARY KEY (ID), CONSTRAINT FK__Address__Account__4222D4EF FOREIGN KEY (AccountID) REFERENCES ShopBGiay.dbo.Account(ID));


-- ShopBGiay.dbo.BrandBanner definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.BrandBanner;

CREATE TABLE ShopBGiay.dbo.BrandBanner ( ID int IDENTITY(1,1) NOT NULL, Brand nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL, ImageID int NOT NULL, DisplayOrder int DEFAULT 0 NULL, Status nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Active' NULL, CreateDate datetime DEFAULT getdate() NULL, UpdateDate datetime NULL, CreateBy nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, UpdateBy nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CONSTRAINT PK__BrandBan__3214EC2744D899AE PRIMARY KEY (ID), CONSTRAINT FK_BrandBanner_Image FOREIGN KEY (ImageID) REFERENCES ShopBGiay.dbo.[Image](ID) ON DELETE CASCADE);
 CREATE NONCLUSTERED INDEX IX_BrandBanner_Brand ON ShopBGiay.dbo.BrandBanner (  Brand ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_BrandBanner_Status ON ShopBGiay.dbo.BrandBanner (  Status ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- ShopBGiay.dbo.Cart definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.Cart;

CREATE TABLE ShopBGiay.dbo.Cart ( ID int IDENTITY(1,1) NOT NULL, CreateDate datetime NULL, EndDate datetime NULL, AccountID int NULL, CONSTRAINT PK__Cart__3214EC271F193365 PRIMARY KEY (ID), CONSTRAINT FK__Cart__AccountID__5535A963 FOREIGN KEY (AccountID) REFERENCES ShopBGiay.dbo.Account(ID));


-- ShopBGiay.dbo.[Order] definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.[Order];

CREATE TABLE ShopBGiay.dbo.[Order] ( ID int IDENTITY(1,1) NOT NULL, Description text COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Total decimal(15,2) NULL, CreateBy varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreateDate datetime NULL, DeliveryDate datetime NULL, UpdateDate datetime NULL, ProductDiscount decimal(15,2) NULL, Status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CustomerID int NULL, EmployeeID int NULL, AddressDeliveryID int NULL, ReceiverName nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, ReceiverPhone varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, ReceiverAddress nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, ProductTotal decimal(15,2) NULL, Code varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, ShippingFee decimal(15,2) DEFAULT 0 NULL, VoucherCode varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, VoucherDiscount decimal(15,2) DEFAULT 0 NULL, PaymentMethod varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, PaymentStatus varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, ShippingStatus varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, EstimatedDeliveryDate datetime NULL, ActualWeight int NULL, Note text COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CONSTRAINT PK__Order__3214EC271E025CDC PRIMARY KEY (ID), CONSTRAINT FK__Order__AddressDe__5DCAEF64 FOREIGN KEY (AddressDeliveryID) REFERENCES ShopBGiay.dbo.Address(ID), CONSTRAINT FK__Order__CustomerI__5BE2A6F2 FOREIGN KEY (CustomerID) REFERENCES ShopBGiay.dbo.Account(ID), CONSTRAINT FK__Order__EmployeeI__5CD6CB2B FOREIGN KEY (EmployeeID) REFERENCES ShopBGiay.dbo.Account(ID));


-- ShopBGiay.dbo.OrderStatusHistory definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.OrderStatusHistory;

CREATE TABLE ShopBGiay.dbo.OrderStatusHistory ( ID int IDENTITY(1,1) NOT NULL, OrderID int NOT NULL, FromStatus nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, ToStatus nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Note text COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreatedBy varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreateDate datetime DEFAULT getdate() NULL, CONSTRAINT PK__OrderSta__3214EC273EAC1B26 PRIMARY KEY (ID), CONSTRAINT FK__OrderStat__Order__0C85DE4D FOREIGN KEY (OrderID) REFERENCES ShopBGiay.dbo.[Order](ID) ON DELETE CASCADE);
 CREATE NONCLUSTERED INDEX IDX_OrderStatusHistory_OrderID ON ShopBGiay.dbo.OrderStatusHistory (  OrderID ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- ShopBGiay.dbo.Payment definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.Payment;

CREATE TABLE ShopBGiay.dbo.Payment ( ID int IDENTITY(1,1) NOT NULL, OrderID int NOT NULL, TransactionNo varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, BankCode varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CardType varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, PaymentMethod varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Amount decimal(15,2) NULL, Status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, ResponseCode varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, ResponseMessage varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, SecureHash varchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreateDate datetime DEFAULT getdate() NULL, UpdateDate datetime NULL, PaidDate datetime NULL, CONSTRAINT PK__Payment__3214EC272F161BC2 PRIMARY KEY (ID), CONSTRAINT FK__Payment__OrderID__02FC7413 FOREIGN KEY (OrderID) REFERENCES ShopBGiay.dbo.[Order](ID) ON DELETE CASCADE);
 CREATE NONCLUSTERED INDEX IDX_Payment_OrderID ON ShopBGiay.dbo.Payment (  OrderID ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IDX_Payment_TransactionNo ON ShopBGiay.dbo.Payment (  TransactionNo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- ShopBGiay.dbo.Product definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.Product;

CREATE TABLE ShopBGiay.dbo.Product ( ID int IDENTITY(1,1) NOT NULL, Name nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Code varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Brand varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Description nvarchar(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CategoryID int NULL, CreateBy varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreateDate datetime NULL, UpdateBy varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, UpdateDate datetime NULL, IsActive bit DEFAULT 1 NOT NULL, Weight int DEFAULT 1000 NOT NULL, CONSTRAINT PK__Product__3214EC27C28975DA PRIMARY KEY (ID), CONSTRAINT FK__Product__Categor__46E78A0C FOREIGN KEY (CategoryID) REFERENCES ShopBGiay.dbo.Category(ID));


-- ShopBGiay.dbo.ProductDetail definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.ProductDetail;

CREATE TABLE ShopBGiay.dbo.ProductDetail ( ID int IDENTITY(1,1) NOT NULL, Price decimal(15,2) NULL, Quantity int NULL, ImageID int NULL, ColorID int NULL, SizeID int NULL, ProductID int NULL, CreateDate datetime NULL, CONSTRAINT PK__ProductD__3214EC2759E29E4C PRIMARY KEY (ID), CONSTRAINT FK__ProductDe__Color__5070F446 FOREIGN KEY (ColorID) REFERENCES ShopBGiay.dbo.Color(ID), CONSTRAINT FK__ProductDe__Image__4F7CD00D FOREIGN KEY (ImageID) REFERENCES ShopBGiay.dbo.[Image](ID), CONSTRAINT FK__ProductDe__Produ__52593CB8 FOREIGN KEY (ProductID) REFERENCES ShopBGiay.dbo.Product(ID), CONSTRAINT FK__ProductDe__SizeI__5165187F FOREIGN KEY (SizeID) REFERENCES ShopBGiay.dbo.[Size](ID));


-- ShopBGiay.dbo.ProductDetailImage definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.ProductDetailImage;

CREATE TABLE ShopBGiay.dbo.ProductDetailImage ( ID int IDENTITY(1,1) NOT NULL, ProductDetailID int NOT NULL, ImageID int NOT NULL, DisplayOrder int DEFAULT 0 NULL, CreateDate datetime DEFAULT getdate() NULL, CONSTRAINT PK__ProductD__3214EC27A4E3FDFE PRIMARY KEY (ID), CONSTRAINT UQ__ProductD__EBDCB9DB98A1F9A8 UNIQUE (ProductDetailID,ImageID), CONSTRAINT FK__ProductDe__Image__6B24EA82 FOREIGN KEY (ImageID) REFERENCES ShopBGiay.dbo.[Image](ID) ON DELETE CASCADE, CONSTRAINT FK__ProductDe__Produ__6A30C649 FOREIGN KEY (ProductDetailID) REFERENCES ShopBGiay.dbo.ProductDetail(ID) ON DELETE CASCADE);
 CREATE NONCLUSTERED INDEX IX_ProductDetailImage_ImageID ON ShopBGiay.dbo.ProductDetailImage (  ImageID ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_ProductDetailImage_ProductDetailID ON ShopBGiay.dbo.ProductDetailImage (  ProductDetailID ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- ShopBGiay.dbo.Shipment definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.Shipment;

CREATE TABLE ShopBGiay.dbo.Shipment ( ID int IDENTITY(1,1) NOT NULL, OrderID int NOT NULL, ShipmentCode varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, PartnerCode varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, PickAddress varchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, PickProvince varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, PickDistrict varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, PickWard varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, PickTel varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, PickName varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, DeliveryAddress varchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, DeliveryProvince varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, DeliveryDistrict varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, DeliveryWard varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, DeliveryTel varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, DeliveryName varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Weight int NULL, Value decimal(15,2) NULL, ShippingFee decimal(15,2) NULL, InsuranceFee decimal(15,2) DEFAULT 0 NULL, Status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, StatusText varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreateDate datetime DEFAULT getdate() NULL, UpdateDate datetime NULL, PickedDate datetime NULL, DeliveredDate datetime NULL, EstimatedDeliveryDate datetime NULL, Note text COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CONSTRAINT PK__Shipment__3214EC27E9CB7BFA PRIMARY KEY (ID), CONSTRAINT FK__Shipment__OrderI__08B54D69 FOREIGN KEY (OrderID) REFERENCES ShopBGiay.dbo.[Order](ID) ON DELETE CASCADE);
 CREATE NONCLUSTERED INDEX IDX_Shipment_OrderID ON ShopBGiay.dbo.Shipment (  OrderID ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IDX_Shipment_ShipmentCode ON ShopBGiay.dbo.Shipment (  ShipmentCode ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- ShopBGiay.dbo.Voucher definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.Voucher;

CREATE TABLE ShopBGiay.dbo.Voucher ( ID int IDENTITY(1,1) NOT NULL, Code varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Name nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, [Type] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Discount decimal(15,2) NULL, MaxDiscount decimal(15,2) NULL, Quantity int NULL, Description nvarchar(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, StartDate datetime NULL, EndDate datetime NULL, CreateBy varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreateDate datetime NULL, UpdateBy varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, UpdateDate datetime NULL, CategoryID int NULL, CONSTRAINT PK__tmp_ms_x__3214EC2799D75436 PRIMARY KEY (ID), CONSTRAINT FK__Voucher__Categor__6E01572D FOREIGN KEY (CategoryID) REFERENCES ShopBGiay.dbo.Category(ID));


-- ShopBGiay.dbo.CartDetail definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.CartDetail;

CREATE TABLE ShopBGiay.dbo.CartDetail ( ID int IDENTITY(1,1) NOT NULL, Quantity int NULL, CartID int NULL, ProductDetailID int NULL, CONSTRAINT PK__CartDeta__3214EC27B3794465 PRIMARY KEY (ID), CONSTRAINT FK__CartDetai__CartI__5812160E FOREIGN KEY (CartID) REFERENCES ShopBGiay.dbo.Cart(ID), CONSTRAINT FK__CartDetai__Produ__59063A47 FOREIGN KEY (ProductDetailID) REFERENCES ShopBGiay.dbo.ProductDetail(ID));


-- ShopBGiay.dbo.Discount definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.Discount;

CREATE TABLE ShopBGiay.dbo.Discount ( ID int IDENTITY(1,1) NOT NULL, ProductID int NOT NULL, DiscountType nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL, DiscountValue decimal(15,2) NOT NULL, StartDate datetime NOT NULL, EndDate datetime NOT NULL, Status nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Active' NULL, CreateBy nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, CreateDate datetime DEFAULT getdate() NULL, UpdateBy nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, UpdateDate datetime NULL, CONSTRAINT PK__Discount__3214EC2744886581 PRIMARY KEY (ID), CONSTRAINT FK_Discount_Product FOREIGN KEY (ProductID) REFERENCES ShopBGiay.dbo.Product(ID) ON DELETE CASCADE);
 CREATE NONCLUSTERED INDEX IX_Discount_Dates ON ShopBGiay.dbo.Discount (  StartDate ASC  , EndDate ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Discount_ProductID ON ShopBGiay.dbo.Discount (  ProductID ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Discount_Status ON ShopBGiay.dbo.Discount (  Status ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
ALTER TABLE ShopBGiay.dbo.Discount WITH NOCHECK ADD CONSTRAINT CK_Discount_Type CHECK (([DiscountType]='Fixed' OR [DiscountType]='Percentage'));
ALTER TABLE ShopBGiay.dbo.Discount WITH NOCHECK ADD CONSTRAINT CK_Discount_Value CHECK (([DiscountValue]>=(0)));
ALTER TABLE ShopBGiay.dbo.Discount WITH NOCHECK ADD CONSTRAINT CK_Discount_Dates CHECK (([EndDate]>=[StartDate]));


-- ShopBGiay.dbo.OrderDetail definition

-- Drop table

-- DROP TABLE ShopBGiay.dbo.OrderDetail;

CREATE TABLE ShopBGiay.dbo.OrderDetail ( ID int IDENTITY(1,1) NOT NULL, Quantity int NULL, ProductName varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, Price decimal(15,2) NULL, ProductDetailID int NULL, OrderID int NULL, CreateDate datetime NULL, OriginalPrice decimal(15,2) NULL, DiscountType varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, DiscountValue decimal(15,2) NULL, FinalPrice decimal(15,2) NULL, CONSTRAINT PK__OrderDet__3214EC2734AD78DD PRIMARY KEY (ID), CONSTRAINT FK__OrderDeta__Order__619B8048 FOREIGN KEY (OrderID) REFERENCES ShopBGiay.dbo.[Order](ID), CONSTRAINT FK__OrderDeta__Produ__60A75C0F FOREIGN KEY (ProductDetailID) REFERENCES ShopBGiay.dbo.ProductDetail(ID));