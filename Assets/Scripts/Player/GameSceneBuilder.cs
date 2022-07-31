using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneBuilder : MonoBehaviour
{
    [Header("Dependency")]
    
    [SerializeField] private GameObject spawnPoints;

    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private GameObject deckCardPrefab;

    [SerializeField] private GameObject background;
    
    [SerializeField] private Vector3 deckCardSpawnPoint;

    [SerializeField] private List<Card> allCards;


    private List<Transform> _boardSpawnPoint = new List<Transform>();

    public List<CardHolder> _cardsInBoard = new List<CardHolder>();


    private void GetSpawnPositions()
    {
        List<CardSpawnPoint> cards = new List<CardSpawnPoint>(CardSpawnPoint.allCards);

        foreach (var card in cards)
        {
            _boardSpawnPoint.Add(card.gameObject.transform);
        }
    }

    private void BuildBoard()
    {
        for (int i = 0; i < _boardSpawnPoint.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab,gameObject.transform,this);

            newCard.transform.position = _boardSpawnPoint[i].position;
            
            _cardsInBoard.Add(newCard.GetComponent<CardHolder>());
        }
    }
    
    public void BuildGameScene()
    {
        Instantiate(spawnPoints,gameObject.transform,this);
        
        Instantiate(background,gameObject.transform,this);

        GameObject deckCardInstance = Instantiate(deckCardPrefab,gameObject.transform,this);

        deckCardInstance.transform.position = deckCardSpawnPoint;
        
        GetSpawnPositions();
        
        BuildBoard();
    }

    public void SetStarterDeck( List<int> cardOnboardId)
    {
        for (int i = 0; i < _cardsInBoard.Count; i++)
        {
            _cardsInBoard[i].card = allCards[cardOnboardId[i]];
            
            SpriteRenderer cardSprite = _cardsInBoard[i].GetComponent<SpriteRenderer>();

            cardSprite.sprite = allCards[cardOnboardId[i]].sprite;
        }
    }
}
