﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaPiezas;
namespace ConsoleAppTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Tablero t = new Tablero();
            while (true)
            {
                t.GenerarPiezas(1);
                t.ExtraerPiezas();
            }
            //t.GenerarPiezas(9);
            //t.GuardarTablero();
            //t.RecuperarTablero("tablero.xml");
            //t.ExtraerPiezas();
        }
    }
}