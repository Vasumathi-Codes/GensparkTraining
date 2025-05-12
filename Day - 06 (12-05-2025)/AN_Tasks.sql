-- 1️⃣ Question:
-- In a transaction, if I perform multiple updates and an error happens in the third statement, but I have not used SAVEPOINT, what will happen if I issue a ROLLBACK?
-- Will my first two updates persist?
No, the first two updates will not persist. The entire transaction will be rolled back.

BEGIN;
UPDATE accounts SET balance = balance - 100 WHERE id = 1; -- Success
UPDATE accounts SET balance = balance + 100 WHERE id = 2; -- Success
UPDATE non_existing_table SET x = 1; -- Error here
ROLLBACK;  -- All the above changes are undone


-- 2️⃣ Question:
-- Suppose Transaction A updates Alice’s balance but does not commit. Can Transaction B read the new balance if the isolation level is set to READ COMMITTED?
No, with the READ COMMITTED isolation level, Transaction B will only be able to see data that has been committed. Since Transaction A has not committed its changes, Transaction B will read the old value of Alice's balance.

-- Transaction A
BEGIN;
UPDATE accounts SET balance = 900 WHERE name = 'Alice';

-- Transaction B
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
SELECT balance FROM accounts WHERE name = 'Alice'; -- Sees old value


-- 3️⃣ Question:
-- What will happen if two concurrent transactions both execute:
-- UPDATE tbl_bank_accounts SET balance = balance - 100 WHERE account_name = 'Alice';
-- at the same time? Will one overwrite the other?
No, If two transactions try to update the same row at the same time, one will wait until the other finishes. This prevents them from changing the row at the same time and avoids overwriting each others changes.

-- Transaction A
BEGIN;
UPDATE accounts SET balance = balance - 100 WHERE id = 1;

-- Transaction B (runs at same time)
BEGIN;
UPDATE accounts SET balance = balance - 100 WHERE id = 1; -- Will wait


-- 4️⃣ Question:
-- If I issue ROLLBACK TO SAVEPOINT after_alice;, will it only undo changes made after the savepoint or everything?
It will undo only the changes made after the savepoint 'after_alice'.

BEGIN;
UPDATE accounts SET balance = balance - 100 WHERE name = 'Alice';
SAVEPOINT after_alice;
UPDATE accounts SET balance = balance + 100 WHERE name = 'Bob';
ROLLBACK TO SAVEPOINT after_alice; -- Undo Bob's update only
COMMIT;


-- 5️⃣ Question:
-- Which isolation level in PostgreSQL prevents phantom reads?
SERIALIZABLE isolation level.


-- 6️⃣ Question:
-- Can Postgres perform a dirty read (reading uncommitted data from another transaction)?
No. PostgreSQL does not allow dirty reads under any isolation level.


-- 7️⃣ Question:
-- If autocommit is ON (default in Postgres), and I execute an UPDATE, is it safe to assume the change is immediately committed?
Yes, the change is committed immediately after execution.


-- 8️⃣ Question:
-- If I do this:

-- BEGIN;
-- UPDATE accounts SET balance = balance - 500 WHERE id = 1;
-- -- (No COMMIT yet)
-- And from another session, I run:

-- SELECT balance FROM accounts WHERE id = 1;
-- Will the second session see the deducted balance?
No. Until the first session commits, the second session sees the old data.

-- Session 1
BEGIN;
UPDATE accounts SET balance = balance - 500 WHERE id = 1;

-- Session 2
SELECT balance FROM accounts WHERE id = 1; -- Still shows old balance
