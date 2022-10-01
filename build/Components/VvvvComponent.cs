using System.Linq;
using System.Reflection;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using Serilog;

public abstract class VvvvComponent
{
    public VvvvComponent()
    {
        GetType().GetFields()
            .Where(f => f.FieldType == typeof(Solution))
            .Where(f => f.GetCustomAttribute<SolutionAttribute>() != null)
            .ForEach(f =>
            {
                var attr = f.GetCustomAttribute<SolutionAttribute>();
                var sln = attr.GetValue(f, this) as Solution;
                f.SetValue(this, sln);
            });
    }

    public virtual void Restore(Build build) {}
    public virtual void Compile(Build build) {}
    public virtual void Install(Build build) {}
}