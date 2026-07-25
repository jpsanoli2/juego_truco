using UnityEngine;
using UnityEngine.UI;

public class FaltaEnvido : MonoBehaviour
{
    private Button boton;
    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(FaltaEnvidoFunc);
    }
    private void FaltaEnvidoFunc()
    {
        GameManager.Instance.ResponderEnvidoRpc(EnumOpcionesEnvido.faltaEnvido);
    }
}
