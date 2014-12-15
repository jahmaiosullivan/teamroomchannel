using System.Collections.Generic;
using HobbyClue.Data.Models;

namespace HobbyClue.Business.Services
{
    public interface ICityService : IBaseService<City>
    {
        City GetByName(string name, string region);
        IEnumerable<City> GetByRegion(string region);
    }
}