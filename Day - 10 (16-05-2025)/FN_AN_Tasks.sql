-- You are tasked with building a PostgreSQL-backed database for an EdTech company that manages online training and certification programs for individuals across various technologies.
-- The goal is to:
-- Design a normalized schema
-- Support querying of training data
-- Ensure secure access
-- Maintain data integrity and control over transactional updates
-- Database planning (Nomalized till 3NF)
-- A student can enroll in multiple courses
-- Each course is led by one trainer
-- Students can receive a certificate after passing
-- Each certificate has a unique serial number
-- Trainers may teach multiple courses
-- Tables to Design (Normalized to 3NF):

CourseFormatMaster
FormatId, FormatName — ('Live', 'Recorded', 'Hybrid')

CourseStatusMaster
CourseStatusId, StatusName — ('Active', 'Upcoming')

TechnologyMaster
TechnologyId, TechnologyName - ('Development', 'Testing', 'Architecture', .....)

EnrollmentStatusMaster
EnrollmentStatusId, StatusName - ('enrolled', 'completed')

PaymentMethodMaster
PaymentMethodId, MethodName - ('CreditCard', 'UPI', 'NetBanking', 'Wallet', 'Cash')

Student
StudentId, Name, Email, PhoneNumber

Trainer
TrainerId, Name, Email, PhoneNumber

Course
CourseId, Title, Description, TechnologyId, FormatId, TrainerId, StatusId, StartDate, EndDate, CourseFee

Enrollment
EnrollmentId, StudentId, CourseId, EnrolledDate, EnrollmentStatusId

Certificate
CertificateId, SerialNumber, EnrollmentId, IssueDate

Payment
PaymentId, EnrollmentId, PaymentDate, Amount, PaymentMethodId


----------------------------------------------------------------------------------------------------------------
-- Phase 2: DDL & DML
-- * Create all tables with appropriate constraints (PK, FK, UNIQUE, NOT NULL)
-- * Insert sample data using `INSERT` statements
-- * Create indexes on `student_id`, `email`, and `course_id`

-- * Create all tables with appropriate constraints (PK, FK, UNIQUE, NOT NULL)
CREATE TABLE students (
    student_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(20)
);


CREATE TABLE courses (
    course_id SERIAL PRIMARY KEY,
    course_name VARCHAR(150) NOT NULL,
    category VARCHAR(100),
    duration_days INT
);


CREATE TABLE trainers (
    trainer_id SERIAL PRIMARY KEY,
    trainer_name VARCHAR(100) NOT NULL,
    expertise VARCHAR(150)
);


CREATE TABLE enrollments (
    enrollment_id SERIAL PRIMARY KEY,
    student_id INT NOT NULL,
    course_id INT NOT NULL,
    enroll_date DATE DEFAULT CURRENT_DATE,
    FOREIGN KEY (student_id) REFERENCES students(student_id) ON DELETE CASCADE,
    FOREIGN KEY (course_id) REFERENCES courses(course_id) ON DELETE CASCADE,
    UNIQUE (student_id, course_id)
);


CREATE TABLE certificates (
    certificate_id SERIAL PRIMARY KEY,
    enrollment_id INT UNIQUE NOT NULL,
    issue_date DATE,
    serial_no UUID DEFAULT gen_random_uuid(),
    FOREIGN KEY (enrollment_id) REFERENCES enrollments(enrollment_id) ON DELETE CASCADE
);


CREATE TABLE course_trainers (
    course_id INT NOT NULL,
    trainer_id INT NOT NULL,
    PRIMARY KEY (course_id, trainer_id),
    FOREIGN KEY (course_id) REFERENCES courses(course_id) ON DELETE CASCADE,
    FOREIGN KEY (trainer_id) REFERENCES trainers(trainer_id) ON DELETE CASCADE
);

