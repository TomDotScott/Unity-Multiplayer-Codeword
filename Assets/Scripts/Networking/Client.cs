using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Networking.Transport;
using UnityEngine;

public class Client : Singleton<Client>
{
    public NetworkDriver Driver;
    private NetworkConnection m_connection;

    private bool m_isActive = false;

    public Action ConnectionDropped;

    public void Init(string ip, ushort port)
    {
        Driver = NetworkDriver.Create();

        // Connect to the specific ip address
        NetworkEndPoint endPoint = NetworkEndPoint.Parse(ip, port);

        // Attempt to connect to the endpoint
        m_connection = Driver.Connect(endPoint);

        Debug.Log($"Attempting to connect to {endPoint.Address}");

        m_isActive = true;

        RegisterEvent();
    }

    public void ShutDown()
    {
        if (!m_isActive) return;

        UnregisterEvent();
        Driver.Dispose();
        m_isActive = false;
    }

    public void OnDestroy()
    {
        ShutDown();
    }

    public void Update()
    {
        if(!m_isActive) return;

        Driver.ScheduleUpdate().Complete();

        CheckConnection();
        UpdateMessages();
    }

    private void CheckConnection()
    {
        if (!m_connection.IsCreated && m_isActive)
        {
            Debug.Log("Something went wrong... Lost connection to server");
            ConnectionDropped?.Invoke();
            ShutDown();
        }
    }

    private void UpdateMessages()
    {
        DataStreamReader stream;
        NetworkEvent.Type command;

        while ((command = m_connection.PopEvent(Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            switch (command)
            {
                case NetworkEvent.Type.Connect:
                    // TODO: SendToServer(new NetWelcome());
                    break;

                case NetworkEvent.Type.Data:
                    // TODO: NetUtility.OnData(stream, default(NetworkConnection));
                    break;

                case NetworkEvent.Type.Disconnect:
                    Debug.Log("Client got disconnected from server!");

                    m_connection = default;
                    ConnectionDropped?.Invoke();
                    ShutDown();
                    break;

                case NetworkEvent.Type.Empty:
                default:
                    break;
            }
        }
    }

    public void SendToServer(Message msg)
    {
        Driver.BeginSend(m_connection, out DataStreamWriter writer);
        msg.Serialise(ref writer);
        Driver.EndSend(writer);
    }

    private void RegisterEvent()
    {
        // TODO: NetUtility.KEEP_ALIVE += KeepAlive;
    }

    private void UnregisterEvent()
    {
        // TODO: NetUtility.KEEP_ALIVE -= KeepAlive;
    }

    private void KeepAlive(Message msg)
    {
        SendToServer(msg);
    }
}
