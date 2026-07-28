using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class DatosJugadorServidor
{
    public ulong clientId;
    public List<string> manoActual = new List<string>();
    //public int puntosEnvidoOFlor = 0;
    public bool tieneFlor = false;
    public bool yaJugoEstaMano;

    private GameManager gameManager;
    

    // Constructor: Nos ayuda a crear el objeto pasándole el ID
    public DatosJugadorServidor(ulong id)
    {
        this.clientId = id;
        this.gameManager = GameManager.Instance;
    }
    public void tieneFlorFunc() 
    {
        tieneFlor = false;
        CrearCarta carta1 = gameManager.BuscarCartaPorNombre(manoActual[0]);
        CrearCarta carta2 = gameManager.BuscarCartaPorNombre(manoActual[1]);
        CrearCarta carta3 = gameManager.BuscarCartaPorNombre(manoActual[2]);

        int cantidadPiezas = 0;
        if (gameManager.cartasPiezas.Contains(carta1)) cantidadPiezas++;
        if (gameManager.cartasPiezas.Contains(carta2)) cantidadPiezas++;
        if (gameManager.cartasPiezas.Contains(carta3)) cantidadPiezas++;
        if (carta1.palo == carta2.palo && carta2.palo == carta3.palo)
        {
            tieneFlor = true;
        }
        else if (cantidadPiezas >= 2)
        {
            tieneFlor = true;
        }
        else if (cantidadPiezas == 1 &&
            ((gameManager.cartasPiezas.Contains(carta1) && carta2.palo == carta3.palo) ||
            (gameManager.cartasPiezas.Contains(carta2) && carta1.palo == carta3.palo) ||
            (gameManager.cartasPiezas.Contains(carta3) && carta1.palo == carta2.palo)))
        {
            tieneFlor = true;
        }
    }
    private List<CrearCarta> ManoActualToCarta()
    {
        List<CrearCarta> manoActualCartas = new List<CrearCarta>();
        foreach (var cartaNombre in manoActual)
        {
            CrearCarta carta = gameManager.BuscarCartaPorNombre(cartaNombre);
            manoActualCartas.Add(carta);
        }
        return manoActualCartas;
    }
    public int CalcularPuntosFlor()
    {
        int puntosFlor = 20;
        foreach(var cartaNombre in manoActual)
        {
            CrearCarta carta = gameManager.BuscarCartaPorNombre(cartaNombre);
            if(gameManager.cartasPiezas.Contains(carta))
            {
                if (carta.numero == 12)
                {
                    puntosFlor += gameManager.cartaMuestra.CalcularEnvidoPieza();
                }
                else
                {
                    puntosFlor += carta.CalcularEnvidoPieza();
                } 
            }
            else
            {
                puntosFlor += carta.valorEnvido;
            }
        }
        return puntosFlor;
    }
    public int CalcularPuntosEnvido()
    {
        List<CrearCarta> manoActualCartas = ManoActualToCarta();
        List<CrearCarta> manoActualOrdenada = manoActualCartas.OrderBy(x => x.valorEnvido).ToList();
        int puntos = 0;
        if (!tieneFlor)
        {
            
            foreach (var cartaNombre in manoActual)
            {
                CrearCarta carta = gameManager.BuscarCartaPorNombre(cartaNombre);
                if (gameManager.cartasPiezas.Contains(carta))
                {
                    if(carta.numero == 12)
                    {
                        puntos += 20;
                        puntos += gameManager.cartaMuestra.CalcularEnvidoPieza();
                    }
                    else
                    {
                        puntos += 20;
                        puntos += carta.CalcularEnvidoPieza();
                    }
                    
                    if (manoActualOrdenada[2] != carta)
                    {
                        puntos += manoActualOrdenada[2].valorEnvido;
                        return puntos;
                    }
                    else
                    {
                        puntos += manoActualOrdenada[1].valorEnvido;
                        return puntos;
                    }
                }
            }
            var listaSinPieza = manoActualCartas.GroupBy(x => x.palo).FirstOrDefault(p => p.Count() == 2);
            if (listaSinPieza != null)
            {
                puntos = listaSinPieza.Sum(x => x.valorEnvido) + 20;
                return puntos;
            }
        }
        puntos = manoActualOrdenada[2].valorEnvido;
        return puntos;
    }
}                                                                           