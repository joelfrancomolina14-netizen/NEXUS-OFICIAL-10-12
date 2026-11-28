using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    public partial class FrmArchivoSQL : Form
    {
        private Panel pnlPrincipal = null!;
        private Label lblEstadoConexion = null!;
        private Label lblContadorRegistros = null!;
        private TextBox txtBuscar = null!;
        private Button btnActualizar = null!;
        private Button btnExportar = null!;
        private Button btnCerrar = null!;
        private DataGridView dgvDatos = null!;

        private Button btnTabHistorial = null!;
        private Button btnTabCadetes = null!;
        private Button btnTabMisiones = null!;
        private Button btnTabCombate = null!;
        private Button btnTabInventario = null!;

        private string vistaActual = "Historial";

        public FrmArchivoSQL()
        {
            InitializeComponent();
            ConfigurarVentana();
            ConstruirInterfaz();
            CargarDatos();
        }

        private void ConfigurarVentana()
        {
            this.Text = "NEXUS // Archivo Central SQL Server (NexusDB)";
            this.ClientSize = new Size(1060, 620);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(6, 10, 20);
            this.BackgroundImage = Properties.Resources.FrmPuenteMando;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void ConstruirInterfaz()
        {
            pnlPrincipal = new Panel
            {
                Size = new Size(1020, 580),
                Location = new Point(20, 20),
                BackColor = Color.FromArgb(228, 8, 14, 28)
            };
            pnlPrincipal.Paint += (s, e) =>
            {
                using Pen borde = new Pen(Color.FromArgb(0, 210, 255), 2);
                e.Graphics.DrawRectangle(borde, 1, 1, pnlPrincipal.Width - 3, pnlPrincipal.Height - 3);
            };
            this.Controls.Add(pnlPrincipal);

            // Encabezado
            Label lblTitulo = new Label
            {
                Text = "🗄️ TERMINAL DE TELEMETRÍA // ARCHIVO CENTRAL SQL SERVER",
                Font = new Font("Consolas", 12.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 240, 255),
                Size = new Size(640, 32),
                Location = new Point(18, 14),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlPrincipal.Controls.Add(lblTitulo);

            lblEstadoConexion = new Label
            {
                Text = "● VERIFICANDO SQL...",
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = Color.Gold,
                Size = new Size(230, 32),
                Location = new Point(665, 14),
                TextAlign = ContentAlignment.MiddleRight
            };
            pnlPrincipal.Controls.Add(lblEstadoConexion);

            btnCerrar = new Button
            {
                Text = "✖ CERRAR",
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 120, 120),
                BackColor = Color.FromArgb(36, 16, 24),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 30),
                Location = new Point(904, 15),
                Cursor = Cursors.Hand
            };
            btnCerrar.FlatAppearance.BorderColor = Color.FromArgb(180, 65, 75);
            btnCerrar.Click += (s, e) =>
            {
                GestorAudio.SonidoClick();
                this.Close();
            };
            pnlPrincipal.Controls.Add(btnCerrar);

            // Botones de navegación entre las 5 tablas SQL
            btnTabHistorial = CrearBotonTabla("📜 HISTORIAL", "Historial", 18);
            btnTabCadetes = CrearBotonTabla("🎖️ CADETES", "Cadetes", 184);
            btnTabMisiones = CrearBotonTabla("🚀 MISIONES", "Misiones", 350);
            btnTabCombate = CrearBotonTabla("⚔️ COMBATE", "Combate", 516);
            btnTabInventario = CrearBotonTabla("🎒 INVENTARIO", "Inventario", 682);

            pnlPrincipal.Controls.Add(btnTabHistorial);
            pnlPrincipal.Controls.Add(btnTabCadetes);
            pnlPrincipal.Controls.Add(btnTabMisiones);
            pnlPrincipal.Controls.Add(btnTabCombate);
            pnlPrincipal.Controls.Add(btnTabInventario);

            // Barra de búsqueda y herramientas
            Label lblBuscar = new Label
            {
                Text = "FILTRAR REGISTROS:",
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(185, 215, 240),
                Size = new Size(155, 30),
                Location = new Point(18, 104),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlPrincipal.Controls.Add(lblBuscar);

            txtBuscar = new TextBox
            {
                Font = new Font("Consolas", 10.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 245, 255),
                BackColor = Color.FromArgb(16, 26, 48),
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(515, 28),
                Location = new Point(176, 105)
            };
            txtBuscar.TextChanged += (s, e) => CargarDatos();
            pnlPrincipal.Controls.Add(txtBuscar);

            btnActualizar = new Button
            {
                Text = "🔄 ACTUALIZAR",
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 240, 255),
                BackColor = Color.FromArgb(16, 30, 54),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 30),
                Location = new Point(704, 104),
                Cursor = Cursors.Hand
            };
            btnActualizar.FlatAppearance.BorderColor = Color.FromArgb(0, 180, 220);
            btnActualizar.Click += (s, e) =>
            {
                GestorAudio.SonidoClick();
                CargarDatos();
            };
            pnlPrincipal.Controls.Add(btnActualizar);

            btnExportar = new Button
            {
                Text = "💾 EXPORTAR CSV",
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(110, 245, 210),
                BackColor = Color.FromArgb(16, 36, 52),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 30),
                Location = new Point(854, 104),
                Cursor = Cursors.Hand
            };
            btnExportar.FlatAppearance.BorderColor = Color.FromArgb(80, 210, 180);
            btnExportar.Click += BtnExportar_Click;
            pnlPrincipal.Controls.Add(btnExportar);

            // Tabla de datos (DataGridView estilizado)
            dgvDatos = new DataGridView
            {
                Size = new Size(986, 392),
                Location = new Point(18, 146),
                BackgroundColor = Color.FromArgb(10, 18, 34),
                BorderStyle = BorderStyle.FixedSingle,
                GridColor = Color.FromArgb(30, 52, 82),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false
            };

            dgvDatos.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(16, 32, 58),
                ForeColor = Color.FromArgb(0, 240, 255),
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(6, 4, 6, 4)
            };
            dgvDatos.ColumnHeadersHeight = 34;

            dgvDatos.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(12, 20, 38),
                ForeColor = Color.FromArgb(220, 235, 250),
                SelectionBackColor = Color.FromArgb(0, 115, 150),
                SelectionForeColor = Color.White,
                Font = new Font("Consolas", 9f, FontStyle.Regular),
                Padding = new Padding(6, 3, 6, 3)
            };

            dgvDatos.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(16, 28, 50),
                ForeColor = Color.FromArgb(220, 235, 250),
                SelectionBackColor = Color.FromArgb(0, 115, 150),
                SelectionForeColor = Color.White
            };
            dgvDatos.RowTemplate.Height = 28;
            pnlPrincipal.Controls.Add(dgvDatos);

            // Pie de estado
            lblContadorRegistros = new Label
            {
                Text = "Registros cargados: 0",
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(140, 185, 220),
                Size = new Size(986, 24),
                Location = new Point(18, 546),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlPrincipal.Controls.Add(lblContadorRegistros);
        }

        private Button CrearBotonTabla(string texto, string claveVista, int x)
        {
            Button btn = new Button
            {
                Text = texto,
                Tag = claveVista,
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(190, 220, 245),
                BackColor = Color.FromArgb(16, 26, 46),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(158, 34),
                Location = new Point(x, 56),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(50, 90, 130);
            btn.FlatAppearance.BorderSize = 1;
            btn.Click += (s, e) =>
            {
                GestorAudio.SonidoClick();
                vistaActual = claveVista;
                CargarDatos();
            };
            return btn;
        }

        private void ActualizarEstiloTabs()
        {
            Button[] tabs = { btnTabHistorial, btnTabCadetes, btnTabMisiones, btnTabCombate, btnTabInventario };
            foreach (var b in tabs)
            {
                bool activo = b.Tag?.ToString() == vistaActual;
                b.BackColor = activo ? Color.FromArgb(0, 210, 245) : Color.FromArgb(16, 26, 46);
                b.ForeColor = activo ? Color.FromArgb(6, 12, 24) : Color.FromArgb(190, 220, 245);
            }
        }

        private void CargarDatos()
        {
            ActualizarEstiloTabs();

            bool conectado = ConexionSQL.ProbarConexion();
            if (conectado)
            {
                lblEstadoConexion.Text = "● SQLEXPRESS // NexusDB OK";
                lblEstadoConexion.ForeColor = Color.FromArgb(80, 255, 160);
            }
            else
            {
                lblEstadoConexion.Text = "● SIN CONEXIÓN A SQL";
                lblEstadoConexion.ForeColor = Color.Salmon;
            }

            DataTable dt = ConexionSQL.ConsultarTabla(vistaActual, txtBuscar.Text.Trim());
            dgvDatos.DataSource = dt;
            lblContadorRegistros.Text = $"Tabla activa: [{vistaActual.ToUpper()}]  |  Registros encontrados: {dt.Rows.Count}";
        }

        private void BtnExportar_Click(object? sender, EventArgs e)
        {
            if (dgvDatos.Rows.Count == 0)
            {
                GestorAudio.SonidoPerderVida();
                MessageBox.Show("No hay registros visibles para exportar.", "NEXUS // Exportación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            GestorAudio.SonidoClick();
            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Archivo CSV (*.csv)|*.csv",
                FileName = $"NexusDB_{vistaActual}_{DateTime.Now:yyyyMMdd_HHmm}.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dgvDatos.Columns.Count; i++)
                {
                    sb.Append(dgvDatos.Columns[i].HeaderText);
                    if (i < dgvDatos.Columns.Count - 1) sb.Append(',');
                }
                sb.AppendLine();

                foreach (DataGridViewRow row in dgvDatos.Rows)
                {
                    for (int i = 0; i < dgvDatos.Columns.Count; i++)
                    {
                        string val = row.Cells[i].Value?.ToString()?.Replace(",", ";") ?? "";
                        sb.Append(val);
                        if (i < dgvDatos.Columns.Count - 1) sb.Append(',');
                    }
                    sb.AppendLine();
                }

                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                GestorAudio.SonidoItem();
                MessageBox.Show("Reporte exportado correctamente.", "NEXUS // Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}