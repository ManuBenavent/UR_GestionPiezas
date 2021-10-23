using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BibliotecaPiezas;

namespace GestorPiezasWinForms
{
    public partial class PiezaForm : UserControl
    {
        Tablero tablero;
        Pieza pieza;
        RoboDK.Item ref_frame;
        RoboDK RDK;
        Form1 formSender;
        public PiezaForm(Tablero tablero, Pieza pieza, RoboDK.Item ref_frame, RoboDK RDK, Form1 formSender)
        {
            InitializeComponent();
            this.tablero = tablero;
            this.pieza = pieza;
            this.ref_frame = ref_frame;
            this.RDK = RDK;
            this.formSender = formSender;
        }

        private void PiezaForm_Load(object sender, EventArgs e)
        {
            textBox_X.Text = pieza.X.ToString();
            textBox_Y.Text = pieza.Y.ToString();
            textBox_Ancho.Text = pieza.Ancho.ToString();
            textBox_Largo.Text = pieza.Largo.ToString();
            textBox_Alto.Text = pieza.Alto.ToString();
            textBox_Orientacion.Text = pieza.Orientacion.ToString();
            if (pieza.EnSimulador)
            {
                label7.Text = "Creada ID: " + pieza.ID;
                textBox_X.Enabled = false;
                textBox_Y.Enabled = false;
                textBox_Ancho.Enabled = false;
                textBox_Alto.Enabled = false;
                textBox_Largo.Enabled = false;
                textBox_Orientacion.Enabled = false;
                button1.Enabled = false;
                button2.Text = "Quitar del tablero";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pieza.X = int.Parse(textBox_X.Text);
            pieza.Y = int.Parse(textBox_Y.Text);
            pieza.Ancho = int.Parse(textBox_Ancho.Text);
            pieza.Alto = int.Parse(textBox_Alto.Text);
            pieza.Largo = int.Parse(textBox_Largo.Text);
            pieza.Orientacion = int.Parse(textBox_Orientacion.Text);
        }

        private void textBox_Validating(object sender, CancelEventArgs e)
        {
            int value;
            bool result = int.TryParse(((TextBox)sender).Text, out value);
            if (!result)
            {
                e.Cancel = true;
                this.errorProvider1.SetError((TextBox)sender, "Debe ser un número entero.");
            }
            else
            {
                e.Cancel = false;
                this.errorProvider1.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pieza.EnSimulador)
            {
                pieza.Item.Delete();
                pieza.EnSimulador = false;
                pieza.Item = null;
            }
            else
            {
                tablero.PiezaToRoboDK(ref_frame, RDK, pieza);
            }
            formSender.UpdateLista();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pieza.EnSimulador)
            {
                pieza.Item.Delete();
            }
            tablero.Piezas.Remove(pieza);
            formSender.UpdateLista();
        }
    }

}
