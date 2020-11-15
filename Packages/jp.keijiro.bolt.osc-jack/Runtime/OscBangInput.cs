using Ludiq;
using OscJack;
using System.Collections.Generic;
using UnityEngine;

namespace Bolt.Addons.OscJack {

[UnitCategory("OSC"), UnitTitle("OSC Input (Bang)")]
public sealed class OscBangInput
  : Unit, IGraphElementWithData, IGraphEventListener
{
    #region Data class

    public sealed class Data : IGraphElementData
    {
        public System.Action<EmptyEventArgs> UpdateAction { get; set; }
        public int BangCount { get; set; }
        public bool IsOpened => _port != 0;

        int _port;
        string _address;

        public void SetDestination(int port, string address)
        {
            if (IsOpened)
            {
                if (port == _port && address == _address)
                {
                    // The current connection is okay.
                }
                else
                {
                    // The destination was changed. Reopen the connection.
                    Close();
                    Open(port, address);
                }
            }
            else
            {
                // No connection. Open the connection.
                Open(port, address);
            }
        }

        void Open(int port, string address)
        {
            _port = port;
            _address = address;

            var server = OscMaster.GetSharedServer(_port);
            server.MessageDispatcher.AddCallback(_address, OnDataReceive);
        }

        public void Close()
        {
            var server = OscMaster.GetSharedServer(_port);
            server.MessageDispatcher.RemoveCallback(_address, OnDataReceive);

            _port = 0;
            _address = null;
        }

        void OnDataReceive(string address, OscDataHandle data)
          => BangCount++;
    }

    public IGraphElementData CreateData() => new Data();

    #endregion

    #region Unit I/O

    [DoNotSerialize]
    public ValueInput Port { get; private set; }

    [DoNotSerialize]
    public ValueInput Address { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ControlOutput Received { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        isControlRoot = true;
        Port = ValueInput<uint>(nameof(Port), 8000);
        Address = ValueInput<string>(nameof(Address), "/unity");
		Received = ControlOutput(nameof(Received));
    }

    #endregion

    #region Graph event listener

    public void StartListening(GraphStack stack)
    {
        var data = stack.GetElementData<Data>(this);
        if (data.UpdateAction != null) return;

        var reference = stack.ToReference();
        data.UpdateAction = args => OnUpdate(reference);

        var hook = new EventHook(EventHooks.Update, stack.machine);
        EventBus.Register(hook, data.UpdateAction);
    }

    public void StopListening(GraphStack stack)
    {
        var data = stack.GetElementData<Data>(this);
        if (data.UpdateAction == null) return;

        var hook = new EventHook(EventHooks.Update, stack.machine);
        EventBus.Unregister(hook, data.UpdateAction);

        if (data.IsOpened) data.Close();
        data.UpdateAction = null;
    }

    public bool IsListening(GraphPointer pointer)
      => pointer.GetElementData<Data>(this).UpdateAction != null;

    #endregion

    #region Update hook

    void OnUpdate(GraphReference reference)
    {
        using (var flow = Flow.New(reference))
        {
            var data = flow.stack.GetElementData<Data>(this);
            var port = (int)flow.GetValue<uint>(Port);
            var address = flow.GetValue<string>(Address);

            data.SetDestination(port, address);

            for (; data.BangCount > 0; data.BangCount--)
                flow.Invoke(Received);
        }
    }

    #endregion
}

} // namespace Bolt.Addons.OscJack
