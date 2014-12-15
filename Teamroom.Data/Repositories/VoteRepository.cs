using System;
using System.Collections.Generic;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public class VoteRepository : BaseDapperRepository<Vote>, IVoteRepository
    {
        public VoteRepository(IQueryManager queryManager) 
                    : base(queryManager)
        {
        }

        public IDictionary<long, IList<Vote>> Get(IEnumerable<long> cardIds)
        {
            var result = new Dictionary<long, IList<Vote>>();
            var clueTags = _queryManager.ExecuteQuery<Vote>("GetVotesForClues", new { @CardIds = cardIds });
            foreach (var t in clueTags)
            {
                if(result.ContainsKey(t.CardId))
                    result[t.CardId].Add(t);
                else
                    result.Add(t.CardId, new List<Vote> { t });
            }
            return result;
        }
    }
}
