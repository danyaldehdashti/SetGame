using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CommandHandler : NetworkBehaviour
{
    private SyncList<GameCommand> gameCommands = new SyncList<GameCommand>();

    public int indexGameCommand;


    public override void OnStartClient()
    {
        base.OnStartClient();

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
                break;
        }
    }

    private void UpdateBoard(GameCommand command)
    {
        if (command.whatTypeCommand == GameCommand.WhatType.Init)
        {
            NetworkClient.connection.identity.GetComponent<CommandInterpreter>().SetStarterBoard(command);
            
        }
        else if (command.whatTypeCommand == GameCommand.WhatType.Update)
        {
            NetworkClient.connection.identity.GetComponent<CommandInterpreter>().GetNewAndOldCard(command);
        }
    }

    public void AddNewCommand(GameCommand command)
    {
        gameCommands.Add(command);
    }
}