-- * Insert sample data using `INSERT` statements
-- -- Students
-- INSERT INTO students (name, email, phone) VALUES
-- ('Vasu', 'vasu@email.com', '9999999999'),
-- ('Sharmi', 'sharmi@email.com', '8888888888'),
-- ('Srijaa', 'srijaa@email.com', '7888888888');

-- -- Continuing from ID 4 after Vasu, Sharmi, Srijaa
-- INSERT INTO students (name, email, phone) VALUES
-- ('Steve Rogers', 'cap.america@avengers.com', '9111111111'),    -- ID 4
-- ('Nami', 'nami@onepiece.com', '9222222222'),                   -- ID 5
-- ('Tony Stark', 'ironman@avengers.com', '9333333333'),          -- ID 6
-- ('Roronoa Zoro', 'zoro@onepiece.com', '9444444444'),           -- ID 7
-- ('Wanda Maximoff', 'wanda@avengers.com', '9555555555'),        -- ID 8
-- ('Monkey D. Luffy', 'luffy@onepiece.com', '9666666666'),       -- ID 9
-- ('Natasha Romanoff', 'blackwidow@avengers.com', '9777777777'), -- ID 10
-- ('Sanji', 'sanji@onepiece.com', '9888888888'),                 -- ID 11
-- ('Bruce Banner', 'hulk@avengers.com', '9999999990'),           -- ID 12
-- ('Nico Robin', 'robin@onepiece.com', '9000000001');            -- ID 13

-- Courses
-- INSERT INTO courses (course_name, category, duration_days) VALUES
-- ('SQL Basics', 'Database', 15),
-- ('Java Fundamentals', 'Programming', 30);

-- INSERT INTO courses (course_name, category, duration_days) VALUES
-- ('Advanced Ethical Hacking', 'Cybersecurity', 45), -- course_id = 3
-- ('DevOps Essentials', 'Cloud/DevOps', 40),         -- 4
-- ('Kali Linux Deep Dive', 'Cybersecurity', 30),     -- 5
-- ('React Fullstack Development', 'Web Development', 60), -- 6
-- ('Agile Methodologies', 'Project Management', 25); -- 7


-- Trainers
-- INSERT INTO trainers (trainer_name, expertise) VALUES
-- ('Tony Stark', 'SQL, PostgreSQL'),
-- ('Heisenberg', 'Java, Spring Boot');

-- INSERT INTO trainers (trainer_name, expertise) VALUES
-- ('Bruce Wayne', 'Cybersecurity, Python'),
-- ('Ada Lovelace', 'Algorithms, Machine Learning'),
-- ('Victor Stone', 'Cloud Architecture, Kubernetes'),
-- ('Kara Danvers', 'Data Engineering, Big Data'),
-- ('Miles Morales', 'Full Stack Development, JavaScript');


-- Enrollments
-- INSERT INTO enrollments (student_id, course_id) VALUES
-- (1, 1),
-- (2, 2);

-- -- Enroll ID continues from previous (3+)
-- INSERT INTO enrollments (student_id, course_id) VALUES
-- (4, 3),  -- Steve Rogers → Advanced Hacking
-- (5, 4),  -- Nami → DevOps
-- (6, 5),  -- Tony Stark → Kali Linux
-- (7, 6),  -- Zoro → React Fullstack
-- (8, 7),  -- Wanda → Agile
-- (9, 3),  -- Luffy → Adv Hacking
-- (10, 4), -- Natasha → DevOps
-- (11, 5), -- Sanji → Kali Linux
-- (12, 6), -- Bruce Banner → React
-- (13, 7); -- Robin → Agile


-- -- Certificates
-- INSERT INTO certificates (enrollment_id, issue_date) VALUES
-- (1, '2024-12-01'),
-- (2, '2024-12-05');

-- INSERT INTO certificates (enrollment_id, issue_date) VALUES
-- (3, '2025-01-10'),  -- Steve Rogers
-- (5, '2025-01-14'),  -- Tony Stark
-- (6, '2025-01-16'),  -- Wanda
-- (8, '2025-01-20');  -- Luffy
-- -- Natasha, Sanji, Bruce, Robin didn’t get certificates yet


