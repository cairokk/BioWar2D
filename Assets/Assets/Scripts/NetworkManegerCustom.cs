using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System.IO;

public class NetworkManegerCustom : NetworkManager
{
    private int vacinaCount = 0;
    private int virusCount = 0;
    public GameObject painelJogo;
    public GameObject painelLobby;

    public override void OnStartServer()
    {
        base.OnStartServer();

    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            return;
        }

        GameObject playerObject = Instantiate(playerPrefab);
        PlayerController player = playerObject.GetComponent<PlayerController>();

        // Atribui o time com base na contagem de jogadores
        if (vacinaCount <= virusCount)
        {
            player.playerTeam = "vacina";
            vacinaCount++;
        }
        else
        {
            player.playerTeam = "virus";
            virusCount++;
        }
        NetworkServer.AddPlayerForConnection(conn, playerObject);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        // Atualiza as contagens ao desconectar
        var player = conn.identity.GetComponent<PlayerController>();
        if (player != null)
        {
            if (player.playerTeam == "vacina")
                vacinaCount--;
            else if (player.playerTeam == "virus")
                virusCount--;
        }

        base.OnServerDisconnect(conn);
    }
}

