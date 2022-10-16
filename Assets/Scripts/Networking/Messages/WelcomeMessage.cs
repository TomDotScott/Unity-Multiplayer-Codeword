using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

public class WelcomeMessage : Message
{
    public int AssignedTeam { get; set; }

    public WelcomeMessage()
    {
        MessageCode = eMessageCode.WELCOME;
    }

    public WelcomeMessage(DataStreamReader reader)
    {
        MessageCode = eMessageCode.WELCOME;
        Deserialise(ref reader);
    }

    public override void Serialise(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte) MessageCode);
        writer.WriteInt(AssignedTeam);
    }

    public override void Deserialise(ref DataStreamReader reader)
    {
        AssignedTeam = reader.ReadInt();
    }

    public override void ReceivedOnClient()
    {
        NetUtility.WELCOME_CLIENT?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection client)
    {
        NetUtility.WELCOME_SERVER?.Invoke(this, client);
    }
}
