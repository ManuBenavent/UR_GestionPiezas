using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaPiezas
{
    internal static class ExtraerPieza
    {
        private static Dictionary<KeyValuePair<int, int>, long> almacen;
        private static List<bool> piezas_validas;
        private static List<long> values;
        private static List<int> sizes;
        private const int kVentosas = 4;

        /// <summary>
        /// Devuelve los ids de la lista de piezas de aquellas que se deberían extraer primero.
        /// </summary>
        /// <param name="piezas"></param>
        /// <returns></returns>
        internal static List<int> BestSolution (List<Pieza> piezas)
        {
            // Inicializacion de valores
            almacen = new Dictionary<KeyValuePair<int, int>, long>();
            values = new List<long>();
            sizes = new List<int>();
            piezas_validas = new List<bool>();
            foreach (Pieza p in piezas)
            {
                values.Add((long)Math.Pow(2, p.Alto)); // Con crecimiento exponencial damos preferencia a las mas altas para evitar choques
                sizes.Add(p.Ventosas);
                piezas_validas.Add(p.EnSimulador && !p.Recogida);
            }
            long r = BestRecursive(piezas.Count, kVentosas);
            List<int> sol = new List<int>();
            ParseSol(values.Count, kVentosas, r, sol);
            return sol;
        }

        private static long BestRecursive (int iterator, int n)
        {
            if (n <= 0 || iterator <= 0)
            {
                return 0;
            }
            KeyValuePair<int, int> key = new KeyValuePair<int, int>(iterator, n);
            if (almacen.ContainsKey(key))
            {
                return almacen[key];
            }
            long res;
            if (n - sizes[iterator - 1] >= 0 && piezas_validas[iterator - 1])
                res = Math.Max(BestRecursive(iterator - 1, n - sizes[iterator - 1]) + values[iterator - 1], BestRecursive(iterator - 1, n));
            else 
                res = BestRecursive(iterator - 1, n);
            almacen.Add(key, res);
            return res;
        }

        private static bool ParseSol(int it, int n, long total, List<int> sol)
        {
            if (n < 0 || total < 0) return false;
            if (it == 0 || n == 0) return true;

            if (piezas_validas[it - 1])
            {
                // NO COGIDO
                long s1 = 0;
                KeyValuePair<int, int> key1 = new KeyValuePair<int, int>(it, n);
                if (almacen.ContainsKey(key1))
                    s1 = almacen[key1];

                // COGIDO
                long s2 = values[it - 1];
                KeyValuePair<int, int> key2 = new KeyValuePair<int, int>(it, n - sizes[it - 1]);
                if (almacen.ContainsKey(key2))
                    s2 += almacen[key2];

                // ELIJO
                if (s2 >= s1)
                {
                    if (ParseSol(it - 1, n - sizes[it - 1], total - values[it - 1], sol))
                        sol.Add(it - 1);
                    else
                        ParseSol(it - 1, n, total, sol);
                }
                else ParseSol(it - 1, n, total, sol);
            }
            else
            {
                ParseSol(it - 1, n, total, sol);
            }
            return true;
        }
    }
}
