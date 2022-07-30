using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeta
{
    public int inGameId = 0;
    public string name = "";
    public int avatarCode;
}

public class Lobby : NetworkBehaviour
{
    #region Private_Varibels

    private LobbyUi _lobbyUi;

    #endregion

    public SyncList<PlayerDeta> players = new SyncList<PlayerDeta>();

    [SerializeField] private float waitTime;

    public override void OnStartClient()
    {
        base.OnStartClient();

        players.Callback += HandlePlayersDetaList;
        
        for (int index = 0; index < players.Count; index++)
            HandlePlayersDetaList(SyncList<PlayerDeta>.Operation.OP_ADD, index, new PlayerDeta(), players[index]);
    }

    private void HandlePlayersDetaList(SyncList<PlayerDeta>.Operation op, int itemindex, PlayerDeta olditem, PlayerDeta newitem)
    {
        switch (op)
        {
            case SyncList<PlayerDeta>.Operation.OP_ADD:
                UpdateLobby();
                break;
            
            case SyncList<PlayerDeta>.Operation.OP_REMOVEAT:
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
