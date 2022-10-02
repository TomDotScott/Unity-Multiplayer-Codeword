using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;
using System;
using System.Linq;

public class Server : Singleton<Server>
{
    public NetworkDriver Driver;

    private NativeList<NetworkConnection> m_connections;

    private bool m_isActive = false;

    private const float m_keepAliveTickRate = 20.0f;
    private float m_lastKeepAlive;

    public Action ConnectionDropped;

    public void Init(ushort port)
    {
        Driver = NetworkDriver.Create();
        
        // Allows anyone to connect
        NetworkEndPoint endPoint = NetworkEndPoint.AnyIpv4;

        endPoint.Port = port;

        // Start listening for incoming connections
        if (Driver.Bind(endPoint) != 0)
        {
            Debug.Log($"Unable to bind port! {endPoint.Port}");
            return;
        }

        Driver.Listen();
        Debug.Log($"Currently listening to port: {endPoint.Port}");

        // Set up the max amount of players
        m_connections = new NativeList<NetworkConnection>(2, Allocator.Persistent);

        m_isActive = true;
    }

    public void ShutDown()
    {
        if (!m_isActive) return;

        Driver.Dispose();
        m_connections.Dispose();
        m_isActive = false;
    }

    public void OnDestroy()
    {
        ShutDown();
    }

    public void Update()
    {
        if(!m_isActive) return;

        // KeepAlive();

        Driver.ScheduleUpdate().Complete();
        
        // If anyone is no longer connected but we still have their reference get rid of them...
        CleanupConnections();

        // Look for new connections and accept incoming requests
        AcceptNewConnections();

        // Are connected players sending messages? 
        UpdateMessages();
    }

    private void CleanupConnections()
    {
        foreach (NetworkConnection connection in m_connections)
        {
            if (!connection.IsCreated)
            {
                m_connections.RemoveAtSwapBack(m_connections.IndexOf(connection));
            }
        }
    }

    private void AcceptNewConnections()
    {
        NetworkConnection connection;

        // Keep looking for new connections
        while ((connection = Driver.Accept()) != default)
        {
            // If we accept a new connection, add it to our list
            m_connections.Add(connection);
        }
    }

    private void UpdateMessages()
    {
        DataStreamReader stream;

        for (int i = 0; i < m_connections.Length; i++)
        {
            NetworkEvent.Type command;

            while ((command = Driver.PopEventForConnection(m_connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                switch (command)
                {
                    case NetworkEvent.Type.Data:
                        // TODO: CREATE THIS FUNCTION
                        // NetUtility.OnData(stream, m_connections[i], this);
                        break;
                    case NetworkEvent.Type.Disconnect:
                        Debug.Log("Client disconnected from server!");
                        m_connections[i] = default;
                        ConnectionDropped?.Invoke();

                        // If the other player disconnects, we shutdown (a two player game can't be played by one person!)
                        ShutDown();
                        break;
                    // We don't care about Type.Connect or Type.Empty here... 
                    case NetworkEvent.Type.Empty:
                    case NetworkEvent.Type.Connect:
                        break;
                }
            }
        }
    }

    public void Broadcast(Message msg)
    {
        // Loop through the connected clients, sending a network message
        foreach (var client in m_connections.Where(client => client.IsCreated))
        {
            Debug.Log($"Sending {msg.Code} to: {client.InternalId}");

            SendToClient(client, msg);
        }
    }

    public void SendToClient(NetworkConnection client, Message msg)
    {
        // Use the networkDriver to send a message
        Driver.BeginSend(client, out var writer);

        // Serialise the message to be sent over the network
        msg.Serialise(ref writer);
        Driver.EndSend(writer);
    }
}
