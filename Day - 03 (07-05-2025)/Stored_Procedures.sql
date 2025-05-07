--Procedure to print Hello World
create procedure proc_FirstProcedure
as
begin
   print 'Hello World!'
end
Go
exec proc_FirstProcedure


create table Products
(id int identity(1,1) constraint pk_productId primary key,
name nvarchar(100) not null,
details nvarchar(max))
Go

--Procedure to print Insert Product 
create proc proc_InsertProduct(@pname nvarchar(100),@pdetails nvarchar(max))
as
begin
    insert into Products(name,details) values(@pname,@pdetails)
end
go
proc_InsertProduct 'Laptop','{"brand":"HP","spec":{"ram":"16GB","cpu":"i7"}}'
proc_InsertProduct 'Laptop','{"brand":"DELL","spec":{"ram":"16GB","cpu":"i5"}}'
proc_InsertProduct 'ASUS','{"brand":"HP","spec":{"ram":"16GB","cpu":"i7"}}'

-- To Extract JSON objects
select JSON_QUERY(details, '$.spec') Product_Specification from products

--Procedure to print Update Product 
create proc proc_UpdateProductSpec(@pid int,@newvalue varchar(20))
as
begin
   update products set details = JSON_MODIFY(details, '$.spec.ram',@newvalue) where id = @pid
end

proc_UpdateProductSpec 1, '24GB'

-- To Extract scalar value from JSON.
select id, name, JSON_VALUE(details, '$.brand') Brand_Name
from Products

create table Posts(
	id int primary key,
	title nvarchar(100),
	user_id int,
	body nvarchar(max)
)
Go

select * from Posts

-- Procedure to Bulk Insert Rows
create proc proc_BulkInsertPosts(@jsondata nvarchar(max))
as
begin
	insert into Posts(user_id,id,title,body)	
	select userId,id,title,body from openjson(@jsondata)
	with (userId int,id int, title varchar(100), body varchar(max))
end

delete from Posts

proc_BulkInsertPosts '
[
{
"userId": 1,
"id": 1,
"title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
"body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
},
{
"userId": 1,
"id": 2,
"title": "qui est esse",
"body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
}]'

select * from products

select * from products where 
try_cast(json_value(details,'$.spec.cpu') as nvarchar(20)) ='i7'

--create a procedure that brings post by taking the user_id as parameter
CREATE PROC proc_getUserPosts(@user_id INT)
AS
BEGIN
	SELECT * from Posts WHERE user_id = @user_id;
END

proc_getPosts '1';