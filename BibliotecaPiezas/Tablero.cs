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
        private const int MIN_ANCHO = 35;
        private const int MAX_ANCHO = 140;
        private const int MIN_LARGO = 35;
        private const int MAX_LARGO = 70;
        private const int MIN_ALTURA = 20;
        private const int MAX_ALTURA = 50;
        private const int MIN_X = -280;
        private const int MIN_Y = 150;
        private const int MAX_X = 280;
        private const int MAX_Y = 280;
        private const int MAX_ORIENTACION = 180;
        private static int ID_PIEZA;

        public Tablero()
        {
            Piezas = new List<Pieza>();
            ID_PIEZA = 1;
        }

        /// <summary>
        /// Genera tantas piezas como indica el parámetro sin que colisionen entre sí de tamaño variable.
        /// </summary>
        /// <param name="n">Número de piezas.</param>
        public bool GenerarPiezas (int n)
        {
            int prev_id = ID_PIEZA;
            Console.WriteLine("Generando piezas");
            Random rnd = new Random();
            List<Pieza> nuevas = new List<Pieza>();
            for (int i = 0; i < n; i++)
            {
                Pieza p;
                int intentos = 0;
                do
                {
                    p = new Pieza(rnd.Next(MIN_X, MAX_X), rnd.Next(MIN_Y, MAX_Y), rnd.Next(MIN_ANCHO, MAX_ANCHO), rnd.Next(MIN_LARGO, MAX_LARGO), rnd.Next(MIN_ALTURA, MAX_ALTURA), rnd.Next(MAX_ORIENTACION), ID_PIEZA);
                    intentos++;
                    if (intentos > 20)
                    {
                        ID_PIEZA = prev_id;
                        return false;
                    }
                } while (ColisionaTablero(p, nuevas));
                ID_PIEZA++;
                nuevas.Add(p);
            }
            Piezas.AddRange(nuevas);
            Console.WriteLine("Piezas generadas");
            return true;
        }

        /// <summary>
        /// Verifica si se producen colisiones entre la pieza pasada como parametro y las existentes en el tablero.
        /// </summary>
        /// <param name="pieza">Pieza que queremos comprobar</param>
        /// <returns></returns>
        internal bool ColisionaTablero(Pieza pieza, List<Pieza> nuevas)
        {
            List<Pieza> piezas_total = new List<Pieza>();
            piezas_total.AddRange(Piezas);
            piezas_total.AddRange(nuevas);
            foreach (Pieza p in piezas_total)
            {
                if (p.Colisiona(pieza) || pieza.Colisiona(p))
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
                ID_PIEZA = 1;
                foreach (XmlNode piezaXml in nodeList)
                {
                    Pieza pieza = new Pieza(ID_PIEZA);
                    ID_PIEZA++;
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

        public List<Pieza> ExtraerPiezas()
        {
            List<int> ids = ExtraerPieza.BestSolution(Piezas);
            IEnumerable<Pieza> query = Piezas.Where((pieza, index) => ids.Contains(index)).OrderBy(pieza => pieza.Alto);
            return query.ToList(); //TODO: ordenar por alturas
        }

        public void TableroToRoboDK (RoboDK.Item ref_frame, RoboDK RDK, RoboDK.Item ROBOT)
        {
            foreach (Pieza p in Piezas)
            {
                if (!p.EnSimulador)
                    PiezaToRoboDK(ref_frame, RDK, p, ROBOT);
            }
        }

        public void PiezaToRoboDK (RoboDK.Item ref_frame, RoboDK RDK, Pieza p, RoboDK.Item ROBOT)
        {
            RoboDK.Item item = RDK.AddFile(Utils.AssemblyDirectory + @"\Resources\pieza.stl", ref_frame);
            double[] scale = new double[3] { p.Ancho/2, p.Largo/2, p.Alto/2 };
            item.Scale(scale);
            Mat rot = Mat.rotz(Utils.DegreesToRadians(p.Orientacion));
            Mat pose = Mat.transl(p.X, p.Y, 0)*rot;
            item.setPose(pose);
            item.setName("pieza_" + p.ID);
            Random rdn = new Random();
            item.SetColor(0, new List<double> { rdn.NextDouble(), rdn.NextDouble() , rdn.NextDouble(), 1});
            p.EnSimulador = true;
            p.Item = item;
        }
    }
}
