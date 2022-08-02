using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CommandHandler : NetworkBehaviour
{
    public SyncList<GameCommand> gameCommands = new SyncList<GameCommand>();

    public int indexGameCommand;


    public override void OnStartServer()
    {
        base.OnStartServer();

        gameCommands.Callback += HandleGameCommand;
        
        for (int index = 0; index < gameCommands.Count; index++)
            HandleGameCommand(SyncList<GameCommand>.Operation.OP_ADD, index, new GameCommand(), gameCommands[index]);
    }

    private void HandleGameCommand(SyncList<GameCommand>.Operation op, int itemindex, GameCommand olditem, GameCommand newitem)
    {
        switch (op)
        {
            case SyncList<GameCommand>.Operation.OP_ADD:
                UpdateBoard(newitem);
                
                Debug.Log(indexGameCommand);
                break;
        }
    }

    private void UpdateBoard(GameCommand command)
    {
        if (command.whatTypeCommand == GameCommand.WhatType.Init)
        {
            Dictionary<int, NetworkConnection> players =
                new Dictionary<int, NetworkConnection>(((SetNetworkManager)NetworkManager.singleton)
                    .inGameIdToNetworkConnections);

            Debug.Log("In Handler" + command.cardOnBoardStart);

            foreach (var net in players)
            {
                net.Value.identity.GetComponent<CommandInterpreter>().RpcSetStarterBoard(command.cardOnBoardStart);
            }
        }
        else if (command.whatTypeCommand == GameCommand.WhatType.Update)
        {
            Dictionary<int, NetworkConnection> players =
                new Dictionary<int, NetworkConnection>(((SetNetworkManager)NetworkManager.singleton)
                    .inGameIdToNetworkConnections);

            Debug.Log("In Handler" + command.cardOnBoardStart);

            foreach (var net in players)
            {
                net.Value.identity.GetComponent<CommandInterpreter>().RpcGetNewAndOldCard(command);
            }
        }
        
    }

    public void AddNewCommand(GameCommand command)
    {
        gameCommands.Add(command);
    }
}
