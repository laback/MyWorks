create database TransportUsing
go

use TransportUsing

Create table UserTypes(
	typeId int Identity(1,1) primary key, 
	typeName varchar(30))
go

create table Users( 
  [userId] int PRIMARY KEY identity(1,1),
  [username] varchar(50) UNIQUE,
  [password] varchar(50),
  [FIO] varchar(100),
  [typeId] int references UserTypes(typeId) on update cascade on delete cascade)
go

CREATE TABLE [Marks] (
  [markId] int PRIMARY KEY identity(1,1),
  [markName] varchar(20)
)

GO
CREATE TABLE [TransportTypes] (
  [transportTypeId] int PRIMARY KEY identity(1,1),
  [transportTypeName] varchar(30)
)

GO

CREATE TABLE [Transports] (
  [transportId] int PRIMARY KEY identity(1,1),
  [transportTypeId] int references [TransportTypes](transportTypeId) on update cascade on delete cascade,
  [markId] int references Marks(markId) on update cascade on delete cascade
)
GO

CREATE TABLE [Routes] (
  [routeId] int PRIMARY KEY identity(1,1),
  [userId] int references Users(userId) on update cascade on delete cascade,
  [distance] int,
  date datetime,
  [transportId] int references Transports(transportId) on update cascade on delete cascade,
  [passengers] int,
  [cargo] int
)