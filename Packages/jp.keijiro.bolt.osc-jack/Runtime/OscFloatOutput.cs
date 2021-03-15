using OscJack;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace OscJack.VisualScripting {

[UnitCategory("OSC"), UnitTitle("OSC Output (Float)")]
[RenamedFrom("Bolt.Addons.OscJack.OscFloatOutput")]
public sealed class OscFloatOutput : Unit
{
    #region Unit I/O

    [DoNotSerialize, PortLabelHidden]
    public ControlInput Enter { get; private set; }

    [DoNotSerialize]
    public ValueInput SendTo { get; private set; }

    [DoNotSerialize]
    public ValueInput Port { get; private set; }

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

        SendTo = ValueInput<string>(nameof(SendTo), "127.0.0.1");
        Port = ValueInput<uint>(nameof(Port), 9000);
        Address = ValueInput<string>(nameof(Address), "/unity");
        Input = ValueInput<float>(nameof(Input), 0);
    }

    ControlOutput OnEnter(Flow flow)
    {
        var sendto = flow.GetValue<string>(SendTo);
        var port = (int)flow.GetValue<uint>(Port);
        var client = OscMaster.GetSharedClient(sendto, port);

        var address = flow.GetValue<string>(Address);
        var input = flow.GetValue<float>(Input);
        client.Send(address, input);

        return Exit;
    }

    #endregion
}

} // namespace OscJack.VisualScripting
