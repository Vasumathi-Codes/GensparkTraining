-- CURSOR
-- Create rental_tax_log Table
CREATE TABLE rental_tax_log (
    rental_id INT,
    customer_name TEXT,
    rental_date TIMESTAMP,
    amount NUMERIC,
    tax NUMERIC
);

-- Insert Data into rental_tax_log using Cursor
DO $$
DECLARE
    rec RECORD;
    cur CURSOR FOR
        SELECT r.rental_id, 
               c.first_name || ' ' || c.last_name AS customer_name,
               r.rental_date,
               p.amount
        FROM rental r
        JOIN payment p ON r.rental_id = p.rental_id
        JOIN customer c ON r.customer_id = c.customer_id;
BEGIN
    OPEN cur;

    LOOP
        FETCH cur INTO rec;
        EXIT WHEN NOT FOUND;

        INSERT INTO rental_tax_log (rental_id, customer_name, rental_date, amount, tax)
        VALUES (
            rec.rental_id,
            rec.customer_name,
            rec.rental_date,
            rec.amount,
            rec.amount * 0.10
        );
    END LOOP;

    CLOSE cur;
END;
$$;

-- Display Data Using Cursor and Raise Notice for Each Record
DO $$ 
DECLARE
    rental_record RECORD;
    rental_cursor CURSOR FOR
        SELECT r.rental_id, c.first_name, c.last_name, r.rental_date
        FROM rental r
        JOIN customer c ON r.customer_id = c.customer_id
        ORDER BY r.rental_id;
BEGIN
    OPEN rental_cursor;

    LOOP
        FETCH rental_cursor INTO rental_record;
        EXIT WHEN NOT FOUND;

        -- Display the rental details using raise notice
        RAISE NOTICE 'rental id: %, customer: % %, date: %',
                     rental_record.rental_id,
                     rental_record.first_name,
                     rental_record.last_name,
                     rental_record.rental_date;
    END LOOP;

    CLOSE rental_cursor;
END;
$$;
-------------------------------------------------------------------------------------
-- Triggers
-- Create audit_log Table for Logging Changes
CREATE TABLE audit_log (
    audit_id SERIAL PRIMARY KEY,
    table_name TEXT,
    field_name TEXT,
    old_value TEXT,
    new_value TEXT,
    updated_date TIMESTAMP DEFAULT current_timestamp
);

-- Create Function to Log Changes
CREATE OR REPLACE FUNCTION Update_Audit_log()
RETURNS TRIGGER AS $$
DECLARE
    col_name TEXT := TG_ARGV[0];
    tab_name TEXT := TG_ARGV[1];
    o_value TEXT;
    n_value TEXT;
BEGIN
    -- Fetch old and new values for the specified column
    EXECUTE FORMAT('SELECT ($1).%I::TEXT', col_name) INTO o_value USING OLD;
    EXECUTE FORMAT('SELECT ($1).%I::TEXT', col_name) INTO n_value USING NEW;

    -- If values are different, log the change in audit_log
    IF o_value IS DISTINCT FROM n_value THEN
        INSERT INTO audit_log(table_name, field_name, old_value, new_value, updated_date) 
        VALUES (tab_name, col_name, o_value, n_value, current_timestamp);
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger to Log Email Changes for Customer
CREATE TRIGGER trg_log_customer_email_change
AFTER UPDATE
ON customer
FOR EACH ROW
EXECUTE FUNCTION Update_Audit_log('email', 'customer');

-- Example Update: Update customer email and check audit_log
UPDATE customer 
SET email = 'mary.smiths@sakilacustomer.org' 
WHERE customer_id = 1;

-- Drop the Trigger (if needed)
DROP TRIGGER trg_log_customer_email_change ON customer;

-- Display Data from audit_log (for verification)
SELECT * FROM audit_log;

-- Drop Trigger and Table if Needed
DROP TRIGGER trg_log_customer_email_change ON customer;
DROP TABLE audit_log;

-- Final Check: Verify customer and audit_log Data
SELECT * FROM customer ORDER BY customer_id;
SELECT * FROM audit_log;
