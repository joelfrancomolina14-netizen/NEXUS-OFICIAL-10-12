namespace NEXUS_WindowsForms
{
    partial class FormMenu
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblCadeteTitulo = new System.Windows.Forms.Label();
            this.lblSalud = new System.Windows.Forms.Label();
            this.pbSalud = new System.Windows.Forms.ProgressBar();
            this.lblValorSalud = new System.Windows.Forms.Label();
            this.lblEnergia = new System.Windows.Forms.Label();
            this.pbEnergia = new System.Windows.Forms.ProgressBar();
            this.lblValorEnergia = new System.Windows.Forms.Label();
            this.lblEstabilidad = new System.Windows.Forms.Label();
            this.pbEstabilidad = new System.Windows.Forms.ProgressBar();
            this.lblValorEstabilidad = new System.Windows.Forms.Label();
            this.lblPeligro = new System.Windows.Forms.Label();
            this.pbPeligro = new System.Windows.Forms.ProgressBar();
            this.lblValorPeligro = new System.Windows.Forms.Label();
            this.btnExplorar = new System.Windows.Forms.Button();
            this.btnAnalizar = new System.Windows.Forms.Button();
            this.btnEstado = new System.Windows.Forms.Button();
            this.btnRecuperar = new System.Windows.Forms.Button();
            this.btnEvaluar = new System.Windows.Forms.Button();
            this.btnDesconectar = new System.Windows.Forms.Button();
            this.btnConectar = new System.Windows.Forms.Button();
            this.btnEnviarMensaje = new System.Windows.Forms.Button();
            this.btnMapa = new System.Windows.Forms.Button();
            this.btnInventario = new System.Windows.Forms.Button();
            this.btnMisiones = new System.Windows.Forms.Button();
            this.btnExtraccion = new System.Windows.Forms.Button();
            this.txtTerminal = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // lblCadeteTitulo
            // 
            this.lblCadeteTitulo.AutoSize = true;
            this.lblCadeteTitulo.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold);
            this.lblCadeteTitulo.ForeColor = System.Drawing.Color.Cyan;
            this.lblCadeteTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblCadeteTitulo.Name = "lblCadeteTitulo";
            this.lblCadeteTitulo.Size = new System.Drawing.Size(340, 22);
            this.lblCadeteTitulo.Text = "NEXUS SYSTEM v1.0 - Centro de Mando";
            // 
            // lblSalud
            // 
            this.lblSalud.AutoSize = true;
            this.lblSalud.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.lblSalud.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblSalud.Location = new System.Drawing.Point(22, 55);
            this.lblSalud.Name = "lblSalud";
            this.lblSalud.Size = new System.Drawing.Size(55, 17);
            this.lblSalud.Text = "Salud:";
            // 
            // pbSalud
            // 
            this.pbSalud.Location = new System.Drawing.Point(125, 52);
            this.pbSalud.Name = "pbSalud";
            this.pbSalud.Size = new System.Drawing.Size(220, 22);
            this.pbSalud.Value = 100;
            // 
            // lblValorSalud
            // 
            this.lblValorSalud.AutoSize = true;
            this.lblValorSalud.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.lblValorSalud.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblValorSalud.Location = new System.Drawing.Point(355, 55);
            this.lblValorSalud.Name = "lblValorSalud";
            this.lblValorSalud.Size = new System.Drawing.Size(39, 17);
            this.lblValorSalud.Text = "100%";
            // 
            // lblEnergia
            // 
            this.lblEnergia.AutoSize = true;
            this.lblEnergia.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.lblEnergia.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblEnergia.Location = new System.Drawing.Point(22, 88);
            this.lblEnergia.Name = "lblEnergia";
            this.lblEnergia.Size = new System.Drawing.Size(71, 17);
            this.lblEnergia.Text = "Energía:";
            // 
            // pbEnergia
            // 
            this.pbEnergia.Location = new System.Drawing.Point(125, 85);
            this.pbEnergia.Name = "pbEnergia";
            this.pbEnergia.Size = new System.Drawing.Size(220, 22);
            this.pbEnergia.Value = 80;
            // 
            // lblValorEnergia
            // 
            this.lblValorEnergia.AutoSize = true;
            this.lblValorEnergia.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.lblValorEnergia.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblValorEnergia.Location = new System.Drawing.Point(355, 88);
            this.lblValorEnergia.Name = "lblValorEnergia";
            this.lblValorEnergia.Size = new System.Drawing.Size(31, 17);
            this.lblValorEnergia.Text = "80%";
            // 
            // lblEstabilidad
            // 
            this.lblEstabilidad.AutoSize = true;
            this.lblEstabilidad.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.lblEstabilidad.ForeColor = System.Drawing.Color.Gold;
            this.lblEstabilidad.Location = new System.Drawing.Point(450, 55);
            this.lblEstabilidad.Name = "lblEstabilidad";
            this.lblEstabilidad.Size = new System.Drawing.Size(103, 17);
            this.lblEstabilidad.Text = "Estabilidad:";
            // 
            // pbEstabilidad
            // 
            this.pbEstabilidad.Location = new System.Drawing.Point(565, 52);
            this.pbEstabilidad.Name = "pbEstabilidad";
            this.pbEstabilidad.Size = new System.Drawing.Size(220, 22);
            this.pbEstabilidad.Value = 85;
            // 
            // lblValorEstabilidad
            // 
            this.lblValorEstabilidad.AutoSize = true;
            this.lblValorEstabilidad.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.lblValorEstabilidad.ForeColor = System.Drawing.Color.Gold;
            this.lblValorEstabilidad.Location = new System.Drawing.Point(795, 55);
            this.lblValorEstabilidad.Name = "lblValorEstabilidad";
            this.lblValorEstabilidad.Size = new System.Drawing.Size(31, 17);
            this.lblValorEstabilidad.Text = "85%";
            // 
            // lblPeligro
            // 
            this.lblPeligro.AutoSize = true;
            this.lblPeligro.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.lblPeligro.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblPeligro.Location = new System.Drawing.Point(450, 88);
            this.lblPeligro.Name = "lblPeligro";
            this.lblPeligro.Size = new System.Drawing.Size(71, 17);
            this.lblPeligro.Text = "Peligro:";
            // 
            // pbPeligro
            // 
            this.pbPeligro.Location = new System.Drawing.Point(565, 85);
            this.pbPeligro.Name = "pbPeligro";
            this.pbPeligro.Size = new System.Drawing.Size(220, 22);
            this.pbPeligro.Value = 15;
            // 
            // lblValorPeligro
            // 
            this.lblValorPeligro.AutoSize = true;
            this.lblValorPeligro.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.lblValorPeligro.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblValorPeligro.Location = new System.Drawing.Point(795, 88);
            this.lblValorPeligro.Name = "lblValorPeligro";
            this.lblValorPeligro.Size = new System.Drawing.Size(31, 17);
            this.lblValorPeligro.Text = "15%";
            // 
            // btnExplorar
            // 
            this.btnExplorar.BackColor = System.Drawing.Color.FromArgb(25, 45, 65);
            this.btnExplorar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExplorar.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExplorar.ForeColor = System.Drawing.Color.Cyan;
            this.btnExplorar.Location = new System.Drawing.Point(22, 130);
            this.btnExplorar.Name = "btnExplorar";
            this.btnExplorar.Size = new System.Drawing.Size(195, 34);
            this.btnExplorar.Text = "1. Explorar sector";
            this.btnExplorar.UseVisualStyleBackColor = false;
            // 
            // btnAnalizar
            // 
            this.btnAnalizar.BackColor = System.Drawing.Color.FromArgb(25, 45, 65);
            this.btnAnalizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnalizar.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAnalizar.ForeColor = System.Drawing.Color.Cyan;
            this.btnAnalizar.Location = new System.Drawing.Point(22, 170);
            this.btnAnalizar.Name = "btnAnalizar";
            this.btnAnalizar.Size = new System.Drawing.Size(195, 34);
            this.btnAnalizar.Text = "2. Analizar anomalía";
            this.btnAnalizar.UseVisualStyleBackColor = false;
            // 
            // btnEstado
            // 
            this.btnEstado.BackColor = System.Drawing.Color.FromArgb(25, 45, 65);
            this.btnEstado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEstado.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnEstado.ForeColor = System.Drawing.Color.Cyan;
            this.btnEstado.Location = new System.Drawing.Point(22, 210);
            this.btnEstado.Name = "btnEstado";
            this.btnEstado.Size = new System.Drawing.Size(195, 34);
            this.btnEstado.Text = "3. Consultar estado";
            this.btnEstado.UseVisualStyleBackColor = false;
            // 
            // btnRecuperar
            // 
            this.btnRecuperar.BackColor = System.Drawing.Color.FromArgb(25, 45, 65);
            this.btnRecuperar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecuperar.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRecuperar.ForeColor = System.Drawing.Color.Cyan;
            this.btnRecuperar.Location = new System.Drawing.Point(22, 250);
            this.btnRecuperar.Name = "btnRecuperar";
            this.btnRecuperar.Size = new System.Drawing.Size(195, 34);
            this.btnRecuperar.Text = "4. Recuperar energía";
            this.btnRecuperar.UseVisualStyleBackColor = false;
            // 
            // btnEvaluar
            // 
            this.btnEvaluar.BackColor = System.Drawing.Color.FromArgb(25, 45, 65);
            this.btnEvaluar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEvaluar.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnEvaluar.ForeColor = System.Drawing.Color.Cyan;
            this.btnEvaluar.Location = new System.Drawing.Point(22, 290);
            this.btnEvaluar.Name = "btnEvaluar";
            this.btnEvaluar.Size = new System.Drawing.Size(195, 34);
            this.btnEvaluar.Text = "5. Evaluar condición";
            this.btnEvaluar.UseVisualStyleBackColor = false;
            // 
            // btnDesconectar
            // 
            this.btnDesconectar.BackColor = System.Drawing.Color.FromArgb(65, 25, 25);
            this.btnDesconectar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDesconectar.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDesconectar.ForeColor = System.Drawing.Color.Salmon;
            this.btnDesconectar.Location = new System.Drawing.Point(22, 330);
            this.btnDesconectar.Name = "btnDesconectar";
            this.btnDesconectar.Size = new System.Drawing.Size(195, 34);
            this.btnDesconectar.Text = "6. Desconectar";
            this.btnDesconectar.UseVisualStyleBackColor = false;
            // 
            // btnConectar
            // 
            this.btnConectar.BackColor = System.Drawing.Color.FromArgb(25, 45, 65);
            this.btnConectar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConectar.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnConectar.ForeColor = System.Drawing.Color.Cyan;
            this.btnConectar.Location = new System.Drawing.Point(225, 130);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(195, 34);
            this.btnConectar.Text = "7. Conectar equipo";
            this.btnConectar.UseVisualStyleBackColor = false;
            // 
            // btnEnviarMensaje
            // 
            this.btnEnviarMensaje.BackColor = System.Drawing.Color.FromArgb(25, 45, 65);
            this.btnEnviarMensaje.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviarMensaje.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnEnviarMensaje.ForeColor = System.Drawing.Color.Cyan;
            this.btnEnviarMensaje.Location = new System.Drawing.Point(225, 170);
            this.btnEnviarMensaje.Name = "btnEnviarMensaje";
            this.btnEnviarMensaje.Size = new System.Drawing.Size(195, 34);
            this.btnEnviarMensaje.Text = "8. Enviar mensaje";
            this.btnEnviarMensaje.UseVisualStyleBackColor = false;
            // 
            // btnMapa
            // 
            this.btnMapa.BackColor = System.Drawing.Color.FromArgb(25, 45, 65);
            this.btnMapa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMapa.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnMapa.ForeColor = System.Drawing.Color.Cyan;
            this.btnMapa.Location = new System.Drawing.Point(225, 210);
            this.btnMapa.Name = "btnMapa";
            this.btnMapa.Size = new System.Drawing.Size(195, 34);
            this.btnMapa.Text = "9. Mapa táctico";
            this.btnMapa.UseVisualStyleBackColor = false;
            // 
            // btnInventario
            // 
            this.btnInventario.BackColor = System.Drawing.Color.FromArgb(25, 45, 65);
            this.btnInventario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInventario.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnInventario.ForeColor = System.Drawing.Color.Cyan;
            this.btnInventario.Location = new System.Drawing.Point(225, 250);
            this.btnInventario.Name = "btnInventario";
            this.btnInventario.Size = new System.Drawing.Size(195, 34);
            this.btnInventario.Text = "10. Inventario";
            this.btnInventario.UseVisualStyleBackColor = false;
            // 
            // btnMisiones
            // 
            this.btnMisiones.BackColor = System.Drawing.Color.FromArgb(25, 45, 65);
            this.btnMisiones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMisiones.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnMisiones.ForeColor = System.Drawing.Color.Cyan;
            this.btnMisiones.Location = new System.Drawing.Point(225, 290);
            this.btnMisiones.Name = "btnMisiones";
            this.btnMisiones.Size = new System.Drawing.Size(195, 34);
            this.btnMisiones.Text = "11. Misiones";
            this.btnMisiones.UseVisualStyleBackColor = false;
            // 
            // btnExtraccion
            // 
            this.btnExtraccion.BackColor = System.Drawing.Color.FromArgb(20, 75, 45);
            this.btnExtraccion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExtraccion.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExtraccion.ForeColor = System.Drawing.Color.Lime;
            this.btnExtraccion.Location = new System.Drawing.Point(225, 330);
            this.btnExtraccion.Name = "btnExtraccion";
            this.btnExtraccion.Size = new System.Drawing.Size(195, 34);
            this.btnExtraccion.Text = "12. Extracción";
            this.btnExtraccion.UseVisualStyleBackColor = false;
            // 
            // txtTerminal
            // 
            this.txtTerminal.BackColor = System.Drawing.Color.Black;
            this.txtTerminal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTerminal.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtTerminal.ForeColor = System.Drawing.Color.Lime;
            this.txtTerminal.Location = new System.Drawing.Point(440, 130);
            this.txtTerminal.Name = "txtTerminal";
            this.txtTerminal.ReadOnly = true;
            this.txtTerminal.Size = new System.Drawing.Size(510, 420);
            this.txtTerminal.Text = "";
            // 
            // FormMenu
            // 
            this.BackColor = System.Drawing.Color.FromArgb(15, 20, 30);
            this.ClientSize = new System.Drawing.Size(980, 580);
            this.Controls.Add(this.txtTerminal);
            this.Controls.Add(this.btnExtraccion);
            this.Controls.Add(this.btnMisiones);
            this.Controls.Add(this.btnInventario);
            this.Controls.Add(this.btnMapa);
            this.Controls.Add(this.btnEnviarMensaje);
            this.Controls.Add(this.btnConectar);
            this.Controls.Add(this.btnDesconectar);
            this.Controls.Add(this.btnEvaluar);
            this.Controls.Add(this.btnRecuperar);
            this.Controls.Add(this.btnEstado);
            this.Controls.Add(this.btnAnalizar);
            this.Controls.Add(this.btnExplorar);
            this.Controls.Add(this.lblValorPeligro);
            this.Controls.Add(this.pbPeligro);
            this.Controls.Add(this.lblPeligro);
            this.Controls.Add(this.lblValorEstabilidad);
            this.Controls.Add(this.pbEstabilidad);
            this.Controls.Add(this.lblEstabilidad);
            this.Controls.Add(this.lblValorEnergia);
            this.Controls.Add(this.pbEnergia);
            this.Controls.Add(this.lblEnergia);
            this.Controls.Add(this.lblValorSalud);
            this.Controls.Add(this.pbSalud);
            this.Controls.Add(this.lblSalud);
            this.Controls.Add(this.lblCadeteTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NEXUS SYSTEM - Centro de Mando";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblCadeteTitulo;
        private System.Windows.Forms.Label lblSalud;
        private System.Windows.Forms.ProgressBar pbSalud;
        private System.Windows.Forms.Label lblValorSalud;
        private System.Windows.Forms.Label lblEnergia;
        private System.Windows.Forms.ProgressBar pbEnergia;
        private System.Windows.Forms.Label lblValorEnergia;
        private System.Windows.Forms.Label lblEstabilidad;
        private System.Windows.Forms.ProgressBar pbEstabilidad;
        private System.Windows.Forms.Label lblValorEstabilidad;
        private System.Windows.Forms.Label lblPeligro;
        private System.Windows.Forms.ProgressBar pbPeligro;
        private System.Windows.Forms.Label lblValorPeligro;
        private System.Windows.Forms.Button btnExplorar;
        private System.Windows.Forms.Button btnAnalizar;
        private System.Windows.Forms.Button btnEstado;
        private System.Windows.Forms.Button btnRecuperar;
        private System.Windows.Forms.Button btnEvaluar;
        private System.Windows.Forms.Button btnDesconectar;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.Button btnEnviarMensaje;
        private System.Windows.Forms.Button btnMapa;
        private System.Windows.Forms.Button btnInventario;
        private System.Windows.Forms.Button btnMisiones;
        private System.Windows.Forms.Button btnExtraccion;
        private System.Windows.Forms.RichTextBox txtTerminal;
    }
}