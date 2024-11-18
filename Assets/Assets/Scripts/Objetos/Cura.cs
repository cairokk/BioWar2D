using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[CreateAssetMenu(fileName = "DadosCura", menuName = "Atributos da Cura")]
public class Cura : ScriptableObject
{
    public int taxaDacura = 0;
    public int taxaDeMortalidade = 0;
    public int taxaDePesquisa = 0;
    public int fatorDeUrgencia = 0;

}
