using System;
using System.Diagnostics;
using System.IO;
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

public class MpVAudioComp : VvvvComponent
{
    [Solution("src/mp.vaudio/src/IpcBpmCounter/IpcBpmCounter.sln")]
    public Solution Sln;

    public override void Restore(Build build)
    {
        NuGetTasks.NuGetRestore(s => s
            .SetProcessWorkingDirectory(Sln.Path.Parent)
        );

        Build.CreateSymlinkOrCopy(Sln.Path.Parent / "vvvv", build.VvvvPath);
    }
    
    public override void Compile(Build build)
    {
        MSBuildTasks.MSBuild(s => s
            .SetSolutionFile(Sln)
            .SetRestore(true)
            .SetConfiguration(build.Configuration)
            .SetVerbosity(MSBuildVerbosity.Quiet)
        );
    }

    public override void Install(Build build)
    {
        Build.CreateSymlinkOrCopy(
            build.VvvvPath / "packs" / "mp.vaudio",
            Build.RootDirectory / "src" / "mp.vaudio"
        );
    }
}