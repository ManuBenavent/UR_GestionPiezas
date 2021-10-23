using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BibliotecaPiezas;
using Microsoft.VisualBasic;
using RoboDk.API;
using RoboDk.API.Model;

namespace GestorPiezasWinForms
{
    public partial class Form1 : Form
    {
        private Tablero tablero;
        // RDK holds the main object to interact with RoboDK.
        // The RoboDK application starts when a RoboDK object is created.
        RoboDK RDK = null;

        // Keep the ROBOT item as a global variable
        RoboDK.Item ROBOT = null;
        RoboDK.Item ROBOT_BASE = null;

        // Define if the robot movements will be blocking
        const bool MOVE_BLOCKING = false;

        // Cajas destino
        RoboDK.Item CAJA1 = null;
        RoboDK.Item CAJA2 = null;
        RoboDK.Item CAJA3 = null;

        public Form1()
        {
            InitializeComponent();
            tablero = new Tablero();
        }

        // Generar piezas
        private void button1_Click(object sender, EventArgs e)
        {
            string val;
            int parsed_val;
            do
            {
                val = Interaction.InputBox("Nº de piezas a generar:", Title: "Generación de piezas", DefaultResponse: "");
                if (val == "")
                {
                    return; // Usuario presiona cancelar
                }
                try
                {
                    parsed_val = int.Parse(val);
                }
                catch (FormatException)
                {
                    parsed_val = -1;
                    Interaction.MsgBox("El valor debe ser un número entero.", MsgBoxStyle.Critical, Title: "Generación de piezas");
                }
            } while (parsed_val == -1);
            tablero.GenerarPiezas(parsed_val);
            UpdateLista();
        }

