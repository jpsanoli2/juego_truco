using UnityEngine;

public class ConFlorEnvidoPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelFlorEnvido;

    public void MostrarPanel()
    {
        panelFlorEnvido.SetActive(true);
    }
    public void OcultarPanel()
    {
        panelFlorEnvido.SetActive(false);
    }
}
