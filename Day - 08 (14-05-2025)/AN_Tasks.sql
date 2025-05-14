--1) Set up streaming replication

-- Initialize Primary and Secondary Clusters
-- Navigate to PostgreSQL binaries:
cd "C:\Program Files\PostgreSQL\17\bin"

-- Initialize the Primary Cluster:
initdb -D "C:/primary"

-- Initialize the Secondary Cluster:
initdb -D "C:/secondary"

-- Start Primary Server on Port 5433
pg_ctl -D C:/primary -o "-p 5433" -l C:/primary/logfile start
-- Output: server started

-- Create Replication Role
psql -p 5433 -d postgres -c "CREATE ROLE replicator WITH REPLICATION LOGIN PASSWORD 'repl_pass';"
-- Output: CREATE ROLE

-- Restart Primary Server (Optional)
pg_ctl -D C:/primary restart

-- Perform Base Backup for Secondary
-- Ensure C:/secondary is empty before proceeding.
pg_basebackup -D C:/secondary -Fp -Xs -P -R -h 127.0.0.1 -U replicator -p 5433
-- Output confirms 100% base backup completed.

-- Start Secondary Server on Port 5435
pg_ctl -D C:/secondary -o "-p 5435" -l C:/secondary/logfile start
-- Output: server started

-- Validate Streaming Replication
-- Connect to the primary:
psql -p 5433 -d postgres

-- Run the replication status check: 
SELECT * FROM pg_stat_replication;
-- This confirms that the secondary is streaming from the primary.

-- Create Table and Insert Data on Primary
-- Connect to the primary:
psql -p 5433 -d postgres

-- Create table and insert values:
CREATE TABLE test_table (
    id INT PRIMARY KEY,
    name TEXT
);
INSERT INTO test_table VALUES (1, 'Vasumathi');
-- Validate Streaming Replication on Secondary

-- Connect to the secondary:
psql -p 5435 -d postgres

-- Check replication status:
SELECT pg_is_in_recovery();

-- Confirm data is replicated:
SELECT * FROM test_table;


-----------------------------------------------------------------------------------------
-- Creating a table rental_log
CREATE TABLE rental_log (
    log_id SERIAL PRIMARY KEY,
    rental_time TIMESTAMP,
    customer_id INT,
    film_id INT,
    amount NUMERIC,
    logged_on TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Procedure to add new rental log entry
CREATE OR REPLACE PROCEDURE sp_add_rental_log(
    p_customer_id INT,
    p_film_id INT,
    p_amount NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO rental_log (rental_time, customer_id, film_id, amount)
    VALUES (CURRENT_TIMESTAMP, p_customer_id, p_film_id, p_amount);
EXCEPTION WHEN OTHERS THEN
    RAISE NOTICE 'Error occurred: %', SQLERRM;
END;
$$;

-- Calling the procedure on the primary:
CALL sp_add_rental_log(1, 100, 4.99);

-- Verifying the data entry from Secondary server (standby)
SELECT * FROM rental_log ORDER BY log_id DESC LIMIT 1;


---------------------------------------------------------------------------------------
--Trigger to log any UPDATE to rental_log

-- Table for logging the updates on rental_log
CREATE TABLE rental_log_history (
    history_id SERIAL PRIMARY KEY,
    log_id INT,
    old_data JSONB,
    new_data JSONB,
    action VARCHAR(10),
    action_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Function for Trigger
CREATE OR REPLACE FUNCTION log_rental_log_changes()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'UPDATE' THEN
        INSERT INTO rental_log_history (log_id, old_data, new_data, action)
        VALUES (
            OLD.log_id,
            jsonb_build_object(
                'rental_time', OLD.rental_time,
                'customer_id', OLD.customer_id,
                'film_id', OLD.film_id,
                'amount', OLD.amount
            ),
            jsonb_build_object(
                'rental_time', NEW.rental_time,
                'customer_id', NEW.customer_id,
                'film_id', NEW.film_id,
                'amount', NEW.amount
            ),
            'UPDATE'
        );
    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO rental_log_history (log_id, old_data, action)
        VALUES (OLD.log_id, jsonb_build_object(
            'rental_time', OLD.rental_time,
            'customer_id', OLD.customer_id,
            'film_id', OLD.film_id,
            'amount', OLD.amount
        ), 'DELETE');
    ELSIF TG_OP = 'INSERT' THEN
        INSERT INTO rental_log_history (log_id, new_data, action)
        VALUES (NEW.log_id, jsonb_build_object(
            'rental_time', NEW.rental_time,
            'customer_id', NEW.customer_id,
            'film_id', NEW.film_id,
            'amount', NEW.amount
        ), 'INSERT');
    END IF;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

-- Create the trigger
CREATE TRIGGER rental_log_changes_trigger
AFTER INSERT OR UPDATE OR DELETE ON rental_log
FOR EACH ROW
EXECUTE FUNCTION log_rental_log_changes();


UPDATE rental_log
SET amount = 5.99
WHERE customer_id = 2;

INSERT INTO rental_log (log_id, rental_time, customer_id, film_id, amount)
VALUES
(2, '2025-05-02 12:30:00', 2, 102, 4.99);

-- Check on Secondary:
SELECT * FROM rental_log;
SELECT * FROM rental_log_history;
-- Pretty-Print Using SQL Query
SELECT 
    history_id,
    log_id,
    jsonb_pretty(old_data) AS old_data,
    jsonb_pretty(new_data) AS new_data,
    action,
    action_time
FROM rental_log_history;


--  history_id | log_id |                    old_data                     |                    new_data                     | action |        action_time
-- ------------+--------+-------------------------------------------------+-------------------------------------------------+--------+----------------------------
--           1 |      1 | {                                              +| {                                              +| UPDATE | 2025-05-14 16:00:51.222716
--             |        |     "amount": 4.99,                            +|     "amount": 5.99,                            +|        |
--             |        |     "film_id": 100,                            +|     "film_id": 100,                            +|        |
--             |        |     "customer_id": 1,                          +|     "customer_id": 1,                          +|        |
--             |        |     "rental_time": "2025-05-14T15:58:15.761865"+|     "rental_time": "2025-05-14T15:58:15.761865"+|        |
--             |        | }                                               | }                                               |        |
--           2 |      2 |                                                 | {                                              +| INSERT | 2025-05-14 16:02:25.546193
--             |        |                                                 |     "amount": 4.99,                            +|        |
--             |        |                                                 |     "film_id": 102,                            +|        |
--             |        |                                                 |     "customer_id": 2,                          +|        |
--             |        |                                                 |     "rental_time": "2025-05-02T12:30:00"       +|        |
--             |        |                                                 | }                                               |        |
-- (2 rows)

