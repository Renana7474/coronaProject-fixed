create database coronaDB3;
create table dbo.Members(
id int identity(1, 1) primary key, 
first_name nvarchar(500),
last_name nvarchar(500),
id_card nvarchar(500) ,
city nvarchar(500),
street nvarchar(500),
number int,
date_of_birth datetime,
phone nvarchar(500),
mobile_phone nvarchar(500),
)
create table dbo.DiseasePeriod(
id int identity(1, 1) primary key,
member_id int FOREIGN KEY REFERENCES Members(id),
detected_date dateTime ,
recovery_date dateTime 
)
create  table dbo.Manafuctures(
id int identity(1, 1) primary key,
manafucture_name nvarchar(500),
)
create table dbo.VaccinationsGetting(
id int identity(1, 1) primary key,
member_id int FOREIGN KEY REFERENCES Members(id),
vaccin_date datetime ,
manafuct_code int FOREIGN KEY REFERENCES Manafuctures(id)
)

