using System;
using System.Configuration;
using AutoMapper;
using GeoLib;
using HobbyClue.Data;
using HobbyClue.Data.Azure;
using HobbyClue.Data.Azure.Base;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;
using Microsoft.WindowsAzure.Storage;
using Nest;

namespace Teamroom.Business.Configuration
{
    public class CoreRegistry : StructureMap.Configuration.DSL.Registry
    {
        public CoreRegistry()
        {
            Scan(s =>
            {
                s.TheCallingAssembly();
                s.AssembliesFromApplicationBaseDirectory(f => (f.FullName.Contains("Teamroom")));
                s.WithDefaultConventions();
            });

            if (ConfigurationManager.ConnectionStrings["StorageConnectionString"] != null)
                For<ICloudClientWrapper>().Use<CloudClientWrapper>().Ctor<CloudStorageAccount>("storageAccount").Is(CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString));
            For<IRepository<Tag>>().Use<TagRepository>();
            For<IImageRepository>().Use<ImageBlobRepository>();
            For<IMappingEngine>().Use(x => Mapper.Engine);
            For<ITableStorageRepository<ImageInfo>>().Use<ImageInfoRepository>();
            For<IQueryManager>().Use<SqlQueryManager>();
            For<IGeoService>().Singleton().Use<GeoService>().Ctor<string>("geosegFilePath").Is(ConfigurationManager.AppSettings["GeoSegFile"]);
            
            var setting = new ConnectionSettings(new Uri("http://localhost:9200"));
            For<IElasticClient>().Singleton().Use(new ElasticClient(setting, null, null, null));                      

        }
    }
}
