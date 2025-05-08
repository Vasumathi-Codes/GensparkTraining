-- Procedure to get CPU as input and return the computers count with that CPU
create proc proc_FilterProducts(@pcpu nvarchar(20), @pcnt int out)
AS
BEGIN
 select @pcnt = count(*) from products where
 TRY_CAST(json_value(details, '$.spec.cpu') as nvarchar(20)) = @pcpu;
END

BEGIN
DECLARE @cnt int
EXEC proc_FilterProducts 'i7', @cnt out
print concat('The number of computers with cpu i7 is ', @cnt)
END

--Predefined procedure
sp_help authors;


create table people
(id int primary key,
name nvarchar(20),
age int)

--Procedure to Bulk Insert values from csv file
create or alter proc proc_BulkInsert(@filepath nvarchar(500))
as
begin
   declare @insertQuery nvarchar(max)
   set @insertQuery = 'BULK INSERT people from '''+ @filepath +'''
   with(
   FIRSTROW =2,
   FIELDTERMINATOR='','',
   ROWTERMINATOR = ''\n'')'
   exec sp_executesql @insertQuery
end

proc_BulkInsert 'C:\Users\VC\Documents\Genspark_Training\Book1.csv'

select * from people

create table BulkInsertLog
(LogId int identity(1,1) primary key,
FilePath nvarchar(1000),
status nvarchar(50) constraint chk_status Check(status in('Success','Failed')),
Message nvarchar(1000),
InsertedOn DateTime default GetDate())

--Procedure to Bulk Insert values from csv file and log into BulkInsertLog table
create or alter proc proc_BulkInsert(@filepath nvarchar(500))
as
begin
  Begin try
	   declare @insertQuery nvarchar(max)

	   set @insertQuery = 'BULK INSERT people from '''+ @filepath +'''
	   with(
	   FIRSTROW =2,
	   FIELDTERMINATOR='','',
	   ROWTERMINATOR = ''\n'')'

	   exec sp_executesql @insertQuery

	   insert into BulkInsertLog(filepath,status,message)
	   values(@filepath,'Success','Bulk insert completed')
  end try
  begin catch
		 insert into BulkInsertLog(filepath,status,message)
		 values(@filepath,'Failed',Error_Message())
  END Catch
end

proc_BulkInsert 'C:\Users\VC\Documents\Genspark_Training\Book1.csv'
select * from BulkInsertLog;


--CTE - Common Table Expressions
WITH cteAuthors AS
(SELECT au_id, concat(au_fname,' ',au_lname) author_name from authors)

SELECT * FROM cteAuthors;


declare @page int =2, @pageSize int=10;
with PaginatedBooks as
( select  title_id,title, price, ROW_Number() over (order by price desc) as RowNum
  from titles
)
select * from PaginatedBooks where rowNUm between((@page-1)*@pageSize) and (@page*@pageSize)


--create a sp that will take the page number and size as param and print the books
CREATE OR ALTER PROC proc_printBooks(@page INT, @pageSize INT)
AS
BEGIN
  SELECT *
  FROM (
    SELECT title_id, title, price,
           ROW_NUMBER() OVER (ORDER BY price DESC) AS RowNum
    FROM titles
  ) AS sub
  WHERE RowNum BETWEEN ((@page - 1) * @pageSize +1) AND (@page * @pageSize)
END

proc_PrintBooks 1, 10;
proc_PrintBooks 2, 10;


-- OFFSET skips the first n rows.
-- FETCH retrieves the next n rows after offset.
-- Used for pagination in queries.
SELECT title_id, title, price 
FROM titles
ORDER BY price desc
offset 2 rows fetch next 10 rows only

--Functions
create function fn_CalculateTax(@baseprice float, @tax float)
returns float
as
begin
    return (@baseprice +(@baseprice*@tax/100))
end

select dbo.fn_CalculateTax(1000,10)
select title,dbo.fn_CalculateTax(price,12) from titles

create or alter function fn_tableSample(@minprice float)
returns table
as
return select title,price from titles where price>= @minprice

select * from dbo.fn_tableSample(10)


--older and slower but supports more logic
create function fn_tableSampleOld(@minprice float)
returns @Result table(Book_Name nvarchar(100), price float)
as
begin
insert into @Result select title,price from titles where price>= @minprice
return 
end

select * from dbo.fn_tableSampleOld(10)