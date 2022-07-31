using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneBulider : MonoBehaviour
{
    [Header("Dependency")]
    
    [SerializeField] private Vector3 deckCardSpawnPoint;
    
    private List<Transform> _boardSpawnPoint = new List<Transform>();

    
    private void Start()
    {
        GetSpawnPositions();
    }

    private void GetSpawnPositions()
    {
        List<CardHolder> cards = new List<CardHolder>(CardHolder.allCards);

        foreach (var card in cards)
        {
            _boardSpawnPoint.Add(card.gameObject.transform);
        }
    }
}
