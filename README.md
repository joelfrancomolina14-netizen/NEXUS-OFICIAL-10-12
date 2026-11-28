# 🌌 NEXUS — Protocolo de Contención Multiversal

Bienvenido al repositorio oficial de **NEXUS**, un videojuego de acción táctica, rescate y gestión de recursos en 2D con temática sci-fi desarrollado en **C# (.NET Windows Forms)**, motor de audio **NAudio** y persistencia relacional en **SQL Server**.

---

## 📌 Propósito del Proyecto
En **NEXUS**, el jugador asume el rol de un **Cadete Operativo** asistido por la **IA Táctica IRIS** para contener una brecha dimensional en Neo Cochabamba. A través de incursiones en laberintos orbitales, el cadete debe neutralizar entidades hostiles, rescatar cápsulas criogénicas y administrar la energía y estabilidad de su traje táctico.

El proyecto evoluciona desde el prototipo inicial de consola (`NEXUSPOO`) hacia una aplicación de escritorio completa (`NEXUS_WindowsForms`) con interfaz gráfica HUD personalizada, renderizado a 60 FPS, efectos de sonido simultáneos y sincronización en tiempo real con base de datos.

---

## 🖥️ Pantallas y Funcionalidades Principales

1. **Cinemática de Arranque (`FrmIntro`):** Secuencia de inicialización de sistemas con narrativa progresiva, control de audio y verificación asíncrona de enlace con SQL Server.
2. **Terminal de Identificación (`FrmLogin`):** Registro biométrico y carga automática de perfil desde `NexusDB` (restaura rango, experiencia XP, nivel desbloqueado e inventario si el alias ya existe).
3. **Puente de Mando Táctico (`FrmPuenteMando`):**
   - Telemetría en vivo de **Salud**, **Escudo de Plasma**, **Energía del Traje**, **Estabilidad Neural** e **Índice de Peligro**.
   - Recarga en Bahía Médica y equipamiento de módulos desde el inventario.
   - Selección progresiva de **3 sectores de misión** con dificultad escalonada.
4. **Arena-Laberinto de Acción (`FrmLaberinto`):**
   - **Sector 01 — Estación Orbital Kepler:** Drones *Sentry Bot* y 3 cápsulas de rescate.
   - **Sector 02 — Núcleo Biológico Xeno:** Xenomorfos reforzados, drones y 4 cápsulas bajo amenaza biológica.
   - **Sector 03 — Abismo Anomalía Omega:** Escolta de élite, 5 cápsulas y combate final contra el jefe **Anomalía Omega Prime**.
5. **Archivo Central SQL (`FrmArchivoSQL`):** Visor integrado de telemetría para consultar, filtrar y exportar a CSV las 5 tablas de la base de datos.

---

## 🎮 Controles en Misión
- **Movimiento:** `W`, `A`, `S`, `D` o `Flechas de dirección`
- **Disparo Láser:** `Barra Espaciadora` o `Clic Izquierdo` (apuntando con el cursor)

---

## 🛠️ Tecnologías y Arquitectura
- **Lenguaje y Framework:** C# / .NET (`Windows Forms`)
- **Motor de Audio:** `NAudio` (música ambiental en bucle continuo y efectos simultáneos)
- **Base de Datos:** SQL Server (`.\SQLEXPRESS`) mediante `Microsoft.Data.SqlClient` (ADO.NET)
- **Programación Orientada a Objetos (`ModelosNexus.cs`):** Herencia y polimorfismo (`Entidad` → `Cadete`, `Robot`, `Enemigo`, `JefeOmega`) e inventario dinámico (`Objeto`).
- **Modelo Relacional (`NexusDB`):**
  - `Cadetes`: Perfil, rango militar, XP, estadísticas vitales y progreso.
  - `InventarioCadete`: Módulos de ataque, escudos y células de energía.
  - `BitacoraMisiones`: Registro de incursiones completadas o fallidas por sector.
  - `HistorialAcciones`: Auditoría de eventos del jugador en tiempo real.
  - `EstadisticasCombate`: Conteo de bajas y daño infligido por tipo de enemigo.

---

## 📂 Estructura de la Documentación

La investigación técnica y el diseño del sistema se encuentran organizados en la carpeta `docs/`:

- 📄 [`docs/diseno-del-juego.md`](./docs/diseno-del-juego.md): Visión, Game Loop, mecánicas, bocetos de pantallas y alcance.
- 📄 [`docs/arquitectura-actual.md`](./docs/arquitectura-actual.md): Análisis del código base C# desarrollado en el prototipo inicial.
- 📄 [`docs/arquitectura-propuesta.md`](./docs/arquitectura-propuesta.md): Diseño por capas (Presentación, Modelos/Lógica y Acceso a Datos).
- 📄 [`docs/modelo-de-datos-propuesto.md`](./docs/modelo-de-datos-propuesto.md): Entidades y tablas relacionales para SQL Server.
- 📄 [`docs/analisis-tecnico.md`](./docs/analisis-tecnico.md): Evaluación de alternativas tecnológicas y justificación técnica.

---

## ⚙️ Cómo Ejecutar el Proyecto

### Requisitos Previos
- **Visual Studio 2022 o superior** (con la carga de trabajo *Desarrollo de escritorio de .NET*).
- **SQL Server Express** (instancia local `.\SQLEXPRESS`) y **SQL Server Management Studio (SSMS)**.

### Pasos de Instalación
1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/joelfrancomolina14-netizen/NEXUS-OFICIAL-10-12.git
   ```
2. **Crear la base de datos en SQL Server:**
   - Abre **SSMS** y conéctate a `.\SQLEXPRESS`.
   - Abre y ejecuta el archivo `NEXUS_WindowsForms/NexusDB_Script.sql` para generar la base de datos `NexusDB` y sus 5 tablas.
3. **Abrir y compilar en Visual Studio:**
   - Abre la solución `NEXUSPOO.slnx` (o `.sln`) en la raíz del repositorio.
   - En el **Explorador de soluciones**, haz clic derecho sobre **`NEXUS_WindowsForms`** y selecciona **Establecer como proyecto de inicio**.
   - Presiona **`Ctrl + Shift + B`** para restaurar automáticamente los paquetes NuGet (`NAudio` y `Microsoft.Data.SqlClient`) y compilar.
   - Presiona **`F5`** para iniciar **NEXUS**.
