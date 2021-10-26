using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private RoboDK RDK = null;

        // Keep the ROBOT item as a global variable
        private RoboDK.Item ROBOT = null;
        private RoboDK.Item ROBOT_BASE = null;
        private RoboDK.Item VENTOSA = null;

        // Define if the robot movements will be blocking
        private const bool MOVE_BLOCKING = true;

        // Cajas destino
        private RoboDK.Item CAJA1 = null;
        private RoboDK.Item CAJA2 = null;
        private RoboDK.Item CAJA3 = null;

        // Posiciones referencia
        private const double Piezas_X = 0;
        private const double Piezas_Y = 300;
        internal readonly struct CAJA
        {
            internal CAJA(double x, double y)
            {
                X = x;
                Y = y;
            }
            internal readonly double X;
            internal readonly double Y;
        };
        private readonly List<CAJA> CAJAS = new List<CAJA> { new CAJA(300, -250), new CAJA(0, -390), new CAJA(-300, -250) };

        // Constructor
        public Form1()
        {
            InitializeComponent();
            tablero = new Tablero();
        }

        // LoadForm
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
                    ROBOT = RDK.AddFile(Utils.AssemblyDirectory + @"\Resources\UR3.robot");
                    ROBOT_BASE = RDK.getItem("UR3 Base");
                    ROBOT.MoveC(new double[] { 40, -90, -90, -45, 90, 0 }, new double[] { 90, -90, -90, -90, 90, 0 });

                    VENTOSA = RDK.AddFile(Utils.AssemblyDirectory + @"\Resources\Ventosa-Tool.stl");
                    VENTOSA.setPoseTool(Mat.transl(0, 0, 50));

                    RoboDK.Item item = RDK.AddFile(Utils.AssemblyDirectory + @"\Resources\Pieza.stl", ROBOT_BASE);
                    double[] scale = new double[3] { 1500, 1500 , 0 };
                    item.Scale(scale);
                    item.setName("suelo");
                    item.SetColor(new List<double> { 100.0 / 255.0, 107.0 / 255.0, 99.0 / 255.0, 1 });

                    CAJA1 = RDK.AddFile(Utils.AssemblyDirectory + @"\Resources\Box.stl", ROBOT_BASE);
                    CAJA1.setName("CajaDestino1");
                    Mat pose = Mat.transl(CAJAS[0].X, CAJAS[0].Y, 0);
                    CAJA1.setPose(pose);
                    CAJA1.Scale(5);
                    CAJA1.SetColor(new List<double> { 75.0 / 255.0, 54.0 / 255.0, 33.0 / 255.0, 1 });

                    CAJA2 = RDK.AddFile(Utils.AssemblyDirectory + @"\Resources\Box.stl", ROBOT_BASE);
                    CAJA2.setName("CajaDestino2");
                    pose = Mat.transl(CAJAS[1].X, CAJAS[1].Y, 0);
                    CAJA2.setPose(pose);
                    CAJA2.Scale(5);
                    CAJA2.SetColor(new List<double> { 75.0 / 255.0, 54.0 / 255.0, 33.0 / 255.0, 1 });

                    CAJA3 = RDK.AddFile(Utils.AssemblyDirectory + @"\Resources\Box.stl", ROBOT_BASE);
                    CAJA3.setName("CajaDestino3");
                    pose = Mat.transl(CAJAS[2].X, CAJAS[2].Y, 0);
                    CAJA3.setPose(pose);
                    CAJA3.Scale(5);
                    CAJA3.SetColor(new List<double> { 75.0 / 255.0, 54.0 / 255.0, 33.0 / 255.0, 1 });
                }
                // display RoboDK by default:
                mostrarRoboDK_radioButton.PerformClick();
                
            }
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
            if (tablero.GenerarPiezas(parsed_val))
                UpdateLista();
            else
                Interaction.MsgBox("Se han producido demasiadas colisiones, introduce un número menor de piezas.", MsgBoxStyle.Critical, Title: "Generación de piezas");

        }

        // Guardar tablero
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
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

        // Cargar tablero
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Indique ubicación del archivo",
                RestoreDirectory = true,
                DefaultExt = "xml",
                Filter = "Archivos XML (*.xml)|*.xml",
            };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (Pieza p in tablero.Piezas)
                {
                    if (p.EnSimulador)
                        p.Item.Delete();
                }
                tablero.Piezas.Clear();
                if (!tablero.RecuperarTablero(openFileDialog.FileName))
                {
                    Interaction.MsgBox("Se ha producido un error al intentar cargar el tablero", MsgBoxStyle.Critical, Title: "Guardar tablero");
                }
                UpdateLista();
            }
        }

        // Mostrar RoboDK
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

        // Ocultar RoboDK         
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
            tablero.TableroToRoboDK(ROBOT_BASE, RDK, ROBOT);
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
                PiezaForm piezaForm = new PiezaForm(tablero, p, ROBOT_BASE, RDK, this, ROBOT);
                piezaForm.Location = new Point(0, i*60);
                panelPiezas.Controls.Add(piezaForm);
                i++;
            }
        }

        // Recoger piezas
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Pieza pieza in tablero.Piezas)
                {
                    // Encima de la pieza
                    MovimientoRobot.MovimientoHorizontal(ROBOT, pieza.X, pieza.Y);

                    // TODO: falta orientar y elegir ventosa
                    // Bajo a por la pieza
                    double altura_previa = ROBOT.Pose().ToTxyzRxyz()[2];
                    MovimientoRobot.MovimientoVertical(ROBOT, pieza.Alto + 2); //2mm de margen para evitar la colisión
                    // TODO: recoger pieza

                    // Vuelvo a altura referencia
                    MovimientoRobot.MovimientoVertical(ROBOT, altura_previa);
                }
                // Elegimos una caja aleatoria
                Random rdn = new Random();
                CAJA caja = CAJAS[rdn.Next(0, 3)];
                // Vamos a un radio conocido (390) en el cual se encuentran las 3 cajas, mantenemos la X y calculamos una nueva Y
                MovimientoRobot.MovimientoHorizontal(ROBOT, ROBOT.Pose().ToTxyzRxyz()[0], Math.Sqrt(Math.Pow(390,2) - Math.Pow((ROBOT.Pose().ToTxyzRxyz()[0]),2)));

                // Calculamos grados de movimiento en función de pos actual
                double x = ROBOT.Pose().ToTxyzRxyz()[0];
                double y = ROBOT.Pose().ToTxyzRxyz()[1];
                double alpha = Utils.RadiansToDegrees(Math.Asin((Utils.EuclideanDistance(x, y, caja.X, caja.Y) / 2) / 390))*2; // Valor siempre de 0 a 180

                // Mediante la ecuacion de la recta determinar el signo de alpha
                alpha *= Utils.GiroPosNeg(x, y, caja.X, caja.Y);

                // Movimiento circular: las 3 cajas en su radio
                MovimientoRobot.MoverBase(ROBOT, alpha);

                // TODO: dejar piezas (asegurar todas dentro)

                // Deshacemos giro de la base
                MovimientoRobot.MoverBase(ROBOT, -1*alpha);
            }catch(Exception excp)
            {
                notifybar.Text = "El robot no se pudo mover a la posición " + excp.Message;
            }
        }
    }
}
