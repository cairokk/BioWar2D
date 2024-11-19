using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBehaviour
{


    public GameObject cartaVirus;
    public GameObject cartaVacina;



    public GameObject playerArea;
    public GameObject enemyArea;
    public GameObject dropZone;

    [SyncVar] public Virus virus;
    [SyncVar] public Cura Cura;

    public Deck deckVirus;
    public Deck deckVacina;
    [SyncVar] public List<Card> playerDeck = new();
    [SyncVar] public List<Card> playerDiscarte = new();
    [SyncVar] public List<GameObject> playerHand = new List<GameObject>();
    [SyncVar] public List<Card> enemyDeck = new();
    [SyncVar] public List<Card> enemyDiscarte = new();

    [SyncVar(hook = nameof(OnTeamChanged))]
    public string playerTeam;
    [SyncVar] public bool verificadorTime = false;

    [SyncVar] private int playerRecurso = 0;
    [SyncVar] private int enemyRecurso = 0;

    private TurnController turnManager;
    private GameController gameController;

    LobbyUIManager lobbyUIManager;

    private bool isGameSceneLoaded = false;

    private void InitializeGameObjects()
    {
        playerArea = GameObject.Find("PlayerArea");
        enemyArea = GameObject.Find("EnemyArea");
        // Verifique se os objetos foram encontrados
        if (playerArea == null || enemyArea == null)
        {
            Debug.LogError("Áreas do jogo não foram encontradas na cena do jogo.");
            return;
        }
    }


    public override void OnStartServer()
    {
        base.OnStartServer();

        lobbyUIManager = FindObjectOfType<LobbyUIManager>();
        turnManager = FindObjectOfType<TurnController>();
        gameController = FindObjectOfType<GameController>();

    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        DontDestroyOnLoad(this);
        InitializeGameObjects();
        lobbyUIManager = FindObjectOfType<LobbyUIManager>();
        turnManager = FindObjectOfType<TurnController>();
        gameController = FindObjectOfType<GameController>();
        lobbyUIManager?.SetPlayerController(this);
    }

    [Command]
    public void CmdDealCards()
    {

        for (int i = 0; i < 5; i++)
        {
            if (playerTeam == "vacina")
            {
                Debug.Log("Entrou na Vacina");
                Carta drawnCard = deckVacina.initialDeck[0];
                GameObject newCard = Instantiate(cartaVacina, new Vector2(0, 0), Quaternion.identity);
                Card carta = newCard.GetComponent<Card>();
                carta.UpdateCard(drawnCard);
                NetworkServer.Spawn(newCard, connectionToClient);
                RpcShowCards(newCard, "Dealt");
            }

            if (playerTeam == "virus")
            {
                Debug.Log("Entrou aqui no virus");
                Carta drawnCard = deckVirus.initialDeck[0];
                GameObject newCard = Instantiate(cartaVirus, new Vector2(0, 0), Quaternion.identity);
                Card carta = newCard.GetComponent<Card>();
                carta.UpdateCard(drawnCard);
                NetworkServer.Spawn(newCard, connectionToClient);
                RpcShowCards(newCard, "Dealt");
            }
        }
    }

    [Command]
    public void CmdDiscardCard(GameObject card)
    {
        RpcDiscardCard(card);
    }

    [ClientRpc]
    public void RpcDiscardCard(GameObject card)
    {
        Destroy(card);

        if (isOwned)
        {
            playerHand.Remove(card);
            playerDiscarte.Add(card.GetComponent<Card>());
            Destroy(card);
        }
        else
        {
            enemyDiscarte.Add(card.GetComponent<Card>());
            Destroy(card);
        }
    }
    private void DiscardHand()
    {
        Debug.Log("entrei no discardHand");
        foreach (var card in playerHand)
        {
            CmdDiscardCard(card);
        }
        playerHand.Clear(); // Limpa a mão do jogador
    }

    public bool PlayCard(GameObject card)
    {
        if (turnManager != null && IsMyTurn())
        {
            CmdPlayCard(card);
            CmdDiscardCard(card);
            return true;
        }
        else
        {
            Debug.Log("Não é o seu turno.");
            return false;
        }
    }

    public bool IsMyTurn()
    {
        // Verifica se o turno atual corresponde ao time do jogador
        if (turnManager.currentTurn == TurnController.TurnState.TurnoVirus && playerTeam == "virus")
        {
            return true;
        }
        else if (turnManager.currentTurn == TurnController.TurnState.TurnoCura && playerTeam == "vacina")
        {
            return true;
        }
        return false;
    }

    [Command]
    void CmdPlayCard(GameObject card)
    {
        Debug.Log(card);
        Debug.Log("Carta Ativada e adicionada ao Descarte.");
    }
    // [Command]
    // void CmdDestroyCard(GameObject card)
    // {
    //     Destroy(card);
    // }

    [ClientRpc]
    void RpcShowCards(GameObject card, string type)
    {

        while (playerArea == null || enemyArea == null)
        {
            Debug.LogWarning("Áreas do jogo ainda não foram inicializadas.");
            InitializeGameObjects();  // Tenta inicializar novamente, caso não tenha sido feito
        }

        if (type == "Dealt")
        {
            if (isOwned)
            {
                playerHand.Add(card);
                card.transform.SetParent(playerArea.transform, false);
            }
            else
            {
                card.GetComponent<CardFlipper>().Flip();
                card.transform.SetParent(enemyArea.transform, false);
            }
        }
    }


    [Command]
    public void CmdSwitchTeamsForAll()
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<PlayerController>();
                if (player != null)
                {
                    // Alterna o time de cada jogador
                    player.playerTeam = (player.playerTeam == "vacina") ? "virus" : "vacina";
                }
            }
        }
    }
    void OnTeamChanged(string oldTeam, string newTeam)
    {
        // Atualiza o destaque na UI para o novo time
        FindObjectOfType<LobbyUIManager>().UpdateTeamHighlight();
    }

    public void EndTurn()
    {
        Debug.Log("Player clicou no fim de turno");
        Debug.Log("É o turno do player?");
        Debug.Log(IsMyTurn());
        if (turnManager != null && IsMyTurn())
        {
            DiscardHand();
            CmdDealCards();
            CmdEndTurn();
        }
    }

    [Command]
    public void CmdEndTurn()
    {
        turnManager = FindObjectOfType<TurnController>();

        if (turnManager != null)
        {
            Debug.Log("o turnManager nao é nulo na hora de encerrar o turno");
            turnManager.RpcEndCurrentTurn();
        }
    }


    public void StartGame()
    {
        Debug.Log("entrei no StartGame dentro da PlayerController");
        CmdStartGame();
    }

    [Command]
    public void CmdStartGame()
    {
        Debug.Log("entrei no CmdStartGame dentro da PlayerController");
        Debug.Log(turnManager == null);
        if (turnManager != null)
        {
            gameController.StartGame();
        }
    }

}
