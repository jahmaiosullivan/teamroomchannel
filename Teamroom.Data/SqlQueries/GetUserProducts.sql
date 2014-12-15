SELECT p.[Id]
      ,p.[CompanyId]
      ,p.[Name]
      ,p.[Description]
      ,p.[LogoUrl]
      ,p.[IsActive]
      ,p.[CreatedBy]
      ,p.[CreatedDate]
      ,p.[LastUpdatedBy]
      ,p.[LastUpdatedDate]
  FROM 
	[Products] p INNER JOIN [UserProducts] up on up.ProductId = p.Id 
  WHERE 
    up.UserId = @UserId
  ORDER BY 
	up.DateAdded DESC