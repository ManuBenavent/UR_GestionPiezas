using BibliotecaPiezas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Tablero t = new Tablero();
            t.RecuperarTablero(@"C:\Users\mbena\Downloads\test.xml");
            foreach (Pieza p in t.Piezas)
            {
                p.EnSimulador = true;
            }
            List<Pieza> piezas = t.ExtraerPiezas();
            foreach (Pieza p in piezas)
            {
                Console.WriteLine(p.X + " " + p.Y);
            }
            Console.ReadLine();
        }
    }
}
