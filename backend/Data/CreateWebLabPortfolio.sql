IF DB_ID(N'WebLabPortfolio') IS NULL
BEGIN
    CREATE DATABASE WebLabPortfolio;
END
GO

USE WebLabPortfolio;
GO

IF OBJECT_ID(N'dbo.Submissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Submissions
    (
        Id uniqueidentifier NOT NULL CONSTRAINT PK_Submissions PRIMARY KEY,
        Type nvarchar(450) NOT NULL,
        Name nvarchar(max) NOT NULL,
        Email nvarchar(450) NOT NULL,
        Message nvarchar(max) NOT NULL,
        StudyProgram nvarchar(max) NULL,
        Interests nvarchar(max) NULL,
        SubmittedAt datetimeoffset NOT NULL
    );

    CREATE INDEX IX_Submissions_Type ON dbo.Submissions(Type);
    CREATE INDEX IX_Submissions_Email ON dbo.Submissions(Email);
END
GO
