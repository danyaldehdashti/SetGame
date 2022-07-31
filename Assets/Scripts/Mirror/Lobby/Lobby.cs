using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData
{
    public int inGameId = 0;
    public string name = "";
    public int avatarCode;
}

public class Lobby : NetworkBehaviour
{
    [Header("Dependency")]

    [SerializeField] private float waitTime;
    
    public SyncList<PlayerData> players = new SyncList<PlayerData>();
    
    private LobbyUi _lobbyUi;



    public override void OnStartClient()
    {
        base.OnStartClient();

        players.Callback += HandlePlayersDetaList;
        
        for (int index = 0; index < players.Count; index++)
            HandlePlayersDetaList(SyncList<PlayerData>.Operation.OP_ADD, index, new PlayerData(), players[index]);
    }

    private void HandlePlayersDetaList(SyncList<PlayerData>.Operation op, int itemindex, PlayerData olditem, PlayerData newitem)
    {
        switch (op)
        {
            case SyncList<PlayerData>.Operation.OP_ADD:
                UpdateLobby();
                break;
            
            case SyncList<PlayerData>.Operation.OP_REMOVEAT:
                UpdateLobby();
                break;
        }
    }

    public void GetLobbyUi()
    {
        _lobbyUi = FindObjectOfType<LobbyUi>();
    }

    private void UpdateLobby()
    {
        if (_lobbyUi == null)
        {
            StartCoroutine(GetLobbyUiIEnumerator());
        }

        for (int i = 0; i < _lobbyUi.players.Count; i++)
        {
            _lobbyUi.players[i].SetActive(false);
        }

        for (int i = 0; i < players.Count; i++)
        {
            _lobbyUi.players[i].SetActive(true);
        }
    }

    IEnumerator GetLobbyUiIEnumerator()
    {
        yield return new WaitForSeconds(waitTime);
        UpdateLobby();
    }
}
