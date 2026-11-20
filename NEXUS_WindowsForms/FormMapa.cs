using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    public partial class FormMapa : Form
    {
        private Cadete explorador;
        private MapaNexus mapa;
        private Random rnd = new Random();

        public FormMapa(Cadete cadete, MapaNexus mapaActual)
        {
            InitializeComponent();
            explorador = cadete;
            mapa = mapaActual;

            btnArriba.Click += (s, e) => Mover(-1, 0);
            btnAbajo.Click += (s, e) => Mover(1, 0);
            btnIzquierda.Click += (s, e) => Mover(0, -1);
            btnDerecha.Click += (s, e) => Mover(0, 1);
            btnVolverMapa.Click += (s, e) => this.Close();

            this.KeyDown += FormMapa_KeyDown;
            RenderizarMapa();
        }

        private void FormMapa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up) Mover(-1, 0);
            else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down) Mover(1, 0);
            else if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) Mover(0, -1);
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right) Mover(0, 1);
            else if (e.KeyCode == Keys.Q || e.KeyCode == Keys.Escape) this.Close();
        }

        private void Mover(int df, int dc)
        {
            int nf = mapa.JugadorFila + df;
            int nc = mapa.JugadorCol + dc;

            if (nf < 0 || nf >= MapaNexus.Filas || nc < 0 || nc >= MapaNexus.Columnas) return;

            char celda = mapa.Grid[nf, nc];
            if (celda == '#')
            {
                lblEventoMapa.Text = "Muro de contención bloqueando el paso.";
                lblEventoMapa.ForeColor = Color.OrangeRed;
                return;
            }

            mapa.JugadorFila = nf;
            mapa.JugadorCol = nc;
            mapa.RevelarAlrededor();

            if (celda == 'R')
            {
                mapa.Grid[nf, nc] = '.';
                mapa.RecursosRecolectados++;
                explorador.Energia += 10;
                explorador.AjustarStats();
                lblEventoMapa.Text = "¡Recurso energético recolectado! (+10% Energía)";
                lblEventoMapa.ForeColor = Color.Lime;
            }
            else if (celda == 'A')
            {
                mapa.Grid[nf, nc] = '.';
                mapa.AnomaliasDetectadas++;
                explorador.AnomaliasAnalizadas++;
                explorador.Estabilidad += 5;
                explorador.AjustarStats();
                lblEventoMapa.Text = "¡Anomalía cartografiada en el sector! (+5% Estabilidad)";
                lblEventoMapa.ForeColor = Color.Yellow;
            }
            else
            {
                lblEventoMapa.Text = $"Sector [{nf}, {nc}] despejado.";
                lblEventoMapa.ForeColor = Color.Cyan;
            }

            RenderizarMapa();
        }

        private void RenderizarMapa()
        {
            StringBuilder sb = new StringBuilder();
            for (int f = 0; f < MapaNexus.Filas; f++)
            {
                for (int c = 0; c < MapaNexus.Columnas; c++)
                {
                    if (f == mapa.JugadorFila && c == mapa.JugadorCol)
                        sb.Append('@');
                    else if (!mapa.Descubierto[f, c])
                        sb.Append(' ');
                    else
                        sb.Append(mapa.Grid[f, c]);

                    sb.Append(' ');
                }
                sb.AppendLine();
            }

            lblMapa.Text = sb.ToString();
            lblStatsMapa.Text = $"Cartografía: {mapa.ObtenerPorcentajeExplorado():F1}% | Recursos: {mapa.RecursosRecolectados} | Anomalías: {mapa.AnomaliasDetectadas}";
        }
    }
}