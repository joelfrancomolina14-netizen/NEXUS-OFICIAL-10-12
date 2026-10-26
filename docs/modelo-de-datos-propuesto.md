# Modelo de Datos Propuesto (SQL Server)

## 1. Estrategia de Persistencia
El progreso del jugador y el estado del juego se almacenarán de forma permanente en un sistema de gestión de bases de datos relacionales (**SQL Server**). Se utilizará la librería **ADO.NET** en C# para ejecutar sentencias SQL y Stored Procedures desde la Capa de Acceso a Datos (DAL).

---

## 2. Diagrama de Entidad-Relación (Conceptual)

```text
[ JUGADORES ] 1 ─── N [ EXPLORADORES ] 1 ─── N [ INVENTARIO ]
      │                         │
      │ 1                       │ 1
      ▼ N                       ▼ N
[ PARTIDAS ]               [ MISIONES_COMPLETADAS ]
````

## 3. Definición de Tablas (Script DDL)
Tabla: Jugadores
Almacena el registro de usuarios del sistema.

JugadorID (INT, Primary Key, Identity)

NombreUsuario (VARCHAR(50), NOT NULL, Unique)

FechaRegistro (DATETIME, DEFAULT GETDATE())

Tabla: Exploradores
Guarda el estado del personaje jugable del Hito 3.

ExploradorID (INT, Primary Key, Identity)

JugadorID (INT, Foreign Key -> Jugadores.JugadorID)

Nombre (VARCHAR(50), NOT NULL)

Nivel (INT, DEFAULT 1)

Experiencia (INT, DEFAULT 0)

SaludActual (INT, NOT NULL)

SaludMaxima (INT, NOT NULL)

EnergiaActual (INT, NOT NULL)

EnergiaMaxima (INT, NOT NULL)

Tabla: Inventario
Almacena los ítems recolectados por cada explorador.

InventarioID (INT, Primary Key, Identity)

ExploradorID (INT, Foreign Key -> Exploradores.ExploradorID)

NombreItem (VARCHAR(50), NOT NULL)

Cantidad (INT, DEFAULT 1)

Tipo (VARCHAR(30)) -- Recurso, Consumible, Artefacto

Tabla: Misiones
Catálogo de misiones disponibles y su estado.

MisionID (INT, Primary Key, Identity)

Titulo (VARCHAR(100), NOT NULL)

Descripcion (TEXT)

NivelRequerido (INT, DEFAULT 1)

RecompensaXP (INT, NOT NULL)

## 4. Estrategia de Guardado y Carga de Partida
Guardado (Save Game): Al finalizar un turno o al hacer clic en Guardar, se ejecuta una transacción SQL (SqlTransaction) que actualiza la SaludActual, EnergiaActual y Experiencia en la tabla Exploradores, además de sincronizar las cantidades en Inventario.

Carga (Load Game): Al iniciar el juego, mediante consultas SELECT asistidas por SqlDataReader, se instancian los objetos de las clases Explorador e Inventario restaurando el estado exacto de la partida.
