using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "AumentarResiliencia", menuName = "Efeitos Carta/Aumetar Resiliencia")]
public class AumentarResiliencia : CartaEfeito
{
    public int qtdResiliencia;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        BaseController regiaoSelecionada = gameController.bases.Find(b => b.regiao.nomeBase == player.baseSelecionada);
        regiaoSelecionada.regiao.defesa += qtdResiliencia;
        gameController.OnAtributosVirusChanged(gameController.atributosVirus);
        Debug.Log($"Aumentou {qtdResiliencia} na base {regiaoSelecionada.regiao.nomeBase}");
    }

}