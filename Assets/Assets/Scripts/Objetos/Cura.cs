using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//[CreateAssetMenu(fileName = "DadosCura", menuName = "Atributos da Cura")]
public class Cura : NetworkBehaviour
{
    [SerializeField][SyncVar] public int taxaDacura = 0;
    [SerializeField][SyncVar] public int taxaDePesquisa = 1;
    [SerializeField][SyncVar] public int fatorDeUrgencia = 1;

    [SerializeField][SyncVar] public int avancoDaCura = 0;


    public void CalcularAvancoDaCura()
    {
        avancoDaCura = (int)(taxaDePesquisa * fatorDeUrgencia * 0.1);
    }
    
    public void CalcularFatorDeUrgencia(Virus virus, int populacaoMorta)
    {
        int populacaoTotal = 47;
        fatorDeUrgencia = (int)(1 + ((virus.taxaDeMortalidade + virus.taxaDeInfeccao) / 40) /
        (((populacaoMorta / populacaoTotal) * 0.2)));
    }

}
