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
    public int CalcularTrucoPieza()
    {
        switch (numero)
        {
            case 2:
                return 19;
                
            case 4:
                return 18;
                
            case 5:
                return 17;
                
            case 10:
                return 15;
                
            case 11:
                return 16;
            case 12:
                if (GameManager.Instance.cartaMuestra.numero == 2) return 19;
                if (GameManager.Instance.cartaMuestra.numero == 4) return 18;
                if (GameManager.Instance.cartaMuestra.numero == 5) return 17;
                if (GameManager.Instance.cartaMuestra.numero == 10) return 15;
                if (GameManager.Instance.cartaMuestra.numero == 11) return 16;
                break;
        }   
        return 0;
    }
}

