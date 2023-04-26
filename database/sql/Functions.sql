
CREATE OR ALTER FUNCTION Application.fn_CheckOrganizations(@UserId INT, @GroupId INT)
	RETURNS INT
	AS
	BEGIN
		DECLARE @Result INT
		IF ((SELECT U.OrganizationId FROM Application.Users U WHERE U.UserId = @UserId) = 
			(SELECT G.OrganizationId FROM Application.Groups G WHERE G.GroupId = @GroupId))
			RETURN 1
		RETURN 0;
END;
