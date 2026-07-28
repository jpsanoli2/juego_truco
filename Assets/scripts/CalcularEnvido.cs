using System.Threading.Tasks;
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
        GameManager.Instance.ProcesarCalculoEnvidoRpc();
    }
    public async Task DibujarEnvido(int puntos) 
    { 
        textoEnvido.text = puntos.ToString();
        await Task.Delay(5000);
        textoEnvido.text = "";
    }

}