using Microsoft.Unity.VisualStudio.Editor;
using Mirror;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class BaseController : NetworkBehaviour
{
    public BaseClass regiao;

    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoDanoFuturos;
    public TextMeshProUGUI textoInfeccao;
    private Image imagem;
    private Color originalColor;


    private void Start()
    {
        imagem = GetComponent<Image>();
        originalColor = imagem.color;
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
        textoVida.text = regiao.vida.ToString();
        textoDanoFuturos.text = regiao.DanoFuturo.ToString();
        textoInfeccao.text = regiao.nivelInfecao.ToString();
    }

    public void UpdateUI()
    {
        textoVida.text = regiao.vida.ToString();
        textoDanoFuturos.text = regiao.DanoFuturo.ToString();
        textoInfeccao.text = regiao.nivelInfecao.ToString();
    }

    public void OnClick()
    {
        GameController gameController = FindObjectOfType<GameController>();
        if (gameController.selecionandoBase == true)
        {
            Debug.Log("Cliquei na base");
            CmdAlterarBaseSelecionada(regiao.nomeBase);

            gameController.selecionandoBase = false;
        }


    }
    [Command]
    public void CmdAlterarBaseSelecionada(string baseSelecionada)
    {
        GameController gameController = FindObjectOfType<GameController>();
        gameController.selecionandoBase = false;
        gameController.baseSelecionada = regiao.nomeBase;
    }

    public void OnEnterHover()
    {
        imagem.color = Color.yellow;
    }

    public void OnExitHover()
    {
        imagem.color = originalColor;
    }


}
