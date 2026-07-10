using Unity.VisualScripting;
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

    public int CalcularEnvidoPieza()
    {
        switch (numero)
        {
            case 2:
                return 10;
            case 4: 
                return 9;
            case 5:
                return 8;
            case 10:
                return 7;
            case 11:
                return 7;
        }
        return 0;
    }
}
