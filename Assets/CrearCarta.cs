using UnityEngine;

[CreateAssetMenu(fileName = "CrearCarta", menuName = "Carta/Crear")]
public class CrearCarta : ScriptableObject
{
    public string nombre;
    public string palo;
    public int numero;
    public int valorTruco;
    public int valorEnvido;
    public Sprite arteCarta;

}
