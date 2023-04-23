use cis560_SP23_T1; 


IF SCHEMA_ID(N'Application') IS NULL
   EXEC(N'CREATE SCHEMA [Application];');
GO


CREATE TABLE Application.Organizations
( 
    OrganizationId INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(32) NOT NULL,
    Active BIT NOT NULL,
     CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET())

    UNIQUE([Name])
    
)
GO

CREATE TABLE Application.Roles
(
    [Name] NVARCHAR(32) NOT NULL PRIMARY KEY
)
GO

CREATE TABLE Application.Users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(32) NOT NULL, 
    Email NVARCHAR(128) NOT NULL,
    Password NVARCHAR(128) NOT NULL, 
    OrganizationId INT NOT NULL FOREIGN KEY 
    REFERENCES Application.Organizations(OrganizationId),  
    RoleName NVARCHAR(32) NOT NULL FOREIGN KEY
    REFERENCES Application.Roles([Name]),
    FirstName NVARCHAR(32) NOT NULL,
    LastName NVARCHAR(32) NOT NULL, 
    Title NVARCHAR(32) NOT NULL,
    Active BIT NOT NULL,
    ProfilePhoto varbinary(max) NOT NULL,
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET())

    UNIQUE(Username, Email)
) 
GO

 CREATE TABLE Application.Groups
(
    GroupId INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(32) NOT NULL,
    OrganizationId INT NOT NULL FOREIGN KEY
    REFERENCES Application.Organizations(OrganizationId),
    Active BIT NOT NULL,
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET())
)
GO

CREATE TABLE Application.Memberships
(
    MembershipId INT IDENTITY(1,1) PRIMARY KEY,
    GroupId INT NOT NULL FOREIGN KEY 
    REFERENCES Application.Groups(GroupId),
    UserId INT NOT NULL FOREIGN KEY
    REFERENCES Application.Users(UserId),
    OrganizationId INT NOT NULL FOREIGN KEY
    REFERENCES Application.Organizations(OrganizationId)

    UNIQUE(GroupId, UserId, OrganizationId)
)
GO


CREATE TABLE Application.Channels
(
    ChannelId INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(32) NOT NULL,
    GroupId INT FOREIGN KEY 
    REFERENCES Application.Groups(GroupId),
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),

    UNIQUE([Name])
)
GO

CREATE TABLE Application.Messages
(
    MsgId INT IDENTITY(1,1) PRIMARY KEY,
    SenderId INT NOT NULL FOREIGN KEY
    REFERENCES Application.Users(UserId),
    [Message] NVARCHAR(255) NOT NULL,
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    ChannelId INT NULL FOREIGN KEY 
    REFERENCES Application.Channels(ChannelId),
    RecipientId INT NULL FOREIGN KEY 
    REFERENCES Application.Users(UserId),
    

)
GO
ALTER TABLE Application.Users
ALTER COLUMN ProfilePhoto NVARCHAR(max);

/* Grant Permissions */

GRANT INSERT ON SCHEMA :: Application TO sbrittain;  
GRANT SELECT ON SCHEMA :: Application TO sbrittain; 

GRANT INSERT ON SCHEMA :: Application TO hcossins;  
GRANT SELECT ON SCHEMA :: Application TO hcossins; 

GRANT INSERT ON SCHEMA :: Application TO cohammo;  
GRANT SELECT ON SCHEMA :: Application TO cohammo; 

GRANT UPDATE ON SCHEMA :: Application TO sbrittain;
GRANT DELETE ON SCHEMA :: Application TO sbrittain;


ALTER TABLE Application.Messages ADD CONSTRAINT [check_only_one_recipient] CHECK(
        Messages.RecipientId IS NULL AND Messages.ChannelId IS NOT NULL OR Messages.ChannelId IS NULL AND Messages.RecipientId IS NOT NULL)
