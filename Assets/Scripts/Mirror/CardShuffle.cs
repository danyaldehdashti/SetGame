using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CardShuffle : NetworkBehaviour
{ 
    private readonly int _numberAllCard = 81;
    
    private readonly int _numberOfCardOnBoard = 12;

    private List<int> _allCardsId = new List<int>();

    public List<int> cardsInBoard = new List<int>();

    
    public void AddStartValueToLists()
    {
        for (int i = 0; i < _numberAllCard; i++)
        {
            _allCardsId.Add(i);
        }
        
        for (int i = 0; i < _numberOfCardOnBoard; i++)
        {
            int randomCard = Random.Range(0, _allCardsId.Count);
            
            cardsInBoard.Add(randomCard);

            _allCardsId.Remove(randomCard);
        }
    }

    public void RemoveCard(List<int> cardRemoved)
    {
        for (int i = 0; i < cardRemoved.Count; i++)
        {
            cardsInBoard.RemoveAt(cardRemoved[i]);
        }
    }
    
    private void AddNewCardOnBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            int randomCard = Random.Range(0, _allCardsId.Count);
            
            cardsInBoard.Add(randomCard);

            _allCardsId.Remove(randomCard);
        }
    }
    
}
