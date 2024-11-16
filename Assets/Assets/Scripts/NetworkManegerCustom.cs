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

    //public string lobbySceneName = "Lobby";
    //public string gameSceneName = "Jogo";

    private string[] listaCenas;

    public override void OnStartServer()
    {
        base.OnStartServer();

       int quantCenas = SceneManager.sceneCountInBuildSettings - 2;
        listaCenas = new string[quantCenas];

        for (int i = 0; i < quantCenas; i++)
        {
            listaCenas[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i + 2));
        }

    }

    public override void OnStopServer()
    {
        StartCoroutine(UnloadScenes());
    }

    public override void OnStopClient()
    {
        if (mode == NetworkManagerMode.Offline)
            StartCoroutine(UnloadScenes());
    }

    IEnumerator LoadSubScenes()
    {
        Debug.Log("Loading Scenes");

        foreach (string sceneName in listaCenas)
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    IEnumerator UnloadScenes()
    {
        Debug.Log("Unloading Subscenes");

        foreach (string sceneName in listaCenas)
            if (SceneManager.GetSceneByName(sceneName).IsValid() || SceneManager.GetSceneByPath(sceneName).IsValid())
                yield return SceneManager.UnloadSceneAsync(sceneName);

        yield return Resources.UnloadUnusedAssets();
    }


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {

        if (conn.identity != null)
        {
            Debug.LogWarning("Já existe um jogador para essa conexão.");
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

    // Função para carregar o lobby
    public void StartLobby()
    {
        if (NetworkServer.active)
        {
            //ServerChangeScene(lobbySceneName);
        }
    }

    // Função para carregar a cena do jogo
    public void StartGame()
    {
        if (NetworkServer.active)
        {
            //ServerChangeScene(gameSceneName);
        }

    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        NetworkServer.SpawnObjects();


        NetworkIdentity[] allObjsWithNetworkIdentity = FindObjectsOfType<NetworkIdentity>();

        foreach (var item in allObjsWithNetworkIdentity)
        {
            item.enabled = true;
            Debug.Log(item.enabled);
        }
    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();

        // Reativa os objetos no cliente após o carregamento da cena.
        foreach (var netObj in FindObjectsOfType<NetworkIdentity>())
        {
            if (!netObj.gameObject.activeSelf)
            {
                Debug.Log("testando 2222uma coisa");
                netObj.gameObject.SetActive(true);
            }
        }

        NetworkServer.SpawnObjects();
    }

}

