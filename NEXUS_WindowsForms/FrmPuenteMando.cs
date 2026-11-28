using System;
using System.Drawing;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    public partial class FrmPuenteMando : Form
    {
        private Button btnSql = null!;
        private Button btnAudio = null!;
        private Button btnCerrarSesion = null!;

        // Panel Izquierdo: Perfil y Signos Vitales
        private Panel pnlPerfil = null!;
        private Label lblNombreCadete = null!;
        private Label lblRango = null!;
        private Label lblStatsExtra = null!;
        private Panel barSalud = null!, barEscudo = null!, barEnergia = null!, barEstabilidad = null!, barPeligro = null!;
        private Label lblValSalud = null!, lblValEscudo = null!, lblValEnergia = null!, lblValEstabilidad = null!, lblValPeligro = null!;
        private Button btnRecargar = null!;

        // Panel Central: Selección de Sector
        private Panel pnlSectores = null!;
        private Panel cardSector1 = null!, cardSector2 = null!, cardSector3 = null!;
        private Label lblEstadoS2 = null!, lblEstadoS3 = null!;
        private Button btnDesplegar = null!;

        // Panel Derecho: Inventario e IA IRIS
        private Panel pnlDerecho = null!;
        private ListBox lstInventario = null!;
        private Button btnUsarObjeto = null!;
        private Label lblIrisDialogo = null!;

        public FrmPuenteMando()
        {
            InitializeComponent();
            ConfigurarVentana();
            ConstruirInterfaz();
            ActualizarUI();
            GestorAudio.MusicaMenu();
        }

        private void ConfigurarVentana()
        {
            this.Text = "NEXUS // Puente de Mando Táctico";
            this.ClientSize = new Size(1100, 660);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(6, 10, 20);
            this.BackgroundImage = Properties.Resources.FrmPuenteMando;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void ConstruirInterfaz()
        {
            // --- BARRA SUPERIOR ---
            Panel pnlTop = new Panel
            {
                Size = new Size(1056, 48),
                Location = new Point(22, 14),
                BackColor = Color.FromArgb(215, 8, 14, 28)
            };
            pnlTop.Paint += (s, e) =>
            {
                using Pen p = new Pen(Color.FromArgb(0, 200, 240), 1);
                e.Graphics.DrawRectangle(p, 0, 0, pnlTop.Width - 1, pnlTop.Height - 1);
            };
            this.Controls.Add(pnlTop);

            Label lblHeader = new Label
            {
                Text = "⚡ NEXUS // PUENTE DE MANDO TÁCTICO",
                Font = new Font("Consolas", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 240, 255),
                Size = new Size(460, 48),
                Location = new Point(16, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlTop.Controls.Add(lblHeader);

            btnSql = CrearBotonTop("🗄️ ARCHIVO SQL", 610, Color.FromArgb(110, 245, 210));
            btnSql.Size = new Size(155, 32);
            btnSql.Click += (s, e) =>
            {
                GestorAudio.SonidoClick();
                using FrmArchivoSQL frmSql = new FrmArchivoSQL();
                frmSql.ShowDialog(this);
            };
            pnlTop.Controls.Add(btnSql);

            btnAudio = CrearBotonTop(GestorAudio.Silenciado ? "🔇 AUDIO: OFF" : "🔊 AUDIO: ON", 775, Color.FromArgb(0, 240, 255));
            btnAudio.Size = new Size(130, 32);
            btnAudio.Click += (s, e) =>
            {
                GestorAudio.AlternarSilencio();
                btnAudio.Text = GestorAudio.Silenciado ? "🔇 AUDIO: OFF" : "🔊 AUDIO: ON";
                btnAudio.ForeColor = GestorAudio.Silenciado ? Color.Salmon : Color.FromArgb(0, 240, 255);
            };
            pnlTop.Controls.Add(btnAudio);

            btnCerrarSesion = CrearBotonTop("⏻ SALIR", 915, Color.FromArgb(255, 120, 120));
            btnCerrarSesion.Size = new Size(125, 32);
            btnCerrarSesion.Click += (s, e) =>
            {
                GestorAudio.SonidoClick();
                if (EstadoJuego.CadeteActual != null)
                {
                    ConexionSQL.RegistrarHistorial(EstadoJuego.CadeteActual, "CIERRE_SESION", "Cadete desconectado del Puente de Mando.");
                }
                EstadoJuego.ReiniciarSesion();
                FrmLogin login = new FrmLogin();
                login.FormClosed += (sender, args) => this.Close();
                this.Hide();
                login.Show();
            };
            pnlTop.Controls.Add(btnCerrarSesion);

            // --- COLUMNA IZQUIERDA: FICHA DEL CADETE (310px) ---
            pnlPerfil = CrearPanelHUD(22, 76, 310, 562);
            this.Controls.Add(pnlPerfil);

            PictureBox picJugador = new PictureBox
            {
                Image = Properties.Resources.jugador,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(92, 108),
                Location = new Point(16, 16),
                BackColor = Color.FromArgb(14, 24, 44)
            };
            pnlPerfil.Controls.Add(picJugador);

            lblNombreCadete = new Label
            {
                Text = "CADETE",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(180, 28),
                Location = new Point(116, 16)
            };
            pnlPerfil.Controls.Add(lblNombreCadete);

            lblRango = new Label
            {
                Text = "CADETE DE CAMPO",
                Font = new Font("Consolas", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 235, 255),
                Size = new Size(180, 20),
                Location = new Point(116, 46)
            };
            pnlPerfil.Controls.Add(lblRango);

            lblStatsExtra = new Label
            {
                Text = "XP: 0 | ATQ: 30\nRescatados: 0",
                Font = new Font("Consolas", 8.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(185, 215, 240),
                Size = new Size(180, 54),
                Location = new Point(116, 70)
            };
            pnlPerfil.Controls.Add(lblStatsExtra);

            // Barras de estado
            CrearMedidor(pnlPerfil, "SALUD INTEGRAL", 142, Color.FromArgb(65, 235, 130), out barSalud, out lblValSalud);
            CrearMedidor(pnlPerfil, "ESCUDO DE PLASMA", 204, Color.FromArgb(0, 210, 255), out barEscudo, out lblValEscudo);
            CrearMedidor(pnlPerfil, "ENERGÍA DEL TRAJE", 266, Color.FromArgb(255, 205, 60), out barEnergia, out lblValEnergia);
            CrearMedidor(pnlPerfil, "ESTABILIDAD NEURAL", 328, Color.FromArgb(180, 130, 255), out barEstabilidad, out lblValEstabilidad);
            CrearMedidor(pnlPerfil, "ÍNDICE DE PELIGRO", 390, Color.FromArgb(255, 95, 95), out barPeligro, out lblValPeligro);

            btnRecargar = new Button
            {
                Text = "⚡ RECARGAR EN BAHÍA MÉDICA",
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 240, 255),
                BackColor = Color.FromArgb(16, 32, 56),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(278, 42),
                Location = new Point(16, 498),
                Cursor = Cursors.Hand
            };
            btnRecargar.FlatAppearance.BorderColor = Color.FromArgb(0, 190, 230);
            btnRecargar.FlatAppearance.BorderSize = 1;
            btnRecargar.Click += BtnRecargar_Click;
            pnlPerfil.Controls.Add(btnRecargar);

            // --- COLUMNA CENTRAL: SECTORES DE DESPLIEGUE (416px) ---
            pnlSectores = CrearPanelHUD(346, 76, 416, 562);
            this.Controls.Add(pnlSectores);

            Label lblTituloSec = new Label
            {
                Text = "MAPA DE CONTENCIÓN // ELIGE SECTOR",
                Font = new Font("Consolas", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 240, 255),
                Size = new Size(384, 28),
                Location = new Point(16, 14)
            };
            pnlSectores.Controls.Add(lblTituloSec);

            cardSector1 = CrearTarjetaSector(
                1,
                "SECTOR 01: ESTACIÓN ORBITAL KEPLER",
                "Amenaza: Drones Sentry Bot | Dificultad: Normal\nObjetivo: Rescatar 3 cápsulas y alcanzar el portal.",
                Properties.Resources.enemigo_dron,
                52,
                out _
            );
            pnlSectores.Controls.Add(cardSector1);

            cardSector2 = CrearTarjetaSector(
                2,
                "SECTOR 02: NÚCLEO BIOLÓGICO XENO",
                "Amenaza: Xenomorfos + Drones | Dificultad: Alta\nObjetivo: Rescatar 4 cápsulas bajo radiación crítica.",
                Properties.Resources.enemigo_alien,
                192,
                out lblEstadoS2
            );
            pnlSectores.Controls.Add(cardSector2);

            cardSector3 = CrearTarjetaSector(
                3,
                "SECTOR 03: ABISMO ANOMALÍA OMEGA",
                "Amenaza: Élite + Anomalía Omega | Dificultad: Extrema\nObjetivo: Rescatar 5 cápsulas y sellar la brecha.",
                Properties.Resources.jefe_omega,
                332,
                out lblEstadoS3
            );
            pnlSectores.Controls.Add(cardSector3);

            btnDesplegar = new Button
            {
                Text = "🚀 DESPLEGAR EN SECTOR 01",
                Font = new Font("Consolas", 11.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(6, 12, 24),
                BackColor = Color.FromArgb(0, 235, 255),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(384, 52),
                Location = new Point(16, 488),
                Cursor = Cursors.Hand
            };
            btnDesplegar.FlatAppearance.BorderSize = 0;
            btnDesplegar.MouseEnter += (s, e) => btnDesplegar.BackColor = Color.FromArgb(120, 250, 255);
            btnDesplegar.MouseLeave += (s, e) => btnDesplegar.BackColor = Color.FromArgb(0, 235, 255);
            btnDesplegar.Click += BtnDesplegar_Click;
            pnlSectores.Controls.Add(btnDesplegar);

            // --- COLUMNA DERECHA: INVENTARIO E IA IRIS (302px) ---
            pnlDerecho = CrearPanelHUD(776, 76, 302, 562);
            this.Controls.Add(pnlDerecho);

            Label lblInvTitulo = new Label
            {
                Text = "ARSENAL E INVENTARIO TÁCTICO",
                Font = new Font("Consolas", 10.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(110, 245, 210),
                Size = new Size(270, 26),
                Location = new Point(16, 14)
            };
            pnlDerecho.Controls.Add(lblInvTitulo);

            lstInventario = new ListBox
            {
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(14, 24, 44),
                BorderStyle = BorderStyle.FixedSingle,
                ItemHeight = 24,
                Size = new Size(270, 210),
                Location = new Point(16, 46)
            };
            pnlDerecho.Controls.Add(lstInventario);

            btnUsarObjeto = new Button
            {
                Text = "🧪 EQUIPAR / USAR MÓDULO",
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(110, 245, 210),
                BackColor = Color.FromArgb(16, 36, 54),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(270, 38),
                Location = new Point(16, 266),
                Cursor = Cursors.Hand
            };
            btnUsarObjeto.FlatAppearance.BorderColor = Color.FromArgb(80, 210, 180);
            btnUsarObjeto.FlatAppearance.BorderSize = 1;
            btnUsarObjeto.Click += BtnUsarObjeto_Click;
            pnlDerecho.Controls.Add(btnUsarObjeto);

            // Módulo IA IRIS
            Panel pnlIris = new Panel
            {
                Size = new Size(270, 218),
                Location = new Point(16, 322),
                BackColor = Color.FromArgb(14, 24, 44)
            };
            pnlIris.Paint += (s, e) =>
            {
                using Pen p = new Pen(Color.FromArgb(0, 180, 220), 1);
                e.Graphics.DrawRectangle(p, 0, 0, pnlIris.Width - 1, pnlIris.Height - 1);
            };
            pnlDerecho.Controls.Add(pnlIris);

            PictureBox picIris = new PictureBox
            {
                Image = Properties.Resources.robot_iris,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(54, 54),
                Location = new Point(12, 12)
            };
            pnlIris.Controls.Add(picIris);

            Label lblIrisNombre = new Label
            {
                Text = "IA TÁCTICA IRIS\n[ENLACE ACTIVO]",
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 235, 255),
                Size = new Size(185, 44),
                Location = new Point(74, 16)
            };
            pnlIris.Controls.Add(lblIrisNombre);

            lblIrisDialogo = new Label
            {
                Text = "Sistemas nominales. Selecciona un sector en el mapa central para iniciar la incursión.",
                Font = new Font("Consolas", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(205, 225, 245),
                Size = new Size(246, 132),
                Location = new Point(12, 74)
            };
            pnlIris.Controls.Add(lblIrisDialogo);
        }

        private Panel CrearPanelHUD(int x, int y, int w, int h)
        {
            Panel p = new Panel
            {
                Size = new Size(w, h),
                Location = new Point(x, y),
                BackColor = Color.FromArgb(220, 8, 14, 28)
            };
            p.Paint += (s, e) =>
            {
                using Pen pen = new Pen(Color.FromArgb(0, 195, 235), 2);
                e.Graphics.DrawRectangle(pen, 1, 1, p.Width - 3, p.Height - 3);
            };
            return p;
        }

        private Button CrearBotonTop(string texto, int x, Color colorTexto)
        {
            Button b = new Button
            {
                Text = texto,
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = colorTexto,
                BackColor = Color.FromArgb(16, 26, 46),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(x, 8),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderColor = Color.FromArgb(60, 110, 150);
            b.FlatAppearance.BorderSize = 1;
            return b;
        }

        private void CrearMedidor(Panel padre, string titulo, int y, Color colorBarra, out Panel barraRelleno, out Label lblValor)
        {
            Label lbl = new Label
            {
                Text = titulo,
                Font = new Font("Consolas", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 210, 235),
                Size = new Size(180, 18),
                Location = new Point(16, y)
            };
            padre.Controls.Add(lbl);

            lblValor = new Label
            {
                Text = "100%",
                Font = new Font("Consolas", 8.5f, FontStyle.Bold),
                ForeColor = colorBarra,
                Size = new Size(95, 18),
                Location = new Point(198, y),
                TextAlign = ContentAlignment.MiddleRight
            };
            padre.Controls.Add(lblValor);

            Panel fondo = new Panel
            {
                Size = new Size(278, 14),
                Location = new Point(16, y + 22),
                BackColor = Color.FromArgb(20, 32, 54)
            };
            padre.Controls.Add(fondo);

            barraRelleno = new Panel
            {
                Size = new Size(278, 14),
                Location = new Point(0, 0),
                BackColor = colorBarra
            };
            fondo.Controls.Add(barraRelleno);
        }

        private Panel CrearTarjetaSector(int numeroSector, string titulo, string desc, Image icono, int y, out Label lblEstado)
        {
            Panel card = new Panel
            {
                Size = new Size(384, 126),
                Location = new Point(16, y),
                BackColor = Color.FromArgb(16, 26, 48),
                Cursor = Cursors.Hand,
                Tag = numeroSector
            };

            card.Paint += (s, e) =>
            {
                bool seleccionado = EstadoJuego.SectorActual == numeroSector;
                Color bordeColor = seleccionado ? Color.FromArgb(0, 245, 255) : Color.FromArgb(50, 85, 120);
                int grosor = seleccionado ? 2 : 1;
                using Pen p = new Pen(bordeColor, grosor);
                e.Graphics.DrawRectangle(p, 1, 1, card.Width - 3, card.Height - 3);
            };

            PictureBox pic = new PictureBox
            {
                Image = icono,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(68, 68),
                Location = new Point(12, 28),
                BackColor = Color.FromArgb(10, 18, 34)
            };
            card.Controls.Add(pic);

            Label lblTit = new Label
            {
                Text = titulo,
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(286, 22),
                Location = new Point(90, 10)
            };
            card.Controls.Add(lblTit);

            Label lblDesc = new Label
            {
                Text = desc,
                Font = new Font("Consolas", 8.2f, FontStyle.Regular),
                ForeColor = Color.FromArgb(185, 210, 235),
                Size = new Size(286, 54),
                Location = new Point(90, 34)
            };
            card.Controls.Add(lblDesc);

            lblEstado = new Label
            {
                Text = "● DESBLOQUEADO // LISTO",
                Font = new Font("Consolas", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 255, 150),
                Size = new Size(286, 22),
                Location = new Point(90, 92)
            };
            card.Controls.Add(lblEstado);

            EventHandler clickHandler = (s, e) => SeleccionarSector(numeroSector);
            card.Click += clickHandler;
            pic.Click += clickHandler;
            lblTit.Click += clickHandler;
            lblDesc.Click += clickHandler;
            lblEstado.Click += clickHandler;

            return card;
        }

        private void SeleccionarSector(int sector)
        {
            var c = EstadoJuego.CadeteActual;
            if (c == null) return;

            if (sector > c.NivelDesbloqueado)
            {
                GestorAudio.SonidoPerderVida();
                lblIrisDialogo.Text = $"ACCESO DENEGADO: Completa el Sector 0{c.NivelDesbloqueado} primero para desbloquear el Sector 0{sector}.";
                return;
            }

            GestorAudio.SonidoClick();
            EstadoJuego.SectorActual = sector;
            btnDesplegar.Text = $"🚀 DESPLEGAR EN SECTOR 0{sector}";
            lblIrisDialogo.Text = sector switch
            {
                1 => "Sector 01 seleccionado. Ideal para recolectar núcleos de datos y calibrar tu armamento láser.",
                2 => "Advertencia: Presencia confirmada de Xenomorfos en el Sector 02. Vigila tu escudo de plasma.",
                3 => "ALERTA MÁXIMA: La Anomalía Omega custodia el núcleo final. Asegúrate de llevar módulos de ataque.",
                _ => "Listo para el despliegue."
            };

            cardSector1.Invalidate();
            cardSector2.Invalidate();
            cardSector3.Invalidate();
        }

        public void ActualizarUI()
        {
            var c = EstadoJuego.CadeteActual;
            if (c == null) return;

            c.ActualizarPeligro();

            lblNombreCadete.Text = c.Nombre.ToUpper();
            lblRango.Text = c.Rango;
            lblStatsExtra.Text = $"Origen: {c.Realidad}\nXP: {c.XP} | ATQ: {c.AtaqueBase}\nRescatados: {c.TotalRescatados}";

            ActualizarBarra(barSalud, lblValSalud, c.Salud, 100, $"{c.Salud}/100");
            ActualizarBarra(barEscudo, lblValEscudo, c.Escudo, 100, $"{c.Escudo} PTS");
            ActualizarBarra(barEnergia, lblValEnergia, c.Energia, 100, $"{c.Energia}%");
            ActualizarBarra(barEstabilidad, lblValEstabilidad, c.Estabilidad, 100, $"{c.Estabilidad}%");
            ActualizarBarra(barPeligro, lblValPeligro, c.Peligro, 100, $"{c.Peligro}%");

            // Estado de desbloqueo de sectores
            if (c.NivelDesbloqueado >= 2)
            {
                lblEstadoS2.Text = "● DESBLOQUEADO // LISTO";
                lblEstadoS2.ForeColor = Color.FromArgb(80, 255, 150);
            }
            else
            {
                lblEstadoS2.Text = "🔒 BLOQUEADO (Completa Sector 01)";
                lblEstadoS2.ForeColor = Color.FromArgb(255, 175, 80);
            }

            if (c.NivelDesbloqueado >= 3)
            {
                lblEstadoS3.Text = "● DESBLOQUEADO // LISTO";
                lblEstadoS3.ForeColor = Color.FromArgb(80, 255, 150);
            }
            else
            {
                lblEstadoS3.Text = "🔒 BLOQUEADO (Completa Sector 02)";
                lblEstadoS3.ForeColor = Color.FromArgb(255, 175, 80);
            }

            // Inventario
            lstInventario.Items.Clear();
            foreach (var obj in c.Inventario)
            {
                lstInventario.Items.Add(obj);
            }
            if (lstInventario.Items.Count > 0) lstInventario.SelectedIndex = 0;

            btnDesplegar.Text = $"🚀 DESPLEGAR EN SECTOR 0{EstadoJuego.SectorActual}";
            cardSector1.Invalidate();
            cardSector2.Invalidate();
            cardSector3.Invalidate();
        }

        private void ActualizarBarra(Panel barra, Label lbl, int valor, int max, string texto)
        {
            int clamped = Math.Clamp(valor, 0, max);
            barra.Width = (int)(278 * (clamped / (double)max));
            lbl.Text = texto;
        }

        private void BtnRecargar_Click(object? sender, EventArgs e)
        {
            var c = EstadoJuego.CadeteActual;
            if (c == null) return;

            if (c.Salud >= 100 && c.Energia >= 100 && c.Estabilidad >= 100)
            {
                GestorAudio.SonidoClick();
                lblIrisDialogo.Text = "Tus signos vitales y energía del traje ya se encuentran al 100%.";
                return;
            }

            GestorAudio.SonidoItem();
            int rec = c.RecuperarEnergia();
            c.Estabilidad = Math.Min(100, c.Estabilidad + 15);
            c.ActualizarPeligro();

            ConexionSQL.RegistrarHistorial(c, "RECARGA_MEDICA", $"Recarga en bahía: +{rec} Energía, +10 Salud, +15 Estabilidad.");
            lblIrisDialogo.Text = $"Recarga completada: +{rec}% Energía, +10 Salud y +15% Estabilidad neural.";
            ActualizarUI();
        }

        private void BtnUsarObjeto_Click(object? sender, EventArgs e)
        {
            var c = EstadoJuego.CadeteActual;
            if (c == null || lstInventario.SelectedIndex < 0) return;

            int idx = lstInventario.SelectedIndex;
            Objeto item = c.Inventario[idx];

            GestorAudio.SonidoItem();

            switch (item.Tipo)
            {
                case TipoObjeto.Energia:
                    c.Energia = Math.Min(100, c.Energia + item.Valor);
                    c.Salud = Math.Min(100, c.Salud + 15);
                    lblIrisDialogo.Text = $"Consumiste {item.Nombre}: +{item.Valor}% Energía y +15 Salud.";
                    break;

                case TipoObjeto.Escudo:
                    c.Escudo = Math.Min(100, c.Escudo + item.Valor);
                    lblIrisDialogo.Text = $"Activaste {item.Nombre}: +{item.Valor} pts de Escudo de Plasma.";
                    break;

                case TipoObjeto.ModuloAtaque:
                    c.ModuloAtaque += item.Valor;
                    lblIrisDialogo.Text = $"Instalaste {item.Nombre}: Ataque base incrementado a {c.AtaqueBase} DMG.";
                    break;

                case TipoObjeto.Analizador:
                    c.DatosIris += item.Valor;
                    c.XP += 50;
                    lblIrisDialogo.Text = $"Analizador procesado: +{item.Valor} Datos IRIS y +50 XP.";
                    break;
            }

            c.Inventario.RemoveAt(idx);
            c.ActualizarPeligro();
            ConexionSQL.RegistrarHistorial(c, "USO_OBJETO", $"Utilizó {item.Nombre} (+{item.Valor}).");
            ActualizarUI();
        }

        private void BtnDesplegar_Click(object? sender, EventArgs e)
        {
            var c = EstadoJuego.CadeteActual;
            if (c == null) return;

            if (c.Salud <= 0) c.Salud = 100;
            if (c.Energia < 20)
            {
                GestorAudio.SonidoPerderVida();
                lblIrisDialogo.Text = "Energía crítica (<20%). Usa la Bahía Médica antes de entrar al laberinto.";
                return;
            }

            GestorAudio.SonidoClick();
            c.Explorar();
            ConexionSQL.RegistrarHistorial(c, "DESPLIEGUE_MISION", $"Incursión iniciada en {EstadoJuego.NombreSectorActual}.");

            FrmLaberinto laberinto = new FrmLaberinto();
            laberinto.FormClosed += (s, args) =>
            {
                if (!this.IsDisposed)
                {
                    GestorAudio.MusicaMenu();
                    ActualizarUI();
                    lblIrisDialogo.Text = "Incursión finalizada. Datos de combate e inventario sincronizados con SQL Server.";
                    this.Show();
                }
            };
            this.Hide();
            laberinto.Show();
        }
    }
}