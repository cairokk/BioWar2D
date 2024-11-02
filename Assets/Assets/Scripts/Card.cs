using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject canvas;
    private bool isDragging = false;
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject dropZone;
    private bool isOverDropZone;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("MainCanvas");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
    }
    public void StartDrag()
    {
        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    public void EndsDrag()
    {
        isDragging = false;

        if (isOverDropZone)
        {
            Debug.Log("Carta ativada!!!!!!!");
            Destroy(this.gameObject, 0.5f);
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
