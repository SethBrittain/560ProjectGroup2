
-- Get all the messages in a channel
CREATE OR ALTER PROCEDURE Application.GetAllChannelMessages
@ChannelId INT
AS
SELECT M.MsgId, M.Message, M.UpdatedOn, M.SenderId, U.FirstName, U.LastName, U.ProfilePhoto
FROM Application.Channels C
	INNER JOIN Application.Messages M ON M.ChannelId = C.ChannelId
	INNER JOIN Application.Users U ON M.SenderId = U.UserId
WHERE C.ChannelId = @ChannelId
ORDER BY M.CreatedOn ASC;
GO

-- Get all direct messages between two given users
CREATE OR ALTER PROCEDURE Application.GetDirectMessages
@CurrentUserId INT,
@OtherUserId INT
AS
SELECT M.MsgId, U.FirstName, U.LastName, M.Message, M.UpdatedOn, M.SenderId, U.ProfilePhoto, IIF(M.SenderId = @CurrentUserId, 1, 0) AS IsMine
FROM Application.Messages M
	INNER JOIN Application.Users U ON M.SenderId = U.UserId
WHERE M.SenderId = @CurrentUserId AND M.RecipientId = @OtherUserId OR M.SenderId = @OtherUserId AND M.RecipientId = @CurrentUserId; 
GO

-- Get the profile photo for a given user
CREATE OR ALTER PROCEDURE Application.GetProfilePhoto
@UserId INT
AS
SELECT U.ProfilePhoto
FROM Application.Users U
WHERE U.UserId = @UserId
GO

-- Get all people who have direct messages with a given user
CREATE OR ALTER PROCEDURE Application.GetDirectMessageChats
@UserId INT
AS
-- Get all direct messages sent from the user
SELECT U2.UserId, U2.FirstName, U2.LastName, M.MsgId, M.SenderId, M.RecipientId
FROM Application.Users U1
	INNER JOIN Application.Messages M ON U1.UserId = M.SenderId
	INNER JOIN Application.Users U2 ON M.RecipientId = U2.UserId
WHERE U1.UserId = @UserId
UNION
-- Get all direct messages sent to the user
SELECT U2.UserId, U2.FirstName, U2.LastName, M.MsgId, M.SenderId, M.RecipientId
FROM Application.Users U1
	INNER JOIN Application.Messages M ON U1.UserId = M.RecipientId
	INNER JOIN Application.Users U2 ON M.SenderId = U2.UserId
WHERE U1.UserId = @UserId
GO

-- Get all the users in an organization that match a given search string
CREATE OR ALTER PROCEDURE Application.SearchUsers
@Substring INT
AS
SELECT U.UserId, U.FirstName, U.LastName
FROM Application.Users U
WHERE U.FirstName + ' ' + U.LastName LIKE '%' + @Substring + '%'
	OR U.Email LIKE '%' + @Substring + '%'
GO

-- Get all the messages in a channel that match a given search string
CREATE OR ALTER PROCEDURE Application.SearchAllChannelMessages
@Substring NVARCHAR(512),
@ChannelId INT,
@UserId INT
AS
SELECT M.Message, M.SenderId, M.ChannelId, M.RecipientId, M.CreatedOn, IIF(M.SenderId = @UserId, 1, 0) AS IsMine
FROM Application.Channels C
INNER JOIN Application.Messages M ON M.ChannelId = C.ChannelId
WHERE C.ChannelId = @ChannelId
AND M.Message LIKE '%' + @Substring + '%'   
GO

-- Get all the messages sent to or from a user and search for a given substring
CREATE OR ALTER PROCEDURE Application.SearchAllUserMessages
@Substring NVARCHAR(512),
@UserId INT
AS
WITH AllUserMessagesCte(MsgId, [Message], UpdatedOn, SenderId, FirstName, LastName, ProfilePhoto, RecipientId, ChannelId, [Name]) AS
(
	-- Get all direct messages sent to or from a user
	SELECT ME.MsgId, ME.Message, ME.UpdatedOn, ME.SenderId, U2.FirstName, U2.LastName, U2.ProfilePhoto, ME.RecipientId, ME.ChannelId, C.Name AS ChannelName
	FROM Application.Users U1
		INNER JOIN Application.Messages ME ON U1.UserId = ME.SenderID OR U1.UserId = RecipientId
		LEFT JOIN Application.Channels C ON ME.ChannelId = C.ChannelId
		INNER JOIN Application.Users U2 ON ME.SenderId = U2.UserId
	WHERE U1.UserId = @UserId
	UNION
	-- Get all channel messages sent to or from a user
	SELECT ME.MsgId, ME.Message, ME.UpdatedOn, ME.SenderId, U2.FirstName, U2.LastName, U2.ProfilePhoto, ME.RecipientId, ME.ChannelId, C.Name AS ChannelName
	FROM Application.Users U1
		INNER JOIN Application.Memberships M ON U1.UserId = M.UserId
		INNER JOIN Application.Channels C ON M.GroupId = C.GroupId
		INNER JOIN Application.Messages ME ON C.ChannelId = ME.ChannelId
		INNER JOIN Application.Users U2 ON ME.SenderId = U2.UserId
	WHERE U1.UserId = @UserId
)
SELECT *
FROM AllUserMessagesCte A
WHERE [Message] LIKE '%' + @Substring + '%'
ORDER BY UpdatedOn DESC
GO

-- Get all the channels that a user is in
CREATE OR ALTER PROCEDURE Application.GetAllChannelsOfUser
@UserId INT
AS
SELECT C.ChannelId, C.Name
FROM Application.Users U
	INNER JOIN Application.Memberships M ON U.UserId = M.UserId
	INNER JOIN Application.Channels C ON M.GroupId = C.GroupId
WHERE U.UserId = @UserId
GO

-- Get the name of the given ChannelId
CREATE OR ALTER PROCEDURE Application.GetChannelName
@ChannelId INT
AS
SELECT C.Name
FROM Application.Channels C
WHERE C.ChannelId = @ChannelId
GO

-- Get the name of the given UserId
CREATE OR ALTER PROCEDURE Application.GetChannelName
@UserId INT
AS
SELECT U.FirstName, U.LastName, U.ProfilePhoto
FROM Application.Users U
WHERE U.UserId = @UserId
GO

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

-- Delete the given message
CREATE OR ALTER PROCEDURE Application.DeleteMessage
@MsgId INT
AS
DELETE FROM Application.Messages WHERE MsgId = @MsgId
GO

-- Update the given message with the given update string
CREATE OR ALTER PROCEDURE Application.UpdateMessage
@MsgId INT,
@Message NVARCHAR(512)
AS
SELECT *
FROM Application.Messages
UPDATE Application.Messages
SET [Message] = @Message
WHERE MsgId = @MsgId

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