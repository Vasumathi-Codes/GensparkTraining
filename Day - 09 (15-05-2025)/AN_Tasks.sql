-- 1. Create a stored procedure to encrypt a given text
-- Task: 
-- Write a stored procedure sp_encrypt_text that takes a plain text input (e.g., email or mobile number) and returns an encrypted version using PostgreSQL's pgcrypto extension.
-- Use pgp_sym_encrypt(text, key) from pgcrypto.

CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE OR REPLACE PROCEDURE sp_encrypt_text(
    IN p_text TEXT,
    IN p_key TEXT,
    OUT p_encrypted BYTEA
)
LANGUAGE plpgsql
AS $$
BEGIN
    p_encrypted := pgp_sym_encrypt(p_text, p_key);
END;
$$;

DO $$
DECLARE
    encrypted_email BYTEA;
BEGIN
    CALL sp_encrypt_text('vasu@gmail.com', 'Kaizoku', encrypted_email);
    RAISE NOTICE 'Encrypted Email: %', encrypted_email;
END;
$$;


-- 2. Create a stored procedure to compare two encrypted texts
-- Task:
-- Write a procedure sp_compare_encrypted that takes two encrypted values and checks if they decrypt to the same plain text.

CREATE OR REPLACE PROCEDURE sp_compare_encrypted(
    IN p_encrypted1 BYTEA,
    IN p_encrypted2 BYTEA,
    IN p_key TEXT,
    OUT p_is_equal BOOLEAN
)
LANGUAGE plpgsql
AS $$
DECLARE
    decrypted1 TEXT;
    decrypted2 TEXT;
BEGIN
    decrypted1 := pgp_sym_decrypt(p_encrypted1, p_key);
    decrypted2 := pgp_sym_decrypt(p_encrypted2, p_key);

    p_is_equal := (decrypted1 = decrypted2);
END;
$$;


DO $$
DECLARE
    encrypted1 BYTEA;
    encrypted2 BYTEA;
    are_equal BOOLEAN;
BEGIN
    -- Encrypt first text
    CALL sp_encrypt_text('vasu@gmail.com', 'Kaizoku', encrypted1);
    
    -- Encrypt second text
    CALL sp_encrypt_text('vasu@gmail.com', 'Kaizoku', encrypted2);
    
    -- To Compare the two encrypted values
    CALL sp_compare_encrypted(encrypted1, encrypted2, 'Kaizoku', are_equal);
    
    IF are_equal THEN
        RAISE NOTICE 'Texts are EQUAL after decryption.';
    ELSE
        RAISE NOTICE 'Texts are NOT equal after decryption.';
    END IF;
END;
$$;


-- 3. Create a stored procedure to partially mask a given text
-- Task: 
-- Write a procedure sp_mask_text that:
-- Shows only the first 2 and last 2 characters of the input string
-- Masks the rest with *
-- E.g., input: 'john.doe@example.com' → output: 'jo***************om'

CREATE OR REPLACE PROCEDURE sp_mask_text(
    IN p_text TEXT,
    OUT p_masked TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
    text_len INT;
    mask_len INT;
BEGIN
    text_len := length(p_text);
    
    IF text_len <= 4 THEN
        p_masked := p_text;
    ELSE
        mask_len := text_len - 4; 
        p_masked := 
            substring(p_text from 1 for 2) ||
            repeat('*', mask_len) ||          
            substring(p_text from text_len - 1 for 2);
    END IF;
END;
$$;

DO $$
DECLARE
    masked_text TEXT;
BEGIN
    CALL sp_mask_text('vasu@gmail.com', masked_text);
    RAISE NOTICE 'Masked: %', masked_text; 
    
    CALL sp_mask_text('vasu', masked_text);
    RAISE NOTICE 'Masked=: %', masked_text;
END;
$$;


-- 4. Create a procedure to insert into customer with encrypted email and masked name
-- Task:
-- Call sp_encrypt_text for email
-- Call sp_mask_text for first_name
-- Insert masked and encrypted values into the customer table
-- Use any valid address_id and store_id to satisfy FK constraints.

CREATE OR REPLACE PROCEDURE sp_insert_customer(
    IN p_first_name TEXT,
	IN p_last_name TEXT,
    IN p_email TEXT,
    IN p_key TEXT,           
    IN p_address_id INT,
    IN p_store_id INT,
	IN p_active INT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_encrypted_email BYTEA;
    v_encrypted_email_hex TEXT;
    v_masked_first_name TEXT;
BEGIN
    -- Encrypt email to BYTEA
    CALL sp_encrypt_text(p_email, p_key, v_encrypted_email);
    
    -- Convert BYTEA encrypted email to HEX string
    v_encrypted_email_hex := encode(v_encrypted_email, 'hex');
    
    -- Mask first name
    CALL sp_mask_text(p_first_name, v_masked_first_name);

    -- Insert masked first name and HEX encrypted email into customer table
    INSERT INTO customer(first_name, last_name, email, address_id, store_id, active)
    VALUES (v_masked_first_name, p_last_name, v_encrypted_email_hex, p_address_id, p_store_id, p_active);
END;
$$;

-- The email column was originally set to VARCHAR(50), but since encrypted emails are much longer, I changed it to VARCHAR(500) to make sure they fit
ALTER TABLE customer ALTER COLUMN email TYPE varchar(500);

CALL sp_insert_customer(
    'Sanji',
	'Vinsmoke',
    'sanji@gmail.com',
    'Kaizoku',     -- encryption key
	1,
    1,            
    1              
);

SELECT * FROM CUSTOMER ORDER BY customer_id desc;


-- 5. Create a procedure to fetch and display masked first_name and decrypted email for all customers
-- Task:
-- Write sp_read_customer_masked() that:
-- Loops through all rows
-- Decrypts email
-- Displays customer_id, masked first name, and decrypted email

CREATE OR REPLACE PROCEDURE sp_read_customer_masked(
    IN p_key TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
    rec RECORD;
    v_email_decrypted TEXT;
    email_decrypt_cur CURSOR FOR 
        SELECT customer_id, first_name, email 
        FROM customer ORDER BY customer_id desc;
BEGIN
    OPEN email_decrypt_cur;

    LOOP
        FETCH email_decrypt_cur INTO rec;
        EXIT WHEN NOT FOUND;

        BEGIN
            v_email_decrypted := pgp_sym_decrypt(decode(rec.email, 'hex'), p_key);
        EXCEPTION
            WHEN OTHERS THEN
                v_email_decrypted := '[decryption failed]';
        END;

        RAISE NOTICE 'ID: %, Name: %, Email: %', rec.customer_id, rec.first_name, v_email_decrypted;
    END LOOP;

    CLOSE email_decrypt_cur;
END;
$$;

CALL sp_read_customer_masked('Kaizoku');
