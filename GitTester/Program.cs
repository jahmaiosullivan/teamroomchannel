using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using GitTester.Models;
using HobbyClue.Business.Services;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Repositories;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace GitTester
{
    public class Program
    {
        private const string RepoPath = @"C:\Dev\TeamRoomChannel";

        static void Main(string[] args)
        {
            //Clone();
           //GitLog("Jahmai");
            //GenerateError();
            var error = ReadError();
            Console.WriteLine("{0}: Started", DateTime.Now.ToString("MM-d-yyyy h:mm:ss"));

            IEnumerable<Commit> commits;
            if(error != null)
            {
                try
                {
                    var index = error.File.IndexOf(RepoPath, StringComparison.OrdinalIgnoreCase);
                    var cleanPath = (index < 0) ? error.File : error.File.Remove(index, RepoPath.Length);

                    commits = GitFileHistory(RepoPath, cleanPath);
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("{0}: Ended", DateTime.Now.ToString("MM-d-yyyy h:mm:ss"));
            Console.ReadLine();
        }


        private static Error ReadError()
        {
            var errorReader = new ErrorReaderService();
            const string stackTrace = @"at HobbyClue.Data.Dapper.SqlQueryManager.GetConnection() in c:\Dev\TeamRoomChannel\Teamroom.Data\Dapper\SqlQueryManager.cs:line 26 " +
                                      @"at HobbyClue.Data.Dapper.SqlQueryManager.ExecuteSql[T](String sql, Object params, IDictionary`2 replaceFields, Nullable`1 commandType, Nullable`1 commandTimeOut) in c:\Dev\TeamRoomChannel\Teamroom.Data\Dapper\SqlQueryManager.cs:line 59 " +
                                      @"at HobbyClue.Data.Dapper.BaseDapperRepository`1.Find(String query) in c:\Dev\TeamRoomChannel\Teamroom.Data\Dapper\BaseDapperRepository.cs:line 64 " +
                                      @"at HobbyClue.Business.Services.BaseService`1.FindAll() in c:\Dev\TeamRoomChannel\Teamroom.Service\Services\BaseService.cs:line 74 " +
                                      @"at GitTester.Program.GenerateError() in c:\Dev\TeamRoomChannel\GitTester\Program.cs:line 26 ";

            var er = errorReader.GetError(stackTrace);
            return er;
        }
        

        private static void GenerateError()
        {
            try
            {
                var service = new CompanyService(new CompanyRepository(new SqlQueryManager()), null);
                service.FindAll();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        static void Clone()
        {
            var repo = Repository.Clone(@"https://github.com/jahmaiosullivan/teamroomchannel.git", @"C:\Dev\Temp\Clones",
                new CloneOptions
                {
                    Checkout = true,
                    CredentialsProvider = LoginHandler,
                    IsBare = false,
                    OnCheckoutProgress = OnCheckoutProgress
                });
        }

        private static void OnCheckoutProgress(string path, int completedSteps, int totalSteps)
        {
            Console.WriteLine(((completedSteps / totalSteps) * 100).ToString());
        }

        private static Credentials CredentialsProvider(string url, string usernameFromUrl, SupportedCredentialTypes types)
        {
            return new UsernamePasswordCredentials
            {
                Username = "jahmaiosullivan",
                Password = "Star2014"
            };
        }

        private static Credentials LoginHandler(string url, string usernameFromUrl, SupportedCredentialTypes types)
        {
            throw new NotImplementedException();
        }

        static void GitLog(string commitAuthor, int commitCount = 15)
        {
            using (var repo = new Repository(@"C:\Dev\HobbyClue"))
            {
                const string rfc2822Format = "ddd dd MMM HH:mm:ss yyyy K";
                IEnumerable<Commit> selectedCommits = repo.Commits.Where(x => x.Author.Name.StartsWith(commitAuthor)).Take(commitCount);
                foreach (var c in selectedCommits)
                {
                    Console.WriteLine("commit {0}", c.Id);

                    if (c.Parents.Count() > 1)
                    {
                        Console.WriteLine("Merge: {0}",
                            string.Join(" ", c.Parents.Select(p => p.Id.Sha.Substring(0, 7)).ToArray()));
                    }

                    Console.WriteLine("Author: {0} <{1}>", c.Author.Name, c.Author.Email);
                    Console.WriteLine("Date:   {0}", c.Author.When.ToString(rfc2822Format, CultureInfo.InvariantCulture));
                    Console.WriteLine();
                    Console.WriteLine(c.Message);
                    Console.WriteLine();
                }
            }
        }

        static IEnumerable<Commit> GitCommits(string repoPath, List<string> hashes)
        {
            List<Commit> selectedCommits = new List<Commit>();
            using (var repo = new Repository(repoPath))
            {
                const string rfc2822Format = "ddd dd MMM HH:mm:ss yyyy K";
                foreach (var id in hashes)
                {
                    var c = repo.Lookup<Commit>(id);
                    selectedCommits.Add(c);

                    Console.WriteLine("commit {0}", c.Id);

                    if (c.Parents.Count() > 1)
                    {
                        Console.WriteLine("Merge: {0}",
                            string.Join(" ", c.Parents.Select(p => p.Id.Sha.Substring(0, 7)).ToArray()));
                    }

                    Console.WriteLine("Author: {0} <{1}>", c.Author.Name, c.Author.Email);
                    Console.WriteLine("Date:   {0}", c.Author.When.ToString(rfc2822Format, CultureInfo.InvariantCulture));
                    Console.WriteLine();
                    Console.WriteLine(c.Message);
                    Console.WriteLine();
                }
            }
            return selectedCommits;
        }


        static IEnumerable<Commit> GitFileHistory(string repoPath, string fileName)
        {
            IEnumerable<Commit> selectedCommits;
            using (var repo = new Repository(repoPath))
            {
                selectedCommits = repo.History(fileName);
            }
            return selectedCommits;
        }
    }
}
