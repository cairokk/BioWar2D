using Mirror;
using TMPro;
using UnityEngine;

public class BaseController : NetworkBehaviour
{
    public BaseClass regiao;

    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoDanoFuturos;
    public TextMeshProUGUI textoInfeccao;


    private void Start()
    {
        InitializeBase();
    }



    private void InitializeBase()
    {
        if (isServer)
        {
            RpcUpdateUI();
        }
    }

    [ClientRpc]
    public void RpcUpdateUI()
    {
        // Atualiza os textos com os valores dos atributos
        textoVida.text = regiao.vida.ToString();
        textoDanoFuturos.text = regiao.DanoFuturo.ToString();
        textoInfeccao.text = regiao.nivelInfecao.ToString();
    }


}
