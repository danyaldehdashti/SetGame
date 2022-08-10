using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private List<PlayerData> _playersData = new List<PlayerData>();


    public void DelayToConvertNetworkConnectionToPlayerData()
    {
        Invoke(nameof(ConvertNetworkConnectionToPlayerData),0.5F);
    }

    private void ConvertNetworkConnectionToPlayerData()
    {
        _playersData.Clear();
        
        List<NetworkConnection> networkConnections =
            new List<NetworkConnection>(((SetNetworkManager)NetworkManager.singleton).inGameIdToNetworkConnections
                .Values);

        foreach (var net in networkConnections)
        {
            PlayerHandler playerHandler = net.identity.GetComponent<PlayerHandler>();
            PlayerInformation playerInformation = net.identity.GetComponent<PlayerInformation>();

            PlayerData playerData = new PlayerData();

            playerData.inGameId = playerHandler.inGameId;
            playerData.score = playerInformation.score;

            _playersData.Add(playerData);
        }


        foreach (var network in networkConnections)
        {
            network.identity.GetComponent<PlayerInformation>().TargetGetPlayersData(_playersData);
        }
    }
}
