using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RoboDk.API;


namespace BibliotecaPiezas
{
    public class Tablero
    {
        public List<Pieza> Piezas { get; }
        // Medidas en MM
        private const int MIN_ALTO = 1;
        private const int MAX_ALTO = 3; // Maximo valor soportado para evitar overflow, si se modifica hay que modificar la clase ExtraerPieza
        // TODO: ajustar ancho y largo en funcion del tablero y el alcance
        private const int MIN_ANCHO = 1;
        private const int MAX_ANCHO = 3;
        private const int MIN_LARGO = 1;
        private const int MAX_LARGO = 3;
        private const int MIN_X = 150;
        private const int MIN_Y = -280;
        private const int MAX_X = 280;
        private const int MAX_Y = 280;
        private const int MAX_ORIENTACION = 180;

        public Tablero()
        {
            Piezas = new List<Pieza>();
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
                Piezas.Add(p);
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
            foreach(Pieza p in Piezas){
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
        public bool GuardarTablero(String ruta="tablero.xml")
        {
            try
            {
                // Creamos el documento Xml con su estructura básica
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<tablero></tablero>");

                // Añadimos las Piezas
                foreach (Pieza p in Piezas)
                {
                    p.GuardarXml(doc);   
                }

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };
                // Guardamos el documento en un fichero con auto indentado
                XmlWriter writer = XmlWriter.Create(ruta, settings);
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
                Piezas.Clear();
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

                // Obtenemos informacion Piezas
                nodeList = root.SelectNodes("pieza");
                foreach (XmlNode piezaXml in nodeList)
                {
                    Pieza pieza = new Pieza();
                    pieza.RecuperarXml(piezaXml);
                    Piezas.Add(pieza);
                }
                return true;
            } catch (XmlException e)
            {
                Console.WriteLine(e.Message);
                Piezas.Clear(); // Eliminamos si se ha creado alguna pieza
                return false;
            }
        }

        public void ExtraerPiezas()
        {
            List<int> ids = ExtraerPieza.BestSolution(Piezas);
            foreach (int id in ids)
            {
                Console.WriteLine(id + ": " + Piezas[id].Area);
            }
        }

        public void TableroToRoboDK (RoboDK.Item ref_frame, RoboDK RDK)
        {
            foreach (Pieza p in Piezas)
            {
                if (!p.EnSimulador)
                    PiezaToRoboDK(ref_frame, RDK, p);
            }
        }

        public void PiezaToRoboDK (RoboDK.Item ref_frame, RoboDK RDK, Pieza p)
        {
            RoboDK.Item item = RDK.AddFile(@"C:\Users\mbena\Downloads\pieza.stl", ref_frame); //TODO: incluir una ruta mas adecuada
            double[] scale = new double[3] { p.Ancho, p.Largo, p.Alto };
            item.Scale(scale);
            Mat pose = Mat.transl(p.X, p.Y, 0);
            item.setPose(pose);
            // TODO: falta rotacion
            p.EnSimulador = true;
            p.Item = item;
        }
    }
}
