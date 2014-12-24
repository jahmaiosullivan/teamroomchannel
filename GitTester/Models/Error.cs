using System.Collections.Generic;

namespace GitTester.Models
{
    public class Error
    {
        public Error()
        {
            Callers = new List<Error>();
        }

        public string MethodName { get; set; }
        public string File { get; set; }
        public int Line { get; set; }
        public IList<Error> Callers { get; set; }
    }
}
