using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using Mirror.BouncyCastle.Asn1.Cms;

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
    [SyncVar] public string playerTeam;
    [SyncVar] public bool verificadorTime = false;

    [SyncVar] private int playerRecurso = 0;
    [SyncVar] private int enemyRecurso = 0;


    List<GameObject> cards = new List<GameObject>();

    public override void OnStartClient()
    {
        if (!verificadorTime)
        {
            playerTeam = "vacina";
            CmdAtualizarVerificadorTime();
        }
        else
        {
            playerTeam = "virus";
            CmdAtualizarVerificadorTime();

        }
        playerArea = GameObject.Find("PlayerArea");
        enemyArea = GameObject.Find("EnemyArea");
        dropZone = GameObject.Find("DropZone");


        base.OnStartClient();


    }

    [Server]
    public override void OnStartServer()
    {

        if (!verificadorTime)
        {
            playerTeam = "vacina";
            CmdAtualizarVerificadorTime();
        }
        else
        {
            playerTeam = "virus";
            CmdAtualizarVerificadorTime();

        }
        cards.Add(cartaVacina);
        cards.Add(cartaVirus);
        Debug.Log(cards);
        base.OnStartServer();
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

            if (playerTeam == "Virus")
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

    [Command]
    void CmdAtualizarVerificadorTime(){
        RpcAtualizarVerificadorTime();
    }

    [ClientRpc]
    void RpcAtualizarVerificadorTime(){
        Debug.Log(verificadorTime);
        verificadorTime = !verificadorTime;
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
}
