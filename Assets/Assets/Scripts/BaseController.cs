using Mirror;
using TMPro;
using UnityEngine;

public class BaseController : NetworkBehaviour
{
    [SyncVar]public Base baseData;

    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoDanoFuturos;
    public TextMeshProUGUI textoInfeccao;


    private void Start()
    {
        InitializeBase();
    }

    private void InitializeBase()
    {

        // Carrega os dados iniciais da base a partir de baseData
        Debug.Log($"{baseData.name} - Health: {baseData.vida}, Contaminated: {baseData.nivelInfecao}");
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
    
    }

    private void UpdateUI()
    {
        // Atualiza os textos com os valores dos atributos
        textoVida.text = baseData.vida.ToString();
        textoDanoFuturos.text =  baseData.DanoFuturo.ToString();
        textoInfeccao.text = baseData.nivelInfecao.ToString();
    }
}
