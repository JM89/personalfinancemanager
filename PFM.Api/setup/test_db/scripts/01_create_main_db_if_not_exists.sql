CREATE DATABASE IF NOT EXISTS PFM_TEST_DB;

USE PFM_TEST_DB;

CREATE TABLE IF NOT EXISTS Persons (
    PersonID int,
    LastName varchar(255),
    FirstName varchar(255),
    Address varchar(255),
    City varchar(255)
);