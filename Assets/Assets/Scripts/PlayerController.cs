using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{


    public GameObject prefabVirus;
    public GameObject prefabVacina;


    public GameObject playerArea;
    public GameObject HistoryArea;

    public GameObject enemyArea;
    public GameObject dropZone;


    [SyncVar] public List<GameObject> playerHand = new List<GameObject>();
    [SyncVar(hook = nameof(OnTeamChanged))]
    public string playerTeam;

    [SyncVar] public bool verificadorTime = false;

    [SyncVar] private int playerRecurso = 0;
    [SyncVar] private int enemyRecurso = 0;

    private TurnController turnManager;
    private GameController gameController;

    LobbyUIManager lobbyUIManager;

    GameUIManager gameUIManager;

    public string baseSelecionada = "Africa";

    private void InitializeGameObjects()
    {
        playerArea = GameObject.Find("PlayerArea");
        enemyArea = GameObject.Find("EnemyArea");
        HistoryArea = GameObject.Find("HistoryContent");

    }


    public override void OnStartServer()
    {
        base.OnStartServer();

        lobbyUIManager = FindObjectOfType<LobbyUIManager>();
        turnManager = FindObjectOfType<TurnController>();
        gameController = FindObjectOfType<GameController>();
        gameUIManager = FindObjectOfType<GameUIManager>();


    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        DontDestroyOnLoad(this);
        InitializeGameObjects();

        lobbyUIManager = FindObjectOfType<LobbyUIManager>();
        lobbyUIManager?.SetPlayerController(this);
        turnManager = FindObjectOfType<TurnController>();
        gameController = FindObjectOfType<GameController>();
        gameUIManager = FindObjectOfType<GameUIManager>();
        turnManager.InitializeGameController(gameController);
    }

    [Command]
    public void CmdDealCards()
    {
        GameObject prefab = playerTeam == "vacina" ? prefabVacina : prefabVirus;

        for (int i = 0; i < 5; i++)
        {
            int idCarta = DrawCard();

            Carta drawnCard = CartaDatabase.Instance.GetCartaById(idCarta);
            GameObject newCard = Instantiate(prefab, new Vector2(0, 0), Quaternion.identity);
            Card carta = newCard.GetComponent<Card>();
            carta.dadosCarta = drawnCard;
            carta.UpdateCard(drawnCard);
            NetworkServer.Spawn(newCard, connectionToClient);
            RpcShowCards(idCarta, newCard, "Dealt");


        }
    }

    [Command]
    public void CmdDealDeckBuildsCards()
    {
        GameObject prefab = playerTeam == "vacina" ? prefabVacina : prefabVirus;

        for (int i = 0; i < 5; i++)
        {
            int idCarta = DrawCard();
            
            Carta drawnCard = CartaDatabase.Instance.GetCartaById(idCarta);
            GameObject newCard = Instantiate(prefab, new Vector2(0, 0), Quaternion.identity);
            Card carta = newCard.GetComponent<Card>();
            carta.dadosCarta = drawnCard;
            carta.UpdateCard(drawnCard);
            NetworkServer.Spawn(newCard, connectionToClient);
            RpcShowCards(idCarta, newCard, "Dealt");


        }
    }

    [Command]
    public void CmdAlterandoBaseSelecionada(string baseSelecionada)
    {
        this.baseSelecionada = baseSelecionada;
    }

    int DrawCard()
    {
        Debug.Log("Etnrei no DrawCard");
        if (playerTeam == "vacina")
        {
            Debug.Log("Etnrei no Deck da Vacina");
            if (gameController.playerVacinaDeck.Count == 0)
            {
                Debug.Log("Etnrei no deck zero da vacina");

                // Reembaralhar descarte no deck
                gameController.playerVacinaDeck.AddRange(gameController.playerVacinaDiscarte);
                gameController.playerVacinaDiscarte.Clear();
                //ShuffleDeck(gameController.playerVacinaDeck);
            }

            int cardId = gameController.playerVacinaDeck[0];
            gameController.playerVacinaDeck.RemoveAt(0);// Remove a carta do topo
            RpcAtualizarBaralhos(gameController.playerVirusDeck, gameController.playerVacinaDeck, gameController.playerVacinaDiscarte, gameController.playerVirusDiscarte);
            return cardId;
        }
        else
        {
            Debug.Log("Etnrei no Deck da Virus");
            if (gameController.playerVirusDeck.Count == 0)
            {
                Debug.Log("Etnrei no deck zero da Virus");
                // Reembaralhar descarte no deck
                gameController.playerVirusDeck.AddRange(gameController.playerVirusDiscarte);
                gameController.playerVirusDiscarte.Clear();
                //ShuffleDeck(gameController.playerVirusDeck);
            }

            int cardId = gameController.playerVirusDeck[0];
            gameController.playerVirusDeck.RemoveAt(0);
            RpcAtualizarBaralhos(gameController.playerVirusDeck, gameController.playerVacinaDeck, gameController.playerVacinaDiscarte, gameController.playerVirusDiscarte);
            return cardId;
        }
    }

    [ClientRpc]
    public void RpcAtualizarBaralhos(List<int> VirusDeck, List<int> VacinaDeck, List<int> VacinaDiscarte, List<int> VirusDiscarte)
    {
        gameController.playerVacinaDeck = VacinaDeck;
        gameController.playerVirusDeck = VirusDeck;
        gameController.playerVacinaDiscarte = VacinaDiscarte;
        gameController.playerVirusDiscarte = VirusDiscarte;

        AtualizarUIBaralhos();
    }

    List<int> ShuffleDeck(List<int> deck)
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (deck[randomIndex], deck[i]) = (deck[i], deck[randomIndex]);
        }
        return deck;
    }


    private void DiscardHand()
    {
        foreach (var card in playerHand)
        {
            CmdDiscardCardHand(card);
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
            return false;
        }
    }

    [Command]
    public void CmdDiscardCard(GameObject card)
    {
        Debug.Log(card == null);
        Debug.Log("Etnrei no CmdDiscardCard");
        int cartaId = card.GetComponent<Card>().dadosCarta.id;
        if (playerTeam == "virus")
        {
            gameController.playerVirusDiscarte.Add(cartaId);
        }
        else
        {
            gameController.playerVacinaDiscarte.Add(cartaId);
        }
        RpcDiscardCard(card);
    }
     [Command]
    public void CmdDiscardCardHand(GameObject card)
    {
        Debug.Log(card == null);
        Debug.Log("Etnrei no CmdDiscardCard");
        int cartaId = card.GetComponent<Card>().dadosCarta.id;
        if (playerTeam == "virus")
        {
            gameController.playerVirusDiscarte.Add(cartaId);
        }
        else
        {
            gameController.playerVacinaDiscarte.Add(cartaId);
        }
        RpcDiscardCardHand(card);
    }
    
    [ClientRpc]
    public void RpcDiscardCard(GameObject card)
    {
        
        if (HistoryArea == null)
        {
            Debug.LogWarning("HistoryArea não foi encontrado, inicializando novamente.");
            HistoryArea = GameObject.Find("History");
        }
        card.transform.SetParent(HistoryArea.transform, false);

        Debug.Log("Entrei no metodo RPC de discartar uma carta");
        if (isOwned)
        {
            playerHand.Remove(card);
        }
        

        int cartaId = card.GetComponent<Card>().dadosCarta.id;
        if (playerTeam == "virus")
        {
            gameController.playerVirusDiscarte.Add(cartaId);
        }
        else
        {
            gameController.playerVacinaDiscarte.Add(cartaId);
        }

        AtualizarUIBaralhos();
    }
