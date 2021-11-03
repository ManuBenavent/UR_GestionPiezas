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

        // Cajas destino
        private readonly List<Caja> CAJAS = new List<Caja> { new Caja(300, -250), new Caja(0, -390), new Caja(-300, -250) };

        // Ventosas
        private List<RoboDK.Item> Ventosas = new List<RoboDK.Item>();

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
                    ROBOT.setConnectionParams("169.254.43.8", 30000, "/programs/", "root", "easybot");
                    
                    ROBOT_BASE = RDK.getItem("UR3 Base");
                    ROBOT.MoveC(new double[] { 40, -90, -90, -45, 90, 0 }, new double[] { 90, -90, -90, -90, 90, 0 });

                    VENTOSA = RDK.AddFile(Utils.AssemblyDirectory + @"\Resources\Ventosa-Tool.stl", ROBOT);
                    VENTOSA.setPoseTool(Mat.transl(0, 0, 75));
                    
                    Ventosas.Add(ROBOT.AddTool(Mat.transl(60,0,75), "ventosa1"));
                    Ventosas[0].setVisible(false);
                    Ventosas.Add(ROBOT.AddTool(Mat.transl(20, 0, 75), "ventosa2"));
                    Ventosas[1].setVisible(false);
                    Ventosas.Add(ROBOT.AddTool(Mat.transl(-20, 0, 75), "ventosa3"));
                    Ventosas[2].setVisible(false);
                    Ventosas.Add(ROBOT.AddTool(Mat.transl(-60, 0, 75), "ventosa4"));
                    Ventosas[3].setVisible(false);
                    Ventosas.Add(ROBOT.AddTool(Mat.transl(40, 0, 75), "ventosa1_2"));
                    Ventosas[4].setVisible(false);
                    Ventosas.Add(ROBOT.AddTool(Mat.transl(0, 0, 75), "ventosa2_3"));
                    Ventosas[5].setVisible(false);
                    Ventosas.Add(ROBOT.AddTool(Mat.transl(-40, 0, 75), "ventosa3_4"));
                    Ventosas[6].setVisible(false);

                    RoboDK.Item item = RDK.AddFile(Utils.AssemblyDirectory + @"\Resources\Pieza.stl", ROBOT_BASE);
                    double[] scale = new double[3] { 1500, 1500 , 0 };
                    item.Scale(scale);
                    item.setName("suelo");
                    item.SetColor(new List<double> { 100.0 / 255.0, 107.0 / 255.0, 99.0 / 255.0, 1 });

                    CAJAS[0].Item = RDK.AddFile(Utils.AssemblyDirectory + @"\Resources\Box.stl", ROBOT_BASE);
                    CAJAS[0].Item.setName("CajaDestino1");
                    Mat pose = Mat.transl(CAJAS[0].X, CAJAS[0].Y, 0);
                    CAJAS[0].Item.setPose(pose);
                    CAJAS[0].Item.Scale(5);
                    CAJAS[0].Item.SetColor(new List<double> { 75.0 / 255.0, 54.0 / 255.0, 33.0 / 255.0, 1 });

                    CAJAS[1].Item = RDK.AddFile(Utils.AssemblyDirectory + @"\Resources\Box.stl", ROBOT_BASE);
                    CAJAS[1].Item.setName("CajaDestino2");
                    pose = Mat.transl(CAJAS[1].X, CAJAS[1].Y, 0);
                    CAJAS[1].Item.setPose(pose);
                    CAJAS[1].Item.Scale(5);
                    CAJAS[1].Item.SetColor(new List<double> { 75.0 / 255.0, 54.0 / 255.0, 33.0 / 255.0, 1 });

                    CAJAS[2].Item = RDK.AddFile(Utils.AssemblyDirectory + @"\Resources\Box.stl", ROBOT_BASE);
                    CAJAS[2].Item.setName("CajaDestino3");
                    pose = Mat.transl(CAJAS[2].X, CAJAS[2].Y, 0);
                    CAJAS[2].Item.setPose(pose);
                    CAJAS[2].Item.Scale(5);
                    CAJAS[2].Item.SetColor(new List<double> { 75.0 / 255.0, 54.0 / 255.0, 33.0 / 255.0, 1 });
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

            Task<int> t = Task.Run(() =>
            {
                return tablero.GenerarPiezas(parsed_val);
            });
            try
            {
                t.Wait();
            }
            catch (AggregateException) { }

            int cantidad = t.Result;
            if (cantidad > 0)
            {
                if (cantidad < parsed_val) // Se han generado menor cantidad de piezas de las solicitadas
                    Interaction.MsgBox("No se ha podido generar el número de piezas solicitado, el máximo posible ha sido: " + cantidad, MsgBoxStyle.Information, Title: "Generación de piezas");
                UpdateLista();
            }
            else
                Interaction.MsgBox("Se han superado el número máximo de intentos para incluir una pieza en el tablero.", MsgBoxStyle.Critical, Title: "Generación de piezas");

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
                PiezaForm piezaForm = new PiezaForm(tablero, p, ROBOT_BASE, RDK, this, ROBOT)
                {
                    Location = new Point(0, i * 60)
                };
                panelPiezas.Controls.Add(piezaForm);
                i++;
            }
        }

        // Recoger piezas
        private void button4_Click(object sender, EventArgs e)
        {
            Task t = Task.Run(() =>
            {
                RecogerPiezas();
            });
            try
            {
                t.Wait();
            }
            catch (AggregateException) { }
            UpdateLista();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Task t = Task.Run(() =>
            {
                do
                {
                    RecogerPiezas();
                } while (tablero.PiezasPendientes);
            });
            try
            {
                t.Wait();
            }
            catch (AggregateException) { }
            UpdateLista();
        }

        private void RecogerPiezas()
        {
            try
            {
                List<Pieza> piezas_mov = tablero.ExtraerPiezas();
                int ventosa_id = 0;
                int pieza_id = 0;
                foreach (Pieza pieza in piezas_mov)
                {
                    if (pieza.EnZonaAmarilla)
                        if (pieza.EnZonaAmarilla && ((pieza_id - 1) < 0 || !piezas_mov[pieza_id - 1].EnZonaAmarilla))
                            MovimientoRobot.ZonaAmarilla(ROBOT);
                    // Oriento la herramienta
                    MovimientoRobot.OrientarVentosa(ROBOT, pieza.Orientacion);

                    // Selecciono posicionamiento herramienta adecuado
                    switch (pieza.Ventosas)
                    {
                        case 1: ROBOT.setTool(Ventosas[ventosa_id]); break;
                        case 2: ROBOT.setTool(Ventosas[4 + ventosa_id]); break;
                        case 3: ROBOT.setTool(Ventosas[(ventosa_id == 0) ? 1 : 2]); break;
                            // case 4: tool central por lo que no se modifica
                    }

                    // Encima de la pieza
                    MovimientoRobot.MovimientoHorizontal(ROBOT, pieza.X, pieza.Y);


                    // Bajo a por la pieza
                    double altura_previa = ROBOT.Pose().ToTxyzRxyz()[2];
                    MovimientoRobot.MovimientoVertical(ROBOT, pieza.Alto + 2); //2mm de margen para evitar la colisión

                    System.Threading.Thread.Sleep(500); // Medio segundo de espera para que se vea claro
                    pieza.Item.setParentStatic(Ventosas[ventosa_id]);

                    // Vuelvo a altura referencia
                    MovimientoRobot.MovimientoVertical(ROBOT, altura_previa);

                    // Reorientamos ventosa
                    MovimientoRobot.OrientarVentosa(ROBOT, pieza.Orientacion, reverse: true);

                    pieza.Recogida = true;
                    ventosa_id += pieza.Ventosas;

                    if (pieza.EnZonaAmarilla && ((pieza_id + 1) >= piezas_mov.Count || !piezas_mov[pieza_id + 1].EnZonaAmarilla))
                        MovimientoRobot.ZonaVerde(ROBOT);

                    pieza_id++;
                }
                ROBOT.setTool(VENTOSA);
                if (piezas_mov.Count > 0)
                {
                    // Elegimos una caja aleatoria
                    Random rdn = new Random();
                    int caja_id = rdn.Next(0, 3);
                    Caja caja = CAJAS[caja_id];

                    // Vamos a un radio conocido (390) en el cual se encuentran las 3 cajas, mantenemos la X y calculamos una nueva Y
                    MovimientoRobot.MovimientoHorizontal(ROBOT, ROBOT.Pose().ToTxyzRxyz()[0], Math.Sqrt(Math.Pow(390, 2) - Math.Pow((ROBOT.Pose().ToTxyzRxyz()[0]), 2)));

                    // Calculamos grados de movimiento en función de pos actual
                    double x = ROBOT.Pose().ToTxyzRxyz()[0];
                    double y = ROBOT.Pose().ToTxyzRxyz()[1];
                    double alpha = Utils.RadiansToDegrees(Math.Asin((Utils.EuclideanDistance(x, y, caja.X, caja.Y) / 2) / 390)) * 2; // Valor siempre de 0 a 180

                    // Mediante la ecuacion de la recta determinar el signo de alpha
                    alpha *= Utils.GiroPosNeg(x, y, caja.X, caja.Y);

                    // Movimiento circular: las 3 cajas en su radio
                    MovimientoRobot.MoverBase(ROBOT, alpha);

                    // TODO: dejar piezas (asegurar todas dentro)
                    foreach (Pieza p in piezas_mov)
                    {
                        p.Caja = caja_id + 1;
                        p.Item.setParent(caja.Item);
                    }

                    // Ajustamos posicion para tener orientacion de herramienta en 0º y en la zona de piezas
                    ROBOT.MoveJ(new double[] { 90, -90, -90, -90, 90, 0 });
                }
            }
            catch (Exception excp)
            {
                notifybar.Text = "El robot no se pudo mover a la posición " + excp.Message;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (ROBOT.ConnectedState() != RoboDK.ROBOTCOM_READY)
            {
                RDK.Finish(); // Detenemos la ejecucion
                if (ROBOT.Connect())
                {
                    RDK.setRunMode(RoboDK.RUNMODE_RUN_ROBOT);
                    ROBOT.MoveJ(new double[] { 90, -90, -90, -90, 90, 0 }); // Establecemos al robot en posicion conocida
                    notifybar.Text = "Conexión con el robot realizada con éxito";
                }
                else
                {
                    Interaction.MsgBox("No se pudo conectar con robot UR3 en 169.254.43.8:30000.", MsgBoxStyle.Critical, Title: "Conexión con robot");
                }
            }
            else
            {
                ROBOT.Disconnect();
                RDK.Finish();
                RDK.setRunMode(RoboDK.RUNMODE_SIMULATE);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                ROBOT.MoveJ(new double[] { 90, -90, -90, -90, 90, 0 }); // Establecemos al robot en posicion conocida
            }
            catch(Exception)
            {
                notifybar.Text = "No se pudo mover el robot a la posición inicial";
            }
        }
    }
}
