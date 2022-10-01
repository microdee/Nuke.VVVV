using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

public class DX11Comp : VvvvComponent
{
    [Solution("src/dx11/vvvv-dx11.sln")]
    public Solution Sln;
    
    public override void Compile(Build build)
    {
        MSBuildTasks.MSBuild(s => s
            .SetSolutionFile(Sln)
            .SetConfiguration(build.Configuration)
            .SetTargetPlatform(MSBuildTargetPlatform.x64)
            .SetVerbosity(MSBuildVerbosity.Quiet)
            .SetRestore(true)
        );
    }

    public override void Install(Build build)
    {
        FileSystemTasks.CopyDirectoryRecursively(
            Sln.Path.Parent / "girlpower",
            build.VvvvPath / "packs" / "dx11",
            DirectoryExistsPolicy.Merge,
            FileExistsPolicy.Overwrite
        );
        FileSystemTasks.CopyDirectoryRecursively(
            Sln.Path.Parent / "Deploy" / build.Configuration.ToString() / "x64" / "packs" / "dx11",
            build.VvvvPath / "packs" / "dx11",
            DirectoryExistsPolicy.Merge,
            FileExistsPolicy.Overwrite
        );
        FileSystemTasks.CopyDirectoryRecursively(
            Build.RootDirectory / "src" / "dx11_aux",
            build.VvvvPath / "packs" / "dx11",
            DirectoryExistsPolicy.Merge,
            FileExistsPolicy.Overwrite
        );
    }
}