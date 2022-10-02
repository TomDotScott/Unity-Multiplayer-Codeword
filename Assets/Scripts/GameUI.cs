using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    public Server GameServer;

    public Client GameClient;

    [SerializeField] private TMPro.TMP_InputField m_inputField;

    [SerializeField] private Animator m_animator;

    private String m_defaultIP = "127.0.0.1";
    private ushort m_port = 6969;

    // Will be passed into the GameManager and Server to determine which logic the game should follow
    public enum eGameMode
    {
        eNone,
        eSinglePlayer,
        eMultiplayer,
        eMultiplayerVersus
    }

    // TODO: Use this value to determine which logic for the codeword game to follow
    public eGameMode GameMode = eGameMode.eNone;


void Start()
    {
        Instance = this;
    }

    public void OnSingleplayerPressed()
    {
        m_animator.SetTrigger("FadeMenus");
        GameMode = eGameMode.eSinglePlayer;

        GameServer.Init(m_port);
        GameClient.Init(m_defaultIP, m_port);
    }

    public void OnMultiplayerPressed()
    {
        m_animator.SetTrigger("OnlineMenu");
        GameMode = eGameMode.eMultiplayer;
    }

    public void OnMultiplayerVersusPressed()
    {
        m_animator.SetTrigger("OnlineMenu");
        GameMode = eGameMode.eMultiplayerVersus;
    }

    public void OnHostPressed()
    {
        // TODO: Start hosting TCP/UDP server and wait for people to connect
        GameServer.Init(m_port);

        // When we host a game, we also want to connect ourselves...
        GameClient.Init(m_defaultIP, m_port);

        Debug.Log("Host pressed!");
        m_animator.SetTrigger("HostMenu");
    }

    public void OnConnectPressed()
    {
        // TODO: Have some additional logic to read from the IP Input and look for a TCP/UDP connection
        String ipAddress = m_inputField.text;
        GameClient.Init(ipAddress, 6969);

        Debug.Log("Connect pressed!");
    }

    public void OnOnlineBackPressed()
    {
        m_animator.SetTrigger("StartMenu");
    }

    public void OnHostBackPressed()
    {
        m_animator.SetTrigger("HostBack");
        GameServer.ShutDown();
        GameClient.ShutDown();
    }
}
