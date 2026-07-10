using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class DatosJugadorServidor
{
    public ulong clientId;
    public List<string> manoActual = new List<string>();
    public int puntosEnvidoOFlor;
    public bool tieneFlor = false;
    public bool yaJugoEstaMano;

    private GameManager gameManager;
    

    // Constructor: Nos ayuda a crear el objeto pasándole el ID
    public DatosJugadorServidor(ulong id)
    {
        this.clientId = id;
    }
    public void tieneFlorFunc()
    {
        string palo = gameManager.cartaMuestra.palo;

        CrearCarta carta1 = gameManager.BuscarCartaPorNombre(manoActual[0]);
        CrearCarta carta2 = gameManager.BuscarCartaPorNombre(manoActual[1]);
        CrearCarta carta3 = gameManager.BuscarCartaPorNombre(manoActual[2]);

        if (carta1.palo == palo && carta2.palo == palo && carta3.palo == palo)
        {
            tieneFlor = true;
        }
        else if(gameManager.cartasPiezas.Contains(carta1) && gameManager.cartasPiezas.Contains(carta2))
        {
            tieneFlor = true;
        }
        else if(gameManager.cartasPiezas.Contains(carta1) && gameManager.cartasPiezas.Contains(carta3))
        {
            tieneFlor = true;
        }
        else if(gameManager.cartasPiezas.Contains(carta2) && gameManager.cartasPiezas.Contains(carta3))
        {
            tieneFlor = true;
        }
    }
    private List<CrearCarta> manoActualToCarta()
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
                puntosFlor += carta.CalcularEnvidoPieza();
            }
            else
            {
                puntosFlor += carta.valorEnvido;
            }
        }
        return puntosFlor;
    }
    public int calcularPuntosEnvido()
    {
        List<CrearCarta> manoActualCartas = manoActualToCarta();
        List<CrearCarta> manoActualOrdenada = manoActualCartas.OrderBy(x => x.valorEnvido).ToList();
        int puntos = 0;
        if (!tieneFlor)
        {
            
            foreach (var cartaNombre in manoActual)
            {
                CrearCarta carta = gameManager.BuscarCartaPorNombre(cartaNombre);
                if (gameManager.cartasPiezas.Contains(carta))
                {
                    puntos += 20;
                    puntos += carta.CalcularEnvidoPieza();
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
            puntos = listaSinPieza.Sum(x => x.valorEnvido) + 20;
            return puntos;
        }
        return manoActualOrdenada[2].valorEnvido;
    }
}                                                                           