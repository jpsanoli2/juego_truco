using UnityEngine;

public class ContraFlorPanel : MonoBehaviour
{
    [SerializeField] private GameObject contraFlorPanel;

    public void MostrarPanel()
    {
        contraFlorPanel.SetActive(true);
    }
    public void OcultarPanel()
    {
        contraFlorPanel.SetActive(false);
    }
}
