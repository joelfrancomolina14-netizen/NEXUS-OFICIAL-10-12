namespace NEXUS_WindowsForms
{
    partial class FormCombate
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
            lblAlertaEntidad = new Label();
            lblInfoEntidad = new Label();
            lblInfoCadeteCombate = new Label();
            txtLogCombate = new RichTextBox();
            btnAtacar = new Button();
            btnEscapar = new Button();
            btnUsarObjetoCombate = new Button();
            btnAnalizarEntidad = new Button();
            SuspendLayout();
            // 
            // lblAlertaEntidad
            // 
            lblAlertaEntidad.AutoSize = true;
            lblAlertaEntidad.Font = new Font("Consolas", 14F, FontStyle.Bold);
            lblAlertaEntidad.ForeColor = Color.Red;
            lblAlertaEntidad.Location = new Point(12, 9);
            lblAlertaEntidad.Name = "lblAlertaEntidad";
            lblAlertaEntidad.Size = new Size(260, 28);
            lblAlertaEntidad.TabIndex = 0;
            lblAlertaEntidad.Text = "⚠️ ENTIDAD DETECTAD";
            // 
            // lblInfoEntidad
            // 
            lblInfoEntidad.AutoSize = true;
            lblInfoEntidad.Font = new Font("Consolas", 11F);
            lblInfoEntidad.ForeColor = Color.Yellow;
            lblInfoEntidad.Location = new Point(177, 48);
            lblInfoEntidad.Name = "lblInfoEntidad";
            lblInfoEntidad.Size = new Size(330, 22);
            lblInfoEntidad.TabIndex = 1;
            lblInfoEntidad.Text = "Entidad: Desconocida | Salud: 50";
            // 
            // lblInfoCadeteCombate
            // 
            lblInfoCadeteCombate.AutoSize = true;
            lblInfoCadeteCombate.Font = new Font("Consolas", 11F);
            lblInfoCadeteCombate.ForeColor = Color.Lime;
            lblInfoCadeteCombate.Location = new Point(298, 15);
            lblInfoCadeteCombate.Name = "lblInfoCadeteCombate";
            lblInfoCadeteCombate.Size = new Size(280, 22);
            lblInfoCadeteCombate.TabIndex = 2;
            lblInfoCadeteCombate.Text = "Tu Salud: 100% | Escudo: 20";
            // 
            // txtLogCombate
            // 
            txtLogCombate.BackColor = Color.FromArgb(10, 10, 15);
            txtLogCombate.Font = new Font("Consolas", 10F);
            txtLogCombate.ForeColor = Color.White;
            txtLogCombate.Location = new Point(40, 99);
            txtLogCombate.Name = "txtLogCombate";
            txtLogCombate.ReadOnly = true;
            txtLogCombate.Size = new Size(550, 200);
            txtLogCombate.TabIndex = 3;
            txtLogCombate.Text = "";
            // 
            // btnAtacar
            // 
            btnAtacar.BackColor = Color.FromArgb(45, 25, 30);
            btnAtacar.FlatStyle = FlatStyle.Flat;
            btnAtacar.ForeColor = Color.White;
            btnAtacar.Location = new Point(142, 324);
            btnAtacar.Name = "btnAtacar";
            btnAtacar.Size = new Size(130, 42);
            btnAtacar.TabIndex = 4;
            btnAtacar.Text = "⚔️ 1. Atacar";
            btnAtacar.UseVisualStyleBackColor = false;
            // 
            // btnEscapar
            // 
            btnEscapar.BackColor = Color.FromArgb(45, 25, 30);
            btnEscapar.FlatStyle = FlatStyle.Flat;
            btnEscapar.ForeColor = Color.White;
            btnEscapar.Location = new Point(340, 324);
            btnEscapar.Name = "btnEscapar";
            btnEscapar.Size = new Size(130, 42);
            btnEscapar.TabIndex = 5;
            btnEscapar.Text = "🏃 2. Escapar";
            btnEscapar.UseVisualStyleBackColor = false;
            // 
            // btnUsarObjetoCombate
            // 
            btnUsarObjetoCombate.BackColor = Color.FromArgb(45, 25, 30);
            btnUsarObjetoCombate.FlatStyle = FlatStyle.Flat;
            btnUsarObjetoCombate.ForeColor = Color.White;
            btnUsarObjetoCombate.Location = new Point(142, 379);
            btnUsarObjetoCombate.Name = "btnUsarObjetoCombate";
            btnUsarObjetoCombate.Size = new Size(130, 42);
            btnUsarObjetoCombate.TabIndex = 6;
            btnUsarObjetoCombate.Text = "🎒 3. Objeto";
            btnUsarObjetoCombate.UseVisualStyleBackColor = false;
            // 
            // btnAnalizarEntidad
            // 
            btnAnalizarEntidad.BackColor = Color.FromArgb(45, 25, 30);
            btnAnalizarEntidad.FlatStyle = FlatStyle.Flat;
            btnAnalizarEntidad.ForeColor = Color.White;
            btnAnalizarEntidad.Location = new Point(340, 379);
            btnAnalizarEntidad.Name = "btnAnalizarEntidad";
            btnAnalizarEntidad.Size = new Size(130, 42);
            btnAnalizarEntidad.TabIndex = 7;
            btnAnalizarEntidad.Text = "🔬 4. Analizar";
            btnAnalizarEntidad.UseVisualStyleBackColor = false;
            // 
            // FormCombate
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(25, 15, 20);
            ClientSize = new Size(602, 433);
            Controls.Add(btnAnalizarEntidad);
            Controls.Add(btnUsarObjetoCombate);
            Controls.Add(btnEscapar);
            Controls.Add(btnAtacar);
            Controls.Add(txtLogCombate);
            Controls.Add(lblInfoCadeteCombate);
            Controls.Add(lblInfoEntidad);
            Controls.Add(lblAlertaEntidad);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "FormCombate";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NEXUS - Alerta de Encuentro Hostil";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblAlertaEntidad;
        private Label lblInfoEntidad;
        private Label lblInfoCadeteCombate;
        private RichTextBox txtLogCombate;
        private Button btnAtacar;
        private Button btnEscapar;
        private Button btnUsarObjetoCombate;
        private Button btnAnalizarEntidad;
    }
}