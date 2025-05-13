-- Cursors 
-- 1) Write a cursor to list all customers and how many rentals each made. Insert these into a summary table.

CREATE TABLE customer_rental_summary (
    customer_id INT,
    total_rentals INT
);

DO $$
DECLARE
    customer_cursor CURSOR FOR SELECT customer_id, first_name, last_name FROM customer;
    cust_id INT;
    f_name TEXT;
    l_name TEXT;
    rental_count INT;
BEGIN
    OPEN customer_cursor;

    LOOP
        FETCH customer_cursor INTO cust_id, f_name, l_name;
        EXIT WHEN NOT FOUND;

        SELECT COUNT(*) INTO rental_count
        FROM rental
        WHERE rental.customer_id = cust_id;

        RAISE NOTICE 'Customer: %, % | Rentals: %', l_name, f_name, rental_count;
		INSERT INTO customer_rental_summary(customer_id, total_rentals)
		VALUES(cust_id, rental_count);
    END LOOP;

    CLOSE customer_cursor;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM customer_rental_summary;

-- 2) Using a cursor, print the titles of films in the 'Comedy' category rented more than 10 times.
DO $$ 
DECLARE
    done BOOLEAN DEFAULT FALSE;
    film_title VARCHAR(255);
    cur CURSOR FOR
        SELECT f.title
        FROM film f
        JOIN film_category fc ON f.film_id = fc.film_id
        JOIN category c ON fc.category_id = c.category_id
        JOIN inventory i ON f.film_id = i.film_id
        JOIN rental r ON i.inventory_id = r.inventory_id
        WHERE c.name = 'Comedy'
        GROUP BY f.title
        HAVING COUNT(r.rental_id) > 10;
BEGIN
    OPEN cur;

    LOOP
        FETCH cur INTO film_title;
        EXIT WHEN NOT FOUND;
        RAISE NOTICE '%', film_title;
    END LOOP;

    CLOSE cur;
END $$;

-- 3) Create a cursor to go through each store and count the number of distinct films available, and insert results into a report table.
CREATE TABLE IF NOT EXISTS store_report (
    store_id INT,
    distinct_film_count INT
);

DO $$
DECLARE
    store_rec RECORD;
    film_count INT;
    distinct_film_cursor CURSOR FOR 
        SELECT store_id FROM store;
BEGIN
    OPEN distinct_film_cursor;

    LOOP
        FETCH distinct_film_cursor INTO store_rec;
        EXIT WHEN NOT FOUND;

        SELECT COUNT(DISTINCT film_id) 
        INTO film_count 
        FROM inventory 
        WHERE store_id = store_rec.store_id;

        RAISE NOTICE 'Store ID: %, Distinct Films: %', store_rec.store_id, film_count;
		INSERT INTO store_report(store_id, distinct_film_count)
		VALUES(store_rec.store_id, film_count);
    END LOOP;

    CLOSE distinct_film_cursor;
END $$;

SELECT * FROM store_report;

-- 4) Loop through all customers who haven't rented in the last 6 months and insert their details into an inactive_customers table.
CREATE TABLE inactive_customers (
    customer_id INT PRIMARY KEY,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    email VARCHAR(100),
    last_rental_date DATE
);

DO $$
DECLARE
    cust RECORD;
    inactive_cursor CURSOR FOR
        SELECT c.customer_id, c.first_name, c.last_name, c.email,
               MAX(r.rental_date) AS last_rental_date
        FROM customer c
        LEFT JOIN rental r ON c.customer_id = r.customer_id
        GROUP BY c.customer_id
        HAVING MAX(r.rental_date) IS NULL OR MAX(r.rental_date) < CURRENT_DATE - INTERVAL '6 months';
BEGIN
    OPEN inactive_cursor;

    LOOP
        FETCH inactive_cursor INTO cust;
        EXIT WHEN NOT FOUND;

        INSERT INTO inactive_customers (customer_id, first_name, last_name, email, last_rental_date)
        VALUES (cust.customer_id, cust.first_name, cust.last_name, cust.email, cust.last_rental_date)
        ON CONFLICT (customer_id) DO NOTHING;
    END LOOP;

    CLOSE inactive_cursor;
END $$;

SELECT * FROM inactive_customers;
-----------------------------------------------------------------------------------

-- Transactions 
-- 1) Write a transaction that inserts a new customer, adds their rental, and logs the payment – all atomically.
DO $$ 
DECLARE
    v_customer_id INT;
    v_rental_id INT;
