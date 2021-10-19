using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BibliotecaPiezas
{
    public class Tablero
    {
        private List<Pieza> piezas;
        private const int MIN_ALTO = 2;
        private const int MIN_ANCHO = 2;
        private const int MIN_LARGO = 2;
        private const int MAX_ALTO = 20;
        private const int MAX_ANCHO = 20;
        private const int MAX_LARGO = 20;
        private const int MIN_X = 0;
        private const int MIN_Y = 0;
        private const int MAX_X = 30;
        private const int MAX_Y = 30;
        private const int MAX_ORIENTACION = 180;

        public Tablero()
        {
            piezas = new List<Pieza>();
        }

        /// <summary>
        /// Genera tantas piezas como indica el parámetro sin que colisionen entre sí de tamaño variable.
        /// </summary>
        /// <param name="n">Número de piezas.</param>
        public void GenerarPiezas (int n)
        {
            System.Console.WriteLine("Generando piezas");
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                Pieza p;
                do
                {
                    p = new Pieza(rnd.Next(MIN_X, MAX_X), rnd.Next(MIN_Y, MAX_Y), rnd.Next(MIN_ANCHO, MAX_ANCHO), rnd.Next(MIN_ALTO, MAX_ALTO), rnd.Next(MIN_LARGO, MAX_LARGO), rnd.Next(MAX_ORIENTACION));
                } while (ColisionaTablero(p));
                piezas.Add(p);
            }
            Console.WriteLine("Piezas generadas");
        }

        /// <summary>
        /// Verifica si se producen colisiones entre la pieza pasada como parametro y las existentes en el tablero.
        /// </summary>
        /// <param name="pieza">Pieza que queremos comprobar</param>
        /// <returns></returns>
        private bool ColisionaTablero(Pieza pieza)
        {
            foreach(Pieza p in piezas){
                if (p.Colisiona(pieza) || pieza.Colisiona(p)) // La pieza nueva no debe colisionar con las anteriores && las anteriores no deben colisionar con la nueva (significa que las estaría enmarcando)
                {
                    Console.WriteLine("Colision producida.");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Guarda la informacion del tablero en un fichero Xml en el fichero tablero.xml de la ruta pasada como parametro.
        /// </summary>
        /// <param name="ruta">Ruta donde deseamos guardar el tablero, default: cadena vacia.</param>
        /// <returns></returns>
        public bool GuardarTablero(String ruta="")
        {
            try
            {
                // Creamos el documento Xml con su estructura básica
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<tablero></tablero>");

                // Añadimos las piezas
                foreach (Pieza p in piezas)
                {
                    p.GuardarXml(doc);   
                }

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };
                // Guardamos el documento en un fichero con auto indentado
                XmlWriter writer = XmlWriter.Create(ruta + "tablero.xml", settings);
                doc.Save(writer);
                writer.Close();
                return true;
            } catch(XmlException e)
            {
                Console.WriteLine("Se ha producido un error al guardar el archivo XML. Mensaje: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Obtiene la informacion de las piezas del fichero Xml y la guardas en <c>piezas</c> borrando el contenido previo de este.
        /// </summary>
        /// <param name="fichero">Fichero Xml en el formato adecuado.</param>
        /// <returns></returns>
        public bool RecuperarTablero(String fichero)
        {
            try
            {
                piezas.Clear();
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(fichero);
                }
                catch (System.IO.FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }

                XmlNodeList nodeList;
                XmlNode root = doc.DocumentElement;

                // Obtenemos informacion piezas
                nodeList = root.SelectNodes("pieza");
                foreach (XmlNode piezaXml in nodeList)
                {
                    Pieza pieza = new Pieza();
                    pieza.RecuperarXml(piezaXml);
                    piezas.Add(pieza);
                }
                return true;
            } catch (XmlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
