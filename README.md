# 🃏 Truco Uruguayo para PC

**Recreación digital del clásico juego de cartas Truco Uruguayo, desarrollada en Unity (C#) enfocada en arquitectura limpia, gestión compleja de estados y lógica de reglas de negocio.**  

---

## 📌 Showcase / Capturas de Pantalla

<!-- CONSEJO: Coloca un GIF corto del gameplay o un par de capturas llamativas aquí.
     Si el reclutador no ve el juego de entrada, probablemente no descargue el ejecutable. -->

| Gameplay Principal | Mecánica Clave / Menú |
| :---: | :---: |
| ![GIF de Gameplay](link-a-tu-gif-o-imagen.gif) | ![Captura de Pantalla](link-a-tu-imagen.png) |
---

📥 **[Descargar Ejecutable (.exe)](https://drive.google.com/drive/folders/1ZOi0_-7c7m3oGlcdn2emeynQiL90WGZw?usp=sharing)**

---

## 🚀 Características Principales

* **1** Al empezar cada mano el mazo se baraja con el algoritmo Fisher-Yates.
* **2** Las cartas se reparten de forma que cada cliente solo puede ver las cartas que le correspondan a su id.
* **3** Con una variable de tipo Ulong llamada turno nos aseguramos de que solo el jugador que corresponda pueda hacer su jugada.
* **4** Al repartir las cartas se asignan las muestras de la mano de forma automática (el 2, 4, 5, 10, 11 y 12 si corresponde).
* **5** El envido se calcula de forma diferente en caso de tener flor o no tenerla.
* **6** El valor del truco de cada carta cambia si es pieza o no lo es (también verifica si en esa mano el 12 toma el valor de una pieza).

---

## 🛠️ Arquitectura y Aspectos Técnicos

```text
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
 ├── 🃏 cartas/     (Todas las cartas creadas con un ScriptableObject, con sus respectivos nombres, puntos de envido, truco, sprite, etc)
 ├── 📁 mazo/       (Contiene el mazo de juego)
 ├── 📷 imgCartas/  (Contiene las imágenes usada en los sprites de cada carta)
 └── 🅱️ Blocks/     (Unity Building Block - Multiplayer Session)
