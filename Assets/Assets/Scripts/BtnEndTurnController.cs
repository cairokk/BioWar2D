using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;


public class BtnEndTurnController : NetworkBehaviour
{
    
    public PlayerController player;
    public void OnClick(){
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        player = networkIdentity.GetComponent<PlayerController>();
        player.EndTurn();
        
    }

}
