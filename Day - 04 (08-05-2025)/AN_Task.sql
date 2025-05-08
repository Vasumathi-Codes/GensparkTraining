
--1) List all orders with the customer name and the employee who handled the order.
--(Join Orders, Customers, and Employees)

SELECT 
    o.OrderID,
    c.CompanyName AS CustomerName,
    e.FirstName + ' ' + e.LastName AS EmployeeName,
    o.OrderDate
FROM Orders o
JOIN Customers c ON o.CustomerID = c.CustomerID
JOIN Employees e ON o.EmployeeID = e.EmployeeID;


--2) Get a list of products along with their category and supplier name.
--(Join Products, Categories, and Suppliers)

SELECT p.ProductId, p.ProductName, c.CategoryName, s.CompanyName as SupplierName
FROM Products p
JOIN Categories c ON p.CategoryID = c.CategoryID
JOIN Suppliers s ON p.SupplierID = s.SupplierID


--3) Show all orders and the products included in each order with quantity and unit price.
--(Join Orders, Order Details, Products)

SELECT o.OrderID, p.ProductName, od.UnitPrice, od.Quantity 
FROM Orders o
JOIN [Order Details] od ON o.OrderID = od.OrderID
JOIN Products p ON od.ProductID = p.ProductID
ORDER BY 1


--4) List employees who report to other employees (manager-subordinate relationship).
--(Self join on Employees)

SELECT 
    e.EmployeeID AS SubordinateID,
    e.FirstName + ' ' + e.LastName AS SubordinateName,
    m.EmployeeID AS ManagerID,
    m.FirstName + ' ' + m.LastName AS ManagerName
FROM Employees e
JOIN Employees m ON e.ReportsTo = m.EmployeeID


--5) Display each customer and their total order count.
--(Join Customers and Orders, then GROUP BY)

SELECT c.CustomerID, c.CompanyName as CustomerName, count(*) as TotalOrders
FROM Customers c
JOIN Orders o ON o.CustomerID = c.CustomerID
GROUP BY c.CustomerID, c.CompanyName
ORDER BY 3 DESC


--6) Find the average unit price of products per category.
--Use AVG() with GROUP BY

SELECT c.CategoryName, AVG(p.UnitPrice) as AverageUnitPrice
FROM Products p
JOIN Categories c ON c.CategoryID = p.CategoryID
GROUP BY c.CategoryID, c.CategoryName


--7) List customers where the contact title starts with 'Owner'.
--Use LIKE or LEFT(ContactTitle, 5)

SELECT * FROM Customers WHERE ContactTitle LIKE 'Owner%'
SELECT * FROM Customers WHERE LEFT(ContactTitle, 5) = 'Owner'


--8) Show the top 5 most expensive products.
--Use ORDER BY UnitPrice DESC and TOP 5

SELECT TOP 5 * FROM Products
ORDER BY UnitPrice DESC


--9) Return the total sales amount (quantity × unit price) per order.
--Use SUM(OrderDetails.Quantity * OrderDetails.UnitPrice) and GROUP BY

SELECT e.EmployeeID, 
	   e.FirstName + ' ' + e.LastName AS EmployeeName,
	   SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS TotalSales
FROM Employees e
JOIN Orders o ON e.EmployeeID = o.EmployeeID
JOIN [Order Details] od ON o.OrderID = od.OrderID
GROUP BY e.EmployeeID, e.FirstName, e.LastName;


--10) Create a stored procedure that returns all orders for a given customer ID.
--Input: @CustomerID

CREATE OR ALTER PROC proc_getAllOrderOfCustomer(@custId nvarchar(20))
AS
BEGIN
	SELECT OrderID, OrderDate 
	FROM Orders WHERE CustomerID = @custId
END
GO
EXEC proc_getAllOrderOfCustomer ALFKI


--11) Write a stored procedure that inserts a new product.
--Inputs: ProductName, SupplierID, CategoryID, UnitPrice, etc.

CREATE PROC proc_insertProduct(@ProductName nvarchar(40), @SupplierID int, @CategoryID int, @UnitPrice money)
as
BEGIN
	INSERT INTO Products(ProductName, SupplierID, CategoryID, UnitPrice) 
	VALUES(@ProductName, @SupplierID, @CategoryID, @UnitPrice)
END
GO
proc_insertProduct 'Appy Juice', 16, 1, 10 


--12) Create a stored procedure that returns total sales per employee.
--Join Orders, Order Details, and Employees

CREATE OR ALTER PROCEDURE proc_getTotalSalesPerEmployee
AS
BEGIN
    SELECT 
        e.EmployeeID, 
        e.FirstName + ' ' + e.LastName AS EmployeeName,
        SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS TotalSales
    FROM Employees e
    JOIN Orders o ON e.EmployeeID = o.EmployeeID
    JOIN [Order Details] od ON o.OrderID = od.OrderID
    GROUP BY e.EmployeeID, e.FirstName, e.LastName
END
GO
EXEC proc_getTotalSalesPerEmployee;


--13) Use a CTE to rank products by unit price within each category.
--Use ROW_NUMBER() or RANK() with PARTITION BY CategoryID

WITH RankedProducts AS (
    SELECT 
        ProductId, 
        ProductName, 
        CategoryID, 
        UnitPrice,
        ROW_NUMBER() OVER (PARTITION BY CategoryID ORDER BY UnitPrice DESC) AS Row_Num
    FROM Products
)
SELECT * FROM RankedProducts;


--14) Create a CTE to calculate total revenue per product and filter products with revenue > 10,000.

WITH ProductRevenue AS (
    SELECT 
        p.ProductID,
        p.ProductName,
        SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS TotalRevenue
    FROM [Order Details] od
    JOIN Products p ON p.ProductID = od.ProductID
    GROUP BY p.ProductID, p.ProductName
)
SELECT * 
FROM ProductRevenue
WHERE TotalRevenue > 10000;


--15) Use a CTE with recursion to display employee hierarchy.
--Start from top-level employee (ReportsTo IS NULL) and drill down

WITH EmployeeHierarchy AS (
    SELECT 
        EmployeeID, FirstName + ' ' + LastName AS EmployeeName, ReportsTo, 1 AS Level
    FROM Employees
    WHERE ReportsTo IS NULL

    UNION ALL

    SELECT 
        e.EmployeeID, e.FirstName + ' ' + e.LastName, e.ReportsTo, eh.Level + 1
    FROM Employees e
    INNER JOIN EmployeeHierarchy eh ON e.ReportsTo = eh.EmployeeID
)
SELECT * FROM EmployeeHierarchy
ORDER BY Level, EmployeeName;
