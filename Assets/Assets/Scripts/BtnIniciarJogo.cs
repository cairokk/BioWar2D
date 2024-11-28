using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;


public class BtnIniciarJogo : NetworkBehaviour
{
    
    public PlayerController player;
    public void OnClick(){
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        player = networkIdentity.GetComponent<PlayerController>();
        player.StartGame();
        Debug.Log(player == null);
    }

}
