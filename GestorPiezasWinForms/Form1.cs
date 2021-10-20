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

        // Define if the robot movements will be blocking
        const bool MOVE_BLOCKING = false;

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
            tablero.GenerarPiezas(9);
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
            RoboDK.Item frame = RDK.AddFrame("MyFrame");
            RoboDK.Item item = RDK.AddFile(@"D:\Programs\Universidad\RoboDK\Library\box.sld", frame); //TODO: incluir una ruta mas adecuada
            double[] scale = new double[3] { 1,1,1 };
            item.Scale(scale);
            Mat pose = Mat.transl(3.0, 3.0, 1);
            item.setPose(pose);


            item = RDK.AddFile(@"D:\Programs\Universidad\RoboDK\Library\box.sld", frame); //TODO: incluir una ruta mas adecuada
            scale = new double[3] { 1,1,1 };
            item.Scale(scale);
            pose = Mat.transl(50, 50, 1);
            item.setPose(pose);
            if (item.Valid())
            {
                notifybar.Text = "Loaded: " + item.Name();
            }
            else
            {
                notifybar.Text = "Could not load a box";
            }
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
    }
}
