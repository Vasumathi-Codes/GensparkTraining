-- SELECT Queries
-- List all films with their length and rental rate, sorted by length descending.
-- Columns: title, length, rental_rate
SELECT title, length, rental_rate from film order by length desc;

-- Find the top 5 customers who have rented the most films.
-- Hint: Use the rental and customer tables.
SELECT c.customer_id, count(r.rental_id) as TotalRentals
FROM customer c
JOIN rental r on c.customer_id = r.customer_id
GROUP BY c.customer_id
ORDER BY 2 desc
LIMIT 5;

-- Display all films that have never been rented.
-- Hint: Use LEFT JOIN between film and inventory → rental.
SELECT f.film_id, f.title
FROM film f
LEFT JOIN inventory i ON f.film_id = i.film_id
LEFT JOIN rental r ON i.inventory_id = r.inventory_id
WHERE r.rental_id IS NULL;

-- JOIN Queries
-- List all actors who appeared in the film ‘Academy Dinosaur’.
-- Tables: film, film_actor, actor
SELECT a.actor_id, CONCAT(a.first_name ,a.last_name) AS ActorName
FROM film f
JOIN film_actor fa ON f.film_id = fa.film_id
JOIN actor a ON fa.actor_id = a.actor_id
WHERE f.title = 'Academy Dinosaur'

-- List each customer along with the total number of rentals they made and the total amount paid.
-- Tables: customer, rental, payment
SELECT 
	c.customer_id, 
	CONCAT(c.first_name ,c.last_name) AS customer_name, 
	COUNT(r.rental_id) as Total_Rentals, 
	SUM(p.amount) as Total_Amount
FROM customer c
JOIN rental r ON c.customer_id = r.customer_id
JOIN payment p ON r.rental_id = p.rental_id
GROUP BY c.customer_id, customer_name
ORDER BY 3 desc;

-- CTE-Based Queries
-- Using a CTE, show the top 3 rented movies by number of rentals.
-- Columns: title, rental_count
WITH TopRentedMovies AS
(
	SELECT f.title, COUNT(r.rental_id) AS rental_count
	FROM film f
	JOIN inventory i  ON f.film_id = i.film_id
	JOIN rental r ON i.inventory_id = r.inventory_id
	GROUP BY f.title
	ORDER BY 2 DESC
)
SELECT * FROM TopRentedMovies LIMIT 3

-- Find customers who have rented more than the average number of films.
-- Use a CTE to compute the average rentals per customer, then filter.
WITH CustomerRentals AS
(
	SELECT c.customer_id, CONCAT(c.first_name ,c.last_name) AS customer_name, COUNT(r.rental_id) as total_rent 
	FROM customer c 
	JOIN rental r ON c.customer_id = r.customer_id
	GROUP BY c.customer_id
	ORDER BY 3 DESC
)
SELECT * FROM CustomerRentals WHERE total_rent > (SELECT AVG(total_rent) FROM CustomerRentals)

--  Function Questions
-- Write a function that returns the total number of rentals for a given customer ID.
-- Function: get_total_rentals(customer_id INT)
CREATE FUNCTION fn_get_total_rentals(cust_id INT)
returns INT AS $$
DECLARE 
	total_rentals INT;
BEGIN
	SELECT COUNT(rental_id) into total_rentals FROM rental
	where customer_id = cust_id;
	RETURN total_rentals;
END;
$$ LANGUAGE plpgsql;

SELECT fn_get_total_rentals(2);

-- Stored Procedure Questions
-- Write a stored procedure that updates the rental rate of a film by film ID and new rate.
-- Procedure: update_rental_rate(film_id INT, new_rate NUMERIC)
CREATE OR REPLACE PROCEDURE update_rental_rate(fm_id INT, new_rate NUMERIC)
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE film
    SET rental_rate = new_rate
    WHERE film_id = fm_id;

END;
$$;

CALL update_rental_rate(10, 4.99);


-- Write a procedure to list overdue rentals (return date is NULL and rental date older than 7 days).
-- Procedure: get_overdue_rentals() that selects relevant columns.
CREATE OR REPLACE FUNCTION get_overdue_rentals()
RETURNS TABLE(rental_id INT, customer_id SMALLINT, rental_date TIMESTAMP, return_date TIMESTAMP) 
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT r.rental_id, r.customer_id, r.rental_date, r.return_date
    FROM rental r
    WHERE r.return_date IS NULL 
    AND (CURRENT_DATE - r.rental_date) > INTERVAL '7 days';
END;
$$;

SELECT * FROM get_overdue_rentals();
