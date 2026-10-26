using NEXUS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NEXUS
{
    // =========================================================
    // 1. SISTEMA DE PERSONAJES (POO: Herencia y Polimorfismo)
    // =========================================================

    public abstract class Personaje
    {
        public string Nombre { get; protected set; }
        public int Salud { get; set; }
        public int Energia { get; set; }

        public Personaje(string nombre, int salud, int energia)
        {
            Nombre = nombre;
            Salud = salud;
            Energia = energia;
        }

        public abstract void Analizar();
    }

    public class Cadete : Personaje
    {
        public int Edad { get; private set; }
        public string Realidad { get; private set; }
        public int Estabilidad { get; private set; }
        public int Peligro { get; private set; }
        public int DatosIris { get; private set; }

        public int Escudo { get; set; }
        public int XP { get; set; }
        public int ModuloAtaque { get; set; }
        public List<Objeto> Inventario { get; private set; }

        public Cadete(string nombre, int edad, string realidad, int energia, int estabilidad)
            : base(nombre, 100, energia)
        {
            Edad = edad;
            Realidad = realidad;
            Estabilidad = estabilidad;
            DatosIris = 0;
            Escudo = 20;
            XP = 0;
            ModuloAtaque = 15;
            Inventario = new List<Objeto>();

            ActualizarPeligro();
        }

        public override void Analizar()
        {
            Console.WriteLine($"\n🔬 [ANÁLISIS DE CADETE]: {Nombre} | Salud: {Salud}% | Energía: {Energia}% | Escudo: {Escudo} | XP: {XP}");
        }

        public void Explorar()
        {
            Energia = Math.Max(0, Energia - 15);
            Estabilidad = Math.Max(0, Estabilidad - 7);
            ActualizarPeligro();
        }

        public int RecuperarEnergia()
        {
            int recuperacion = Energia < 50 ? 20 : 10;
            Energia = Math.Min(100, Energia + recuperacion);
            ActualizarPeligro();
            return recuperacion;
        }

        public void ConsumirEnergia(int cantidad)
        {
            Energia = Math.Max(0, Energia - cantidad);
            ActualizarPeligro();
        }

        public void ReducirEstabilidad(int cantidad)
        {
            Estabilidad = Math.Max(0, Estabilidad - cantidad);
            ActualizarPeligro();
        }

        public void RecibirDaño(int cantidad)
        {
            if (Escudo > 0)
            {
                int absorcion = Math.Min(Escudo, cantidad);
                Escudo -= absorcion;
                cantidad -= absorcion;
            }
            if (cantidad > 0)
            {
                Salud = Math.Max(0, Salud - cantidad);
            }
        }

        public void ObtenerDatoIris()
        {
            DatosIris++;
        }

        public void ActualizarPeligro()
        {
            Peligro = Math.Clamp(100 - ((Energia + Estabilidad) / 2), 0, 100);
        }

        public bool PuedeExplorar() => Energia >= 40;
        public bool CondicionSegura() => Energia >= 50 && Peligro < 50 && Estabilidad >= 50;
    }

    public class Robot : Personaje
    {
        public bool DefensivoActivo { get; set; }

        public Robot(string nombre, int salud, int energia) : base(nombre, salud, energia)
        {
            DefensivoActivo = false;
        }

        public override void Analizar()
        {
            Console.WriteLine($"\n🔬 [ANÁLISIS ROBÓTICO]: {Nombre} | Defensa: {(DefensivoActivo ? "ACTIVA" : "INACTIVA")} | Batería: {Energia}%");
        }

        public void ActivarDefensa()
        {
            DefensivoActivo = true;
            Console.WriteLine($"🤖 {Nombre} activa sus ESCUDOS DEFENSIVOS de emergencia.");
        }
    }

    public class Enemigo : Personaje
    {
        public int FuerzaAtaque { get; set; }

        public Enemigo(string nombre, int salud, int energia, int fuerza) : base(nombre, salud, energia)
        {
            FuerzaAtaque = fuerza;
        }

        public override void Analizar()
        {
            Console.WriteLine($"\n🔬 [ANÁLISIS DE AMENAZA]: {Nombre} | Amenaza: CRÍTICA | Vida: {Salud} | Daño: {FuerzaAtaque}");
        }
    }

    // Alias para mantener compatibilidad con la clase Explorador
    class Explorador : Cadete
    {
        public Explorador(string nombre, int edad, string realidad, int energia, int estabilidad)
            : base(nombre, edad, realidad, energia, estabilidad) { }
    }


    // =========================================================
    // 2. SISTEMA DE INVENTARIO Y OBJETOS
    // =========================================================

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


    // =========================================================
    // 3. SISTEMA DE MISIONES
    // =========================================================

    public class Mision
    {
        public string Nombre { get; set; }
        public bool ZonaLlegada { get; set; }
        public bool NucleoEncontrado { get; set; }
        public bool EntidadSobrevivida { get; set; }
        public bool ExtraccionCompletada { get; set; }

        public Mision(string nombre)
        {
            Nombre = nombre;
            ZonaLlegada = false;
            NucleoEncontrado = false;
            EntidadSobrevivida = false;
            ExtraccionCompletada = false;
        }

        public bool EstaCompletada() => ZonaLlegada && NucleoEncontrado && EntidadSobrevivida && ExtraccionCompletada;
    }


    // =========================================================
    // 4. CLASE MAPA
    // =========================================================

    class Mapa
    {
        private readonly char[,] mapa = new char[13, 29];
        private readonly bool[,] zonas = new bool[13, 29];

        private int filaJugador = 6;
        private int columnaJugador = 14;

        private int recursos;
        private int puntosInteres;
        private int anomalias;

        private bool preparado;

        private readonly string[] nombresRecursos =
        {
            "Cristal energetico",
            "Fragmento de datos",
            "Mineral desconocido",
            "Modulo IRIS"
        };

        public int Recursos => recursos;
        public int PuntosInteres => puntosInteres;
        public int Anomalias => anomalias;

        public Mapa()
        {
            Inicializar();
        }

        private void Inicializar()
        {
            if (preparado) return;

            for (int f = 0; f < 13; f++)
            {
                for (int c = 0; c < 29; c++)
                {
                    mapa[f, c] = '.';
                    zonas[f, c] = false;
                }
            }

            for (int c = 0; c < 29; c++)
            {
                mapa[2, c] = '~';
                mapa[10, c] = '^';
            }

            for (int f = 3; f < 10; f++)
                mapa[f, 25] = '#';

            mapa[6, 14] = 'H';
            mapa[4, 20] = 'R';
            mapa[8, 6] = 'A';
            mapa[5, 23] = 'I';

            for (int f = 4; f <= 8; f++)
            {
                for (int c = 11; c <= 17; c++)
                    zonas[f, c] = true;
            }

            preparado = true;
        }

        public void Mostrar(Cadete explorador)
        {
            string tecla = "";

            while (tecla != "Q")
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==========================================");
                Console.WriteLine("          MAPA DE CARTOGRAFIA");
                Console.WriteLine("==========================================");
                Console.ResetColor();

                Dibujar();

                Console.WriteLine($"\nCartografia: {CalcularCartografia():0.0}%");
                Console.WriteLine($"Recursos: {recursos} | Puntos: {puntosInteres} | Anomalias: {anomalias}");
                Console.WriteLine("W/S/A/D = mover | Q = salir");

                tecla = Console.ReadKey(true).Key.ToString().ToUpper();

                if ("WSAD".Contains(tecla))
                    Mover(tecla, explorador);
            }
        }

        private void Dibujar()
        {
            Console.Write("    ");
            for (int c = 0; c < 29; c++)
                Console.Write((c % 10) + " ");

            Console.WriteLine();

            for (int f = 0; f < 13; f++)
            {
                Console.Write($"{f,2}  ");

                for (int c = 0; c < 29; c++)
                {
                    char simbolo = (f == filaJugador && c == columnaJugador)
                        ? 'X'
                        : zonas[f, c]
                        ? mapa[f, c]
                        : '?';

                    Console.Write(simbolo + " ");
                }
                Console.WriteLine();
            }
        }

        private void Mover(string tecla, Cadete explorador)
        {
            int nf = filaJugador;
            int nc = columnaJugador;

            if (tecla == "W") nf--;
            if (tecla == "S") nf++;
            if (tecla == "A") nc--;
            if (tecla == "D") nc++;

            if (nf < 0 || nf >= 13 || nc < 0 || nc >= 29)
            {
                Console.WriteLine("⚠️ [CONTROL NEXUS]: Limite de mapa alcanzado. Movimiento no permitido.");
                return;
            }

            filaJugador = nf;
            columnaJugador = nc;
            zonas[nf, nc] = true;

            explorador.ConsumirEnergia(3);

            char lugar = mapa[nf, nc];

            if (lugar == 'R')
            {
                recursos++;
                string recursoNombre = nombresRecursos[(recursos - 1) % nombresRecursos.Length];
                Console.WriteLine("🎁 Recurso encontrado: " + recursoNombre);
                explorador.Inventario.Add(new Objeto(recursoNombre, TipoObjeto.Energia, 20));
                mapa[nf, nc] = '.';
            }
            else if (lugar == 'A')
            {
                anomalias++;
                explorador.ReducirEstabilidad(5);
                Console.WriteLine("⚡ Anomalia detectada. Estabilidad -5.");
                mapa[nf, nc] = '.';
            }
            else if (lugar == 'I')
            {
                explorador.ObtenerDatoIris();
                Console.WriteLine("🔬 Modulo IRIS encontrado e inventariado.");
                explorador.Inventario.Add(new Objeto("🔬 Analizador IRIS", TipoObjeto.Analizador, 0));
                mapa[nf, nc] = '.';
            }
            else if (lugar == 'H') Console.WriteLine(" Base NEXUS.");
            else if (lugar == '~') Console.WriteLine("🌊 Zona acuatica.");
            else if (lugar == '^') Console.WriteLine("⛰️ Zona montañosa.");
            else if (lugar == '#') Console.WriteLine("🚫 Zona bloqueada.");
        }

        private double CalcularCartografia()
        {
            int total = 13 * 29;
            int registradas = 0;

            for (int f = 0; f < 13; f++)
            {
                for (int c = 0; c < 29; c++)
                {
                    if (zonas[f, c]) registradas++;
                }
            }
            return registradas * 100.0 / total;
        }
    }


    // =========================================================
    // 5. CLASE CONEXION NEXUS
    // =========================================================

    class ConexionNexus
    {
        private TcpListener servidor;
        private TcpClient cliente;
        private NetworkStream flujo;
        private StreamReader lector;
        private StreamWriter escritor;

        public bool ConexionActiva { get; private set; }

        public void ConectarServidor()
        {
            servidor = new TcpListener(IPAddress.Any, 5000);
            servidor.Start();

            Console.WriteLine("Servidor esperando en el puerto 5000...");
            cliente = servidor.AcceptTcpClient();

            PrepararFlujo();
            ConexionActiva = true;

            string recibido = lector.ReadLine();
            Console.WriteLine("Cliente: " + recibido);

            escritor.WriteLine("Conexion NEXUS establecida correctamente.");
        }

        public void ConectarCliente(string ip, string nombre)
        {
            cliente = new TcpClient();
            cliente.Connect(ip, 5000);

            PrepararFlujo();
            ConexionActiva = true;

            escritor.WriteLine($"Hola servidor, soy {nombre}.");
            Console.WriteLine("Servidor: " + lector.ReadLine());
        }

        private void PrepararFlujo()
        {
            flujo = cliente.GetStream();
            lector = new StreamReader(flujo);
            escritor = new StreamWriter(flujo) { AutoFlush = true };
        }

        public string EnviarMensaje(string mensaje)
        {
            if (!ConexionActiva || escritor == null)
                return "Primero debe conectarse a otro equipo.";

            escritor.WriteLine(mensaje);

            try
            {
                return lector.ReadLine();
            }
            catch
            {
                return "No se pudo recibir respuesta.";
            }
        }

        public void Cerrar()
        {
            try { escritor?.Close(); } catch { }
            try { lector?.Close(); } catch { }
            try { flujo?.Close(); } catch { }
            try { cliente?.Close(); } catch { }
            try { servidor?.Stop(); } catch { }

            ConexionActiva = false;
        }
    }


    // =========================================================
    // 6. CLASE PRINCIPAL DEL SISTEMA NEXUS
    // =========================================================

    class NexusSistema
    {
        private Cadete explorador;
        private Mapa mapa;
        private ConexionNexus conexion;
        private Mision misionActual;
        private Random rnd = new Random();

        private bool sesionActiva = true;

        public NexusSistema()
        {
            mapa = new Mapa();
            conexion = new ConexionNexus();
            misionActual = new Mision("La anomalía perdida");
        }

        public void Ejecutar()
        {
            MostrarInicio();
            CrearExplorador();

            if (explorador.Edad < 18 || explorador.Energia < 40)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nACCESO DENEGADO: El cadete no cumple con los requisitos fisicos/edad.");
                Console.ResetColor();

                Finalizar();
                return;
            }

            explorador.Inventario.Add(new Objeto("🔋 Célula de energía", TipoObjeto.Energia, 25));
            explorador.Inventario.Add(new Objeto("🛡️️ Escudo de Plasma", TipoObjeto.Escudo, 20));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nACCESO AUTORIZADO. Iniciando sesion como Cadete Explorer NEXUS...");
            Console.ResetColor();

            Pausa();

            while (sesionActiva && explorador.Salud > 0)
            {
                MostrarMenu();

                int opcion = LeerNumero("Seleccione una opcion: ", 1, 12);

                switch (opcion)
                {
                    case 1: Explorar(); break;
                    case 2: Escanear(); break;
                    case 3: MostrarEstado(); break;
                    case 4: RecuperarEnergia(); break;
                    case 5: EvaluarCondicion(); break;
                    case 6: IntentarDesconexion(); break;
                    case 7: Conectar(); break;
                    case 8: EnviarMensaje(); break;
                    case 9: mapa.Mostrar(explorador); break;
                    case 10: GestionarInventario(); break;
                    case 11: EstadoMision(); break;
                    case 12: IntentarExtraccion(); break;
                }

                if (explorador.Peligro >= 80 && explorador.Estabilidad <= 30)
                {
                    ActivarIRIS();
                }
            }

            if (explorador.Salud <= 0)
            {
                Console.WriteLine("\n💀 [SISTEMA CRÍTICO]: El Cadete ha caído en combate. MISIÓN FALLIDA.");
            }

            conexion.Cerrar();
            Finalizar();
        }

        private void CrearExplorador()
        {
            string nombre = LeerTexto("Nombre del cadete explorador: ");
            int edad = LeerNumero("Edad (1-100): ", 1, 100);

            string realidad;
            do
            {
                realidad = LeerTexto("Realidad: ");
                if (!realidad.Equals("Neo Cochabamba", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Realidad no autorizada.");
                }
            } while (!realidad.Equals("Neo Cochabamba", StringComparison.OrdinalIgnoreCase));

            int energia = LeerNumero("Energia inicial (0-100): ", 0, 100);
            int estabilidad = LeerNumero("Estabilidad inicial (0-100): ", 0, 100);

            explorador = new Cadete(nombre, edad, realidad, energia, estabilidad);
        }

        private void Explorar()
        {
            Titulo("EXPLORACION DE LA REALIDAD");

            if (!explorador.PuedeExplorar())
            {
                Console.WriteLine("Energia insuficiente para explorar.");
                Pausa();
                return;
            }

            if (explorador.Peligro >= 70)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ADVERTENCIA: nivel de peligro alto.");
                Console.ResetColor();

                if (LeerTexto("¿Continuar? (SI/NO): ").ToUpper() != "SI")
                {
                    Console.WriteLine("Exploracion cancelada.");
                    Pausa();
                    return;
                }
            }

            MostrarProgreso("Sincronizando con la realidad");
            explorador.Explorar();

            int evento = rnd.Next(1, 6);
            switch (evento)
            {
                case 1:
                    Console.WriteLine("🎁 OBJETO ENCONTRADO: ¡Has hallado una Célula de energía superior!");
                    explorador.Inventario.Add(new Objeto("🔋 Célula de energía+", TipoObjeto.Energia, 35));
                    break;
                case 2:
                    Console.WriteLine("👾 ENTIDAD DETECTADA EN LAS CERCANÍAS");
                    GenerarEncuentro();
                    break;
                case 3:
                    Console.WriteLine("⚡ ANOMALÍA DETECTADA: Radiación desconocida presente.");
                    Console.WriteLine("🔋 PÉRDIDA DE ENERGÍA: -10 adicionales.");
                    explorador.ConsumirEnergia(10);
                    misionActual.ZonaLlegada = true;
                    break;
                case 4:
                    Console.WriteLine("🚨 SISTEMA DAÑADO: Un fallo de fase desgasta tu blindaje.");
                    explorador.RecibirDaño(15);
                    break;
                case 5:
                    Console.WriteLine("💎 Hito alcanzado: ¡Has hallado el Núcleo Energético!");
                    misionActual.NucleoEncontrado = true;
                    explorador.Inventario.Add(new Objeto("💎 Núcleo Energético", TipoObjeto.Energia, 50));
                    break;
            }

            EstadoPeligro();
            Pausa();
        }

        private void GenerarEncuentro()
        {
            Personaje entidad = rnd.Next(0, 2) == 0
                ? new Robot("Sentry Bot Mk-II", 40, 100)
                : new Enemigo("Xenomorfo de las Sombras", 50, 50, 12);

            Console.WriteLine($"\n⚠️ Encontraste una entidad: {entidad.Nombre}");
            bool enEncuentro = true;

            while (enEncuentro && explorador.Salud > 0 && entidad.Salud > 0)
            {
                Console.WriteLine("\n--- SISTEMA DE COMBATE ---");
                Console.WriteLine("1. Atacar");
                Console.WriteLine("2. Escapar");
                Console.WriteLine("3. Utilizar un objeto");
                Console.WriteLine("4. Analizar entidad");
                Console.Write("Selección: ");

                string opt = Console.ReadLine();
                switch (opt)
                {
                    case "1":
                        int dano = 15 + explorador.ModuloAtaque;
                        entidad.Salud -= dano;
                        Console.WriteLine($"⚔️ Atacas a {entidad.Nombre} causando {dano} de daño.");

                        if (entidad.Salud <= 0)
                        {
                            Console.WriteLine($"🎯 Has derrotado a {entidad.Nombre}.");
                            misionActual.EntidadSobrevivida = true;
                            enEncuentro = false;
                        }
                        else
                        {
                            TurnoEntidad(entidad);
                        }
                        break;

                    case "2":
                        if (rnd.Next(0, 2) == 0)
                        {
                            Console.WriteLine("🏃 Lograste escapar exitosamente.");
                            enEncuentro = false;
                        }
                        else
                        {
                            Console.WriteLine("❌ Escape fallido. La entidad bloquea tu salida.");
                            TurnoEntidad(entidad);
                        }
                        break;

                    case "3":
                        GestionarInventario();
                        break;

                    case "4":
                        entidad.Analizar();
                        break;

                    default:
                        Console.WriteLine("⚠️ Opcion invalida. La entidad aprovecha tu duda.");
                        TurnoEntidad(entidad);
                        break;
                }
            }
        }

        private void TurnoEntidad(Personaje entidad)
        {
            if (entidad is Robot robot)
            {
                if (!robot.DefensivoActivo) robot.ActivarDefensa();
                else Console.WriteLine($"🤖 {robot.Nombre} reasigna escudos.");
            }
            else if (entidad is Enemigo enemigo)
            {
                Console.WriteLine($"👾 {enemigo.Nombre} te ataca e inflige {enemigo.FuerzaAtaque} de daño!");
                explorador.RecibirDaño(enemigo.FuerzaAtaque);
            }
        }

        private void GestionarInventario()
        {
            Titulo("INVENTARIO DE OBJETOS");

            if (explorador.Inventario.Count == 0)
            {
                Console.WriteLine("Tu inventario está completamente vacío.");
                Pausa();
                return;
            }

            for (int i = 0; i < explorador.Inventario.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {explorador.Inventario[i].Nombre} (Valor: +{explorador.Inventario[i].Valor})");
            }
            Console.WriteLine($"{explorador.Inventario.Count + 1}. Salir");

            int sel = LeerNumero("Seleccione objeto (o salir): ", 1, explorador.Inventario.Count + 1);

            if (sel <= explorador.Inventario.Count)
            {
                Objeto obj = explorador.Inventario[sel - 1];
                int acc = LeerNumero($"Objeto [{obj.Nombre}]: 1. Usar | 2. Eliminar: ", 1, 2);

                if (acc == 1)
                {
                    AplicarObjeto(obj);
                    explorador.Inventario.RemoveAt(sel - 1);
                }
                else
                {
                    explorador.Inventario.RemoveAt(sel - 1);
                    Console.WriteLine($"🗑️ Objeto {obj.Nombre} descartado.");
                }
            }
            Pausa();
        }

        private void AplicarObjeto(Objeto obj)
        {
            switch (obj.Tipo)
            {
                case TipoObjeto.Energia:
                    int prevE = explorador.Energia;
                    explorador.RecuperarEnergia();
                    explorador.Energia = Math.Min(100, explorador.Energia + obj.Valor);
                    Console.WriteLine($"🔋 Energia: {prevE} → {explorador.Energia}");
                    break;
                case TipoObjeto.Escudo:
                    explorador.Escudo += obj.Valor;
                    Console.WriteLine($"🛡️ Escudo incrementado a: {explorador.Escudo}");
                    break;
                case TipoObjeto.ModuloAtaque:
                    explorador.ModuloAtaque += obj.Valor;
                    Console.WriteLine($"🔫 Módulo equipado (+{obj.Valor} de daño base).");
                    break;
                case TipoObjeto.Analizador:
                    explorador.ObtenerDatoIris();
                    Console.WriteLine("🔬 Analizador desplegado: Datos IRIS incrementados.");
                    break;
            }
        }

        private void Escanear()
        {
            Titulo("ESCANEO DE ANOMALIAS");

            if (explorador.Peligro < 30) Console.WriteLine("ESCANEO SEGURO");
            else if (explorador.Peligro < 70) Console.WriteLine("ESCANEO INESTABLE");
            else Console.WriteLine("ESCANEO PELIGROSO");

            if (explorador.Energia < 10)
            {
                Console.WriteLine("Energia insuficiente para escanear.");
                Pausa();
                return;
            }

            MostrarProgreso("Analizando frecuencia IRIS");
            explorador.ConsumirEnergia(10);
            explorador.ObtenerDatoIris();

            Console.WriteLine($"Datos IRIS obtenidos: {explorador.DatosIris}");
            Pausa();
        }

        private void MostrarEstado()
        {
            Titulo("ESTADO DEL EXPLORADOR");

            Console.WriteLine($"Cadete: {explorador.Nombre} | Edad: {explorador.Edad} | Realidad: {explorador.Realidad}");
            Console.WriteLine($"Salud: {explorador.Salud}% | Escudo: {explorador.Escudo} | XP: {explorador.XP}");

            MostrarBarra("Energia", explorador.Energia);
            MostrarBarra("Estabilidad", explorador.Estabilidad);
            MostrarBarra("Peligro", explorador.Peligro);

            Console.WriteLine($"Datos IRIS: {explorador.DatosIris}");

            if (explorador.Estabilidad >= 70) Console.WriteLine("Estado: REALIDAD ESTABLE");
            else if (explorador.Estabilidad >= 40) Console.WriteLine("Estado: ESTABILIDAD INESTABLE");
            else Console.WriteLine("Estado: REALIDAD PELIGROSA");

            EstadoPeligro();
            Pausa();
        }

        private void RecuperarEnergia()
        {
            Titulo("RECUPERACION DE ENERGIA");
            int recuperacion = explorador.RecuperarEnergia();
            Console.WriteLine($"Energia recuperada: +{recuperacion}");
            Console.WriteLine($"Energia actual: {explorador.Energia}");
            Pausa();
        }

        private void EvaluarCondicion()
        {
            Titulo("EVALUACION DE SEGURIDAD");
            if (explorador.CondicionSegura())
                Console.WriteLine("CONDICION SEGURA: se puede continuar.");
            else
                Console.WriteLine("CONDICION NO SEGURA: se recomienda precaucion.");

            Pausa();
        }

        private void EstadoMision()
        {
            Titulo("ESTADO DE MISIÓN");
            Console.WriteLine($"📜 Misión: \"{misionActual.Nombre}\"");
            Console.WriteLine($" [{(misionActual.ZonaLlegada ? "X" : " ")}] Llegar a la zona de anomalía");
            Console.WriteLine($" [{(misionActual.NucleoEncontrado ? "X" : " ")}] Encontrar el núcleo energético");
            Console.WriteLine($" [{(misionActual.EntidadSobrevivida ? "X" : " ")}] Sobrevivir al encuentro con la entidad");
            Console.WriteLine($" [{(misionActual.ExtraccionCompletada ? "X" : " ")}] Regresar al punto de extracción");
            Pausa();
        }

        private void IntentarExtraccion()
        {
            Titulo("PUNTO DE EXTRACCIÓN");
            if (misionActual.ZonaLlegada && misionActual.NucleoEncontrado && misionActual.EntidadSobrevivida)
            {
                misionActual.ExtraccionCompletada = true;
                explorador.XP += 100;

                Console.WriteLine("==========================================");
                Console.WriteLine("🟢 MISIÓN COMPLETADA");
                Console.WriteLine("+100 XP");
                Console.WriteLine("+1 Módulo desconocido");
                Console.WriteLine("==========================================");
                Console.WriteLine("¡El Cadete NEXUS ha regresado con vida!");
                sesionActiva = false;
            }
            else
            {
                Console.WriteLine("⚠️ Aún faltan objetivos por cumplir antes de solicitar la extracción.");
            }
            Pausa();
        }

        private void IntentarDesconexion()
        {
            Titulo("DESCONEXION");
            if (LeerTexto("¿Desea desconectarse? (SI/NO): ").ToUpper() == "SI")
            {
                sesionActiva = false;
                Console.WriteLine("Desconexion realizada.");
            }
            else
            {
                Console.WriteLine("La sesion continua.");
            }
            Pausa();
        }

        private void ActivarIRIS()
        {
            Titulo("ANOMALIA IRIS");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("PELIGRO CRITICO DETECTADO. Peligro >= 80 y estabilidad <= 30.");
            Console.ResetColor();

            MostrarProgreso("Protocolo IRIS activado");
            sesionActiva = false;
        }

        private void Conectar()
        {
            Titulo("CONEXION ENTRE EQUIPOS");
            Console.WriteLine("1. Servidor");
            Console.WriteLine("2. Cliente");

            int opcion = LeerNumero("Seleccione: ", 1, 2);

            try
            {
                if (opcion == 1) conexion.ConectarServidor();
                else
                {
                    string ip = LeerTexto("IP del servidor: ");
                    conexion.ConectarCliente(ip, explorador.Nombre);
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("No fue posible establecer la conexion.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Pausa();
        }

        private void EnviarMensaje()
        {
            Titulo("MENSAJERIA");
            if (!conexion.ConexionActiva)
            {
                Console.WriteLine("Primero debe conectarse a otro equipo.");
                Pausa();
                return;
            }

            string mensaje = LeerTexto("Mensaje: ");
            string respuesta = conexion.EnviarMensaje(mensaje);

            Console.WriteLine("Respuesta: " + respuesta);
            Pausa();
        }

        private void MostrarMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==========================================");
            Console.WriteLine($"     NEXUS SYSTEM v1.0 - Cadete {explorador.Nombre}");
            Console.WriteLine("==========================================");
            Console.ResetColor();

            MostrarBarra("Salud", explorador.Salud);
            MostrarBarra("Energia", explorador.Energia);
            MostrarBarra("Estabilidad", explorador.Estabilidad);
            MostrarBarra("Peligro", explorador.Peligro);

            Console.WriteLine("\n--- ACCIONES PRINCIPALES ---");
            Console.WriteLine("1. Explorar realidad");
            Console.WriteLine("2. Analizar / escanear anomalia");
            Console.WriteLine("3. Consultar estado");
            Console.WriteLine("4. Recuperar energia");
            Console.WriteLine("5. Evaluar condicion segura");
            Console.WriteLine("6. Intentar desconexion");
            Console.WriteLine("7. Conectar con otro equipo");
            Console.WriteLine("8. Enviar mensaje");
            Console.WriteLine("9. Mapa / matriz (2D)");
            Console.WriteLine("10. Gestionar Inventario");
            Console.WriteLine("11. Ver Misión y Objetivos");
            Console.WriteLine("12. Regresar al punto de extracción");
            Console.WriteLine();
        }

        private void MostrarBarra(string nombre, int valor)
        {
            int llenas = valor / 5;
            Console.Write($"{nombre,-12} [");

            Console.ForegroundColor = valor < 30 ? ConsoleColor.Red : valor < 60 ? ConsoleColor.Yellow : ConsoleColor.Green;
            Console.Write(new string('█', llenas));
            Console.ResetColor();

            Console.Write(new string('░', 20 - llenas));
            Console.WriteLine($"] {valor}%");
        }

        private void MostrarProgreso(string texto)
        {
            Console.Write(texto + " ");
            for (int i = 0; i < 20; i++)
            {
                Console.Write("█");
                Thread.Sleep(20);
            }
            Console.WriteLine(" OK");
        }

        private void EstadoPeligro()
        {
            Console.WriteLine(explorador.Peligro < 30 ? "Nivel: SEGURO" : explorador.Peligro < 70 ? "Nivel: INESTABLE" : "Nivel: PELIGROSO");
        }

        private string LeerTexto(string mensaje)
        {
            string valor;
            do
            {
                Console.Write(mensaje);
                valor = Console.ReadLine()?.Trim() ?? "";
            } while (valor == "");
            return valor;
        }

        private int LeerNumero(string mensaje, int minimo, int maximo)
        {
            int numero;
            while (true)
            {
                Console.Write(mensaje);
                if (int.TryParse(Console.ReadLine(), out numero) && numero >= minimo && numero <= maximo)
                {
                    return numero;
                }
                Console.WriteLine($"⚠️ [ERROR]: Ingrese un numero valido entre {minimo} y {maximo}.");
            }
        }

        private void MostrarInicio()
        {
            Console.Title = "NEXUS Training System";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==========================================");
            Console.WriteLine("          NEXUS TRAINING SYSTEM");
            Console.WriteLine("==========================================");
            Console.ResetColor();
        }

        private void Titulo(string texto)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==========================================");
            Console.WriteLine(" " + texto);
            Console.WriteLine("==========================================");
            Console.ResetColor();
        }

        private void Pausa()
        {
            Console.WriteLine("\nPresione ENTER para continuar...");
            Console.ReadLine();
        }

        private void Finalizar()
        {
            Console.WriteLine("\nNEXUS finalizado.");
        }
    }
