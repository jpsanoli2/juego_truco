using Unity.Netcode;
using UnityEngine;

public class ManejadorEscenaMultiplayer : MonoBehaviour
{
    void Start()
    {
        // Le decimos a Netcode que nos avise cada vez que un jugador se conecte con éxito
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += AlConectarseUnCliente;
        }
    }

    private void AlConectarseUnCliente(ulong clientId)
    {
        // REGLA: Si soy el Servidor/Host Y ya somos 2 jugadores en total conectados
        if (NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            Debug.Log("¡Sala llena! Moviendo a ambos jugadores a la Mesa de Juego...");

            // Usamos el cargador oficial de Netcode que arrastra a todos los clientes conectados
            NetworkManager.Singleton.SceneManager.LoadScene(
                "MesaDeJuego",
                UnityEngine.SceneManagement.LoadSceneMode.Single
            );
        }
    }

    void OnDestroy()
    {
        // Limpieza del evento al cambiar de escena
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= AlConectarseUnCliente;
        }
    }
}