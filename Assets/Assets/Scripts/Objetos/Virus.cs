using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Virus : NetworkBehaviour
{
    [SyncVar] public int taxaDeInfeccao = 1;
    [SyncVar] public int taxaDeMortalidade = 1;

    [SyncVar] public int recurso = 0;



}
