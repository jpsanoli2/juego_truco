using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Mazo", menuName = "Mazo/Crear")]
public class Mazo : ScriptableObject
{
    public List<CrearCarta> cartas = new List<CrearCarta>();
}
