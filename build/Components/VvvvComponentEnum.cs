
using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using Nuke.Common.Git;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;
using System.Reflection;
using System.Collections;

[TypeConverter(typeof(TypeConverter<VvvvComponentEnum>))]
public class VvvvComponentEnum : Enumeration
{
    public static readonly VvvvComponentEnum DX11 = new() { Value = nameof(DX11), Component = new DX11Comp() };
    public static readonly VvvvComponentEnum MdStdl = new() { Value = nameof(MdStdl), Component = new MdStdlComp() };
    public static readonly VvvvComponentEnum MpDX = new() { Value = nameof(MpDX), Component = new MpDXComp() };
    public static readonly VvvvComponentEnum MpEssentials = new() { Value = nameof(MpEssentials), Component = new MpEssentialsComp() };
    public static readonly VvvvComponentEnum MpFxh = new() { Value = nameof(MpFxh), Component = new MpFxhComp() };
    public static readonly VvvvComponentEnum MpPddn = new() { Value = nameof(MpPddn), Component = new MpPddnComp() };
    public static readonly VvvvComponentEnum MpVAudio = new() { Value = nameof(MpVAudio), Component = new MpVAudioComp() };
    public static readonly VvvvComponentEnum Notui = new() { Value = nameof(Notui), Component = new NotuiComp() };
    public static readonly VvvvComponentEnum Notuiv = new() { Value = nameof(Notuiv), Component = new NotuivComp() };

    public static implicit operator string(VvvvComponentEnum configuration)
    {
        return configuration.Value;
    }

    public static IEnumerable<VvvvComponentEnum> All => typeof(VvvvComponentEnum)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.FieldType.Equals(typeof(VvvvComponentEnum)))
        .Select(f => f.GetValue(null) as VvvvComponentEnum);

    public VvvvComponent Component { init; get; }
}
