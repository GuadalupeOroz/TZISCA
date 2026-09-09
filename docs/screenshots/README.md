# Capturas del proyecto

[Documentación](../README.md) · [Inicio](../../README.md)

Ya hay 73 pantallas y variantes del prototipo V3 exportadas de Figma. Todavía no hay capturas de una aplicación implementada. Ya hay diagramas reales exportados del FigJam en [diagramas](../diagramas/README.md). La portada del README es una ilustración conceptual y no representa una interfaz implementada.

## Convención de nombres

Usar minúsculas, guiones y el patrón `rol-vista.png`:

- `cliente-login.png`
- `cliente-catalogo.png`
- `cliente-carrito.png`
- `admin-dashboard.png`
- `recepcion-agenda.png`
- `proveedor-agenda.png`

Guardar las capturas originales en esta carpeta. Cuando existan, crear miniaturas en `thumbs/` con el mismo nombre y enlazarlas al original desde el README principal. Conservar la proporción y evitar datos personales o credenciales visibles.

Los diseños de Figma deben identificarse como prototipos y documentarse en [vistas](../vistas/README.md), separados de las capturas reales.

## Miniaturas de diagramas disponibles

Los originales se guardan una sola vez en `../diagramas/`. Aquí se conservan miniaturas PNG con el mismo nombre en `thumbs/`, con un lado mayor de hasta 720 px y proporción original. Son previews documentales, no vistas de interfaz.

| Diagrama | Miniatura | Imagen completa |
|---|---|---|
| Casos de uso general | [Preview](thumbs/casos-uso-general.png) | [Original](../diagramas/casos-uso-general.png) |
| Casos de uso — Cliente | [Preview](thumbs/casos-uso-cliente.png) | [Original](../diagramas/casos-uso-cliente.png) |
| Casos de uso — Administrador general | [Preview](thumbs/casos-uso-administrador.png) | [Original](../diagramas/casos-uso-administrador.png) |
| Casos de uso — Recepción y cabinas | [Preview](thumbs/casos-uso-recepcion.png) | [Original](../diagramas/casos-uso-recepcion.png) |
| Casos de uso — Proveedor y automatizaciones | [Preview](thumbs/casos-uso-proveedor.png) | [Original](../diagramas/casos-uso-proveedor.png) |
| Reservación del cliente | [Preview](thumbs/flujo-reservacion-figma.png) | [Original](../diagramas/flujo-reservacion-figma.png) |
| Cancelación de tratamiento o reservación | [Preview](thumbs/flujo-cancelacion-figma.png) | [Original](../diagramas/flujo-cancelacion-figma.png) |
| Indisponibilidad y sustitución de proveedor | [Preview](thumbs/flujo-sustitucion-proveedor-figma.png) | [Original](../diagramas/flujo-sustitucion-proveedor-figma.png) |
| Reservación del cliente — carrito, ciclos y bloqueos | [Preview](thumbs/secuencia-reservacion.png) | [Original](../diagramas/secuencia-reservacion.png) |
| Cancelación de tratamiento o reservación | [Preview](thumbs/secuencia-cancelacion.png) | [Original](../diagramas/secuencia-cancelacion.png) |
| Indisponibilidad y sustitución de proveedor | [Preview](thumbs/secuencia-sustitucion-proveedor.png) | [Original](../diagramas/secuencia-sustitucion-proveedor.png) |
| Realizar pago y confirmar reservación | [Preview](thumbs/secuencia-pago-confirmacion.png) | [Original](../diagramas/secuencia-pago-confirmacion.png) |
| Cancelación y devolución | [Preview](thumbs/secuencia-cancelacion-devolucion.png) | [Original](../diagramas/secuencia-cancelacion-devolucion.png) |
| Arquitectura técnica de TZISCA | [Preview](thumbs/arquitectura-tecnica-figma.png) | [Original](../diagramas/arquitectura-tecnica-figma.png) |
| Diagrama Entidad–Relación | [Preview](thumbs/modelo-entidad-relacion-figma.png) | [Original](../diagramas/modelo-entidad-relacion-figma.png) |

Para futuras vistas de Figma, conservar el original en `../vistas/rol/` y referenciarlo aquí mediante un enlace relativo, sin duplicarlo.

## Interfaces V3 disponibles

Las miniaturas enlazan a un único original en la carpeta de su rol. [Inventario y nodos de origen](../vistas/origen-figma.md).

