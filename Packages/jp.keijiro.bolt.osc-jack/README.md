Bolt OSC Jack
=============

![gif](https://i.imgur.com/j2ODNQe.gif)

**Bolt OSC Jack** is an add-on for Unity's [Bolt visual scripting system] that
adds custom units for handling [OSC (Open Sound Control)] messages.

[Bolt visual scripting system]:
  https://assetstore.unity.com/packages/tools/visual-scripting/bolt-163802

[OSC (Open Sound Control)]:
  https://en.wikipedia.org/wiki/Open_Sound_Control

It uses [OSC Jack] as a backend. If you're interested in using OSC without
visual scripting, check the [OSC Jack] project.

[OSC Jack]: https://github.com/keijiro/OscJack

Custom Units
------------

### OSC Input unit

![OSC Input Unit](https://i.imgur.com/t13Tq7I.png)

**OSC Input** is a unit for receiving OSC messages in a flow graph. You can
specify a UDP port number and an OSC address to which the unit listens.

There are variants for different data types:

- OscBangInput (trigger without data)
- OscIntInput
- OscFloatInput
- OscStringInput
- OscVector2Input
- OscVector3Input
- OscVector4Input

Note that it could trigger the event multiple times in a single frame.

### OSC Output unit

![OSC Output Unit](https://i.imgur.com/cCbkine.png)

**OSC Output** is a unit for sending OSC messages from a flow graph. You can
specify a destination IP address, a UDP port number, and an OSC address to
which the unit delivers messages.

<!--4567890123456789012345678901234567890123456789012345678901234567890123456-->

There are variants for different data types:

- OscBangOutput (trigger without data)
- OscIntOutput
- OscFloatOutput
- OscStringOutput
- OscVector2Output
- OscVector3Output
- OscVector4Output

How to try the sample project
-----------------------------

This repository doesn't contain the Bolt assets due to the license restriction.
You have to import [Bolt via Asset Store] manually.

[Bolt via Asset Store]:
  https://assetstore.unity.com/packages/tools/visual-scripting/bolt-163802

You can't use the "Install Bolt" tool due to compilation errors caused by
missing file references. You have to manually double-click the
`Bolt_1_4_X_NET4.unitypackage` file in the "Install Bolt" directory instead.

![unitypackage](https://i.imgur.com/cNxH458.png)

After importing the unitypackage file, it automatically opens the Bolt Setup
Wizard.

![wizard](https://i.imgur.com/wxlvRh7.png)

On the Assembly Options page, add `Bolt.Addons.OscJack.Runtime` to the assembly
list.

![assembly options](https://i.imgur.com/udy3MV8.png)

How to install the add-on to an existing project
------------------------------------------------

### Installing the package via Package Manager

This package uses the [scoped registry] feature to resolve package
dependencies. Please add the following sections to the manifest file
(Packages/manifest.json).

[scoped registry]: https://docs.unity3d.com/Manual/upm-scoped.html

To the `scopedRegistries` section:

```
{
  "name": "Keijiro",
  "url": "https://registry.npmjs.com",
  "scopes": [ "jp.keijiro" ]
}
```

To the `dependencies` section:

```
"jp.keijiro.bolt.osc-jack": "1.0.1"
```

After changes, the manifest file should look like below:

```
{
  "scopedRegistries": [
    {
      "name": "Keijiro",
      "url": "https://registry.npmjs.com",
      "scopes": [ "jp.keijiro" ]
    }
  ],
  "dependencies": {
    "jp.keijiro.bolt.osc-jack": "1.0.1",
    ...
```

### Adding the add-on assembly

Navigate to "Tools" > "Bolt" > "Unit Options Wizard".

![assembly options](https://i.imgur.com/udy3MV8.png)

Add `Bolt.Addons.OscJack.Runtime` to the assembly list. Then press "Next" and
"Generate."