-- -- Course-Trainers
-- INSERT INTO course_trainers (course_id, trainer_id) VALUES
-- (1, 1),
-- (2, 2);
-- INSERT INTO course_trainers (course_id, trainer_id) VALUES
-- (3, 4), -- Adv Hacking ← Luffy
-- (4, 5), -- DevOps ← Wanda
-- (5, 4), -- Kali Linux ← Luffy
-- (6, 6), -- React ← Zoro
-- (7, 3), -- Agile ← Steve Rogers
-- (7, 7); -- Agile ← Robin (co-trainer)


SELECT * FROM Students;
SELECT * FROM courses;
SELECT * FROM trainers;
SELECT * FROM course_trainers;
SELECT * FROM enrollments;
SELECT * FROM certificates;


-- Create indexes on `student_id`, `email`, and `course_id`
CREATE INDEX idx_students_student_id ON students(student_id);
CREATE INDEX idx_students_email ON students(email);
CREATE INDEX idx_courses_course_id ON courses(course_id);


----------------------------------------------------------------------------------------------------------------
-- Phase 3: SQL Joins Practice
-- Write queries to:
-- 1. List students and the courses they enrolled in
-- 2. Show students who received certificates with trainer names
-- 3. Count number of students per course

-- 1. List students and the courses they enrolled in
SELECT 
    s.name AS student_name,
    c.course_name
FROM enrollments e
JOIN students s ON e.student_id = s.student_id
JOIN courses c ON e.course_id = c.course_id;

-- 2. Show students who received certificates with trainer names
SELECT 
    s.name AS student_name,
    c.course_name,
    t.trainer_name,
    cert.issue_date,
    cert.serial_no
FROM certificates cert
JOIN enrollments e ON cert.enrollment_id = e.enrollment_id
JOIN students s ON e.student_id = s.student_id
JOIN courses c ON e.course_id = c.course_id
JOIN course_trainers ct ON c.course_id = ct.course_id
JOIN trainers t ON ct.trainer_id = t.trainer_id;
	
-- 3. Count number of students per course
SELECT
	c.course_name,
	COUNT(s.student_id) AS No_of_students
FROM students s
JOIN enrollments e ON e.student_id = s.student_id
JOIN courses c ON e.course_id = c.course_id
GROUP BY c.course_id;


----------------------------------------------------------------------------------------------------------------
-- Phase 4: Functions & Stored Procedures
-- Function:
-- Create `get_certified_students(course_id INT)`
-- → Returns a list of students who completed the given course and received certificates.

CREATE OR REPLACE FUNCTION get_certified_students(p_course_id INT)
RETURNS TABLE(student_id INT, student_name VARCHAR, email VARCHAR, issue_date DATE) AS
$$
BEGIN
    RETURN QUERY
    SELECT 
        s.student_id,
        s.name,
        s.email,
        cert.issue_date
    FROM certificates cert
    JOIN enrollments e ON cert.enrollment_id = e.enrollment_id
    JOIN students s ON e.student_id = s.student_id
    WHERE e.course_id = p_course_id;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM get_certified_students(2);


-- Stored Procedure:
-- Create `sp_enroll_student(p_student_id, p_course_id)`
-- → Inserts into `enrollments` and conditionally adds a certificate if completed (simulate with status flag).

CREATE OR REPLACE PROCEDURE sp_enroll_student(p_student_id INT, p_course_id INT, p_completed BOOLEAN)
LANGUAGE plpgsql
AS $$
DECLARE
	v_enrollment_id INT;
