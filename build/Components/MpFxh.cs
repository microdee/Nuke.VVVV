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

public class MpFxhComp : VvvvComponent
{
    public override void Install(Build build)
    {
        Build.CreateSymlinkOrCopy(
            build.VvvvPath / "packs" / "mp.fxh",
            Build.RootDirectory / "src" / "mp.fxh"
        );
    }
}