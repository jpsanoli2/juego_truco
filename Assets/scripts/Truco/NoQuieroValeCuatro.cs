using UnityEngine;
using UnityEngine.UI;

public class NoQuieroValeCuatro : MonoBehaviour
{
    private Button boton;
    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(AlClickear);
    }
    private void AlClickear()
    {
        GameManager.Instance.ResponderValeCuatroRpc(EnumTruco.noQuiero);
    }
}
