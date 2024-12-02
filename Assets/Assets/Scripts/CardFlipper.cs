using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardFlipper : MonoBehaviour
{
    public GameObject back;
    public bool isVirada = false;
    // Start is called before the first frame update
    public void Flip()
    {
        if (isVirada)
        {
            back.SetActive(false);
            isVirada = false;
        }
        else
        {
            back.SetActive(true);
            isVirada = true;
        }
    }
    public void EnsureFaceUp()
    {
        if (!isVirada) // Se já está desflipada, não faça nada
            return;

        Flip(); // Desflipa a carta
    }
}
