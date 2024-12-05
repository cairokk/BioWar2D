using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{


    public GameObject prefabVirus;
    public GameObject prefabVacina;
    public GameObject prefabCardDeckBuildVacina;
    public GameObject prefabCardDeckBuildVirus;
    public GameObject playerArea;
    public GameObject HistoryArea;
    public GameObject enemyDeckBuildArea;
    public GameObject playerDeckBuildArea;
    public GameObject enemyArea;
    public GameObject dropZone;


    [SyncVar] public List<GameObject> playerHand = new List<GameObject>();
    [SyncVar(hook = nameof(OnTeamChanged))]
    public string playerTeam;

    [SyncVar] public bool verificadorTime = false;
    private string nome = "player";
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
        playerDeckBuildArea = GameObject.Find("PlayerDeckBuildContent");
        enemyDeckBuildArea = GameObject.Find("EnemyDeckBuildContent");

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
        if (playerTeam == "vacina")
            {
                gameController.playerVacinaDeckBuild = ShuffleDeck(gameController.playerVacinaDeckBuild);
            }
        else
            {
                gameController.playerVirusDeckBuild = ShuffleDeck(gameController.playerVirusDeckBuild);
            }
        
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
    public void CmdDealCardsToDeckBuild()
    {
        GameObject prefab = playerTeam == "vacina" ? prefabVacina : prefabVirus;
        

        for (int i = 0; i < 5; i++)
        {   

            int idCarta = DrawCardDeckBuild();
            Carta drawnCard = CartaDatabase.Instance.GetCartaById(idCarta);
            GameObject newCard = Instantiate(prefab, new Vector2(0, 0), Quaternion.identity);
            Card carta = newCard.GetComponent<Card>();
            Debug.Log("Pegando carta pro deckbuild id:" + idCarta );

            carta.dadosCarta = drawnCard;
            carta.UpdateCard(drawnCard);
            carta.isDeckbuildCard = true;
            carta.SetDraggable(false);
            NetworkServer.Spawn(newCard, connectionToClient);
            RpcShowCards(idCarta, newCard, "DealtToDeckBuild");


        }
    }

    [Command]
    public void CmdDealOneCardToDeckBuild()
    {
        Debug.Log("METODO CMDDEALONECARD ");

        GameObject prefab = playerTeam == "vacina" ? prefabVacina : prefabVirus;
        int idCarta = DrawCardDeckBuild();
        if (idCarta == -1) return; 

        Carta drawnCard = CartaDatabase.Instance.GetCartaById(idCarta);
        GameObject newCard = Instantiate(prefab, new Vector2(0, 0), Quaternion.identity);
        Card cardComponent = newCard.GetComponent<Card>();

        cardComponent.dadosCarta = drawnCard;
        cardComponent.UpdateCard(drawnCard);
        cardComponent.isDeckbuildCard = true;
        cardComponent.SetDraggable(false); 

        NetworkServer.Spawn(newCard, connectionToClient);
        RpcShowCards(idCarta, newCard, "DealtToDeckBuild");

         if (playerTeam == "vacina")
        {
            gameController.visibleCardsCura++;
        }
        else
        {
            gameController.visibleCardsVirus++;
        }
        
        RpcAtualizarDeckBuild(gameController.visibleCardsCura, gameController.visibleCardsVirus, gameController.playerVacinaDeckBuild, gameController.playerVirusDeckBuild);

    }
    public void StartTurnCheckDeckBuild()
    {
        Debug.Log(playerTeam);
        Debug.Log(playerTeam);
        Debug.Log(playerTeam);
        if (playerTeam == "virus")
        {
            while (gameController.visibleCardsVirus < 5)
            {
                CmdDealOneCardToDeckBuild();
                gameController.visibleCardsVirus++;
            }
        }
        else
        {
            while (gameController.visibleCardsCura < 5)
            {
                CmdDealOneCardToDeckBuild();
                gameController.visibleCardsCura++;

            }
        }
    }

    [ClientRpc]
    public void RpcSetDraggable(GameObject cardObject, bool draggable)
    {
        Card card = cardObject.GetComponent<Card>();
        card.SetDraggable(false);
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

    int DrawCardDeckBuild()
    {
        List<int> currentDeckBuild = playerTeam == "vacina"  ? gameController.playerVacinaDeckBuild : gameController.playerVirusDeckBuild;
        if (currentDeckBuild.Count == 0) return -1; // No cards to draw
        
        currentDeckBuild = ShuffleDeck(currentDeckBuild);
        int cardId = currentDeckBuild[0];
        currentDeckBuild.RemoveAt(0);

        if(playerTeam == "vacina"){
            RpcAtualizarDeckBuild(gameController.visibleCardsCura, gameController.visibleCardsVirus, currentDeckBuild, gameController.playerVirusDeckBuild);

        }else{
            RpcAtualizarDeckBuild(gameController.visibleCardsCura, gameController.visibleCardsVirus,  gameController.playerVacinaDeckBuild, currentDeckBuild);
        }

        
        
        return cardId;
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


    private void RecupearCartasAux(){
        foreach (var card in gameController.playerVirusDeckBuildAux){
            gameController.playerVirusDeckBuild.Add(card);
            gameController.playerVirusDeckBuildAux.Remove(card);
        }
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

         CardFlipper cardFlipper = card.GetComponent<CardFlipper>();
        if (cardFlipper != null)
        {
            cardFlipper.EnsureFaceUp();
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
         CardFlipper cardFlipper = card.GetComponent<CardFlipper>();
        if (cardFlipper != null)
        {
            cardFlipper.EnsureFaceUp();
        }
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
        Card cardComponent = card.GetComponent<Card>();

        cardComponent.dadosCarta = drawnCard;
        cardComponent.UpdateCard(drawnCard);
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
        if(type == "DealtToDeckBuild") 
        {
            cardComponent.SetDraggable(false); 
            cardComponent.isDeckbuildCard = true;

            if (isOwned)
            {   
                card.transform.SetParent(playerDeckBuildArea.transform, false);
            }
            else
            {
                card.transform.SetParent(enemyDeckBuildArea.transform, false);
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
            //RecoverDeckBuildCards();
            CmdDealCards();
            //ClearDeckBuild();
            //CmdDealCardsToDeckBuild();
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
    public void cardDeckBuildClick(GameObject card)
    {
        Card cardComponent = card.GetComponent<Card>();
        string tipoCarta = cardComponent.dadosCarta.id > 100 ? "vacina" : "virus";
        Debug.Log(cardComponent.dadosCarta.id);
        bool cartaOwner = tipoCarta == playerTeam ? true : false;
        if (!IsMyTurn()){
            Debug.LogError("Não é seu turno");
            return;
        }

        if (cardComponent == null)
        {
            Debug.LogError("null");
            return;
        }
        Debug.Log("DONO DA CARTA=---");
        Debug.Log(cartaOwner);
        int cardId = cardComponent.dadosCarta.id;

        if (!cartaOwner)
        {
            if(playerTeam == "vacina"){
                gameController.visibleCardsVirus--;
                gameController.playerVirusDeckBuild.Add(cardId);
            }else{
                gameController.visibleCardsCura--;
                gameController.playerVacinaDeckBuild.Add(cardId);

            }
            CmdReturnCardToOpponent(card, cardId,gameController.visibleCardsCura ,gameController.visibleCardsVirus ,gameController.playerVacinaDeckBuild ,gameController.playerVirusDeckBuild  );
            return;
        }
        if(playerTeam == "vacina"){
            gameController.visibleCardsCura--;

        }else{
            gameController.visibleCardsVirus--;

        }
        CmdAtualizarDeckBuild(gameController.visibleCardsCura ,gameController.visibleCardsVirus ,gameController.playerVacinaDeckBuild ,gameController.playerVirusDeckBuild  );


        int cardCost = cardComponent.dadosCarta.custo;
        int currentResource = GetCurrentResource();
        
        if (currentResource >= cardCost)
        {
            cardComponent.SetCardSaturation(false); // Restaura a cor normal
            CmdTransferToDiscard(card);
            CmdDealOneCardToDeckBuild();

        }
        else
        {
            Debug.Log("Sem recurso" + currentResource + " " +cardCost);
            cardComponent.SetCardSaturation(true); // Deixa a carta em tons de cinza

        }
    }

    [Command]
    private void CmdReturnCardToOpponent(GameObject card, int cardId, int visibleCardsCura, int visibleCardsVirus, List<int> VacinaDeckBuild,List<int> VirusDeckBuild)
    {
        Debug.Log(playerTeam);
        Debug.Log(playerTeam);
        Debug.Log(playerTeam);
        Debug.Log("----------------------");
        Debug.Log(playerTeam);
        Debug.Log(playerTeam);
        gameController.visibleCardsCura = visibleCardsCura;
        gameController.visibleCardsVirus = visibleCardsVirus;
        gameController.playerVacinaDeckBuild = VacinaDeckBuild;
        gameController.playerVirusDeckBuild = VirusDeckBuild;
        RpcAtualizarDeckBuild(visibleCardsCura, visibleCardsVirus, VacinaDeckBuild,VirusDeckBuild);
        RpcRemoveCardVisual(card);
    }
    [ClientRpc]
    public void RpcAtualizarDeckBuild(int visibleCardsCura, int visibleCardsVirus, List<int> VacinaDeckBuild,List<int> VirusDeckBuild){
        Debug.Log("ENTRANDO NO RPC ATUALZIARDDEBKUILD");
        Debug.Log(visibleCardsCura);
        Debug.Log(visibleCardsVirus);
        gameController.visibleCardsCura = visibleCardsCura;
        gameController.visibleCardsVirus = visibleCardsVirus;
        gameController.playerVacinaDeckBuild = VacinaDeckBuild;
        gameController.playerVacinaDeckBuild = VirusDeckBuild;
    }
    [Command]
    public void CmdAtualizarDeckBuild(int visibleCardsCura, int visibleCardsVirus, List<int> VacinaDeckBuild,List<int> VirusDeckBuild){
        Debug.Log("ENTRANDO NO Cmd ATUALZIARDDEBKUILD");
        Debug.Log(visibleCardsCura);
        Debug.Log(visibleCardsVirus);
        gameController.visibleCardsCura = visibleCardsCura;
        gameController.visibleCardsVirus = visibleCardsVirus;
        gameController.playerVacinaDeckBuild = VacinaDeckBuild;
        gameController.playerVacinaDeckBuild = VirusDeckBuild;

        RpcAtualizarDeckBuild(visibleCardsCura, visibleCardsVirus, VacinaDeckBuild,VirusDeckBuild);
    }
    

    [Command]
    private void CmdTransferToDiscard(GameObject card)
    {
        Debug.Log("Discard");
        Debug.Log("Discard");
        Debug.Log("Discard");
        Card cardComponent = card.GetComponent<Card>();
        if (cardComponent == null) return;

        int cardCost = cardComponent.dadosCarta.custo;

        if (playerTeam == "vacina")
        {
            var cura = FindObjectOfType<Cura>();
            if (cura != null) cura.recurso -= cardCost;
        }
        else if (playerTeam == "virus")
        {
            var virus = FindObjectOfType<Virus>();
            if (virus != null) virus.recurso -= cardCost;
        }

        int cartaId = cardComponent.dadosCarta.id;
        if (playerTeam == "virus")
        {
            gameController.playerVirusDiscarte.Add(cartaId);
        }
        else
        {
            gameController.playerVacinaDiscarte.Add(cartaId);
        }
        RpcTransferToDiscard(cartaId);
        RpcRemoveCardVisual(card);

    }
    [ClientRpc]
    public void RpcTransferToDiscard(int cartaId){
         if (playerTeam == "virus")
        {
            gameController.playerVirusDiscarte.Add(cartaId);
        }
        else
        {
            gameController.playerVacinaDiscarte.Add(cartaId);
        }

    }

    

    [ClientRpc]
    private void RpcRemoveCardVisual(GameObject card)
    {
        Debug.Log("DESTRUINDO");
        Debug.Log("DESTRUINDO");
        Debug.Log("DESTRUINDO");

        Destroy(card);
        AtualizarUIBaralhos();
    }

    

    private void RecoverDeckBuildCards()
{
    if (playerTeam == "virus")
    {
        foreach (var cardId in gameController.playerVirusDeckBuildAux)
        {
            gameController.playerVirusDeckBuild.Add(cardId);
        }
        gameController.playerVirusDeckBuildAux.Clear(); 
    }
    else
    {
        foreach (var cardId in gameController.playerVacinaDeckBuildAux)
        {
            gameController.playerVacinaDeckBuild.Add(cardId);
        }
        gameController.playerVacinaDeckBuildAux.Clear(); 
    }
}

    private int GetCurrentResource()
    {
        if (playerTeam == "vacina")
        {
            var cura = FindObjectOfType<Cura>();
            return cura != null ? cura.recurso : 0;
        }
        else if (playerTeam == "virus")
        {
            var virus = FindObjectOfType<Virus>();
            return virus != null ? virus.recurso : 0;
        }
        return 0; 
    }

    

}
