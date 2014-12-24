using System;
using System.Linq;
using System.Text.RegularExpressions;
using GitTester.Models;

namespace GitTester
{
    public class ErrorReaderService
    {
        private static readonly Regex ErrorRegex = new Regex(@"^at\s+(?<methodname>.+?(?=\s+in\s+)\s)+in\s+(?<filelocation>.+?(?=line))line\s+(?<linenum>\d+)$");
     
        public Error GetError(string exceptionString)
        {
            var result = new Error();
            var lines = Regex.Split(exceptionString, @"(?=at\s+)").Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(x.Trim())).Select(x => x.Trim()).ToList();
            for (var i = 0; i < lines.Count(); i++)
            {
                var src = ErrorRegex.Matches(lines[i]);
                var errorGenerated = Generate(src);
                if(i == 0)
                    result = errorGenerated;
                else
                {
                    result.Callers.Add(errorGenerated);
                }
            }
            return result;
        }

        private static Error Generate(MatchCollection match)
        {
            var result = new Error
            {
                MethodName = match[0].Groups["methodname"].ToString(),
                File = match[0].Groups["filelocation"].ToString(),
                Line = Convert.ToInt32(match[0].Groups["linenum"].ToString())
            };
            return result;
        }
    }
}
