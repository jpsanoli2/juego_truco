using UnityEngine;
using UnityEngine.UI;

public class MeVoy : MonoBehaviour
{
    private Button boton;
    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(MeVoyF);
    }
    private void MeVoyF()
    {
        GameManager.Instance.MeVoyRpc();
    }
}
