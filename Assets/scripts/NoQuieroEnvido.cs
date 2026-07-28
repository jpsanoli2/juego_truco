using UnityEngine;
using UnityEngine.UI;

public class NoQuieroEnvido : MonoBehaviour
{
    
    private Button boton;
    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(NoQuiero);
    }
    private void NoQuiero()
    {
        GameManager.Instance.ResponderEnvidoRpc(EnumOpcionesEnvido.noQuiero);
    }
}