BEGIN
    INSERT INTO customer (store_id, first_name, last_name, email, address_id, active, create_date)
    VALUES (1, 'Vasumathi', 'P', 'vasu@gmail.com', 5, 1, CURRENT_DATE)
    RETURNING customer_id INTO v_customer_id;

    INSERT INTO rental (rental_date, inventory_id, customer_id, staff_id)
    VALUES (CURRENT_TIMESTAMP, 10, v_customer_id, 1)
    RETURNING rental_id INTO v_rental_id;

    INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date)
    VALUES (v_customer_id, 1, v_rental_id, 150.00, CURRENT_TIMESTAMP);

    COMMIT;
END $$;

select * from customer order by customer_id desc
select * from payment where customer_id = 610
drop TRIGGER trigger_check_outstanding_balance on rental

-- 2) Simulate a transaction where one update fails (e.g., invalid rental ID), and ensure the entire transaction rolls back.

BEGIN;
INSERT INTO rental (rental_date, inventory_id, customer_id, staff_id)
VALUES (CURRENT_TIMESTAMP, 10, 610, 1);

UPDATE rental
SET rental_date = CURRENT_TIMESTAMP
WHERE rental_id = 999909; 

ROLLBACK;


-- 3) Use SAVEPOINT to update multiple payment amounts. Roll back only one payment update using ROLLBACK TO SAVEPOINT.
BEGIN TRANSACTION;
UPDATE payment SET amount = amount + 1 WHERE payment_id = 1;
SAVEPOINT S1;
UPDATE payment SET amount = amount + 2 WHERE payment_id = 1;
SAVEPOINT S2;
UPDATE payment SET amount = amount + 3 WHERE payment_id = 1;
ROLLBACK TO SAVEPOINT S2;
COMMIT;


-- 4) Perform a transaction that transfers inventory from one store to another (delete + insert) safely.
BEGIN TRANSACTION;
DO $$
DECLARE
    v_store_id INTEGER;
BEGIN
    SELECT store_id INTO v_store_id
    FROM inventory
    WHERE inventory_id = 98;

    IF v_store_id = 1 THEN
        UPDATE inventory SET store_id = 2 WHERE inventory_id = 98;
    ELSIF v_store_id = 2 THEN
        UPDATE inventory SET store_id = 1 WHERE inventory_id = 98;
    END IF;
END $$;
COMMIT;


-- 5) Create a transaction that deletes a customer and all associated records (rental, payment), ensuring referential integrity.
BEGIN TRANSACTION;
DELETE FROM payment WHERE customer_id = 123;
DELETE FROM rental WHERE customer_id = 123;
DELETE FROM customer WHERE customer_id = 123;
COMMIT;

-------------------------------------------------------------------------------------

-- Triggers
-- 1) Create a trigger to prevent inserting payments of zero or negative amount.
CREATE OR REPLACE FUNCTION prevent_invalid_payment()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.amount <= 0 THEN
        RAISE EXCEPTION 'Invalid payment amount: %', NEW.amount;
    END IF;
    RETURN NEW;  
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER check_payment_amount
BEFORE INSERT ON payment
FOR EACH ROW
EXECUTE FUNCTION prevent_invalid_payment();

INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date)
VALUES (1, 1, 1001, 0, CURRENT_TIMESTAMP);

-- 2) Set up a trigger that automatically updates last_update on the film table when the title or rental rate is changed.
CREATE OR REPLACE FUNCTION update_last_update()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.title <> OLD.title OR NEW.rental_rate <> OLD.rental_rate THEN
        NEW.last_update := CURRENT_TIMESTAMP;
	END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_update_last_update
BEFORE UPDATE ON film
FOR EACH ROW
EXECUTE FUNCTION update_last_update();

UPDATE film SET title = 'Ace Gol D. Roger' WHERE film_id = 2;
 
-- 3) Write a trigger that inserts a log into rental_log whenever a film is rented more than 3 times in a week.
DROP TABLE rental_log
CREATE TABLE rental_log (
    log_id SERIAL PRIMARY KEY,
    film_id INT,
    rental_count INT,
    log_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION check_rental_frequency()
RETURNS TRIGGER AS $$
DECLARE
    rental_count INT;
	filmid INT;
BEGIN
	SELECT film_id INTO filmid
    FROM inventory
    WHERE inventory_id = NEW.inventory_id;

    SELECT COUNT(*)
    INTO rental_count
    FROM rental
    WHERE inventory_id = NEW.inventory_id
    AND rental_date >= CURRENT_DATE - INTERVAL '7 days';

    IF rental_count > 3 THEN
        INSERT INTO rental_log (film_id, rental_count)
        VALUES (filmid, rental_count);
    END IF;

    RETURN NEW; 
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_rental_frequency
AFTER INSERT ON rental
FOR EACH ROW
EXECUTE FUNCTION check_rental_frequency();

INSERT INTO rental (rental_date, inventory_id, customer_id, staff_id)
VALUES (CURRENT_TIMESTAMP, 2, 1, 1); 

SELECT * FROM rental_log;