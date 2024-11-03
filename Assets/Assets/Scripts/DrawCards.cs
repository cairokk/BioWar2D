using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;


public class DrawCards : NetworkBehaviour
{
    
    public Player player;
    public void onClick(){
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        player = networkIdentity.GetComponent<Player>();
        player.CmdDealCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