        // Guardar tablero
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                InitialDirectory = @"C:\",
                CheckFileExists = false,
                CheckPathExists = true,
                Title = "Indique ubicación del archivo",
                RestoreDirectory = true,
                DefaultExt = "xml",
                Filter = "Archivos XML (*.xml)|*.xml",
            };

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!tablero.GuardarTablero(saveFileDialog.FileName))
                {
                    Interaction.MsgBox("Se ha producido un error al intentar guardar el archivo", MsgBoxStyle.Critical, Title: "Guardar tablero");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = @"C:\",
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Indique ubicación del archivo",
                RestoreDirectory = true,
                DefaultExt = "xml",
                Filter = "Archivos XML (*.xml)|*.xml",
            };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!tablero.RecuperarTablero(openFileDialog.FileName))
                {
                    Interaction.MsgBox("Se ha producido un error al intentar cargar el tablero", MsgBoxStyle.Critical, Title: "Guardar tablero");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Start RoboDK here if we want to start it before the Form is displayed
            if (!Check_RDK())
            {
                // RoboDK starts here. We can optionally pass arguments to start it hidden or start it remotely on another computer provided the computer IP.
                // If RoboDK was already running it will just connect to the API. We can force a new RoboDK instance and specify a communication port
                RDK = new RoboDK();

                // Check if RoboDK started properly
                if (Check_RDK())
                {
                    notifybar.Text = "RoboDK is Running...";

                    // attempt to auto select the robot:
                    //SelectRobot();
                    ROBOT = RDK.AddFile(@"C:\Users\mbena\Downloads\UR3.robot");
                    ROBOT_BASE = RDK.getItem("UR3 Base");
                    CAJA1 = RDK.AddFile(@"C:\Users\mbena\Downloads\Box.stl", ROBOT_BASE);
                    CAJA1.setName("CajaDestino1");
                    Mat pose = Mat.transl(-150, -300, 0);
                    CAJA1.setPose(pose);
                    CAJA1.Scale(5);

                    CAJA2 = RDK.AddFile(@"C:\Users\mbena\Downloads\Box.stl", ROBOT_BASE);
                    CAJA2.setName("CajaDestino2");
                    pose = Mat.transl(-330, 0, 0);
                    CAJA2.setPose(pose);
                    CAJA2.Scale(5);

                    CAJA3 = RDK.AddFile(@"C:\Users\mbena\Downloads\Box.stl", ROBOT_BASE);
                    CAJA3.setName("CajaDestino2");
                    pose = Mat.transl(-150, 300, 0);
                    CAJA3.setPose(pose);
                    CAJA3.Scale(5);
                }

                // display RoboDK by default:
                mostrarRoboDK_radioButton.PerformClick();
            }
        }

        private void mostrarRoboDK_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            // skip if the radio button became unchecked
            RadioButton rad_sender = (RadioButton)sender;
            if (!rad_sender.Checked) { return; }

            // Check RoboDK connection
            if (!Check_RDK()) { return; }

            // unhook from the integrated panel it is inside the main panel
            SetParent(RDK.GetWindowHandle(), IntPtr.Zero);

            RDK.setWindowState(RoboDK.WINDOWSTATE_NORMAL); // removes Cinema mode and shows the screen
            RDK.setWindowState(RoboDK.WINDOWSTATE_MAXIMIZED); // shows maximized


            //Alternatively: RDK.ShowRoboDK();
            this.BringToFront(); // show on top of RoboDK
        }

        //////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        ///////////////// GROUP DISPLAY MODE ////////////////
        // Import SetParent/GetParent command from user32 dll to identify if the main window is a subprocess
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private void ocultarRoboDK_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            // skip if the radio button became unchecked
            RadioButton rad_sender = (RadioButton)sender;
            if (!rad_sender.Checked) { return; }

            if (!Check_RDK()) { return; }

            RDK.setWindowState(RoboDK.WINDOWSTATE_HIDDEN);
        }

        /// <summary>
        /// Check if the RDK object is ready.
        /// Returns True if the RoboDK API is available or False if the RoboDK API is not available.
        /// </summary>
        /// <returns></returns>
        public bool Check_RDK()
        {
            // check if the RDK object has been initialised:
            if (RDK == null)
            {
                notifybar.Text = "RoboDK no se ha iniciado";
                return false;
            }

            // Check if the RDK API is connected
            if (!RDK.Connected())
            {
                notifybar.Text = "Conectando a RoboDK...";
                // Attempt to connect to the RDK API
                if (!RDK.Connect())
                {
                    notifybar.Text = "La API de RoboDK no se encuentra disponible...";
                    return false;
                }
            }
            return true;
        }

        private void piezasToRoboDK_Click(object sender, EventArgs e)
        {
            notifybar.Text = "Cargando piezas";
            tablero.TableroToRoboDK(ROBOT_BASE, RDK);
            notifybar.Text = "Piezas cargadas";
            UpdateLista();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!Check_RDK()) { return; }
            // Force to stop and close RoboDK (optional)
            //RDK.CloseAllStations(); // close all stations
            // RDK.Save("path_to_save.rdk"); // save the project if desired
            CloseAllStations();
            RDK.CloseRoboDK();
            RDK = null;
        }

        /// <summary>
        /// Close all the stations available in RoboDK (top level items)
        /// </summary>
        public void CloseAllStations()
        {
            // Get all the RoboDK stations available
            RoboDK.Item[] all_stations = RDK.getItemList(RoboDK.ITEM_TYPE_STATION);
            foreach (RoboDK.Item station in all_stations)
            {
                notifybar.Text = "Closing " + station.Name();
                // this will close a station without asking to save:
                station.Delete();
            }
        }

        public void UpdateLista()
        {
            int i = 0;
            panelPiezas.Controls.Clear();
            foreach (Pieza p in tablero.Piezas)
            {
                PiezaForm piezaForm = new PiezaForm(tablero, p, ROBOT_BASE, RDK, this);
                piezaForm.Location = new Point(0, i*60);
                panelPiezas.Controls.Add(piezaForm);
                i++;
            }
        }
    }
}
