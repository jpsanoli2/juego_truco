using UnityEngine;

public class Flor : MonoBehaviour
{
    [SerializeField] private GameObject panelFlor;
    public void MostrarPanelFlor()
    {
        panelFlor.SetActive(true);
    }
    public void OcultarPanelFlor()
    {
        panelFlor.SetActive(false);
    }
}
