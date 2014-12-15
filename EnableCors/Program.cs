using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table;

namespace EnableCors
{
    class Program
    {
        static void Main(string[] args)
        {
            var _storageAccountName = "critikdev";
            var _storageAccountKey = "3hA0Bz0Vqtf7tkMltr8DPyPAcnuaIP3hZKU3QTOaCStTnEP+qJpYwn7a3UHmHDoSuAOmf77lsN+KN/b1SsEzQw==";

            var accountAndKey = new StorageCredentials(_storageAccountName, _storageAccountKey);
            var storageaccount = new CloudStorageAccount(accountAndKey, true);
            //var storageClient = storageaccount.CreateCloudBlobClient();
            var storageClient = storageaccount.CreateCloudTableClient();

            //Console.WriteLine("Storage Account: " + storageaccount.BlobEndpoint);
            Console.WriteLine("Storage Account: " + storageaccount.TableEndpoint);
            var newProperties = storageClient.GetServiceProperties();

            //Set service to new version:
            //newProperties.DefaultServiceVersion = "2013-08-15"; //"2012-02-12"; // "2011-08-18"; // null; -- Only for blob storage
            storageClient.SetServiceProperties(newProperties);

            var addRule = true;
            if (addRule)
            {

                //Add a wide open rule to allow uploads:
                var ruleWideOpenWriter = new CorsRule()
                {
                    AllowedHeaders = { "*" },
                    AllowedOrigins = { "*" },
                    AllowedMethods =
                        CorsHttpMethods.Options |
                        CorsHttpMethods.Get |
                        CorsHttpMethods.Post |
                        CorsHttpMethods.Put |
                        CorsHttpMethods.Merge,
                    ExposedHeaders = { "*" },
                    MaxAgeInSeconds = (int)TimeSpan.FromDays(5).TotalSeconds
                };
                newProperties.Cors.CorsRules.Clear();
                newProperties.Cors.CorsRules.Add(ruleWideOpenWriter);
                storageClient.SetServiceProperties(newProperties);

                Console.WriteLine("New Properties:");
                CurrentProperties(storageClient);
            }

            Console.ReadKey();
        }

        private static ServiceProperties CurrentProperties(CloudBlobClient blobClient)
        {
            var currentProperties = blobClient.GetServiceProperties();
            if (currentProperties != null)
            {
                if (currentProperties.Cors != null)
                {
                    Console.WriteLine("Cors.CorsRules.Count          : " + currentProperties.Cors.CorsRules.Count);
                    for (int index = 0; index < currentProperties.Cors.CorsRules.Count; index++)
                    {
                        var corsRule = currentProperties.Cors.CorsRules[index];
                        Console.WriteLine("corsRule[index]              : " + index);
                        foreach (var allowedHeader in corsRule.AllowedHeaders)
                        {
                            Console.WriteLine("corsRule.AllowedHeaders      : " + allowedHeader);
                        }
                        Console.WriteLine("corsRule.AllowedMethods      : " + corsRule.AllowedMethods);

                        foreach (var allowedOrigins in corsRule.AllowedOrigins)
                        {
                            Console.WriteLine("corsRule.AllowedOrigins      : " + allowedOrigins);
                        }
                        foreach (var exposedHeaders in corsRule.ExposedHeaders)
                        {
                            Console.WriteLine("corsRule.ExposedHeaders      : " + exposedHeaders);
                        }
                        Console.WriteLine("corsRule.MaxAgeInSeconds     : " + corsRule.MaxAgeInSeconds);
                    }
                }
                Console.WriteLine("DefaultServiceVersion         : " + currentProperties.DefaultServiceVersion);
                Console.WriteLine("HourMetrics.MetricsLevel      : " + currentProperties.HourMetrics.MetricsLevel);
                Console.WriteLine("HourMetrics.RetentionDays     : " + currentProperties.HourMetrics.RetentionDays);
                Console.WriteLine("HourMetrics.Version           : " + currentProperties.HourMetrics.Version);
                Console.WriteLine("Logging.LoggingOperations     : " + currentProperties.Logging.LoggingOperations);
                Console.WriteLine("Logging.RetentionDays         : " + currentProperties.Logging.RetentionDays);
                Console.WriteLine("Logging.Version               : " + currentProperties.Logging.Version);
                Console.WriteLine("MinuteMetrics.MetricsLevel    : " + currentProperties.MinuteMetrics.MetricsLevel);
                Console.WriteLine("MinuteMetrics.RetentionDays   : " + currentProperties.MinuteMetrics.RetentionDays);
                Console.WriteLine("MinuteMetrics.Version         : " + currentProperties.MinuteMetrics.Version);
            }
            return currentProperties;
        }

        private static ServiceProperties CurrentProperties(CloudTableClient tableClient)
        {
            var currentProperties = tableClient.GetServiceProperties();
            if (currentProperties != null)
            {
                if (currentProperties.Cors != null)
                {
                    Console.WriteLine("Cors.CorsRules.Count          : " + currentProperties.Cors.CorsRules.Count);
                    for (int index = 0; index < currentProperties.Cors.CorsRules.Count; index++)
                    {
                        var corsRule = currentProperties.Cors.CorsRules[index];
                        Console.WriteLine("corsRule[index]              : " + index);
                        foreach (var allowedHeader in corsRule.AllowedHeaders)
                        {
                            Console.WriteLine("corsRule.AllowedHeaders      : " + allowedHeader);
                        }
                        Console.WriteLine("corsRule.AllowedMethods      : " + corsRule.AllowedMethods);

                        foreach (var allowedOrigins in corsRule.AllowedOrigins)
                        {
                            Console.WriteLine("corsRule.AllowedOrigins      : " + allowedOrigins);
                        }
                        foreach (var exposedHeaders in corsRule.ExposedHeaders)
                        {
                            Console.WriteLine("corsRule.ExposedHeaders      : " + exposedHeaders);
                        }
                        Console.WriteLine("corsRule.MaxAgeInSeconds     : " + corsRule.MaxAgeInSeconds);
                    }
                }
                Console.WriteLine("DefaultServiceVersion         : " + currentProperties.DefaultServiceVersion);
                Console.WriteLine("HourMetrics.MetricsLevel      : " + currentProperties.HourMetrics.MetricsLevel);
                Console.WriteLine("HourMetrics.RetentionDays     : " + currentProperties.HourMetrics.RetentionDays);
                Console.WriteLine("HourMetrics.Version           : " + currentProperties.HourMetrics.Version);
                Console.WriteLine("Logging.LoggingOperations     : " + currentProperties.Logging.LoggingOperations);
                Console.WriteLine("Logging.RetentionDays         : " + currentProperties.Logging.RetentionDays);
                Console.WriteLine("Logging.Version               : " + currentProperties.Logging.Version);
                Console.WriteLine("MinuteMetrics.MetricsLevel    : " + currentProperties.MinuteMetrics.MetricsLevel);
                Console.WriteLine("MinuteMetrics.RetentionDays   : " + currentProperties.MinuteMetrics.RetentionDays);
                Console.WriteLine("MinuteMetrics.Version         : " + currentProperties.MinuteMetrics.Version);
            }
            return currentProperties;
        }

    }
}