BEGIN
	INSERT INTO enrollments(student_id, course_id)
	VALUES(p_student_id, p_course_id)
	RETURNING enrollment_id INTO v_enrollment_id;

	IF p_completed THEN
        INSERT INTO certificates (enrollment_id, issue_date)
        VALUES (v_enrollment_id, CURRENT_DATE);
    END IF;

	RAISE NOTICE 'Student % enrolled in course %, Completed? %', p_student_id, p_course_id, p_completed;
END;
$$;

CALL sp_enroll_student(13, 1, 't');

SELECT * FROM enrollments ORDER BY enrollment_id DESC;
SELECT * FROM certificates ORDER BY certificate_id DESC;


----------------------------------------------------------------------------------------------------------------
-- Phase 5: Cursor
-- Use a cursor to:
-- * Loop through all students in a course
-- * Print name and email of those who do not yet have certificates

CREATE OR REPLACE PROCEDURE show_students_without_certificates(p_course_id INT)
LANGUAGE plpgsql
AS $$
DECLARE
    rec RECORD;
    cur_students CURSOR FOR
        SELECT 
            s.student_id, 
            s.name, 
            s.email, 
            e.enrollment_id
        FROM students s
        JOIN enrollments e ON s.student_id = e.student_id
        WHERE e.course_id = p_course_id;
BEGIN
    OPEN cur_students;
    
    LOOP
        FETCH cur_students INTO rec;
        EXIT WHEN NOT FOUND;
        
        -- Check if certificate exists
        IF NOT EXISTS (SELECT 1 FROM certificates c WHERE c.enrollment_id = rec.enrollment_id) THEN
            RAISE NOTICE 'Student: Name = %, Email = %', rec.name, rec.email;
        END IF;
    END LOOP;

    CLOSE cur_students;
END;
$$;

CALL show_students_without_certificates(3);


----------------------------------------------------------------------------------------------------------------
-- Phase 6: Security & Roles
-- 1. Create a `readonly_user` role:
--    * Can run `SELECT` on `students`, `courses`, and `certificates`
--    * Cannot `INSERT`, `UPDATE`, or `DELETE`
-- 2. Create a `data_entry_user` role:
--    * Can `INSERT` into `students`, `enrollments`
--    * Cannot modify certificates directly

CREATE ROLE readonly_user NOINHERIT LOGIN PASSWORD 'readonly';

CREATE ROLE dataentry_user NOINHERIT LOGIN PASSWORD 'dataentry';

GRANT USAGE ON SCHEMA public TO readonly_user;

GRANT USAGE ON SCHEMA public TO dataentry_user;

GRANT SELECT ON students, courses, certificates TO readonly_user;

GRANT INSERT ON students, enrollments TO dataentry_user;

REVOKE ALL ON certificates FROM dataentry_user;

GRANT USAGE, SELECT ON SEQUENCE students_student_id_seq TO dataentry_user;
GRANT USAGE, SELECT ON SEQUENCE enrollments_enrollment_id_seq TO dataentry_user;


----------------------------------------------------------------------------------------------------------------
-- Phase 7: Transactions & Atomicity
-- Write a transaction block that:
-- * Enrolls a student
-- * Issues a certificate
-- * Fails if certificate generation fails (rollback)

CREATE OR REPLACE PROCEDURE sp_enroll_and_certify(
    p_student_id INT,
    p_course_id INT
)
LANGUAGE plpgsql
AS $$
DECLARE
	v_enrollment_id INT;
BEGIN
	BEGIN
		INSERT INTO enrollments (student_id, course_id, enroll_date)
		VALUES (p_student_id, p_course_id, CURRENT_DATE)
		RETURNING enrollment_id INTO v_enrollment_id;
		
		INSERT INTO certificates (enrollment_id, issue_date)
		VALUES (v_enrollment_id, CURRENT_DATE);
		
		EXCEPTION
		    WHEN OTHERS THEN
		        RAISE NOTICE 'Error: %, rolling back.', SQLERRM;
	END;
END;
$$;

CALL sp_enroll_and_certify(10, 2);
SELECT * FROM certificates;

