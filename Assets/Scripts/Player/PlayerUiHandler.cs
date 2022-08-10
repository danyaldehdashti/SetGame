using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class PlayerUiHandler : MonoBehaviour
{
    [Header("Dependency")]
    
    [SerializeField] private GameObject playerCanvasPrefab;


    private PlayerCanvas _playerCanvas;

    public void SpawnCanvas()
    {
        GameObject instance =  Instantiate(playerCanvasPrefab);

        _playerCanvas = instance.GetComponent<PlayerCanvas>();
    }

    public void UpdateTimer(float value)
    {
        _playerCanvas.SetTimer(value);
    }

    public void UpdateBullets( int count)
    {
        _playerCanvas.SetBullets(count);
    }

    public void UpdateScoreBoard(List<PlayerData> playersData)
    {
        PlayerHandler playerHandler = GetComponent<PlayerHandler>();
        
        
        _playerCanvas.SetScoreBoard(playersData,playerHandler.inGameId);
    }
}
