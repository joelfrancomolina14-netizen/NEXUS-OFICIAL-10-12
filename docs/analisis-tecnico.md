# Análisis Técnico y Evaluación de Alternativas (NEXUS)

## 1. Evaluación de Alternativas Tecnológicas

### A. Interfaz Gráfica: Windows Forms vs. Aplicación de Consola vs. WPF
- **Consola (H3):** 
  - *Ventajas:* Simple de implementar.
  - *Desventajas:* Experiencia de usuario pobre, imposibilidad de mostrar gráficos interactivos o mapas visuales de forma amigable.
- **WPF (Windows Presentation Foundation):**
  - *Ventajas:* Soporta animaciones avanzadas y enlace de datos (XAML/MVVM) muy potente.
  - *Desventajas:* Curva de aprendizaje más elevada y mayor complejidad de configuración inicial para el tiempo disponible.
- **Windows Forms (Elección Seleccionada):**
  - *Ventajas:* Integración nativa rápida con C#, diseño visual mediante arrastrar y soltar (WYSIWYG), controles estándar perfectos para una grilla 2D (Panels, DataGrid, ProgressBars) y compatibilidad directa con las clases del Hito 3.
  - *Desventajas:* Menor rendimiento para gráficos 3D o animaciones complejas (no requeridas para este MVP).


### B. Persistencia de Datos: Archivos Locales (JSON/TXT) vs. SQL Server
- **Archivos Locales (JSON/Archivos de texto):**
  - *Ventajas:* No requiere servidor ni motor de base de datos instalado.
  - *Desventajas:* Vulnerable a corrupción de archivos, falta de integridad referencial entre entidades (Jugador, Explorador, Inventario) y difícil escalabilidad.
- **SQL Server (Elección Seleccionada):**
  - *Ventajas:* Garantiza integridad de los datos mediante llaves primarias y foráneas, permite consultas complejas para reportes, proporciona transacciones seguras de guardado/carga mediante ADO.NET (`SqlConnection`) y cumple estrictamente los requisitos del proyecto.


## 2. Justificación de Decisiones
- **Reutilización del Código H3:** La separación en arquitectura de 3 capas permite migrar las clases `Explorador`, `Mision` y `Mapa` a la Capa de Lógica de Negocio (BLL) sin tener que reescribir las reglas del juego.
- **Mantenibilidad y Escalabilidad:** El uso del patrón Repository en la Capa de Datos (DAL) aísla el SQL Server del resto del sistema, permitiendo cambiar las consultas o modificar tablas sin romper la interfaz gráfica.


## 3. Fuentes Técnicas y Verificación
- **Documentación Oficial de Microsoft C# / Windows Forms:** Guías de arquitectura WinForms y ciclo de vida de formularios.
- **Microsoft Docs (ADO.NET & SQL Server):** Patrones de conexión recomendados mediante `SqlConnection`, `SqlCommand` y manejo seguro de parámetros SQL.
- **Validación y Verificación con IA:** Se utilizó Inteligencia Artificial como herramienta de apoyo conceptual para estructurar los diagramas en Markdown y validar que la migración desde la consola hacia WinForms respetara el paradigma de Programación Orientada a Objetos (POO) del proyecto original.
