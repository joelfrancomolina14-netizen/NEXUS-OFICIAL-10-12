using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace NEXUS_WindowsForms
{
    public static class ConexionSQL
    {
        public static string CadenaConexion = @"Server=.\SQLEXPRESS;Database=NexusDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static bool ProbarConexion()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CadenaConexion))
                {
                    con.Open();
                    return true;
                }
            }
            catch { return false; }
        }

        public static Cadete CargarOCrearCadete(string nombre, int edad, string realidad)
        {
            Cadete cadete = new Cadete(nombre, edad, realidad);
            try
            {
                using (SqlConnection con = new SqlConnection(CadenaConexion))
                {
                    con.Open();
                    string selectSql = "SELECT * FROM Cadetes WHERE Nombre = @Nombre";
                    using (SqlCommand cmd = new SqlCommand(selectSql, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", nombre);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                cadete.CadeteID = Convert.ToInt32(dr["CadeteID"]);
                                cadete.Edad = Convert.ToInt32(dr["Edad"]);
                                cadete.Realidad = dr["Realidad"].ToString() ?? realidad;
                                cadete.Salud = Math.Max(50, Convert.ToInt32(dr["Salud"]));
                                cadete.Energia = Math.Max(50, Convert.ToInt32(dr["Energia"]));
                                cadete.Estabilidad = Math.Max(50, Convert.ToInt32(dr["Estabilidad"]));
                                cadete.Escudo = Convert.ToInt32(dr["Escudo"]);
                                cadete.XP = Convert.ToInt32(dr["XP"]);
                                cadete.ModuloAtaque = Convert.ToInt32(dr["ModuloAtaque"]);
                                cadete.DatosIris = Convert.ToInt32(dr["DatosIris"]);
                                cadete.NivelDesbloqueado = dr["NivelDesbloqueado"] != DBNull.Value ? Convert.ToInt32(dr["NivelDesbloqueado"]) : 1;
                                cadete.TotalRescatados = dr["TotalRescatados"] != DBNull.Value ? Convert.ToInt32(dr["TotalRescatados"]) : 0;
                                cadete.ActualizarPeligro();
                            }
                        }
                    }

                    if (cadete.CadeteID > 0)
                    {
                        CargarInventario(con, cadete);
                    }
                    else
                    {
                        RegistrarOGuardarCadete(cadete);
                    }
                }
            }
            catch
            {
                // Si SQL Server no está activo, permite jugar en modo local sin bloquear
            }
            return cadete;
        }

        private static void CargarInventario(SqlConnection con, Cadete c)
        {
            string sql = "SELECT NombreObjeto, TipoObjeto, Valor FROM InventarioCadete WHERE CadeteID = @Id";
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Id", c.CadeteID);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    List<Objeto> lista = new List<Objeto>();
                    while (dr.Read())
                    {
                        if (Enum.TryParse(dr["TipoObjeto"].ToString(), out TipoObjeto tipo))
                        {
                            lista.Add(new Objeto(dr["NombreObjeto"].ToString() ?? "Módulo", tipo, Convert.ToInt32(dr["Valor"])));
                        }
                    }
                    if (lista.Count > 0) c.Inventario = lista;
                }
            }
        }

        public static int RegistrarOGuardarCadete(Cadete c)
        {
            if (c == null) return 0;
            try
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
                                NivelDesbloqueado = @Nivel, TotalRescatados = @Rescatados,
                                UltimaConexion = GETDATE()
                            WHERE Nombre = @Nombre;
                            SELECT CadeteID FROM Cadetes WHERE Nombre = @Nombre;
                        END
                        ELSE
                        BEGIN
                            INSERT INTO Cadetes (Nombre, Edad, Realidad, Salud, Energia, Estabilidad, Peligro, Escudo, XP, ModuloAtaque, DatosIris, NivelDesbloqueado, TotalRescatados)
                            VALUES (@Nombre, @Edad, @Realidad, @Salud, @Energia, @Estabilidad, @Peligro, @Escudo, @XP, @ModuloAtaque, @DatosIris, @Nivel, @Rescatados);
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
                        cmd.Parameters.AddWithValue("@Nivel", c.NivelDesbloqueado);
                        cmd.Parameters.AddWithValue("@Rescatados", c.TotalRescatados);

                        int id = Convert.ToInt32(cmd.ExecuteScalar());
                        c.CadeteID = id;
                        SincronizarInventario(con, c);
                        return id;
                    }
                }
            }
            catch { return 0; }
        }

        private static void SincronizarInventario(SqlConnection con, Cadete c)
        {
            if (c.CadeteID <= 0) return;
            using (SqlCommand del = new SqlCommand("DELETE FROM InventarioCadete WHERE CadeteID = @Id", con))
            {
                del.Parameters.AddWithValue("@Id", c.CadeteID);
                del.ExecuteNonQuery();
            }

            foreach (var item in c.Inventario)
            {
                string ins = "INSERT INTO InventarioCadete (CadeteID, NombreObjeto, TipoObjeto, Valor) VALUES (@Id, @Nom, @Tipo, @Val)";
                using (SqlCommand cmd = new SqlCommand(ins, con))
                {
                    cmd.Parameters.AddWithValue("@Id", c.CadeteID);
                    cmd.Parameters.AddWithValue("@Nom", item.Nombre);
                    cmd.Parameters.AddWithValue("@Tipo", item.Tipo.ToString());
                    cmd.Parameters.AddWithValue("@Val", item.Valor);
                    cmd.ExecuteNonQuery();
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

        public static void RegistrarMision(Cadete c, string sector, int rescatados, int puntos, string resultado)
        {
            if (c == null) return;
            try
            {
                RegistrarOGuardarCadete(c);
                using (SqlConnection con = new SqlConnection(CadenaConexion))
                {
                    con.Open();
                    string sql = @"INSERT INTO BitacoraMisiones (CadeteID, Sector, Rescatados, PuntuacionObtenida, Resultado) 
                                   VALUES (@Id, @Sector, @Resc, @Pts, @Res)";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", c.CadeteID);
                        cmd.Parameters.AddWithValue("@Sector", sector);
                        cmd.Parameters.AddWithValue("@Resc", rescatados);
                        cmd.Parameters.AddWithValue("@Pts", puntos);
                        cmd.Parameters.AddWithValue("@Res", resultado);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public static void RegistrarCombate(Cadete c, string tipoEnemigo, int danioInfligido)
        {
            if (c == null) return;
            try
            {
                if (c.CadeteID <= 0) RegistrarOGuardarCadete(c);
                using (SqlConnection con = new SqlConnection(CadenaConexion))
                {
                    con.Open();
                    int dron = tipoEnemigo == "Dron" ? 1 : 0;
                    int alien = tipoEnemigo == "Alien" ? 1 : 0;
                    int jefe = tipoEnemigo == "Jefe" ? 1 : 0;

                    string sql = @"
                        IF EXISTS (SELECT 1 FROM EstadisticasCombate WHERE CadeteID = @Id)
                        BEGIN
                            UPDATE EstadisticasCombate SET 
                                DronesEliminados = DronesEliminados + @D,
                                AliensEliminados = AliensEliminados + @A,
                                JefesDerrotados = JefesDerrotados + @J,
                                DanioTotalInfligido = DanioTotalInfligido + @Danio
                            WHERE CadeteID = @Id;
                        END
                        ELSE
                        BEGIN
                            INSERT INTO EstadisticasCombate (CadeteID, DronesEliminados, AliensEliminados, JefesDerrotados, DanioTotalInfligido)
                            VALUES (@Id, @D, @A, @J, @Danio);
                        END";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", c.CadeteID);
                        cmd.Parameters.AddWithValue("@D", dron);
                        cmd.Parameters.AddWithValue("@A", alien);
                        cmd.Parameters.AddWithValue("@J", jefe);
                        cmd.Parameters.AddWithValue("@Danio", danioInfligido);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public static DataTable ConsultarTabla(string vista, string filtro = "")
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(CadenaConexion))
                {
                    con.Open();
                    string sql = vista switch
                    {
                        "Cadetes" => @"SELECT CadeteID AS [ID], Nombre AS [Cadete], Realidad AS [Realidad Origen], 
                                       XP AS [Experiencia], NivelDesbloqueado AS [Sector Máx], TotalRescatados AS [Rescatados], 
                                       UltimaConexion AS [Última Conexión] FROM Cadetes 
                                       WHERE Nombre LIKE @Filtro OR Realidad LIKE @Filtro ORDER BY XP DESC",

                        "Misiones" => @"SELECT m.MisionID AS [ID], c.Nombre AS [Cadete], m.Sector, m.Rescatados, 
                                        m.PuntuacionObtenida AS [Puntos], m.Resultado, m.FechaRegistro AS [Fecha]
                                        FROM BitacoraMisiones m INNER JOIN Cadetes c ON m.CadeteID = c.CadeteID
                                        WHERE c.Nombre LIKE @Filtro OR m.Sector LIKE @Filtro OR m.Resultado LIKE @Filtro
                                        ORDER BY m.MisionID DESC",

                        "Combate" => @"SELECT c.Nombre AS [Cadete], e.DronesEliminados AS [Drones], 
                                       e.AliensEliminados AS [Xenomorfos], e.JefesDerrotados AS [Jefes Omega], 
                                       e.DanioTotalInfligido AS [Daño Total]
                                       FROM EstadisticasCombate e INNER JOIN Cadetes c ON e.CadeteID = c.CadeteID
                                       WHERE c.Nombre LIKE @Filtro ORDER BY e.DanioTotalInfligido DESC",

                        "Inventario" => @"SELECT i.ItemID AS [ID], c.Nombre AS [Cadete], i.NombreObjeto AS [Módulo / Objeto], 
                                          i.TipoObjeto AS [Categoría], i.Valor AS [Potencia], i.FechaObtencion AS [Adquirido]
                                          FROM InventarioCadete i INNER JOIN Cadetes c ON i.CadeteID = c.CadeteID
                                          WHERE c.Nombre LIKE @Filtro OR i.NombreObjeto LIKE @Filtro ORDER BY i.ItemID DESC",

                        _ => @"SELECT HistorialID AS [ID], NombreCadete AS [Cadete], TipoAccion AS [Acción], 
                               Detalle AS [Movimiento / Evento], FechaHora AS [Fecha y Hora] 
                               FROM HistorialAcciones 
                               WHERE NombreCadete LIKE @Filtro OR TipoAccion LIKE @Filtro OR Detalle LIKE @Filtro
                               ORDER BY HistorialID DESC"
                    };

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Filtro", $"%{filtro}%");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch { }
            return dt;
        }
    }
}