using System;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    public partial class FormIngreso : Form
    {
        public FormIngreso()
        {
            InitializeComponent();
            btnIniciar.Click += BtnIniciar_Click;
            txtNombre.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) BtnIniciar_Click(s, e);
            };
        }

        private void BtnIniciar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("Por favor ingresa tu nombre o código de Cadete.", "NEXUS - Acceso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            Cadete nuevoCadete = new Cadete(nombre);
            nuevoCadete.Energia = (int)numEnergia.Value;
            nuevoCadete.Estabilidad = (int)numEstabilidad.Value;

            FormMenu menu = new FormMenu(nuevoCadete);
            this.Hide();
            menu.FormClosed += (s, args) => this.Close();
            menu.Show();
        }
    }
}