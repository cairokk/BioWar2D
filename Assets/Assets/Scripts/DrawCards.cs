using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;


public class DrawCards : NetworkBehaviour
{
    
    public PlayerController player;
    public void onClick(){
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        player = networkIdentity.GetComponent<PlayerController>();
        player.CmdDealCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
