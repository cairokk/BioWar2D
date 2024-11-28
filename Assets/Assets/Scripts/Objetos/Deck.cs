using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDeck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    public List<int> initialDeck; // Lista de cartas que compõem o deck inicial
}