using System;
using System.Drawing;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    public partial class FormCombate : Form
    {
        private Cadete explorador;
        private Entidad enemigo;
        private Random rnd = new Random();

        public FormCombate(Cadete cadete, Entidad entidadHostil)
        {
            InitializeComponent();
            explorador = cadete;
            enemigo = entidadHostil;

            lblAlertaEntidad.Text = $"⚠️ {enemigo.Nombre.ToUpper()} DETECTADO";
            ActualizarEstadoCombate();
            EscribirLog($"Un {enemigo.Nombre} bloquea tu ruta. Prepárate para actuar.");

            btnAtacar.Click += (s, e) => TurnoAtacar();
            btnEscapar.Click += (s, e) => TurnoEscapar();
            btnUsarObjetoCombate.Click += (s, e) => TurnoUsarObjeto();
            btnAnalizarEntidad.Click += (s, e) => TurnoAnalizar();
        }

        private void ActualizarEstadoCombate()
        {
            lblInfoEntidad.Text = $"Entidad: {enemigo.Nombre} | Salud: {Math.Max(0, enemigo.Salud)}";
            lblInfoCadeteCombate.Text = $"Tu Salud: {explorador.Salud}% | Escudo: {explorador.Escudo}";
        }

        private void EscribirLog(string mensaje)
        {
            txtLogCombate.AppendText($"> {mensaje}{Environment.NewLine}");
            txtLogCombate.ScrollToCaret();
        }

        private void TurnoAtacar()
        {
            int dano = explorador.AtaqueBase + rnd.Next(-4, 8);
            enemigo.Salud -= dano;
            EscribirLog($"Disparas tu pulso táctico e infliges {dano} de daño a {enemigo.Nombre}.");

            if (enemigo.Salud <= 0)
            {
                EscribirLog($"¡Has neutralizado a {enemigo.Nombre}!");
                explorador.Estabilidad += 10;
                explorador.Peligro -= 8;
                explorador.AjustarStats();
                MessageBox.Show($"¡Victoria! Has derrotado a {enemigo.Nombre}.", "Combate Ganado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            ContraataqueEnemigo();
        }

        private void TurnoEscapar()
        {
            if (rnd.Next(100) < 65)
            {
                EscribirLog("Lograste evadir al enemigo usando propulsores laterales.");
                MessageBox.Show("Escapaste con éxito del combate.", "Retirada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                EscribirLog("¡Intento de escape fallido! El enemigo te intercepta.");
                ContraataqueEnemigo();
            }
        }

        private void TurnoUsarObjeto()
        {
            using (FormInventario inv = new FormInventario(explorador))
            {
                inv.ShowDialog(this);
            }
            ActualizarEstadoCombate();
            EscribirLog("Revisaste tu equipo táctico.");
        }

        private void TurnoAnalizar()
        {
            EscribirLog($"[ANÁLISIS] {enemigo.Descripcion()} (Ataque estimado: {enemigo.Ataque})");
            explorador.AtaqueBase += 4;
            EscribirLog("Debilidad estructural detectada: +4 a tu Ataque Base en este encuentro.");
            ContraataqueEnemigo();
        }

        private void ContraataqueEnemigo()
        {
            int danoRecibido = Math.Max(3, enemigo.Ataque - (explorador.Escudo / 4) + rnd.Next(-2, 4));
            explorador.Salud -= danoRecibido;
            explorador.AjustarStats();
            EscribirLog($"{enemigo.Nombre} contraataca y te causa {danoRecibido}% de daño.");
            ActualizarEstadoCombate();

            if (explorador.Salud <= 0)
            {
                MessageBox.Show("Has caído en combate...", "Derrota", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}