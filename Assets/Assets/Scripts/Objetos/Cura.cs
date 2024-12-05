using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//[CreateAssetMenu(fileName = "DadosCura", menuName = "Atributos da Cura")]
public class Cura : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnTaxaCuraChanged))] public int taxaDacura = 0;
    [SyncVar(hook = nameof(OnTaxaPesquisaChanged))] public int taxaDePesquisa = 0;
    [SyncVar(hook = nameof(OnFatorUrgenciaChanged))] public int fatorDeUrgencia = 0;
    [SyncVar(hook = nameof(OnAvancoCuraChanged))] public int avancoDaCura = 0;

    [SyncVar(hook = nameof(OnRecursoCuraChanged))] public int recurso = 0;

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
    private void OnRecursoCuraChanged(int oldValue, int newValue)
    {
        OnCuraChanged?.Invoke();
    }
    public void CalcularAvancoDaCura()
    {
        int avancoDaCura = (int)(taxaDePesquisa * fatorDeUrgencia / 4);
        this.avancoDaCura += Mathf.Clamp(avancoDaCura, 0, 10);
    }


    public void CalcularFatorDeUrgencia(Virus virus, int populacaoMorta)
    {
        int fatorBase = 10;
        int populacaoTotal = 47;
        int fatorInfeccao = (virus.taxaDeMortalidade + virus.taxaDeInfeccao) / 4; // Divisão controlada para manter inteiros
        int fatorPopulacao = populacaoTotal > 0 ? populacaoMorta  / populacaoTotal * 2 : 0; // Evita divisão por zero
        int fatorUrgencia = fatorBase + fatorInfeccao + fatorPopulacao;
        fatorDeUrgencia = Mathf.Clamp(fatorUrgencia, 0, 10);

    }

}
