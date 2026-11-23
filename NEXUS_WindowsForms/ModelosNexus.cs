using System;
using System.Collections.Generic;

namespace NEXUS_WindowsForms
{
    public enum TipoObjeto { Energia, Escudo, ModuloAtaque, Analizador }

    public class Objeto
    {
        public string Nombre { get; set; }
        public TipoObjeto Tipo { get; set; }
        public int Valor { get; set; }

        public Objeto(string nombre, TipoObjeto tipo, int valor)
        {
            Nombre = nombre;
            Tipo = tipo;
            Valor = valor;
        }
    }

    public class Mision
    {
        public string Nombre { get; set; }
        public bool ZonaLlegada { get; set; }
        public bool NucleoEncontrado { get; set; }
        public bool EntidadSobrevivida { get; set; }
        public bool ExtraccionCompletada { get; set; }

        public Mision(string nombre = "La anomalía perdida")
        {
            Nombre = nombre;
        }
    }

    public class Cadete
    {
        public int CadeteID { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Realidad { get; set; }
        public int Salud { get; set; }
        public int Energia { get; set; }
        public int Estabilidad { get; set; }
        public int Peligro { get; set; }
        public int DatosIris { get; set; }
        public int Escudo { get; set; }
        public int XP { get; set; }
        public int ModuloAtaque { get; set; }
        public int AtaqueBase => 15 + ModuloAtaque;
        public List<Objeto> Inventario { get; set; }
        public Mision MisionActual { get; set; }

        public Cadete(string nombre, int edad = 18, string realidad = "Neo Cochabamba", int energia = 80, int estabilidad = 85)
        {
            Nombre = nombre;
            Edad = edad;
            Realidad = realidad;
            Salud = 100;
            Energia = energia;
            Estabilidad = estabilidad;
            DatosIris = 0;
            Escudo = 20;
            XP = 0;
            ModuloAtaque = 15;
            Inventario = new List<Objeto>
            {
                new Objeto("🔋 Célula de energía", TipoObjeto.Energia, 25),
                new Objeto("🛡️ Escudo de Plasma", TipoObjeto.Escudo, 20)
            };
            MisionActual = new Mision();
            ActualizarPeligro();
        }

        public void ActualizarPeligro() => Peligro = Math.Clamp(100 - ((Energia + Estabilidad) / 2), 0, 100);

        public void Explorar()
        {
            Energia = Math.Max(0, Energia - 15);
            Estabilidad = Math.Max(0, Estabilidad - 7);
            ActualizarPeligro();
        }

        public int RecuperarEnergia()
        {
            int rec = Energia < 50 ? 20 : 10;
            Energia = Math.Min(100, Energia + rec);
            ActualizarPeligro();
            return rec;
        }

        public void ConsumirEnergia(int cant)
        {
            Energia = Math.Max(0, Energia - cant);
            ActualizarPeligro();
        }

        public void ReducirEstabilidad(int cant)
        {
            Estabilidad = Math.Max(0, Estabilidad - cant);
            ActualizarPeligro();
        }

        public void RecibirDaño(int cant)
        {
            if (Escudo > 0)
            {
                int abs = Math.Min(Escudo, cant);
                Escudo -= abs;
                cant -= abs;
            }
            if (cant > 0) Salud = Math.Max(0, Salud - cant);
        }
    }

    public abstract class Entidad
    {
        public string Nombre { get; set; }
        public int Salud { get; set; }
        public int Ataque { get; set; }
        public string Descripcion { get; set; }

        protected Entidad(string nombre, int salud, int ataque, string desc)
        {
            Nombre = nombre;
            Salud = salud;
            Ataque = ataque;
            Descripcion = desc;
        }
    }

    public class Robot : Entidad
    {
        public Robot() : base("Sentry Bot Mk-II", 45, 12, "Unidad autónoma con blindaje ligero y escudos defensivos.") { }
    }

    public class Enemigo : Entidad
    {
        public Enemigo() : base("Xenomorfo de las Sombras", 60, 15, "Entidad interdimensional altamente agresiva.") { }
    }
}