use pubs
go

select * from publishers

select * from titles

select title,pub_id from titles

select title, pub_name 
from titles join publishers
on titles.pub_id = publishers.pub_id

select title, pub_name, publishers.pub_id 
from titles join publishers 
on titles.pub_id = publishers.pub_id

select title, pub_name, p.pub_id 
from titles t join publishers p
on t.pub_id = p.pub_id

--print the publisher deatils of the publisher who has never published
select * from publishers where pub_id not in
(select distinct pub_id from titles)

select title, pub_name 
from titles right outer join publishers
on titles.pub_id = publishers.pub_id

--Select the author_id for all the books. Print the author_id and the book name
SELECT au_id, title 
FROM titleauthor ta
JOIN titles t
ON ta.title_id = t.title_id;

select * from authors

select concat(au_fname,' ',au_lname), title_id Author_Name from authors a
join titleauthor ta on a.au_id = ta.au_id
order by title_id

SELECT 
    a.au_fname + ' ' + a.au_lname AS author_name,
    t.title AS book_name
FROM 
    authors a
JOIN 
    titleauthor ta ON a.au_id = ta.au_id
JOIN 
    titles t ON ta.title_id = t.title_id;


select * from sales

--Print the publisher's name, book name and the order date of the  books
SELECT pub_name Publisher_Name, title Book_Name, ord_date Order_Date
FROM publishers p 
JOIN titles t ON p.pub_id = t.pub_id
JOIN sales s ON s.title_id = t.title_id
order by 3 desc

--Print the publisher name and the first book sale date for all the publishers
SELECT pub_name Publisher_Name, min(ord_date) First_Order_Date
FROM publishers p 
left outer JOIN titles t ON p.pub_id = t.pub_id
left outer JOIN sales s ON s.title_id = t.title_id
group by  pub_name
order by 2 desc

select * from stores

--print the bookname and teh store address of the sale
SELECT
    T.title AS Book_Name,
    CONCAT(S.stor_address,',', S.city, ',', S.state) AS Store_Address
FROM
    sales AS SL 
JOIN
    titles AS T ON SL.title_id = T.title_id
JOIN
    stores AS S ON SL.stor_id = S.stor_id
ORDER BY 1;

