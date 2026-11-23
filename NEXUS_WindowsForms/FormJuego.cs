using System;
using System.Drawing;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    public partial class FormJuego : Form
    {
        private Cadete cadete;
        private char[,] mapa = new char[13, 29];
        private bool[,] zonas = new bool[13, 29];
        private int filaJ = 6, colJ = 14, recursos = 0, anomalias = 0;
        private Random rnd = new Random();
        private Entidad enemigoActual;
        private bool combateDesdeMapa = false;

        private Panel pnlLogin, pnlJuego, pnlMapa, pnlCombate, pnlHistorial;
        private TextBox txtNombre, txtRealidad;
        private NumericUpDown numEdad, numEnergia, numEstabilidad;
        private Label lblHeader, lblSalud, lblEnergia, lblEstabilidad, lblPeligro, lblExtraStats;
        private ProgressBar pbSalud, pbEnergia, pbEstabilidad, pbPeligro;
        private RichTextBox rtbConsola;
        private ListBox lstInventario;
        private Label lblMapaGrid, lblMapaInfo, lblMapaEvento;
        private Label lblEnemigoInfo, lblCombateCadete;
        private RichTextBox rtbCombateLog;
        private DataGridView dgvHistorial;

        public FormJuego()
        {
            this.Text = "NEXUS SYSTEM v2.0 - SQL Server";
            this.Size = new Size(1050, 680);
            this.BackColor = Color.FromArgb(10, 14, 22);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.KeyPreview = true;
            this.KeyDown += FormJuego_KeyDown;

            ConstruirLogin();
            ConstruirJuego();
            ConstruirMapa();
            ConstruirCombate();
            ConstruirHistorial();

            MostrarPanel(pnlLogin);
        }

        private void MostrarPanel(Panel objetivo)
        {
            pnlLogin.Visible = (objetivo == pnlLogin);
            pnlJuego.Visible = (objetivo == pnlJuego);
            pnlMapa.Visible = (objetivo == pnlMapa);
            pnlCombate.Visible = (objetivo == pnlCombate);
            pnlHistorial.Visible = (objetivo == pnlHistorial);
        }

        private void ConstruirLogin()
        {
            pnlLogin = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(10, 14, 22) };
            Panel card = new Panel { Size = new Size(460, 440), Location = new Point(295, 100), BackColor = Color.FromArgb(18, 25, 38), BorderStyle = BorderStyle.FixedSingle };
            Label titulo = new Label { Text = "NEXUS TRAINING SYSTEM", Font = new Font("Consolas", 16F, FontStyle.Bold), ForeColor = Color.Cyan, Location = new Point(75, 25), AutoSize = true };

            txtNombre = CrearInputLogin(card, "Nombre del Cadete:", "Joel", 80);
            numEdad = CrearNumLogin(card, "Edad (18-100):", 18, 1, 100, 130);
            txtRealidad = CrearInputLogin(card, "Realidad Autorizada:", "Neo Cochabamba", 180);
            numEnergia = CrearNumLogin(card, "Energía Inicial (40-100):", 80, 0, 100, 230);
            numEstabilidad = CrearNumLogin(card, "Estabilidad Inicial:", 85, 0, 100, 280);

            Button btnEntrar = new Button { Text = "INICIAR ENLACE NEURONAL", Font = new Font("Consolas", 11F, FontStyle.Bold), BackColor = Color.FromArgb(0, 120, 140), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(40, 350), Size = new Size(380, 45) };
            btnEntrar.Click += BtnEntrar_Click;

            card.Controls.Add(titulo);
            card.Controls.Add(btnEntrar);
            pnlLogin.Controls.Add(card);
            this.Controls.Add(pnlLogin);
        }

        private TextBox CrearInputLogin(Panel p, string texto, string def, int y)
        {
            p.Controls.Add(new Label { Text = texto, ForeColor = Color.LightGray, Font = new Font("Consolas", 10F), Location = new Point(40, y), AutoSize = true });
            TextBox tb = new TextBox { Text = def, Font = new Font("Consolas", 10F), BackColor = Color.FromArgb(10, 14, 22), ForeColor = Color.Cyan, Location = new Point(230, y - 3), Size = new Size(190, 26) };
            p.Controls.Add(tb);
            return tb;
        }

        private NumericUpDown CrearNumLogin(Panel p, string texto, int val, int min, int max, int y)
        {
            p.Controls.Add(new Label { Text = texto, ForeColor = Color.LightGray, Font = new Font("Consolas", 10F), Location = new Point(40, y), AutoSize = true });
            NumericUpDown num = new NumericUpDown { Minimum = min, Maximum = max, Value = val, Font = new Font("Consolas", 10F), BackColor = Color.FromArgb(10, 14, 22), ForeColor = Color.Cyan, Location = new Point(230, y - 3), Size = new Size(190, 26) };
            p.Controls.Add(num);
            return num;
        }

        private void BtnEntrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) { MessageBox.Show("Ingresa un nombre."); return; }
            if (numEdad.Value < 18 || numEnergia.Value < 40) { MessageBox.Show("ACCESO DENEGADO: Edad mínima 18 y Energía mínima 40."); return; }
            if (!txtRealidad.Text.Trim().Equals("Neo Cochabamba", StringComparison.OrdinalIgnoreCase)) { MessageBox.Show("Realidad no autorizada. Debe ser 'Neo Cochabamba'."); return; }

            cadete = new Cadete(txtNombre.Text.Trim(), (int)numEdad.Value, txtRealidad.Text.Trim(), (int)numEnergia.Value, (int)numEstabilidad.Value);
            InicializarMapa();
            ConexionSQL.RegistrarHistorial(cadete, "INGRESO", $"Sesión iniciada en realidad {cadete.Realidad} (Edad: {cadete.Edad})");

            rtbConsola.Clear();
            Log($"[SISTEMA] Bienvenido, Cadete {cadete.Nombre}. Enlace neuronal establecido.", Color.Cyan);
            Log($"[SQL SERVER] Registro guardado en HistorialAcciones.", Color.Lime);
            if (!ConexionSQL.ProbarConexion())
                Log("[ADVERTENCIA SQL] Sin conexión con el servidor de base de datos.", Color.OrangeRed);
            ActualizarHUD();
            MostrarPanel(pnlJuego);
        }

        private void ConstruirJuego()
        {
            pnlJuego = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(10, 14, 22) };
            lblHeader = new Label { Font = new Font("Consolas", 15F, FontStyle.Bold), ForeColor = Color.Cyan, Location = new Point(20, 15), AutoSize = true };
            lblExtraStats = new Label { Font = new Font("Consolas", 10F, FontStyle.Bold), ForeColor = Color.Gold, Location = new Point(620, 20), AutoSize = true };

            pbSalud = CrearBarra(pnlJuego, "Salud:", Color.LimeGreen, 20, 55, out lblSalud);
            pbEnergia = CrearBarra(pnlJuego, "Energía:", Color.DeepSkyBlue, 20, 90, out lblEnergia);
            pbEstabilidad = CrearBarra(pnlJuego, "Estabilidad:", Color.Gold, 480, 55, out lblEstabilidad);
            pbPeligro = CrearBarra(pnlJuego, "Peligro:", Color.OrangeRed, 480, 90, out lblPeligro);

            int bx = 20, by = 140, w = 210, h = 42, gap = 48;
            CrearBotonMando("1. Explorar Sector", bx, by, w, h, Color.FromArgb(20, 35, 55), (s, e) => AccionExplorar());
            CrearBotonMando("2. Escanear Anomalía", bx, by + gap, w, h, Color.FromArgb(20, 35, 55), (s, e) => AccionEscanear());
            CrearBotonMando("3. Recuperar Energía", bx, by + gap * 2, w, h, Color.FromArgb(20, 35, 55), (s, e) => AccionRecuperar());
            CrearBotonMando("4. Evaluar Condición", bx, by + gap * 3, w, h, Color.FromArgb(20, 35, 55), (s, e) => AccionEvaluar());
            CrearBotonMando("5. Ver Misión Actual", bx, by + gap * 4, w, h, Color.FromArgb(20, 35, 55), (s, e) => AccionMision());
            CrearBotonMando("6. Solicitar Extracción", bx, by + gap * 5, w, h, Color.FromArgb(20, 90, 50), (s, e) => AccionExtraccion());

            int bx2 = 240;
            CrearBotonMando("7. Mapa Táctico 2D", bx2, by, w, h, Color.FromArgb(0, 95, 115), (s, e) => { ActualizarVistaMapa(); MostrarPanel(pnlMapa); });
            CrearBotonMando("8. Usar Objeto Sel.", bx2, by + gap, w, h, Color.FromArgb(20, 35, 55), (s, e) => AccionUsarObjeto());
            CrearBotonMando("9. Simular Encuentro", bx2, by + gap * 2, w, h, Color.FromArgb(110, 35, 35), (s, e) => { combateDesdeMapa = false; IniciarCombate(); });
            CrearBotonMando("10. VER HISTORIAL SQL", bx2, by + gap * 3, w, h, Color.FromArgb(120, 85, 0), (s, e) => { dgvHistorial.DataSource = ConexionSQL.ObtenerHistorialCompleto(); MostrarPanel(pnlHistorial); });
            CrearBotonMando("11. Desconectar", bx2, by + gap * 5, w, h, Color.FromArgb(95, 25, 25), (s, e) => { ConexionSQL.RegistrarHistorial(cadete, "SALIDA", "Cadete desconectado."); MostrarPanel(pnlLogin); });

            rtbConsola = new RichTextBox { Location = new Point(470, 140), Size = new Size(540, 320), BackColor = Color.FromArgb(6, 9, 14), ForeColor = Color.Cyan, Font = new Font("Consolas", 10F), ReadOnly = true, BorderStyle = BorderStyle.FixedSingle };
            Label lblInv = new Label { Text = "INVENTARIO (Selecciona y pulsa '8. Usar Objeto Sel.'):", ForeColor = Color.Cyan, Font = new Font("Consolas", 9.5F, FontStyle.Bold), Location = new Point(470, 470), AutoSize = true };
            lstInventario = new ListBox { Location = new Point(470, 495), Size = new Size(540, 120), BackColor = Color.FromArgb(16, 22, 34), ForeColor = Color.White, Font = new Font("Consolas", 10F) };

            pnlJuego.Controls.AddRange(new Control[] { lblHeader, lblExtraStats, rtbConsola, lblInv, lstInventario });
            this.Controls.Add(pnlJuego);
        }

        private ProgressBar CrearBarra(Panel p, string titulo, Color col, int x, int y, out Label lblPorc)
        {
            p.Controls.Add(new Label { Text = titulo, ForeColor = col, Font = new Font("Consolas", 10F, FontStyle.Bold), Location = new Point(x, y), AutoSize = true });
            ProgressBar pb = new ProgressBar { Location = new Point(x + 115, y), Size = new Size(250, 20), Maximum = 100 };
            lblPorc = new Label { Text = "100%", ForeColor = col, Font = new Font("Consolas", 10F, FontStyle.Bold), Location = new Point(x + 375, y), AutoSize = true };
            p.Controls.Add(pb);
            p.Controls.Add(lblPorc);
            return pb;
        }

        private void CrearBotonMando(string texto, int x, int y, int w, int h, Color bg, EventHandler click)
        {
            Button btn = new Button { Text = texto, Location = new Point(x, y), Size = new Size(w, h), BackColor = bg, ForeColor = Color.Cyan, Font = new Font("Consolas", 10F, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btn.FlatAppearance.BorderColor = Color.FromArgb(0, 150, 180);
            btn.Click += click;
            pnlJuego.Controls.Add(btn);
        }

        private void ActualizarHUD()
        {
            lblHeader.Text = $"NEXUS SYSTEM v2.0 - Cadete {cadete.Nombre}";
            lblExtraStats.Text = $"Escudo: {cadete.Escudo} | XP: {cadete.XP} | IRIS: {cadete.DatosIris}";

            pbSalud.Value = Math.Clamp(cadete.Salud, 0, 100); lblSalud.Text = $"{cadete.Salud}%";
            pbEnergia.Value = Math.Clamp(cadete.Energia, 0, 100); lblEnergia.Text = $"{cadete.Energia}%";
            pbEstabilidad.Value = Math.Clamp(cadete.Estabilidad, 0, 100); lblEstabilidad.Text = $"{cadete.Estabilidad}%";
            pbPeligro.Value = Math.Clamp(cadete.Peligro, 0, 100); lblPeligro.Text = $"{cadete.Peligro}%";

            lstInventario.Items.Clear();
            foreach (var obj in cadete.Inventario)
                lstInventario.Items.Add($"{obj.Nombre} [Tipo: {obj.Tipo} | +{obj.Valor}]");

            if (cadete.Salud <= 0)
            {
                ConexionSQL.RegistrarHistorial(cadete, "MUERTE", "El cadete cayó en combate. Misión fallida.");
                MessageBox.Show("El Cadete ha caído en combate. MISIÓN FALLIDA.");
                MostrarPanel(pnlLogin);
            }
        }

        private void Log(string msg, Color? col = null)
        {
            rtbConsola.SelectionStart = rtbConsola.TextLength;
            rtbConsola.SelectionColor = col ?? Color.Cyan;
            rtbConsola.AppendText(msg + "\n");
            rtbConsola.ScrollToCaret();
        }

        private void AccionExplorar()
        {
        
            if (cadete.Energia < 40) { Log("Energía insuficiente para explorar (mínimo 40%).", Color.Orange); return; }
            cadete.Explorar();
            if (cadete.DatosIris >= 2)
            {
                cadete.XP += 25;
                Log("BONO TÁCTICO IRIS: +25 XP extra por análisis previo.", Color.Gold);
            }
            int ev = rnd.Next(1, 6);
            string detalleSQL = "";
            switch (ev)
            {
                case 1:
                    cadete.Inventario.Add(new Objeto("Célula de energía+", TipoObjeto.Energia, 35));
                    detalleSQL = "Exploró sector y halló Célula de energía+";
                    Log("OBJETO ENCONTRADO: Célula de energía+ (+35)", Color.Lime);
                    break;
                case 2:
                    detalleSQL = "Exploró sector y detectó una entidad hostil";
                    Log("¡ENTIDAD HOSTIL DETECTADA! Iniciando combate...", Color.Red);
                    ActualizarHUD();
                    ConexionSQL.RegistrarHistorial(cadete, "EXPLORACION", detalleSQL);
                    combateDesdeMapa = false;
                    IniciarCombate();
                    return;
                case 3:
                    cadete.ConsumirEnergia(10);
                    cadete.MisionActual.ZonaLlegada = true;
                    detalleSQL = "Llegó a la zona de anomalía (-10 energía)";
                    Log("ANOMALÍA DETECTADA: Radiación presente (-10 energía). [Objetivo cumplido]", Color.Yellow);
                    break;
                case 4:
                    cadete.RecibirDaño(15);
                    detalleSQL = "Fallo de fase durante exploración (-15 daño)";
                    Log("FALLO DE FASE: Recibes 15 de daño en el blindaje.", Color.OrangeRed);
                    break;
                case 5:
                    cadete.MisionActual.NucleoEncontrado = true;
                    cadete.Inventario.Add(new Objeto("Núcleo Energético", TipoObjeto.Energia, 50));
                    detalleSQL = "Encontró el Núcleo Energético";
                    Log("¡HITO ALCANZADO! Has encontrado el Núcleo Energético.", Color.Cyan);
                    break;
            }
            ConexionSQL.RegistrarHistorial(cadete, "EXPLORACION", detalleSQL);
            ActualizarHUD();
        }

        private void AccionEscanear()
        {
            if (cadete.Energia < 10) { Log("Energía insuficiente para escanear.", Color.Orange); return; }
            cadete.ConsumirEnergia(10);
            cadete.DatosIris++;
            Log($"Escaneo completado. Datos IRIS acumulados: {cadete.DatosIris}", Color.Cyan);
            ConexionSQL.RegistrarHistorial(cadete, "ESCANEO", $"Escaneó anomalía. Datos IRIS = {cadete.DatosIris}");
            ActualizarHUD();
        }

        private void AccionRecuperar()
        {
            int rec = cadete.RecuperarEnergia();
            Log($"Energía recuperada: +{rec}%. Actual: {cadete.Energia}%", Color.Lime);
            ConexionSQL.RegistrarHistorial(cadete, "RECUPERACION", $"Recuperó +{rec}% de energía");
            ActualizarHUD();
        }

        private void AccionEvaluar()
        {
            bool segura = cadete.Energia >= 50 && cadete.Peligro < 50 && cadete.Estabilidad >= 50;
            Log(segura ? "CONDICIÓN SEGURA: Parámetros óptimos." : "CONDICIÓN INESTABLE: Se recomienda precaución.", segura ? Color.Lime : Color.Yellow);
            ConexionSQL.RegistrarHistorial(cadete, "EVALUACION", segura ? "Evaluación: Condición segura" : "Evaluación: Condición inestable");
        }

        private void AccionMision()
        {
            var m = cadete.MisionActual;
            Log($"MISIÓN: {m.Nombre}", Color.Gold);
            Log($"  [{(m.ZonaLlegada ? "X" : " ")}] Llegar a la zona de anomalía");
            Log($"  [{(m.NucleoEncontrado ? "X" : " ")}] Encontrar el núcleo energético");
            Log($"  [{(m.EntidadSobrevivida ? "X" : " ")}] Sobrevivir al encuentro con la entidad");
        }

        private void AccionExtraccion()
        {
            var m = cadete.MisionActual;
            if (m.ZonaLlegada && m.NucleoEncontrado && m.EntidadSobrevivida)
            {
                m.ExtraccionCompletada = true;
                cadete.XP += 100;
                Log("¡MISIÓN COMPLETADA! +100 XP obtenidos.", Color.Lime);
                ConexionSQL.RegistrarHistorial(cadete, "MISION", "¡Misión completada con éxito! +100 XP");
                ActualizarHUD();
            }
            else
            {
                Log("Aún faltan objetivos por cumplir para la extracción.", Color.Orange);
            }
        }

        private void AccionUsarObjeto()
        {
            int idx = lstInventario.SelectedIndex;
            if (idx < 0) { Log("Selecciona un objeto del inventario primero.", Color.Orange); return; }

            Objeto obj = cadete.Inventario[idx];
            if (obj.Tipo == TipoObjeto.Energia) cadete.Energia = Math.Min(100, cadete.Energia + obj.Valor);
            else if (obj.Tipo == TipoObjeto.Escudo) cadete.Escudo += obj.Valor;
            else if (obj.Tipo == TipoObjeto.ModuloAtaque) cadete.ModuloAtaque += obj.Valor;
            else if (obj.Tipo == TipoObjeto.Analizador) cadete.DatosIris++;

            cadete.Inventario.RemoveAt(idx);
            Log($"Usaste [{obj.Nombre}]. Efecto aplicado.", Color.Lime);
            ConexionSQL.RegistrarHistorial(cadete, "OBJETO", $"Utilizó objeto: {obj.Nombre}");
            ActualizarHUD();
        }

        private void ConstruirMapa()
        {
            pnlMapa = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(10, 14, 22) };
            Label tit = new Label { Text = "MAPA DE CARTOGRAFÍA 2D (Usa botones o teclas W/A/S/D)", Font = new Font("Consolas", 13.5F, FontStyle.Bold), ForeColor = Color.Cyan, Location = new Point(25, 15), AutoSize = true };

            lblMapaGrid = new Label
            {
                Font = new Font("Consolas", 10.5F, FontStyle.Bold),
                ForeColor = Color.Lime,
                BackColor = Color.FromArgb(6, 9, 14),
                Location = new Point(25, 50),
                Size = new Size(640, 315),
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblMapaInfo = new Label { Font = new Font("Consolas", 11F, FontStyle.Bold), ForeColor = Color.Gold, Location = new Point(25, 380), AutoSize = true };

            lblMapaEvento = new Label
            {
                Text = "Radar activo: Muévete para explorar el sector. Cuidado con las entidades hostiles (E).",
                Font = new Font("Consolas", 10.5F, FontStyle.Bold),
                ForeColor = Color.Cyan,
                BackColor = Color.FromArgb(16, 22, 34),
                Location = new Point(25, 415),
                Size = new Size(640, 65),
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            Panel pnlLeyenda = new Panel { Location = new Point(685, 50), Size = new Size(320, 235), BackColor = Color.FromArgb(16, 22, 34), BorderStyle = BorderStyle.FixedSingle };
            Label lblTitLeyenda = new Label { Text = "LEYENDA DEL SECTOR", Font = new Font("Consolas", 11F, FontStyle.Bold), ForeColor = Color.Cyan, Location = new Point(12, 10), AutoSize = true };
            Label lblTextoLeyenda = new Label
            {
                Text = "X  = Tu posición (Cadete)\nH  = Base Central NEXUS\nR  = Recurso Energético (+20)\nA  = Anomalía (-5 Estabilidad)\nI  = Módulo Analizador IRIS\nE  = Entidad Hostil (Combate)\n#  = Muro / Zona Bloqueada\n~  = Sector Acuático\n^  = Terreno Montañoso\n?  = Zona sin explorar",
                Font = new Font("Consolas", 9.5F),
                ForeColor = Color.LightGray,
                Location = new Point(12, 38),
                AutoSize = true
            };
            pnlLeyenda.Controls.Add(lblTitLeyenda);
            pnlLeyenda.Controls.Add(lblTextoLeyenda);

            Button btnW = new Button { Text = "W (Norte)", Location = new Point(795, 300), Size = new Size(100, 45), BackColor = Color.FromArgb(20, 40, 60), ForeColor = Color.Cyan, Font = new Font("Consolas", 10F, FontStyle.Bold) };
            Button btnS = new Button { Text = "S (Sur)", Location = new Point(795, 400), Size = new Size(100, 45), BackColor = Color.FromArgb(20, 40, 60), ForeColor = Color.Cyan, Font = new Font("Consolas", 10F, FontStyle.Bold) };
            Button btnA = new Button { Text = "A (Oeste)", Location = new Point(690, 350), Size = new Size(100, 45), BackColor = Color.FromArgb(20, 40, 60), ForeColor = Color.Cyan, Font = new Font("Consolas", 10F, FontStyle.Bold) };
            Button btnD = new Button { Text = "D (Este)", Location = new Point(900, 350), Size = new Size(100, 45), BackColor = Color.FromArgb(20, 40, 60), ForeColor = Color.Cyan, Font = new Font("Consolas", 10F, FontStyle.Bold) };

            btnW.Click += (s, e) => MoverEnMapa(-1, 0, "Norte (W)");
            btnS.Click += (s, e) => MoverEnMapa(1, 0, "Sur (S)");
            btnA.Click += (s, e) => MoverEnMapa(0, -1, "Oeste (A)");
            btnD.Click += (s, e) => MoverEnMapa(0, 1, "Este (D)");

            Button btnVolver = new Button { Text = "VOLVER AL CENTRO DE MANDO", Location = new Point(685, 470), Size = new Size(320, 45), BackColor = Color.FromArgb(0, 110, 130), ForeColor = Color.White, Font = new Font("Consolas", 10.5F, FontStyle.Bold) };
            btnVolver.Click += (s, e) => { ActualizarHUD(); MostrarPanel(pnlJuego); };

            pnlMapa.Controls.AddRange(new Control[] { tit, lblMapaGrid, lblMapaInfo, lblMapaEvento, pnlLeyenda, btnW, btnS, btnA, btnD, btnVolver });
            this.Controls.Add(pnlMapa);
        }
        private void InicializarMapa()
        {
            filaJ = 6; colJ = 14; recursos = 0; anomalias = 0;
            for (int f = 0; f < 13; f++)
                for (int c = 0; c < 29; c++) { mapa[f, c] = '.'; zonas[f, c] = (f >= 4 && f <= 8 && c >= 11 && c <= 17); }

            for (int c = 0; c < 29; c++) { mapa[2, c] = '~'; mapa[10, c] = '^'; }
            for (int f = 3; f < 10; f++) mapa[f, 25] = '#';

            mapa[6, 14] = 'H'; mapa[4, 20] = 'R'; mapa[8, 6] = 'A'; mapa[5, 23] = 'I';
            mapa[5, 16] = 'E'; mapa[7, 12] = 'E'; mapa[3, 19] = 'E';
        }

        private double CalcularPorcentajeCartografia()
        {
            int reg = 0;
            for (int f = 0; f < 13; f++)
                for (int c = 0; c < 29; c++)
                    if (zonas[f, c]) reg++;
            return reg * 100.0 / (13 * 29);
        }

        private void ActualizarVistaMapa()
        {
            string txt = "    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8\n";
            for (int f = 0; f < 13; f++)
            {
                txt += $"{f,2}  ";
                for (int c = 0; c < 29; c++)
                {
                    char sim = (f == filaJ && c == colJ) ? 'X' : (zonas[f, c] ? mapa[f, c] : '?');
                    txt += sim + " ";
                }
                txt += "\n";
            }
            lblMapaGrid.Text = txt;
            lblMapaInfo.Text = $"Posición: [{filaJ}, {colJ}] | Cartografía: {CalcularPorcentajeCartografia():0.0}% | Recursos: {recursos} | Energía: {cadete.Energia}%";
        }

        private void MoverEnMapa(int df, int dc, string dir)
        {
            int nf = filaJ + df, nc = colJ + dc;
            if (nf < 0 || nf >= 13 || nc < 0 || nc >= 29) { lblMapaEvento.ForeColor = Color.Orange; lblMapaEvento.Text = "Límite del mapa alcanzado."; return; }
            if (mapa[nf, nc] == '#') { zonas[nf, nc] = true; lblMapaEvento.ForeColor = Color.OrangeRed; lblMapaEvento.Text = "Zona bloqueada (#): Barrera estructural."; ActualizarVistaMapa(); return; }

            bool eraDesconocida = !zonas[nf, nc];
            filaJ = nf; colJ = nc;
            zonas[nf, nc] = true;
            cadete.ConsumirEnergia(1);

            string evento = $"Se movió hacia {dir} -> Coordenada [{filaJ},{colJ}]";
            char lugar = mapa[nf, nc];

            if (lugar == 'E')
            {
                mapa[nf, nc] = '.';
                ConexionSQL.RegistrarHistorial(cadete, "MOVIMIENTO_MAPA", evento + " (¡Emboscada!)");
                ActualizarVistaMapa();
                combateDesdeMapa = true;
                IniciarCombate();
                return;
            }
            else if (lugar == 'R') { recursos++; cadete.Inventario.Add(new Objeto("Cristal Energético", TipoObjeto.Energia, 20)); mapa[nf, nc] = '.'; evento += " (¡Halló Recurso!)"; lblMapaEvento.ForeColor = Color.Lime; lblMapaEvento.Text = "¡RECURSO ENCONTRADO! Cristal Energético añadido."; }
            else if (lugar == 'A') { anomalias++; cadete.ReducirEstabilidad(5); cadete.MisionActual.ZonaLlegada = true; mapa[nf, nc] = '.'; evento += " (¡Anomalía!)"; lblMapaEvento.ForeColor = Color.Yellow; lblMapaEvento.Text = "ANOMALÍA DETECTADA: Radiación espacial (-5 Estabilidad)."; }
            else if (lugar == 'I') { cadete.DatosIris++; cadete.Inventario.Add(new Objeto("Analizador IRIS", TipoObjeto.Analizador, 0)); mapa[nf, nc] = '.'; evento += " (¡Módulo IRIS!)"; lblMapaEvento.ForeColor = Color.Cyan; lblMapaEvento.Text = "¡MÓDULO IRIS ENCONTRADO! Datos IRIS incrementados."; }
            else if (lugar == 'H') { lblMapaEvento.ForeColor = Color.Cyan; lblMapaEvento.Text = "Base Central NEXUS: Punto seguro de operaciones."; }
            else if (lugar == '~') { lblMapaEvento.ForeColor = Color.DeepSkyBlue; lblMapaEvento.Text = "Sector Acuático: Navegando sobre superficie líquida."; }
            else if (lugar == '^') { lblMapaEvento.ForeColor = Color.SandyBrown; lblMapaEvento.Text = "Terreno Montañoso: Elevación rocosa cartografiada."; }
            else if (eraDesconocida && rnd.Next(1, 101) <= 15) { ConexionSQL.RegistrarHistorial(cadete, "MOVIMIENTO_MAPA", evento + " (¡Encuentro sorpresa!)"); ActualizarVistaMapa(); combateDesdeMapa = true; IniciarCombate(); return; }
            else { lblMapaEvento.ForeColor = Color.LightGreen; lblMapaEvento.Text = $"Sector [{filaJ}, {colJ}] despejado y cartografiado correctamente."; }

            ConexionSQL.RegistrarHistorial(cadete, "MOVIMIENTO_MAPA", evento);
            ActualizarVistaMapa();
        }

        private void FormJuego_KeyDown(object sender, KeyEventArgs e)
        {
            if (!pnlMapa.Visible) return;
            if (e.KeyCode == Keys.W) MoverEnMapa(-1, 0, "Norte (W)");
            else if (e.KeyCode == Keys.S) MoverEnMapa(1, 0, "Sur (S)");
            else if (e.KeyCode == Keys.A) MoverEnMapa(0, -1, "Oeste (A)");
            else if (e.KeyCode == Keys.D) MoverEnMapa(0, 1, "Este (D)");
        }

        private void ConstruirCombate()
        {
            pnlCombate = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(22, 10, 14) };
            Label alerta = new Label { Text = "ALERTA DE ENCUENTRO HOSTIL", Font = new Font("Consolas", 16F, FontStyle.Bold), ForeColor = Color.Red, Location = new Point(30, 25), AutoSize = true };
            lblEnemigoInfo = new Label { Font = new Font("Consolas", 12F, FontStyle.Bold), ForeColor = Color.Gold, Location = new Point(30, 70), AutoSize = true };
            lblCombateCadete = new Label { Font = new Font("Consolas", 12F, FontStyle.Bold), ForeColor = Color.Lime, Location = new Point(30, 100), AutoSize = true };
            rtbCombateLog = new RichTextBox { Location = new Point(30, 140), Size = new Size(960, 320), BackColor = Color.Black, ForeColor = Color.White, Font = new Font("Consolas", 11F), ReadOnly = true };

            Button btnAtacar = new Button { Text = "1. ATACAR", Location = new Point(30, 490), Size = new Size(220, 50), BackColor = Color.DarkRed, ForeColor = Color.White, Font = new Font("Consolas", 11F, FontStyle.Bold) };
            Button btnEscapar = new Button { Text = "2. ESCAPAR", Location = new Point(275, 490), Size = new Size(220, 50), BackColor = Color.FromArgb(40, 40, 60), ForeColor = Color.Cyan, Font = new Font("Consolas", 11F, FontStyle.Bold) };
            Button btnAnalizar = new Button { Text = "3. ANALIZAR", Location = new Point(520, 490), Size = new Size(220, 50), BackColor = Color.FromArgb(0, 90, 110), ForeColor = Color.White, Font = new Font("Consolas", 11F, FontStyle.Bold) };

            btnAtacar.Click += BtnAtacar_Click;
            btnEscapar.Click += BtnEscapar_Click;
            btnAnalizar.Click += (s, e) => { rtbCombateLog.AppendText($"Análisis: {enemigoActual.Descripcion} (Ataque: {enemigoActual.Ataque})\n"); };

            pnlCombate.Controls.AddRange(new Control[] { alerta, lblEnemigoInfo, lblCombateCadete, rtbCombateLog, btnAtacar, btnEscapar, btnAnalizar });
            this.Controls.Add(pnlCombate);
        }

        private void IniciarCombate()
        {
            enemigoActual = rnd.Next(0, 2) == 0 ? (Entidad)new Robot() : new Enemigo();
            rtbCombateLog.Clear();
            rtbCombateLog.AppendText($"¡{enemigoActual.Nombre} bloquea tu camino!\n");
            ActualizarVistaCombate();
            MostrarPanel(pnlCombate);
        }

        private void ActualizarVistaCombate()
        {
            lblEnemigoInfo.Text = $"Entidad: {enemigoActual.Nombre} | Salud Enemiga: {Math.Max(0, enemigoActual.Salud)}";
            lblCombateCadete.Text = $"Cadete {cadete.Nombre} -> Salud: {cadete.Salud}% | Escudo: {cadete.Escudo}";
        }

        private void BtnAtacar_Click(object sender, EventArgs e)
        {
            int dano = cadete.AtaqueBase;
            enemigoActual.Salud -= dano;
            rtbCombateLog.AppendText($"Atacas a {enemigoActual.Nombre} causando {dano} de daño.\n");

            if (enemigoActual.Salud <= 0)
            {
                cadete.XP += 35;
                cadete.MisionActual.EntidadSobrevivida = true;
                ConexionSQL.RegistrarHistorial(cadete, "COMBATE", $"Derrotó a {enemigoActual.Nombre} (+35 XP)");
                MessageBox.Show($"¡Has derrotado a {enemigoActual.Nombre}! +35 XP");
                ActualizarHUD();
                if (combateDesdeMapa) { ActualizarVistaMapa(); MostrarPanel(pnlMapa); }
                else MostrarPanel(pnlJuego);
                return;
            }

            cadete.RecibirDaño(enemigoActual.Ataque);
            rtbCombateLog.AppendText($"{enemigoActual.Nombre} contraataca e inflige {enemigoActual.Ataque} de daño.\n");
            ActualizarVistaCombate();
            if (cadete.Salud <= 0) ActualizarHUD();
        }

        private void BtnEscapar_Click(object sender, EventArgs e)
        {
            if (rnd.Next(0, 2) == 0)
            {
                ConexionSQL.RegistrarHistorial(cadete, "COMBATE", $"Escapó con éxito de {enemigoActual.Nombre}");
                MessageBox.Show("Escapaste exitosamente del combate.");
                ActualizarHUD();
                if (combateDesdeMapa) { ActualizarVistaMapa(); MostrarPanel(pnlMapa); }
                else MostrarPanel(pnlJuego);
            }
            else
            {
                cadete.RecibirDaño(enemigoActual.Ataque);
                rtbCombateLog.AppendText($"Escape fallido. Recibes {enemigoActual.Ataque} de daño.\n");
                ActualizarVistaCombate();
                if (cadete.Salud <= 0) ActualizarHUD();
            }
        }

        private void ConstruirHistorial()
        {
            pnlHistorial = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(10, 14, 22) };
            Label tit = new Label { Text = "HISTORIAL DE MOVIMIENTOS Y PARTIDAS (SQL SERVER)", Font = new Font("Consolas", 14F, FontStyle.Bold), ForeColor = Color.Cyan, Location = new Point(25, 20), AutoSize = true };

            Button btnRefrescar = new Button { Text = "Refrescar SQL", Location = new Point(680, 15), Size = new Size(150, 35), BackColor = Color.FromArgb(0, 110, 130), ForeColor = Color.White, Font = new Font("Consolas", 10F, FontStyle.Bold) };
            btnRefrescar.Click += (s, e) => dgvHistorial.DataSource = ConexionSQL.ObtenerHistorialCompleto();

            Button btnVolver = new Button { Text = "Volver", Location = new Point(850, 15), Size = new Size(150, 35), BackColor = Color.FromArgb(95, 25, 25), ForeColor = Color.White, Font = new Font("Consolas", 10F, FontStyle.Bold) };
            btnVolver.Click += (s, e) => MostrarPanel(pnlJuego);

            dgvHistorial = new DataGridView
            {
                Location = new Point(25, 65),
                Size = new Size(975, 550),
                BackgroundColor = Color.FromArgb(16, 22, 34),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };
            dgvHistorial.DefaultCellStyle.BackColor = Color.FromArgb(20, 28, 42);
            dgvHistorial.DefaultCellStyle.ForeColor = Color.Cyan;
            dgvHistorial.DefaultCellStyle.Font = new Font("Consolas", 9.5F);

            pnlHistorial.Controls.AddRange(new Control[] { tit, btnRefrescar, btnVolver, dgvHistorial });
            this.Controls.Add(pnlHistorial);
        }
    }
}
