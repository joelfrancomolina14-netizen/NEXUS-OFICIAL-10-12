using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    public partial class FrmLaberinto : Form
    {
        private const int TILE = 50;
        private const int COLS = 16;
        private const int ROWS = 12;

        private ArenaCanvas canvas = null!;
        private System.Windows.Forms.Timer timerJuego = null!;

        // Controles HUD derecho
        private Label lblSectorHud = null!;
        private Label lblObjetivoHud = null!;
        private Label lblPortalHud = null!;
        private Panel barSalud = null!, barEscudo = null!, barEnergia = null!;
        private Label lblSaludVal = null!, lblEscudoVal = null!, lblEnergiaVal = null!;
        private Label lblPuntosHud = null!;
        private Label lblIrisTactico = null!;
        private Button btnAudio = null!;
        private Button btnAbortar = null!;

        // Estado del mapa y entidades
        private int[,] mapa = new int[ROWS, COLS];
        private RectangleF jugadorRect;
        private PointF direccionVista = new PointF(1, 0);
        private readonly HashSet<Keys> teclasActivas = new HashSet<Keys>();

        private readonly List<EnemigoActivo> enemigos = new List<EnemigoActivo>();
        private readonly List<ProyectilLaser> proyectiles = new List<ProyectilLaser>();
        private readonly List<Recolectable> recolectables = new List<Recolectable>();
        private RectangleF portalRect;

        private int capsulasTotales = 0;
        private int capsulasRescatadas = 0;
        private int puntosMision = 0;
        private int cooldownDisparo = 0;
        private int cooldownDanio = 0;
        private int tickAnimacion = 0;
        private bool misionTerminada = false;

        private readonly Random rng = new Random();

        public FrmLaberinto()
        {
            InitializeComponent();
            ConfigurarVentana();
            ConstruirInterfaz();
            InicializarNivel(EstadoJuego.SectorActual);
            GestorAudio.MusicaLaberinto();
            IniciarBucleJuego();
        }

        private void ConfigurarVentana()
        {
            this.Text = $"NEXUS // Incursión Táctica — {EstadoJuego.NombreSectorActual}";
            this.ClientSize = new Size(1100, 660);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.BackColor = Color.FromArgb(6, 10, 20);
        }

        private void ConstruirInterfaz()
        {
            Panel marcoArena = new Panel
            {
                Size = new Size(804, 604),
                Location = new Point(20, 28),
                BackColor = Color.FromArgb(0, 210, 255),
                Padding = new Padding(2)
            };
            this.Controls.Add(marcoArena);

            canvas = new ArenaCanvas
            {
                Size = new Size(800, 600),
                Location = new Point(2, 2),
                BackColor = Color.FromArgb(8, 13, 24)
            };
            canvas.Paint += Canvas_Paint;
            canvas.MouseDown += Canvas_MouseDown;
            marcoArena.Controls.Add(canvas);

            Panel pnlHud = new Panel
            {
                Size = new Size(242, 604),
                Location = new Point(838, 28),
                BackColor = Color.FromArgb(10, 16, 32)
            };
            pnlHud.Paint += (s, e) =>
            {
                using Pen p = new Pen(Color.FromArgb(0, 195, 235), 2);
                e.Graphics.DrawRectangle(p, 1, 1, pnlHud.Width - 3, pnlHud.Height - 3);
            };
            this.Controls.Add(pnlHud);

            lblSectorHud = new Label
            {
                Text = $"SECTOR 0{EstadoJuego.SectorActual}",
                Font = new Font("Consolas", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 240, 255),
                Size = new Size(218, 28),
                Location = new Point(12, 12),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlHud.Controls.Add(lblSectorHud);

            lblObjetivoHud = new Label
            {
                Text = "CÁPSULAS: 0 / 3",
                Font = new Font("Consolas", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 255, 160),
                BackColor = Color.FromArgb(16, 30, 52),
                Size = new Size(218, 30),
                Location = new Point(12, 44),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlHud.Controls.Add(lblObjetivoHud);

            lblPortalHud = new Label
            {
                Text = "🔒 PORTAL SELLADO",
                Font = new Font("Consolas", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 185, 80),
                Size = new Size(218, 24),
                Location = new Point(12, 78),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlHud.Controls.Add(lblPortalHud);

            CrearBarraHud(pnlHud, "SALUD", 114, Color.FromArgb(65, 235, 130), out barSalud, out lblSaludVal);
            CrearBarraHud(pnlHud, "ESCUDO", 168, Color.FromArgb(0, 210, 255), out barEscudo, out lblEscudoVal);
            CrearBarraHud(pnlHud, "ENERGÍA LÁSER", 222, Color.FromArgb(255, 205, 60), out barEnergia, out lblEnergiaVal);

            lblPuntosHud = new Label
            {
                Text = "PUNTOS MISIÓN: 0 XP",
                Font = new Font("Consolas", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(18, 28, 50),
                Size = new Size(218, 28),
                Location = new Point(12, 280),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlHud.Controls.Add(lblPuntosHud);

            Panel pnlIris = new Panel
            {
                Size = new Size(218, 124),
                Location = new Point(12, 318),
                BackColor = Color.FromArgb(14, 24, 44)
            };
            pnlIris.Paint += (s, e) =>
            {
                using Pen p = new Pen(Color.FromArgb(0, 170, 210), 1);
                e.Graphics.DrawRectangle(p, 0, 0, pnlIris.Width - 1, pnlIris.Height - 1);
            };
            pnlHud.Controls.Add(pnlIris);

            PictureBox picIris = new PictureBox
            {
                Image = Properties.Resources.robot_iris,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(38, 38),
                Location = new Point(8, 8)
            };
            pnlIris.Controls.Add(picIris);

            Label lblIrisTit = new Label
            {
                Text = "IA IRIS // RADAR",
                Font = new Font("Consolas", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 235, 255),
                Size = new Size(160, 34),
                Location = new Point(52, 10)
            };
            pnlIris.Controls.Add(lblIrisTit);

            lblIrisTactico = new Label
            {
                Text = "Rescata todas las cápsulas criogénicas para activar el portal de extracción.",
                Font = new Font("Consolas", 8.2f, FontStyle.Regular),
                ForeColor = Color.FromArgb(205, 225, 245),
                Size = new Size(202, 68),
                Location = new Point(8, 50)
            };
            pnlIris.Controls.Add(lblIrisTactico);

            Label lblControles = new Label
            {
                Text = "CONTROLES TÁCTICOS:\n[WASD / Flechas] Moverse\n[ESPACIO / Clic] Disparar",
                Font = new Font("Consolas", 8.2f, FontStyle.Regular),
                ForeColor = Color.FromArgb(150, 185, 215),
                Size = new Size(218, 56),
                Location = new Point(12, 450)
            };
            pnlHud.Controls.Add(lblControles);

            btnAudio = new Button
            {
                Text = GestorAudio.Silenciado ? "🔇 AUDIO: OFF" : "🔊 AUDIO: ON",
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = GestorAudio.Silenciado ? Color.Salmon : Color.FromArgb(0, 240, 255),
                BackColor = Color.FromArgb(16, 26, 46),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(218, 34),
                Location = new Point(12, 514),
                TabStop = false,
                Cursor = Cursors.Hand
            };
            btnAudio.FlatAppearance.BorderColor = Color.FromArgb(0, 170, 210);
            btnAudio.Click += (s, e) =>
            {
                GestorAudio.AlternarSilencio();
                btnAudio.Text = GestorAudio.Silenciado ? "🔇 AUDIO: OFF" : "🔊 AUDIO: ON";
                btnAudio.ForeColor = GestorAudio.Silenciado ? Color.Salmon : Color.FromArgb(0, 240, 255);
                this.Focus();
            };
            pnlHud.Controls.Add(btnAudio);

            btnAbortar = new Button
            {
                Text = "🏳 ABORTAR INCURSIÓN",
                Font = new Font("Consolas", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 120, 120),
                BackColor = Color.FromArgb(36, 16, 24),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(218, 36),
                Location = new Point(12, 554),
                TabStop = false,
                Cursor = Cursors.Hand
            };
            btnAbortar.FlatAppearance.BorderColor = Color.FromArgb(180, 65, 75);
            btnAbortar.Click += (s, e) => FinalizarMision(false, "Incursión abortada por el cadete.");
            pnlHud.Controls.Add(btnAbortar);
        }

        private void CrearBarraHud(Panel padre, string titulo, int y, Color color, out Panel relleno, out Label lblVal)
        {
            Label lbl = new Label
            {
                Text = titulo,
                Font = new Font("Consolas", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(185, 215, 240),
                Size = new Size(125, 18),
                Location = new Point(12, y)
            };
            padre.Controls.Add(lbl);

            lblVal = new Label
            {
                Text = "100",
                Font = new Font("Consolas", 8.5f, FontStyle.Bold),
                ForeColor = color,
                Size = new Size(90, 18),
                Location = new Point(140, y),
                TextAlign = ContentAlignment.MiddleRight
            };
            padre.Controls.Add(lblVal);

            Panel fondo = new Panel
            {
                Size = new Size(218, 12),
                Location = new Point(12, y + 20),
                BackColor = Color.FromArgb(20, 32, 54)
            };
            padre.Controls.Add(fondo);

            relleno = new Panel
            {
                Size = new Size(218, 12),
                Location = new Point(0, 0),
                BackColor = color
            };
            fondo.Controls.Add(relleno);
        }

        private void InicializarNivel(int sector)
        {
            enemigos.Clear();
            proyectiles.Clear();
            recolectables.Clear();
            capsulasRescatadas = 0;
            puntosMision = 0;

            int[,] mapaSector1 = new int[ROWS, COLS]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,0,0,0,1,0,0,0,0,0,1,0,0,0,0,1},
                {1,0,1,0,1,0,1,1,1,0,1,0,1,1,0,1},
                {1,0,1,0,0,0,0,0,1,0,0,0,0,1,0,1},
                {1,0,1,1,1,1,1,0,1,1,1,1,0,1,0,1},
                {1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1},
                {1,1,1,0,1,0,1,0,1,1,1,0,1,1,0,1},
                {1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,1},
                {1,0,1,1,1,1,1,0,1,0,1,1,1,0,1,1},
                {1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
                {1,0,1,1,0,1,1,1,1,0,0,0,1,1,0,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
            };

            int[,] mapaSector2 = new int[ROWS, COLS]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1},
                {1,0,1,1,0,1,0,1,1,0,1,0,1,1,0,1},
                {1,0,1,0,0,0,0,1,0,0,0,0,0,1,0,1},
                {1,0,1,0,1,1,0,1,0,1,1,1,0,1,0,1},
                {1,0,0,0,1,0,0,0,0,0,0,1,0,0,0,1},
                {1,1,1,0,1,0,1,1,1,1,0,1,1,1,0,1},
                {1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1},
                {1,0,1,1,1,0,1,0,1,1,1,1,0,1,0,1},
                {1,0,1,0,0,0,0,0,1,0,0,0,0,1,0,1},
                {1,0,0,0,1,1,1,0,0,0,1,1,0,0,0,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
            };

            int[,] mapaSector3 = new int[ROWS, COLS]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,0,0,0,1,0,0,0,0,0,0,1,0,0,0,1},
                {1,0,1,0,1,0,1,1,1,1,0,1,0,1,0,1},
                {1,0,1,0,0,0,0,0,0,0,0,0,0,1,0,1},
                {1,0,1,1,1,0,1,0,0,1,0,1,1,1,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,1,1,1,0,1,0,0,1,0,1,1,1,0,1},
                {1,0,1,0,0,0,0,0,0,0,0,0,0,1,0,1},
                {1,0,1,0,1,0,1,1,1,1,0,1,0,1,0,1},
                {1,0,0,0,1,0,0,0,0,0,0,1,0,0,0,1},
                {1,0,1,0,0,0,1,1,1,1,0,0,0,1,0,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
            };

            mapa = sector switch
            {
                2 => mapaSector2,
                3 => mapaSector3,
                _ => mapaSector1
            };

            jugadorRect = new RectangleF(1 * TILE + 8, 1 * TILE + 8, 34, 34);
            portalRect = new RectangleF(14 * TILE + 5, 10 * TILE + 5, 40, 40);

            if (sector == 1)
            {
                AgregarCapsula(7, 1);
                AgregarCapsula(1, 9);
                AgregarCapsula(12, 5);

                AgregarBotin(9, 5, TipoBotin.Energia, "Célula de Energía");
                AgregarBotin(14, 1, TipoBotin.Escudo, "Escudo de Plasma");

                AgregarEnemigo(new Robot(), 5, 3, 2.2f);
                AgregarEnemigo(new Robot(), 9, 7, 2.4f);
                AgregarEnemigo(new Robot(), 11, 9, 2.5f);
            }
            else if (sector == 2)
            {
                AgregarCapsula(8, 1);
                AgregarCapsula(1, 7);
                AgregarCapsula(14, 5);
                AgregarCapsula(7, 10);

                // Reubicado en pasillo libre (12, 3) para que se pueda recoger sin bloqueo
                AgregarBotin(5, 5, TipoBotin.Escudo, "Escudo Reforzado");
                AgregarBotin(12, 3, TipoBotin.ModuloAtaque, "Amplificador Láser");

                // Enemigos reforzados para mayor reto en el Sector 02
                AgregarEnemigo(new Robot(), 6, 3, 2.7f, saludOverride: 105, ataqueOverride: 16);
                AgregarEnemigo(new Enemigo(), 9, 5, 3.1f, saludOverride: 155, ataqueOverride: 22);
                AgregarEnemigo(new Enemigo(), 4, 9, 3.1f, saludOverride: 155, ataqueOverride: 22);
                AgregarEnemigo(new Enemigo(), 12, 8, 3.2f, saludOverride: 155, ataqueOverride: 22);
            }
            else
            {
                // Todas las 5 cápsulas en pasillos abiertos (la 4ta movida a 7, 9)
                AgregarCapsula(1, 5);
                AgregarCapsula(14, 5);
                AgregarCapsula(7, 1);
                AgregarCapsula(7, 9);
                AgregarCapsula(14, 1);

                AgregarBotin(3, 3, TipoBotin.Energia, "Núcleo Cuántico");
                AgregarBotin(12, 7, TipoBotin.Escudo, "Barrera Omega");
                AgregarBotin(3, 9, TipoBotin.ModuloAtaque, "Cañón de Plasma");

                // Escolta de Xenomorfos Élite + Jefe Final Anomalía Omega Prime (520 HP)
                AgregarEnemigo(new Enemigo(), 5, 3, 3.2f, saludOverride: 180, ataqueOverride: 25);
                AgregarEnemigo(new Enemigo(), 10, 3, 3.2f, saludOverride: 180, ataqueOverride: 25);
                AgregarEnemigo(new Enemigo(), 5, 8, 3.3f, saludOverride: 180, ataqueOverride: 25);
                AgregarEnemigo(new Enemigo(), 10, 8, 3.3f, saludOverride: 180, ataqueOverride: 25);
                AgregarEnemigo(new JefeOmega(), 12, 9, 2.9f, saludOverride: 520, ataqueOverride: 35);
            }

            capsulasTotales = recolectables.FindAll(r => r.EsCapsula).Count;
            ActualizarHud();
        }

        private void AgregarCapsula(int col, int row)
        {
            recolectables.Add(new Recolectable
            {
                Rect = new RectangleF(col * TILE + 11, row * TILE + 11, 28, 28),
                EsCapsula = true,
                Nombre = "Cápsula Criogénica"
            });
        }

        private void AgregarBotin(int col, int row, TipoBotin tipo, string nombre)
        {
            recolectables.Add(new Recolectable
            {
                Rect = new RectangleF(col * TILE + 12, row * TILE + 12, 26, 26),
                EsCapsula = false,
                Tipo = tipo,
                Nombre = nombre
            });
        }

        private void AgregarEnemigo(Entidad entidad, int col, int row, float velocidad, int saludOverride = 0, int ataqueOverride = 0)
        {
            if (saludOverride > 0)
            {
                entidad.Salud = saludOverride;
                entidad.SaludMaxima = saludOverride;
            }
            if (ataqueOverride > 0)
            {
                entidad.Ataque = ataqueOverride;
            }

            int tam = entidad is JefeOmega ? 44 : 34;
            int offset = (TILE - tam) / 2;
            enemigos.Add(new EnemigoActivo
            {
                Datos = entidad,
                Rect = new RectangleF(col * TILE + offset, row * TILE + offset, tam, tam),
                Velocidad = velocidad,
                DirX = rng.Next(0, 2) == 0 ? 1 : -1,
                DirY = 0
            });
        }

        private void IniciarBucleJuego()
        {
            timerJuego = new System.Windows.Forms.Timer { Interval = 16 };
            timerJuego.Tick += TimerJuego_Tick;
            timerJuego.Start();
        }

        private void TimerJuego_Tick(object? sender, EventArgs e)
        {
            if (misionTerminada) return;

            tickAnimacion++;
            if (cooldownDisparo > 0) cooldownDisparo--;
            if (cooldownDanio > 0) cooldownDanio--;

            var c = EstadoJuego.CadeteActual;
            if (c != null && tickAnimacion % 45 == 0 && c.Energia < 100)
            {
                c.Energia++;
            }

            MoverJugador();
            ActualizarProyectiles();
            ActualizarEnemigos();
            VerificarRecolecciones();
            VerificarPortal();
            ActualizarHud();

            canvas.Invalidate();
        }

        private void MoverJugador()
        {
            float dx = 0, dy = 0;
            if (teclasActivas.Contains(Keys.W) || teclasActivas.Contains(Keys.Up)) dy -= 1;
            if (teclasActivas.Contains(Keys.S) || teclasActivas.Contains(Keys.Down)) dy += 1;
            if (teclasActivas.Contains(Keys.A) || teclasActivas.Contains(Keys.Left)) dx -= 1;
            if (teclasActivas.Contains(Keys.D) || teclasActivas.Contains(Keys.Right)) dx += 1;

            if (dx != 0 || dy != 0)
            {
                float len = (float)Math.Sqrt(dx * dx + dy * dy);
                dx /= len;
                dy /= len;
                direccionVista = new PointF(dx, dy);

                float velocidad = 4.4f;
                RectangleF intentoX = new RectangleF(jugadorRect.X + dx * velocidad, jugadorRect.Y, jugadorRect.Width, jugadorRect.Height);
                if (!ColisionaConMuro(intentoX)) jugadorRect.X = intentoX.X;

                RectangleF intentoY = new RectangleF(jugadorRect.X, jugadorRect.Y + dy * velocidad, jugadorRect.Width, jugadorRect.Height);
                if (!ColisionaConMuro(intentoY)) jugadorRect.Y = intentoY.Y;
            }

            if (teclasActivas.Contains(Keys.Space))
            {
                DispararLaser(direccionVista);
            }
        }

        private void DispararLaser(PointF dir)
        {
            if (cooldownDisparo > 0 || misionTerminada) return;
            var c = EstadoJuego.CadeteActual;
            if (c == null) return;

            if (c.Energia < 2)
            {
                lblIrisTactico.Text = "⚠ Energía baja: espera un instante a que se recarguen los capacitores.";
                return;
            }

            cooldownDisparo = 14;
            if (tickAnimacion % 2 == 0) c.ConsumirEnergia(1);

            GestorAudio.SonidoLaser();

            float cx = jugadorRect.X + jugadorRect.Width / 2f - 4;
            float cy = jugadorRect.Y + jugadorRect.Height / 2f - 4;

            proyectiles.Add(new ProyectilLaser
            {
                Rect = new RectangleF(cx, cy, 8, 8),
                VelX = dir.X * 10.5f,
                VelY = dir.Y * 10.5f
            });
        }

        private void ActualizarProyectiles()
        {
            var c = EstadoJuego.CadeteActual;
            if (c == null) return;

            for (int i = proyectiles.Count - 1; i >= 0; i--)
            {
                var p = proyectiles[i];
                p.Rect = new RectangleF(p.Rect.X + p.VelX, p.Rect.Y + p.VelY, p.Rect.Width, p.Rect.Height);

                if (ColisionaConMuro(p.Rect))
                {
                    proyectiles.RemoveAt(i);
                    continue;
                }

                bool impacto = false;
                for (int j = enemigos.Count - 1; j >= 0; j--)
                {
                    var ene = enemigos[j];
                    if (p.Rect.IntersectsWith(ene.Rect))
                    {
                        int danio = c.AtaqueBase;
                        ene.Datos.Salud -= danio;
                        ene.FlashDanio = 5;
                        impacto = true;

                        if (ene.Datos.Salud <= 0)
                        {
                            string tipoSql = ene.Datos is JefeOmega ? "Jefe" : (ene.Datos is Enemigo ? "Alien" : "Dron");
                            int xpGanada = ene.Datos is JefeOmega ? 350 : (ene.Datos is Enemigo ? 95 : 45);
                            puntosMision += xpGanada;
                            c.XP += xpGanada;

                            ConexionSQL.RegistrarCombate(c, tipoSql, danio);
                            lblIrisTactico.Text = $"✔ {ene.Datos.Nombre} neutralizado (+{xpGanada} XP).";
                            GestorAudio.SonidoItem();
                            enemigos.RemoveAt(j);
                        }
                        break;
                    }
                }

                if (impacto) proyectiles.RemoveAt(i);
            }
        }

        private void ActualizarEnemigos()
        {
            var c = EstadoJuego.CadeteActual;
            if (c == null) return;

            PointF centroJugador = new PointF(jugadorRect.X + jugadorRect.Width / 2f, jugadorRect.Y + jugadorRect.Height / 2f);

            foreach (var ene in enemigos)
            {
                if (ene.FlashDanio > 0) ene.FlashDanio--;

                PointF centroEne = new PointF(ene.Rect.X + ene.Rect.Width / 2f, ene.Rect.Y + ene.Rect.Height / 2f);
                float distX = centroJugador.X - centroEne.X;
                float distY = centroJugador.Y - centroEne.Y;
                float distancia = (float)Math.Sqrt(distX * distX + distY * distY);

                float rangoDeteccion = ene.Datos is JefeOmega ? 380f : 250f;

                if (distancia < rangoDeteccion && distancia > 1)
                {
                    ene.DirX = distX / distancia;
                    ene.DirY = distY / distancia;
                }
                else if (tickAnimacion % 55 == 0)
                {
                    int dir = rng.Next(0, 4);
                    ene.DirX = dir == 0 ? 1 : (dir == 1 ? -1 : 0);
                    ene.DirY = dir == 2 ? 1 : (dir == 3 ? -1 : 0);
                }

                RectangleF movX = new RectangleF(ene.Rect.X + ene.DirX * ene.Velocidad, ene.Rect.Y, ene.Rect.Width, ene.Rect.Height);
                if (!ColisionaConMuro(movX)) ene.Rect.X = movX.X;
                else ene.DirX = -ene.DirX;

                RectangleF movY = new RectangleF(ene.Rect.X, ene.Rect.Y + ene.DirY * ene.Velocidad, ene.Rect.Width, ene.Rect.Height);
                if (!ColisionaConMuro(movY)) ene.Rect.Y = movY.Y;
                else ene.DirY = -ene.DirY;

                if (ene.Rect.IntersectsWith(jugadorRect) && cooldownDanio == 0)
                {
                    cooldownDanio = 42;
                    c.RecibirDaño(ene.Datos.Ataque);
                    c.ReducirEstabilidad(5);
                    GestorAudio.SonidoPerderVida();
                    lblIrisTactico.Text = $"⚠ IMPACTO DE {ene.Datos.Nombre.ToUpper()} (-{ene.Datos.Ataque} DMG).";

                    if (c.Salud <= 0)
                    {
                        FinalizarMision(false, $"Traje destruido por {ene.Datos.Nombre}.");
                        return;
                    }
                }
            }
        }

        private void VerificarRecolecciones()
        {
            var c = EstadoJuego.CadeteActual;
            if (c == null) return;

            for (int i = recolectables.Count - 1; i >= 0; i--)
            {
                var item = recolectables[i];
                if (!jugadorRect.IntersectsWith(item.Rect)) continue;

                GestorAudio.SonidoItem();
                if (item.EsCapsula)
                {
                    capsulasRescatadas++;
                    puntosMision += 100;
                    c.XP += 100;
                    c.TotalRescatados++;

                    if (capsulasRescatadas >= capsulasTotales)
                        lblIrisTactico.Text = "🌀 ¡TODAS LAS CÁPSULAS RESCATADAS! Dirígete al portal inferior derecho.";
                    else
                        lblIrisTactico.Text = $"✔ Cápsula asegurada ({capsulasRescatadas}/{capsulasTotales}). ¡Continúa!";
                }
                else
                {
                    puntosMision += 40;
                    if (item.Tipo == TipoBotin.Energia)
                    {
                        c.Energia = Math.Min(100, c.Energia + 30);
                        c.Salud = Math.Min(100, c.Salud + 15);
                        c.Inventario.Add(new Objeto(item.Nombre, TipoObjeto.Energia, 25));
                        lblIrisTactico.Text = $"🔋 {item.Nombre} recogida (+30% Energía y guardada en inventario).";
                    }
                    else if (item.Tipo == TipoBotin.Escudo)
                    {
                        c.Escudo = Math.Min(100, c.Escudo + 25);
                        c.Inventario.Add(new Objeto(item.Nombre, TipoObjeto.Escudo, 20));
                        lblIrisTactico.Text = $"🛡️ {item.Nombre} activado (+25 Escudo de Plasma).";
                    }
                    else
                    {
                        c.ModuloAtaque += 5;
                        c.Inventario.Add(new Objeto(item.Nombre, TipoObjeto.ModuloAtaque, 5));
                        lblIrisTactico.Text = $"⚡ {item.Nombre} instalado (Ataque base: {c.AtaqueBase}).";
                    }
                }

                recolectables.RemoveAt(i);
            }
        }

        private void VerificarPortal()
        {
            bool portalAbierto = capsulasRescatadas >= capsulasTotales;
            if (portalAbierto && jugadorRect.IntersectsWith(portalRect))
            {
                FinalizarMision(true, "Extracción dimensional completada con éxito.");
            }
        }

        private void FinalizarMision(bool victoria, string motivo)
        {
            if (misionTerminada) return;
            misionTerminada = true;
            timerJuego.Stop();

            var c = EstadoJuego.CadeteActual;
            if (c != null)
            {
                if (victoria)
                {
                    GestorAudio.SonidoNivelCompletado();
                    int bono = EstadoJuego.SectorActual * 150;
                    puntosMision += bono;
                    c.XP += bono;
                    c.DatosIris += capsulasRescatadas * 10;

                    if (c.NivelDesbloqueado <= EstadoJuego.SectorActual && EstadoJuego.SectorActual < 3)
                    {
                        c.NivelDesbloqueado = EstadoJuego.SectorActual + 1;
                    }

                    ConexionSQL.RegistrarMision(c, EstadoJuego.NombreSectorActual, capsulasRescatadas, puntosMision, "COMPLETADA");
                    ConexionSQL.RegistrarHistorial(c, "MISION_COMPLETADA", $"{EstadoJuego.NombreSectorActual}: {capsulasRescatadas} rescatados (+{puntosMision} XP).");

                    MessageBox.Show(
                        $"¡MISIÓN CUMPLIDA, CADETE {c.Nombre.ToUpper()}!\n\n" +
                        $"Sector: {EstadoJuego.NombreSectorActual}\n" +
                        $"Cápsulas rescatadas: {capsulasRescatadas}/{capsulasTotales}\n" +
                        $"Experiencia obtenida: +{puntosMision} XP\n" +
                        $"Rango actual: {c.Rango}\n\n" +
                        "Todos los registros fueron guardados en NexusDB.",
                        "NEXUS // EXTRACCIÓN EXITOSA",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    GestorAudio.SonidoPerderVida();
                    c.Salud = 50;
                    c.ActualizarPeligro();

                    ConexionSQL.RegistrarMision(c, EstadoJuego.NombreSectorActual, capsulasRescatadas, puntosMision, "FALLIDA");
                    ConexionSQL.RegistrarHistorial(c, "MISION_FALLIDA", $"{EstadoJuego.NombreSectorActual}: {motivo}");

                    MessageBox.Show(
                        $"INCURSIÓN INTERRUMPIDA\n\nMotivo: {motivo}\n" +
                        $"Cápsulas aseguradas: {capsulasRescatadas}/{capsulasTotales}\n\n" +
                        "La IA IRIS te ha teletransportado de vuelta al Puente de Mando.",
                        "NEXUS // ALERTA DE EVACUACIÓN",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }

            this.Close();
        }

        private bool ColisionaConMuro(RectangleF rect)
        {
            int left = (int)(rect.Left / TILE);
            int right = (int)((rect.Right - 0.1f) / TILE);
            int top = (int)(rect.Top / TILE);
            int bottom = (int)((rect.Bottom - 0.1f) / TILE);

            for (int r = top; r <= bottom; r++)
            {
                for (int c = left; c <= right; c++)
                {
                    if (r < 0 || r >= ROWS || c < 0 || c >= COLS) return true;
                    if (mapa[r, c] == 1) return true;
                }
            }
            return false;
        }

        private void ActualizarHud()
        {
            var c = EstadoJuego.CadeteActual;
            if (c == null) return;

            lblObjetivoHud.Text = $"CÁPSULAS: {capsulasRescatadas} / {capsulasTotales}";
            if (capsulasRescatadas >= capsulasTotales)
            {
                lblPortalHud.Text = "🌀 PORTAL ABIERTO — ¡VE!";
                lblPortalHud.ForeColor = Color.FromArgb(80, 255, 160);
            }

            barSalud.Width = (int)(218 * (Math.Clamp(c.Salud, 0, 100) / 100.0));
            lblSaludVal.Text = $"{c.Salud}/100";

            barEscudo.Width = (int)(218 * (Math.Clamp(c.Escudo, 0, 100) / 100.0));
            lblEscudoVal.Text = $"{c.Escudo} PTS";

            barEnergia.Width = (int)(218 * (Math.Clamp(c.Energia, 0, 100) / 100.0));
            lblEnergiaVal.Text = $"{c.Energia}%";

            lblPuntosHud.Text = $"PUNTOS MISIÓN: {puntosMision} XP";
        }

        private void Canvas_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color colorMuro = EstadoJuego.SectorActual switch
            {
                2 => Color.FromArgb(24, 58, 52),
                3 => Color.FromArgb(58, 22, 48),
                _ => Color.FromArgb(18, 38, 68)
            };

            Color bordeMuro = EstadoJuego.SectorActual switch
            {
                2 => Color.FromArgb(60, 235, 160),
                3 => Color.FromArgb(235, 75, 175),
                _ => Color.FromArgb(0, 195, 245)
            };

            using SolidBrush brochaMuro = new SolidBrush(colorMuro);
            using Pen penMuro = new Pen(bordeMuro, 1.5f);
            using Pen penGrid = new Pen(Color.FromArgb(16, 26, 46), 1);

            for (int r = 0; r < ROWS; r++)
            {
                for (int c = 0; c < COLS; c++)
                {
                    Rectangle tileRect = new Rectangle(c * TILE, r * TILE, TILE, TILE);
                    if (mapa[r, c] == 1)
                    {
                        g.FillRectangle(brochaMuro, tileRect);
                        g.DrawRectangle(penMuro, tileRect.X + 1, tileRect.Y + 1, TILE - 2, TILE - 2);
                    }
                    else
                    {
                        g.DrawRectangle(penGrid, tileRect);
                    }
                }
            }

            bool portalActivo = capsulasRescatadas >= capsulasTotales;
            Color colorPortal = portalActivo ? Color.FromArgb(0, 255, 180) : Color.FromArgb(90, 110, 140);
            using (Pen penPortal = new Pen(colorPortal, portalActivo ? 3f : 1.5f))
            {
                g.DrawEllipse(penPortal, portalRect);
                float pulso = (tickAnimacion % 20) * 0.5f;
                g.DrawEllipse(penPortal, portalRect.X + pulso, portalRect.Y + pulso, portalRect.Width - pulso * 2, portalRect.Height - pulso * 2);
            }

            foreach (var item in recolectables)
            {
                if (item.EsCapsula)
                {
                    using SolidBrush bCap = new SolidBrush(Color.FromArgb(50, 245, 165));
                    using Pen pCap = new Pen(Color.White, 1.5f);
                    g.FillEllipse(bCap, item.Rect);
                    g.DrawEllipse(pCap, item.Rect);
                }
                else
                {
                    Color cBotin = item.Tipo == TipoBotin.Energia ? Color.Gold
                                 : item.Tipo == TipoBotin.Escudo ? Color.DeepSkyBlue
                                 : Color.MediumOrchid;
                    using SolidBrush bBot = new SolidBrush(cBotin);
                    g.FillRectangle(bBot, item.Rect);
                    g.DrawRectangle(Pens.White, item.Rect.X, item.Rect.Y, item.Rect.Width, item.Rect.Height);
                }
            }

            using SolidBrush bLaser = new SolidBrush(Color.FromArgb(0, 250, 255));
            foreach (var p in proyectiles)
            {
                g.FillEllipse(bLaser, p.Rect);
            }

            foreach (var ene in enemigos)
            {
                Image sprite = ene.Datos is JefeOmega ? Properties.Resources.jefe_omega
                             : ene.Datos is Enemigo ? Properties.Resources.enemigo_alien
                             : Properties.Resources.enemigo_dron;

                g.DrawImage(sprite, ene.Rect);

                float pct = Math.Clamp(ene.Datos.Salud / (float)ene.Datos.SaludMaxima, 0f, 1f);
                g.FillRectangle(Brushes.DarkRed, ene.Rect.X, ene.Rect.Y - 6, ene.Rect.Width, 4);
                g.FillRectangle(Brushes.LimeGreen, ene.Rect.X, ene.Rect.Y - 6, ene.Rect.Width * pct, 4);
            }

            if (cooldownDanio == 0 || (cooldownDanio / 4) % 2 == 0)
            {
                g.DrawImage(Properties.Resources.jugador, jugadorRect);
            }
        }

        private void Canvas_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            float cx = jugadorRect.X + jugadorRect.Width / 2f;
            float cy = jugadorRect.Y + jugadorRect.Height / 2f;
            float dx = e.X - cx;
            float dy = e.Y - cy;
            float len = (float)Math.Sqrt(dx * dx + dy * dy);
            if (len > 1)
            {
                direccionVista = new PointF(dx / len, dy / len);
                DispararLaser(direccionVista);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys key = keyData & Keys.KeyCode;
            if (key == Keys.Up || key == Keys.Down || key == Keys.Left || key == Keys.Right || key == Keys.Space)
            {
                teclasActivas.Add(key);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            teclasActivas.Add(e.KeyCode);
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            teclasActivas.Remove(e.KeyCode);
            base.OnKeyUp(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timerJuego?.Stop();
            timerJuego?.Dispose();
            base.OnFormClosing(e);
        }

        private enum TipoBotin { Energia, Escudo, ModuloAtaque }

        private class EnemigoActivo
        {
            public Entidad Datos { get; set; } = null!;
            public RectangleF Rect;
            public float Velocidad;
            public float DirX;
            public float DirY;
            public int FlashDanio;
        }

        private class ProyectilLaser
        {
            public RectangleF Rect;
            public float VelX;
            public float VelY;
        }

        private class Recolectable
        {
            public RectangleF Rect;
            public bool EsCapsula;
            public TipoBotin Tipo;
            public string Nombre = string.Empty;
        }

        private class ArenaCanvas : Panel
        {
            public ArenaCanvas()
            {
                this.DoubleBuffered = true;
                this.ResizeRedraw = true;
            }
        }
    }
}