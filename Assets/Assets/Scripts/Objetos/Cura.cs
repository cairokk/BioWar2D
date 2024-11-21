using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//[CreateAssetMenu(fileName = "DadosCura", menuName = "Atributos da Cura")]
public class Cura : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnTaxaCuraChanged))] public int taxaDacura = 0;
    [SyncVar(hook = nameof(OnTaxaPesquisaChanged))] public int taxaDePesquisa = 1;
    [SyncVar(hook = nameof(OnFatorUrgenciaChanged))] public int fatorDeUrgencia = 1;
    [SyncVar(hook = nameof(OnAvancoCuraChanged))] public int avancoDaCura = 0;

    public delegate void AtributoCuraChanged();
    public event AtributoCuraChanged OnCuraChanged;   
     private void OnTaxaCuraChanged(int oldValue, int newValue)
    {
        OnCuraChanged?.Invoke();
    }

    private void OnTaxaPesquisaChanged(int oldValue, int newValue)
    {
        OnCuraChanged?.Invoke();
    }

    private void OnFatorUrgenciaChanged(int oldValue, int newValue)
    {
        OnCuraChanged?.Invoke();
    }

    private void OnAvancoCuraChanged(int oldValue, int newValue)
    {
        OnCuraChanged?.Invoke();
    } 
    
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
