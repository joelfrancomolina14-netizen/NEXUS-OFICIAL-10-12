using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEXUS_WindowsForms
{
    public partial class FormConexion : Form
    {
        private Cadete explorador;
        private TcpListener servidor;
        private TcpClient cliente;
        private StreamReader lector;
        private StreamWriter escritor;
        private const int Puerto = 5000;

        public FormConexion(Cadete cadete)
        {
            InitializeComponent();
            explorador = cadete;

            btnIniciarServidor.Click += async (s, e) => await IniciarServidorAsync();
            btnConectarCliente.Click += async (s, e) => await ConectarClienteAsync();
            btnEnviarMensajeRed.Click += async (s, e) => await EnviarMensajeAsync();
            txtMensajeRed.KeyDown += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) await EnviarMensajeAsync();
            };

            this.FormClosing += (s, e) => CerrarRed();
        }

        private async Task IniciarServidorAsync()
        {
            try
            {
                servidor = new TcpListener(IPAddress.Any, Puerto);
                servidor.Start();
                lblEstadoRed.Text = $"Estado: ESPERANDO EN PUERTO {Puerto}...";
                lblEstadoRed.ForeColor = Color.Yellow;
                EscribirChat("[RED] Servidor iniciado. Esperando conexión de otro equipo...");

                cliente = await servidor.AcceptTcpClientAsync();
                ConfigurarFlujos();
                lblEstadoRed.Text = "Estado: CONECTADO (SERVIDOR)";
                lblEstadoRed.ForeColor = Color.Lime;
                EscribirChat("[RED] ¡Otro equipo se ha conectado!");
                _ = EscucharMensajesAsync();
            }
            catch (Exception ex)
            {
                EscribirChat($"[ERROR] {ex.Message}");
            }
        }

        private async Task ConectarClienteAsync()
        {
            try
            {
                string ip = txtIpServidor.Text.Trim();
                cliente = new TcpClient();
                await cliente.ConnectAsync(ip, Puerto);
                ConfigurarFlujos();
                lblEstadoRed.Text = $"Estado: CONECTADO A {ip}";
                lblEstadoRed.ForeColor = Color.Lime;
                EscribirChat($"[RED] Conectado exitosamente al servidor {ip}.");
                _ = EscucharMensajesAsync();
            }
            catch (Exception ex)
            {
                EscribirChat($"[ERROR] No se pudo conectar: {ex.Message}");
            }
        }

        private void ConfigurarFlujos()
        {
            NetworkStream stream = cliente.GetStream();
            lector = new StreamReader(stream);
            escritor = new StreamWriter(stream) { AutoFlush = true };
        }

        private async Task EscucharMensajesAsync()
        {
            try
            {
                while (cliente != null && cliente.Connected)
                {
                    string linea = await lector.ReadLineAsync();
                    if (linea == null) break;
                    EscribirChat(linea);
                }
            }
            catch { }
        }

        private async Task EnviarMensajeAsync()
        {
            if (escritor == null || string.IsNullOrWhiteSpace(txtMensajeRed.Text)) return;
            try
            {
                string msg = $"[{explorador.Nombre}]: {txtMensajeRed.Text.Trim()}";
                await escritor.WriteLineAsync(msg);
                EscribirChat(msg);
                txtMensajeRed.Clear();
            }
            catch (Exception ex)
            {
                EscribirChat($"[ERROR] {ex.Message}");
            }
        }

        private void EscribirChat(string texto)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(EscribirChat), texto);
                return;
            }
            txtChatRed.AppendText(texto + Environment.NewLine);
            txtChatRed.ScrollToCaret();
        }

        private void CerrarRed()
        {
            try { servidor?.Stop(); } catch { }
            try { cliente?.Close(); } catch { }
        }
    }
}