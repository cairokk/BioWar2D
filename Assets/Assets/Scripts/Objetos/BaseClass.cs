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

        int dano = virus.taxaDeMortalidade * nivelInfecao / 8 * defesa;
        DanoFuturo += Mathf.Clamp(dano, 0, 10);
    }

    public void CalcularNivelInfeccao(Virus virus)
    {
        int fatorNivel = Mathf.Max(1, nivelInfecao);
        int fatorResiliencia = Mathf.Max(1, defesa);
        int novoNivelDeInfeccao = virus.taxaDeInfeccao * fatorNivel / 5 * fatorResiliencia;
        nivelInfecao = Mathf.Clamp(novoNivelDeInfeccao, nivelInfecao, 10);

    }

    public void CalcularDanoDaRodada()
    {
        int danoAplicado = Mathf.Clamp(DanoFuturo, 0, nivelInfecao);

        nivelInfecao -= danoAplicado;

        vida -= Mathf.Clamp(danoAplicado, 0, vida);

        DanoFuturo = 0;
    }
}
