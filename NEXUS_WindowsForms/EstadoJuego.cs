namespace NEXUS_WindowsForms
{
    public static class EstadoJuego
    {
        public static Cadete? CadeteActual { get; set; }
        public static int SectorActual { get; set; } = 1;

        public static string NombreSectorActual => SectorActual switch
        {
            1 => "SECTOR 01: ESTACIÓN ORBITAL KEPLER",
            2 => "SECTOR 02: NÚCLEO BIOLÓGICO XENO",
            3 => "SECTOR 03: ABISMO ANOMALÍA OMEGA",
            _ => "SECTOR 01: ESTACIÓN ORBITAL KEPLER"
        };

        public static void ReiniciarSesion()
        {
            CadeteActual = null;
            SectorActual = 1;
        }
    }
}