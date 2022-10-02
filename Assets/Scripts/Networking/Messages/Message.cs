using System;
using Unity.Networking.Transport;

public abstract class Message
{
    public enum eMessageCode
    {
        HEARTBEAT = 0x1,
        WELCOME = 0x2,
        START_GAME = 0x3,
        MAKE_MOVE = 0x4,
        REMATCH = 0x5
    }

    public eMessageCode MessageCode;

    public virtual void Serialise(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte) MessageCode);
    }

    public virtual void Deserialise(ref DataStreamReader reader)
    {
        byte msg = reader.ReadByte();
        MessageCode = (eMessageCode) msg;
    }

    public virtual void ReceivedOnClient()
    {

    }

    public virtual void ReceivedOnServer(NetworkConnection client)
    {

    }

}
