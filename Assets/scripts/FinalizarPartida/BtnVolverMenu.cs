using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para SceneManager nativo
using UnityEngine.UI;

public class BtnVolverMenu : MonoBehaviour
{
    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(SalirAlMenuPrincipal);
    }

    public void SalirAlMenuPrincipal()
    {
        // 1. Apagamos la red para el jugador actual
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }

        // 2. Cargamos la escena localmente con el SceneManager de UnityEngine
        SceneManager.LoadScene("JoinByCode", LoadSceneMode.Single);
    }
}