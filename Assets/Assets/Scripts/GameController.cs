using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Collections;

public class GameController : NetworkBehaviour
{
    // Lista de objetos do lobby e do jogo
    private List<GameObject> lobbyComponents;
    private TurnController turnController;

    [SerializeField]
    [SyncVar]
    public Virus atributosVirus;

    [SerializeField]
    [SyncVar]
    public Cura atributosCura;

    private int populacaoMorta = 0;

    private List<PlayerController> players = new();

    public List<BaseController> bases = new();

    public List<int> playerVirusDeck = new();
    public List<int> playerVacinaDeck = new();

    public List<int> playerVirusDeckBuild = new();
    public List<int> playerVacinaDeckBuild = new();

    public List<int> playerVirusDeckBuildAux = new();
    public List<int> playerVacinaDeckBuildAux = new();
    public List<int> playerVirusDiscarte = new();
    public List<int> playerVacinaDiscarte = new();
    [SyncVar] public int visibleCardsVirus = 5; 
    [SyncVar] public int visibleCardsCura = 5; 
    public GameObject virusWinPanel;
    public GameObject curaWinPanel;
    public Deck deckVirus;
    public Deck deckVacina;

    public Deck deckBuildVirus;
    public Deck deckBuildVacina;


    public bool selecionandoBase = false;


    void Start()
    {
        // Coleta todos os objetos do lobby e do jogo usando as tags
        lobbyComponents = new List<GameObject>(GameObject.FindGameObjectsWithTag("Lobby"));

        turnController = FindObjectOfType<TurnController>();
        turnController.InitializeWinPanels(virusWinPanel, curaWinPanel);

    }

    [Server]
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

        playerVacinaDeck.AddRange(deckVacina.initialDeck);
        playerVirusDeck.AddRange(deckVirus.initialDeck);
        
        
        playerVacinaDeckBuild.AddRange(deckBuildVacina.initialDeck);
        playerVirusDeckBuild.AddRange(deckBuildVirus.initialDeck);


        RpcStartGame();
    }
    [ClientRpc]
    void RpcStartGame()
    {
        SetActiveComponents(lobbyComponents, false);
        bases.AddRange(FindObjectsOfType<BaseController>());
        playerVacinaDeck.AddRange(deckVacina.initialDeck);
        playerVirusDeck.AddRange(deckVirus.initialDeck);
             
        playerVacinaDeckBuild.AddRange(deckBuildVacina.initialDeck);
        playerVirusDeckBuild.AddRange(deckBuildVirus.initialDeck);


        foreach (var player in FindObjectsOfType<PlayerController>())
        {
            player.baseSelecionada = "Africa";
            player.AtualizarUIBaralhos();
            player.CmdDealCards();
            player.CmdDealCardsToDeckBuild();

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
        RpcAtualizarBases(novoVirus);
        RpcAtualizarAtributosCura(novoVirus);

    }

    [Command]
    public void CmdAtualizarBases(Virus virus)
    {
        RpcAtualizarBases(virus);
        RpcAtualizarAtributosCura(virus);
    }

    [ClientRpc]
    public void RpcAtualizarBases(Virus virus)
    {
        foreach (var componente in FindObjectsOfType<BaseController>())
        {

            componente.regiao.CalcularDanoFuturo(virus);
            componente.UpdateUI();
        }
    }

    [ClientRpc]
    public void RpcAtualizarAtributosCura(Virus virus)
    {
        atributosCura.CalcularFatorDeUrgencia(virus, populacaoMorta);
    }

    [ClientRpc]
    public void RpcAtualizarSelecionandoBase(bool valor)
    {
        selecionandoBase = valor;
    }

    void IniciarRegioes()
    {
        foreach (var componente in bases)
        {
            componente.RpcUpdateUI();
        }
    }

}
