using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
//using UnityEditor.Timeline.Actions;


//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public Mazo mazo;
    [SerializeField] private Image mostrarMuestra;
    [SerializeField] private Image cartaRival_1;
    [SerializeField] private Image cartaRival_2;
    [SerializeField] private Image cartaRival_3;
    [SerializeField] private Image carta1;
    [SerializeField] private CartaUI carta1UI;
    [SerializeField] private Image carta2;
    [SerializeField] private CartaUI carta2UI;
    [SerializeField] private Image carta3;
    [SerializeField] private CartaUI carta3UI;
    [SerializeField] private Sprite atrasCarta;
    [SerializeField] private ScriptBotones botones;
    [SerializeField] private CalcularEnvido calcularEnvido;
    [SerializeField] private CantarEnvido cantarEnvido;

    [SerializeField] private BotonTruco botonTruco;

    [SerializeField] private Image cartaJugadaDer;
    [SerializeField] private Image cartaJugadaIzq;

    [SerializeField] private Flor panelFlor;
    [SerializeField] private ConFlorEnvidoPanel panelConFlorEnvido;
    [SerializeField] private ContraFlorPanel panelContraFlor;
    [SerializeField] private PanelLaMiaEsFlor panelLaMiaEsFlor;
    [SerializeField] private PanelFaltaEnvido panelFaltaEnvido;
    [SerializeField] private PanelReEnvido panelReEnvido;
    [SerializeField] private PanelTruco panelTruco;
    [SerializeField] private PanelReTruco panelReTruco;
    [SerializeField] private PanelValeCuatro panelValeCuatro;

    [SerializeField] private PanelFinalizarPartida panelFinalizarPartida;

    [SerializeField] private TextMeshProUGUI puntosEnvidoMios;
    [SerializeField] private TextMeshProUGUI puntosEnvidoRival;

    private List<DatosJugadorServidor> listaJugadores = new List<DatosJugadorServidor>();
    private List<CrearCarta> mazoEnJuego = new List<CrearCarta>();
    public CrearCarta cartaMuestra;
    public List<CrearCarta> cartasPiezas = new List<CrearCarta>();
    private bool rondaEnCurso = false;
    private bool hostEsMano = true; // El Host empieza siendo la mano, luego alternamos

    public int rondaActual = 0;
    private int rondaGanadaHost = 0;
    private int rondaGanadaCliente = 0;
    private List<CrearCarta> cartasJugadas = new List<CrearCarta>();

    public bool existeFlor = false;
    private bool envidoJugado = false;
    private bool esperandoRespuesta = false;
    private int valorTruco = 1;

    ulong turno;
    ulong? tienePalabraTruco = null;

    private int puntosHost = 0;
    private int puntosCliente = 0;
    [SerializeField] private TextMeshProUGUI puntosHostText;
    [SerializeField] private TextMeshProUGUI puntosClienteText;
    private void Awake()
    {
        // Si la instancia ya está vacía, nos asignamos a nosotros mismos
        if (Instance == null)
        {
            Instance = this;
        }
        // Si ya existe otra instancia en escena y no soy yo, me destruyo para no duplicar
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
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
        envidoJugado = false;
        rondaActual = 0;
        rondaActual++;
        existeFlor = false;
        esperandoRespuesta = false;

        TextoBotonTrucoRpc();
        valorTruco = 1;
        listaJugadores[0].manoActual.Clear();
        listaJugadores[1].manoActual.Clear();
        cartasPiezas.Clear();
        rondaGanadaCliente = 0;
        rondaGanadaHost = 0;

        RepartirCartas();
        asignarMuestras();
        AsignarFloresRpc();
        
        
        if (existeFlor)
        {
            DeshabilitarBotonEnvidoRpc();
        }
    }

    private void RepartirCartas()
    {
        if (hostEsMano)
        {
            //CrearCarta c1 = mazoEnJuego.First(c => c.nombre == "1_basto");
            //CrearCarta c2 = mazoEnJuego.First(c => c.nombre == "2_basto");
            //CrearCarta c3 = mazoEnJuego.First(c => c.nombre == "3_basto");
            //CrearCarta c11 = mazoEnJuego.First(c => c.nombre == "4_basto");
            //CrearCarta c22 = mazoEnJuego.First(c => c.nombre == "5_basto");
            //CrearCarta c33 = mazoEnJuego.First(c => c.nombre == "6_basto");
            //listaJugadores[0].manoActual.Add(c1.nombre);
            //listaJugadores[0].manoActual.Add(c2.nombre);
            //listaJugadores[0].manoActual.Add(c3.nombre);
            //listaJugadores[1].manoActual.Add(c11.nombre);
            //listaJugadores[1].manoActual.Add(c22.nombre);
            //listaJugadores[1].manoActual.Add(c33.nombre);
            listaJugadores[0].manoActual.Add(mazoEnJuego[39].nombre);
            listaJugadores[1].manoActual.Add(mazoEnJuego[38].nombre);
            listaJugadores[0].manoActual.Add(mazoEnJuego[37].nombre);
            listaJugadores[1].manoActual.Add(mazoEnJuego[36].nombre);
            listaJugadores[0].manoActual.Add(mazoEnJuego[35].nombre);
            listaJugadores[1].manoActual.Add(mazoEnJuego[34].nombre);
            cartaMuestra = mazoEnJuego[33];
            DibujarDatosGlobalesRpc(cartaMuestra.nombre);
            RepartirCartasEInformarClientes();
            ulong idOponente = listaJugadores[1].clientId;
            DeshabilitarBotonesSinCalcularEnvidoRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
            HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(listaJugadores[0].clientId, RpcTargetUse.Temp));
            
            turno = listaJugadores[0].clientId;
            hostEsMano = false;



        }
        else if (!hostEsMano)
        {
            listaJugadores[1].manoActual.Add(mazoEnJuego[39].nombre);
            listaJugadores[0].manoActual.Add(mazoEnJuego[38].nombre);
            listaJugadores[1].manoActual.Add(mazoEnJuego[37].nombre);
            listaJugadores[0].manoActual.Add(mazoEnJuego[36].nombre);
            listaJugadores[1].manoActual.Add(mazoEnJuego[35].nombre);
            listaJugadores[0].manoActual.Add(mazoEnJuego[34].nombre);
            cartaMuestra = mazoEnJuego[33];
            DibujarDatosGlobalesRpc(cartaMuestra.nombre);
            RepartirCartasEInformarClientes();

            ulong idOponente = listaJugadores[0].clientId;
            DeshabilitarBotonesSinCalcularEnvidoRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
            HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(listaJugadores[1].clientId, RpcTargetUse.Temp));
            turno = listaJugadores[1].clientId;
            hostEsMano = true;
        }

    }
    [Rpc(SendTo.ClientsAndHost)]
    private void DibujarDatosGlobalesRpc(string muestraNombre)
    {
        CrearCarta muestra = BuscarCartaPorNombre(muestraNombre);
        mostrarMuestra.sprite = muestra.arteCarta;
        mostrarMuestra.enabled = true;
        cartaRival_1.sprite = atrasCarta;
        cartaRival_1.enabled = true;
        cartaRival_2.sprite = atrasCarta;
        cartaRival_2.enabled = true;
        cartaRival_3.sprite = atrasCarta;
        cartaRival_3.enabled = true;
    }
    public CrearCarta BuscarCartaPorNombre(string nombre)
    {
        // Buscamos en el mazo base (la lista con las 40 cartas que tienen todos)
        return mazo.cartas.Find(c => c.nombre == nombre);
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
                case 12 when carta.palo == cartaMuestra.palo:
                    if (cartaMuestra.numero == 2 || cartaMuestra.numero == 4 || cartaMuestra.numero == 5 || cartaMuestra.numero == 10 || cartaMuestra.numero == 11)
                    {
                        cartasPiezas.Add(carta);
                    }
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

            RecibirMisCartasPrivadasRpc(cartasRed, NetworkManager.RpcTarget.Single(jugador.clientId, RpcTargetUse.Temp));
            // 3. Enviamos las cartas de forma privada a su pantalla

        }
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void RecibirMisCartasPrivadasRpc(FixedString32Bytes[] nombresCartasRed, RpcParams clientRpcParams = default)
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
        carta1.enabled = true;
        carta1UI.nombreCartaAsignada = manoLocal[0].nombre;
        carta2.sprite = manoLocal[1].arteCarta;
        carta2.enabled = true;
        carta2UI.nombreCartaAsignada = manoLocal[1].nombre;
        carta3.sprite = manoLocal[2].arteCarta;
        carta3.enabled = true;
        carta3UI.nombreCartaAsignada = manoLocal[2].nombre;
    }



    [Rpc(SendTo.Server)]
    public void ProcesarCalculoEnvidoRpc(RpcParams rpcParams = default)
    {
        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;

        DatosJugadorServidor jugador = listaJugadores.Find(j => j.clientId == idClienteQueApreto);

        if (jugador != null)
        {
            int puntosDeEsteJugador = 0;
            if (jugador.tieneFlor)
            {
                puntosDeEsteJugador = jugador.CalcularPuntosFlor();
            }
            else
            {
                puntosDeEsteJugador = jugador.CalcularPuntosEnvido();
            }
            RecibirCalculoEnvidoRpc(puntosDeEsteJugador, NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));

        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void TextoBotonTrucoRpc()
    {
        botonTruco.TextoTruco();
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void TextoBotonReTrucoRpc()
    {
        botonTruco.TextoReTruco();
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void TextoBotonValeCuatroRpc()
    {
        botonTruco.TextoValeCuatro();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void RecibirCalculoEnvidoRpc(int puntosResultantes, RpcParams clientRpcParams = default)
    {
        calcularEnvido.DibujarEnvido(puntosResultantes);
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void DeshabilitarBotonesClienteRpc(RpcParams rpcParams = default)
    {
        botones.SetInteraccionBotones(false);
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void HabilitadBotonSinEnvidoRpc(RpcParams rpcParams = default)
    {
        botones.HabilitadBotonSinEnvido();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void DeshabilitarBotonesSinCalcularEnvidoRpc(RpcParams rpcParams = default)
    {
        botones.SetInteraccionBotonesSinCalcularEnvido(false);
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void HabilitarBotonesClienteRpc(RpcParams rpcParams = default)
    {
        botones.SetInteraccionBotones(true);
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void MostrarPanelPropuestaClienteRpc(RpcParams rpcParams = default)
    {
        cantarEnvido.mostrarPanelEnvido();
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void OcultarPanelPropuestaClienteRpc(RpcParams rpcParams = default)
    {
        cantarEnvido.ocultarPanelEnvido();
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void MostrarPanelFinalizarPartidaRpc(string texto, RpcParams rpcParams = default)
    {
        panelFinalizarPartida.MostrarPanel(texto);
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void DeshabilitarBotonesTodosLosClientesRpc()
    {
        botones.SetInteraccionBotones(false);
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void DeshabilitarBotonEnvidoRpc()
    {
        botones.DeshabilitadBotonEnvido();
    }
    


    [Rpc(SendTo.ClientsAndHost)]
    private void HabilitarBotonesTodosLosClientesRpc()
    {
        botones.SetInteraccionBotones(true);
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void MostrarPanelFlorRpc(RpcParams rpcParams = default)
    {
        panelFlor.MostrarPanelFlor();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void OcultarPanelFlorRpc(RpcParams rpcParams = default)
    {
        panelFlor.OcultarPanelFlor();
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void MostrarTextoTieneFlorRpc(bool florHost, bool florCliente, ulong host, ulong cliente)
    {
        TextoTieneFlor(florHost, florCliente, host, cliente);
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void MostrarPanelConFlorEnvidoRpc(RpcParams rpcParams = default)
    {
        panelConFlorEnvido.MostrarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void OcultarPanelConFlorEnvidoRpc(RpcParams rpcParams = default)
    {
        panelConFlorEnvido.OcultarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void MostrarPanelContraFlorRpc(RpcParams rpcParams = default)
    {
        panelContraFlor.MostrarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void OcultarPanelContraFlorRpc(RpcParams rpcParams = default)
    {
        panelContraFlor.OcultarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void MostrarPanelLaMiaEsFlorRpc(RpcParams rpcParams = default)
    {
        panelLaMiaEsFlor.MostrarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void OcultarPanelLaMiaEsFlorRpc(RpcParams rpcParams = default)
    {
        panelLaMiaEsFlor.OcultarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void MostrarPanelFaltaEnvidoRpc(RpcParams rpcParams = default)
    {
        panelFaltaEnvido.MostrarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void OcultarPanelFaltaEnvidoRpc(RpcParams rpcParams = default)
    {
        panelFaltaEnvido.OcultarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void MostrarPanelReEnvidoRpc(RpcParams rpcParams = default)
    {
        panelReEnvido.MostrarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void OcultarPanelREnvidoRpc(RpcParams rpcParams = default)
    {
        panelReEnvido.OcultarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void MostrarPanelTrucoRpc(RpcParams rpcParams = default)
    {
        panelTruco.MostrarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void OcultarPanelTrucoRpc(RpcParams rpcParams = default)
    {
        panelTruco.OcultarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void MostrarPanelReTrucoRpc(RpcParams rpcParams = default)
    {
        panelReTruco.MostrarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void OcultarPanelReTrucoRpc(RpcParams rpcParams = default)
    {
        panelReTruco.OcultarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void MostrarPanelValeCuatroRpc(RpcParams rpcParams = default)
    {
        panelValeCuatro.MostrarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void OcultarPanelValeCuatroRpc(RpcParams rpcParams = default)
    {
        panelReTruco.OcultarPanel();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    private void SetBotonTrucoRpc(bool interactuable, RpcParams rpcParams = default)
    {
        botones.SetBotonTruco(interactuable);
    }
    [Rpc(SendTo.Server)]
    public void ProcesarCantarEnvidoRpc(RpcParams rpcParams = default)
    {
        if (esperandoRespuesta) return;
        if (existeFlor) return;
        if (rondaActual != 1) return;
        if (envidoJugado) return;

        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;

        if (idClienteQueApreto != turno) return;
        
        DatosJugadorServidor jugador = listaJugadores.Find(j => j.clientId == idClienteQueApreto);
        esperandoRespuesta = true;

        ulong idOponente = listaJugadores.Find(j => j.clientId != idClienteQueApreto).clientId;

        envidoJugado = true;

        DeshabilitarBotonEnvidoRpc();
        DeshabilitarBotonesTodosLosClientesRpc();

        MostrarPanelPropuestaClienteRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
    }
    [Rpc(SendTo.Server)]
    public void ResponderEnvidoRpc(EnumOpcionesEnvido decision, RpcParams rpcParams = default)
    {
        if (!esperandoRespuesta) return;
        if (existeFlor) return;
        if (rondaActual != 1) return;
            
        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;
        DatosJugadorServidor jugador = listaJugadores.Find(j => j.clientId == idClienteQueApreto);

        ulong idOponente = listaJugadores.Find(j => j.clientId != idClienteQueApreto).clientId;
        DatosJugadorServidor jugadorOponente = listaJugadores.Find(j => j.clientId == idOponente);

        switch (decision)
        {
            case EnumOpcionesEnvido.quiero:
                if (!esperandoRespuesta) return;
                DatosJugadorServidor ganador = ResolverEnvido(jugador, jugadorOponente);
                DatosJugadorServidor perdedor = (ganador == jugador) ? jugadorOponente : jugador;
                if (ganador == null)
                {
                    if (hostEsMano)
                    {
                        SumarPuntosRpc(2, "host");

                    }
                    else
                    {
                        SumarPuntosRpc(2, "cliente");
                    }
                }
                if (ganador == jugador && jugador == listaJugadores[0])
                {
                    MostrarPuntosEnPantallasRpc(idClienteQueApreto, ganador.CalcularPuntosEnvido(), idOponente, perdedor.CalcularPuntosEnvido());
                    SumarPuntosRpc(2, "host");
                }
                else if (ganador == jugador && jugador == listaJugadores[1])
                {
                    MostrarPuntosEnPantallasRpc(idClienteQueApreto, ganador.CalcularPuntosEnvido(), idOponente, perdedor.CalcularPuntosEnvido());
                    SumarPuntosRpc(2, "cliente");
                }
                else if (ganador == jugadorOponente && jugadorOponente == listaJugadores[0])
                {
                    MostrarPuntosEnPantallasRpc(idClienteQueApreto, perdedor.CalcularPuntosEnvido(), idOponente, ganador.CalcularPuntosEnvido());
                    SumarPuntosRpc(2, "host");
                }
                else if (ganador == jugadorOponente && jugadorOponente == listaJugadores[1])
                {
                    MostrarPuntosEnPantallasRpc(idClienteQueApreto, perdedor.CalcularPuntosEnvido(), idOponente, ganador.CalcularPuntosEnvido());
                    SumarPuntosRpc(2, "cliente");
                }
                OcultarPanelPropuestaClienteRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                //HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                HabilitadBotonSinEnvidoRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                break;
            case EnumOpcionesEnvido.noQuiero:
                if (!esperandoRespuesta) return;
                if (jugadorOponente != listaJugadores[0])
                {
                    SumarPuntosRpc(1, "cliente");

                }
                else
                {
                    SumarPuntosRpc(1, "host");
                }
                esperandoRespuesta = false;
                OcultarPanelPropuestaClienteRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                MostrarTextoRpc("No quiero", idClienteQueApreto, "", idOponente);
                HabilitadBotonSinEnvidoRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                //HabilitarBotones(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                break;
            case EnumOpcionesEnvido.reEnvido:
                if (!esperandoRespuesta) return;
                OcultarPanelPropuestaClienteRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                MostrarPanelReEnvidoRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                break;
            case EnumOpcionesEnvido.faltaEnvido:
                if (!esperandoRespuesta) return;
                OcultarPanelPropuestaClienteRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                MostrarPanelFaltaEnvidoRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                break;
        }

    }
    private DatosJugadorServidor ResolverEnvido(DatosJugadorServidor jugador1, DatosJugadorServidor jugador2)
    {
        esperandoRespuesta = false;
        int puntosJugador1 = jugador1.CalcularPuntosEnvido();
        int puntosJugador2 = jugador2.CalcularPuntosEnvido();
        if (puntosJugador1 > puntosJugador2)
        {
            return jugador1;
        }
        else if (puntosJugador2 > puntosJugador1)
        {
            return jugador2;
        }
        return null;
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void MostrarPuntosEnPantallasRpc(ulong idA, int puntosA, ulong idB, int puntosB)
    {
        ActualizarTextosEnvido(idA, puntosA, idB, puntosB);
    }
    private void SumarPuntos(int puntos, string destinatario)
    {
        if (destinatario == "host")
        {
            puntosHost += puntos;
        }
        else if (destinatario == "cliente")
        {
            puntosCliente += puntos;
        }
        IntentarGanarPartida();
        puntosHostText.text = puntosHost.ToString();
        puntosClienteText.text = puntosCliente.ToString();
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void SumarPuntosRpc(int puntos, string destinatario)
    {
        SumarPuntos(puntos, destinatario);
    }
    private void IntentarGanarPartida()
    {
        if (puntosHost >= 30)
        {
            MostrarPanelFinalizarPartidaRpc($"El host ha ganado la partida {puntosHost} - {puntosCliente}");
            esperandoRespuesta = true;
            DeshabilitarBotonesTodosLosClientesRpc();
        }
        else if (puntosCliente >= 30)
        {
            MostrarPanelFinalizarPartidaRpc($"El cliente ha ganado la partida {puntosCliente} - {puntosHost}");
            esperandoRespuesta = true;
            DeshabilitarBotonesTodosLosClientesRpc();
        }
    }
    [Rpc(SendTo.Server)]
    private void AsignarFloresRpc()
    {

        listaJugadores[0].tieneFlorFunc();
        listaJugadores[1].tieneFlorFunc();
        ulong host = listaJugadores[0].clientId;
        ulong cliente = listaJugadores[1].clientId;
        if (listaJugadores[0].tieneFlor && listaJugadores[1].tieneFlor)
        {
            existeFlor = true;

            MostrarTextoTieneFlorRpc(true, true, host, cliente);
            if (hostEsMano)
            {
                MostrarPanelFlorRpc(NetworkManager.RpcTarget.Single(host, RpcTargetUse.Temp));
            }
            else
            {
                MostrarPanelFlorRpc(NetworkManager.RpcTarget.Single(cliente, RpcTargetUse.Temp));
            }
        }
        else if (listaJugadores[0].tieneFlor)
        {
            existeFlor = true;
            MostrarTextoTieneFlorRpc(true, false, host, cliente);
            SumarPuntosRpc(3, "host");

        }
        else if (listaJugadores[1].tieneFlor)
        {
            existeFlor = true;
            MostrarTextoTieneFlorRpc(false, true, host, cliente);
            SumarPuntosRpc(3, "cliente");
        }
    }
    public async Task ActualizarTextosEnvido(ulong idJugadorA, int valorA, ulong idJugadorB, int valorB)
    {
        // 1. Obtenemos el ID de la máquina local (quién está mirando esta pantalla)
        ulong miIdLocal = NetworkManager.Singleton.LocalClientId;

        // 2. Comparamos: ¿Soy el Jugador A?
        if (miIdLocal == idJugadorA)
        {
            // Para mi pantalla (Jugador A): C es mío (A), D es el rival (B)
            puntosEnvidoMios.text = valorA.ToString();
            puntosEnvidoRival.text = valorB.ToString();
        }
        // 3. Si no soy el A, significa que soy el Jugador B
        else if (miIdLocal == idJugadorB)
        {
            // Para mi pantalla (Jugador B): C es mío (B), D es el rival (A)
            puntosEnvidoMios.text = valorB.ToString();
            puntosEnvidoRival.text = valorA.ToString();
        }
        await Task.Delay(10000);
        puntosEnvidoMios.text = "";
        puntosEnvidoRival.text = "";
    }

    public async Task TextoTieneFlor(bool florHost, bool florCliente, ulong host, ulong cliente)
    {
        ulong miIdLocal = NetworkManager.Singleton.LocalClientId;
        if (miIdLocal == host)
        {
            if (florHost)
            {
                puntosEnvidoMios.text = "¡Tengo Flor!";
            }
            if (florCliente)
            {
                puntosEnvidoRival.text = "¡Tengo Flor!";
            }
        }
        else if (miIdLocal == cliente)
        {
            if (florCliente)
            {
                puntosEnvidoMios.text = "¡Tengo Flor!";
            }
            if (florHost)
            {
                puntosEnvidoRival.text = "¡Tengo Flor!";
            }
        }
        await Task.Delay(10000);
        puntosEnvidoMios.text = "";
        puntosEnvidoRival.text = "";
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void MostrarTextoRpc(string mensaje1, ulong id1, string mensaje2, ulong id2)
    {
        MostrarTexto(mensaje1, id1, mensaje2, id2);
    }
    public async Task MostrarTexto(string mensaje1, ulong id1, string mensaje2, ulong id2)
    {
        ulong miIdLocal = NetworkManager.Singleton.LocalClientId;
        if (miIdLocal == id1)
        {
            puntosEnvidoMios.text = mensaje1;
            puntosEnvidoRival.text = mensaje2;
        }
        else if (miIdLocal == id2)
        {
            puntosEnvidoMios.text = mensaje2;
            puntosEnvidoRival.text = mensaje1;
        }
        await Task.Delay(10000);
        puntosEnvidoMios.text = "";
        puntosEnvidoRival.text = "";
    }
    [Rpc(SendTo.Server)]
    public void ProcesarRespuestaFlorRpc(string respuesta, RpcParams rpcParams = default)
    {
        if (esperandoRespuesta) return;
        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;
        ulong idOponente = listaJugadores.Find(j => j.clientId != idClienteQueApreto).clientId;
        esperandoRespuesta = true;
        switch (respuesta)
        {
            case "envido":
                if (!esperandoRespuesta) return;
                MostrarPanelConFlorEnvidoRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                OcultarPanelFlorRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                
                break;
            case "contraFlor":
                if (!esperandoRespuesta) return;
                MostrarPanelContraFlorRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                OcultarPanelFlorRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                break;
            case "flor":
                if (!esperandoRespuesta) return;
                MostrarPanelLaMiaEsFlorRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                OcultarPanelFlorRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                break;
        }
    }
    [Rpc(SendTo.Server)]
    public void ProcesarRespuesConFlorEnvidoRpc(EnumConFlorEnvido respuesta, RpcParams rpcParams = default)
    {
        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;

        ulong idOponente = listaJugadores.Find(j => j.clientId != idClienteQueApreto).clientId;

        DatosJugadorServidor jugador = listaJugadores.Find(j => j.clientId == idClienteQueApreto);
        DatosJugadorServidor jugador2 = listaJugadores.Find(j => j.clientId == idOponente);
        switch (respuesta)
        {
            case EnumConFlorEnvido.quiero:
                if (!esperandoRespuesta) return;
                OcultarPanelConFlorEnvidoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                esperandoRespuesta = false;
                DatosJugadorServidor ganador = ResolverConFlorEnvido(listaJugadores.Find(j => j.clientId == idClienteQueApreto), listaJugadores.Find(j => j.clientId == idOponente));
                if (ganador != null)
                {
                    if (ganador == listaJugadores[0])
                    {
                        SumarPuntosRpc(5, "host");

                    }
                    else if (ganador == listaJugadores[1])
                    {
                        SumarPuntosRpc(5, "cliente");
                    }
                }
                else
                {
                    if (hostEsMano)
                    {
                        SumarPuntosRpc(5, "host");
                    }
                    else if (!hostEsMano)
                    {
                        SumarPuntosRpc(5, "cliente");
                    }
                }
                MostrarPuntosEnPantallasRpc(idClienteQueApreto, jugador.CalcularPuntosFlor(), idOponente, jugador2.CalcularPuntosFlor());
                break;
            case EnumConFlorEnvido.noQuiero:
                if (!esperandoRespuesta) return;
                OcultarPanelConFlorEnvidoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                esperandoRespuesta = false;
                if (idOponente == listaJugadores[0].clientId)
                {
                    SumarPuntosRpc(3, "host");
                }
                else if (idOponente == listaJugadores[1].clientId)
                {
                    SumarPuntosRpc(3, "cliente");
                }
                MostrarTextoRpc("No quiero", idClienteQueApreto, "", idOponente);
                break;
            case EnumConFlorEnvido.contraFlor:
                if (!esperandoRespuesta) return;
                OcultarPanelConFlorEnvidoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                MostrarPanelContraFlorRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                break;
        }
    }
    public DatosJugadorServidor ResolverConFlorEnvido(DatosJugadorServidor jugador1, DatosJugadorServidor jugador2)
    {
        esperandoRespuesta = false;
        int puntosJugador1 = jugador1.CalcularPuntosFlor();
        int puntosJugador2 = jugador2.CalcularPuntosFlor();
        if (puntosJugador1 > puntosJugador2)
        {
            return jugador1;
        }
        else if (puntosJugador2 > puntosJugador1)
        {
            return jugador2;
        }
        return null;
    }

    [Rpc(SendTo.Server)]
    public void ProcesarRespuestaContraFlorRpc(EnumContraFlor respuesta, RpcParams rpcParams = default)
    {
        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;
        ulong idOponente = listaJugadores.Find(j => j.clientId != idClienteQueApreto).clientId;

        DatosJugadorServidor jugador1 = listaJugadores.Find(j => j.clientId == idClienteQueApreto);
        DatosJugadorServidor jugador2 = listaJugadores.Find(j => j.clientId == idOponente);

        switch (respuesta)
        {
            case EnumContraFlor.quiero:
                if (!esperandoRespuesta) return;
                OcultarPanelContraFlorRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                DatosJugadorServidor ganador = ResolverConFlorEnvido(jugador1, jugador2);
                if (ganador != null)
                {
                    if (ganador == listaJugadores[0])
                    {
                        int puntaje = 30 - puntosHost;
                        SumarPuntosRpc(puntaje, "host");
                    }
                    else if (ganador == listaJugadores[1])
                    {
                        int puntaje = 30 - puntosHost;
                        SumarPuntosRpc(puntaje, "cliente");
                    }
                }
                else
                {
                    if (hostEsMano)
                    {
                        int puntaje = 30 - puntosHost;
                        SumarPuntosRpc(puntaje, "host");
                    }
                    else if (!hostEsMano)
                    {
                        int puntaje = 30 - puntosHost;
                        SumarPuntosRpc(puntaje, "cliente");
                    }
                }
                break;
            case EnumContraFlor.noQuiero:
                if (!esperandoRespuesta) return;
                esperandoRespuesta = false;
                OcultarPanelContraFlorRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                MostrarTextoRpc("No quiero", idClienteQueApreto, "", idOponente);
                if (idClienteQueApreto == listaJugadores[0].clientId)
                {
                    SumarPuntosRpc(3, "cliente");
                }
                else if (idClienteQueApreto == listaJugadores[1].clientId)
                {
                    SumarPuntosRpc(3, "host");
                }
                break;
        }
    }
    [Rpc(SendTo.Server)]
    public void LaMiaEsFlorRpc(EnumLaMiaEsFlor respuesta, RpcParams rpcParams = default)
    {
        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;
        ulong idOponente = listaJugadores.Find(j => j.clientId != idClienteQueApreto).clientId;

        DatosJugadorServidor jugador = listaJugadores.Find(j => j.clientId == idClienteQueApreto);
        DatosJugadorServidor jugador2 = listaJugadores.Find(j => j.clientId == idOponente);

        OcultarPanelLaMiaEsFlorRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
        switch (respuesta)
        {
            case EnumLaMiaEsFlor.flor:
                esperandoRespuesta = false;
                DatosJugadorServidor ganador = ResolverConFlorEnvido(listaJugadores.Find(j => j.clientId == idClienteQueApreto), listaJugadores.Find(j => j.clientId == idOponente));
                if (ganador != null)
                {
                    if (ganador == listaJugadores[0])
                    {
                        SumarPuntosRpc(3, "host");

                    }
                    else if (ganador == listaJugadores[1])
                    {
                        SumarPuntosRpc(3, "cliente");
                    }
                }
                else
                {
                    if (hostEsMano)
                    {
                        SumarPuntosRpc(3, "host");
                    }
                    else if (!hostEsMano)
                    {
                        SumarPuntosRpc(3, "cliente");
                    }
                }
                break;
            case EnumLaMiaEsFlor.envido:
                MostrarPanelConFlorEnvidoRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                break;

            case EnumLaMiaEsFlor.resto:
                MostrarPanelContraFlorRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                break;

        }
        
    }
    [Rpc(SendTo.Server)]
    public void ProcesarReFaltaEnvidoRpc(EnumFaltaEnvido respuesta, RpcParams rpcParams = default)
    {
        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;
        ulong idOponente = listaJugadores.Find(j => j.clientId != idClienteQueApreto).clientId;

        DatosJugadorServidor jugador1 = listaJugadores.Find(j => j.clientId == idClienteQueApreto);
        DatosJugadorServidor jugador2 = listaJugadores.Find(j => j.clientId == idOponente);

        switch (respuesta)
        {
            case EnumFaltaEnvido.quiero:
                esperandoRespuesta = false;
                OcultarPanelFaltaEnvidoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                //HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                HabilitadBotonSinEnvidoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                DeshabilitarBotonEnvidoRpc();
                DatosJugadorServidor ganador = ResolverEnvido(jugador1, jugador2);
                if (ganador != null)
                {
                    if (ganador == listaJugadores[0])
                    {
                        if(puntosHost < 15 && puntosCliente < 15)
                        {
                            int puntosASumar = 15 - puntosHost;
                            SumarPuntosRpc(puntosASumar, "host");
                        }
                        else if(puntosHost >= 15 || puntosCliente <= 15)
                        {
                            int puntosA = 30 - puntosHost;
                            int puntosB = 30 - puntosCliente;
                            int mayor = (puntosA > puntosB) ? puntosA : puntosB;
                            SumarPuntosRpc(mayor, "host");
                        }
                        MostrarPuntosEnPantallasRpc(idClienteQueApreto, jugador1.CalcularPuntosEnvido(), idOponente, jugador2.CalcularPuntosEnvido());
                    }
                    else if (ganador == listaJugadores[1])
                    {
                        if (puntosHost < 15 && puntosCliente < 15)
                        {
                            int puntosASumar = 15 - puntosCliente;
                            SumarPuntosRpc(puntosASumar, "cliente");
                        }
                        else if (puntosHost >= 15 || puntosCliente <= 15)
                        {
                            int puntosA = 30 - puntosHost;
                            int puntosB = 30 - puntosCliente;
                            int mayor = (puntosA > puntosB) ? puntosA : puntosB;
                            SumarPuntosRpc(mayor, "cliente");
                        }
                        MostrarPuntosEnPantallasRpc(idClienteQueApreto, jugador1.CalcularPuntosEnvido(), idOponente, jugador2.CalcularPuntosEnvido());
                    }
                  
                }
                else
                {
                    if (hostEsMano)
                    {
                        if (puntosHost < 15 && puntosCliente < 15)
                        {
                            int puntosASumar = 15 - puntosHost;
                            SumarPuntosRpc(puntosASumar, "host");
                        }
                        else if (puntosHost >= 15 || puntosCliente <= 15)
                        {
                            int puntosA = 30 - puntosHost;
                            int puntosB = 30 - puntosCliente;
                            int mayor = (puntosA > puntosB) ? puntosA : puntosB;
                            SumarPuntosRpc(mayor, "host");
                        }
                        MostrarPuntosEnPantallasRpc(idClienteQueApreto, jugador1.CalcularPuntosEnvido(), idOponente, jugador2.CalcularPuntosEnvido());
                    }
                    else
                    {
                        if (puntosHost < 15 && puntosCliente < 15)
                        {
                            int puntosASumar = 15 - puntosCliente;
                            SumarPuntosRpc(puntosASumar, "cliente");
                        }
                        else if (puntosHost >= 15 || puntosCliente <= 15)
                        {
                            int puntosA = 30 - puntosHost;
                            int puntosB = 30 - puntosCliente;
                            int mayor = (puntosA > puntosB) ? puntosA : puntosB;
                            SumarPuntosRpc(mayor, "cliente");
                        }
                        MostrarPuntosEnPantallasRpc(idClienteQueApreto, jugador1.CalcularPuntosEnvido(), idOponente, jugador2.CalcularPuntosEnvido());
                    }
                }
                break;
            case EnumFaltaEnvido.noQuiero:
                esperandoRespuesta = false;
                DeshabilitarBotonEnvidoRpc();
                OcultarPanelFaltaEnvidoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                //HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                HabilitadBotonSinEnvidoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                MostrarTextoRpc("No quiero", idClienteQueApreto, "", idOponente);
                if (idClienteQueApreto == listaJugadores[0].clientId)
                {
                    SumarPuntosRpc(2, "cliente");
                }
                else if (idClienteQueApreto == listaJugadores[1].clientId)
                {
                    SumarPuntosRpc(2, "host");
                }
                break;
        }
    }
    [Rpc(SendTo.Server)]
    public void ProcesarReEnvidoRpc(EnumReEnvido respuesta, RpcParams rpcParams = default)
    {
        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;
        ulong idOponente = listaJugadores.Find(j => j.clientId != idClienteQueApreto).clientId;


        DatosJugadorServidor jugador1 = listaJugadores.Find(j => j.clientId == idClienteQueApreto);
        DatosJugadorServidor jugador2 = listaJugadores.Find(j => j.clientId == idOponente);
        switch (respuesta)
        {
            case EnumReEnvido.quiero:
                esperandoRespuesta = false;
                envidoJugado = true;
                OcultarPanelREnvidoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                //HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                HabilitadBotonSinEnvidoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                DatosJugadorServidor ganador = ResolverEnvido(jugador1, jugador2);
                if (ganador != null)
                {
                    if (ganador == listaJugadores[0])
                    {
                        SumarPuntosRpc(4, "host");
                    }
                        MostrarPuntosEnPantallasRpc(idClienteQueApreto, jugador1.CalcularPuntosEnvido(), idOponente, jugador2.CalcularPuntosEnvido());
                    }
                    else if (ganador == listaJugadores[1])
                    {
                        SumarPuntosRpc(4, "cliente");
                        
                        MostrarPuntosEnPantallasRpc(idClienteQueApreto, jugador1.CalcularPuntosEnvido(), idOponente, jugador2.CalcularPuntosEnvido());
                    }
                else
                {
                    if (hostEsMano)
                    {
                        SumarPuntosRpc(4, "host");

                        MostrarPuntosEnPantallasRpc(idClienteQueApreto, jugador1.CalcularPuntosEnvido(), idOponente, jugador2.CalcularPuntosEnvido());
                    }
                    else
                    {
                        SumarPuntosRpc(4, "cliente");

                        MostrarPuntosEnPantallasRpc(idClienteQueApreto, jugador1.CalcularPuntosEnvido(), idOponente, jugador2.CalcularPuntosEnvido());
                    }   
                }
                break;
            case EnumReEnvido.noQuiero:
                esperandoRespuesta = false;
                envidoJugado = true;
                OcultarPanelREnvidoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                //HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                HabilitadBotonSinEnvidoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                MostrarTextoRpc("No quiero", idClienteQueApreto, "", idOponente);
                if (idClienteQueApreto == listaJugadores[0].clientId)
                {
                    SumarPuntosRpc(2, "cliente");
                }
                else if (idClienteQueApreto == listaJugadores[1].clientId)
                {
                    SumarPuntosRpc(2, "host");
                }
                break;
        }
    }
    [Rpc(SendTo.Server)]
    public void ProcesarFaltaEnvidoRpc(RpcParams rpcParams = default)
    {
        if (esperandoRespuesta) return;
        if (existeFlor) return;
        if (rondaActual != 1) return;
        if (envidoJugado) return;

        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;

        if (hostEsMano && listaJugadores[0].clientId != idClienteQueApreto) return;

        DatosJugadorServidor jugador = listaJugadores.Find(j => j.clientId == idClienteQueApreto);
        esperandoRespuesta = true;

        ulong idOponente = listaJugadores.Find(j => j.clientId != idClienteQueApreto).clientId;

        envidoJugado = true;

        DeshabilitarBotonesTodosLosClientesRpc();
        DeshabilitarBotonEnvidoRpc();
        MostrarPanelFaltaEnvidoRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
    }
    public void CambiarTurno()
    {
        if (listaJugadores[0].clientId != turno)
        {
            turno = listaJugadores[0].clientId;
        }
        else
        {
            turno = listaJugadores[1].clientId;
        }
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void DibujarCartaJugadaIzqRpc(bool dibujar, string? carta, RpcParams rpcParams = default)
    {
        cartaJugadaIzq.enabled = dibujar;
        if(carta != null)
        {
            CrearCarta cartaAgregar = mazo.cartas.Find(c => c.nombre == carta);
            cartaJugadaIzq.sprite = cartaAgregar.arteCarta;
        }
        
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void DibujarCartaJugadaDerRpc(bool dibujar, string? carta, RpcParams rpcParams = default)
    {
        cartaJugadaDer.enabled = dibujar;
        if(carta != null)
        {
            CrearCarta cartaAgregar = mazo.cartas.Find(c => c.nombre == carta);
            cartaJugadaDer.sprite = cartaAgregar.arteCarta;
        }
        
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void BorrarCartasTrucoRpc(RpcParams rpcParams = default)
    {
        cartaJugadaIzq.sprite = null;
        cartaJugadaDer.sprite = null;

        cartaJugadaIzq.enabled = false;
        cartaJugadaDer.enabled = false;

        cartasJugadas.Clear();
    }
    [Rpc(SendTo.SpecifiedInParams)]
    public void BorrarCartaManoRpc(string carta, RpcParams rpcParams = default)
    {
        CrearCarta cartaAgregar = mazo.cartas.Find(c => c.nombre == carta);

        if (carta1.sprite == cartaAgregar.arteCarta)
        {
            carta1.enabled = false;
        }
        else if (carta2.sprite == cartaAgregar.arteCarta)
        {
            carta2.enabled = false;
        }
        else
        {
            carta3.enabled = false;
        }
    }
    [Rpc(SendTo.SpecifiedInParams)]
    public void BorrarCartaRivalRpc(RpcParams rpcParams = default)
    {
        if(cartaRival_1.enabled == true)
        {
            cartaRival_1.enabled = false;
        }
        else if(cartaRival_2.enabled == true)
        {
            cartaRival_2.enabled = false;
        }
        else if(cartaRival_3.enabled == true)
        {
            cartaRival_3.enabled = false;
        }
    }
    [Rpc(SendTo.Server)]
    public void IntentarJugarCartaRpc(string carta, RpcParams rpcParams = default)
    {
        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;

        if (idClienteQueApreto != turno) return;

        ulong idOponente = listaJugadores.Find(j => j.clientId != idClienteQueApreto).clientId;

        CrearCarta cartaAgregar = mazo.cartas.Find(c => c.nombre == carta);

        if(cartasJugadas.Count == 0)
        {
            DibujarCartaJugadaIzqRpc(true, carta);
        }
        else
        {
            DibujarCartaJugadaDerRpc(true, carta);
        }

        cartasJugadas.Add(cartaAgregar);

        BorrarCartaManoRpc(carta, NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
        BorrarCartaRivalRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
        DeshabilitarBotonesTodosLosClientesRpc();
        StartCoroutine(EsperarYHacerAlgo());
        //CambiarTurno();
        

        //HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
        //DeshabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));

        

    }
    IEnumerator EsperarYHacerAlgo()
    {
        
        yield return new WaitForSeconds(2f);
        ResolverTrucoRpc();

    }
    [Rpc(SendTo.Server)]
    public void ResolverTrucoRpc(RpcParams rpcParams = default)
    {
        if (cartasJugadas.Count != 2)
        {
            ulong idOponente = listaJugadores.Find(j => j.clientId != turno).clientId;
            

            if (!envidoJugado)
            {
                HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                DeshabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(turno, RpcTargetUse.Temp));
            }
            else
            {
                HabilitadBotonSinEnvidoRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                DeshabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(turno, RpcTargetUse.Temp));
            }
            CambiarTurno();
            return;
        }

        CrearCarta carta1 = cartasJugadas[0];
        CrearCarta carta2 = cartasJugadas[1];
        int valorCarta1 = 0;
        int valorCarta2 = 0;
        CrearCarta cartaGanadora = null;
        if(cartasPiezas.Contains(carta1)) valorCarta1 = carta1.CalcularTrucoPieza();
        else
        {
            valorCarta1 = carta1.valorTruco;
        }
        if (cartasPiezas.Contains(carta2)) valorCarta2 = carta2.CalcularTrucoPieza();
        else
        {
            valorCarta2 = carta2.valorTruco;
        }
        if (valorCarta1 > valorCarta2) cartaGanadora = carta1;
        else if (valorCarta2 > valorCarta1) cartaGanadora = carta2;

        DatosJugadorServidor ganador = null;

        if (cartaGanadora == null) // --- CASO PARDA (EMPATE) ---
        {
            // En parda, ambos ganan la ronda
            rondaGanadaHost++;
            rondaGanadaCliente++;

            // El turno de la siguiente carta le corresponde al que es MANO
            ganador = hostEsMano ? listaJugadores[1] : listaJugadores[0];
            turno = ganador.clientId;

            // Notificar botones a clientes
            ulong idOponente = listaJugadores.Find(j => j.clientId != ganador.clientId).clientId;
            DeshabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
            HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(ganador.clientId, RpcTargetUse.Temp));
        }
        else // --- CASO HAY GANADOR DE CARTA ---
        {
            DeshabilitarBotonesTodosLosClientesRpc();
            
            if (listaJugadores[0].manoActual.Contains(cartaGanadora.nombre))
            {
                ganador = listaJugadores[0];
                rondaGanadaHost++;
            }
            else if (listaJugadores[1].manoActual.Contains(cartaGanadora.nombre))
            {
                ganador = listaJugadores[1];
                rondaGanadaCliente++;
            }

            turno = ganador.clientId;

            // Habilitar al ganador y deshabilitar al perdedor
            ulong idPerdedor = listaJugadores.Find(j => j.clientId != ganador.clientId).clientId;
            DeshabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(idPerdedor, RpcTargetUse.Temp));
            HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(ganador.clientId, RpcTargetUse.Temp));
        }

        rondaActual++;
        if (rondaActual != 1)
        {
            DeshabilitarBotonEnvidoRpc();
        }
        BorrarCartasTrucoRpc();

        // Comprobar fin de mano (2 rondas ganadas por alguien o resolucion de parda)
        if (rondaGanadaCliente >= 2 || rondaGanadaHost >= 2)
        {
            if (rondaGanadaHost > rondaGanadaCliente)
            {
                SumarPuntosRpc(valorTruco, "host");
            }
            else if (rondaGanadaCliente > rondaGanadaHost)
            {
                SumarPuntosRpc(valorTruco, "cliente");
            }
            else // Si empataron todas las rondas (parda en 1ra y 2da), gana el mano
            {
                string ganadorMano = hostEsMano ? "host" : "cliente";
                SumarPuntosRpc(valorTruco, ganadorMano);
            }

            EmpezarNuevaRonda();
        }
    }
    [Rpc(SendTo.Server)]
    public void ProcesarTrucoRpc(RpcParams rpcParams = default)
    {
        if (esperandoRespuesta) return;
        
        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;
        if (tienePalabraTruco != null && tienePalabraTruco != idClienteQueApreto) return;
        if (idClienteQueApreto != turno) return;
        esperandoRespuesta = true;
        ulong idOponente = listaJugadores.Find(j => j.clientId != idClienteQueApreto).clientId;
        DeshabilitarBotonesTodosLosClientesRpc();
        if(valorTruco == 1)
        {
            MostrarPanelTrucoRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
            TextoBotonReTrucoRpc();
            tienePalabraTruco = idOponente;
            SetBotonTrucoRpc(false, NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
        }
        else if(valorTruco == 2)
        {
            MostrarPanelReTrucoRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
            TextoBotonValeCuatroRpc();
            tienePalabraTruco = idOponente;
            SetBotonTrucoRpc(false, NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
            SetBotonTrucoRpc(true, NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
        }
        else if(valorTruco == 3)
        {
            MostrarPanelValeCuatroRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
            SetBotonTrucoRpc(false, NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
            SetBotonTrucoRpc(false, NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
            tienePalabraTruco = idOponente;
        }
    }

    [Rpc(SendTo.Server)]
    public void MeVoyRpc(RpcParams rpcParams = default)
    {
        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;
        if(idClienteQueApreto == listaJugadores[0].clientId)
        {
            if (existeFlor)
            {
                SumarPuntosRpc(valorTruco, "cliente");
            }
            else if(rondaActual == 1 && envidoJugado == false)
            {
                SumarPuntosRpc(valorTruco + 1, "cliente");
            }
            else
            {
                SumarPuntosRpc(valorTruco, "cliente");
            }
        }
        else
        {
            if (existeFlor)
            {
                SumarPuntosRpc(valorTruco, "host");
            }
            else if (rondaActual == 1 && envidoJugado == false)
            {
                SumarPuntosRpc(valorTruco + 1, "host");
            }
            else
            {
                SumarPuntosRpc(valorTruco, "host");
            }
        }
        BorrarCartasTrucoRpc();
        EmpezarNuevaRonda();
    }
    
    [Rpc(SendTo.Server)]
    public void ResponderTrucoRpc(EnumTruco respuesta, RpcParams rpcParams = default)
    {
        ulong idClienteQueApreto = rpcParams.Receive.SenderClientId;
        ulong idOponente = listaJugadores.Find(j => j.clientId != idClienteQueApreto).clientId;
       
        switch (respuesta)
        {
            case EnumTruco.quiero:
                valorTruco = 2;
                OcultarPanelTrucoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                esperandoRespuesta = false;
                break;
            case EnumTruco.noQuiero:
                if (idClienteQueApreto == listaJugadores[0].clientId)
                {
                    if (rondaActual == 1 && envidoJugado == false)
                    {
                        SumarPuntosRpc(valorTruco + 1, "cliente");
                    }
                    else
                    {
                        SumarPuntosRpc(valorTruco, "cliente");
                    }
                }
                else
                {
                    if (rondaActual == 1 && envidoJugado == false)
                    {
                        SumarPuntosRpc(valorTruco + 1, "host");
                    }
                    else
                    {
                        SumarPuntosRpc(valorTruco, "host");
                    }
                }
                esperandoRespuesta = false;
                OcultarPanelTrucoRpc(NetworkManager.RpcTarget.Single(idClienteQueApreto, RpcTargetUse.Temp));
                HabilitarBotonesClienteRpc(NetworkManager.RpcTarget.Single(idOponente, RpcTargetUse.Temp));
                BorrarCartasTrucoRpc();
                EmpezarNuevaRonda();
                break;
            case EnumTruco.reTruco:
                break;
        }
    }
}