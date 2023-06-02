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
	aPassword nvarchar(100) NOT NULL,
	displayname nvarchar(100) NOT NULL DEFAULT N'Chưa đặt tên',
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

SELECT * FROM Account
GO

-- Insert FoodCategory
INSERT INTO FoodCategory (fcName)
VALUES
	(N'Cà phê'),
	(N'Đá xay'),
	(N'Trà'),
	(N'Sữa chua'),
	(N'Bánh'),
	(N'Nước ép');

-- Insert Food
INSERT INTO Food (fName, idCategory, price)
VALUES
	(N'Cà phê đen',  1, 15000), 
	(N'Cà phê sữa', 1, 18000),
	(N'Bạc xỉu', 1, 20000),
	(N'Bánh bông lan trứng muối', 5, 55000),
	(N'Nước ép ổi', 16, 35000),
	(N'Trà đào', 3, 20000),
	(N'Đá xay việt quất', 2, 32000),
	(N'Sữa chua kiwi', 4, 32000),

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
GO

-- Procedure la cai ham de tai su dung nhieu lan
CREATE PROC USP_GetAccountByUsername
@userName nvarchar(50) -- Tham so truyen vo la @username
AS
BEGIN 
	SELECT * FROM Account WHERE aUsername=@userName
END
GO



CREATE PROC USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
AS
BEGIN
	--SELECT * FROM Account WHERE aUsername=@userName AND aPassword=@passWord
	SELECT * FROM Account WHERE aUsername COLLATE SQL_Latin1_General_CP1_CS_AS = @userName AND aPassword COLLATE SQL_Latin1_General_CP1_CS_AS = @passWord
END
GO


DECLARE @i INT = 0
WHILE @i <= 22
BEGIN
	INSERT FoodTable (tName) VALUES (N'Bàn ' + CAST(@i AS nvarchar(50)))
	SET @i = @i + 1
END

SELECT * FROM FoodTable
GO


ALTER PROC USP_GetTableList
AS 
	SELECT id , tName, tStatus FROM FoodTable
GO

EXEC USP_GetTableList
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


CREATE TRIGGER UTG_DeleteBillDetail ON BillDetail
FOR DELETE
AS
BEGIN
	DECLARE @idBillDetail INT
	DECLARE @idBill INT
	SELECT @idBillDetail = id, @idBill = DELETED.idBill FROM DELETED

	DECLARE @idTable INT
	SELECT @idTable = idTable FROM Bill WHERE id = @idBill
	DECLARE @count INT = 0
	SELECT @count = COUNT(*) FROM BillDetail AS bd, Bill AS b WHERE b.id = bd.idBill AND b.billStatus = 0 AND b.id = @idBill

	IF (@count = 0)
		UPDATE FoodTable SET tStatus = N'Trống' WHERE id = @idTable
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

-- Hàm có sẵn: chuyển có dấu về không dấu
-- Ý tưởng: chuyển cả input và data về không dấu và tìm kiếm
CREATE FUNCTION  ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END
GO


CREATE PROC USP_GetNumBillByDate
@checkIn date, @checkOut date
AS 
BEGIN
	SELECT COUNT(*)
	FROM dbo.Bill AS b, FoodTable AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.billStatus = 1
	AND t.id = b.idTable
END
GO

CREATE PROC USP_GetListBillByDateAndPage
@checkIn DATE, @checkOut DATE, @page INT
AS
BEGIN
	DECLARE @pageRows INT = 14
	DECLARE @selectRows INT = @pageRows
	DECLARE @exceptRow INT = (@page - 1) * @pageRows
	
	-- Tạo ra bảng tạm
	;WITH BillShow AS (SELECT b.id AS ID, tName, dateCheckin, dateCheckout, b.totalPrice
	FROM Bill AS b, FoodTable AS ft --, Food AS f
	WHERE dateCheckin >= @checkIn AND dateCheckout <= @checkOut AND billStatus = 1 AND b.idTable = ft.id)

	SELECT TOP (@pageRows) ID , tName AS N'Tên bàn', dateCheckin AS N'Ngày ra', dateCheckout AS N'Ngày vào', totalPrice AS N'Tổng tiền' 
	FROM BillShow WHERE ID NOT IN (SELECT TOP (@exceptRow) ID FROM BillShow)
END
GO


CREATE PROC USP_GetTotalStatisticByDate
@checkIn date, @checkOut date
AS 
BEGIN
	SELECT SUM(totalPrice) FROM Bill WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut
END
GO
