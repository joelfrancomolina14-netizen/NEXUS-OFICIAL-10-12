using System;
using System.Drawing;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    public partial class FormMenu : Form
    {
        public Cadete Explorador { get; private set; }
        public MapaNexus Mapa { get; private set; }
        private Random rnd = new Random();

        public FormMenu(Cadete cadete)
        {
            InitializeComponent();
            Explorador = cadete;
            Mapa = new MapaNexus();

            lblCadeteTitulo.Text = $"NEXUS SYSTEM v1.0 - Cadete {Explorador.Nombre}";
            ConectarEventos();
            ActualizarHUD();
            EscribirTerminal($"[SISTEMA] Bienvenido, Cadete {Explorador.Nombre}. Enlace neuronal estabilizado.", Color.Cyan);
            EscribirTerminal("[SISTEMA] Selecciona una operación en el panel izquierdo.", Color.LightGray);
        }

        private void ConectarEventos()
        {
            btnExplorar.Click += (s, e) => AccionExplorar();
            btnAnalizar.Click += (s, e) => AccionAnalizar();
            btnEstado.Click += (s, e) => AccionConsultarEstado();
            btnRecuperar.Click += (s, e) => AccionRecuperarEnergia();
            btnEvaluar.Click += (s, e) => AccionEvaluarCondicion();
            btnDesconectar.Click += (s, e) => AccionDesconectar();
            btnConectar.Click += (s, e) => AbrirConexion();
            btnEnviarMensaje.Click += (s, e) => AbrirConexion();
            btnMapa.Click += (s, e) => AbrirMapa();
            btnInventario.Click += (s, e) => AbrirInventario();
            btnMisiones.Click += (s, e) => AccionVerMisiones();
            btnExtraccion.Click += (s, e) => AccionExtraccion();
        }

        public void ActualizarHUD()
        {
            Explorador.AjustarStats();
            Explorador.VerificarMisiones(Mapa.ObtenerPorcentajeExplorado());

            pbSalud.Value = Explorador.Salud;
            lblValorSalud.Text = $"{Explorador.Salud}%";

            pbEnergia.Value = Explorador.Energia;
            lblValorEnergia.Text = $"{Explorador.Energia}%";

            pbEstabilidad.Value = Explorador.Estabilidad;
            lblValorEstabilidad.Text = $"{Explorador.Estabilidad}%";

            pbPeligro.Value = Explorador.Peligro;
            lblValorPeligro.Text = $"{Explorador.Peligro}%";

            if (Explorador.Salud <= 0)
            {
                MessageBox.Show("Tu señal vital cayó a 0%. Misión fallida.", "NEXUS - Colapso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        public void EscribirTerminal(string texto, Color color)
        {
            txtTerminal.SelectionStart = txtTerminal.TextLength;
            txtTerminal.SelectionLength = 0;
            txtTerminal.SelectionColor = color;
            txtTerminal.AppendText(texto + Environment.NewLine);
            txtTerminal.ScrollToCaret();
        }

        private void AccionExplorar()
        {
            if (Explorador.Energia < 10)
            {
                EscribirTerminal("[ALERTA] Energía insuficiente (mínimo 10%). Recupera energía primero.", Color.Orange);
                return;
            }

            Explorador.Energia -= 10;
            int evento = rnd.Next(1, 5);

            if (evento == 1)
            {
                string[] nombresAnomalias = { "Fisura Temporal Alpha", "Eco Gravitacional", "Singularidad Invertida", "Distorsión de Fase" };
                string nombre = nombresAnomalias[rnd.Next(nombresAnomalias.Length)];
                int peligro = rnd.Next(15, 35);
                Explorador.AnomaliaActual = new Anomalia(nombre, peligro);
                Explorador.Peligro += 10;
                Explorador.Estabilidad -= 8;
                EscribirTerminal($"[EXPLORACIÓN] ¡Anomalía detectada!: {nombre} (Peligro: {peligro}). Usa la opción 2 para analizarla.", Color.Yellow);
            }
            else if (evento == 2)
            {
                Entidad enemigo = rnd.Next(2) == 0 ? (Entidad)new Robot() : new Enemigo();
                EscribirTerminal($"[COMBATE] ¡Encuentro hostil con {enemigo.Nombre}!", Color.Red);
                using (FormCombate combate = new FormCombate(Explorador, enemigo))
                {
                    combate.ShowDialog(this);
                }
                EscribirTerminal("[COMBATE] Encuentro finalizado. Revisa tus signos vitales.", Color.Cyan);
            }
            else if (evento == 3)
            {
                Objeto[] botin = {
                    new Objeto("Botiquín Nano-Med", "Curacion", 35),
                    new Objeto("Célula de Energía", "Energia", 40),
                    new Objeto("Estabilizador Cuántico", "Estabilidad", 30)
                };
                Objeto encontrado = botin[rnd.Next(botin.Length)];
                Explorador.Inventario.Add(encontrado);
                EscribirTerminal($"[HALLAZGO] Encontraste un suministro: {encontrado.Nombre}. Añadido al inventario.", Color.Lime);
            }
            else
            {
                Explorador.Estabilidad += 5;
                EscribirTerminal("[EXPLORACIÓN] Sector despejado. Lecturas ambientales estables (+5% Estabilidad).", Color.LightGreen);
            }

            ActualizarHUD();
        }

        private void AccionAnalizar()
        {
            if (Explorador.AnomaliaActual == null || Explorador.AnomaliaActual.Analizada)
            {
                EscribirTerminal("[ESCÁNER] No hay anomalías pendientes por analizar. Explora primero (Opción 1).", Color.Gray);
                return;
            }
            if (Explorador.Energia < 15)
            {
                EscribirTerminal("[ALERTA] Necesitas al menos 15% de energía para ejecutar el escáner.", Color.Orange);
                return;
            }

            Explorador.Energia -= 15;
            Explorador.AnomaliaActual.Analizada = true;
            Explorador.AnomaliasAnalizadas++;
            Explorador.Estabilidad += 12;
            Explorador.Peligro -= 10;
            EscribirTerminal($"[ESCÁNER] Anomalía '{Explorador.AnomaliaActual.Nombre}' decodificada con éxito (+12% Estabilidad, -10% Peligro).", Color.Cyan);
            ActualizarHUD();
        }

        private void AccionConsultarEstado()
        {
            EscribirTerminal("--------------------------------------------------", Color.DarkCyan);
            EscribirTerminal($"[ESTADO] Cadete: {Explorador.Nombre} | Escudo: {Explorador.Escudo} | Ataque: {Explorador.AtaqueBase}", Color.White);
            EscribirTerminal($"         Salud: {Explorador.Salud}% | Energía: {Explorador.Energia}% | Estabilidad: {Explorador.Estabilidad}% | Peligro: {Explorador.Peligro}%", Color.Cyan);
            EscribirTerminal($"         Anomalías analizadas: {Explorador.AnomaliasAnalizadas} | Mapa explorado: {Mapa.ObtenerPorcentajeExplorado():F1}%", Color.LightGray);
            EscribirTerminal("--------------------------------------------------", Color.DarkCyan);
        }

        private void AccionRecuperarEnergia()
        {
            int ganada = rnd.Next(20, 31);
            Explorador.Energia += ganada;
            Explorador.Peligro += 5;
            EscribirTerminal($"[RECARGA] Módulos solares activados: +{ganada}% Energía (Peligro +5% por emisión térmica).", Color.Lime);
            ActualizarHUD();
        }

        private void AccionEvaluarCondicion()
        {
            bool segura = Explorador.Estabilidad >= 60 && Explorador.Peligro <= 40 && Explorador.Salud >= 40;
            if (segura)
                EscribirTerminal("[DIAGNÓSTICO] CONDICIÓN SEGURA: Parámetros óptimos para extracción o exploración profunda.", Color.Lime);
            else
                EscribirTerminal("[DIAGNÓSTICO] CONDICIÓN CRÍTICA: Requiere Estabilidad >= 60%, Peligro <= 40% y Salud >= 40%.", Color.OrangeRed);
        }

        private void AccionDesconectar()
        {
            var resp = MessageBox.Show("¿Deseas intentar desconexión inmediata de la simulación NEXUS?", "Desconexión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
            {
                if (Explorador.Peligro > 60)
                    EscribirTerminal("[ERROR] Interferencia demasiado alta (Peligro > 60%). Reduce el peligro antes de desconectar.", Color.Red);
                else
                    this.Close();
            }
        }

        private void AccionVerMisiones()
        {
            Explorador.VerificarMisiones(Mapa.ObtenerPorcentajeExplorado());
            EscribirTerminal("=== OBJETIVOS DE MISIÓN NEXUS ===", Color.Cyan);
            foreach (var m in Explorador.Misiones)
            {
                string estado = m.Completada ? "[COMPLETADA]" : "[PENDIENTE]";
                EscribirTerminal($"{estado} {m.Titulo}: {m.Descripcion}", m.Completada ? Color.Lime : Color.Yellow);
            }
        }

        private void AccionExtraccion()
        {
            if (Explorador.Estabilidad >= 60 && Explorador.Peligro <= 40)
            {
                MessageBox.Show($"¡EXTRACCIÓN EXITOSA, Cadete {Explorador.Nombre}!\nHas completado tu incursión en NEXUS.", "NEXUS - Victoria", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                EscribirTerminal("[EXTRACCIÓN DENEGADA] Necesitas Estabilidad >= 60% y Peligro <= 40% para abrir el portal.", Color.Red);
            }
        }

        private void AbrirMapa()
        {
            using (FormMapa fMapa = new FormMapa(Explorador, Mapa))
            {
                fMapa.ShowDialog(this);
            }
            ActualizarHUD();
            EscribirTerminal($"[CARTOGRAFÍA] Regresaste del mapa. Progreso actual: {Mapa.ObtenerPorcentajeExplorado():F1}%.", Color.Cyan);
        }

        private void AbrirInventario()
        {
            using (FormInventario fInv = new FormInventario(Explorador))
            {
                fInv.ShowDialog(this);
            }
            ActualizarHUD();
        }

        private void AbrirConexion()
        {
            using (FormConexion fCon = new FormConexion(Explorador))
            {
                fCon.ShowDialog(this);
            }
        }
    }
}