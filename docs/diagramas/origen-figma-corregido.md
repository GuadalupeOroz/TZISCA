# Origen de los elementos visuales

Propuesta de corrección local de `docs/diagramas/origen-figma.md`.

## Fuente y alcance

- Tablero de referencia: [FigJam de TZISCA](https://www.figma.com/board/kjk49fziEiQzOM85H3MqRy/Sin-t%C3%ADtulo?node-id=0-1).
- Archivo: `kjk49fziEiQzOM85H3MqRy`, página `Page 1`, nodo raíz `0:1`.
- Las exportaciones PNG existentes se conservan como referencia visual. Esta propuesta no modifica el tablero ni reemplaza sus archivos originales.

## Representaciones entidad–relación

Existen dos representaciones ER de referencia:

| Representación | Archivo original | Propuesta local |
| --- | --- | --- |
| Modelo relacional | `modelo-entidad-relacion-figma.png` | `modelo-entidad-relacion-figma-corregido.png` |
| Modelo en notación Chen | `modelo-entidad-relacion-chen-figjam.png` | `modelo-entidad-relacion-chen-figjam-corregido.png` |

Las propuestas locales se contrastan con `docs/modelo-datos/diccionario-datos.md`. Su contenido correcto son las 19 entidades oficiales: Rol, Usuario, Cliente, PreferenciaCliente, Tratamiento, Paquete, PaqueteTratamiento, Carrito, Cita, CitaCabina, Proveedor, TratamientoProveedor, DisponibilidadProveedor, Cabina, EstadoCabina, Pago, Cancelacion, Devolucion y Transaccion.

No son entidades vigentes Reservacion, ReservacionTratamiento, AsignacionProveedor, HistorialEstadoTratamiento, Indisponibilidad ni BloqueoTemporal. La disponibilidad del proveedor y el bloqueo de una cita se representan con las estructuras vigentes del diccionario.

## Pendiente de aprobación

Antes de sustituir cualquier exportación dentro del repositorio, debe revisarse la propuesta visual y, si se desea mantener el FigJam como fuente editable, aplicarse el mismo ajuste en una copia del tablero.
