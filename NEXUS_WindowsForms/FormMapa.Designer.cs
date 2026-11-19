namespace NEXUS_WindowsForms
{
    partial class FormMapa
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
            lblMapa = new Label();
            lblStatsMapa = new Label();
            lblEventoMapa = new Label();
            btnIzquierda = new Button();
            btnArriba = new Button();
            btnAbajo = new Button();
            btnDerecha = new Button();
            btnVolverMapa = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Consolas", 12F, FontStyle.Bold);
            label1.ForeColor = Color.Cyan;
            label1.Location = new Point(133, 24);
            label1.Name = "label1";
            label1.Size = new Size(560, 23);
            label1.TabIndex = 0;
            label1.Text = "MAPA DE CARTOGRAFÍA (Usa W, A, S, D o los botones)";
            // 
            // lblMapa
            // 
            lblMapa.BackColor = Color.Black;
            lblMapa.BorderStyle = BorderStyle.FixedSingle;
            lblMapa.Font = new Font("Consolas", 12F);
            lblMapa.ForeColor = Color.Cyan;
            lblMapa.Location = new Point(49, 74);
            lblMapa.Name = "lblMapa";
            lblMapa.Size = new Size(600, 310);
            lblMapa.TabIndex = 1;
            lblMapa.Text = "(Matriz 13x29)";
            // 
            // lblStatsMapa
            // 
            lblStatsMapa.AutoSize = true;
            lblStatsMapa.Font = new Font("Consolas", 10F);
            lblStatsMapa.ForeColor = Color.White;
            lblStatsMapa.Location = new Point(24, 403);
            lblStatsMapa.Name = "lblStatsMapa";
            lblStatsMapa.Size = new Size(423, 20);
            lblStatsMapa.TabIndex = 2;
            lblStatsMapa.Text = "Cartografía: 0.0% | Recursos: 0 | Anomalías: 0";
            // 
            // lblEventoMapa
            // 
            lblEventoMapa.AutoSize = true;
            lblEventoMapa.Font = new Font("Consolas", 10F, FontStyle.Bold);
            lblEventoMapa.ForeColor = Color.Lime;
            lblEventoMapa.Location = new Point(24, 423);
            lblEventoMapa.Name = "lblEventoMapa";
            lblEventoMapa.Size = new Size(216, 20);
            lblEventoMapa.TabIndex = 3;
            lblEventoMapa.Text = "Esperando movimiento...";
            // 
            // btnIzquierda
            // 
            btnIzquierda.BackColor = Color.FromArgb(25, 35, 50);
            btnIzquierda.FlatStyle = FlatStyle.Flat;
            btnIzquierda.ForeColor = Color.Cyan;
            btnIzquierda.Location = new Point(584, 423);
            btnIzquierda.Name = "btnIzquierda";
            btnIzquierda.Size = new Size(55, 45);
            btnIzquierda.TabIndex = 4;
            btnIzquierda.Text = "A ←";
            btnIzquierda.UseVisualStyleBackColor = false;
            // 
            // btnArriba
            // 
            btnArriba.BackColor = Color.FromArgb(25, 35, 50);
            btnArriba.FlatStyle = FlatStyle.Flat;
            btnArriba.ForeColor = Color.Cyan;
            btnArriba.Location = new Point(655, 365);
            btnArriba.Name = "btnArriba";
            btnArriba.Size = new Size(55, 45);
            btnArriba.TabIndex = 5;
            btnArriba.Text = "W ↑";
            btnArriba.UseVisualStyleBackColor = false;
            // 
            // btnAbajo
            // 
            btnAbajo.BackColor = Color.FromArgb(25, 35, 50);
            btnAbajo.FlatStyle = FlatStyle.Flat;
            btnAbajo.ForeColor = Color.Cyan;
            btnAbajo.Location = new Point(655, 476);
            btnAbajo.Name = "btnAbajo";
            btnAbajo.Size = new Size(55, 45);
            btnAbajo.TabIndex = 6;
            btnAbajo.Text = "S ↓";
            btnAbajo.UseVisualStyleBackColor = false;
            // 
            // btnDerecha
            // 
            btnDerecha.BackColor = Color.FromArgb(25, 35, 50);
            btnDerecha.FlatStyle = FlatStyle.Flat;
            btnDerecha.ForeColor = Color.Cyan;
            btnDerecha.Location = new Point(724, 423);
            btnDerecha.Name = "btnDerecha";
            btnDerecha.Size = new Size(55, 45);
            btnDerecha.TabIndex = 7;
            btnDerecha.Text = "D →";
            btnDerecha.UseVisualStyleBackColor = false;
            // 
            // btnVolverMapa
            // 
            btnVolverMapa.BackColor = Color.FromArgb(140, 40, 40);
            btnVolverMapa.FlatStyle = FlatStyle.Flat;
            btnVolverMapa.ForeColor = Color.White;
            btnVolverMapa.Location = new Point(353, 466);
            btnVolverMapa.Name = "btnVolverMapa";
            btnVolverMapa.Size = new Size(180, 40);
            btnVolverMapa.TabIndex = 8;
            btnVolverMapa.Text = "VOLVER AL MENÚ (Q)";
            btnVolverMapa.UseVisualStyleBackColor = false;
            // 
            // FormMapa
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 20, 30);
            ClientSize = new Size(802, 533);
            Controls.Add(btnVolverMapa);
            Controls.Add(btnDerecha);
            Controls.Add(btnAbajo);
            Controls.Add(btnArriba);
            Controls.Add(btnIzquierda);
            Controls.Add(lblEventoMapa);
            Controls.Add(lblStatsMapa);
            Controls.Add(lblMapa);
            Controls.Add(label1);
            KeyPreview = true;
            MaximizeBox = false;
            Name = "FormMapa";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NEXUS - Mapa de Cartografía 2D";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label lblMapa;
        private Label lblStatsMapa;
        private Label lblEventoMapa;
        private Button btnIzquierda;
        private Button btnArriba;
        private Button btnAbajo;
        private Button btnDerecha;
        private Button btnVolverMapa;
    }
}