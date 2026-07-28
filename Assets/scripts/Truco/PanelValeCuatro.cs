using UnityEngine;

public class PanelValeCuatro : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    public void MostrarPanel()
    {
        panel.SetActive(true);
    }
    public void OcultarPanel()
    {
        panel.SetActive(false);
    }
}
