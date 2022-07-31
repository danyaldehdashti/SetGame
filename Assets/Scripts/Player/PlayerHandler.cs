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



    public List<int> _cardOnBoard = new List<int>();


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

    [Client]
    private void GameInProgress()
    {
        _playerUiHandler.SpawnCanvas();
        
        _gameSceneBuilder.BuildGameScene();
        
        SetStarterValue();
        
    }

    [Client]
    private void SetStarterValue()
    {
        int countOfCardOnBoard = 12;
        for (int i = 0; i < countOfCardOnBoard; i++)
        {
            _cardOnBoard.Add(i);
        }
        
        for (int i = 0; i < _cardShuffle.cardsInBoard.Count; i++)
        {
            _cardOnBoard[i] = _cardShuffle.cardsInBoard[i];
        }
        
        _gameSceneBuilder.SetStarterDeck(_cardOnBoard);
    }

    [Client]
    private void UpdateBoard()
    {
        for (int i = 0; i < _cardShuffle.cardsInBoard.Count; i++)
        {
            _cardOnBoard[i] = _cardShuffle.cardsInBoard[i];
        }
        
        _gameSceneBuilder.SetStarterDeck(_cardOnBoard);
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
        
        _cardShuffle = FindObjectOfType<CardShuffle>();

        //GameInProgress();
    }

    #endregion
    
}
