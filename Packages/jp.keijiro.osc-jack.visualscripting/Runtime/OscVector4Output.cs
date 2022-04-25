using OscJack;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace OscJack.VisualScripting {

[UnitCategory("OSC"), UnitTitle("OSC Output (Vector 4)")]
[RenamedFrom("Bolt.Addons.OscJack.OscVector4Output")]
public sealed class OscVector4Output : Unit
{
    #region Unit I/O

    [DoNotSerialize, PortLabelHidden]
    public ControlInput Enter { get; private set; }

    [DoNotSerialize]
    public ValueInput Connection { get; private set; }

    [DoNotSerialize]
    public ValueInput Address { get; private set; }

    [DoNotSerialize, PortLabel("Data")]
    public ValueInput Input { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ControlOutput Exit { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        Enter = ControlInput(nameof(Enter), OnEnter);
        Exit = ControlOutput(nameof(Exit));
        Succession(Enter, Exit);

        Connection = ValueInput<OscConnection>(nameof(Connection), null);
        Address = ValueInput<string>(nameof(Address), "/unity");
        Input = ValueInput<Vector4>(nameof(Input), Vector4.zero);
    }

    ControlOutput OnEnter(Flow flow)
    {
        var connection = flow.GetValue<OscConnection>(Connection);
        if (connection == null) return Exit;

        var client = OscMaster.GetSharedClient(connection.host, connection.port);

        var address = flow.GetValue<string>(Address);
        var input = flow.GetValue<Vector4>(Input);
        client.Send(address, input.x, input.y, input.z, input.w);

        return Exit;
    }

    #endregion
}

} // namespace OscJack.VisualScripting
