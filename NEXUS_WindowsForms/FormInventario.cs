using System;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    public partial class FormInventario : Form
    {
        private Cadete explorador;

        public FormInventario(Cadete cadete)
        {
            InitializeComponent();
            explorador = cadete;

            btnUsarObjeto.Click += BtnUsarObjeto_Click;
            btnEliminarObjeto.Click += BtnEliminarObjeto_Click;
            btnCerrarInventario.Click += (s, e) => this.Close();

            RefrescarLista();
        }

        private void RefrescarLista()
        {
            lstInventario.Items.Clear();
            foreach (var obj in explorador.Inventario)
            {
                lstInventario.Items.Add(obj);
            }
        }

        private void BtnUsarObjeto_Click(object sender, EventArgs e)
        {
            if (lstInventario.SelectedIndex < 0)
            {
                MessageBox.Show("Selecciona un objeto de la lista primero.", "Inventario", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Objeto seleccionado = explorador.Inventario[lstInventario.SelectedIndex];

            if (seleccionado.Tipo == "Curacion")
                explorador.Salud += seleccionado.Poder;
            else if (seleccionado.Tipo == "Energia")
                explorador.Energia += seleccionado.Poder;
            else if (seleccionado.Tipo == "Estabilidad")
                explorador.Estabilidad += seleccionado.Poder;

            explorador.AjustarStats();
            explorador.Inventario.RemoveAt(lstInventario.SelectedIndex);
            RefrescarLista();

            MessageBox.Show($"Usaste {seleccionado.Nombre}. Efecto aplicado (+{seleccionado.Poder}).", "Objeto Equipado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEliminarObjeto_Click(object sender, EventArgs e)
        {
            if (lstInventario.SelectedIndex < 0) return;
            explorador.Inventario.RemoveAt(lstInventario.SelectedIndex);
            RefrescarLista();
        }
    }
}