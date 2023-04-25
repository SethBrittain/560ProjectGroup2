
-- Get all the messages in a channel
CREATE OR ALTER PROCEDURE Application.GetAllChannelMessages
@ChannelId INT
AS
SELECT M.Message, U.FirstName, U.LastName , M.CreatedOn /* update to include the name of the user who send the message */ 
FROM Application.Channels C
INNER JOIN Application.Messages M ON M.ChannelId = C.ChannelId
INNER JOIN Application.Users U ON M.SenderId = U.UserId
WHERE C.ChannelId = @ChannelId
ORDER BY M.CreatedOn ASC; 
GO

-- Get all direct messages between two given users
CREATE OR ALTER PROCEDURE Application.GetDirectMessages
@UserAId INT,
@UserBId INT
AS
SELECT M.Message, M.SenderId
FROM Application.Messages M
WHERE M.SenderId = @UserAId AND M.RecipientId = @UserBId OR M.SenderId = @UserBId AND M.RecipientId = @UserAId; 
GO

-- Get all people who have direct messages with a given user

-- Get all users that match a given search substring
CREATE OR ALTER PROCEDURE Application.GetUsersMatchingSubstring
@Substring NVARCHAR(512)
AS
SELECT U.UserId, U.FirstName, U.LastName
FROM Application.Users U
WHERE U.FirstName + ' ' + U.LastName LIKE '%' + @Substring + '%'
	OR U.Email LIKE '%' + @Substring + '%'
GO

-- Get all the messages that match a given search substring
CREATE OR ALTER PROCEDURE Application.GetAllMessagesMatchingSubstring
@Substring NVARCHAR(512),
@ChannelId INT
AS
SELECT M.Message, M.SenderId,M.ChannelId, M.RecipientId, M.CreatedOn
FROM Application.Channels C
INNER JOIN Application.Messages M ON M.ChannelId = C.ChannelId
WHERE C.ChannelId = @ChannelId
AND M.Message LIKE '%' + @Substring + '%'   
GO

-- not working yet
CREATE OR ALTER PROCEDURE Application.GetAllMessagesOfUserMatchingSubstring
@Substring NVARCHAR(512),
@UserId INT
AS
SELECT U.UserId, G.GroupId, G.Name AS GroupName, C.ChannelId, C.Name AS ChannelName, ME.RecipientId, ME.ChannelId, ME.MsgId, ME.Message
FROM Application.Users U
	INNER JOIN Application.Memberships M ON U.UserId = M.UserId
	INNER JOIN Application.Groups G ON M.GroupId = G.GroupId
	INNER JOIN Application.Channels C ON G.GroupId = C.GroupId
	INNER JOIN Application.Messages ME ON U.UserId = ME.SenderId OR C.ChannelId = ME.ChannelId
WHERE U.UserId = @UserId --AND ME.Message LIKE '%' + @Substring + '%'  
ORDER BY U.UserId ASC
GO

CREATE OR ALTER PROCEDURE Application.GetAllChannelsOfUser
@UserId INT
AS
SELECT C.ChannelId, C.Name
FROM Application.Users U
	INNER JOIN Application.Memberships M ON U.UserId = M.UserId
	INNER JOIN Application.Channels C ON M.GroupId = C.GroupId
WHERE U.UserId = @UserId
GO

-- query to get the name of a channel or person based on the channelId or UserId

-- Get all the channels in an organization
CREATE OR ALTER PROCEDURE Application.GetAllChannelsInOrganization
@OrganizationId INT
AS
SELECT C.ChannelId, C.Name
FROM Application.Organizations O
INNER JOIN Application.Groups G ON G.OrganizationId = O.OrganizationId
INNER JOIN Application.Channels C ON C.GroupId = G.GroupId
WHERE O.OrganizationId = @OrganizationId
GO

