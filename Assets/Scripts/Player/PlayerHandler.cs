using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : NetworkBehaviour
{
    [Header("Dependency")]

    [SerializeField] private LobbyUi lobbyUi;
    
    
    [SyncVar]
    public int inGameId = 0;

    [SyncVar(hook = nameof(IsPartyOwnerHook))]
    public bool isPartyOwner;

    [SyncVar(hook = nameof(IsGameInProgressHook))]
    public bool isGameInProgress;
    
    private LobbyUi _lobbyUi;

    private PlayerUiHandler _playerUiHandler;

    private GameSceneBulider _gameSceneBulider;
    

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
    
    [Server]
    public void SetGameInProgress(bool state)
    {
        isGameInProgress = state;
    }
    
    [Command]
    private void CmdStartGame()
    {
        ((SetNetworkManager) NetworkManager.singleton).StartGame();
    }
    
    #endregion


    #region Client

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (!isLocalPlayer){ return;}

        _lobbyUi.startButton.onClick.AddListener(CmdStartGame);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isLocalPlayer){ return;}
        
        SpawnLobbyUi();
        
        _playerUiHandler = GetComponent<PlayerUiHandler>();
        
        Debug.Log(_playerUiHandler);
    }

    [Client]
    private void SpawnLobbyUi()
    {
        GameObject lobbyUiInstance =  Instantiate(lobbyUi.gameObject);

        _lobbyUi = lobbyUiInstance.GetComponent<LobbyUi>();

        Lobby lobby = FindObjectOfType<Lobby>();
        
        lobby.GetLobbyUi();
    }
    
    #endregion
    

    #region Hooks

    private void IsPartyOwnerHook(bool oldValue, bool newValue)
    {
        if (!isLocalPlayer){ return;}

        _lobbyUi.startButton.gameObject.SetActive(newValue);
    }

    private void IsGameInProgressHook(bool oldValue, bool newValue)
    {
        if (!isLocalPlayer){ return;}

        _playerUiHandler.SpawnCanvas();
    }

    #endregion
    
    
}
