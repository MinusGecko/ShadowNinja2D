# Shadow Ninja 2D — Godot 4.7

Proyecto de plataformas 2D de acción y aventura. Versión de referencia: **Godot 4.7**, GDScript y renderizador Compatibility.

## Escenas disponibles
- `Scenes/Main.tscn`: Aldea de las Cenizas / primer recorrido ampliado.
- `Scenes/ForestRuins_2.tscn`: Bosque de los Circuitos.
- `Scenes/Catacombs.tscn`: Catacumbas del Pulso.

Los portales llevan de una escena a la siguiente. El proyecto conserva el argumento y los nombres principales de la historia; estas escenas son prototipos jugables de recorrido, no cinemáticas terminadas.

## Mecánicas del prototipo
- Kairo: movimiento, salto, ataque, animación procedural de espera/carrera/salto/ataque.
- Enemigos Centinela del Vacío con vida y animación flotante.
- Cristales de energía y recuerdos coleccionables.
- HUD con vida, energía y recuerdos.
- Plataformas, cámara de seguimiento y portales entre escenas.
- Controles táctiles en dispositivos compatibles.

## Controles de PC
- A/D o flechas: moverse.
- Espacio: saltar.
- Clic izquierdo: atacar.

## Abrir el proyecto
1. Descarga el repositorio ZIP y extrae la carpeta.
2. Abre Godot 4.7.
3. En el administrador de proyectos pulsa **Importar**.
4. Selecciona `project.godot`.
5. Ejecuta con F6 la escena abierta o F5 para iniciar el proyecto.

## Importante
Los gráficos y animaciones siguen siendo arte provisional del prototipo. Es necesario probar las escenas en el editor Godot 4.7 y corregir cualquier error que aparezca en la consola antes de considerar esta versión estable. La exportación para Windows y Android aún no está configurada.
