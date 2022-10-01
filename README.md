# Nuke.VVVV

A workflow for using old-school vvvv with source-code, and setting up a project for an unsuspecting computer. It's using [Nuke](https://nuke.build), an execution graph engine for orchestrating building applications.

To fully initialize a project specific VVVV setup use this:

```
nuke init
nuke install
```

The build project works with "components" which can be built and installed for VVVV. These are usually VVVV plugins. Each component can control how it's being restored, built and installed. "Installing" a component making it available for VVVV. If no compoent is specified for nuke all of them are handled. For building/installing individual components use

```
nuke install --component dx11 mpdx
```

To add your component, create a class inheriting from `VvvvComponent` and add that to the `VvvvComponentEnum` class as a static value.

```CSharp
public class MyPluginComp : VvvvComponent { ... }
```

```CSharp
public class VvvvComponentEnum
{
    ...
    public static readonly VvvvComponentEnum MyPlugin = new() {
        Value = nameof(MyPlugin),
        Component = new MyPluginComp()
    };
    ...
}
```