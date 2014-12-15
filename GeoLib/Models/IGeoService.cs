using GeoLib.Models;

namespace GeoLib
{
    public interface IGeoService
    {
        GeoPlace Get(string ipaddress);
    }
}