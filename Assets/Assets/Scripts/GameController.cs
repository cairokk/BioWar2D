using Mirror;
using UnityEngine;
using System.Collections.Generic;



public class GameController : NetworkBehaviour
{
    public PlayerController curePlayer;
    public PlayerController virusPlayer;

    public List<GameObject> bases;  

    // Método para retornar atributos específicos do time
    public int GetTeamAttribute(string team, string attribute)
    {
        if (team == "cura")
        {
            Debug.Log("testando Cura");
        }
        else if (team == "virus")
        {
            Debug.Log("testando Virus");
        }
        return 0;
    }
}
