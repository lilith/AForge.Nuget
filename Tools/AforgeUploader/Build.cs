using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BuildTools;
using LitS3;
using System.Net;

namespace AForgeUpoader {
    public class Build :Interaction {
        FolderFinder f = new FolderFinder("nuget" );
        Devenv d = null;
        FsQuery q = null;
        VersionEditor v = null;
        NugetManager nuget = null;

        public Build() {
            d = new Devenv(Path.Combine(Path.Combine(f.ParentPath,"Sources"),"Build All.sln"));
            v = new VersionEditor(Path.Combine(f.ParentPath, "Sources\\Core\\Properties\\AssemblyInfo.cs"));
            nuget = new NugetManager(Path.Combine(f.ParentPath, "nuget"));
        }

        List<PackageDescriptor> packages = new List<PackageDescriptor>();

        [STAThread]
        public void Run() {
            MakeConsoleNicer();

            say("Project root: " + f.ParentPath);
            nl();

            say("AssemblyFileVersion: " + v.get("AssemblyFileVersion"));
            say("FileVersion: " + v.get("AssemblyVersion"));

            string fileVer = change("NugetVersion", v.get("AssemblyFileVersion").TrimEnd('.', '*'));


            //Get all the .nuspec packages on in the /nuget directory.
            IList<NPackageDescriptor> npackages =NPackageDescriptor.GetPackagesIn(Path.Combine(f.ParentPath,"nuget"));

            if (ask("Create or upload any NuGet packages?")) {

                say("For each nuget package, specify all operations to perform, then press enter. ");
                say("(c (create and overwrite), u (upload to nuget.org)");
                foreach (NPackageDescriptor desc in npackages) {

                    desc.Version = fileVer;
                    desc.OutputDirectory = Path.Combine(Path.Combine(f.ParentPath, "Releases", "nuget-packages"));
                    string opts = "";

                    ConsoleColor original = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (desc.PackageExists) say(Path.GetFileName(desc.PackagePath) + " exists");
                    if (desc.SymbolPackageExists) say(Path.GetFileName(desc.SymbolPackagePath) + " exists");
                    Console.ForegroundColor = original;

                    Console.Write(desc.BaseName + " (" + opts + "):");
                    opts = Console.ReadLine().Trim();

                    desc.Options = opts;
                    if (desc.Build) isMakingNugetPackage = true;

                }
            }

            //10 - Pacakge all nuget configurations
            foreach (NPackageDescriptor pd in npackages) {
                if (pd.Skip) continue;
                
                if (pd.Build) nuget.Pack(pd);

            }

            //2 - Upload all nuget configurations
            foreach (NPackageDescriptor pd in npackages) {
                if (pd.Skip || !pd.Upload) continue;
                nuget.Push(pd);

            }

            say("Everything is done.");
            
        }








    }
}
