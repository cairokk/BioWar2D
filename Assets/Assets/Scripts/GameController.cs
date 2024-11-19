using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameController : NetworkBehaviour
{
    // Lista de objetos do lobby e do jogo
    private List<GameObject> lobbyComponents;
    private TurnController turnController;
    private List<PlayerController> players = new List<PlayerController>();

    void Start()
    {
        // Coleta todos os objetos do lobby e do jogo usando as tags
        lobbyComponents = new List<GameObject>(GameObject.FindGameObjectsWithTag("Lobby"));

        turnController = FindObjectOfType<TurnController>();
    }

    // [Command]
    // public void CmdRequestStartGame()
    // {
    //     if (isServer)
    //     {
    //         players.AddRange(FindObjectsOfType<PlayerController>());

    //         // Envia os jogadores para o TurnController e inicia o turno
    //         if (turnController != null)
    //         {
    //             turnController.InitializePlayers(players);

    //             turnController.StartTurn(TurnController.TurnState.TurnoVirus); // Começa com o turno do vírus
    //         }
    //     }

    //     RpcStartGame();
    // }
    public void StartGame()
    {
        players.AddRange(FindObjectsOfType<PlayerController>());
        Debug.Log("entrei no gameController");
        // Envia os jogadores para o TurnController e inicia o turno
        if (turnController != null)
        {
            turnController.InitializePlayers(players);

            turnController.StartTurn(TurnController.TurnState.TurnoVirus); // Começa com o turno do vírus
        }

        RpcStartGame();
    }
    [ClientRpc]
    void RpcStartGame()
    {
        Debug.Log("entrei no RpcStartGame");

        SetActiveComponents(lobbyComponents, false);
        foreach (var player in FindObjectsOfType<PlayerController>())
        {
            Debug.Log("entrei no laço de geração de carta");
            if (player.isLocalPlayer)
            {
                player.CmdDealCards();
            }
        }
    }
    private void SetActiveComponents(List<GameObject> components, bool isActive)
    {
        foreach (var component in components)
        {
            component.SetActive(isActive);
        }
    }

}
