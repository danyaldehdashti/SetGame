using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerHandler : NetworkBehaviour
{
    #region SyncVars

    [SyncVar]
    public int inGameId = 0;

    [SyncVar(hook = nameof(IsPartyOwnerHook))]
    public bool isPartyOwner;

    #endregion


    [SerializeField] private GameObject lobbyUi;

    private LobbyUi _lobbyUi;


    #region Server
    
    [Server]
    public void SetPlayerId(int id)
    {
        inGameId = id;
    }

    [Server]
    public void SetIsPartyOwner(bool state)
    {
        isPartyOwner = state;
    }
    
    #endregion


    #region Client

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isLocalPlayer){ return;}
        
        SpawnLobbyUi();
    }

    [Client]
    private void SpawnLobbyUi()
    {
        GameObject lobbyUiInstance =  Instantiate(lobbyUi);

        _lobbyUi = lobbyUiInstance.GetComponent<LobbyUi>();

        Lobby lobby = FindObjectOfType<Lobby>();
        
        lobby.GetLobbyUi();
        
    }
    

    #endregion
    
    
    private void IsPartyOwnerHook(bool oldValue, bool newValue)
    {
        _lobbyUi.startButton.gameObject.SetActive(newValue);
    }
}
