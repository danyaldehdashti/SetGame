using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneBuilder : MonoBehaviour
{
    [Header("Dependency")]
    
    [SerializeField] private GameObject spawnPoints;
    
    [SerializeField] private Vector3 deckCardSpawnPoint;

    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private GameObject deckCardPrefab;

    [SerializeField] private GameObject background;
    
    private List<Transform> _boardSpawnPoint = new List<Transform>();
    

    private void GetSpawnPositions()
    {
        List<CardHolder> cards = new List<CardHolder>(CardHolder.allCards);

        foreach (var card in cards)
        {
            _boardSpawnPoint.Add(card.gameObject.transform);
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

    private void BuildBoard()
    {
        Debug.Log(_boardSpawnPoint.Count);
        for (int i = 0; i < _boardSpawnPoint.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab,gameObject.transform,this);

            newCard.transform.position = _boardSpawnPoint[i].position;
        }
    }
}