-- Get all the users in an organization
CREATE OR ALTER PROCEDURE Application.GetAllUsersInOrganization
@OrganizationId INT 
AS 
SELECT U.UserId, U.FirstName, U.LastName, U.ProfilePhoto
FROM Application.Organizations O 
INNER JOIN Application.Users U ON O.OrganizationId = U.OrganizationId
WHERE O.OrganizationId = @OrganizationId;
GO

-- Insert a channel message
CREATE OR ALTER PROCEDURE Application.InsertMessageIntoChannel
@Message NVARCHAR(128),
@SenderId INT,
@ChannelId INT
AS 
INSERT INTO Application.Messages ([Message], SenderId, ChannelId)
VALUES (@Message, @SenderId, @ChannelId)
GO 

-- Insert a direct message
CREATE OR ALTER PROCEDURE Application.InsertDirectMessage
@Message NVARCHAR(128),
@SenderId INT,
@RecipientId INT
AS 
INSERT INTO Application.Messages ([Message], SenderId, RecipientId)
VALUES (@Message, @SenderId, @RecipientId)
GO

-- Insert a new user
CREATE OR ALTER PROCEDURE Application.InsertNewUser
@OrganizationId INT,
@Email NVARCHAR(128),
@FirstName NVARCHAR(64),
@LastName NVARCHAR(64),
@Title NVARCHAR(64),
@ProfilePhoto NVARCHAR(max)
AS
INSERT INTO Application.Users
(OrganizationId, FirstName, LastName, Title, ProfilePhoto)
VALUES (@OrganizationId, @FirstName, @LastName, @Title, @ProfilePhoto)
GO

-- delete message from database
CREATE OR ALTER PROCEDURE Application.DeleteMessage
@MessageId INT
AS
DELETE FROM Application.Messages WHERE MsgId = @MessageId
GO

/*
-- get user id from api key
CREATE PROCEDURE Application.GetUserIdFromAPIKey
@apikey NVARCHAR(MAX)
AS
SELECT U.UserId
FROM Application.Users U
WHERE U.ApiKey = @apikey;
GO


-- Aggregated Queries

-- Aggregate Query 1
CREATE OR ALTER PROCEDURE Application.GetOrganizationData
@FirstDate DATETIMEOFFSET,
@LastDate DATETIMEOFFSET
AS
SELECT O.Name, Count(DISTINCT IIF(U.Active = 1, U.UserId, NULL)) AS ActiveUserCount, Count(M.MsgId) AS MessageCount
FROM Application.Organizations O
INNER JOIN Application.Users U ON O.OrganizationId = U.OrganizationId
LEFT JOIN Application.Messages M ON M.SenderId = U.UserId
WHERE M.CreatedOn BETWEEN @FirstDate AND @LastDate
GROUP BY O.Name
GO

-- Come up with a 2nd one

-- Aggregate Query 3 -- Get the message traffic over a given month or months?
CREATE PROCEDURE Application.GetMonthlyTraffic
@FirstDate DATETIMEOFFSET,
@LastDate DATETIMEOFFSET
AS
SELECT DATEPART(hour, M.CreatedOn) AS Hours,
Count(*) AS MessagesSent, 
RANK() OVER (ORDER BY COUNT(*) DESC) AS Rank
FROM Application.Messages M
WHERE 
M.CreatedOn >= @FirstDate AND M.CreatedOn <= @LastDate
GROUP BY DATEPART(hour, M.CreatedOn);
GO

SELECT *
FROM Application.Messages M;

-- Aggregate Query 4 -- get the growth of users from a given date and over a given number of months
CREATE PROCEDURE Application.GetAppGrowth
@StartDate DATETIMEOFFSET,
@Months INT
AS
SELECT 
*/


SELECT C.ChannelId, C.Name
FROM Application.Users U
	INNER JOIN Application.Memberships M ON U.UserId = M.UserId
	INNER JOIN Application.Channels C ON M.GroupId = C.GroupId
WHERE U.UserId = 9;