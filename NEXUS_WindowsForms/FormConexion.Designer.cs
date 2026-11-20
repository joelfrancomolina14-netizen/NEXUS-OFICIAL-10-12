namespace NEXUS_WindowsForms
{
    partial class FormConexion
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
            lblEstadoRed = new Label();
            label1 = new Label();
            txtIpServidor = new TextBox();
            btnIniciarServidor = new Button();
            btnConectarCliente = new Button();
            txtChatRed = new RichTextBox();
            txtMensajeRed = new TextBox();
            btnEnviarMensajeRed = new Button();
            SuspendLayout();
            // 
            // lblEstadoRed
            // 
            lblEstadoRed.AutoSize = true;
            lblEstadoRed.Font = new Font("Consolas", 11F, FontStyle.Bold);
            lblEstadoRed.ForeColor = Color.Red;
            lblEstadoRed.Location = new Point(12, 9);
            lblEstadoRed.Name = "lblEstadoRed";
            lblEstadoRed.Size = new Size(210, 22);
            lblEstadoRed.TabIndex = 0;
            lblEstadoRed.Text = "Estado: DESCONECTADO";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(20, 42);
            label1.Name = "label1";
            label1.Size = new Size(108, 20);
            label1.TabIndex = 1;
            label1.Text = "IP del Servidor:";
            // 
            // txtIpServidor
            // 
            txtIpServidor.Location = new Point(138, 39);
            txtIpServidor.Name = "txtIpServidor";
            txtIpServidor.Size = new Size(160, 27);
            txtIpServidor.TabIndex = 2;
            txtIpServidor.Text = "127.0.0.1";
            // 
            // btnIniciarServidor
            // 
            btnIniciarServidor.BackColor = Color.FromArgb(25, 35, 50);
            btnIniciarServidor.FlatStyle = FlatStyle.Flat;
            btnIniciarServidor.ForeColor = Color.Cyan;
            btnIniciarServidor.Location = new Point(20, 349);
            btnIniciarServidor.Name = "btnIniciarServidor";
            btnIniciarServidor.Size = new Size(190, 32);
            btnIniciarServidor.TabIndex = 3;
            btnIniciarServidor.Text = "1. Iniciar Servidor";
            btnIniciarServidor.UseVisualStyleBackColor = false;
            // 
            // btnConectarCliente
            // 
            btnConectarCliente.BackColor = Color.FromArgb(25, 35, 50);
            btnConectarCliente.FlatStyle = FlatStyle.Flat;
            btnConectarCliente.ForeColor = Color.Cyan;
            btnConectarCliente.Location = new Point(291, 349);
            btnConectarCliente.Name = "btnConectarCliente";
            btnConectarCliente.Size = new Size(190, 32);
            btnConectarCliente.TabIndex = 4;
            btnConectarCliente.Text = "2. Conectar como Cliente";
            btnConectarCliente.UseVisualStyleBackColor = false;
            // 
            // txtChatRed
            // 
            txtChatRed.BackColor = Color.FromArgb(10, 12, 18);
            txtChatRed.Font = new Font("Consolas", 10F);
            txtChatRed.ForeColor = Color.Cyan;
            txtChatRed.Location = new Point(27, 89);
            txtChatRed.Name = "txtChatRed";
            txtChatRed.ReadOnly = true;
            txtChatRed.Size = new Size(460, 170);
            txtChatRed.TabIndex = 5;
            txtChatRed.Text = "";
            // 
            // txtMensajeRed
            // 
            txtMensajeRed.Location = new Point(41, 108);
            txtMensajeRed.Name = "txtMensajeRed";
            txtMensajeRed.Size = new Size(340, 27);
            txtMensajeRed.TabIndex = 6;
            // 
            // btnEnviarMensajeRed
            // 
            btnEnviarMensajeRed.BackColor = Color.FromArgb(30, 100, 60);
            btnEnviarMensajeRed.FlatStyle = FlatStyle.Flat;
            btnEnviarMensajeRed.ForeColor = Color.White;
            btnEnviarMensajeRed.Location = new Point(34, 274);
            btnEnviarMensajeRed.Name = "btnEnviarMensajeRed";
            btnEnviarMensajeRed.Size = new Size(110, 28);
            btnEnviarMensajeRed.TabIndex = 7;
            btnEnviarMensajeRed.Text = "ENVIAR";
            btnEnviarMensajeRed.UseVisualStyleBackColor = false;
            // 
            // FormConexion
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 20, 30);
            ClientSize = new Size(502, 403);
            Controls.Add(btnEnviarMensajeRed);
            Controls.Add(txtMensajeRed);
            Controls.Add(txtChatRed);
            Controls.Add(btnConectarCliente);
            Controls.Add(btnIniciarServidor);
            Controls.Add(txtIpServidor);
            Controls.Add(label1);
            Controls.Add(lblEstadoRed);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormConexion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NEXUS - Enlace de Red entre Equipos";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblEstadoRed;
        private Label label1;
        private TextBox txtIpServidor;
        private Button btnIniciarServidor;
        private Button btnConectarCliente;
        private RichTextBox txtChatRed;
        private TextBox txtMensajeRed;
        private Button btnEnviarMensajeRed;
    }
}