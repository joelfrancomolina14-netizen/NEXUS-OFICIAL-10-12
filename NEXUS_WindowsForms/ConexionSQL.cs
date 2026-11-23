using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace NEXUS_WindowsForms
{
    public static class ConexionSQL
    {
        public static string CadenaConexion = @"Server=.\SQLEXPRESS;Database=NexusDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static int RegistrarOGuardarCadete(Cadete c)
        {
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                con.Open();
                string query = @"
                    IF EXISTS (SELECT 1 FROM Cadetes WHERE Nombre = @Nombre)
                    BEGIN
                        UPDATE Cadetes SET 
                            Edad = @Edad, Realidad = @Realidad, Salud = @Salud, Energia = @Energia,
                            Estabilidad = @Estabilidad, Peligro = @Peligro, Escudo = @Escudo,
                            XP = @XP, ModuloAtaque = @ModuloAtaque, DatosIris = @DatosIris,
                            UltimaConexion = GETDATE()
                        WHERE Nombre = @Nombre;
                        SELECT CadeteID FROM Cadetes WHERE Nombre = @Nombre;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO Cadetes (Nombre, Edad, Realidad, Salud, Energia, Estabilidad, Peligro, Escudo, XP, ModuloAtaque, DatosIris)
                        VALUES (@Nombre, @Edad, @Realidad, @Salud, @Energia, @Estabilidad, @Peligro, @Escudo, @XP, @ModuloAtaque, @DatosIris);
                        SELECT SCOPE_IDENTITY();
                    END";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                    cmd.Parameters.AddWithValue("@Edad", c.Edad);
                    cmd.Parameters.AddWithValue("@Realidad", c.Realidad);
                    cmd.Parameters.AddWithValue("@Salud", c.Salud);
                    cmd.Parameters.AddWithValue("@Energia", c.Energia);
                    cmd.Parameters.AddWithValue("@Estabilidad", c.Estabilidad);
                    cmd.Parameters.AddWithValue("@Peligro", c.Peligro);
                    cmd.Parameters.AddWithValue("@Escudo", c.Escudo);
                    cmd.Parameters.AddWithValue("@XP", c.XP);
                    cmd.Parameters.AddWithValue("@ModuloAtaque", c.ModuloAtaque);
                    cmd.Parameters.AddWithValue("@DatosIris", c.DatosIris);

                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    c.CadeteID = id;
                    return id;
                }
            }
        }

        public static void RegistrarHistorial(Cadete c, string tipoAccion, string detalle)
        {
            if (c == null) return;
            try
            {
                RegistrarOGuardarCadete(c);
                using (SqlConnection con = new SqlConnection(CadenaConexion))
                {
                    con.Open();
                    string sql = "INSERT INTO HistorialAcciones (CadeteID, NombreCadete, TipoAccion, Detalle) VALUES (@Id, @Nombre, @Tipo, @Detalle)";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", c.CadeteID);
                        cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                        cmd.Parameters.AddWithValue("@Tipo", tipoAccion);
                        cmd.Parameters.AddWithValue("@Detalle", detalle);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public static DataTable ObtenerHistorialCompleto()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(CadenaConexion))
                {
                    con.Open();
                    string sql = @"SELECT HistorialID AS [ID], NombreCadete AS [Cadete], TipoAccion AS [Acción], 
                                   Detalle AS [Movimiento / Evento], FechaHora AS [Fecha y Hora] 
                                   FROM HistorialAcciones ORDER BY HistorialID DESC";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch { }
            return dt;
        }
    }
}