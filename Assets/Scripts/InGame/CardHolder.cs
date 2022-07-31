using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour,IStaticable
{
    public static readonly List<CardHolder> allCards = new List<CardHolder>();
    public void Awake()
    {
        allCards.Add(this);
    }

    public void OnDestroy()
    {
        allCards.Remove(this);
    }
}
