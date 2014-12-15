using System;
using System.Collections.Generic;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public interface ITagRepository : IDapperRepository<Tag>
    {
         IDictionary<long, IList<Tag>> Get(IEnumerable<long> cardIds);
        IEnumerable<Tag> GetTopLevelTags();
        IEnumerable<Tag> GetCityDefaultTags(long cityId);
        Tag GetForUser(Guid userId, long tagId);
        void AddUserTag(Guid userId, long tagId);
        void DeleteUserTag(Guid userId, long tagId);
        void AddCardTag(long tagId, long cardId);
        IEnumerable<Tag> GetUsedByCards();
        IEnumerable<Tag> GetLastUsed(int number, int pageNum);
        IEnumerable<Tag> GetForUser(Guid userId);
        IEnumerable<Tag> GetChildTags(long tagId);
        IEnumerable<Tag> GetForHobby(long hobbyId);
    }
}