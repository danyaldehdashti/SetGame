using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameInput : NetworkBehaviour
{
    private bool _isBuildStarterDeck = false;

    private CardShuffle _cardShuffle;

    private CommandHandler _commandHandler;

    private CheckCards _checkCards;


    private void Awake()
    {
        _checkCards = GetComponent<CheckCards>();
    }
    

    public void GetCommandForInit(GameCommand newCommandFromPlayer)
    {
        _cardShuffle = FindObjectOfType<CardShuffle>();
        _commandHandler = FindObjectOfType<CommandHandler>();

        CheckCommand(newCommandFromPlayer);
    }

    public void GetCommandForUpdate(List<int> cardsId,List<int> spawnPoints,int playerId)
    {
        GameCommand newCommand = new GameCommand();

        newCommand.whatTypeCommand = GameCommand.WhatType.Update;

        newCommand.playerId = playerId;

        newCommand.playerCardChose = cardsId;

        newCommand.spawnPointsPlayerCardChose = spawnPoints;
        
        CheckCommand(newCommand);
    }

    private void CheckCommand(GameCommand newCommand)
    {
        if (newCommand.whatTypeCommand == GameCommand.WhatType.Init)
        {
            if (_isBuildStarterDeck) {return;}
            
            _cardShuffle.AddStartValueToLists();
            _isBuildStarterDeck = true;

            newCommand.cardOnBoardStart = _cardShuffle.cardsInBoard;

            newCommand.countOfAllCard = _cardShuffle.allCardsId.Count;
            
            _commandHandler.AddNewCommand(newCommand);
            
            _commandHandler.indexGameCommand++;
        }

        if (newCommand.whatTypeCommand == GameCommand.WhatType.Update)
        {
            if (_checkCards.PickUpConditions(newCommand.playerCardChose))
            {
                newCommand.newCards = _cardShuffle.RemoveAndAddItem(newCommand.playerCardChose);

                newCommand.countOfAllCard = _cardShuffle.allCardsId.Count;
                
                _commandHandler.AddNewCommand(newCommand);
                _commandHandler.indexGameCommand++;
            }
            else
            {
                Debug.Log("Not Set");
            }
        }

        GameManager gameManager = FindObjectOfType<GameManager>();
        
        gameManager.DelayToConvertNetworkConnectionToPlayerData();
    }
}
