using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[CreateAssetMenu(fileName = "DadosCarta", menuName = "Atributos da Carta")]
public class Carta : ScriptableObject
{
    public int id;
    public string nome;
    public string descricao;
    public int custo;
    public Sprite imagem;
    public List<int> efeitos = new();

}