| Rol | Pantalla | Miniatura | Imagen completa |
|---|---|---|---|
| Cliente | Acceso común — Todos los roles | [Preview](thumbs/cliente-login.png) | [PNG](../vistas/cliente/cliente-login.png) |
| Cliente | Credenciales incorrectas | [Preview](thumbs/cliente-credenciales-incorrectas.png) | [PNG](../vistas/cliente/cliente-credenciales-incorrectas.png) |
| Cliente | Cuenta inactiva | [Preview](thumbs/cliente-cuenta-inactiva.png) | [PNG](../vistas/cliente/cliente-cuenta-inactiva.png) |
| Cliente | Recuperar contraseña | [Preview](thumbs/cliente-recuperar-contrasena.png) | [PNG](../vistas/cliente/cliente-recuperar-contrasena.png) |
| Cliente | Correo de recuperación enviado | [Preview](thumbs/cliente-correo-de-recuperacion-enviado.png) | [PNG](../vistas/cliente/cliente-correo-de-recuperacion-enviado.png) |
| Cliente | Registro de cliente | [Preview](thumbs/cliente-registro.png) | [PNG](../vistas/cliente/cliente-registro.png) |
| Cliente | Inicio y catálogo — Cliente | [Preview](thumbs/cliente-catalogo.png) | [PNG](../vistas/cliente/cliente-catalogo.png) |
| Cliente | Detalle de tratamiento | [Preview](thumbs/cliente-detalle-tratamiento.png) | [PNG](../vistas/cliente/cliente-detalle-tratamiento.png) |
| Cliente | Tu carrito | [Preview](thumbs/cliente-carrito.png) | [PNG](../vistas/cliente/cliente-carrito.png) |
| Cliente | Tu cabina recomendada | [Preview](thumbs/cliente-recomendacion.png) | [PNG](../vistas/cliente/cliente-recomendacion.png) |
| Cliente | Elige tu cabina | [Preview](thumbs/cliente-seleccion-cabina.png) | [PNG](../vistas/cliente/cliente-seleccion-cabina.png) |
| Cliente | Fecha y hora | [Preview](thumbs/cliente-fecha-hora.png) | [PNG](../vistas/cliente/cliente-fecha-hora.png) |
| Cliente | Revisa tu reservación | [Preview](thumbs/cliente-resumen.png) | [PNG](../vistas/cliente/cliente-resumen.png) |
| Cliente | Reservación confirmada | [Preview](thumbs/cliente-confirmacion.png) | [PNG](../vistas/cliente/cliente-confirmacion.png) |
| Cliente | Mis reservaciones | [Preview](thumbs/cliente-mis-reservaciones.png) | [PNG](../vistas/cliente/cliente-mis-reservaciones.png) |
| Cliente | Detalle de mi reservación | [Preview](thumbs/cliente-detalle-reservacion.png) | [PNG](../vistas/cliente/cliente-detalle-reservacion.png) |
| Cliente | Cancelar reservación | [Preview](thumbs/cliente-cancelacion.png) | [PNG](../vistas/cliente/cliente-cancelacion.png) |
| Cliente | Cancelación registrada | [Preview](thumbs/cliente-cancelacion-registrada.png) | [PNG](../vistas/cliente/cliente-cancelacion-registrada.png) |
| Cliente | Historial de reservaciones | [Preview](thumbs/cliente-historial-de-reservaciones.png) | [PNG](../vistas/cliente/cliente-historial-de-reservaciones.png) |
| Cliente | Cancelación parcial registrada | [Preview](thumbs/cliente-cancelacion-parcial-registrada.png) | [PNG](../vistas/cliente/cliente-cancelacion-parcial-registrada.png) |
| Cliente | Detalle · Facial hidratante | [Preview](thumbs/cliente-detalle-facial-hidratante.png) | [PNG](../vistas/cliente/cliente-detalle-facial-hidratante.png) |
| Cliente | Detalle · Hidroterapia | [Preview](thumbs/cliente-detalle-hidroterapia.png) | [PNG](../vistas/cliente/cliente-detalle-hidroterapia.png) |
| Cliente | Selecciona tu método de pago | [Preview](thumbs/cliente-pago.png) | [PNG](../vistas/cliente/cliente-pago.png) |
| Cliente | Procesando pago | [Preview](thumbs/cliente-procesando-pago.png) | [PNG](../vistas/cliente/cliente-procesando-pago.png) |
| Cliente | Pago exitoso | [Preview](thumbs/cliente-pago-exitoso.png) | [PNG](../vistas/cliente/cliente-pago-exitoso.png) |
| Cliente | Pago rechazado | [Preview](thumbs/cliente-pago-rechazado.png) | [PNG](../vistas/cliente/cliente-pago-rechazado.png) |
| Cliente | Detalle de pago | [Preview](thumbs/cliente-detalle-de-pago.png) | [PNG](../vistas/cliente/cliente-detalle-de-pago.png) |
| Cliente | Comprobante de pago | [Preview](thumbs/cliente-comprobante-de-pago.png) | [PNG](../vistas/cliente/cliente-comprobante-de-pago.png) |
| Recepción y cabinas | Agenda diaria — Recepción | [Preview](thumbs/recepcion-agenda-diaria.png) | [PNG](../vistas/recepcion/recepcion-agenda-diaria.png) |
| Recepción y cabinas | Detalle de reservación | [Preview](thumbs/recepcion-detalle-de-reservacion.png) | [PNG](../vistas/recepcion/recepcion-detalle-de-reservacion.png) |
| Recepción y cabinas | Reserva · 1 Cliente | [Preview](thumbs/recepcion-reservacion-manual.png) | [PNG](../vistas/recepcion/recepcion-reservacion-manual.png) |
| Recepción y cabinas | Registrar nuevo cliente | [Preview](thumbs/recepcion-registrar-nuevo-cliente.png) | [PNG](../vistas/recepcion/recepcion-registrar-nuevo-cliente.png) |
| Recepción y cabinas | Reserva · 2 Tratamientos | [Preview](thumbs/recepcion-reserva-2-tratamientos.png) | [PNG](../vistas/recepcion/recepcion-reserva-2-tratamientos.png) |
| Recepción y cabinas | Reserva · 3 Cabina y horario | [Preview](thumbs/recepcion-reserva-3-cabina-y-horario.png) | [PNG](../vistas/recepcion/recepcion-reserva-3-cabina-y-horario.png) |
| Recepción y cabinas | Reserva · 4 Resumen | [Preview](thumbs/recepcion-reserva-4-resumen.png) | [PNG](../vistas/recepcion/recepcion-reserva-4-resumen.png) |
| Recepción y cabinas | Reservación confirmada | [Preview](thumbs/recepcion-reservacion-confirmada.png) | [PNG](../vistas/recepcion/recepcion-reservacion-confirmada.png) |
| Recepción y cabinas | Disponibilidad | [Preview](thumbs/recepcion-disponibilidad.png) | [PNG](../vistas/recepcion/recepcion-disponibilidad.png) |
| Recepción y cabinas | Bloqueos de cabina | [Preview](thumbs/recepcion-bloqueos.png) | [PNG](../vistas/recepcion/recepcion-bloqueos.png) |
| Recepción y cabinas | Conflictos de bloqueo | [Preview](thumbs/recepcion-conflictos-de-bloqueo.png) | [PNG](../vistas/recepcion/recepcion-conflictos-de-bloqueo.png) |
| Recepción y cabinas | Estado operativo de cabinas | [Preview](thumbs/recepcion-estado-cabinas.png) | [PNG](../vistas/recepcion/recepcion-estado-cabinas.png) |
| Recepción y cabinas | Cambiar estado de cabina | [Preview](thumbs/recepcion-cambiar-estado-de-cabina.png) | [PNG](../vistas/recepcion/recepcion-cambiar-estado-de-cabina.png) |
| Recepción y cabinas | Buscar reservación para cancelar | [Preview](thumbs/recepcion-cancelaciones.png) | [PNG](../vistas/recepcion/recepcion-cancelaciones.png) |
| Recepción y cabinas | Cancelación · Detalle | [Preview](thumbs/recepcion-cancelacion-detalle.png) | [PNG](../vistas/recepcion/recepcion-cancelacion-detalle.png) |
| Recepción y cabinas | Confirmar cancelación parcial | [Preview](thumbs/recepcion-confirmar-cancelacion-parcial.png) | [PNG](../vistas/recepcion/recepcion-confirmar-cancelacion-parcial.png) |
| Recepción y cabinas | Confirmar cancelación completa | [Preview](thumbs/recepcion-confirmar-cancelacion-completa.png) | [PNG](../vistas/recepcion/recepcion-confirmar-cancelacion-completa.png) |
| Recepción y cabinas | Cancelación registrada | [Preview](thumbs/recepcion-cancelacion-registrada.png) | [PNG](../vistas/recepcion/recepcion-cancelacion-registrada.png) |
| Recepción y cabinas | Cancelación completa registrada | [Preview](thumbs/recepcion-cancelacion-completa-registrada.png) | [PNG](../vistas/recepcion/recepcion-cancelacion-completa-registrada.png) |
| Recepción y cabinas | Cancelar facial | [Preview](thumbs/recepcion-cancelar-facial.png) | [PNG](../vistas/recepcion/recepcion-cancelar-facial.png) |
| Recepción y cabinas | Facial cancelado | [Preview](thumbs/recepcion-facial-cancelado.png) | [PNG](../vistas/recepcion/recepcion-facial-cancelado.png) |
| Recepción y cabinas | Perfil de Recepción | [Preview](thumbs/recepcion-perfil-de-recepcion.png) | [PNG](../vistas/recepcion/recepcion-perfil-de-recepcion.png) |
| Recepción y cabinas | Reserva · 5 Pago | [Preview](thumbs/recepcion-pago-manual.png) | [PNG](../vistas/recepcion/recepcion-pago-manual.png) |
| Proveedor de tratamiento | Mi agenda | [Preview](thumbs/proveedor-agenda.png) | [PNG](../vistas/proveedor/proveedor-agenda.png) |
| Proveedor de tratamiento | Detalle del tratamiento | [Preview](thumbs/proveedor-detalle-tratamiento.png) | [PNG](../vistas/proveedor/proveedor-detalle-tratamiento.png) |
| Proveedor de tratamiento | Confirmar inicio de atención | [Preview](thumbs/proveedor-iniciar-atencion.png) | [PNG](../vistas/proveedor/proveedor-iniciar-atencion.png) |
| Proveedor de tratamiento | Atención en curso | [Preview](thumbs/proveedor-atencion-en-curso.png) | [PNG](../vistas/proveedor/proveedor-atencion-en-curso.png) |
| Proveedor de tratamiento | Completar atención | [Preview](thumbs/proveedor-completar-atencion.png) | [PNG](../vistas/proveedor/proveedor-completar-atencion.png) |
| Proveedor de tratamiento | Atención completada | [Preview](thumbs/proveedor-atencion-completada.png) | [PNG](../vistas/proveedor/proveedor-atencion-completada.png) |
| Proveedor de tratamiento | Registrar indisponibilidad | [Preview](thumbs/proveedor-indisponibilidad.png) | [PNG](../vistas/proveedor/proveedor-indisponibilidad.png) |
| Proveedor de tratamiento | Servicios afectados | [Preview](thumbs/proveedor-servicios-afectados.png) | [PNG](../vistas/proveedor/proveedor-servicios-afectados.png) |
| Proveedor de tratamiento | Historial de atenciones | [Preview](thumbs/proveedor-historial.png) | [PNG](../vistas/proveedor/proveedor-historial.png) |
| Proveedor de tratamiento | Historial de indisponibilidades | [Preview](thumbs/proveedor-historial-de-indisponibilidades.png) | [PNG](../vistas/proveedor/proveedor-historial-de-indisponibilidades.png) |
| Proveedor de tratamiento | Agenda sin servicios | [Preview](thumbs/proveedor-agenda-sin-servicios.png) | [PNG](../vistas/proveedor/proveedor-agenda-sin-servicios.png) |
| Proveedor de tratamiento | Perfil del usuario | [Preview](thumbs/proveedor-perfil-del-usuario.png) | [PNG](../vistas/proveedor/proveedor-perfil-del-usuario.png) |
| Proveedor de tratamiento | Confirmar cierre de sesión | [Preview](thumbs/proveedor-confirmar-cierre-de-sesion.png) | [PNG](../vistas/proveedor/proveedor-confirmar-cierre-de-sesion.png) |
| Administrador general | Dashboard | [Preview](thumbs/admin-dashboard.png) | [PNG](../vistas/administrador/admin-dashboard.png) |
| Administrador general | Tratamientos | [Preview](thumbs/admin-tratamientos.png) | [PNG](../vistas/administrador/admin-tratamientos.png) |
| Administrador general | Cabinas | [Preview](thumbs/admin-cabinas.png) | [PNG](../vistas/administrador/admin-cabinas.png) |
| Administrador general | Usuarios y roles | [Preview](thumbs/admin-usuarios-roles.png) | [PNG](../vistas/administrador/admin-usuarios-roles.png) |
| Administrador general | Proveedores | [Preview](thumbs/admin-proveedores.png) | [PNG](../vistas/administrador/admin-proveedores.png) |
| Administrador general | Asignar proveedor | [Preview](thumbs/admin-asignacion-proveedor.png) | [PNG](../vistas/administrador/admin-asignacion-proveedor.png) |
| Administrador general | Reservaciones | [Preview](thumbs/admin-reservaciones.png) | [PNG](../vistas/administrador/admin-reservaciones.png) |
| Administrador general | Reportes | [Preview](thumbs/admin-reportes.png) | [PNG](../vistas/administrador/admin-reportes.png) |
| Administrador general | Pagos | [Preview](thumbs/admin-pagos.png) | [PNG](../vistas/administrador/admin-pagos.png) |
