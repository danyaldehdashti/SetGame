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
    

    public List<int> cardOnBoard = new List<int>();


    private LobbyUi _lobbyUi;

    private PlayerUiHandler _playerUiHandler;

    private GameSceneBuilder _gameSceneBuilder;

    private CardShuffle _cardShuffle;
    

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

        _gameSceneBuilder = FindObjectOfType<GameSceneBuilder>();
        
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

    
    #endregion
    
}
