using UnityEngine;
using UnityEngine.UI;

public class ReEnvido : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Button boton;
    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(ReEnvidoFunc);
    }
    private void ReEnvidoFunc()
    {
        GameManager.Instance.ResponderEnvidoRpc(EnumOpcionesEnvido.reEnvido);
    }
}
