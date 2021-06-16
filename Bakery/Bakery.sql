create database Bakery
go

use Bakery

CREATE TABLE [Products] (
  [productId] int PRIMARY KEY IDENTITY(1, 1),
  [productName] nvarchar(100)
)
GO

CREATE TABLE [Raws] (
  [rawId] int PRIMARY KEY IDENTITY(1, 1),
  [rawName] nvarchar(100)
)
GO

CREATE TABLE [Materials] (
  [materialId] int PRIMARY KEY IDENTITY(1, 1),
  [materialName] nvarchar(100)
)
GO

CREATE TABLE [Norms] (
  [normId] int PRIMARY KEY IDENTITY(1, 1),
  [rawId] int,
  [productId] int,
  [quantity] float
)
GO

CREATE TABLE [ProductsMaterials] (
  [productMaterial] int PRIMARY KEY IDENTITY(1, 1),
  [materialId] int,
  [productId] int,
  [quantity] float
)
GO

CREATE TABLE [ProductsPlans] (
  [productPlan] int PRIMARY KEY IDENTITY(1, 1),
  [dayPlanId] int,
  [productId] int,
  [count] int
)
GO

CREATE TABLE [DayPlans] (
  [dayPlanId] int PRIMARY KEY IDENTITY(1, 1),
  [date] date
)
GO

CREATE TABLE [DayProductions] (
  [dayProductionId] int PRIMARY KEY IDENTITY(1, 1),
  [date] date
)
GO

CREATE TABLE [ProductsProductions] (
  [productProductionId] int PRIMARY KEY IDENTITY(1, 1),
  [dayProductionId] int,
  [productId] int,
  [count] int
)
GO

ALTER TABLE [Norms] ADD FOREIGN KEY ([rowId]) REFERENCES [Raws] ([rawId]) on update cascade on delete cascade
GO

ALTER TABLE [Norms] ADD FOREIGN KEY ([productId]) REFERENCES [Products] ([productId]) on update cascade on delete cascade
GO

ALTER TABLE [ProductsMaterials] ADD FOREIGN KEY ([materialId]) REFERENCES [Materials] ([materialId]) on update cascade on delete cascade
GO

ALTER TABLE [ProductsMaterials] ADD FOREIGN KEY ([productId]) REFERENCES [Products] ([productId]) on update cascade on delete cascade
GO

ALTER TABLE [ProductsPlans] ADD FOREIGN KEY ([dayPlanId]) REFERENCES [DayPlans] ([dayPlanId]) on update cascade on delete cascade
GO

ALTER TABLE [ProductsPlans] ADD FOREIGN KEY ([productId]) REFERENCES [Products] ([productId]) on update cascade on delete cascade
GO

ALTER TABLE [ProductsProductions] ADD FOREIGN KEY ([dayProductionId]) REFERENCES [DayProductions] ([dayProductionId]) on update cascade on delete cascade
GO

ALTER TABLE [ProductsProductions] ADD FOREIGN KEY ([productId]) REFERENCES [Products] ([productId]) on update cascade on delete cascade
GO
