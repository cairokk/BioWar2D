using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameController : NetworkBehaviour
{
    // Lista de objetos do lobby e do jogo
    private List<GameObject> lobbyComponents;
    private List<GameObject> gameComponents;

    void Start()
    {
        // Coleta todos os objetos do lobby e do jogo usando as tags
        lobbyComponents = new List<GameObject>(GameObject.FindGameObjectsWithTag("Lobby"));
        gameComponents = new List<GameObject>(GameObject.FindGameObjectsWithTag("Jogo"));

        // Garante que os componentes do Jogo estejam desativados no início
        //SetActiveComponents(gameComponents, false);
    }

    [ClientRpc]
    void RpcStartGame()
    {
        // Desativa os componentes do Lobby e ativa os do Jogo
        SetActiveComponents(lobbyComponents, false);
        //SetActiveComponents(gameComponents, true);
    }

    [Server]
    public void StartGame()
    {
        RpcStartGame();
    }

    private void SetActiveComponents(List<GameObject> components, bool isActive)
    {
        foreach (var component in components)
        {
            component.SetActive(isActive);
        }
    }

    public void OnStartGameButtonClicked()
    {
        if (isServer)
        {
            StartGame();

        }
    }
}
