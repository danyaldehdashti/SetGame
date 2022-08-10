using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public Card card;

    public SpriteRenderer spriteRenderer;

    public int index;

    public int spawnPointIndex;


    public void SendCardToPlayer()
    {
        NetworkClient.connection.identity.GetComponent<PlayerInformation>().GetCardChose(index,spawnPointIndex);
        ActiveCard();
    }

    private void ActiveCard()
    {
        spriteRenderer.gameObject.SetActive(true);
        spriteRenderer.sprite = card.selectedSprite;
    }
}
