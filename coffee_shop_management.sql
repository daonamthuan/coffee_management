CREATE DATABASE  COFFEESHOPMANAGEMENT
GO

USE COFFEESHOPMANAGEMENT
GO

-- FOOD
-- TABLE
-- FOODCATEGORY
-- ACCOUNT
-- BILL
-- BILLDETAIL

SET DATEFORMAT DMY

CREATE TABLE FoodTable
(
	id int identity PRIMARY KEY, -- identity là tự tăng giá trị
	tName nvarchar(100) NOT NULL DEFAULT N'Chưa đặt tên',
	tStatus nvarchar(100) NOT NULL DEFAULT N'Trống'
)
GO

CREATE TABLE Account
(
	aUsername nvarchar(100) PRIMARY KEY,
	aPassword nvarchar(1000) NOT NULL,
	displayname nvarchar(100) NOT NULL DEFAULT N'Kter',
	aType int NOT NULL DEFAULT 0		-- 1 la admin, 0 la staff
)
GO

CREATE TABLE FoodCategory
(
	id int identity PRIMARY KEY,
	fcName nvarchar(100) DEFAULT N'Chưa đặt tên',
)
GO

CREATE TABLE Food
(
	id int identity PRIMARY KEY,
	fName nvarchar(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory int FOREIGN KEY REFERENCES FoodCategory(id) NOT NULL,
	price float NOT NULL DEFAULT 0,
)
GO

ALTER TABLE Bill ADD dateCheckout DATE

CREATE TABLE Bill
(
	id int identity PRIMARY KEY,
	dateCheckin DATE NOT NULL DEFAULT GETDATE(),
	dateCheckout DATE,
	idTable int FOREIGN KEY REFERENCES FoodTable(id) NOT NULL,
	--totalBill float NOT NULL,
	billStatus int NOT NULL	DEFAULT 0	-- 1 la thanh toan, 0 la chua	
)
GO

CREATE TABLE BillDetail
(
	id int identity PRIMARY KEY,
	idBill int FOREIGN KEY REFERENCES Bill(id) NOT NULL,
	idFood int FOREIGN KEY REFERENCES Food(id) NOT NULL,
	quantity int NOT NULL DEFAULT 0
)
GO

INSERT INTO Account (aUsername, displayname, aPassword, aType)
VALUES
	(N'K9', N'RongK9', N'1', 1),
	(N'staff', N'staff', N'1', 0);

SELECT * FROM Account
GO

-- Insert FoodCategory
INSERT INTO FoodCategory (fcName)
VALUES
	(N'Hải sản'),
	(N'Nông sản'),
	(N'Lâm sản'),
	(N'Sản sản'),
	(N'Nước');

-- Insert Food
INSERT INTO Food (fName, idCategory, price)
VALUES
	(N'Mực một nắng nướng sa tế',  1, 120000), 
	(N'Nghêu hấp xả', 1, 50000),
	(N'Dú dê nướng sữa', 2, 60000),
	(N'Heo rừng nướng muối ớt', 3, 75000),
	(N'Cơm chiên mushi', 4, 99999),
	(N'7up', 5, 15000),
	(N'Cafe', 2, 12000);

-- Insert Bill
INSERT INTO Bill (dateCheckin, dateCheckout, idTable, billStatus)
VALUES
	(GETDATE(), NULL, 3, 0),
	(GETDATE(), NULL, 4, 0),
	(GETDATE(), GETDATE(), 5, 1);

-- Insert BillDetail
INSERT INTO BillDetail(idBill, idFood, quantity)
VALUES
	(2, 1, 2), 
	(2, 3, 4),
	(2, 5, 1),
	(3, 1, 2),
	(3, 6, 2),
	(4, 5, 2);


-- Procedure la cai ham de tai su dung nhieu lan
CREATE PROC USP_GetAccountByUsername
@userName nvarchar(50) -- Tham so truyen vo la @username
AS
BEGIN 
	SELECT * FROM Account WHERE aUsername=@userName
END
GO

-- Goi ham
EXEC USP_GetAccountByUsername @userName = N'K9' 
GO


CREATE PROC USP_Login
@userName nvarchar(50), @passWord nvarchar(50)
AS
BEGIN
	SELECT * FROM Account WHERE aUsername=@userName AND aPassword=@passWord
END
GO

DECLARE @i INT = 0
WHILE @i <= 10
BEGIN
	INSERT FoodTable (tName) VALUES (N'Bàn ' + CAST(@i AS nvarchar(50)))
	SET @i = @i + 1
END

SELECT * FROM FoodTable
GO

DECLARE @i INT = 0
WHILE @i <= 10
BEGIN
	INSERT FoodTable (tName) VALUES (N'Bàn ' + CAST(@i AS nvarchar(50)))
	SET @i = @i + 1
END

SELECT * FROM FoodTable
GO

CREATE PROC USP_GetTableList
AS SELECT * FROM FoodTable
GO

EXEC USP_GetTableList
GO


DECLARE @i INT = 22
WHILE @i > 0
BEGIN
	UPDATE FoodTable SET tName = (N'Bàn ' + CAST(@i AS nvarchar(50))) WHERE id = @i
	SET @i = @i - 1
END
GO

CREATE PROC USP_InsertBill
@idTable INT
AS
BEGIN
	INSERT INTO Bill (dateCheckin, dateCheckout, idTable, billStatus)
	VALUES (GETDATE(), NULL, @idTable, 0);
END
GO

CREATE PROC USP_InsertBillDetail
@idBill INT, @idFood INT, @quantity INT
AS
BEGIN
	INSERT INTO BillDetail (idBill, idFood, quantity)
	VALUES (@idBill, @idFood, @quantity);
END
GO

-- Xử lí 2 trường hợp thêm BillDetail khi chưa có món đó trong BillDetail, và khi đã có (thay đổi số lượng)
CREATE PROC USP_InsertBillDetail
@idBill INT, @idFood INT, @quantity INT
AS
BEGIN
	DECLARE @isExistBillDetail INT = 0;
	DECLARE @currentQuantity INT = 0;
	SELECT @isExistBillDetail = b.id, @currentQuantity = b.quantity FROM BillDetail as b WHERE idBill = @idBill AND idFood = @idFood
	IF (@isExistBillDetail > 0) -- Nếu bill đã có món => cập nhật lại số lượng
	BEGIN
		DECLARE @newQuantity INT = @currentQuantity + @quantity -- Trường hợp thêm vào số lượng âm => kiểm tra còn > 0 hay không
		IF (@newQuantity > 0)
			UPDATE BillDetail SET quantity = @newQuantity WHERE idBill = @idBill AND idFood = @idFood
		ELSE
			DELETE BillDetail WHERE idBill = @idBill AND idFood = @idFood
	END
	ELSE IF (@isExistBillDetail <= 0 AND @quantity > 0) -- Nếu bill chưa có món
	BEGIN
		INSERT BillDetail (idBill, idFood, quantity)
		VALUES (@idBill, @idFood, @quantity);
	END
END
GO


/*SELECT * FROM Bill
GO
SELECT * FROM BillDetail
SELECT * FROM Food
SELECT * FROM FoodTable
SELECT * FROM FoodCategory

SELECT * FROM BillDetail WHERE idBill = 2
SELECT * FROM Bill WHERE idTable = 3 AND billStatus = 0 */

/*SELECT f.fName, bd.quantity, f.price, f.price*bd.quantity as totalPrice FROM BillDetail as bd, Bill as b, Food as f
WHERE b.id=bd.idBill AND f.id=bd.idFood AND b.idTable=3

SELECT f.fName, bd.quantity, f.price, f.price*bd.quantity as totalPrice FROM BillDetail as bd, Bill as b, Food as f WHERE b.id = bd.idBill AND f.id = bd.idFood AND b.idTable = 3 AND b.billStatus = 0

SELECT * FROM FoodCategory
SELECT * FROM Food

SELECT * FROM Food WHERE idCategory = 5

EXEC USP_InsertBill 3
EXEC USP_InsertBillDetail 3, 1, 1 */

--UPDATE Bill SET billStatus = 1 WHERE id = 1
--GO

CREATE TRIGGER UTG_UpdateBillDetail ON BillDetail 
FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBill = idBill FROM INSERTED
	
	DECLARE @idTable INT
	SELECT @idTable = idTable FROM Bill WHERE id = @idBill AND billStatus = 0

	UPDATE FoodTable SET tStatus = N'Có người' WHERE id = @idTable
END
GO

CREATE TRIGGER UTG_UpdateBill ON Bill
FOR UPDATE 
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBill = id FROM INSERTED
		
	DECLARE @idTable INT
	SELECT @idTable = idTable FROM Bill WHERE id = @idBill

	DECLARE @count INT = 0
	SELECT @count = COUNT(*) FROM Bill WHERE idTable = @idTable AND billStatus = 0
	IF (@count = 0) -- Tức là không có bill nào chưa thanh toán cho bàn này
		UPDATE FoodTable SET tStatus = N'Trống' WHERE id = @idTable
END
GO

--DELETE BillDetail
--DELETE Bill

SET DATEFORMAT DMY

ALTER TABLE Bill ADD totalPrice FLOAT DEFAULT 0
GO
-- create store procedure at 
CREATE PROC USP_GetListBillByDate
@checkIn DATE, @checkOut DATE
AS
BEGIN
	SELECT tName AS N'Tên bàn', dateCheckin AS N'Ngày ra', dateCheckout AS N'Ngày vào', b.totalPrice AS N'Tổng tiền'
	FROM Bill AS b, FoodTable AS ft --, Food AS f
	WHERE dateCheckin >= @checkIn AND dateCheckout <= @checkOut AND billStatus = 1 AND b.idTable = ft.id
END
GO

SELECT tName AS N'Tên bàn', dateCheckin AS N'Ngày ra', dateCheckout AS N'Ngày vào', b.totalPrice AS N'Tổng tiền'
FROM Bill AS b, FoodTable AS ft --, Food AS f
WHERE dateCheckin >= '20230501' AND dateCheckout <= '20230530' AND billStatus = 1 AND b.idTable = ft.id

EXEC USP_GetListBillByDate @checkIn = '20230501' , @checkOut = '20230530'

SELECT * FROM Account
GO

CREATE PROC USP_UpdateAccount
@username nvarchar(50), @displayName nvarchar(50), @password nvarchar(50), @newpassword nvarchar(50)
AS
BEGIN
	DECLARE @isRightPass INT = 0

	SELECT @isRightPass = COUNT(*) FROM Account WHERE aUsername = @username AND aPassword = @password 

	IF (@isRightPass = 1)
	BEGIN
		IF (@newpassword = NULL OR @newpassword = '')
		BEGIN
			UPDATE Account SET displayname = @displayName WHERE aUsername = @username
		END
		ELSE
			UPDATE Account SET displayname = @displayName, aPassword = @newpassword WHERE aUsername = @username
	END
END
GO