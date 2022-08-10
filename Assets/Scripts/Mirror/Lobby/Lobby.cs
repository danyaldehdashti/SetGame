using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;



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
    

    public void RemovePlayer( int id)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].inGameId == id)
            {
                players.RemoveAt(i);
            }
        }
    }

    private void UpdateLobby()
    {
        _lobbyUi = FindObjectOfType<LobbyUi>();

        if (_lobbyUi == null)
        {
            Invoke(nameof(UpdateLobby) , 1);
            return;
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
}
