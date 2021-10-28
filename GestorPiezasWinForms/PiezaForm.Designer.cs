
namespace GestorPiezasWinForms
{
    partial class PiezaForm
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

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_X = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Y = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Ancho = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Largo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox_Alto = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Orientacion = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "X:";
            // 
            // textBox_X
            // 
            this.textBox_X.Location = new System.Drawing.Point(28, 4);
            this.textBox_X.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_X.Name = "textBox_X";
            this.textBox_X.Size = new System.Drawing.Size(64, 22);
            this.textBox_X.TabIndex = 1;
            this.textBox_X.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(99, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y:";
            // 
            // textBox_Y
            // 
            this.textBox_Y.Location = new System.Drawing.Point(121, 4);
            this.textBox_Y.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_Y.Name = "textBox_Y";
            this.textBox_Y.Size = new System.Drawing.Size(64, 22);
            this.textBox_Y.TabIndex = 3;
            this.textBox_Y.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ancho:";
            // 
            // textBox_Ancho
            // 
            this.textBox_Ancho.Location = new System.Drawing.Point(59, 39);
            this.textBox_Ancho.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_Ancho.Name = "textBox_Ancho";
            this.textBox_Ancho.Size = new System.Drawing.Size(64, 22);
            this.textBox_Ancho.TabIndex = 5;
            this.textBox_Ancho.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(140, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Largo:";
            // 
            // textBox_Largo
            // 
            this.textBox_Largo.Location = new System.Drawing.Point(191, 39);
            this.textBox_Largo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_Largo.Name = "textBox_Largo";
            this.textBox_Largo.Size = new System.Drawing.Size(64, 22);
            this.textBox_Largo.TabIndex = 7;
            this.textBox_Largo.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(261, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Alto:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(580, 4);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 49);
            this.button1.TabIndex = 9;
            this.button1.Text = "Actualizar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(667, 6);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 49);
            this.button2.TabIndex = 10;
            this.button2.Text = "Mover a tablero";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox_Alto
            // 
            this.textBox_Alto.Location = new System.Drawing.Point(299, 39);
            this.textBox_Alto.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_Alto.Name = "textBox_Alto";
            this.textBox_Alto.Size = new System.Drawing.Size(64, 22);
            this.textBox_Alto.TabIndex = 11;
            this.textBox_Alto.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(207, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Orientación:";
            // 
            // textBox_Orientacion
            // 
            this.textBox_Orientacion.Location = new System.Drawing.Point(293, 4);
            this.textBox_Orientacion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_Orientacion.Name = "textBox_Orientacion";
            this.textBox_Orientacion.Size = new System.Drawing.Size(64, 22);
            this.textBox_Orientacion.TabIndex = 13;
            this.textBox_Orientacion.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(369, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Generada";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(369, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "Ventosas";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(492, 4);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(81, 49);
            this.button3.TabIndex = 16;
            this.button3.Text = "Eliminar";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // PiezaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_Orientacion);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_Alto);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_Largo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_Ancho);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_Y);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_X);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PiezaForm";
            this.Size = new System.Drawing.Size(757, 63);
            this.Load += new System.EventHandler(this.PiezaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_X;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Y;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Ancho;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_Largo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox_Alto;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_Orientacion;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button3;
    }
}
