using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;

public class Player : NetworkBehaviour
{
    public GameObject cartaVirus;
    public GameObject cartaVacina;
    public GameObject playerArea;
    public GameObject enemyArea;
    public GameObject dropZone;

    List<GameObject> cards = new List<GameObject>();

    public override void OnStartClient()
    {
        playerArea = GameObject.Find("PlayerArea");
        enemyArea = GameObject.Find("EnemyArea");
        dropZone = GameObject.Find("DropZone");
        base.OnStartClient();
    }

    [Server]
    public override void OnStartServer()
    {
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
            GameObject newCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector2(0, 0), Quaternion.identity);
            NetworkServer.Spawn(newCard, connectionToClient);
            RpcShowCards(newCard,"Dealt");
        }
    }
    [ClientRpc]
    void RpcShowCards(GameObject card, string type)
    {
        if (type == "Dealt")
        {
            if (isOwned)
            {
                card.transform.SetParent(playerArea.transform,false);
            }
            else
            {
                card.transform.SetParent(enemyArea.transform,false);
            }
        }
    }
}
