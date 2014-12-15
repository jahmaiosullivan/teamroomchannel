using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobService;
using log4net;
using Nest;

namespace ElasticSearch.Indexer
{
    public class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            var log = LogManager.GetLogger(typeof(Program));
            
            log.Info("Starting Indexer");
            DateTime start = DateTime.Now;

            log.Info("Database ConnectionString : " + ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            log.Info("ElasticSearch EndPoint : " + ConfigurationManager.AppSettings["ElasticSearch.EndPoint"]);

            IndexAllData();

            log.Info("DONE");
            Console.ReadLine();
        }

        static void IndexAllData()
        {
           var client = JobServiceContainer.Current.GetInstance<IElasticClient>();

           var indexer = new ElasticSearchIndexer();
           //var allThings = indexer.Retrieve("master", client);
            //indexer.DeleteAllIndexes(client);
           //client.DeleteIndexAsync(x => x.Index("discussions"));
           //indexer.CreateIndex("discussions", client);
           indexer.IndexAllData("discussions", client);
            //DeleteAllIndexes(client);
            //CreateForumIndexes(client);
            //client.Index(new IndexRequest<object>())

        }

        static void CreateIndex(string indexName, IElasticClient client)
        {
            if(!client.IndexExists(x => x.Index(indexName)).Exists)
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
                                   .AddMapping<Object>(mapping => mapping
                                      .Type("data")
                                      .AllField(af => af.Enabled())
                                      .Properties(prop => prop
                                         .String(sprop => sprop
                                           .Name("text")
                                          )
                                       )
                                   )
   );
        }

        static void DeleteAllIndexes(IElasticClient client)
        {
            var allIndexes = client.GetAliases(x => x.Local()).Indices.Select(x => x.Key);
            foreach (var index in allIndexes)
            {
                client.DeleteIndexAsync(x => x.Index(index));
            }
        }
    }
}
