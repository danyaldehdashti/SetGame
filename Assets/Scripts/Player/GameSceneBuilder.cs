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

    [SerializeField] private Vector3 playerDataPos;

    [SerializeField] private List<Card> allCards;
    
    [SerializeField] [Range(0f, 1f)] private float removedTimeLine;

    [SerializeField] [Range(0, 1f)] private float addTimeLine;



    private readonly List<Transform> _boardSpawnPoint = new List<Transform>();

    public List<CardHolder> cardsInBoard = new List<CardHolder>();
    
    private bool _isMove;


    private List<int> _newSpawnPoint = new List<int>();
    private List<GameObject> _removedCard = new List<GameObject>();
    private List<GameObject> _newCards = new List<GameObject>();


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

            newCard.GetComponent<CardHolder>().spawnPointIndex = i;
            
            cardsInBoard.Add(newCard.GetComponent<CardHolder>());
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
        for (int i = 0; i < cardsInBoard.Count; i++)
        {
            cardsInBoard[i].index = cardOnboardId[i];
            
            cardsInBoard[i].card = allCards[cardOnboardId[i]]; ;
            
            SpriteRenderer cardSprite = cardsInBoard[i].GetComponent<SpriteRenderer>();

            cardSprite.sprite = allCards[cardOnboardId[i]].sprite;
        }
    }

    public void RemoveAndSetNewItem(GameCommand command)
    {
        SetNewCard(command);

        _isMove = true;
        
        Invoke(nameof(OnEnableAnimations),3f);
    }

    private void SetNewCard(GameCommand command)
    {
        List<GameObject> removedCards = new List<GameObject>();

        List<GameObject> newCards = new List<GameObject>();

        foreach (var id in command.playerCardChose)
        {
            for (int j = 0; j < cardsInBoard.Count; j++)
            {
                if (id == cardsInBoard[j].index)
                {
                    removedCards.Add(cardsInBoard[j].gameObject);

                }  
            }
        }

        for (int i = 0; i < command.newCards.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab,gameObject.transform,this);

            newCard.GetComponent<CardHolder>().card = allCards[command.newCards[i]];

            newCard.GetComponent<CardHolder>().index = command.newCards[i];

            newCard.GetComponent<SpriteRenderer>().sprite = allCards[command.newCards[i]].sprite;

            newCard.transform.position = deckCardSpawnPoint;
            
            newCards.Add(newCard);

            cardsInBoard[command.spawnPointsPlayerCardChose[i]] = newCard.GetComponent<CardHolder>();
        }

        for (int i = 0; i < _boardSpawnPoint.Count; i++)
        {
            cardsInBoard[i].spawnPointIndex = i;
        }
        _removedCard = removedCards;
        _newCards = newCards;
        _newSpawnPoint = command.spawnPointsPlayerCardChose;
    }
    
    private void MoveCards(List<int> spawnPoint, List<GameObject> removedCard,List<GameObject> newCards)
    {
        for (int i = 0; i < removedCard.Count; i++)
        {
            var newPos = Vector3.Lerp(removedCard[i].transform.position,playerDataPos,removedTimeLine);

            removedCard[i].transform.position = newPos;
        }
        
        for (int i = 0; i < newCards.Count; i++)
        {

            var newPos = Vector3.Lerp(newCards[i].transform.position,_boardSpawnPoint[spawnPoint[i]].transform.position,addTimeLine);

            newCards[i].transform.position = newPos;
        }
    }
    
    private void Update()
    {
        if (_isMove)
        {
            MoveCards(_newSpawnPoint,_removedCard,_newCards);
        }
    }

    private void OnEnableAnimations()
    {
        _isMove = false;
        
        Debug.Log("Fuck");
    }
}
