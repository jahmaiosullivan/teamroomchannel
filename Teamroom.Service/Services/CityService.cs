using System.Collections.Generic;
using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;
using Teamroom.Business.Providers;

namespace HobbyClue.Business.Services
{
    public class CityService : BaseDapperService<City>, ICityService
    {
        private readonly ICityRepository _cityRepository;
        public CityService(ICityRepository repository, IUserProvider userProvider)
            : base(repository, userProvider)
        {
            _cityRepository = repository;
        }

        public City GetByName(string name, string region)
        {
            return _cityRepository.GetByName(name.Trim(), region.Trim());
        }


        public IEnumerable<City> GetByRegion(string region)
        {
            return _cityRepository.GetByRegion(region.Trim());
        }

    }
}
