using System;
using System.Linq;
using System.IO;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;
using Serilog;
using Konsole;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using System.Text.RegularExpressions;
using Nuke.Common.Tools.Git;

public class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Init);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    public readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter]
    public readonly VvvvComponentEnum[] Component = VvvvComponentEnum.All.ToArray();
    
    public AbsolutePath VvvvPath => RootDirectory / "vvvv";

    Target Init => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            var nukeTargets = RootDirectory / "build";
            var archive = RootDirectory / "engine.7z";

            Tool sevenZip = ToolResolver.GetLocalTool(nukeTargets / "7zr.exe");
            ProgressBar extractProgress = null;
            Log.Information("Extracting Engine.7z");
            sevenZip(
                arguments: "x -aoa -r \"*.7z\" -o* -bsp1 -bse1",
                workingDirectory: RootDirectory,
                logInvocation: true,
                customLogger: (ot, text) =>
                {
                    var regexMatches = text.MatchGroup(@"(?<PERCENT>\d+?)%\s+?(?<NUM>\d+)(?<REST>.*)");
                    int percent, num = 0;
                    bool valid = int.TryParse(regexMatches?["PERCENT"]?.Value ?? "", out percent)
                        && int.TryParse(regexMatches?["NUM"]?.Value ?? "", out num);
                    if (valid)
                    {
                        extractProgress ??= new ProgressBar(PbStyle.DoubleLine, 100);
                        var info = regexMatches?["REST"]?.Value ?? "";
                        extractProgress.Refresh(percent, num.ToString() + " " + info);
                    }
                    else if(!string.IsNullOrWhiteSpace(text))
                    {
                        Log.Debug(text);
                    }
                }
            );
        });

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            GitTasks.Git("clean -xdf", RootDirectory);
            GitTasks.Git("submodule foreach git clean -xdf", RootDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            Component.ForEach(c => {
                Log.Information("Restoring {0}", c);
                c.Component.Restore(this);
            });
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            Component.ForEach(c => {
                Log.Information("Compiling {0}", c);
                c.Component.Compile(this);
            });
        });

    Target Install => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            Component.ForEach(c => {
                Log.Information("Installing {0}", c);
                c.Component.Install(this);
            });
        });

    public static void CreateSymlinkOrCopy(AbsolutePath createAt, AbsolutePath from)
    {
        Log.Information("Creating symlink @ {0} from {1}", createAt, from);
        if (createAt.Exists())
        {
            Log.Information("Target already exists");
            return;
        }
        try
        {
            Directory.CreateSymbolicLink(createAt, from);
        }
        catch (Exception e)
        {
            Log.Warning("Couldn't create symlink, copying instead. Reason: {0}", e.Message);
            if (from.DirectoryExists()) CopyDirectoryRecursively(from, createAt);
            else if(from.FileExists()) CopyFile(from, createAt);
        }
    }
}

public static class StringUtils
{
    public static GroupCollection MatchGroup(this string text, string pattern, RegexOptions options = RegexOptions.Multiline | RegexOptions.CultureInvariant)
    {
        return Regex.Match(text, pattern, options)?.Groups;
    }

    public static GroupCollection Fetch(this GroupCollection groups, string capturename, out string result)
    {
        result = groups[capturename]?.Value;
        return groups;
    }
}
