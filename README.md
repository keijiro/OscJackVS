OSCJackVS
=========

![gif](https://i.imgur.com/j2ODNQe.gif)

**OscJackVS** is an extension for visual scripting in Unity that adds custom
units for handling [OSC (Open Sound Control)] messages.

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

How to install
--------------

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
"jp.keijiro.osc-jack.visualscripting": "1.0.2"
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
    "jp.keijiro.osc-jack.visualscripting": "1.0.2",
    ...
```
