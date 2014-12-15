using System;
using System.Collections.Generic;
using HobbyClue.Data.Models;

namespace HobbyClue.Business.Services
{
    public interface IVoteService : IBaseService<Vote>
    {
        IDictionary<long, IList<Vote>> Get(IEnumerable<long> cardIds = null);
        Vote GetForUser(long cardId, Guid userId);
    }
}