
using System.Collections.Generic;

public class CommandSystem
{
    public enum WhatType
    {
        Init,
        Update
    };

    public List<int> previousCards;

    public List<int> newCards;

    public int playerId;
}
