using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class DrawCards : MonoBehaviour
{
    public GameObject cartaVacina;
    public GameObject cartaVirus;
    public GameObject playerArea;
    public GameObject enemyArea;
    const int handSize =  5;
    // Start is called before the first frame update

    public void onClick(){
        for (int i = 0; i < handSize; i++)
        {
            GameObject newCard = Instantiate(cartaVacina, new Vector2(0,0), quaternion.identity);
            newCard.transform.SetParent(playerArea.transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
