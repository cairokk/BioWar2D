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
    [SyncVar] public List<Card> enemyDeck = new();
    [SyncVar] public List<Card> enemyDiscarte = new();

    [SyncVar(hook = nameof(OnTeamChanged))]
    public string playerTeam;
    [SyncVar] public bool verificadorTime = false;

    [SyncVar] private int playerRecurso = 0;
    [SyncVar] private int enemyRecurso = 0;

    LobbyUIManager lobbyUIManager;

    private bool isGameSceneLoaded = false;

    private void Awake()
    {
        // Adiciona um evento para detectar o carregamento da cena do jogo
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Remove o evento ao destruir o objeto para evitar referência inválida
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verifica se é a cena do jogo
        if (scene.name == "Jogo")
        {
            isGameSceneLoaded = true;
            InitializeGameObjects();
        }
    }
    private void InitializeGameObjects()
    {
        // Inicializa as áreas do jogo
        playerArea = GameObject.Find("PlayerArea");
        enemyArea = GameObject.Find("EnemyArea");
        // Verifique se os objetos foram encontrados
        if (playerArea == null || enemyArea == null)
        {
            Debug.LogError("Áreas do jogo não foram encontradas na cena do jogo.");
            return;
        }

        // Inicialização do deck ou outras configurações específicas do jogo
        Debug.Log("Jogo inicializado para " + playerTeam);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        DontDestroyOnLoad(gameObject);
        lobbyUIManager = FindObjectOfType<LobbyUIManager>();
        if (lobbyUIManager != null)
        {
            lobbyUIManager.SetPlayerController(this);
        }
    }

    [Command]
    public void CmdDealCards()
    {
        for (int i = 0; i < 5; i++)
        {
            if (playerTeam == "vacina")
            {
                Carta drawnCard = deckVacina.initialDeck[0];
                GameObject newCard = Instantiate(cartaVacina, new Vector2(0, 0), Quaternion.identity);
                Card carta = newCard.GetComponent<Card>();
                carta.UpdateCard(drawnCard);
                NetworkServer.Spawn(newCard, connectionToClient);
                RpcShowCards(newCard, "Dealt");
            }

            if (playerTeam == "virus")
            {
                Carta drawnCard = deckVirus.initialDeck[0];
                GameObject newCard = Instantiate(cartaVirus, new Vector2(0, 0), Quaternion.identity);
                Card carta = newCard.GetComponent<Card>();
                carta.UpdateCard(drawnCard);
                NetworkServer.Spawn(newCard, connectionToClient);
                RpcShowCards(newCard, "Dealt");
            }
        }
    }

    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
    }

    [Command]
    void CmdPlayCard(GameObject card)
    {
        Debug.Log(card);
        Debug.Log("Carta Ativada e adicionada ao Descarte.");
        Destroy(card);
    }
    [ClientRpc]
    void RpcShowCards(GameObject card, string type)
    {

        if (playerArea == null || enemyArea == null)
        {
            Debug.LogWarning("Áreas do jogo ainda não foram inicializadas.");
            InitializeGameObjects();  // Tenta inicializar novamente, caso não tenha sido feito
        }
        if (type == "Dealt")
        {
            if (isOwned)
            {
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
}
