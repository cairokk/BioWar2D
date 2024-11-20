using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//[CreateAssetMenu(fileName = "DadosVirus", menuName = "Atributos do Virus")]
public class Virus : NetworkBehaviour
{
    [SyncVar] public int taxaDeInfeccao = 1;
    [SyncVar] public int taxaDeMortalidade = 1;



}
