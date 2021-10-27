using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaPiezas
{
    public class Caja
    {
        public int X { get; set; }
        public int Y { get; set; }
        public RoboDK.Item Item { get; set; }

        public Caja(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
            Item = null;
        }
    }
}
