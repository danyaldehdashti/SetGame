using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CommandInterpreter : MonoBehaviour
{
    private GameSceneBuilder _gameSceneBuilder;

    private CardDeck _cardDeck;

    private PlayerInformation _playerInformation;

    private PlayerHandler _playerHandler;

    public void SetStarterBoard(GameCommand command)
    {
        _gameSceneBuilder = GetComponent<GameSceneBuilder>();
        
        _gameSceneBuilder.SetStarterDeck(command.cardOnBoardStart);
        
        
        SetCardDeck(command);
    }

    public void GetNewAndOldCard(GameCommand command) 
    {
        _gameSceneBuilder = GetComponent<GameSceneBuilder>();
        
        _gameSceneBuilder.RemoveAndSetNewItem(command);
        
        
        SetCardDeck(command);
        CheckCommand(command);
    }

    private void SetCardDeck(GameCommand command)
    {
        _cardDeck = FindObjectOfType<CardDeck>();
        
        _cardDeck.SetNumberOfDeckCard(command.countOfAllCard);
    }

    private void CheckCommand(GameCommand command)
    {
        _playerHandler = GetComponent<PlayerHandler>();
        _playerInformation = GetComponent<PlayerInformation>();

        _playerInformation.DeSelectedCards();
        
        if (command.playerId == _playerHandler.inGameId)
        {
            _playerInformation.AddScore();
        }
    }
}
