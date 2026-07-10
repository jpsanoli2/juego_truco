using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;

public class GameManager : NetworkBehaviour
{
    public Mazo mazo;
    [SerializeField] private Image mostrarMuestra;
    [SerializeField] private Image cartaRival_1;
    [SerializeField] private Image cartaRival_2;
    [SerializeField] private Image cartaRival_3;
    [SerializeField] private Image carta1;
    [SerializeField] private Image carta2;
    [SerializeField] private Image carta3;
    [SerializeField] private Sprite atrasCarta;

    [SerializeField]private List<DatosJugadorServidor> listaJugadores = new List<DatosJugadorServidor>();
    private List<CrearCarta> mazoEnJuego = new List<CrearCarta>();
    public CrearCarta cartaMuestra;
    public List<CrearCarta> cartasPiezas = new List<CrearCarta>();
    private bool rondaEnCurso = false;
    private bool hostEsMano = true; // El Host empieza siendo la mano, luego alternamos
    public override void OnNetworkSpawn()
    {
        // Solo el Servidor/Host coordina la lógica
        if (!IsServer) return;

        Debug.Log("GameManager iniciado en el Servidor.");

        // Guardamos las IDs de los jugadores que ya están conectados en la sesión
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            DatosJugadorServidor nuevoJugador = new DatosJugadorServidor(client.ClientId);
            listaJugadores.Add(nuevoJugador);
        }

