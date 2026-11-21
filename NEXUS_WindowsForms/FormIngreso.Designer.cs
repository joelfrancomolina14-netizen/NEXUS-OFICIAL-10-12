namespace NEXUS_WindowsForms
{
    partial class FormIngreso
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            txtNombre = new TextBox();
            label3 = new Label();
            numericUpDown1 = new NumericUpDown();
            label4 = new Label();
            txtRealidad = new TextBox();
            label5 = new Label();
            numEnergia = new NumericUpDown();
            label6 = new Label();
            numEstabilidad = new NumericUpDown();
            btnIniciar = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numEnergia).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numEstabilidad).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Consolas", 14F, FontStyle.Bold);
            label1.ForeColor = Color.Cyan;
            label1.Location = new Point(79, 9);
            label1.Name = "label1";
            label1.Size = new Size(285, 28);
            label1.TabIndex = 0;
            label1.Text = "NEXUS TRAINING SYSTEM";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(34, 40);
            label2.Name = "label2";
            label2.Size = new Size(143, 20);
            label2.TabIndex = 1;
            label2.Text = "Nombre del Cadete:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(189, 37);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(280, 27);
            txtNombre.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(71, 73);
            label3.Name = "label3";
            label3.Size = new Size(106, 20);
            label3.TabIndex = 3;
            label3.Text = "Edad (1 - 100):";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(189, 73);
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(150, 27);
            numericUpDown1.TabIndex = 4;
            numericUpDown1.Value = new decimal(new int[] { 18, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(106, 111);
            label4.Name = "label4";
            label4.Size = new Size(71, 20);
            label4.TabIndex = 5;
            label4.Text = "Realidad:";
            // 
            // txtRealidad
            // 
            txtRealidad.Location = new Point(189, 111);
            txtRealidad.Name = "txtRealidad";
            txtRealidad.Size = new Size(280, 27);
            txtRealidad.TabIndex = 6;
            txtRealidad.Text = "Neo Cochabamba";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.White;
            label5.Location = new Point(12, 144);
            label5.Name = "label5";
            label5.Size = new Size(165, 20);
            label5.TabIndex = 7;
            label5.Text = "Energía Inicial (0 - 100):";
            // 
            // numEnergia
            // 
            numEnergia.Location = new Point(189, 142);
            numEnergia.Name = "numEnergia";
            numEnergia.Size = new Size(150, 27);
            numEnergia.TabIndex = 8;
            numEnergia.Value = new decimal(new int[] { 80, 0, 0, 0 });
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.White;
            label6.Location = new Point(-6, 173);
            label6.Name = "label6";
            label6.Size = new Size(189, 20);
            label6.TabIndex = 9;
            label6.Text = "Estabilidad Inicial (0 - 100):";
            // 
            // numEstabilidad
            // 
            numEstabilidad.Location = new Point(189, 171);
            numEstabilidad.Name = "numEstabilidad";
            numEstabilidad.Size = new Size(150, 27);
            numEstabilidad.TabIndex = 10;
            numEstabilidad.Value = new decimal(new int[] { 85, 0, 0, 0 });
            // 
            // btnIniciar
            // 
            btnIniciar.BackColor = Color.FromArgb(0, 120, 140);
            btnIniciar.ForeColor = Color.White;
            btnIniciar.Location = new Point(79, 221);
            btnIniciar.Name = "btnIniciar";
            btnIniciar.Size = new Size(280, 45);
            btnIniciar.TabIndex = 11;
            btnIniciar.Text = "INICIAR SESIÓN";
            btnIniciar.UseVisualStyleBackColor = false;
            // 
            // numEdad
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 20, 30);
            ClientSize = new Size(427, 278);
            Controls.Add(btnIniciar);
            Controls.Add(numEstabilidad);
            Controls.Add(label6);
            Controls.Add(numEnergia);
            Controls.Add(label5);
            Controls.Add(txtRealidad);
            Controls.Add(label4);
            Controls.Add(numericUpDown1);
            Controls.Add(label3);
            Controls.Add(txtNombre);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "numEdad";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NEXUS TRAINING SYSTEM - Acceso";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numEnergia).EndInit();
            ((System.ComponentModel.ISupportInitialize)numEstabilidad).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtNombre;
        private Label label3;
        private NumericUpDown numericUpDown1;
        private Label label4;
        private TextBox txtRealidad;
        private Label label5;
        private NumericUpDown numEnergia;
        private Label label6;
        private NumericUpDown numEstabilidad;
        private Button btnIniciar;
    }
}