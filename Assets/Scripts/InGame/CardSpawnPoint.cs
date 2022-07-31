using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnPoint : MonoBehaviour,IStaticable
{

    public static readonly List<CardSpawnPoint> allCards = new List<CardSpawnPoint>();
    public void Awake()
    {
        allCards.Add(this);
    }

    public void OnDestroy()
    {
        allCards.Remove(this);
    }
}
