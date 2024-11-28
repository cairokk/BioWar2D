using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BaseClass : NetworkBehaviour
{
    [SyncVar] public string nomeBase;
    [SyncVar] public int vida;
    [SyncVar] public int nivelInfecao = 1;
    [SyncVar] public int DanoFuturo = 0;
    [SyncVar] public int defesa;

    public void CalcularDanoFuturo(Virus virus)
    {
        DanoFuturo += virus.taxaDeMortalidade * nivelInfecao * (10 - defesa) / 100;
    }

    public void CalcularNivelInfeccao(Virus virus)
    {
        int novoNivelDeInfeccao = nivelInfecao + virus.taxaDeInfeccao * (10 - nivelInfecao) * (10 - defesa) / 100;
        if (novoNivelDeInfeccao <= 10 && novoNivelDeInfeccao >= 0)
        {
            nivelInfecao = novoNivelDeInfeccao;
        }
    }

    public void CalcularDanoDaRodada()
    {
        vida -= DanoFuturo;
    }
}
