using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    public partial class FrmIntro : Form
    {
        private Panel pnlContenedor = null!;
        private Label lblEstadoSQL = null!;
        private Button btnAudio = null!;
        private Label lblCodigoSistema = null!;
        private Label lblTitulo = null!;
        private Label lblSubtitulo = null!;
        private Label lblNarrativa = null!;
        private Panel pnlBarraFondo = null!;
        private Panel pnlBarraProgreso = null!;
        private Label lblPorcentaje = null!;
        private Button btnIniciar = null!;

        private System.Windows.Forms.Timer timerBoot = null!;
        private System.Windows.Forms.Timer timerNarrativa = null!;

        private int progreso = 0;
        private int indiceTexto = 0;
        private readonly string mensajeIntro =
            "ALERTA DIMENSIONAL EN NEO COCHABAMBA...\n" +
            "Una brecha cuántica ha liberado entidades hostiles en los sectores orbitales.\n" +
            "La IA Táctica IRIS requiere un Cadete operativo para contener la Anomalía Omega.";

        public FrmIntro()
        {
            InitializeComponent();
            ConfigurarVentana();
            ConstruirInterfaz();
            IniciarSecuencia();
        }

        private void ConfigurarVentana()
        {
            this.Text = "NEXUS // Protocolo de Contención Multiversal";
            this.ClientSize = new Size(1100, 660);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(6, 10, 20);
            this.BackgroundImage = Properties.Resources.FrmIntro;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void ConstruirInterfaz()
        {
            // Indicador de conexión a SQL Server (arriba a la izquierda)
            lblEstadoSQL = new Label
            {
                Text = "● VERIFICANDO ENLACE SQL...",
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 200, 70),
                BackColor = Color.FromArgb(190, 10, 16, 30),
                AutoSize = false,
                Size = new Size(270, 30),
                Location = new Point(22, 18),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblEstadoSQL);

            // Botón de silencio / sonido (arriba a la derecha)
            btnAudio = new Button
            {
                Text = "🔊 AUDIO: ON",
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 240, 255),
                BackColor = Color.FromArgb(190, 10, 16, 30),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(135, 32),
                Location = new Point(940, 18),
                Cursor = Cursors.Hand
            };
            btnAudio.FlatAppearance.BorderColor = Color.FromArgb(0, 180, 210);
            btnAudio.FlatAppearance.BorderSize = 1;
            btnAudio.Click += (s, e) =>
            {
                GestorAudio.AlternarSilencio();
                btnAudio.Text = GestorAudio.Silenciado ? "🔇 AUDIO: OFF" : "🔊 AUDIO: ON";
                btnAudio.ForeColor = GestorAudio.Silenciado ? Color.Salmon : Color.FromArgb(0, 240, 255);
            };
            this.Controls.Add(btnAudio);

            // Panel HUD translúcido inferior
            pnlContenedor = new Panel
            {
                Size = new Size(860, 295),
                Location = new Point((this.ClientSize.Width - 860) / 2, 330),
                BackColor = Color.FromArgb(205, 8, 14, 28)
            };
            pnlContenedor.Paint += (s, e) =>
            {
                using Pen borde = new Pen(Color.FromArgb(0, 210, 255), 2);
                e.Graphics.DrawRectangle(borde, 1, 1, pnlContenedor.Width - 3, pnlContenedor.Height - 3);
            };
            this.Controls.Add(pnlContenedor);

            lblCodigoSistema = new Label
            {
                Text = "[ TRANSMISIÓN PRIORITARIA // NÚCLEO CENTRAL NEXUS-09 ]",
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 220, 255),
                AutoSize = false,
                Size = new Size(820, 22),
                Location = new Point(20, 14),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlContenedor.Controls.Add(lblCodigoSistema);

            lblTitulo = new Label
            {
                Text = "N E X U S",
                Font = new Font("Segoe UI", 34f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(820, 58),
                Location = new Point(20, 34),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlContenedor.Controls.Add(lblTitulo);

            lblSubtitulo = new Label
            {
                Text = "OPERACIÓN: RESCATE EN EL ABISMO CUÁNTICO",
                Font = new Font("Consolas", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(110, 245, 210),
                AutoSize = false,
                Size = new Size(820, 24),
                Location = new Point(20, 92),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlContenedor.Controls.Add(lblSubtitulo);

            lblNarrativa = new Label
            {
                Text = "",
                Font = new Font("Consolas", 10f, FontStyle.Regular),
                ForeColor = Color.FromArgb(215, 230, 245),
                AutoSize = false,
                Size = new Size(780, 62),
                Location = new Point(40, 122),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlContenedor.Controls.Add(lblNarrativa);

            // Barra de carga personalizada
            pnlBarraFondo = new Panel
            {
                Size = new Size(680, 14),
                Location = new Point(90, 192),
                BackColor = Color.FromArgb(22, 34, 58)
            };
            pnlContenedor.Controls.Add(pnlBarraFondo);

            pnlBarraProgreso = new Panel
            {
                Size = new Size(0, 14),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(0, 230, 255)
            };
            pnlBarraFondo.Controls.Add(pnlBarraProgreso);

            lblPorcentaje = new Label
            {
                Text = "INICIALIZANDO SISTEMAS... 0%",
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(150, 190, 220),
                AutoSize = false,
                Size = new Size(680, 20),
                Location = new Point(90, 210),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlContenedor.Controls.Add(lblPorcentaje);

            btnIniciar = new Button
            {
                Text = "▶ INICIAR PROTOCOLO DE ACCESO",
                Font = new Font("Consolas", 11.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(6, 12, 24),
                BackColor = Color.FromArgb(0, 230, 255),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(340, 42),
                Location = new Point((pnlContenedor.Width - 340) / 2, 238),
                Cursor = Cursors.Hand
            };
            btnIniciar.FlatAppearance.BorderSize = 0;
            btnIniciar.MouseEnter += (s, e) => btnIniciar.BackColor = Color.FromArgb(120, 250, 255);
            btnIniciar.MouseLeave += (s, e) => btnIniciar.BackColor = Color.FromArgb(0, 230, 255);
            btnIniciar.Click += BtnIniciar_Click;
            pnlContenedor.Controls.Add(btnIniciar);
        }

        private async void IniciarSecuencia()
        {
            GestorAudio.MusicaMenu();

            // Verifica SQL Server en segundo plano para no trabar la animación
            bool sqlActivo = await Task.Run(() => ConexionSQL.ProbarConexion());
            if (!this.IsDisposed)
            {
                if (sqlActivo)
                {
                    lblEstadoSQL.Text = "● SQL SERVER: EN LÍNEA";
                    lblEstadoSQL.ForeColor = Color.FromArgb(80, 255, 150);
                }
                else
                {
                    lblEstadoSQL.Text = "● MODO LOCAL (SIN SQL)";
                    lblEstadoSQL.ForeColor = Color.FromArgb(255, 180, 70);
                }
            }

            timerNarrativa = new System.Windows.Forms.Timer { Interval = 28 };
            timerNarrativa.Tick += (s, e) =>
            {
                if (indiceTexto < mensajeIntro.Length)
                {
                    indiceTexto++;
                    lblNarrativa.Text = mensajeIntro.Substring(0, indiceTexto);
                }
                else
                {
                    timerNarrativa.Stop();
                }
            };
            timerNarrativa.Start();

            timerBoot = new System.Windows.Forms.Timer { Interval = 35 };
            timerBoot.Tick += (s, e) =>
            {
                if (progreso < 100)
                {
                    progreso += 2;
                    pnlBarraProgreso.Width = (int)(pnlBarraFondo.Width * (progreso / 100.0));
                    string fase = progreso < 40 ? "CARGANDO TELEMETRÍA..."
                                : progreso < 80 ? "SINCRONIZANDO IA IRIS..."
                                : "SISTEMAS LISTOS";
                    lblPorcentaje.Text = $"{fase} {progreso}%";
                }
                else
                {
                    timerBoot.Stop();
                    lblPorcentaje.ForeColor = Color.FromArgb(80, 255, 150);
                    lblPorcentaje.Text = "SISTEMAS OPERATIVOS AL 100% — LISTO PARA DESPLIEGUE";
                }
            };
            timerBoot.Start();
        }

        private void BtnIniciar_Click(object? sender, EventArgs e)
        {
            GestorAudio.SonidoClick();
            timerBoot?.Stop();
            timerNarrativa?.Stop();

            FrmLogin login = new FrmLogin();
            login.FormClosed += (s, args) => this.Close();
            this.Hide();
            login.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timerBoot?.Stop();
            timerNarrativa?.Stop();
            base.OnFormClosing(e);
        }
    }
}