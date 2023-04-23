IF SCHEMA_ID(N'Application') IS NULL
   EXEC(N'CREATE SCHEMA [Application];');
GO

DROP TABLE IF EXISTS Application.Messages;
DROP TABLE IF EXISTS Application.Channels;
DROP TABLE IF EXISTS Application.Memberships;
DROP TABLE IF EXISTS Application.Groups;
DROP TABLE IF EXISTS Application.Users;
DROP TABLE IF EXISTS Application.Roles;
DROP TABLE IF EXISTS Application.Organizations;
DROP FUNCTION IF EXISTS Application.CheckOrganizations
GO

CREATE OR ALTER FUNCTION Application.CheckOrganizations(@UserId INT, @GroupId INT)
	RETURNS INT
	AS
	BEGIN
		DECLARE @Result INT
		IF ((SELECT U.OrganizationId FROM Application.Users U WHERE U.UserId = @UserId) = 
			(SELECT G.OrganizationId FROM Application.Groups G WHERE G.GroupId = @GroupId))
			RETURN 1
		RETURN 0;
END;
GO

CREATE TABLE Application.Organizations
( 
    OrganizationId INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(64) NOT NULL,
    Active BIT NOT NULL,
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET())

    UNIQUE([Name])
)

CREATE TABLE Application.Roles
(
    [Name] NVARCHAR(32) NOT NULL PRIMARY KEY
)

CREATE TABLE Application.Users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
	OrganizationId INT NOT NULL FOREIGN KEY REFERENCES Application.Organizations(OrganizationId),
	RoleName NVARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Application.Roles([Name]),
    Username NVARCHAR(64) NOT NULL, 
    Email NVARCHAR(128) NOT NULL,
	[Password] NVARCHAR(128) NOT NULL,
	FirstName NVARCHAR(64) NOT NULL,
    LastName NVARCHAR(64) NOT NULL,
    Title NVARCHAR(64) NOT NULL,
    ProfilePhoto NVARCHAR(max) NOT NULL,
	Active BIT NOT NULL,
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET())

    UNIQUE(Username, Email)
)

 CREATE TABLE Application.Groups
(
    GroupId INT IDENTITY(1,1) PRIMARY KEY,
    OrganizationId INT NOT NULL FOREIGN KEY REFERENCES Application.Organizations(OrganizationId),
	[Name] NVARCHAR(64) NOT NULL,
    Active BIT NOT NULL,
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET())

	UNIQUE(Name, OrganizationId)
)

CREATE TABLE Application.Memberships
(
    MembershipId INT IDENTITY(1,1) PRIMARY KEY,
	OrganizationId INT NOT NULL FOREIGN KEY REFERENCES Application.Organizations(OrganizationId),
    GroupId INT NOT NULL FOREIGN KEY REFERENCES Application.Groups(GroupId),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Application.Users(UserId),

    UNIQUE(GroupId, UserId, OrganizationId),
	CONSTRAINT [user_must_not_be_in_group_twice] UNIQUE(GroupId, UserId),
	CONSTRAINT [user_and_group_must_have_same_organization] CHECK(Application.CheckOrganizations(UserId, GroupId) = 1)
)

CREATE TABLE Application.Channels
(
    ChannelId INT IDENTITY(1,1) PRIMARY KEY,
	GroupId INT FOREIGN KEY REFERENCES Application.Groups(GroupId),
    [Name] NVARCHAR(64) NOT NULL,
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),

    UNIQUE([Name], GroupId)
)

CREATE TABLE Application.Messages
(
    MsgId INT IDENTITY(1,1) PRIMARY KEY,
    SenderId INT NOT NULL FOREIGN KEY REFERENCES Application.Users(UserId),
	ChannelId INT NULL FOREIGN KEY REFERENCES Application.Channels(ChannelId),
	RecipientId INT NULL FOREIGN KEY REFERENCES Application.Users(UserId),
    [Message] NVARCHAR(512) NOT NULL,
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    
	CONSTRAINT [check_only_one_recipient] CHECK(
		Messages.RecipientId IS NULL AND Messages.ChannelId IS NOT NULL OR Messages.ChannelId IS NULL AND Messages.RecipientId IS NOT NULL)
)
GO
ALTER TABLE Application.Users
ALTER COLUMN ProfilePhoto NVARCHAR(max);

/* Grant Permissions */

/*
GRANT CONTROL ON SCHEMA :: Application TO hcossins;
GRANT CONTROL ON SCHEMA :: Application TO sbrittain;
GRANT CONTROL ON SCHEMA :: Application TO slhaynes4542;*/
