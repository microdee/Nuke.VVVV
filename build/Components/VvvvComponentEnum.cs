
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
public abstract class VvvvComponentEnum : Enumeration
{
    public static readonly VvvvComponentEnum DX11 = new VvvvComponentEnum<DX11Comp>() { Value = nameof(DX11) };
    public static readonly VvvvComponentEnum MdStdl = new VvvvComponentEnum<MdStdlComp>() { Value = nameof(MdStdl) };
    public static readonly VvvvComponentEnum MpDX = new VvvvComponentEnum<MpDXComp>() { Value = nameof(MpDX) };
    public static readonly VvvvComponentEnum MpEssentials = new VvvvComponentEnum<MpEssentialsComp>() { Value = nameof(MpEssentials) };
    public static readonly VvvvComponentEnum MpFxh = new VvvvComponentEnum<MpFxhComp>() { Value = nameof(MpFxh) };
    public static readonly VvvvComponentEnum MpPddn = new VvvvComponentEnum<MpPddnComp>() { Value = nameof(MpPddn) };
    public static readonly VvvvComponentEnum MpVAudio = new VvvvComponentEnum<MpVAudioComp>() { Value = nameof(MpVAudio) };
    public static readonly VvvvComponentEnum Notui = new VvvvComponentEnum<NotuiComp>() { Value = nameof(Notui) };
    public static readonly VvvvComponentEnum Notuiv = new VvvvComponentEnum<NotuivComp>() { Value = nameof(Notuiv) };

    public static implicit operator string(VvvvComponentEnum configuration)
    {
        return configuration.Value;
    }

    public static IEnumerable<VvvvComponentEnum> All => typeof(VvvvComponentEnum)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.FieldType.Equals(typeof(VvvvComponentEnum)))
        .Select(f => f.GetValue(null) as VvvvComponentEnum);

    public abstract VvvvComponent Component { get; }
}

public class VvvvComponentEnum<T> : VvvvComponentEnum where T : VvvvComponent, new()
{
    private VvvvComponent _component = null;
    public override VvvvComponent Component => _component ??= new T();
}
