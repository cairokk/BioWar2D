using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Card : NetworkBehaviour
{
    public GameObject canvas;
    public Player player;
    private bool isDragging = false;
    private bool isDraggable = false;
    private GameObject startParent;
    private Vector2 startPosition;
    private bool isOverDropZone;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("MainCanvas");
        if (isOwned)
        {
            isDraggable = true;
        }
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
        if (!isDraggable) return;
        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    public void EndsDrag()
    {
        if (!isDraggable) return;
        isDragging = false;
        if (isOverDropZone)
        {
            Debug.Log("Carta ativada!!!!!!!");
            Destroy(gameObject, 0.5f);
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            player = networkIdentity.GetComponent<Player>();
            player.PlayCard(gameObject);  
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
            transform.SetParent(canvas.transform, true);
        }
    }
}