        // Si ya tenemos a los 2 jugadores listos en la mesa, arrancamos la ronda
        if (listaJugadores.Count == 2)
        {
            EmpezarNuevaRonda();
        }
    }
    public void EmpezarNuevaRonda()
    {
        // 1. Clonamos la lista original para no desordenar el ScriptableObject permanente
        mazoEnJuego = new List<CrearCarta>(mazo.cartas);

        // 2. Mezclamos el mazo (Algoritmo Fisher-Yates)
        for (int i = mazoEnJuego.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            CrearCarta temporal = mazoEnJuego[i];
            mazoEnJuego[i] = mazoEnJuego[rnd];
            mazoEnJuego[rnd] = temporal;
        }

        Debug.Log("¡Mazo mezclado y listo para repartir!");
        RepartirCartas();
        // Aquí ya puedes hacer mazoEnJuego[0] para darle la primera carta al Jugador 1
    }

    private void RepartirCartas()
    {
        if (hostEsMano)
        {
            listaJugadores[0].manoActual.Add(mazoEnJuego[39].nombre);
            listaJugadores[1].manoActual.Add(mazoEnJuego[38].nombre);
            listaJugadores[0].manoActual.Add(mazoEnJuego[37].nombre);
            listaJugadores[1].manoActual.Add(mazoEnJuego[36].nombre);
            listaJugadores[0].manoActual.Add(mazoEnJuego[35].nombre);
            listaJugadores[1].manoActual.Add(mazoEnJuego[34].nombre);
            cartaMuestra = mazoEnJuego[33];
            asignarMuestras();
            DibujarDatosGlobalesClientRpc(cartaMuestra.nombre);
            RepartirCartasEInformarClientes();
            hostEsMano = false;
        }
        else if(!hostEsMano)
        {
            listaJugadores[1].manoActual.Add(mazoEnJuego[39].nombre);
            listaJugadores[0].manoActual.Add(mazoEnJuego[38].nombre);
            listaJugadores[1].manoActual.Add(mazoEnJuego[37].nombre);
            listaJugadores[0].manoActual.Add(mazoEnJuego[36].nombre);
            listaJugadores[1].manoActual.Add(mazoEnJuego[35].nombre);
            listaJugadores[0].manoActual.Add(mazoEnJuego[34].nombre);
            cartaMuestra = mazoEnJuego[33];
            DibujarDatosGlobalesClientRpc(cartaMuestra.nombre);
            RepartirCartasEInformarClientes();
            hostEsMano = true;
        }
        
    }
    [ClientRpc]
    private void DibujarDatosGlobalesClientRpc(string muestraNombre)
    {
        CrearCarta muestra = BuscarCartaPorNombre(muestraNombre);
        mostrarMuestra.sprite = muestra.arteCarta;
        cartaRival_1.sprite = atrasCarta;
        cartaRival_2.sprite = atrasCarta;
        cartaRival_3.sprite = atrasCarta;
    }
    //[ClientRpc]
    ////{
        //if (NetworkManager.Singleton.LocalClientId != clientId) return;
        // Aquí dibujamos las cartas del jugador local
        //for (int i = 0; i < mano.Count; i++)
        //{
            //CrearCarta carta = BuscarCartaPorNombre(mano[i]);
            // Aquí deberías tener un método para dibujar la carta en la UI del jugador
            // Por ejemplo: DibujarCartaEnUI(carta, i);
        //}
    //}
    public CrearCarta BuscarCartaPorNombre(string nombre)
    {
        // Buscamos en el mazo base (la lista con las 40 cartas que tienen todos)
        return mazo.cartas.Find(c => c.nombre == nombre);
    }
    private void ResolucionTruco(string cartaHost, string cartaRival)
    {
        CrearCarta cartaHostObj = BuscarCartaPorNombre(cartaHost);
        CrearCarta cartaRivalObj = BuscarCartaPorNombre(cartaRival);
    }
    private void asignarMuestras()
    {
        foreach (var carta in mazo.cartas)
        {
            switch (carta.numero) 
            {
                case 2 when carta.palo == cartaMuestra.palo:
                    cartasPiezas.Add(carta);
                    break;
                case 4 when carta.palo == cartaMuestra.palo:
                    cartasPiezas.Add(carta);
                    break;
                case 5 when carta.palo == cartaMuestra.palo:
                    cartasPiezas.Add(carta);
                    break;
                case 10 when carta.palo == cartaMuestra.palo:
                    cartasPiezas.Add(carta);
                    break;
                case 11 when carta.palo == cartaMuestra.palo:
                    cartasPiezas.Add(carta);
                    break;
            }
        }
    }
    public void RepartirCartasEInformarClientes()
    {
        if (!IsServer) return;

        foreach (var jugador in listaJugadores)
        {
            // 1. Convertimos la lista de strings (la mano del jugador) en un Array de strings
            // (Netcode maneja mejor los Arrays [] a través de la red que las Listas)
            FixedString32Bytes[] cartasRed = new FixedString32Bytes[jugador.manoActual.Count];

            for (int i = 0; i < jugador.manoActual.Count; i++)
        {
            cartasRed[i] = jugador.manoActual[i]; 
        }
            // 2. Creamos el filtro con el ID único de ESTE jugador
            ClientRpcParams parametrosPrivados = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { jugador.clientId }
                }
            };

            // 3. Enviamos las cartas de forma privada a su pantalla
            RecibirMisCartasPrivadasClientRpc(cartasRed, parametrosPrivados);
        }
    }
    [ClientRpc]
    private void RecibirMisCartasPrivadasClientRpc(FixedString32Bytes[] nombresCartasRed, ClientRpcParams clientRpcParams = default)
    {
        List<CrearCarta> manoLocal = new List<CrearCarta>();

        for (int i = 0; i < nombresCartasRed.Length; i++)
        {
            // Convertimos el FixedString a un string común de C# para buscarlo por nombre
            string nombreDeLaCarta = nombresCartasRed[i].ToString();

            // Tu lógica de antes para buscar en el mazo e instanciar en la UI
            CrearCarta datosCartaLocal = mazo.cartas.Find(c => c.name == nombreDeLaCarta);

            if (datosCartaLocal != null)
            {
                manoLocal.Add(datosCartaLocal);
            }
        }
        carta1.sprite = manoLocal[0].arteCarta;
        carta2.sprite = manoLocal[1].arteCarta;
        carta3.sprite = manoLocal[2].arteCarta;
    }
}
