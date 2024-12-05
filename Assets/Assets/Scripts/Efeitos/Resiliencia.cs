using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "Resiliencia", menuName = "Efeitos Carta/Alterar Resiliencia")]
public class Resiliencia : CartaEfeito
{
    public int qtdResiliencia;

    public override void ApplyEffect(PlayerController player, GameController gameController)
    {
        BaseController regiaoSelecionada = gameController.bases.Find(b => b.regiao.nomeBase == player.baseSelecionada);
        int novaDefesa = regiaoSelecionada.regiao.defesa + qtdResiliencia;
        regiaoSelecionada.regiao.defesa = Mathf.Clamp(novaDefesa, 0, 10);
        gameController.OnAtributosVirusChanged(gameController.atributosVirus);
    }

}