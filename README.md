OSCJackVS
=========

![gif](https://i.imgur.com/j2ODNQe.gif)

**OscJackVS** is an extension for visual scripting in [Unity] that adds custom
nodes for handling [OSC (Open Sound Control)] messages.

[OSC (Open Sound Control)]: http://opensoundcontrol.org/
[Unity]: https://unity3d.com/

It uses [OSC Jack] as a backend. If you're interested in using OSC without
visual scripting, please check the [OSC Jack] project.

[OSC Jack]: https://github.com/keijiro/OscJack

System Requirements
-------------------

- Unity 2021.3 or later

OSC Jack requires `System.Net.Sockets` supported on most platforms but a few
network-restrictive platforms like WebGL.

How To Install
--------------

This package is available in the `Keijiro` scoped registry.

- Name: `Keijiro`
- URL: `https://registry.npmjs.com`
- Scope: `jp.keijiro`

Please follow [this gist] to add the registry to your project.

[this gist]: https://gist.github.com/keijiro/f8c7e8ff29bfe63d86b888901b82644c

OSC Connection
--------------

![OSC Connection](https://user-images.githubusercontent.com/343936/165038054-33bebb1c-27b6-4fa3-9dd7-6f4091c7eb65.png)

The OSC Jack components require **OSC Connection** files to specify connection
types, host addresses and port numbers. To create a new OSC Connection file,
navigate to Assets > Create > ScriptableObjects > OSC Jack > Connection.

You must specify a target host address to send OSC messages (leave it empty for
receive-only connections).

Custom Nodes
------------

### OSC Input

![OSC Input](https://user-images.githubusercontent.com/343936/165106715-2f970d2f-0f2c-4b98-90d9-2ebb7bb99461.png)

**OSC Input** receives OSC messages in a flow graph.

There are the following variants for different data types:

- OscBangInput (trigger without data)
- OscIntInput
- OscFloatInput
- OscStringInput
- OscVector2Input
- OscVector3Input
- OscVector4Input

Note that it triggers the event multiple times in a single frame.

### OSC Output

![OSC Output](https://user-images.githubusercontent.com/343936/165106832-4d19d13e-343b-488f-8e5e-84c337e05c05.png)

**OSC Output** sends OSC messages from a flow graph.

There are the following variants for different data types:

- OscBangOutput (trigger without data)
- OscIntOutput
- OscFloatOutput
- OscStringOutput
- OscVector2Output
- OscVector3Output
- OscVector4Output
