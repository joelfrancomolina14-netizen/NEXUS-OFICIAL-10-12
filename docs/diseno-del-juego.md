# Diseño del Juego

## 1. Visión del Juego
NEXUS es un videojuego de estrategia y exploración espacial/dimensional en 2D desarrollado en C# y a fuituro en Windows Forms. El jugador asume el rol de Comandante de una flota de exploradores encargados de mapear sectores desconocidos, gestionar energía limitada y completar misiones de recolección y supervivencia.

- **Experiencia ofrecida:** Una mezcla de toma de decisiones tácticas y gestión de recursos donde el peligro constante de quedar sin energía genera tensión en cada turno.
- **Acciones del jugador:** Seleccionar exploradores, gestionar el inventario, navegar mapas en grilla, enfrentar eventos y completar misiones.
- **Motivación para continuar:** La progresión del explorador (subida de nivel y acumulación de XP), la mejora de la flota y el descubrimiento de sectores más complejos con recompensas más valiosas.


## 2. Ciclo Principal de Juego 

Inicio de la partida: El jugador ingresa al menú principal, selecciona su explorador activo y escoge una misión disponible.

Fase de exploración: Se despliega el mapa en grilla. Cada movimiento hacia una casilla consume energía. En el trayecto puede encontrar eventos, recursos o enemigos.

Resolución y progresión: Al cumplir el objetivo de la misión o quedarse sin energía, finaliza la expedición. La experiencia y los ítems recolectados se transfieren al perfil del jugador y se guardan en SQL Server.

 
 ## 3. Mecánicas y Reglas
 
Exploradores: Poseen atributos dinámicos como Salud, Energía Máxima, Energía Actual, Nivel y Experiencia (XP).

Consumo de Energía: Cada desplazamiento por las casillas del mapa o acción especial consume puntos de energía. Si la energía se agota a 0 antes de completar la misión, la expedición fracasa.

Misiones: Se clasifican en Misiones de Exploración, Recolección y Enfrentamiento. Requieren un nivel mínimo de explorador para ser aceptadas y otorgan recompensas en XP e ítems.

Inventario y Capacidad: Capacidad limitada de almacenamiento por explorador para transportar minerales, núcleos de energía y artefactos hallados durante la exploración.


## 4. Diseño de Pantallas (Wireframes y Navegación)
Pantalla 1: Menú Principal
Elementos visibles: Título del juego (NEXUS), Botones: Nueva Partida, Cargar Partida, Opciones y Salir.

Navegación: Permite acceder a la selección de explorador o cargar partidas previas conectadas a la base de datos SQL Server.

Pantalla 2: Mapa de Exploración
Elementos visibles: Grilla visual interactiva (panel de casillas), barra superior con la salud y energía restante del explorador activo, e indicador de coordenadas.

Navegación: Botones direccionales o interacción con casillas para avanzar en el mapa; botón de Regresar a la Base para abortar la expedición.

Pantalla 3: Panel de Misiones
Elementos visibles: Lista (DataGrid o ListBox) de misiones disponibles, panel de detalles (objetivo, nivel requerido, recompensas) y botón Aceptar Misión.

Navegación: Permite elegir la misión activa antes de desplegar el mapa.

Pantalla 4: Gestión de Exploradores e Inventario
Elementos visibles: Lista de exploradores disponibles, barras de progreso de XP, cuadro de estadísticas y tabla interactiva con los ítems del inventario (cantidad y descripción).

Navegación: Acceso directo desde el menú principal o la pantalla de misiones para equipar o seleccionar exploradores.


## 5. Experiencia de Usuario (UX)
Legibilidad y Jerarquía Visual: Uso de contraste claro entre el fondo oscuro del espacio y los paneles de control. Los datos críticos como Salud y Energía destacan con colores llamativos (Rojo y Verde/Azul).

Consistencia: Todos los formularios comparten la misma paleta de colores, tipografía y estilo de botones.

Facilidad de Navegación: Acceso permanente a los menús clave mediante una barra de navegación superior o lateral.

Retroalimentación de Acciones: Mensajes emergentes (MessageBox) de confirmación para guardar partida, advertencias de energía baja en pantalla y alertas de victoria/derrota.


## 6. Alcance y Restricciones
Funcionalidades Prioritarias (MVP):

Interfaz basada en Windows Forms con controles nativos (Panel, DataGrid, ProgressBar).

Lógica de movimiento en grilla con consumo de energía basada en el proyecto de H3.

Guardado y carga de estado del juego conectado a SQL Server.

Funcionalidades Opcionales:

Animaciones complejas de sprites y efectos de sonido.

Restricciones Técnicas:

Mantener la compatibilidad estricta con la arquitectura de clases definida en el proyecto H3.
