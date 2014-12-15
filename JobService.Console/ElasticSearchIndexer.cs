using System.Collections.Generic;
using System.Linq;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;
using Nest;

namespace JobService
{
    
    public class ElasticSearchIndexer
    {
        private readonly IThreadService threadService;
        private readonly ElasticSearchDiscussionRepository elasticSearchDiscussionRepository;

        public ElasticSearchIndexer()
        {
            elasticSearchDiscussionRepository = JobServiceContainer.Current.GetInstance<ElasticSearchDiscussionRepository>();
            threadService = JobServiceContainer.Current.GetInstance<IThreadService>();
        }

        public void IndexAllData(string indexName, IElasticClient client)
        {
            //var p1 = new Discussion {Title = "Jahmai", Body = "OSullivan", Id = Guid.NewGuid()};
            //client.Index(p1, x => x.Index(indexName).Id(p1.Id.ToString()));

            IEnumerable<Discussion> discussions = threadService.FindAll();
            foreach (var discussion in discussions)
            {
                if (elasticSearchDiscussionRepository.GetById(discussion.Id) == null)
                    client.Index(discussion);
            }
        }

        public void CreateIndex(string indexName, IElasticClient client)
        {
            //if (!client.IndexExists(x => x.Index(indexName)).Exists)
                client.CreateIndex(indexName, s => s
                                     .NumberOfReplicas(1)
                                     .NumberOfShards(3)
                                      .Settings(settings => settings
                                         .Add("merge.policy.merge_factor", "10")
                                         .Add("search.slowlog.threshold.fetch.warn", "1s")
                                         .Add("mapping.allow_type_wrapper", true)
                                         .Add("analysis.filter.trigrams_filter.type", "nGram")
                                         .Add("analysis.filter.trigrams_filter.min_gram", "3")
                                         .Add("analysis.filter.trigrams_filter.max_gram", "3")
                                         .Add("analysis.analyzer.trigrams.type", "custom")
                                         .Add("analysis.analyzer.trigrams.tokenizer", "standard")
                                         .Add("analysis.analyzer.trigrams.filter.0", "lowercase")
                                         .Add("analysis.analyzer.trigrams.filter.1", "trigrams_filter")
                                       )
                                       .AddMapping<Discussion>(y => y
                                                                        .MapFromAttributes()
                                                                        .Properties(props => props
                                                                        .String(set => set
                                                                            .Name(p => p.Id)
                                                                            .IndexAnalyzer("keyword"))))
                                );
       }
    

        public void DeleteAllIndexes(IElasticClient client)
        {
            var allIndexes = client.GetAliases(x => x.Local()).Indices.Select(x => x.Key);
            foreach (var index in allIndexes)
            {
                if(index != "master")
                    client.DeleteIndex(x => x.Index(index));
            }
        }

        public IEnumerable<Discussion> Retrieve(string indexName, IElasticClient client)
        {
            ISearchResponse<Discussion> results = client.Search<Discussion>(s => s
                .Index(indexName)
                .From(0)
                .Size(10));
            return results.Documents;
            //.Fields(f => f.Id, f => f.Name)
            //.SortAscending(f => f.LOC)
            //.SortDescending(f => f.Name)
            //.Query(q =>
            //    q.Term(f => f.Title, "NEST", Boost: 2.0)
            //    || q.Match(mq => mq.OnField(f => f.Title).Query(userInput))
            //)

        }
    }
}
