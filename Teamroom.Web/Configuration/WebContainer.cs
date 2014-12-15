using HobbyClue.Web.Configuration;
using StructureMap;
using Teamroom.Business.Configuration;

namespace Teamroom.Web.Configuration
{
    public class WebContainer
    {
        public static IContainer Current = GetWebContainer();

        static IContainer GetWebContainer()
        {
            var container = new Container(
            
                x => { 
                    x.AddRegistry<CoreRegistry>();
                    x.AddRegistry<WebRegistry>();
                });

            return container;
        }
    }
}