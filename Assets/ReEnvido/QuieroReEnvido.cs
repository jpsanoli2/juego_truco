using UnityEngine;
using UnityEngine.UI;

public class QuieroReEnvido : MonoBehaviour
{
    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(AlQuerer);
    }
    private void AlQuerer()
    {
        GameManager.Instance.ProcesarReEnvidoRpc(EnumReEnvido.quiero);
    }
}
