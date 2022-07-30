using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetNetworkManager : NetworkManager
{
    #region Private_Varibels
    
    private List<NetworkConnection> playersList = new List<NetworkConnection>();

    private Dictionary<int, NetworkConnection> playersDictionary = new Dictionary<int, NetworkConnection>();

    private Lobby _lobby;

    private int _numberOfPlayersInRoom = 0;
    
    private string _lobbyScene = "Lobby";
    
    #endregion

    
    [SerializeField] private GameObject lobbyPrefab;
    

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        
        AddNewPlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        PlayerHandler playerHandler = conn.identity.GetComponent<PlayerHandler>();

        playersList.Remove(conn);

        playersDictionary.Remove(playerHandler.inGameId);

        _numberOfPlayersInRoom--;

        _lobby.players.RemoveAt(_numberOfPlayersInRoom);
        
        base.OnServerDisconnect(conn);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if (SceneManager.GetActiveScene().name.StartsWith(_lobbyScene));
        {
            SpawnLobby();
        }
    }

    public void StartGame()
    {
        
    }

    private void AddNewPlayer(NetworkConnection connection)
    {
        PlayerHandler playerHandler = connection.identity.GetComponent<PlayerHandler>();
        
        playerHandler.SetPlayerId(GetNewPlayerId());
        
        playersList.Add(connection);
        
        playersDictionary.Add(playerHandler.inGameId,connection);

        if (playersList.Count == 1)
        {
            playerHandler.SetIsPartyOwner(true);
        }

        PlayerDeta playerDeta = new PlayerDeta();

        playerDeta.inGameId = playerHandler.inGameId;
        
        _lobby.players.Add(playerDeta);
    }

    private int GetNewPlayerId()
    {
        _numberOfPlayersInRoom++;

        return _numberOfPlayersInRoom;
    }

    private void SpawnLobby()
    {
        GameObject lobbyInstance = Instantiate(lobbyPrefab);

        _lobby = lobbyInstance.GetComponent<Lobby>();
        
        NetworkServer.Spawn(lobbyInstance);
    }
    
}
