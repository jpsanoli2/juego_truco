# 🃏 Truco Uruguayo para PC

**Recreación digital del clásico juego de cartas Truco Uruguayo, desarrollada en Unity (C#) enfocada en arquitectura limpia, gestión compleja de estados y lógica de reglas de negocio.**  


---

## 📌 Showcase / Capturas de Pantalla

| Gameplay Principal | Mecánica Clave / Menú |
| :---: | :---: |
| [Video de Gameplay](https://github.com/jpsanoli2/juego_truco/blob/main/Assets/mediagh/videoTruco.mp4) | ![Captura de Pantalla](https://github.com/jpsanoli2/juego_truco/blob/main/Assets/mediagh/captura.png) |

---

📥 **[Descargar Ejecutable (.exe)](https://drive.google.com/drive/folders/1ZOi0_-7c7m3oGlcdn2emeynQiL90WGZw?usp=sharing)**


---

## 🚀 Características Principales

- Al empezar cada mano el mazo se baraja con el algoritmo Fisher-Yates.
- Las cartas se reparten de forma que cada cliente solo puede ver las cartas que le correspondan a su id.
- Con una variable de tipo Ulong llamada turno nos aseguramos de que solo el jugador que corresponda pueda hacer su jugada.
- Al repartir las cartas se asignan las muestras de la mano de forma automática (el 2, 4, 5, 10, 11 y 12 si corresponde).
- El envido se calcula de forma diferente en caso de tener flor o no tenerla.
- El valor del truco de cada carta cambia si es pieza o no lo es (también verifica si en esa mano el 12 toma el valor de una pieza).


---

## 🛠️ Arquitectura y Aspectos Técnicos

El juego es un proyecto Universal 2D, creado en Unity, programado en C#.
Es un juego MultiJugador (exclusivamente pensando para 2 jugadores (1vs1)).
Utilicé el asset oficial de Unity llamado: Unity Building Block - Multiplayer Session para que, con un sistema de códigos se puedan unir ambos
jugadores a la misma sesión. Gracias a esto y Netcode for GameObjects me permitió manejar toda la lógica del multijugador de una forma eficaz.
Implementé un modelo Host/Server Authoritative de forma que el servidor valida toda las reglas, evitando así jugadores que quieran modificar 
parte del juego para tener una ventaja competitiva.
Uso de RPCs para recibir solicitudes de parte de los clientes [Rpc(SendTo.Server)] y disparar eventos ya sea para un cliente en 
específico [Rpc(SendTo.SpecifiedInParams)], o para todos los clientes [Rpc(SendTo.ClientsAndHost)].


---

## 📂 Estructura del Proyecto

```text
Assets/
 ├── 📄 scripts/    (Todos los scripts con la lógica del juego)
 ├── 🃏 Cartas/     (Todas las cartas creadas con un ScriptableObject, con sus respectivos nombres, puntos de envido, truco, sprite, etc)
 ├── 📁 Mazo/       (Contiene el mazo de juego)
 ├── 📷 imgCartas/  (Contiene las imágenes usada en los sprites de cada carta)
 └── 🅱️ Blocks/     (Unity Building Block - Multiplayer Session)
```

## Requisitos

- Unity Hub y **Unity 6000.4.11f1**, versión registrada en [ProjectVersion.txt](ProjectSettings/ProjectVersion.txt).
- Git para clonar el repositorio.
- Los paquetes del proyecto se restauran desde la carpeta `Packages/` al abrirlo en Unity.

## Cómo abrir y ejecutar

1. Clona el repositorio:

   ```bash
   git clone https://github.com/jpsanoli2/juego_truco.git
   ```

2. En Unity Hub, selecciona **Add / Agregar proyecto desde disco** y elige la carpeta `juego_truco` que contiene `Assets/`, `Packages/` y `ProjectSettings/`.
3. Ábrelo con la versión de Unity indicada y espera a que termine la importación.
4. Abre la escena [JoinByCode.unity](Assets/Scenes/JoinByCode.unity).
5. Pulsa **Play** en el editor.

Para generar un ejecutable, añade la escena a **File > Build Profiles**, selecciona la plataforma e instala su módulo de compilación desde Unity Hub.

## Comprobación y solución de problemas

- Si hay errores de importación, comprueba la versión del editor y la restauración de paquetes antes de modificar el proyecto.
- Revisa la consola de Unity y las referencias del Inspector si faltan componentes, materiales o elementos de interfaz.
- Conserva los archivos `.meta`: mantienen las referencias entre escenas, scripts y recursos.

Las instrucciones describen la configuración del repositorio; no constituyen una prueba de ejecución o compilación en todas las plataformas.

## Prueba multijugador

El juego está diseñado para dos jugadores (1 contra 1). La escena de entrada es `JoinByCode` y la mesa se encuentra en [MesaDeJuego.unity](Assets/Scenes/MesaDeJuego.unity).

1. Comprueba que la configuración del proyecto y de Unity Gaming Services corresponde a tu entorno de desarrollo.
2. Ejecuta dos instancias del juego: una crea la sesión y la otra se une con su código.
3. Revisa el reparto de cartas, la alternancia de turnos y las reglas de envido, flor y truco.
4. Para compilar, incluye las dos escenas y mantén sus nombres: el flujo de sesión puede depender de ellos.

La conexión requiere las dependencias y servicios del sistema multijugador; abrir solamente la escena de la mesa no sustituye la creación de sesión.

## Contribuciones y recursos

Para reportar un problema, indica la versión de Unity, escena, pasos para reproducirlo y mensajes de la consola. Las capturas y enlaces de descarga de la documentación original se conservan; los recursos de terceros mantienen sus propias condiciones de uso.
