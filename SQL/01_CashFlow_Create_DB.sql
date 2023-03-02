USE [master]

IF db_id('CashFlow') IS NULL
  CREATE DATABASE [CashFlow]
GO

USE [CashFlow]
GO

DROP TABLE IF EXISTS [User];
DROP TABLE IF EXISTS [Transaction];
DROP TABLE IF EXISTS [Category];
DROP TABLE IF EXISTS [CategoryTransaction];
GO

CREATE TABLE [UserProfile] (
  [Id] int PRIMARY KEY IDENTITY,
  [FirebaseUserId] nvarchar(255) NOT NULL,
  [Username] nvarchar(255) NOT NULL,
  [FirstName] nvarchar(255) NOT NULL,
  [LastName] nvarchar(255) NOT NULL,
  [Email] nvarchar(255) NOT NULL,
  [CreateDate] datetime NOT NULL
)
GO


CREATE TABLE [Transaction] (
  [Id] int PRIMARY KEY IDENTITY,
  [Ammount] int NOT NULL,
  [Note] nvarchar(255) NOT NULL,
  [Date] datetime NOT NULL,
  [UserProfileId] int NOT NULL
)
GO

CREATE TABLE [TransactionCategory] (
  [Id] int PRIMARY KEY IDENTITY,
  [TransactionId] int NOT NULL,
  [CateagoryId] int NOT NULL
)
GO

CREATE TABLE [Category] (
  [Id] int PRIMARY KEY IDENTITY,
  [Name] nvarchar(255) NOT NULL
  [Type] nvarchar(255) NOT NULL
)
GO

ALTER TABLE [Transaction] ADD FOREIGN KEY ([UserProfileId]) REFERENCES [UserProfile] ([Id])
GO

ALTER TABLE [TransactionCategory] ADD FOREIGN KEY ([TransactionId]) REFERENCES [Transaction] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [TransactionCategory] ADD FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id])
GO
