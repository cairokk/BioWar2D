using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[CreateAssetMenu(fileName = "DadosBase", menuName = "Dados da Base")]

public class Base : ScriptableObject
{
    public string nomeBase;
    public int vida;
    public int nivelInfecao;
    public int DanoFuturo;
    public int defesa;

}
