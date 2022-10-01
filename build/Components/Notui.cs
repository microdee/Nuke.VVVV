using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Build.Construction;
using Microsoft.Build.Tasks;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.IO.PathConstruction;

public class NotuiComp : VvvvComponent
{

    [Solution("src/Notui/Notui.sln")]
    public Solution Sln;

    public override void Restore(Build build)
    {
        NuGetTasks.NuGetRestore(s => s
            .SetProcessWorkingDirectory(Sln.Path.Parent)
        );
    }
    
    public override void Compile(Build build)
    {
        MSBuildTasks.MSBuild(s => s
            .SetSolutionFile(Sln)
            .SetRestore(true)
            .SetConfiguration(build.Configuration)
            .SetVerbosity(MSBuildVerbosity.Quiet)
            .SetTargetPlatform(MSBuildTargetPlatform.MSIL)
        );
    }

    public override void Install(Build build)
    {
        FileSystemTasks.CopyDirectoryRecursively(
            Sln.GetProject("Notui").Path.Parent / "bin" / build.Configuration,
            build.VvvvPath / "lib" / "core",
            DirectoryExistsPolicy.Merge,
            FileExistsPolicy.Overwrite
        );
    }
}