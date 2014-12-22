using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GitTester
{
    public class GitCommandLine
    {
        private static string RunProcess(string command)
        {
            // Start the child process.
            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "git.exe",
                    Arguments = command
                }
            };
            // Redirect the output stream of the child process.
            p.Start();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        public List<GitCommit> LoadHistory(string repoPath, string filePath)
        {
            var output = RunProcess(string.Format(@" --git-dir={0}/.git --work-tree={1} log --follow {2}", repoPath.Replace("\\", "/"), repoPath.Replace("\\", "/"), filePath));
            return Populate(output);
        }

        private static List<GitCommit> Populate(string output)
        {
            GitCommit commit = null;
            var commits = new List<GitCommit>();
            bool processingMessage = false;
            using (var strReader = new StringReader(output))
            {
                do
                {
                    var line = strReader.ReadLine();

                    if( line.StartsWith("commit ") )
                    {
                        if (commit != null)
                            commits.Add(commit);
                        commit = new GitCommit();
                        commit.Sha = line.Split(' ')[1];
                    }

                    if ( StartsWithHeader(line) )
                    {
                        var header = line.Split(':')[0];
                        var val = string.Join(":",line.Split(':').Skip(1)).Trim();

                        // headers
                        commit.Headers.Add(header, val);
                    }

                    if (string.IsNullOrEmpty(line) )
                    {
                        // commit message divider
                        processingMessage = !processingMessage;
                    }

                    if (line.Length > 0 && line[0] == '\t')
                    { 
                        // commit message.
                        commit.Message += line;
                    }

                    if (line.Length > 1 && Char.IsLetter(line[0]) && line[1] == '\t')
                    {
                        var status = line.Split('\t')[0];
                        var file = line.Split('\t')[1];
                        commit.Files.Add(new GitFileStatus() { Status = status, File = file } );
                    }
                }
                while (strReader.Peek() != -1);
            }
            if (commit != null)
                commits.Add(commit);

            return commits;
        }

        private static bool StartsWithHeader(string line)
        {
            if (line.Length > 0 && char.IsLetter(line[0]))
            {
                var seq = line.SkipWhile(ch => Char.IsLetter(ch) && ch != ':');
                return seq.FirstOrDefault() == ':';
            }
            return false;
        }
}

    public class GitCommit
    {
        public GitCommit()
        {
            Headers = new Dictionary<string, string>();
            Files = new List<GitFileStatus>();
            Message = "";
        }

        public Dictionary<string, string> Headers { get; set; }
        public string Sha { get; set; }
        public string Message { get; set; }
        public List<GitFileStatus> Files { get; set; }
    }

    public class GitFileStatus
    {
        public string Status { get; set; }
        public string File { get; set; }
    }
}
