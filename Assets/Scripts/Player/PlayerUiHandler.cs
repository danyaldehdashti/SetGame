using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUiHandler : MonoBehaviour
{
    [Header("Dependency")]
    
    [SerializeField] private GameObject playerCanvas;


    public void SpawnCanvas()
    {
        Instantiate(playerCanvas);
    }
}
