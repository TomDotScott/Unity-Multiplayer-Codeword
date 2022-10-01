using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    [SerializeField] private TMPro.TMP_InputField m_InputField;

    void Start()
    {
        Instance = this;
    }

    public void OnSingleplayerPressed()
    {
        Debug.Log("Singleplayer pressed!");
    }

    public void OnMultiplayerPressed()
    {
        Debug.Log("Multiplayer pressed!");
    }

    public void OnMultiplayerVersusPressed()
    {
        Debug.Log("Multiplayer versus pressed!");
    }

    public void OnHostPressed()
    {
        Debug.Log("Host pressed!");
    }

    public void OnConnectPressed()
    {
        Debug.Log("Connect pressed!");
    }

    public void OnOnlineBackPressed()
    {
        Debug.Log("ONLINE back pressed!");
    }

    public void OnHostBackPressed()
    {
        Debug.Log("HOST back pressed!");
    }
}
