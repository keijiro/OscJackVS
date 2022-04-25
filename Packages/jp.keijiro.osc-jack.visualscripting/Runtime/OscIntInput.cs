using OscJack;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace OscJack.VisualScripting {

[UnitCategory("OSC"), UnitTitle("OSC Input (int)")]
[RenamedFrom("Bolt.Addons.OscJack.OscIntInput")]
public sealed class OscIntInput
  : Unit, IGraphElementWithData, IGraphEventListener
{
    #region Data class

    public sealed class Data : IGraphElementData
    {
        public System.Action<EmptyEventArgs> UpdateAction { get; set; }
        public int LastValue { get; private set; }
        public bool IsOpened => _port != 0;
        public bool HasNewValue => _queue.Count > 0;

        int _port;
        string _address;
        Queue<int> _queue = new Queue<int>();

        public void Dequeue()
          => LastValue = _queue.Dequeue();

        public void UpdateConnection(OscConnection connection, string address)
        {
            if (IsOpened)
            {
                if (connection?.port == _port && address == _address)
                {
                    // The current connection is okay.
                }
                else
                {
                    // The destination was changed. Reopen the connection.
                    Close();
                    TryOpen(connection, address);
                }
            }
            else
            {
                // No connection. Open the connection.
                TryOpen(connection, address);
            }
        }

        void TryOpen(OscConnection connection, string address)
        {
            if (connection == null || string.IsNullOrEmpty(address)) return;

            _port = connection.port;
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
            _queue.Clear();
        }

        void OnDataReceive(string address, OscDataHandle data)
        {
            lock (_queue) _queue.Enqueue(data.GetElementAsInt(0));
        }
    }

    public IGraphElementData CreateData() => new Data();

    #endregion

    #region Unit I/O

    [DoNotSerialize]
    public ValueInput Connection { get; private set; }

    [DoNotSerialize]
    public ValueInput Address { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ControlOutput Received { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ValueOutput Value { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        isControlRoot = true;
        Connection = ValueInput<OscConnection>(nameof(Connection), null);
        Address = ValueInput<string>(nameof(Address), "/unity");
		Received = ControlOutput(nameof(Received));
        Value = ValueOutput<int>(nameof(Value), GetValue);
    }

    int GetValue(Flow flow)
      => flow.stack.GetElementData<Data>(this).LastValue;

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
        using var flow = Flow.New(reference);

        var data = flow.stack.GetElementData<Data>(this);
        var connection = flow.GetValue<OscConnection>(Connection);
        var address = flow.GetValue<string>(Address);

        data.UpdateConnection(connection, address);

        while (data.HasNewValue)
        {
            data.Dequeue();
            flow.Invoke(Received);
        }
    }

    #endregion
}

} // namespace OscJack.VisualScripting
