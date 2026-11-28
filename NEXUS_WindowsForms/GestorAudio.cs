using System;
using System.IO;
using NAudio.Wave;

namespace NEXUS_WindowsForms
{
    public static class GestorAudio
    {
        private static WaveOutEvent? salidaMusica;
        private static AudioFileReader? lectorMusica;
        private static string pistaActual = string.Empty;
        public static bool Silenciado { get; private set; } = false;

        private static string ObtenerRutaAudio(string nombreArchivo)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", nombreArchivo);
        }

        public static void ReproducirMusica(string nombreArchivo, float volumen = 0.35f)
        {
            if (pistaActual == nombreArchivo && salidaMusica != null && salidaMusica.PlaybackState == PlaybackState.Playing)
                return;

            DetenerMusica();
            pistaActual = nombreArchivo;

            if (Silenciado) return;

            string ruta = ObtenerRutaAudio(nombreArchivo);
            if (!File.Exists(ruta)) return;

            try
            {
                lectorMusica = new AudioFileReader(ruta) { Volume = volumen };
                var loop = new LoopStream(lectorMusica);
                salidaMusica = new WaveOutEvent();
                salidaMusica.Init(loop);
                salidaMusica.Play();
            }
            catch { }
        }

        public static void ReproducirEfecto(string nombreArchivo, float volumen = 0.75f)
        {
            if (Silenciado) return;

            string ruta = ObtenerRutaAudio(nombreArchivo);
            if (!File.Exists(ruta)) return;

            try
            {
                var lector = new AudioFileReader(ruta) { Volume = volumen };
                var salida = new WaveOutEvent();
                salida.Init(lector);
                salida.PlaybackStopped += (s, e) =>
                {
                    salida.Dispose();
                    lector.Dispose();
                };
                salida.Play();
            }
            catch { }
        }

        public static void AlternarSilencio()
        {
            Silenciado = !Silenciado;
            if (Silenciado)
            {
                DetenerMusica();
            }
            else if (!string.IsNullOrEmpty(pistaActual))
            {
                string pista = pistaActual;
                pistaActual = string.Empty;
                ReproducirMusica(pista);
            }
        }

        public static void DetenerMusica()
        {
            try
            {
                salidaMusica?.Stop();
                salidaMusica?.Dispose();
                lectorMusica?.Dispose();
            }
            catch { }
            finally
            {
                salidaMusica = null;
                lectorMusica = null;
            }
        }

        public static void MusicaMenu() => ReproducirMusica("audio-menu.mp3", 0.35f);
        public static void MusicaLaberinto() => ReproducirMusica("Cyberpunk_laberinto.mp3", 0.35f);
        public static void SonidoClick() => ReproducirEfecto("click-form.mp3", 0.7f);
        public static void SonidoLaser() => ReproducirEfecto("audio-laser.mp3", 0.65f);
        public static void SonidoItem() => ReproducirEfecto("audio-recoger_items.mp3", 0.8f);
        public static void SonidoPerderVida() => ReproducirEfecto("audio-perdervida.mp3", 0.85f);
        public static void SonidoNivelCompletado() => ReproducirEfecto("audio-completarnivel.mp3", 0.85f);
    }

    internal class LoopStream : WaveStream
    {
        private readonly WaveStream sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
        }

        public override WaveFormat WaveFormat => sourceStream.WaveFormat;
        public override long Length => sourceStream.Length;

        public override long Position
        {
            get => sourceStream.Position;
            set => sourceStream.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;
            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0) break;
                    sourceStream.Position = 0;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}