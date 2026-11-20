using System;
using System.Collections.Generic;
using System.Linq;

namespace NEXUS_WindowsForms
{
    public class Objeto
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int Poder { get; set; }

        public Objeto(string nombre, string tipo, int poder)
        {
            Nombre = nombre;
            Tipo = tipo;
            Poder = poder;
        }

        public override string ToString()
        {
            return $"{Nombre} [{Tipo}] (+{Poder})";
        }
    }

    public class Anomalia
    {
        public string Nombre { get; set; }
        public int NivelPeligro { get; set; }
        public bool Analizada { get; set; }

        public Anomalia(string nombre, int peligro)
        {
            Nombre = nombre;
            NivelPeligro = peligro;
            Analizada = false;
        }
    }

    public class Mision
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public bool Completada { get; set; }

        public Mision(string titulo, string descripcion)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            Completada = false;
        }
    }

    public abstract class Entidad
    {
        public string Nombre { get; set; }
        public int Salud { get; set; }
        public int Ataque { get; set; }

        protected Entidad(string nombre, int salud, int ataque)
        {
            Nombre = nombre;
            Salud = salud;
            Ataque = ataque;
        }

        public abstract string Descripcion();
    }

    public class Robot : Entidad
    {
        public Robot() : base("Sentry Bot Mk-II", 55, 14) { }
        public override string Descripcion() => "Unidad autónoma de seguridad con blindaje ligero y pulso láser.";
    }

    public class Enemigo : Entidad
    {
        public Enemigo() : base("Xenomorfo de las Sombras", 70, 18) { }
        public override string Descripcion() => "Entidad interdimensional altamente agresiva distorsionada por la brecha.";
    }

    public class Cadete
    {
        public string Nombre { get; set; }
        public int Salud { get; set; } = 100;
        public int Energia { get; set; } = 80;
        public int Estabilidad { get; set; } = 85;
        public int Peligro { get; set; } = 15;
        public int Escudo { get; set; } = 20;
        public int AtaqueBase { get; set; } = 22;
        public int AnomaliasAnalizadas { get; set; } = 0;

        public List<Objeto> Inventario { get; set; } = new List<Objeto>();
        public List<Mision> Misiones { get; set; } = new List<Mision>();
        public Anomalia AnomaliaActual { get; set; }

        public Cadete(string nombre)
        {
            Nombre = string.IsNullOrWhiteSpace(nombre) ? "Recluta-01" : nombre.Trim();

            Inventario.Add(new Objeto("Botiquín Nano-Med", "Curacion", 35));
            Inventario.Add(new Objeto("Célula de Energía", "Energia", 40));
            Inventario.Add(new Objeto("Estabilizador Cuántico", "Estabilidad", 30));

            Misiones.Add(new Mision("Cartografiar Sector", "Explorar al menos el 25% de la matriz 2D."));
            Misiones.Add(new Mision("Cazador de Anomalías", "Analizar 2 anomalías dimensionales."));
            Misiones.Add(new Mision("Extracción Segura", "Alcanzar Estabilidad >= 60% y Peligro <= 40% para extraer."));
        }

        public void AjustarStats()
        {
            Salud = Math.Max(0, Math.Min(100, Salud));
            Energia = Math.Max(0, Math.Min(100, Energia));
            Estabilidad = Math.Max(0, Math.Min(100, Estabilidad));
            Peligro = Math.Max(0, Math.Min(100, Peligro));
        }

        public void VerificarMisiones(double porcentajeMapa)
        {
            if (porcentajeMapa >= 25.0) Misiones[0].Completada = true;
            if (AnomaliasAnalizadas >= 2) Misiones[1].Completada = true;
            if (Estabilidad >= 60 && Peligro <= 40) Misiones[2].Completada = true;
        }
    }

    public class MapaNexus
    {
        public const int Filas = 13;
        public const int Columnas = 29;
        public char[,] Grid { get; private set; } = new char[Filas, Columnas];
        public bool[,] Descubierto { get; private set; } = new bool[Filas, Columnas];
        public int JugadorFila { get; set; } = 6;
        public int JugadorCol { get; set; } = 14;
        public int RecursosRecolectados { get; set; } = 0;
        public int AnomaliasDetectadas { get; set; } = 0;

        public MapaNexus()
        {
            GenerarMapa();
        }

        private void GenerarMapa()
        {
            Random rnd = new Random();
            for (int f = 0; f < Filas; f++)
            {
                for (int c = 0; c < Columnas; c++)
                {
                    if (f == 0 || f == Filas - 1 || c == 0 || c == Columnas - 1)
                        Grid[f, c] = '#';
                    else
                    {
                        int n = rnd.Next(100);
                        if (n < 12) Grid[f, c] = '#';
                        else if (n < 18) Grid[f, c] = 'R';
                        else if (n < 23) Grid[f, c] = 'A';
                        else Grid[f, c] = '.';
                    }
                }
            }
            Grid[JugadorFila, JugadorCol] = '.';
            RevelarAlrededor();
        }

        public void RevelarAlrededor()
        {
            for (int df = -1; df <= 1; df++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    int nf = JugadorFila + df;
                    int nc = JugadorCol + dc;
                    if (nf >= 0 && nf < Filas && nc >= 0 && nc < Columnas)
                        Descubierto[nf, nc] = true;
                }
            }
        }

        public double ObtenerPorcentajeExplorado()
        {
            int total = Filas * Columnas;
            int vis = 0;
            for (int f = 0; f < Filas; f++)
                for (int c = 0; c < Columnas; c++)
                    if (Descubierto[f, c]) vis++;
            return (vis * 100.0) / total;
        }
    }
}