[ClientRpc]
    public void RpcDiscardCardHand(GameObject card)
    {
        Destroy(card);
        card.transform.SetParent(HistoryArea.transform, false);

        Debug.Log("Entrei no metodo RPC de discartar uma carta");
        if (isOwned)
        {
            playerHand.Remove(card);
        }
        

        int cartaId = card.GetComponent<Card>().dadosCarta.id;
        if (playerTeam == "virus")
        {
            gameController.playerVirusDiscarte.Add(cartaId);
        }
        else
        {
            gameController.playerVacinaDiscarte.Add(cartaId);
        }

        AtualizarUIBaralhos();
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
        Card cardComponent = card.GetComponent<Card>();
        Carta carta = cardComponent.dadosCarta;
        foreach (var idEfeito in carta.efeitos)
        {
            CartaEfeito efeito = EfeitosDatabase.Instance.GetCartaById(idEfeito);
            efeito.ApplyEffect(this, gameController);
        }
        Debug.Log(card);
        Debug.Log("Carta Ativada e adicionada ao Descarte.");
    }
    // [Command]
    // void CmdDestroyCard(GameObject card)
    // {
    //     Destroy(card);
    // }

    [ClientRpc]
    void RpcShowCards(int idCarta, GameObject card, string type)
    {
        Carta drawnCard = CartaDatabase.Instance.GetCartaById(idCarta);
        card.GetComponent<Card>().dadosCarta = drawnCard;
        card.GetComponent<Card>().UpdateCard(drawnCard);
        while (playerArea == null || enemyArea == null)
        {
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
        FindObjectOfType<LobbyUIManager>().UpdateTeamHighlight();
    }

    public void EndTurn()
    {
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
            turnManager.RpcEndCurrentTurn();
        }
    }


    public void StartGame()
    {
        CmdStartGame();
    }

    [Command]
    public void CmdStartGame()
    {

        if (turnManager != null)
        {
            gameController.StartGame();
        }
    }

    void OnAtributosVirusChanged(Virus oldAtributtes, Virus newAtributtes)
    {
        //CmdAtualizarBases(newAtributtes);
    }

    void OnAtributosCuraChanged(Cura oldAtributtes, Cura newAtributtes)
    {

    }

    [ClientRpc]
    void RpcAtualizarUIBaralhos()
    {
        Debug.Log("Entrei no metodo RPC de atualizar UI Baralhos");

        AtualizarUIBaralhos();
    }
    public void AtualizarUIBaralhos()
    {
        Debug.Log("Entrei no metodo local de atualizar UI Baralhos");
        gameUIManager = FindObjectOfType<GameUIManager>();
        gameController = FindObjectOfType<GameController>();
        int playerDeckCount, playerDiscarteCount, enemyDiscarteCount, enemyDeckCount;
        if (isOwned)
        {
            Debug.Log("Entrei Dentro dos IsOwned do atualizarBaralho");

            if (playerTeam == "virus")
            {
                playerDeckCount = gameController.playerVirusDeck.Count;
                playerDiscarteCount = gameController.playerVirusDiscarte.Count;
                enemyDeckCount = gameController.playerVacinaDeck.Count;
                enemyDiscarteCount = gameController.playerVacinaDiscarte.Count;
            }
            else
            {
                playerDeckCount = gameController.playerVacinaDeck.Count;
                playerDiscarteCount = gameController.playerVacinaDiscarte.Count;
                enemyDeckCount = gameController.playerVirusDeck.Count;
                enemyDiscarteCount = gameController.playerVirusDiscarte.Count;
            }
        }
        else
        {
            Debug.Log("Entrei Dentro do else do  IsOwned do atualizarBaralho");
            if (playerTeam == "virus")
            {
                playerDeckCount = gameController.playerVacinaDeck.Count;
                playerDiscarteCount = gameController.playerVacinaDiscarte.Count;
                enemyDeckCount = gameController.playerVirusDeck.Count;
                enemyDiscarteCount = gameController.playerVirusDiscarte.Count;

            }
            else
            {
                playerDeckCount = gameController.playerVirusDeck.Count;
                playerDiscarteCount = gameController.playerVirusDiscarte.Count;
                enemyDeckCount = gameController.playerVacinaDeck.Count;
                enemyDiscarteCount = gameController.playerVacinaDiscarte.Count;
            }
        }
        gameUIManager.UpdateUI(playerDeckCount, playerDiscarteCount, enemyDeckCount, enemyDiscarteCount);

    }


}
