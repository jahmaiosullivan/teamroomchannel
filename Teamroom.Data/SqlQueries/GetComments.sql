SELECT c.[Id]
      ,c.[Body]
      ,c.[DeckId]
      ,c.[IsActive]
      ,c.[ParentId]
      ,c.[CreatedBy]
      ,c.[CreatedDate]
      ,c.[LastUpdatedDate]
      ,c.[LastUpdatedBy]
  FROM 
	[dbo].[Comments] c