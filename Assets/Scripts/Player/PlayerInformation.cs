using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerInformation : NetworkBehaviour
{
    [Header("Dependency")]
    
    [SyncVar(hook = nameof(NumberOfBulletsHooks))]
    public int numberOfBullet;

    [SyncVar(hook = nameof(TimerHooks))]
    public float timer;
    
    [SyncVar(hook = nameof(IsGameInProgressHooks))]
    public bool isGameInProgress;
    
    [SyncVar(hook = nameof(ScoreHooks))]
    public int score;
    

    [SerializeField] private float timerTime;

    public bool isPartyOwner = false;
    
    private PlayerHandler _playerHandler;

    private PlayerUiHandler _playerUiHandler;

    private GameSceneBuilder _gameSceneBuilder;
    
    private GameInput _gameInput;

    private readonly List<int> _cardChose = new List<int>();

    private readonly List<int> _spawnPoints = new List<int>();
    

    #region Server

    [Server]
    public void SetGameInProgress(bool state)
    {
        isGameInProgress = state;
    }

    [Command]
    private void CmdTimerActive()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            timer = timerTime;
            numberOfBullet++;
        }
    }
    
    [Command]
    private void CmdUseBullet()
    {
        numberOfBullet--;
    }
    
    [Command]
    private void CmdCardChoseDone(List<int> ids,List<int> spawnPoint,int playerId)
    {
        _gameInput = FindObjectOfType<GameInput>();
        
        _gameInput.GetCommandForUpdate(ids,spawnPoint,playerId);
    }

    [Command]
    private void CmdStartGame()
    {
        GameCommand newCommand = new GameCommand();

        newCommand.whatTypeCommand = GameCommand.WhatType.Init;

        GameInput _gameInput = FindObjectOfType<GameInput>();
        
        _gameInput.GetCommandForInit(newCommand);
    }

    [Command]
    private void CmdAddScore()
    {
        score++;
    }
    
    #endregion


    #region Client

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isLocalPlayer) { return; }

        _playerUiHandler = GetComponent<PlayerUiHandler>();
        _gameSceneBuilder = GetComponent<GameSceneBuilder>();
    }

    [TargetRpc]
    public void TargetGetPlayersData(List<PlayerData> playersData)
    {
        _playerUiHandler.UpdateScoreBoard(playersData);
    }

    [Client]
    private void GameInProgress()
    {
        _playerUiHandler.SpawnCanvas();

        _gameSceneBuilder.BuildGameScene();
        
        if (isPartyOwner)
        {
            CmdStartGame();
        }
    }

    [Client]
    public void GetCardChose(int id, int spawnPoint)
    {
        _cardChose.Add(id);
        _spawnPoints.Add(spawnPoint);
        
        if (_cardChose.Count == 3)
        {
            ChoseThreeCard();
        }
    }

    [Client]
    private void ChoseThreeCard()
    {
        _playerHandler = GetComponent<PlayerHandler>();
        CmdCardChoseDone(_cardChose,_spawnPoints,_playerHandler.inGameId);
        
        DeSelectedCards();
        
        CmdUseBullet();
    }

    [Client]
    public void DeSelectedCards()
    {
        _gameSceneBuilder.DeSelectedCardInPlayerInformation();
        
        _cardChose.Clear();
        _spawnPoints.Clear();
    }

    [Client]
    public void AddScore()
    {
        CmdAddScore();
    }
    
    [ClientCallback]
    private void Update()
    {
        if (numberOfBullet < 6)
        {
            CmdTimerActive();
        }
    }

    #endregion


    #region Hooks

    private void IsGameInProgressHooks(bool oldValue, bool newValue)
    {
        if (!isLocalPlayer){ return;}

        _gameInput = FindObjectOfType<GameInput>();

        GameInProgress();
    }
    

    private void NumberOfBulletsHooks(int oldValue, int newValue)
    {
        if (!isLocalPlayer){ return;}
        
        if (!isGameInProgress){ return;}

        _playerUiHandler.UpdateBullets(newValue);
    }


    private void TimerHooks(float oldValue, float newValue)
    {
        if (!isLocalPlayer){ return;}
        
        if (!isGameInProgress){ return;}

        _playerUiHandler.UpdateTimer(newValue);
    }

    private void ScoreHooks(int oldValue, int newValue)
    {
        if (!isLocalPlayer){ return;}
    }


    #endregion
}
