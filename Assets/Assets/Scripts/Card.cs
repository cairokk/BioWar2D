using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Card : NetworkBehaviour
{
    public Carta dadosCarta;
    public GameObject canvas;
    public PlayerController player;
    private bool isDragging = false;
    private bool isDraggable = false;
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





    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("MainCanvas");
        if (isOwned)
        {
            isDraggable = true;
        }
    }

    public void UpdateCard(Carta novosDados)
    {
        dadosCarta = novosDados;
        textoNome.text = dadosCarta.nome;
        textoNomeZoom.text = dadosCarta.nome;
        textoDescricao.text = dadosCarta.descricao;
        textoDescricaoZoom.text = dadosCarta.descricao;

        moldura.sprite = costSprites[dadosCarta.custo];
        molduraZoom.sprite = costSprites[dadosCarta.custo];
        ImagemCarta.sprite = dadosCarta.imagem;
        ImagemCartaZoom.sprite = dadosCarta.imagem;

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

        if (!isDraggable || !player.IsMyTurn()) return;
        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    public void EndsDrag()
    {

        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        player = networkIdentity.GetComponent<PlayerController>();

        if (!isDraggable || !player.IsMyTurn()) return;
        isDragging = false;
        if (isOverDropZone)
        {
            if (player.PlayCard(gameObject))
            {
                Destroy(gameObject, 0.5f);
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
}
