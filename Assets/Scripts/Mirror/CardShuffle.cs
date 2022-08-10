using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CardShuffle : NetworkBehaviour
{ 
    private readonly int _numberAllCard = 81;
    
    private readonly int _numberOfCardOnBoard = 12;

    public List<int> allCardsId = new List<int>();

    public List<int> cardsInBoard = new List<int>();

    
    public void AddStartValueToLists()
    {
        for (int i = 0; i < _numberAllCard; i++)
        {
            allCardsId.Add(i);
        }
        
        for (int i = 0; i < _numberOfCardOnBoard; i++)
        {
            int randomCard = Random.Range(0, allCardsId.Count);
            
            cardsInBoard.Add(allCardsId[randomCard]);
            
            allCardsId.RemoveAt(randomCard);
        }
    }

    public List<int> RemoveAndAddItem(List<int> oldItems)
    {
        List<int> newItems = new List<int>();

        foreach (var item in oldItems)
        {
            cardsInBoard.Remove(item);
        }

        for (int i = 0; i < 3; i++)
        {
            int randomCard = Random.Range(0, allCardsId.Count);
            
            cardsInBoard.Add(allCardsId[randomCard]);
            
            newItems.Add(allCardsId[randomCard]);
            
            allCardsId.RemoveAt(randomCard);
        }
        
        return newItems;
    }
}
