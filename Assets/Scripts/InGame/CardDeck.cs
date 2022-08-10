using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;


    public void SetNumberOfDeckCard(int index)
    {
        tmpText.text = index.ToString("0");
    }
}
