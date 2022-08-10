using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetNetworkManager : NetworkManager
{
    
    public List<int> playersId = new List<int>();

    public readonly Dictionary<int, NetworkConnection> inGameIdToNetworkConnections = new Dictionary<int, NetworkConnection>();
    

    private Lobby _lobby;

    private CardShuffle _cardShuffle;

    private int _uniqueId = 0;
    
    private string _lobbyScene = "Lobby";
    
    private string _gameScene = "Game";
    

    
    [SerializeField] private Lobby lobbyPrefab;

    [SerializeField] private CardShuffle cardShufflePrefab;

    [SerializeField] private GameManager gameManagerPrefab;
     

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

        _lobby.RemovePlayer(playerHandler.inGameId);
            
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
            SpawnCardShuffle();
            SpawnGameManager();
        }
    }
    
    public void StartGame()
    {
        ServerChangeScene(_gameScene);
        
        for (int i = 0; i < inGameIdToNetworkConnections.Count; i++)
        {
            inGameIdToNetworkConnections[playersId[i]].identity.GetComponent<PlayerInformation>().SetGameInProgress(true);
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
    
    private void SpawnCardShuffle()
    {
        GameObject cardShuffleInstance = Instantiate(cardShufflePrefab.gameObject);

        _cardShuffle = cardShuffleInstance.GetComponent<CardShuffle>();
        
        NetworkServer.Spawn(cardShuffleInstance);
    }
    
    private void SpawnGameManager()
    {
        GameObject gameManagerInstance = Instantiate(gameManagerPrefab.gameObject);
        
        NetworkServer.Spawn(gameManagerInstance);
    }
    
}
