create database TaxOfficeActivity
go

use TaxOfficeActivity
go

CREATE TABLE [Activities] (
  [activityId] int PRIMARY KEY IDENTITY(1, 1),
  [activityName] nvarchar(150),
  [tax] decimal
)
GO

CREATE TABLE [Entrepreneurs] (
  [entrepreneurId] int PRIMARY KEY IDENTITY(1, 1),
  [taxOfficeId] int,
  [fullName] nvarchar(150)
)
GO

CREATE TABLE [TaxOffices] (
  [taxOfficeId] int PRIMARY KEY IDENTITY(1, 1),
  [districtId] int,
  [taxOfficeName] nvarchar(150)
)
GO

CREATE TABLE [Districts] (
  [districtId] int PRIMARY KEY IDENTITY(1, 1),
  [districtName] nvarchar(150)
)
GO

CREATE TABLE [ActivitiesAccounting] (
  [activityAccountingId] int PRIMARY KEY IDENTITY(1, 1),
  [entrepreneurId] int,
  [activityId] int,
  [year] int,
  [quarter] int,
  [income] decimal,
  [isTaxPaid] bit
)
GO

ALTER TABLE [Entrepreneurs] ADD FOREIGN KEY ([taxOfficeId]) REFERENCES [TaxOffices] ([taxOfficeId]) on delete cascade on update cascade
GO

ALTER TABLE [TaxOffices] ADD FOREIGN KEY ([districtId]) REFERENCES [Districts] ([districtId]) on delete cascade on update cascade
GO

ALTER TABLE [ActivitiesAccounting] ADD FOREIGN KEY ([entrepreneurId]) REFERENCES [Entrepreneurs] ([entrepreneurId]) on delete cascade on update cascade
GO

ALTER TABLE [ActivitiesAccounting] ADD FOREIGN KEY ([activityId]) REFERENCES [Activities] ([activityId]) on delete cascade on update cascade
GO