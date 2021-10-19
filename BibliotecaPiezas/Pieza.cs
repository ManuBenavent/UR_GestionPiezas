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
        private double _x;
        private double _y;
        private double _orientacion;
        private double _minX;
        private double _minY;
        private double _maxX;
        private double _maxY;

        /// <summary>
        /// Coordenada X
        /// </summary>
        public double X { get { return _x; } set { _x = value; CalcularMinMax(); } }
        /// <summary>
        /// Coordenada Y
        /// </summary>
        public double Y { get { return _y; } set { _y = value; CalcularMinMax(); } }
        /// <summary>
        /// Ancho (eje X)
        /// </summary>
        public double Ancho { get; set; }
        /// <summary>
        /// Largo (eje Y)
        /// </summary>
        public double Largo { get; set; }
        /// <summary>
        /// Alto (eje Z)
        /// </summary>
        public double Alto { get; set; }
        /// <summary>Orientación (en grados) de la pieza en el eje X</summary>
        public double Orientacion { get { return _orientacion; } set { _orientacion = value; CalcularMinMax();  } }
        internal double MinX { get { return _minX; } }
        internal double MaxX { get { return _maxX; } }
        internal double MinY { get { return _minY; } }
        internal double MaxY { get { return _maxY; } }

        /// <summary>
        /// Constructor de pieza.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ancho"></param>
        /// <param name="alto"></param>
        /// <param name="largo"></param>
        /// <param name="Orientacion"></param>
        internal Pieza (double x, double y, double ancho, double alto, double largo, double Orientacion)
        {
            _x = x;
            _y = y;
            Ancho = ancho;
            Alto = alto;
            Largo = largo;
            _orientacion = Orientacion;
            CalcularMinMax();
        }

        /// <summary>
        /// Constructor de pieza. Utilizado unicamente al recupear piezas de un Xml
        /// </summary>
        internal Pieza()
        {
            _x = 0;
            _y = 0;
            Ancho = 0;
            Alto = 0;
            Largo = 0;
            _orientacion = 0;
        }


        /// <summary>
        /// Calculamos MinX, MaxX, MinY, MaxY para crear "bounding boxes" que enmarquen las piezas rotadas y evitar colisiones
        /// </summary>
        private void CalcularMinMax()
        {
            _minX = Math.Min(X * Math.Cos(Orientacion) + Y * Math.Sin(Orientacion), (X + Ancho) * Math.Cos(Orientacion) + Y * Math.Sin(Orientacion)); // InfIzq vs InfDer
            _minX = Math.Min(MinX, X * Math.Cos(Orientacion) + (Y + Largo) * Math.Sin(Orientacion)); // vs SupIzq
            _minX = Math.Min(MinX, (X + Ancho) * Math.Cos(Orientacion) + (Y + Largo) * Math.Sin(Orientacion)); // vs SupDer

            _maxX = Math.Max(X * Math.Cos(Orientacion) + Y * Math.Sin(Orientacion), (X + Ancho) * Math.Cos(Orientacion) + Y * Math.Sin(Orientacion)); // InfIzq vs InfDer
            _maxX = Math.Max(MaxX, X * Math.Cos(Orientacion) + (Y + Largo) * Math.Sin(Orientacion)); // vs SupIzq
            _maxX = Math.Max(MaxX, (X + Ancho) * Math.Cos(Orientacion) + (Y + Largo) * Math.Sin(Orientacion)); // vs SupDer

            _minY = Math.Min(Y * Math.Cos(Orientacion) - X * Math.Sin(Orientacion), Y * Math.Cos(Orientacion) - (X + Ancho) * Math.Sin(Orientacion)); // InfIzq vs InfDer
            _minY = Math.Min(MinY, (Y + Largo) * Math.Cos(Orientacion) - X * Math.Sin(Orientacion)); // vs SupIzq
            _minY = Math.Min(MinY, (Y + Largo) * Math.Cos(Orientacion) - (X + Ancho) * Math.Sin(Orientacion)); // vs SupDer

            _maxY = Math.Max(Y * Math.Cos(Orientacion) - X * Math.Sin(Orientacion), Y * Math.Cos(Orientacion) - (X + Ancho) * Math.Sin(Orientacion)); // InfIzq vs InfDer
            _maxY = Math.Max(MaxY, (Y + Largo) * Math.Cos(Orientacion) - X * Math.Sin(Orientacion)); // vs SupIzq
            _maxY = Math.Max(MaxY, (Y + Largo) * Math.Cos(Orientacion) - (X + Ancho) * Math.Sin(Orientacion)); // vs SupDer
        }

        /// <summary>
        /// Comprueba si alguna de las esquinas de la pieza pasada como parametro esta dentro del area simplicada de la pieza objeto.
        /// El area simplificada consiste en un rectangulo con 0 grados de rotacion que enmarca las 4 esquinas de la pieza.
        /// </summary>
        /// <param name="pieza">Pieza de la cual se quieren verificar las esquinas.</param>
        /// <returns>TRUE si colisiona, FALSE si no.</returns>
        internal bool Colisiona (Pieza pieza)
        {
            // Esquina superior derecha
            return (pieza.MaxX >= MinX
                && pieza.MaxX <= MaxX
                && pieza.MaxY >= MinY
                && pieza.MaxY <= MaxY)
                || // Esquina superior izquierda
                (pieza.MinX >= MinX
                && pieza.MinX <= MaxX
                && pieza.MaxY >= MinY
                && pieza.MaxY <= MaxY)
                || // Esquina inferior derecha
                (pieza.MaxX >= MinX
                && pieza.MaxX <= MaxX
                && pieza.MinY >= MinY
                && pieza.MinY <= MaxY)
                || // Esquina inferior izquierda
                (pieza.MinX >= MinX
                && pieza.MinX <= MaxX
                && pieza.MinY >= MinY
                && pieza.MinY <= MaxY);
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
            X = Double.Parse(xmlPieza["x"].InnerText);
            Y = Double.Parse(xmlPieza["x"].InnerText);
            Ancho = Double.Parse(xmlPieza["ancho"].InnerText);
            Alto = Double.Parse(xmlPieza["alto"].InnerText);
            Largo = Double.Parse(xmlPieza["largo"].InnerText);
            Orientacion = Double.Parse(xmlPieza["orientacion"].InnerText);
        }
    }
}
