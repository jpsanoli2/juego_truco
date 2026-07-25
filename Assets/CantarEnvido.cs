using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CantarEnvido : MonoBehaviour
{
    private Button boton;
    [SerializeField] private GameObject envidoClienteUI;
    [SerializeField] private TextMeshProUGUI mensajeEnvido;

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(Cantar);
    }
    private void Cantar()
    {
        
        GameManager.Instance.ProcesarCantarEnvidoRpc();
        mostarMensaje();
        
    }
    public void mostrarPanelEnvido() 
    {
        envidoClienteUI.SetActive(true);
    }
    public void ocultarPanelEnvido()
    {
        envidoClienteUI.SetActive(false);
    }
    public async Task mostarMensaje()
    {
        mensajeEnvido.text = "Envido!! Esperando respuesta";
        await Task.Delay(5000);
        mensajeEnvido.text = "";
    }
}
