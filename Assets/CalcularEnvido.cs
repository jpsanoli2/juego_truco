using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class CalcularEnvido : MonoBehaviour
{
    
    private Button boton;
    [SerializeField] private TextMeshProUGUI textoEnvido;
    

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(AlCalcularEnvido);
    }

    private void AlCalcularEnvido()
    {
        
    }
    private void DibujarEnvido(int puntos) 
    { 
        textoEnvido.text = puntos.ToString();
    }

}