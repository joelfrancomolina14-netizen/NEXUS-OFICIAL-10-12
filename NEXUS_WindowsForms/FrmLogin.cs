using System;
using System.Drawing;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    public partial class FrmLogin : Form
    {
        private Button btnVolver = null!;
        private Button btnAudio = null!;
        private Panel pnlCard = null!;
        private PictureBox picAvatar = null!;
        private PictureBox picIris = null!;
        private Label lblIrisMensaje = null!;
        private Label lblTitulo = null!;
        private Label lblSubtitulo = null!;
        private Label lblNombre = null!;
        private TextBox txtNombre = null!;
        private Label lblEdad = null!;
        private NumericUpDown numEdad = null!;
        private Label lblRealidad = null!;
        private ComboBox cmbRealidad = null!;
        private Label lblEstado = null!;
        private Button btnIngresar = null!;

        public FrmLogin()
        {
            InitializeComponent();
            ConfigurarVentana();
            ConstruirInterfaz();
            GestorAudio.MusicaMenu();
        }

        private void ConfigurarVentana()
        {
            this.Text = "NEXUS // Terminal de Identificación de Cadetes";
            this.ClientSize = new Size(1100, 660);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(6, 10, 20);
            this.BackgroundImage = Properties.Resources.FrmLogin;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void ConstruirInterfaz()
        {
            // Botón volver a la Intro
            btnVolver = new Button
            {
                Text = "◀ VOLVER",
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 210, 235),
                BackColor = Color.FromArgb(190, 10, 16, 30),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 32),
                Location = new Point(22, 18),
                Cursor = Cursors.Hand
            };
            btnVolver.FlatAppearance.BorderColor = Color.FromArgb(70, 110, 150);
            btnVolver.FlatAppearance.BorderSize = 1;
            btnVolver.Click += (s, e) =>
            {
                GestorAudio.SonidoClick();
                FrmIntro intro = new FrmIntro();
                intro.FormClosed += (sender, args) => this.Close();
                this.Hide();
                intro.Show();
            };
            this.Controls.Add(btnVolver);

            // Botón Audio
            btnAudio = new Button
            {
                Text = GestorAudio.Silenciado ? "🔇 AUDIO: OFF" : "🔊 AUDIO: ON",
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = GestorAudio.Silenciado ? Color.Salmon : Color.FromArgb(0, 240, 255),
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

            // Panel Central HUD
            pnlCard = new Panel
            {
                Size = new Size(780, 490),
                Location = new Point((this.ClientSize.Width - 780) / 2, 85),
                BackColor = Color.FromArgb(220, 8, 14, 28)
            };
            pnlCard.Paint += (s, e) =>
            {
                using Pen borde = new Pen(Color.FromArgb(0, 210, 255), 2);
                e.Graphics.DrawRectangle(borde, 1, 1, pnlCard.Width - 3, pnlCard.Height - 3);
                using Pen separador = new Pen(Color.FromArgb(45, 90, 130), 1);
                e.Graphics.DrawLine(separador, 270, 35, 270, pnlCard.Height - 35);
            };
            this.Controls.Add(pnlCard);

            // Columna Izquierda: Avatar del Cadete + Asistente IRIS
            picAvatar = new PictureBox
            {
                Image = Properties.Resources.jugador,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(200, 240),
                Location = new Point(35, 38),
                BackColor = Color.FromArgb(14, 24, 44)
            };
            picAvatar.Paint += (s, e) =>
            {
                using Pen p = new Pen(Color.FromArgb(0, 190, 230), 1);
                e.Graphics.DrawRectangle(p, 0, 0, picAvatar.Width - 1, picAvatar.Height - 1);
            };
            pnlCard.Controls.Add(picAvatar);

            Label lblBadge = new Label
            {
                Text = "UNIDAD TÁCTICA // MK-IV",
                Font = new Font("Consolas", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 230, 255),
                BackColor = Color.FromArgb(18, 35, 60),
                Size = new Size(200, 24),
                Location = new Point(35, 284),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlCard.Controls.Add(lblBadge);

            // Mini módulo IA IRIS
            picIris = new PictureBox
            {
                Image = Properties.Resources.robot_iris,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(56, 56),
                Location = new Point(35, 330),
                BackColor = Color.FromArgb(14, 24, 44)
            };
            pnlCard.Controls.Add(picIris);

            lblIrisMensaje = new Label
            {
                Text = "IA IRIS:\n\"Ingresa tu alias para cargar tu perfil en NexusDB o crear un nuevo expediente.\"",
                Font = new Font("Consolas", 8.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(170, 225, 245),
                Size = new Size(142, 95),
                Location = new Point(98, 326)
            };
            pnlCard.Controls.Add(lblIrisMensaje);

            // Columna Derecha: Formulario de Identificación
            lblTitulo = new Label
            {
                Text = "REGISTRO DE CADETE",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(450, 38),
                Location = new Point(300, 32)
            };
            pnlCard.Controls.Add(lblTitulo);

            lblSubtitulo = new Label
            {
                Text = "SINCRONIZACIÓN BIOMÉTRICA CON BASE DE DATOS NEXUS",
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 220, 255),
                Size = new Size(450, 20),
                Location = new Point(302, 72)
            };
            pnlCard.Controls.Add(lblSubtitulo);

            // Campo Nombre / Alias
            lblNombre = new Label
            {
                Text = "ALIAS O NOMBRE DEL CADETE:",
                Font = new Font("Consolas", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 220, 240),
                Size = new Size(440, 22),
                Location = new Point(302, 112)
            };
            pnlCard.Controls.Add(lblNombre);

            txtNombre = new TextBox
            {
                Font = new Font("Consolas", 12.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 245, 255),
                BackColor = Color.FromArgb(16, 26, 48),
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(440, 32),
                Location = new Point(302, 138),
                MaxLength = 30
            };
            txtNombre.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    ProcesarIngreso();
                }
            };
            pnlCard.Controls.Add(txtNombre);

            // Campo Edad
            lblEdad = new Label
            {
                Text = "EDAD OPERATIVA:",
                Font = new Font("Consolas", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 220, 240),
                Size = new Size(200, 22),
                Location = new Point(302, 188)
            };
            pnlCard.Controls.Add(lblEdad);

            numEdad = new NumericUpDown
            {
                Font = new Font("Consolas", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(16, 26, 48),
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(140, 30),
                Location = new Point(302, 214),
                Minimum = 15,
                Maximum = 99,
                Value = 19
            };
            pnlCard.Controls.Add(numEdad);

            // Campo Realidad Origen
            lblRealidad = new Label
            {
                Text = "REALIDAD / SECTOR DE ORIGEN:",
                Font = new Font("Consolas", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 220, 240),
                Size = new Size(440, 22),
                Location = new Point(302, 262)
            };
            pnlCard.Controls.Add(lblRealidad);

            cmbRealidad = new ComboBox
            {
                Font = new Font("Consolas", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(120, 250, 210),
                BackColor = Color.FromArgb(16, 26, 48),
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDown,
                Size = new Size(440, 30),
                Location = new Point(302, 288)
            };
            cmbRealidad.Items.AddRange(new object[]
            {
                "Neo Cochabamba",
                "Estación Orbital Kepler-9",
                "Distrito Andino 2099",
                "Colonia Cibernética Valkiria",
                "Realidad Cero // Núcleo"
            });
            cmbRealidad.SelectedIndex = 0;
            pnlCard.Controls.Add(cmbRealidad);

            // Mensaje de validación / estado
            lblEstado = new Label
            {
                Text = "● Si tu alias ya existe en SQL Server, se restaurará tu rango y XP.",
                Font = new Font("Consolas", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(140, 185, 220),
                Size = new Size(440, 36),
                Location = new Point(302, 336)
            };
            pnlCard.Controls.Add(lblEstado);

            // Botón Principal de Acceso
            btnIngresar = new Button
            {
                Text = "⚡ ACCEDER AL PUENTE DE MANDO",
                Font = new Font("Consolas", 11.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(6, 12, 24),
                BackColor = Color.FromArgb(0, 230, 255),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(440, 46),
                Location = new Point(302, 392),
                Cursor = Cursors.Hand
            };
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.MouseEnter += (s, e) => btnIngresar.BackColor = Color.FromArgb(120, 250, 255);
            btnIngresar.MouseLeave += (s, e) => btnIngresar.BackColor = Color.FromArgb(0, 230, 255);
            btnIngresar.Click += (s, e) => ProcesarIngreso();
            pnlCard.Controls.Add(btnIngresar);
        }

        private void ProcesarIngreso()
        {
            string nombre = txtNombre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                GestorAudio.SonidoPerderVida();
                lblEstado.ForeColor = Color.FromArgb(255, 110, 110);
                lblEstado.Text = "⚠ ERROR: Debes ingresar un alias o nombre de cadete para continuar.";
                txtNombre.Focus();
                return;
            }

            GestorAudio.SonidoClick();
            btnIngresar.Enabled = false;
            lblEstado.ForeColor = Color.FromArgb(80, 255, 160);
            lblEstado.Text = "● SINCRONIZANDO EXPEDIENTE EN SQL SERVER...";
            Application.DoEvents();

            int edad = (int)numEdad.Value;
            string realidad = string.IsNullOrWhiteSpace(cmbRealidad.Text) ? "Neo Cochabamba" : cmbRealidad.Text.Trim();

            Cadete cadete = ConexionSQL.CargarOCrearCadete(nombre, edad, realidad);
            EstadoJuego.CadeteActual = cadete;
            EstadoJuego.SectorActual = 1;

            ConexionSQL.RegistrarHistorial(
                cadete,
                "INICIO_SESION",
                $"Cadete {cadete.Nombre} ({cadete.Rango}) conectado desde {cadete.Realidad}."
            );

            FrmPuenteMando puente = new FrmPuenteMando();
            puente.FormClosed += (s, args) => this.Close();
            this.Hide();
            puente.Show();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            txtNombre.Focus();
        }
    }
}