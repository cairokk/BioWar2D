using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using DG.Tweening; 

public class Card : NetworkBehaviour
{
    public Carta dadosCarta;
    public GameObject canvas;
    public PlayerController player;
    private bool isDragging = false;
    [SyncVar] private bool isDraggable = false;
    private GameObject HistoryArea;
    private GameObject startParent;
    private Vector2 startPosition;

    private bool isOverDropZone;
    public TextMeshProUGUI textoNome;
    public TextMeshProUGUI textoDescricao;
    public TextMeshProUGUI textoNomeZoom;
    public TextMeshProUGUI textoDescricaoZoom;
    public Sprite[] costSprites;
    public Image ImagemCarta;
    public Image ImagemCartaZoom;
    public Image moldura;
    public Image molduraZoom;
    public bool isDeckbuildCard = false;
    private Vector3 originalScale;
    

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("MainCanvas");
        HistoryArea = GameObject.Find("HistoryContent");

        if (isOwned)
        {
            isDraggable = true;
        }
        originalScale = transform.localScale;

    }

    public void UpdateCard(Carta novosDados)
    {
        if (novosDados == null)
        {
            Debug.LogError("UpdateCard received null Carta data.");
            return;
        }

        dadosCarta = novosDados;

        if (textoNome != null) textoNome.text = dadosCarta.nome;
        if (textoNomeZoom != null) textoNomeZoom.text = dadosCarta.nome;
        if (textoDescricao != null) textoDescricao.text = dadosCarta.descricao;
        if (textoDescricaoZoom != null) textoDescricaoZoom.text = dadosCarta.descricao;
        
        if (moldura != null && dadosCarta.custo >= 0 && dadosCarta.custo < costSprites.Length)
            moldura.sprite = costSprites[dadosCarta.custo];
        if (molduraZoom != null && dadosCarta.custo >= 0 && dadosCarta.custo < costSprites.Length)
            molduraZoom.sprite = costSprites[dadosCarta.custo];
        
        if (ImagemCarta != null) ImagemCarta.sprite = dadosCarta.imagem;
        if (ImagemCartaZoom != null) ImagemCartaZoom.sprite = dadosCarta.imagem;
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = true;
    }
    public void StartDrag()
    {
        
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        player = networkIdentity.GetComponent<PlayerController>();
        
        if (isDeckbuildCard){
            return;
        }

        if (isDraggable == false) {
            return;
        };
        if (!player.IsMyTurn()) {
            Debug.Log("Arrastar desativado: isDraggable = " + isDraggable);
            return;
        };
        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    public void EndsDrag()
    {

        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        player = networkIdentity.GetComponent<PlayerController>();

        if (!isDraggable || !player.IsMyTurn() || isDeckbuildCard) return;
        isDragging = false;
        if (isOverDropZone)
        {
            if (player.PlayCard(gameObject))
                {
                    transform.SetParent(HistoryArea.transform, false);
                    isDraggable = false;
                    
                }        
            }
            
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
    public void SetDraggable(bool value)
    {
        isDraggable = value;
        
    }

     public void cardPointerEnter()
    {
        if(isDeckbuildCard){
            transform.DOScale(originalScale * 1.2f, 0.3f); 
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); 
        }
    }
    public void cardPointerExit()
    {
        if(isDeckbuildCard){
            transform.DOScale(originalScale, 0.3f);
        }
    }

     public void cardClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        player = networkIdentity.GetComponent<PlayerController>();
        if(isDeckbuildCard){
            player.cardDeckBuildClick(gameObject);
        }
    }
    public void SetCardSaturation(bool isGrayscale)
    {
        if (ImagemCarta != null)
        {
            ImagemCarta.color = isGrayscale ? new Color(0.3f, 0.3f, 0.3f) : Color.white;
        }
        if (ImagemCartaZoom != null)
        {
            ImagemCartaZoom.color = isGrayscale ? new Color(0.3f, 0.3f, 0.3f) : Color.white;
        }
        if (moldura != null)
        {
            moldura.color = isGrayscale ? new Color(0.3f, 0.3f, 0.3f) : Color.white;
        }
        if (molduraZoom != null)
        {
            molduraZoom.color = isGrayscale ? new Color(0.3f, 0.3f, 0.3f) : Color.white;
        }
    }
}
