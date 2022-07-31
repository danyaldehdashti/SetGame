using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetNetworkManager : NetworkManager
{
    
    public List<int> playersId = new List<int>();

    public Dictionary<int, NetworkConnection> inGameIdToNetworkConnections = new Dictionary<int, NetworkConnection>();
    

    private Lobby _lobby;

    private int _uniqueId = 0;
    
    private string _lobbyScene = "Lobby";
    
    private string _gameScene = "Game";
    
    private bool _isGameInProgress = false;
    

    
    [SerializeField] private Lobby lobbyPrefab;
    

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        
        AddNewPlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        PlayerHandler playerHandler = conn.identity.GetComponent<PlayerHandler>();

        playersId.Remove(playerHandler.inGameId);

        inGameIdToNetworkConnections.Remove(playerHandler.inGameId);

        PlayerData playerData = new PlayerData();

        playerData.inGameId = playerHandler.inGameId;
        
        _lobby.players.Remove(playerData);
        
        base.OnServerDisconnect(conn);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        
        if (SceneManager.GetActiveScene().name.StartsWith(_lobbyScene))
        {
            SpawnLobby();
        }
        else if(SceneManager.GetActiveScene().name.StartsWith(_gameScene))
        {
            
        }
    }
    
    public void StartGame()
    {
        _isGameInProgress = true;
        
        ServerChangeScene(_gameScene);

        for (int i = 0; i < inGameIdToNetworkConnections.Count; i++)
        {
            inGameIdToNetworkConnections[playersId[i]].identity.GetComponent<PlayerHandler>().SetGameInProgress(true);
        }
    }

    private void AddNewPlayer(NetworkConnection connection)
    {
        PlayerHandler playerHandler = connection.identity.GetComponent<PlayerHandler>();
        
        playerHandler.SetPlayerId(GetNewPlayerId());
        
        playersId.Add(playerHandler.inGameId);
        
        inGameIdToNetworkConnections.Add(playerHandler.inGameId,connection);

        if (playersId.Count == 1)
        {
            playerHandler.SetIsPartyOwner(true);
        }

        PlayerData playerData = new PlayerData();

        playerData.inGameId = playerHandler.inGameId;
        
        _lobby.players.Add(playerData);
    }

    private int GetNewPlayerId()
    {
        _uniqueId++;

        return _uniqueId;
    }

    private void SpawnLobby()
    {
        GameObject lobbyInstance = Instantiate(lobbyPrefab.gameObject);

        _lobby = lobbyInstance.GetComponent<Lobby>();
        
        NetworkServer.Spawn(lobbyInstance);
    }
    
}
