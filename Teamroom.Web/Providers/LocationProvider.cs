using HobbyClue.Business.Services;
using HobbyClue.Data.Models;

namespace HobbyClue.Web.Providers
{
    public class LocationProvider : ILocationProvider
    {
        private readonly ICityService cityService;

        public LocationProvider(ICityService cityService)
        {
            this.cityService = cityService;
        }

        public City GetCurrent(string cityName, string region)
        {
            City currentCity = null;
            if (!string.IsNullOrWhiteSpace(cityName))
            {
                currentCity = cityService.GetByName(cityName, region);
            }
            
            return currentCity;
        }
    }
}