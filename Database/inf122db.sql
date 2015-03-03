DROP SCHEMA `INF122DB` ;
CREATE SCHEMA `INF122DB` ;
USE `INF122DB`;

create table USERS(
userID integer NOT NULL AUTO_INCREMENT,
email varchar(100) UNIQUE, 
loginID varchar(100) NOT NULL UNIQUE,
pw varchar(100) ,
wins int DEFAULT 0,
primary key (userID)
);
