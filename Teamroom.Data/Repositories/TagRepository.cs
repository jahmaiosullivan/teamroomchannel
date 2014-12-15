using System;
using System.Collections.Generic;
using System.Linq;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public class TagRepository : BaseDapperRepository<Tag>, ITagRepository
    {
        public TagRepository(IQueryManager queryManager) : base( queryManager)
        {
        }

        public IDictionary<long, IList<Tag>> Get(IEnumerable<long> cardIds)
        {
            var result = new Dictionary<long, IList<Tag>>();
            return result;
        }

        public IEnumerable<Tag> GetTopLevelTags()
        {
            var query = BaseQuery + " WHERE ParentId is NULL AND EXISTS (SELECT 1 from Decks d where d.MainTagId = t.Id)";
            return _queryManager.ExecuteSql<Tag>(query);
        }

        public IEnumerable<Tag> GetForHobby(long hobbyId)
        {
            var query = BaseQuery + " WHERE Id in (Select TagId from HobbyTags where HobbyId = @HobbyId)";
            return _queryManager.ExecuteSql<Tag>(query, new { @HobbyId = hobbyId });
        }

        public IEnumerable<Tag> GetChildTags(long tagId)
        {
            var query = BaseQuery + " WHERE ParentId = @TagId";
            return _queryManager.ExecuteSql<Tag>(query, new { @TagId = tagId });
        }

        public IEnumerable<Tag> GetUsedByCards()
        {
            const string query = "SELECT distinct t.* FROM [Tags] t INNER JOIN [CardTags] ct on t.Id = ct.TagId";
            return _queryManager.ExecuteSql<Tag>(query);
        }


        public IEnumerable<Tag> GetForUser(Guid userId)
        {
            const string query = "SELECT DISTINCT t.* FROM [Tags] t INNER JOIN [UserTags] ut on t.Id = ut.TagId where UserId = @userId";
            return _queryManager.ExecuteSql<Tag>(query, new { @userId = userId});
        }

        public IEnumerable<Tag> GetCityDefaultTags(long cityId)
        {
            const string query = "SELECT DISTINCT t.* FROM [Tags] t INNER JOIN [CityTags] ct on t.Id = ct.TagId where CityId = @cityId";
            return _queryManager.ExecuteSql<Tag>(query, new { @cityId = cityId });
        }

        public IEnumerable<Tag> GetLastUsed(int number, int pageNum)
        {
            const string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY a.CreatedDate) as NUMBER,t.* FROM (SELECT ct.*,c.CreatedDate,ROW_NUMBER() OVER (PARTITION BY ct.TagId ORDER BY ct.CardId) AS RowNumber FROM [CardTags] ct inner join [Cards] c on c.Id = ct.CardId) as a inner join Tags t on t.Id = a.TagId  WHERE COALESCE(t.IsActive, 1) = 1 and a.RowNumber = 1) As tags WHERE tags.Number BETWEEN ((@PageNumber - 1) * @number + 1) AND (@PageNumber * @number)";
            return _queryManager.ExecuteSql<Tag>(query, new { @number = number,  @PageNumber = pageNum });
        }

        public Tag GetForUser(Guid userId, long tagId)
        {
            const string query = "SELECT distinct t.* FROM [Tags] t INNER JOIN [UserTags] ut on t.Id = ut.TagId where UserId = @userId and TagId = @tagId";
            return _queryManager.ExecuteSql<Tag>(query, new { @userId = userId }).FirstOrDefault();
        }

        public void AddUserTag(Guid userId, long tagId)
        {
            const string query = "IF NOT EXISTS(SELECT 1 FROM [UserTags] WHERE UserId=@UserId and TagId = @TagId ) " +
                                 "   INSERT INTO [UserTags] ([UserId],[TagId]) VALUES(@UserId,@TagId )";
            _queryManager.ExecuteNonQuery(query, new { @UserId = userId, @TagId = tagId });
        }

        public void DeleteUserTag(Guid userId, long tagId)
        {
            const string query = "DELETE FROM [UserTags] WHERE TagId = @TagId and UserId=@UserId";
            _queryManager.ExecuteNonQuery(query, new { @UserId = userId, @TagId = tagId });
        }

        public void AddCardTag(long tagId, long cardId)
        {
            const string query = "IF NOT EXISTS(SELECT 1 FROM [CardTags] WHERE CardId=@CardId and TagId = @TagId ) " +
                                 "   INSERT INTO [CardTags] ([CardId],[TagId]) VALUES(@CardId,@TagId )";
            _queryManager.ExecuteNonQuery(query, new { CardId = cardId, @TagId = tagId });
        }
    }
}
