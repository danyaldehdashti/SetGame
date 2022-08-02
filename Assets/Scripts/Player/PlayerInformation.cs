using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerInformation : NetworkBehaviour
{
    [Header("Dependency")]
    
    [SyncVar]
    public int numberOfBullet;
    
    [SyncVar(hook = nameof(IsGameInProgressHook))]
    public bool isGameInProgress;

    
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
    private void CmdBuildStartBoard()
    {
        GameCommand newCommand = new GameCommand();

        newCommand.whatTypeCommand = GameCommand.WhatType.Init;

        _gameInput = FindObjectOfType<GameInput>();
        
        _gameInput.GetCommandForInit(newCommand);
    }

    [Command]
    private void CmdCardChoseDone(List<int> ids,List<int> spawnPoint,int playerId)
    {
        
        _gameInput = FindObjectOfType<GameInput>();
        
        _gameInput.GetCommandForUpdate(ids,spawnPoint,playerId);
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

    [Client]
    private void GameInProgress()
    {
        _playerUiHandler.SpawnCanvas();
        
        _gameSceneBuilder.BuildGameScene();
        
        CmdBuildStartBoard();
    }

    [Client]
    public void GetCardChose(int id, int spawnPoint)
    {
        _cardChose.Add(id);
        _spawnPoints.Add(spawnPoint);
        
        Debug.Log(spawnPoint);

        if (_cardChose.Count == 3)
        {
            _playerHandler = GetComponent<PlayerHandler>();
            CmdCardChoseDone(_cardChose,_spawnPoints,_playerHandler.inGameId);
            _cardChose.Clear();
            _spawnPoints.Clear();
        }
    }

    
    #endregion


    #region Hooks

    private void IsGameInProgressHook(bool oldValue, bool newValue)
    {
        if (!isLocalPlayer){ return;}

        _gameInput = FindObjectOfType<GameInput>();

        GameInProgress();
    }


    #endregion
}
