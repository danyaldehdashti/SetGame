
using System.Collections.Generic;

public class GameCommand
{
    public enum WhatType
    {
        Init,
        Update
    };

    public WhatType whatTypeCommand;

    public List<int> cardOnBoardStart = new List<int>();

    public List<int> playerCardChose = new List<int>();

    public List<int> newCards = new List<int>();

    public List<int> spawnPointsPlayerCardChose = new List<int>();

    public int countOfAllCard;

    public int playerId = 0;
}
