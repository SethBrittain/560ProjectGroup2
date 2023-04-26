
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
SELECT M.MsgId, M.Message, M.UpdatedOn, M.SenderId, M.RecipientId, U.FirstName, U.LastName, U.ProfilePhoto, IIF(M.SenderId = @CurrentUserId, 1, 0) AS IsMine
FROM Application.Messages M
	INNER JOIN Application.Users U ON M.SenderId = U.UserId
WHERE M.SenderId = @CurrentUserId AND M.RecipientId = @OtherUserId OR M.SenderId = @OtherUserId AND M.RecipientId = @CurrentUserId; 
GO

-- Get all the messages in a channel that match a given search string
CREATE OR ALTER PROCEDURE Application.SearchChannelMessages
@UserId INT,
@ChannelId INT,
@Substring NVARCHAR(512)
AS
SELECT M.MsgId, M.Message, M.SenderId, M.ChannelId, M.RecipientId, M.UpdatedOn, IIF(M.SenderId = @UserId, 1, 0) AS IsMine
FROM Application.Channels C
INNER JOIN Application.Messages M ON M.ChannelId = C.ChannelId
WHERE C.ChannelId = @ChannelId
AND M.Message LIKE '%' + @Substring + '%'  
GO


-- edit to not have nulls
-- Get all the messages sent to or from a user that match a given search string
CREATE OR ALTER PROCEDURE Application.SearchUserMessages
@UserId INT,
@Substring NVARCHAR(512)
AS
WITH AllUserMessagesCte(MsgId, [Message], UpdatedOn, SenderId, Id, [Type], FirstName, LastName, ProfilePhoto, [Name]) AS
(
	-- Get all direct messages sent to or from a user
	SELECT ME.MsgId, ME.Message, ME.UpdatedOn, ME.SenderId, IIF(ME.SenderId = @UserId,IIF(ME.ChannelId IS NULL, ME.RecipientId, ME.ChannelId),IIF(ME.ChannelId IS NULL, ME.SenderId, ME.ChannelId)), IIF(ME.ChannelId IS NULL, 'chat','channel'), U2.FirstName, U2.LastName, U2.ProfilePhoto, IIF(ME.ChannelId IS NULL, U1.FirstName + ' ' +  U1.LastName, C.Name) AS [Name]
	FROM Application.Users U1
		INNER JOIN Application.Messages ME ON U1.UserId = ME.SenderID OR U1.UserId = RecipientId
		LEFT JOIN Application.Channels C ON ME.ChannelId = C.ChannelId
		INNER JOIN Application.Users U2 ON ME.SenderId = U2.UserId
	WHERE U1.UserId = @UserId
	UNION
	-- Get all channel messages sent to or from a user
	SELECT ME.MsgId, ME.Message, ME.UpdatedOn, ME.SenderId,  IIF(ME.SenderId = @UserId,IIF(ME.ChannelId IS NULL, ME.RecipientId, ME.ChannelId),IIF(ME.ChannelId IS NULL, ME.SenderId, ME.ChannelId)), IIF(ME.ChannelId IS NULL, 'chat','channel'), U2.FirstName, U2.LastName, U2.ProfilePhoto, IIF(ME.ChannelId IS NULL, U1.FirstName + ' ' + U1.LastName, C.Name) AS [Name]
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
-- Get all users who have direct messages with a given user
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

-- Get the profile photo for a given user
CREATE OR ALTER PROCEDURE Application.GetProfilePhoto
@UserId INT
AS
SELECT U.ProfilePhoto, U.FirstName, U.LastName
FROM Application.Users U
WHERE U.UserId = @UserId
GO

-- Get the channel name for a given channel
CREATE OR ALTER PROCEDURE Application.GetChannelName
@ChannelId INT
AS
SELECT C.Name
FROM Application.Channels C
WHERE C.ChannelId = @ChannelId
GO

-- Get all the users in an organization that match a given search string
CREATE OR ALTER PROCEDURE Application.SearchUsersInOrganization
@Substring INT,
@OrganizationId INT
AS
SELECT U.UserId, U.FirstName, U.LastName, U.ProfilePhoto
FROM Application.Users U
WHERE (U.FirstName + ' ' + U.LastName LIKE '%' + @Substring + '%'
	OR U.Email LIKE '%' + @Substring + '%') AND U.OrganizationId = @OrganizationId
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
SELECT TOP(10) U.UserId, U.FirstName, U.LastName, U.ProfilePhoto
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
SET [Message] = @Message, UpdatedOn = SYSDATETIMEOFFSET()
WHERE MsgId = @MsgId
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
*/


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
CREATE OR ALTER PROCEDURE Application.GetMonthlyTraffic
@FirstDate DATETIMEOFFSET,
@LastDate DATETIMEOFFSET
AS
SELECT DATENAME(month, M.CreatedOn) AS MONTH, DATENAME(year,M.CreatedOn) AS YEAR,
Count(*) AS MessagesSent, 
RANK() OVER (ORDER BY COUNT(*) DESC) AS Rank
FROM Application.Messages M
WHERE 
M.CreatedOn >= @FirstDate AND M.CreatedOn <= @LastDate
GROUP BY DATENAME(month, M.CreatedOn), DATENAME(year,M.CreatedOn)
GO

SELECT *
FROM Application.Messages M;
GO

-- Aggregate Query 4 -- get the growth of users from a given date and over a given number of months

CREATE OR ALTER PROCEDURE Application.GetAppGrowth
@StartDate DATETIMEOFFSET,
@EndDate   DATETIMEOFFSET
AS
SELECT SUM( IIF(U.Active = 1, 1, 0)) AS NumberOfActiveUsers,SUM(IIF(U.Active = 0, 1, 0)) AS NumberOfInactiveUsers,
(
	SELECT COUNT(*)
	FROM Application.Organizations O
	WHERE O.Active = 1
) AS NumberOfActiveOrgs,
(
	SELECT COUNT(*)
	FROM Application.Organizations O
	WHERE O.Active = 0
)AS NumberOfInactiveOrgs
FROM Application.Users U;


EXEC  Application.SearchUserMessages 4," ";
EXEC Application.GetDirectMessages 3,4;

EXEC Application.GetMonthlyTraffic '01/01/2022','02/25/2023';

EXEC Application.GetAppGrowth '01/05/2022','01/05/2023';

INSERT INTO Application.Organizations([Name],Active,CreatedOn,UpdatedOn)
VALUES
('Garmin',1,'01/01/2022','01/01/2022'),
('AutoZone',1,'01/01/2022','01/01/2022'),
('MatressHub',1,'01/01/2022','01/01/2022'),
('KU',0,'01/01/2022','01/01/2022')
GO

SELECT *
FROM Application.Organizations O;

SELECT *
FROM Application.Messages;

SELECT *
FROM Application.Messages M
WHERE 
M.CreatedOn >= '2022-04-01' AND M.CreatedOn <= '2022-5-01';

EXEC Application.GetMonthlyTraffic '2022-01-01','2023-12-01';

EXEC Application.GetAppGrowth '2022-04-01','2022-5-01';