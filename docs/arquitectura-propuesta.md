# Arquitectura Propuesta (NEXUS - Versión Gráfica)

## 1. Diseño de Arquitectura en Capas (N-Capas)
Para la evolución del proyecto NEXUS se propone un patrón de arquitectura en tres capas. Esto permite desacoplar la interfaz de usuario de la lógica de negocio y aislar el acceso a datos.

+-------------------------------------------------------------+
|               Capa de Presentación (UI)                     |
|  - FormMenuPrincipal        - FormMapa                      |
|  - FormInventario           - FormMisiones                  |
+-------------------------------------------------------------+
                              │
                              ▼
+-------------------------------------------------------------+
|            Capa de Lógica de Negocio (BLL)                  |
|  - ExploradorService        - MapManager                    |
|  - MisionManager            - GameController                |
+-------------------------------------------------------------+
                              │
                              ▼
+-------------------------------------------------------------+
|             Capa de Acceso a Datos (DAL)                    |
|  - ConexionBD (SqlConnection)                               |
|  - ExploradorRepository     - InventarioRepository          |
|  - MisionRepository                                         |
+-------------------------------------------------------------+
                              │
                              ▼
+-------------------------------------------------------------+
|              Base de Datos Relacional                       |
|  - SQL Server (Tablas: Jugadores, Exploradores, etc.)       |
+-------------------------------------------------------------+

## 2. Responsabilidad de cada Capa
Capa de Presentación (UI - Windows Forms)
Captura los eventos del usuario (clics en botones, teclas de dirección).

Renderiza el mapa en un panel gráfico de WinForms.

Muestra el estado del explorador (Salud, Energía, XP) mediante ProgressBar y controles visuales.

Capa de Lógica de Negocio (BLL - Business Logic Layer)
Integra y reutiliza el código fuente del Hito 3.

Valida las reglas de juego: movimiento en grilla, cálculo de consumo de energía por casilla, aumento de experiencia al completar misiones y derrota cuando la energía llega a 0.

No contiene ninguna referencia a controles de WinForms ni salidas de Console.

Capa de Acceso a Datos (DAL - Data Access Layer)
Gestiona la comunicación con SQL Server mediante ADO.NET (SqlConnection, SqlCommand, SqlDataReader).

Implementa el patrón Repository para operaciones CRUD (Crear, Leer, Actualizar, Eliminar) del progreso del jugador.


## 3. Beneficios de esta Arquitectura
Mantenibilidad: Si se modifica la interfaz visual, la lógica del juego y los datos no se ven afectados.

Reutilización: Se aprovechan las clases base creadas en H3 sin necesidad de reescribirlas desde cero.

Testabilidad: Permite probar las reglas de juego de forma independiente a la pantalla.

