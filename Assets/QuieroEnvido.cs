using UnityEngine;
using UnityEngine.UI;

public class QuieroEnvido : MonoBehaviour
{
    private Button boton;
    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(quieroEnvido);
    }
    private void quieroEnvido()
    {
        GameManager.Instance.ResponderEnvidoRpc(EnumOpcionesEnvido.quiero);
    }
}
