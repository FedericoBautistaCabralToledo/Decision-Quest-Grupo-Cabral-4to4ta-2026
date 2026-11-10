using System.Collections.Generic;

namespace JuegoDecisionQuest.MecanicasGenerales
{
    // GRAFO (lista de adyacencia) + DFS RECURSIVO.
    // Modela los estados de la pelea y recorre todos los caminos que llegan a un final.
    public class GrafoFinales
    {
        private readonly Dictionary<string, List<string>> adyacencia =
            new Dictionary<string, List<string>>();

        public void AgregarArista(string desde, string hacia)
        {
            if (!adyacencia.ContainsKey(desde))
                adyacencia[desde] = new List<string>();

            if (!adyacencia.ContainsKey(hacia))
                adyacencia[hacia] = new List<string>();

            adyacencia[desde].Add(hacia);
        }

        // Los mismos estados y transiciones que usa PeleaConRuee
        public static GrafoFinales DePeleaConRuee()
        {
            GrafoFinales g = new GrafoFinales();

            g.AgregarArista("Atacar", "Defensa");
            g.AgregarArista("Atacar", "Decision");
            g.AgregarArista("Defensa", "Atacar");
            g.AgregarArista("Defensa", "Derrota");
            g.AgregarArista("Decision", "Victoria");
            g.AgregarArista("Decision", "Perdonado");

            return g;
        }

        // Devuelve cada camino desde "inicio" hasta un nodo final (sin salidas),
        // por ejemplo: "Atacar -> Decision -> Perdonado"
        public List<string> CaminosHastaFinales(string inicio)
        {
            List<string> resultado = new List<string>();
            List<string> camino = new List<string>();

            Visitar(inicio, camino, resultado);

            return resultado;
        }

        // DFS recursivo con retroceso (backtracking)
        private void Visitar(
            string nodo,
            List<string> camino,
            List<string> resultado)
        {
            camino.Add(nodo);

            List<string> vecinos = adyacencia[nodo];

            if (vecinos.Count == 0)
            {
                // Nodo final: se guarda el camino completo
                resultado.Add(string.Join(" -> ", camino));
            }
            else
            {
                foreach (string vecino in vecinos)
                {
                    // No se repiten nodos del camino actual (evita el ciclo Atacar <-> Defensa)
                    if (!camino.Contains(vecino))
                        Visitar(vecino, camino, resultado);
                }
            }

            camino.RemoveAt(camino.Count - 1);   // retrocede
        }
    }
}