using System;

namespace ConsoleApplication
{
    using Microsoft.Build;
    using Microsoft.Build.Evaluation;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Host;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Text;
    using System.Linq;
    using System.IO;
    public class Program
    {

        public static void Main(string[] args)
        {
            try
            {
                // setting environment variables used by msbuild
                // location of msbuild.dll or msbuild.exe
                Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH",@"C:\Program Files\dotnet\sdk\1.0.0-preview3-004056\MSBuild.dll");
                // msbuild substitute $(MSBuildExtensionsPath) in *proj to this environment variable
                Environment.SetEnvironmentVariable("MSBuildExtensionsPath",@"C:\Program Files\dotnet\sdk\1.0.0-preview3-004056");
                var projects = new ProjectCollection();
                var proj = projects.LoadProject("dotnet-1.1.0-test.csproj");
                Console.WriteLine($"fullpath is {proj.FullPath}");
                var space = new AdhocWorkspace();
                var csproj = space.AddProject("test", LanguageNames.CSharp);
                foreach(var item in proj.GetItems("Compile"))
                {
                    Console.WriteLine($"item = {item.ItemType},{item.EvaluatedInclude}");
                    var filePath = Path.Combine(Path.GetDirectoryName(proj.FullPath),item.EvaluatedInclude);
                    var doc = csproj.AddDocument(item.EvaluatedInclude,File.ReadAllText(filePath));
                    csproj = doc.Project; 
                }
                Console.WriteLine($"{csproj.Documents.Count()}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            Console.WriteLine("Hello World!");
        }
    }
}
