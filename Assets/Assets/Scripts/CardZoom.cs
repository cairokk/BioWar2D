using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZoom : MonoBehaviour
{

    public GameObject canvas;
    public  GameObject zoomCarta;
    void Awake()
    {

        canvas = GameObject.Find("MainCanvas");

    }
    public void onHoverStart()
    {

        zoomCarta.SetActive(true);

    }

    public void OnHoverEnds()
    {
        zoomCarta.SetActive(false);

    }
}
