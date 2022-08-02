using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CommandInterpreter : NetworkBehaviour
{
    private GameSceneBuilder _gameSceneBuilder;
    
    private List<int> _cardIOnBoard;


    #region Server

    [ClientRpc]
    public void RpcSetStarterBoard(List<int> cardsId)
    {
        _gameSceneBuilder = GetComponent<GameSceneBuilder>();
        
        _gameSceneBuilder.SetStarterDeck(cardsId);
    }

    [ClientRpc]
    public void RpcGetNewAndOldCard(GameCommand command) 
    {
        _gameSceneBuilder = GetComponent<GameSceneBuilder>();
        
        _gameSceneBuilder.RemoveAndSetNewItem(command);
    }

    #endregion

    #region Client
    

    #endregion
}
