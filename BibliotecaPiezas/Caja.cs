using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaPiezas
{
    /// <summary>
    /// Representa una caja destino para almacenaje
    /// </summary>
    public class Caja
    {
        /// <summary>
        /// Posición X
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Posición Y
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Referencia RoboDK
        /// </summary>
        public RoboDK.Item Item { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="X">Posición X</param>
        /// <param name="Y">Posición Y</param>
        public Caja(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
            Item = null;
        }
    }
}
