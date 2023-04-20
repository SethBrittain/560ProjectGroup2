USE slhaynes4542
GO


/* Aggregating Query 1 */ 
CREATE PROCEDURE Application.GetOrganizationData
@FirstDate DATETIME,
@LastDate DATETIME
AS
SELECT O.Name, SUM(IIF(U.Active = 1,1,0)) AS ActiveUserCount, Count(M.MsgId) AS MessageCount
FROM Application.Organizations O
INNER JOIN Application.Users U ON O.OrganizationId = U.OrganizationId
LEFT JOIN Application.Messages M ON M.SenderId = U.UserId
WHERE M.CreatedOn BETWEEN @FirstDate AND @LastDate
GROUP BY O.Name
GO



/* General Query 1: Fetch all messages for a channel */
CREATE PROCEDURE Application.GetAllChannelMessages
@ChannelId INT
AS
SELECT M.Message
FROM Application.Channels C
INNER JOIN Application.Messages M ON M.ChannelId = C.ChannelId
WHERE C.ChannelId = @ChannelId
ORDER BY M.CreatedOn ASC; 
GO


/* General Query 2: Fetch all messages between given sender and recipient */
CREATE PROCEDURE Application.GetAllMessagesBetweenUsers 
@UserAId INT,
@UserBId INT
AS
 SELECT M.Message
 FROM Application.Messages M
 WHERE M.SenderId = @UserAId AND M.RecipientId = @UserBId OR M.SenderId = @UserBId AND M.RecipientId = @UserAId; 
GO

/* General Query 3: Show all messages that match a substring within a given channel over a specified date range. */
CREATE PROCEDURE Application.GetAllMessagesMatchingSubstring   
@FirstDate DATETIME, 
@LastDate DATETIME, 
@Substring NVARCHAR(255),
@ChannelId INT
AS
 SELECT *
 FROM Application.Channels C
 INNER JOIN Application.Messages M ON M.ChannelId = C.ChannelId
 WHERE C.ChannelId = @ChannelId AND M.CreatedOn BETWEEN @FirstDate AND @LastDate
 AND M.Message LIKE '%' + @Substring + '%'   
GO

/* General Query 4: Fetch all channels in a group */
CREATE PROCEDURE Application.GetAllChannelsInGroup
@GroupId INT
AS
SELECT C.ChannelId, C.Name
FROM Application.Groups G
INNER JOIN Application.Channels C ON C.GroupId = G.GroupId
WHERE G.GroupId = @GroupId
GO

/* General Query 5: Query Channel matching substring */
CREATE PROCEDURE Application.GetChannelsMatchingSubstring
@Substring NVARCHAR(255)
AS
SELECT C.ChannelId, C.Name
FROM Application.Channels C
WHERE C.Name LIKE '%' + @Substring + '%'
GO

/* General Query 6: Query Channel across all groups */
CREATE PROCEDURE Application.GetAllChannelsInOrganization
@OrganizationId INT
AS
SELECT C.ChannelId, C.Name
FROM Application.Organizations O 
INNER JOIN Application.Groups G ON O.OrganizationId = G.OrganizationId
WHERE O.OrganizationId = @OrganizationId
GO

/* General Query 7: Get all groups user is in */ 
CREATE PROCEDURE Application.GetAllGroupsUserIsIn
@UserId INT
AS 
SELECT G.GroupId, G.Name
FROM Application.Users U
INNER JOIN Application.Memberships M ON U.UserId = M.UserId
INNER JOIN Application.Groups G ON G.GroupId = M.GroupId
WHERE U.UserId = @UserId 
GO

/* General Query 8: Get all users in Organization */
CREATE PROCEDURE Application.GetAllUsersInOrganization
@OrganizationId INT 
AS 
SELECT U.UserId, U.FirstName, U.LastName, U.ProfilePhoto
FROM Application.Organizations O 
INNER JOIN Application.Users U ON O.OrganizationId = U.OrganizationId
WHERE O.OrganizationId = @OrganizationId
GO

/* General Query 9: Get users info via username */ 
CREATE PROCEDURE Application.GetUserInfo
@Username NVARCHAR(32)
AS
SELECT U.UserName, U.FirstName, U.LastName, U.Email, U.Password, U.OrganizationId
FROM Application.Users U
WHERE U.Username = @Username
GO 




 


 



