using HobbyClue.Business.Configuration;
using HobbyClue.Data.Repositories;
using StructureMap;

namespace JobService
{
    public class JobServiceContainer
    {
        public static IContainer Current = GetJobContainer();

        static IContainer GetJobContainer()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CoreRegistry>();
                x.For<IDiscussionRepository>().Use<DiscussionRepository>();
                x.For<ElasticSearchDiscussionRepository>().Use<ElasticSearchDiscussionRepository>();
            });
            return container;
        }
    }
}
