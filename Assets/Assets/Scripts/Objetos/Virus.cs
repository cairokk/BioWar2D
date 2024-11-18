using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[CreateAssetMenu(fileName = "DadosVirus", menuName = "Atributos do Virus")]
public class Virus : ScriptableObject
{
    public int taxaDeInfeccao = 1;
    public int taxaDeMortalidade = 0;

}
