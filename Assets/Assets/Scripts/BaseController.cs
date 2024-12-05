using Microsoft.Unity.VisualStudio.Editor;
using Mirror;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D), typeof(EdgeCollider2D))]
public class BaseController : NetworkBehaviour
{   
    public BaseClass regiao;

    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoDanoFuturos;
    public TextMeshProUGUI textoInfeccao;
    public TextMeshProUGUI defesa;

    private Image imagem;
    private Color originalColor;
    
    private Vector3 originalPosition;
    private bool isMouseOver = false; // Controle para evitar repetição de eventos
    private static BaseController baseSelecionadaAtual;
    private Vector2 offScreenPosition = new Vector2(-220, -129); // Ajuste conforme necessário.
    private Vector2 onScreenPosition = new Vector2(-58, -129);

    private void Start()
    {
        imagem = GetComponent<Image>();
        originalColor = imagem.color;
        originalPosition = transform.localPosition;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Para evitar interações físicas desnecessárias
    
        InitializeBase();
    }

    void Update(){
        textoVida.text = regiao.vida.ToString();
        textoDanoFuturos.text = regiao.DanoFuturo.ToString();
        textoInfeccao.text = regiao.nivelInfecao.ToString();
        defesa.text = regiao.defesa.ToString();

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
        defesa.text = regiao.defesa.ToString();

    }

    public void UpdateUI()
    {
        textoVida.text = regiao.vida.ToString();
        textoDanoFuturos.text = regiao.DanoFuturo.ToString();
        textoInfeccao.text = regiao.nivelInfecao.ToString();
        defesa.text = regiao.defesa.ToString();

    }

    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerController player = networkIdentity.GetComponent<PlayerController>();
        Debug.Log("Cliquei na base");
        player.baseSelecionada = regiao.nomeBase;
        player.CmdAlterandoBaseSelecionada(regiao.nomeBase);
        SelecionarBase();

    }
     private void SelecionarBase()
    {
        if (baseSelecionadaAtual != null)
        {
            // Resetar a cor ou desativar animação da base anterior
            baseSelecionadaAtual.imagem.DOKill();
            baseSelecionadaAtual.imagem.color = baseSelecionadaAtual.originalColor;
        }

        // Atualizar a base selecionada
        baseSelecionadaAtual = this;

        // Criar um efeito de brilho animado
        imagem.DOKill(); // Parar animações anteriores
        imagem.DOColor(Color.yellow, 0.4f).SetLoops(-1, LoopType.Yoyo);
    }





       public void OnEnterHover()
    {
        if (!isMouseOver)
        {
            isMouseOver = true;
            transform.DOLocalMoveY(originalPosition.y + 10f, 0.3f).SetEase(Ease.OutQuad);
        }
    }

    public void OnExitHover()
    {
        if (isMouseOver)
        {
            isMouseOver = false;
            transform.DOLocalMoveY(originalPosition.y, 0.3f).SetEase(Ease.OutQuad);
        }
    }


}
