using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaPiezas
{
    /// <summary>
    /// Clase de funciones auxuliares de cálculo y ejecución del progarma
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Devuelve directorio de recursos
        /// </summary>
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// Conversión grados a radianes
        /// </summary>
        /// <param name="value">valor en grados</param>
        /// <returns></returns>
        public static double DegreesToRadians (double value)
        {
            return (Math.PI / 180) * value;
        }

        /// <summary>
        /// Convierte de radianes a grados
        /// </summary>
        /// <param name="value">valor en radianes</param>
        /// <returns></returns>
        public static double RadiansToDegrees(double value)
        {
            return value * 180 / Math.PI;

        }

        /// <summary>
        /// Calcula la distancia euclidea entre 2 puntos
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <returns></returns>
        public static double EuclideanDistance (double x, double y, double x1, double y1)
        {
            return Math.Sqrt(Math.Pow((x - x1), 2) + Math.Pow((y - y1), 2));
        }

        /// <summary>
        /// Calcula el giro positivo o negativo de la base del robot en función de la posición actual(herramienta) y la destino (caja)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static double GiroPosNeg (double x1, double y1, double x2, double y2)
        {
            double m = (y2 - y1) / (x2 - x1);
            double b = y1 - m * x1;
            //y = mx + (y1 – mx1)
            // Para y=0
            double x = (-1 * b) / m;
            return (x>=0)?-1:1; // El valor inverso nos indica si cruza eje positivo o negativo
        }
    }
}
