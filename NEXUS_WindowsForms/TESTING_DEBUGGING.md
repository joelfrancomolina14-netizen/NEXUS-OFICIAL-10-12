# Registro de Software Testing & Debugging - NEXUS

## 1. Prueba de entradas inválidas
- **Entrada:** Nombre vacío, edad = 15, energía = 20, realidad = "Tierra 616".
- **Resultado esperado:** Bloqueo de acceso con alerta clara sin caída del sistema.
- **Estado:** APROBADO.

## 2. Prueba de guardado y recuperación de partidas
- **Acción:** Exploración de sector, escaneo IRIS, cierre total de NEXUS y reapertura con el mismo cadete.
- **Resultado esperado:** Persistencia intacta en `Cadetes` e `HistorialAcciones`.
- **Estado:** APROBADO.

## 3. Prueba de fallo de conexión con SQL Server
- **Simulación:** Cambio temporal de cadena de conexión a base de datos inexistente.
- **Defecto detectado:** El fallo en `RegistrarHistorial` ocurría de forma silenciosa para el usuario.
- **Corrección:** Se agregó notificación visual `[ADVERTENCIA SQL]` en la consola de `FormJuego.cs` cuando `ConexionSQL` detecta fallo de servidor.
- **Estado:** CORREGIDO Y VERIFICADO.