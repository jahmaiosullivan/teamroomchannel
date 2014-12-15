using System;
using System.Collections.Generic;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public interface IVoteRepository : IDapperRepository<Vote> 
    {
        IDictionary<long, IList<Vote>> Get(IEnumerable<long> cardIds);
    }
}