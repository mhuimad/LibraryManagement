CREATE TABLE [dbo].[Books]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Title] NVARCHAR(200) NOT NULL,
    [Author] NVARCHAR(200) NOT NULL,
    [TotalCopies] INT NOT NULL,
    [AvailableCopies] INT NOT NULL
);
