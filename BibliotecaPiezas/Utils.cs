using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaPiezas
{
    public static class Utils
    {
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

        public static double DegreesToRadians (double value)
        {
            return (Math.PI / 180) * value;
        }

        public static double RadiansToDegrees(double value)
        {
            return value * 180 / Math.PI;

        }

        public static double EuclideanDistance (double x, double y, double x1, double y1)
        {
            return Math.Sqrt(Math.Pow((x - x1), 2) + Math.Pow((y - y1), 2));
        }

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
