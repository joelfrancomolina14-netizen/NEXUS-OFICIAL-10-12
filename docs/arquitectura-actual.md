# Arquitectura Actual (NEXUS - Hito 3)

## 1. Descripción del Estado Actual
El proyecto NEXUS actual (Hito 3) es una aplicación de consola desarrollada en C# bajo el paradigma de Programación Orientada a Objetos (POO). La ejecución del flujo de juego se realiza de forma secuencial y sincrónica mediante menús de texto.


## 2. Clases Existentes y Sus Responsabilidades
- **`Explorador`**: Representa a la entidad jugable. Almacena atributos clave como Nombre, Salud, Energía Actual, Energía Máxima, Nivel y Experiencia (XP).
- **`Mapa` / `Tablero`**: Maneja la representación matricial (arreglos o listas bidimensionales) del sector explorado. Controla la posición actual del jugador y las casillas visitadas.
- **`Mision`**: Define los objetivos de recolección o exploración, almacenando recompensas en XP e ítems, así como el estado de completitud.
- **`Inventario` / `Item`**: Gestiona los recursos recolectados por el explorador durante las expediciones.
- **`Program` / `Juego`**: Contiene el punto de entrada (`Main`), el bucle principal de juego y la lógica de interacción a través de `Console.ReadLine()` y `Console.WriteLine()`.


## 3. Limitaciones Identificadas
- **Acoplamiento de la Interfaz:** La lógica de negocio está fuertemente acoplada a la salida por consola (`Console.WriteLine`), lo que dificulta reutilizar el código en una interfaz gráfica.
- **Persistencia Temporal:** El estado del juego se mantiene únicamente en memoria RAM durante la ejecución de la aplicación, perdiendo todo el progreso al cerrar la consola.
- **Escalabilidad Reducida:** No existe una separación clara de responsabilidades en capas, complicando la integración de servicios externos como bases de datos relacionales.
- **Experiencia de Usuario (UX) Limitada:** La navegación basada en entradas numéricas de consola resulta poco intuitiva comparada con un mapa visual interactivo.
