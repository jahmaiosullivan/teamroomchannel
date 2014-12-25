using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;

namespace FindAllReferences
{
    class Program
    {
        static void Main(string[] args)
        {
            const string solutionPath = @"C:\Dev\TeamRoomChannel\TeamRoom.sln";
            const string className = "HobbyClue.Data.Repositories.EventRepository";
            const string methodName = "AssignPostToEvent";

            var testClasses = new List<string> {"Xunit.FactAttribute"};

            var msWorkspace = MSBuildWorkspace.Create();
            var solution = msWorkspace.OpenSolutionAsync(solutionPath).Result;
            var symbol = solution.Projects.Select(x => x.GetCompilationAsync().Result.GetTypeByMetadataName(className))
                                          .FirstOrDefault(x => x != null);

            if (symbol != null)
            {   var method = symbol.GetMembers(methodName).OfType<IMethodSymbol>().FirstOrDefault(x => x.DeclaredAccessibility == Accessibility.Public && x.Parameters.Count() == 2);
                if(method != null)
                {
                    var referenceLocations = SymbolFinder.FindCallersAsync(method, solution).Result
                                                         .Select(x => x.CallingSymbol)
                                                         .OfType<IMethodSymbol>()
                                                         .Where(x => x.GetAttributes().Any(t => testClasses.Contains(t.ToString()))).ToList();
                    
                }
            }
            Console.ReadLine();
        }


    }
}
