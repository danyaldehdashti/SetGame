using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public Card card;
    
    public int index;

    public int spawnPointIndex;


    public void SendCardToPlayer()
    {
        NetworkClient.connection.identity.GetComponent<PlayerInformation>().GetCardChose(index,spawnPointIndex);
    }
}
