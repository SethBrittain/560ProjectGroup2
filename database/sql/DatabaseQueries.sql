
/* Aggregating Query 1 */ 
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

/* General Query 1: Fetch all messages for a channel */
CREATE OR ALTER PROCEDURE Application.GetAllChannelMessages
@ChannelId INT
AS
SELECT M.Message, M.SenderId /* update to include the name of the user who send the message */ 
FROM Application.Channels C
INNER JOIN Application.Messages M ON M.ChannelId = C.ChannelId
WHERE C.ChannelId = @ChannelId
ORDER BY M.CreatedOn ASC; 
GO

/* General Query 2: Fetch all messages between given sender and recipient */
CREATE OR ALTER PROCEDURE Application.GetDirectMessages
@UserAId INT,
@UserBId INT
AS
 SELECT M.Message, M.SenderId
 FROM Application.Messages M
 WHERE M.SenderId = @UserAId AND M.RecipientId = @UserBId OR M.SenderId = @UserBId AND M.RecipientId = @UserAId; 
GO

/* General Query 3: Show all messages that match a substring within a given channel over a specified date range. */
CREATE OR ALTER PROCEDURE Application.GetAllMessagesMatchingSubstring   
@Substring NVARCHAR(255),
@ChannelId INT
AS
 SELECT M.Message, M.SenderId
 FROM Application.Channels C
 INNER JOIN Application.Messages M ON M.ChannelId = C.ChannelId
 WHERE C.ChannelId = @ChannelId
 AND M.Message LIKE '%' + @Substring + '%'   
GO

/* General Query 4: Get all channels in a Organization */
CREATE OR ALTER PROCEDURE Application.GetAllChannelsInOrganization
@OrganizationId INT
AS
SELECT C.ChannelId, C.Name
FROM Application.Organizations O
INNER JOIN Application.Groups G ON G.OrganizationId = O.OrganizationId
INNER JOIN Application.Channels C ON C.GroupId = G.GroupId
WHERE O.OrganizationId = @OrganizationId
GO

/*General Query 5: Get All Channels In Groups*/
CREATE OR ALTER PROCEDURE Application.GetAllChannelsInGroup
@OrganizationId INT,
@GroupId INT
AS
SELECT *
FROM Application.Groups G
INNER JOIN Application.Organizations O ON O.OrganizationId = G.OrganizationId
WHERE G.GroupId = @GroupId AND O.OrganizationId = @OrganizationId
GO

/* General Query 6: Get all users in Organization */
CREATE OR ALTER PROCEDURE Application.GetAllUsersInOrganization
@OrganizationId INT 
AS 
SELECT U.UserId, U.FirstName, U.LastName, U.Username
FROM Application.Organizations O 
INNER JOIN Application.Users U ON O.OrganizationId = U.OrganizationId
WHERE O.OrganizationId = @OrganizationId;
GO

/* General Query 7: Get users info via username */ 
CREATE OR ALTER PROCEDURE Application.GetUserInfo
@Username NVARCHAR(32)
AS
SELECT U.Email, U.FirstName, U.LastName, U.Password, U.OrganizationId
FROM Application.Users U
WHERE U.Email = @Username
GO

/*General Query 8: Insert Message into channel */
CREATE OR ALTER PROCEDURE Application.InsertMessageIntoChannel
@Message NVARCHAR(128),
@SenderId INT,
@ChannelId INT
AS 
INSERT INTO Application.Messages ([Message], SenderId, ChannelId)
VALUES (@Message, @SenderId, @ChannelId)
GO 

/*General Query 9: Insert Direct Message */
CREATE OR ALTER PROCEDURE Application.InsertDirectMessage
@Message NVARCHAR(128),
@SenderId INT,
@RecipientId INT
AS 
INSERT INTO Application.Messages ([Message], SenderId, RecipientId)
VALUES (@Message, @SenderId, @RecipientId)
GO

/*Query 10: Insert New User*/
CREATE OR ALTER PROCEDURE Application.InsertNewUser
@OrganizationId INT,
@Username NVARCHAR(64),
@Email NVARCHAR(128),
@Password NVARCHAR(128),
@FirstName NVARCHAR(64),
@LastName NVARCHAR(64),
@Title NVARCHAR(64),
@ProfilePhoto NVARCHAR(max)
AS

INSERT INTO Application.Users
(OrganizationId, Username, [Password], FirstName, LastName, Title, ProfilePhoto)
VALUES (@OrganizationId, @Username, @Password, @FirstName, @LastName, @Title, @ProfilePhoto)
GO