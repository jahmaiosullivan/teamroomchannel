SELECT e.*, 
	       g.Name as "GroupName",
		   g.Url as "GroupUrl" 
FROM [Events] e  inner join Groups g on e.GroupId = g.Id  
	  WHERE e.GroupId in  
	  	  (Select gm.GroupId from GroupMembers gm where gm.UserId = @userId)
ORDER BY e.StartDateTime ASC