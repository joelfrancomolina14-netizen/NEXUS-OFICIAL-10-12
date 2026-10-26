# 🌌 NEXUS — Proyecto de Exploración Espacial y Estrategia 2D

Bienvenido al repositorio oficial de **NEXUS**, un videojuego de estrategia, gestión de recursos y exploración en 2D desarrollado en **C#**.

---

## 📌 Propósito del Juego
En **NEXUS**, el jugador asume el rol de Comandante de una flota de exploradores encargados de mapear sectores desconocidos, gestionar energía limitada y completar misiones tácticas. El proyecto representa la evolución de un prototipo funcional de consola (Hito 3) hacia una aplicación de escritorio completa con interfaz gráfica y persistencia relacional.

---

## 🚀 Funcionalidades Principales
- **Gestión de Exploradores:** Control de atributos clave como Salud, Energía Máxima/Actual, Nivel y Experiencia (XP).
- **Exploración en Grilla:** Movimiento por mapa interactivo con consumo progresivo de energía por casilla.
- **Sistema de Misiones:** Selección y cumplimiento de objetivos de recolección y enfrentamiento con recompensas en XP e ítems.
- **Inventario Limitado:** Almacenamiento dinámico de recursos recolectados durante la expedición.
- **Persistencia de Datos:** Guardado y carga del progreso del usuario conectado a base de datos relacional.

---

## 🛠️ Tecnologías Utilizadas
- **Lenguaje:** C# (.NET Core / .NET Framework)
- **Interfaz Gráfica:** Windows Forms (WinForms)
- **Persistencia de Datos:** SQL Server + ADO.NET (`SqlConnection`, `SqlCommand`)
- **Arquitectura:** Diseño en 3 Capas (Presentación, Lógica de Negocio y Acceso a Datos)

---

## 📂 Estructura de la Documentación

La investigación técnica y el diseño del videojuego se encuentran organizados dentro de la carpeta `docs/`:

- 📄 [`docs/diseno-del-juego.md`](./docs/diseno-del-juego.md): Visión, Game Loop, mecánicas, bocetos de pantallas y alcance.
- 📄 [`docs/arquitectura-actual.md`](./docs/arquitectura-actual.md): Análisis del código base C# desarrollado en Hito 3.
- 📄 [`docs/arquitectura-propuesta.md`](./docs/arquitectura-propuesta.md): Propuesta de diseño en 3 capas (UI, BLL, DAL) y diagrama.
- 📄 [`docs/modelo-de-datos-propuesto.md`](./docs/modelo-de-datos-propuesto.md): Entidades y tablas relacionales para SQL Server.
- 📄 [`docs/analisis-tecnico.md`](./docs/analisis-tecnico.md): Evaluación de alternativas tecnológicas y justificación técnica.

---

## ⚙️ Cómo Ejecutar el Proyecto

1. **Clonar el repositorio:**
   ```bash
   git clone [https://github.com/joelfrancomolina14-netizen/NEXUS-OFICIAL-10-12.git](https://github.com/joelfrancomolina14-netizen/NEXUS-OFICIAL-10-12.git)
