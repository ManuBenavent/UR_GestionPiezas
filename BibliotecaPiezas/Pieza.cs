using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BibliotecaPiezas {
    /// <summary>
    /// Class <c>Pieza</c> representa una pieza con su posición (X, Y), tamaño (Ancho, Largo, Alto) y orientación (rotación sobre eje X) sobre el suelo (Z = 0).
    /// </summary>
    public class Pieza
    {
        /// <summary>Coordenada X </summary>
        public int X { get; set; }
        /// <summary>Coordenada Y </summary>
        public int Y { get; set; }
        /// <summary> Ancho (eje X)</summary>
        public int Ancho { get; set; }
        /// <summary>Largo (eje Y)</summary>
        public int Largo { get; set; }
        /// <summary> Alto (eje Z)</summary>
        public int Alto { get; set; }
        /// <summary>Orientación (en grados) de la pieza en el eje X</summary>
        public int Orientacion { get; set; }
        /// <summary>Determina si la pieza se ha cargado en RoboDK</summary>
        public bool EnSimulador { get; set; }
        /// <summary>Determina si la pieza se ha recogido en RoboDK</summary>
        public bool Recogida { get; set; }
        /// <summary>Referencia al item que representa la pieza en RoboDK</summary>
        public RoboDK.Item Item { get; set; }
        public int Caja { get; set; }
        ///<summary>ID de la pieza</summary>
        public int ID { get; set; }
        private int Radio { get { return Math.Max(Ancho, Largo) + 10; } }


        private const int ANCHO_1_VENTOSA = 35; 
        private const int ANCHO_2_VENTOSA = 70;
        private const int ANCHO_3_VENTOSA = 105;
        /// <summary>Determina el numero de ventosas que se necesitan para la pieza</summary>
        public int Ventosas { get
            {
                if (Ancho <= ANCHO_1_VENTOSA) return 1;
                else if (Ancho <= ANCHO_2_VENTOSA) return 2;
                else if (Ancho <= ANCHO_3_VENTOSA) return 3;
                else return 4;
        } }

        public bool EnZonaAmarilla { get
        {
                return Utils.EuclideanDistance(0, 0, X, Y) <= 200;
        } }
        
        


        /// <summary>
        /// Constructor de pieza.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ancho"></param>
        /// <param name="alto"></param>
        /// <param name="largo"></param>
        /// <param name="Orientacion"></param>
        internal Pieza (int x, int y, int ancho, int largo, int alto, int orientacion, int ID)
        {
            X = x;
            Y = y;
            Ancho = ancho;
            Alto = alto;
            Largo = largo;
            Orientacion = orientacion;
            Item = null;
            EnSimulador = false;
            this.ID = ID;
            Recogida = false;
        }

        /// <summary>
        /// Constructor de pieza. Utilizado unicamente al recupear piezas de un Xml
        /// </summary>
        internal Pieza(int ID)
        {
            X = 0;
            Y = 0;
            Ancho = 0;
            Alto = 0;
            Largo = 0;
            Orientacion = 0;
            this.ID = ID;
            Item = null;
            EnSimulador = false;
            Recogida = false;
        }

        /// <summary>
        /// Comprueba las colisiones entre dos piezas mediante la distancia euclidea de los centros, esta no debe ser menor al maximo lado (ancho o largo) más una separación X de esta pieza
        /// </summary>
        /// <param name="pieza">Pieza de la cual se quieren verificar las colisiones.</param>
        /// <returns>TRUE si colisiona, FALSE si no.</returns>
        internal bool Colisiona (Pieza pieza)
        {
            return Utils.EuclideanDistance(pieza.X, pieza.Y, X, Y) < Radio;
        }

        /// <summary>
        /// Guarda una pieza en el XmlDocument pasado como parametro.
        /// </summary>
        /// <param name="doc">XmlDocument donde guardamos la pieza.</param>
        internal void GuardarXml(XmlDocument doc)
        {
            XmlElement xml_pieza = doc.CreateElement("pieza");
            XmlElement xml_value = doc.CreateElement("x");
            xml_value.InnerText = X.ToString();
            xml_pieza.AppendChild(xml_value);
            xml_value = doc.CreateElement("y");
            xml_value.InnerText = Y.ToString();
            xml_pieza.AppendChild(xml_value);
            xml_value = doc.CreateElement("ancho");
            xml_value.InnerText = Ancho.ToString();
            xml_pieza.AppendChild(xml_value);
            xml_value = doc.CreateElement("largo");
            xml_value.InnerText = Largo.ToString();
            xml_pieza.AppendChild(xml_value);
            xml_value = doc.CreateElement("alto");
            xml_value.InnerText = Alto.ToString();
            xml_pieza.AppendChild(xml_value);
            xml_value = doc.CreateElement("orientacion");
            xml_value.InnerText = Orientacion.ToString();
            xml_pieza.AppendChild(xml_value);
            doc.DocumentElement.AppendChild(xml_pieza);
        }

        /// <summary>
        /// Recupera la informacion de una pieza desde un XmlNode (se asume nodo y formato de este correcto).
        /// </summary>
        /// <param name="xmlPieza">Nodo con la informacion de la pieza</param>
        internal void RecuperarXml(XmlNode xmlPieza)
        {
            X = int.Parse(xmlPieza["x"].InnerText);
            Y = int.Parse(xmlPieza["y"].InnerText);
            Ancho = int.Parse(xmlPieza["ancho"].InnerText);
            Alto = int.Parse(xmlPieza["alto"].InnerText);
            Largo = int.Parse(xmlPieza["largo"].InnerText);
            Orientacion = int.Parse(xmlPieza["orientacion"].InnerText);
        }
    }
}
