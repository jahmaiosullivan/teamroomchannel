using System;
using System.Collections.Generic;
using System.Linq;
using HobbyClue.Common.Models;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public class CityRepository : BaseDapperRepository<City>, ICityRepository
    {
        public CityRepository(IQueryManager queryManager) : base(queryManager)
        {
        }

        public City GetByName(string name, string region)
        {
            var sql = BaseQuery + " where Name = @name and Region = @region";
            var cities = _queryManager.ExecuteSql<City>(sql, new {@name = name, @region = region});
            return cities.EmptyListIfNull().FirstOrDefault();
        }

        public IEnumerable<City> GetByRegion(string region)
        {
            var sql = BaseQuery + " where Region = @region";
            var cities = _queryManager.ExecuteSql<City>(sql, new { @region = region });
            return cities.EmptyListIfNull();
        }


       
    }
}
