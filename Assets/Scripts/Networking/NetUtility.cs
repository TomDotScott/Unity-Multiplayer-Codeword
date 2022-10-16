using System;
using Unity.Networking.Transport;
using UnityEngine;

public static class NetUtility
{
    #region CLIENT_MESSAGES
    public static Action<Message> HEARTBEAT_CLIENT;
    public static Action<Message> WELCOME_CLIENT;
    public static Action<Message> START_GAME_CLIENT;
    public static Action<Message> MAKE_MOVE_CLIENT;
    public static Action<Message> REMATCH_CLIENT;
    #endregion

    #region SERVER_MESSAGES
    public static Action<Message, NetworkConnection> HEARTBEAT_SERVER;
    public static Action<Message, NetworkConnection> WELCOME_SERVER;
    public static Action<Message, NetworkConnection> START_GAME_SERVER;
    public static Action<Message, NetworkConnection> MAKE_MOVE_SERVER;
    public static Action<Message, NetworkConnection> REMATCH_SERVER;
    #endregion


    public static void OnData(DataStreamReader stream, NetworkConnection connection, Server server = null)
    {
        Message msg = null;

        Message.eMessageCode code = (Message.eMessageCode)  stream.ReadByte();

        switch (code)
        {
            case Message.eMessageCode.HEARTBEAT:
                msg = new HeartBeatMessage(stream);
                break;
            // TODO: 
            case Message.eMessageCode.WELCOME:
                msg = new WelcomeMessage(stream);
                break;
            //case Message.eMessageCode.START_GAME:
            //    msg = new StartGameMessage(stream);
            //    break;
            //case Message.eMessageCode.MAKE_MOVE:
            //    msg = new MakeMoveMessage(stream);
            //    break;
            //case Message.eMessageCode.REMATCH:
            //    msg = new RematchMessage(stream);
            //    break;
            default:
                Debug.Log("Message received had a bad code... Was this a mistake?");
                break;
        }

        if (server != null)
        {
            msg?.ReceivedOnServer(connection);
        }
        else
        {
            msg?.ReceivedOnClient();
        }
    }
}

