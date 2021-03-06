--CREATE DATABASE [$DATABASE];

USE [$DATABASE];

CREATE TABLE product
  (
     id   INT NOT NULL auto_increment PRIMARY KEY,
     name VARCHAR(255) NULL
  );

CREATE TABLE logs
  (
     id              INT NOT NULL auto_increment PRIMARY KEY,
     message         VARCHAR(3000) NULL,
     messagetemplate VARCHAR(3000) NULL,
     level           VARCHAR(128) NULL,
     timestamp       DATETIME NOT NULL,
     exception       VARCHAR(3000) NULL,
     properties      VARCHAR(3000) NULL,
     loglevel        VARCHAR(3000) NULL
  ); 