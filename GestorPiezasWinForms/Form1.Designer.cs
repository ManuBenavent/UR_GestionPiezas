
namespace GestorPiezasWinForms
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupRoboDKWindow = new System.Windows.Forms.GroupBox();
            this.ocultarRoboDK_radioButton = new System.Windows.Forms.RadioButton();
            this.mostrarRoboDK_radioButton = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.notifybar = new System.Windows.Forms.ToolStripStatusLabel();
            this.piezasToRoboDK = new System.Windows.Forms.Button();
            this.groupRoboDKWindow.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 10);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 52);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generar piezas aleatorias";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(118, 11);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 52);
            this.button2.TabIndex = 1;
            this.button2.Text = "Guardar tablero";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(227, 11);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 52);
            this.button3.TabIndex = 2;
            this.button3.Text = "Cargar tablero";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupRoboDKWindow
            // 
            this.groupRoboDKWindow.Controls.Add(this.ocultarRoboDK_radioButton);
            this.groupRoboDKWindow.Controls.Add(this.mostrarRoboDK_radioButton);
            this.groupRoboDKWindow.Location = new System.Drawing.Point(336, 11);
            this.groupRoboDKWindow.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupRoboDKWindow.Name = "groupRoboDKWindow";
            this.groupRoboDKWindow.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupRoboDKWindow.Size = new System.Drawing.Size(150, 68);
            this.groupRoboDKWindow.TabIndex = 3;
            this.groupRoboDKWindow.TabStop = false;
            this.groupRoboDKWindow.Text = "Modo visualización";
            // 
            // ocultarRoboDK_radioButton
            // 
            this.ocultarRoboDK_radioButton.AutoSize = true;
            this.ocultarRoboDK_radioButton.Location = new System.Drawing.Point(4, 40);
            this.ocultarRoboDK_radioButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ocultarRoboDK_radioButton.Name = "ocultarRoboDK_radioButton";
            this.ocultarRoboDK_radioButton.Size = new System.Drawing.Size(103, 17);
            this.ocultarRoboDK_radioButton.TabIndex = 1;
            this.ocultarRoboDK_radioButton.TabStop = true;
            this.ocultarRoboDK_radioButton.Text = "Ocultar RoboDK";
            this.ocultarRoboDK_radioButton.UseVisualStyleBackColor = true;
            this.ocultarRoboDK_radioButton.CheckedChanged += new System.EventHandler(this.ocultarRoboDK_radioButton_CheckedChanged);
            // 
            // mostrarRoboDK_radioButton
            // 
            this.mostrarRoboDK_radioButton.AutoSize = true;
            this.mostrarRoboDK_radioButton.Location = new System.Drawing.Point(5, 18);
            this.mostrarRoboDK_radioButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mostrarRoboDK_radioButton.Name = "mostrarRoboDK_radioButton";
            this.mostrarRoboDK_radioButton.Size = new System.Drawing.Size(104, 17);
            this.mostrarRoboDK_radioButton.TabIndex = 0;
            this.mostrarRoboDK_radioButton.TabStop = true;
            this.mostrarRoboDK_radioButton.Text = "Mostrar RoboDK";
            this.mostrarRoboDK_radioButton.UseVisualStyleBackColor = true;
            this.mostrarRoboDK_radioButton.CheckedChanged += new System.EventHandler(this.mostrarRoboDK_radioButton_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notifybar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 351);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(500, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // notifybar
            // 
            this.notifybar.Name = "notifybar";
            this.notifybar.Size = new System.Drawing.Size(86, 17);
            this.notifybar.Text = "Notificaciones:";
            // 
            // piezasToRoboDK
            // 
            this.piezasToRoboDK.Location = new System.Drawing.Point(12, 67);
            this.piezasToRoboDK.Name = "piezasToRoboDK";
            this.piezasToRoboDK.Size = new System.Drawing.Size(105, 51);
            this.piezasToRoboDK.TabIndex = 5;
            this.piezasToRoboDK.Text = "Añadir piezas";
            this.piezasToRoboDK.UseVisualStyleBackColor = true;
            this.piezasToRoboDK.Click += new System.EventHandler(this.piezasToRoboDK_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 373);
            this.Controls.Add(this.piezasToRoboDK);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupRoboDKWindow);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Gestion de piezas";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupRoboDKWindow.ResumeLayout(false);
            this.groupRoboDKWindow.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupRoboDKWindow;
        private System.Windows.Forms.RadioButton mostrarRoboDK_radioButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel notifybar;
        private System.Windows.Forms.RadioButton ocultarRoboDK_radioButton;
        private System.Windows.Forms.Button piezasToRoboDK;
    }
}

