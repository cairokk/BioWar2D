using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameController : NetworkBehaviour
{
    // Lista de objetos do lobby e do jogo
    private List<GameObject> lobbyComponents;
    private TurnController turnController;

    [SerializeField] [SyncVar]
    public Virus atributosVirus;

    [SerializeField]
    [SyncVar]
    public Cura atributosCura;

    private int populacaoMorta = 0;

    private List<PlayerController> players = new();

    public List<BaseController> bases = new();

    void Start()
    {
        // Coleta todos os objetos do lobby e do jogo usando as tags
        lobbyComponents = new List<GameObject>(GameObject.FindGameObjectsWithTag("Lobby"));

        turnController = FindObjectOfType<TurnController>();
    }

    public void StartGame()
    {
        players.AddRange(FindObjectsOfType<PlayerController>());
        bases.AddRange(FindObjectsOfType<BaseController>());
        IniciarRegioes();
        turnController.InitializeGameController(this);
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

    public void OnAtributosVirusChanged(Virus novoVirus)
    {
        Debug.Log("entrei no Hook do virus");
        RpcAtualizarBases(novoVirus);
        RpcAtualizarAtributosCura(novoVirus);
    }

    public void OnAtributosCuraChanged(Cura novaCura)
    {

    }

    [Command]
    public void CmdAtualizarBases(Virus virus)
    {
        RpcAtualizarBases(virus);
    }

    [ClientRpc]
    public void RpcAtualizarBases(Virus virus)
    {
        Debug.Log("entrei no RPC");
        foreach (var componente in bases)
        {
            Debug.Log("entrei no FOR");
            componente.regiao.CalcularDanoFuturo(virus);
            componente.RpcUpdateUI();
        }
    }

    [ClientRpc]
    public void RpcAtualizarAtributosCura(Virus virus)
    {
        atributosCura.calcularFatorDeUrgencia(virus,populacaoMorta);
    }

    void IniciarRegioes(){
        foreach (var componente in bases)
        {
            componente.RpcUpdateUI();
        }
    }



}
