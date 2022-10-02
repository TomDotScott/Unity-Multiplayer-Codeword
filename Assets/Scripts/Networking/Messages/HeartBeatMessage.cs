
using Unity.Networking.Transport;

public class HeartBeatMessage : Message
{
    public HeartBeatMessage()
    {
        MessageCode = eMessageCode.HEARTBEAT;
    }

    public HeartBeatMessage(DataStreamReader reader)
    {
        MessageCode = eMessageCode.HEARTBEAT;
        Deserialise(ref reader);
    }

    public override void Serialise(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte) MessageCode);
    }

    public override void Deserialise(ref DataStreamReader reader)
    {
        // KeepAlive contains no information, it's just there to maintain a connection between client and server. Deserialise will be empty
    }

    public override void ReceivedOnClient()
    {
        NetUtility.HEARTBEAT_CLIENT?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection client)
    {
        NetUtility.HEARTBEAT_SERVER?.Invoke(this, client);
    }
}
