using System;
using System.Collections.Generic;
using HobbyClue.Data.Models;

namespace HobbyClue.Business.Services
{
    public interface ITagService : IBaseService<Tag> 
    {
        IDictionary<long, IList<Tag>> Get(IEnumerable<long> cardIds = null);
        IEnumerable<Tag> GetForUser(Guid userId);
        void AddUserTag(Guid userId, long tagId);
        void RemoveUserTag(Guid userId, long tagId);
        Tag GetForUser(Guid userId, long tagId);
        IEnumerable<Tag> FindAllInUse();
        IEnumerable<Tag> GetLastUsed(int number, int pageNum = 1);
        IEnumerable<Tag> GetForCity(long cityId);
        IEnumerable<Tag> GetTopLevelTags();
        IEnumerable<Tag> GetChildTags(long tagId);
        IEnumerable<Tag> GetForHobby(long hobbyId);
    }
}