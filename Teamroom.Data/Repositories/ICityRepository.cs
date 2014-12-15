using System.Collections.Generic;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public interface ICityRepository : IDapperRepository<City>
    {
        City GetByName(string name, string region);
        IEnumerable<City> GetByRegion(string region);
    }
}