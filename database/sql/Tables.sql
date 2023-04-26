CREATE TABLE Application.Organizations
( 
    OrganizationId INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(64) NOT NULL,
    Active BIT NOT NULL,
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET())

    UNIQUE([Name])
)

CREATE TABLE Application.Users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
	OrganizationId INT NOT NULL FOREIGN KEY REFERENCES Application.Organizations(OrganizationId),
    Email NVARCHAR(128) NOT NULL,
	FirstName NVARCHAR(64) NOT NULL,
    LastName NVARCHAR(64) NOT NULL,
    Title NVARCHAR(64),
    ProfilePhoto NVARCHAR(max) NULL,
	Active BIT NOT NULL DEFAULT (1),
    ApiKey VARBINARY(max) DEFAULT (HASHBYTES('SHA2_256', SUBSTRING(CAST (NEWID() AS nvarchar(36)), 1, 32))) NOT NULL,
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET())

    UNIQUE(Email)
)

 CREATE TABLE Application.Groups
(
    GroupId INT IDENTITY(1,1) PRIMARY KEY,
    OrganizationId INT NOT NULL FOREIGN KEY REFERENCES Application.Organizations(OrganizationId),
	[Name] NVARCHAR(64) NOT NULL,
    Active BIT NOT NULL,
    CreatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    UpdatedOn DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET())

	UNIQUE([Name], OrganizationId)
)

CREATE TABLE Application.Memberships
(
    MembershipId INT IDENTITY(1,1) PRIMARY KEY,
    GroupId INT NOT NULL FOREIGN KEY REFERENCES Application.Groups(GroupId),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Application.Users(UserId),

    UNIQUE(GroupId, UserId),
	CONSTRAINT [user_must_not_be_in_group_twice] UNIQUE(GroupId, UserId),
	CONSTRAINT [user_and_group_must_have_same_organization] CHECK(Application.fn_CheckOrganizations(UserId, GroupId) = 1)
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