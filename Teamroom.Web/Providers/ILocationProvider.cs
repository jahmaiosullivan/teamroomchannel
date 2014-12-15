using HobbyClue.Data.Models;

namespace HobbyClue.Web.Providers
{
    public interface ILocationProvider
    {
        City GetCurrent(string cityName, string region);
    }
}