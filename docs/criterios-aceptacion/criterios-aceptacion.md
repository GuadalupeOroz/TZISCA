**TZISCA**

**CRITERIOS DE ACEPTACIÓN**

Casos de Uso CU-01 a CU-43

Este documento reúne los criterios de aceptación asociados a los 43 casos de uso definidos para TZISCA. Cada criterio establece una condición verificable que permitirá determinar si una funcionalidad cumple con el comportamiento esperado. Los criterios están organizados por rol y servirán como base para pruebas, validación funcional y diseño técnico posterior.

# 1. Criterios de aceptación — Cliente

Los siguientes criterios corresponden a las funcionalidades disponibles para el Cliente, desde el registro y consulta de tratamientos hasta la confirmación, consulta y cancelación de reservaciones.

## CU-01 — Registrarse

**CA-01.1 — Registro correcto.** Dado que una persona no tiene cuenta, cuando ingrese los datos obligatorios correctamente y seleccione registrarse, entonces TZISCA deberá crear su cuenta con rol Cliente.

**CA-01.2 — Correo duplicado.** Dado que ya existe una cuenta con el mismo correo, cuando se intente registrar nuevamente, entonces el sistema deberá rechazar el registro e informar que el correo ya está registrado.

**CA-01.3 — Datos obligatorios.** Dado que el usuario está completando el formulario, cuando omita un dato obligatorio, entonces el sistema no deberá crear la cuenta.

**CA-01.4 — Contraseña.** Cuando la contraseña no cumpla las reglas de seguridad definidas, TZISCA deberá indicarlo y solicitar una contraseña válida.

**CA-01.5 — Perfil inicial.** Después de crear la cuenta, el usuario deberá poder completar opcionalmente sus preferencias para personalizar recomendaciones.

## CU-02 — Iniciar sesión

**CA-02.1 — Credenciales correctas.** Dado que el usuario posee una cuenta activa, cuando ingrese correo y contraseña correctos, entonces deberá acceder al sistema.

**CA-02.2 — Credenciales incorrectas.** Cuando el correo o contraseña sean incorrectos, TZISCA deberá rechazar el acceso sin revelar cuál dato fue incorrecto.

**CA-02.3 — Cuenta inactiva.** Si la cuenta está desactivada, el sistema no deberá permitir iniciar sesión.

**CA-02.4 — Acceso según rol.** Después de iniciar sesión, TZISCA deberá mostrar únicamente las funciones correspondientes al rol del usuario.

## CU-03 — Consultar tratamientos

**CA-03.1 — Mostrar tratamientos activos.** Cuando el Cliente abra el catálogo, deberá visualizar únicamente los tratamientos activos.

**CA-03.2 — Información básica.** Cada tratamiento deberá mostrar al menos su nombre, descripción breve y duración.

**CA-03.3 — Tratamientos desactivados.** Un tratamiento desactivado no deberá aparecer como opción disponible para nuevas reservaciones.

**CA-03.4 — Selección.** Al seleccionar un tratamiento, el cliente deberá poder acceder a su ficha detallada.

## CU-04 — Consultar detalle de tratamiento

**CA-04.1 — Información completa.** Al abrir un tratamiento, TZISCA deberá mostrar la información registrada del servicio, incluyendo nombre, descripción, duración, beneficios informativos, características, recomendaciones generales y cabinas relacionadas cuando corresponda.

**CA-04.2 — Agregar al carrito.** Desde el detalle, el Cliente deberá disponer de una opción para agregar el tratamiento al carrito.

**CA-04.3 — Tratamiento desactivado durante la consulta.** Si el tratamiento deja de estar activo antes de agregarse, el sistema deberá impedir agregarlo.

## CU-05 — Agregar tratamiento al carrito

**CA-05.1 — Agregar correctamente.** Cuando el Cliente seleccione “Agregar al carrito”, el tratamiento deberá incorporarse al carrito.

**CA-05.2 — Múltiples tratamientos.** El carrito deberá permitir contener más de un tratamiento.

**CA-05.3 — Tratamiento repetido.** El Cliente podrá agregar el mismo tratamiento más de una vez y cada instancia deberá manejarse de manera independiente.

**CA-05.4 — Sin reservación definitiva.** Agregar un tratamiento al carrito no deberá generar todavía una reservación confirmada.

## CU-06 — Gestionar carrito

**CA-06.1 — Consultar contenido.** Cuando el cliente entre al carrito, deberá ver todos los tratamientos agregados.

**CA-06.2 — Eliminar tratamiento.** El cliente deberá poder eliminar individualmente cualquier tratamiento antes de confirmar.

**CA-06.3 — Modificar tratamiento.** Antes de confirmar, deberá poder modificar número de personas, cabina, fecha y hora.

**CA-06.4 — Elementos independientes.** Modificar un tratamiento repetido no deberá modificar automáticamente otra instancia del mismo tratamiento.

**CA-06.5 — Liberar bloqueo.** Si el tratamiento eliminado ya tiene un bloqueo temporal, TZISCA deberá liberar inmediatamente ese recurso.

**CA-06.6 — Carrito vacío.** Si se eliminan todos los tratamientos, el sistema deberá indicar que el carrito está vacío.

## CU-07 — Obtener recomendación de cabina

**CA-07.1 — Compatibilidad obligatoria.** El sistema no deberá recomendar una cabina incompatible con el tratamiento.

**CA-07.2 — Capacidad.** La cabina recomendada deberá tener capacidad igual o superior al número de personas.

**CA-07.3 — Estado operativo.** Una cabina en mantenimiento, fuera de servicio o desactivada no podrá recomendarse.

**CA-07.4 — Disponibilidad.** Cuando ya exista fecha y hora definida, el sistema deberá considerar la disponibilidad para determinar opciones válidas.

**CA-07.5 — Preferencias personales.** Si el Cliente completó preferencias, TZISCA deberá utilizarlas para ordenar las opciones compatibles.

**CA-07.6 — Sin preferencias.** Si el Cliente no proporcionó preferencias, el sistema deberá seguir funcionando mediante compatibilidad, capacidad, disponibilidad y especialización.

**CA-07.7 — Cabina recomendada.** TZISCA deberá distinguir visualmente cuál opción considera recomendada.

**CA-07.8 — Explicación.** La recomendación deberá mostrar información que ayude al cliente a comprender por qué la cabina resulta adecuada.

**CA-07.9 — Alternativas.** Si existen otras cabinas válidas, deberán mostrarse como alternativas.

## CU-08 — Seleccionar cabina

**CA-08.1 — Aceptar recomendación.** El Cliente deberá poder seleccionar directamente la cabina recomendada.

**CA-08.2 — Elegir alternativa.** El Cliente podrá elegir otra cabina compatible sin estar obligado a seleccionar la recomendada.

**CA-08.3 — Cabina inválida.** El sistema no deberá permitir seleccionar una cabina incompatible o con capacidad insuficiente.

**CA-08.4 — Información de cabina.** Antes de seleccionarla, el cliente podrá consultar sus características relevantes.

**CA-08.5 — Asociación temporal.** La cabina seleccionada deberá quedar asociada al elemento correspondiente del carrito, pero todavía no confirmada definitivamente.

## CU-09 — Consultar disponibilidad y seleccionar fecha/hora

**CA-09.1 — Selección de fecha.** El cliente podrá seleccionar una fecha válida para el tratamiento.

**CA-09.2 — Horarios disponibles.** TZISCA deberá mostrar únicamente horarios que puedan cubrir la duración completa del tratamiento.

**CA-09.3 — Cálculo de finalización.** El sistema deberá calcular automáticamente la hora final a partir de la duración del tratamiento.

**CA-09.4 — Prevención de traslapes.** Si existe una reservación, bloqueo o indisponibilidad que se superponga con cualquier parte del intervalo, el horario deberá considerarse ocupado.

**CA-09.5 — Alternativas.** Si la hora seleccionada no está disponible, TZISCA deberá permitir seleccionar otra hora.

**CA-09.6 — Otros tratamientos intactos.** Cambiar el horario de un tratamiento no deberá modificar los horarios de los demás elementos del carrito.

## CU-10 — Bloquear temporalmente horario

CA-10.1 — Creación del bloqueo. Dado que el Cliente selecciona una cabina, fecha y hora válidas, cuando TZISCA comprueba su disponibilidad, entonces deberá crear un bloqueo temporal para ese recurso e intervalo.

CA-10.2 — Duración configurable de 15 minutos. Dado que la duración del bloqueo temporal está aprobada como parámetro operativo (15 minutos, DP-OP-08 / RN-98), cuando TZISCA cree el bloqueo, entonces deberá calcular su vencimiento usando ese parámetro configurado y no un valor fijo incorporado directamente en el código.

CA-10.3 — Exclusividad. Dado un bloqueo temporal vigente, cuando otro Cliente intente confirmar un tratamiento que entre en conflicto con el mismo recurso e intervalo, entonces TZISCA deberá impedir esa confirmación.

CA-10.4 — Vencimiento. Dado un bloqueo temporal sin confirmación definitiva, cuando se alcance el momento de expiración calculado con la duración configurada, entonces TZISCA deberá vencer el bloqueo y liberar el recurso sin asumir un valor fijo de tiempo.

CA-10.5 — Cambio de horario. Dado un tratamiento con bloqueo temporal, cuando el Cliente cambie su horario, entonces TZISCA deberá liberar inmediatamente el bloqueo anterior.

CA-10.6 — Nuevo bloqueo. Después de liberar el horario anterior, cuando el Cliente elija otro intervalo disponible, entonces TZISCA deberá validar el recurso y crear un nuevo bloqueo temporal con la duración configurada.

CA-10.7 — Varios tratamientos. Dado un carrito con varios tratamientos configurados, cuando sus recursos estén disponibles, entonces TZISCA deberá mantener un bloqueo temporal independiente para cada tratamiento.

## CU-11 — Confirmar reservación

CA-11.1 — Reservación previa al pago. Dado un carrito configurado, con disponibilidad válida y bloqueos temporales vigentes, cuando el Cliente continúe a la confirmación, entonces TZISCA deberá crear una Reservacion en estado EN_PROCESO y asignarle id_reservacion.

CA-11.2 — Sin confirmación anticipada. Dada una Reservacion recién creada en estado EN_PROCESO, cuando aún no se cumplan el pago requerido y la revalidación final, entonces el sistema no deberá presentarla ni tratarla como confirmada.

CA-11.3 — Tratamientos pendientes. Al crear la Reservacion EN_PROCESO, cada ReservacionTratamiento deberá crearse en estado PENDIENTE antes de iniciar el pago.

CA-11.4 — Importe y pago. Dada la Reservacion EN_PROCESO, cuando TZISCA calcule el importe conforme a la política aprobada, entonces deberá relacionar el proceso de CU-39 con su id_reservacion.

CA-11.5 — Pago aprobado y revalidación. Dado un Pago en estado Pagado, cuando CU-39 devuelva el control a CU-11, entonces TZISCA deberá revalidar la disponibilidad de todos los recursos antes de confirmar.

CA-11.6 — Confirmación definitiva. Dado un Pago Pagado y una revalidación satisfactoria, cuando TZISCA confirme la Reservacion, entonces cada ReservacionTratamiento válido deberá pasar de PENDIENTE a CONFIRMADO.

CA-11.7 — Ocupación real. Al completar la confirmación definitiva, los bloqueos temporales de los tratamientos confirmados deberán convertirse en ocupaciones reales de sus recursos e intervalos.

CA-11.8 — Pago fallido. Dado que el intento de pago quede en estado Fallido, cuando TZISCA reciba el resultado, entonces deberá conservar la trazabilidad del intento y no deberá confirmar la Reservacion ni cambiar sus tratamientos a CONFIRMADO.

CA-11.9 — Pago cancelado. Dado que el Pago quede en estado Cancelado, cuando TZISCA procese el resultado, entonces la Reservacion deberá permanecer EN_PROCESO, sus tratamientos deberán permanecer PENDIENTE y no deberá producirse la confirmación.

CA-11.10 — Bloqueo expirado. Dado que un bloqueo temporal haya expirado, cuando se intente continuar la confirmación, entonces TZISCA deberá liberarlo, revalidar la disponibilidad y no deberá confirmar automáticamente.

CA-11.11 — Pago aprobado con disponibilidad perdida. Dado un Pago Pagado y una revalidación que detecte disponibilidad perdida, cuando TZISCA procese el resultado, entonces no deberá confirmar el tratamiento afectado, deberá mantener la Reservacion en EN_PROCESO, informar al Cliente del conflicto conforme a DP-EC-02 (RN-105) y ofrecerle seleccionar otra cabina u horario disponible, conservar los tratamientos válidos con devolución parcial del afectado, o cancelar la operación con devolución total, sin presumir ninguna de estas acciones de forma automática.

CA-11.12 — Prevención de confirmación duplicada. Dada una Reservacion ya confirmada, cuando TZISCA reciba nuevamente la misma respuesta de pago o la misma solicitud de confirmación, entonces no deberá crear otra Reservacion, repetir la transición a CONFIRMADO ni duplicar las ocupaciones reales.

CA-11.13 — Conflicto parcial. Si la revalidación detecta conflicto en un solo tratamiento, TZISCA deberá conservar sin cambios los demás y permitir al Cliente modificar o eliminar el afectado, conservar los válidos o cancelar el proceso.

CA-11.14 — Asignación inicial del proveedor. Después de la confirmación definitiva, la asignación inicial del proveedor deberá quedar disponible exclusivamente para el Administrador general y no deberá ejecutarse durante el carrito, el pago ni el estado EN_PROCESO.

## CU-12 — Consultar mis reservaciones

CA-12.1 — Privacidad de reservaciones. Dado un Cliente autenticado, cuando consulte sus reservaciones, entonces TZISCA solo deberá devolver las asociadas a su propia cuenta.

CA-12.2 — Lista. Cuando el Cliente abra Mis reservaciones, el sistema deberá mostrar sus reservaciones existentes con información suficiente para seleccionarlas.

CA-12.3 — Detalle. Cuando el Cliente abra una reservación propia, TZISCA deberá mostrar los tratamientos que la integran.

CA-12.4 — Información individual. Por cada tratamiento, el detalle deberá mostrar tratamiento, cabina, fecha, hora, número de personas, estado y proveedor asignado cuando exista.

CA-12.5 — Estados independientes. Dada una reservación con tratamientos en estados diferentes, cuando se consulte su detalle, entonces TZISCA deberá mostrar el estado real de cada tratamiento sin unificarlos.

CA-12.6 — Estado de pago. Cuando una reservación tenga un Pago relacionado, TZISCA deberá mostrar su estado actual usando únicamente Pendiente, Procesando, Pagado, Fallido, Cancelado, Reembolsado o Reembolsado parcialmente.

CA-12.7 — Consulta del pago. Desde una reservación propia con Pago relacionado, el Cliente deberá poder abrir el detalle definido en CU-40 y consultar importe, fecha, medio y referencia cuando existan.

CA-12.8 — Privacidad del pago. Dado un Cliente autenticado, cuando solicite el pago de una reservación ajena o datos financieros sensibles completos, entonces TZISCA deberá denegar la consulta y no deberá exponer esa información.

## CU-13 — Cancelar tratamiento

CA-13.1 — Cancelación individual. Dado que un tratamiento puede cancelarse, cuando el Cliente confirme la cancelación, entonces TZISCA deberá cancelar solo ese tratamiento.

CA-13.2 — Confirmación de la acción. Antes de modificar el estado del tratamiento, TZISCA deberá solicitar una confirmación explícita al Cliente.

CA-13.3 — Cambio de estado. Después de una cancelación válida, el ReservacionTratamiento seleccionado deberá quedar en estado CANCELADO.

CA-13.4 — Liberación de recursos. Al cancelar el tratamiento, TZISCA deberá liberar su cabina, horario y proveedor asignado, si existe.

CA-13.5 — Otros tratamientos. La cancelación individual no deberá modificar automáticamente los demás tratamientos de la Reservacion.

CA-13.6 — Detección del pago. Después de cancelar, TZISCA deberá consultar el Pago relacionado e identificar de forma verificable si se encuentra en estado Pagado.

CA-13.7 — Importe afectado. Si existe un Pago Pagado, TZISCA deberá calcular el importe afectado por el tratamiento cancelado conforme a la política aprobada y registrar el resultado de esa evaluación.

CA-13.8 — Posible devolución parcial. Solo cuando la política aprobada autorice una devolución y exista importe reembolsable, TZISCA deberá iniciar CU-43 con tipo PARCIAL y el importe determinado.

CA-13.9 — Sin devolución aplicable. Si no existe Pago Pagado, la política no autoriza devolución o el importe reembolsable es cero, TZISCA deberá conservar la cancelación sin crear una Devolucion.

CA-13.10 — Trazabilidad económica. La cancelación deberá conservar la relación entre Reservacion, ReservacionTratamiento, Pago y, cuando se haya iniciado CU-43, la Devolucion, junto con el importe afectado y el resultado de la evaluación.

## CU-14 — Cancelar reservación completa

CA-14.1 — Advertencia. Antes de cancelar la Reservacion, TZISCA deberá mostrar los tratamientos activos que serán afectados.

CA-14.2 — Confirmación. La cancelación completa solo deberá ejecutarse después de la confirmación explícita del Cliente.

CA-14.3 — Estados. Al ejecutar la cancelación, todos los tratamientos que todavía puedan cancelarse deberán pasar a CANCELADO; los tratamientos completados deberán conservar su estado histórico.

CA-14.4 — Liberación total. TZISCA deberá liberar las cabinas, horarios y proveedores correspondientes a los tratamientos cancelados.

CA-14.5 — Conservación histórica. La Reservacion y sus tratamientos no deberán eliminarse físicamente y deberán conservarse para consulta y auditoría.

CA-14.6 — Detección del pago. Después de cancelar, TZISCA deberá consultar el Pago relacionado e identificar si se encuentra en estado Pagado.

CA-14.7 — Importe afectado. Si existe un Pago Pagado, el sistema deberá calcular el importe afectado por los elementos cancelados conforme a la política aprobada y registrar el resultado.

CA-14.8 — Posible devolución total o parcial. Solo cuando la política aprobada autorice una devolución y exista importe reembolsable, TZISCA deberá iniciar CU-43 con tipo TOTAL o PARCIAL según el alcance económico determinado.

CA-14.9 — Sin devolución aplicable. Si no existe Pago Pagado, la política no autoriza devolución o el importe reembolsable es cero, TZISCA no deberá crear una Devolucion.

CA-14.10 — Trazabilidad económica. La cancelación deberá conservar el Pago original, el importe afectado, la decisión de la política y, cuando corresponda, el tipo, importe y estado de la Devolucion.

# 2. Criterios de aceptación — Administrador general

Estos criterios validan las funciones de configuración, administración y supervisión que corresponden al Administrador general.

## CU-15 — Gestionar tratamientos

**CA-15.1 — Crear tratamiento.** Dado que el Administrador general inició sesión, cuando capture correctamente los datos obligatorios de un nuevo tratamiento, entonces TZISCA deberá registrarlo.

**CA-15.2 — Validar campos obligatorios.** Si falta nombre, duración u otro dato requerido, el sistema no deberá guardar el tratamiento.

**CA-15.3 — Editar tratamiento.** El Administrador general deberá poder modificar la información de un tratamiento existente.

**CA-15.4 — Desactivar tratamiento.** El Administrador general deberá poder desactivar un tratamiento para impedir que aparezca en nuevas reservaciones.

**CA-15.5 — Conservar historial.** Si un tratamiento ya fue utilizado en reservaciones, el sistema no deberá eliminar su información histórica.

**CA-15.6 — Relación con cabinas.** El Administrador general deberá poder indicar con qué cabinas es compatible el tratamiento.

## CU-16 — Gestionar cabinas

**CA-16.1 — Crear cabina.** El Administrador general deberá poder registrar una nueva cabina con sus datos obligatorios.

**CA-16.2 — Capacidad obligatoria.** Toda cabina deberá tener definida una capacidad válida.

**CA-16.3 — Editar cabina.** El Administrador general deberá poder modificar información como nombre, descripción, capacidad, características y equipamiento.

**CA-16.4 — Compatibilidades.** Deberá poder relacionar cada cabina con uno o varios tratamientos compatibles.

**CA-16.5 — Desactivar cabina.** Una cabina desactivada no deberá aparecer disponible para nuevas reservaciones.

**CA-16.6 — Mantener historial.** Una cabina que ya tenga reservaciones asociadas no deberá eliminarse físicamente si eso afecta el historial.

## CU-17 — Gestionar usuarios y roles

**CA-17.1 — Consultar usuarios.** El Administrador general deberá poder consultar las cuentas registradas.

**CA-17.2 — Consultar rol.** El sistema deberá mostrar el rol actual de cada usuario.

**CA-17.3 — Cambiar rol.** El Administrador general podrá asignar un rol permitido a un usuario.

**CA-17.4 — Aplicar permisos.** Cuando se cambie el rol, los permisos visibles y disponibles deberán actualizarse de acuerdo con el nuevo rol.

**CA-17.5 — Roles permitidos.** El sistema deberá reconocer al menos Cliente, Administrador general, Recepción y cabinas y Proveedor de tratamiento.

**CA-17.6 — Cuenta desactivada.** El Administrador general deberá poder desactivar una cuenta sin eliminar su información histórica.

**CA-17.7 — Restricción de acceso.** Una cuenta desactivada no deberá poder iniciar sesión.

## CU-18 — Gestionar proveedores de tratamiento

**CA-18.1 — Registrar proveedor.** El Administrador general deberá poder registrar un nuevo proveedor.

**CA-18.2 — Asociar tratamientos.** Cada proveedor deberá poder relacionarse con uno o varios tratamientos que esté autorizado para realizar.

**CA-18.3 — Editar proveedor.** El Administrador general deberá poder modificar la información del proveedor.

**CA-18.4 — Cambiar tratamientos asociados.** El Administrador general deberá poder agregar o quitar tratamientos de la lista que puede atender el proveedor.

**CA-18.5 — Desactivar proveedor.** Un proveedor desactivado no deberá aparecer disponible para nuevas asignaciones.

**CA-18.6 — Historial.** La desactivación no deberá eliminar los tratamientos que haya atendido anteriormente.

## CU-19 — Asignar proveedor a tratamiento

**CA-19.1 — Mostrar proveedores compatibles.** Cuando el Administrador general seleccione un tratamiento confirmado, TZISCA deberá mostrar únicamente proveedores autorizados para realizarlo.

**CA-19.2 — Validar disponibilidad.** El sistema deberá comprobar que el proveedor esté disponible durante todo el intervalo del tratamiento.

**CA-19.3 — Evitar traslapes.** No deberá permitirse asignar un proveedor que tenga otro tratamiento que se traslape.

**CA-19.4 — Indisponibilidad registrada.** Un proveedor con una indisponibilidad registrada para ese periodo no deberá mostrarse como disponible.

**CA-19.5 — Asignación correcta.** Cuando el Administrador seleccione un proveedor válido, TZISCA deberá asociarlo al tratamiento.

**CA-19.6 — Actualizar agenda.** Después de la asignación, el tratamiento deberá aparecer en la agenda del proveedor.

**CA-19.7 — Sin proveedor disponible.** Si no existe ningún proveedor válido, el sistema deberá informar que no hay disponibilidad.

**CA-19.8 — Permiso exclusivo.** La asignación inicial de proveedor deberá estar disponible únicamente para el Administrador general.

## CU-20 — Consultar todas las reservaciones

**CA-20.1 — Acceso global.** El Administrador general deberá poder consultar las reservaciones de todos los clientes.

**CA-20.2 — Información general.** Cada reservación deberá mostrar al menos la identificación correspondiente y los datos básicos del cliente.

**CA-20.3 — Tratamientos incluidos.** Al abrir una reservación, el sistema deberá mostrar todos sus tratamientos.

**CA-20.4 — Detalle por tratamiento.** Para cada tratamiento deberá poder consultar cabina, fecha, hora, número de personas, estado y proveedor asignado.

**CA-20.5 — Estados diferentes.** El sistema deberá permitir que los tratamientos de una misma reservación tengan estados diferentes.

**CA-20.6 — Información actualizada.** Los cambios realizados por otros roles deberán reflejarse al volver a consultar la reservación.

## CU-21 — Cancelar tratamiento o reservación

CA-21.1 — Cancelar tratamiento individual. Dado un Administrador general autorizado, cuando seleccione un tratamiento cancelable y confirme la acción, entonces TZISCA deberá cancelar únicamente ese tratamiento.

CA-21.2 — Cancelar reservación completa. Dado un Administrador general autorizado, cuando confirme la cancelación completa, entonces TZISCA deberá cancelar todos los tratamientos activos que puedan cancelarse.

CA-21.3 — Confirmación. Antes de ejecutar cualquiera de las cancelaciones, el sistema deberá solicitar una confirmación explícita.

CA-21.4 — Liberación de recursos. Por cada tratamiento cancelado, TZISCA deberá liberar la cabina, el horario y el proveedor asignado, si existe.

CA-21.5 — Estado. Cada tratamiento cancelado deberá quedar en estado CANCELADO y los elementos no afectados deberán conservar su estado.

CA-21.6 — Registro operativo. TZISCA deberá registrar el Administrador responsable, la fecha, la hora y el motivo cuando corresponda.

CA-21.7 — Detección del pago. Después de la cancelación, TZISCA deberá consultar el Pago relacionado e identificar si está Pagado.

CA-21.8 — Importe afectado. Si existe un Pago Pagado, el sistema deberá calcular y registrar el importe afectado conforme a la política aprobada.

CA-21.9 — Posible devolución. Solo si la política aprobada autoriza una devolución y existe importe reembolsable, TZISCA deberá ejecutar CU-43 con tipo PARCIAL o TOTAL según los elementos cancelados.

CA-21.10 — Sin devolución aplicable. Si no existe Pago Pagado, la política no autoriza devolución o no existe importe reembolsable, la cancelación deberá conservarse sin iniciar CU-43.

CA-21.11 — Trazabilidad económica. La operación deberá conservar la relación entre la cancelación, la Reservacion, los tratamientos afectados, el Pago original y cualquier Devolucion iniciada.

## CU-22 — Consultar reportes básicos

**CA-22.1 — Acceso restringido.** Los reportes definidos para administración deberán estar disponibles para el Administrador general.

**CA-22.2 — Seleccionar periodo.** El Administrador deberá poder elegir el periodo que desea consultar.

**CA-22.3 — Reservaciones por día.** El sistema deberá poder mostrar la cantidad de reservaciones registradas en el periodo correspondiente.

**CA-22.4 — Tratamientos más solicitados.** TZISCA deberá poder identificar los tratamientos con mayor número de reservas.

**CA-22.5 — Cabinas más utilizadas.** El sistema deberá poder mostrar las cabinas con mayor utilización.

**CA-22.6 — Cancelaciones.** El reporte deberá poder mostrar información básica sobre tratamientos o reservaciones canceladas.

**CA-22.7 — Ocupación de cabinas.** El Administrador deberá poder consultar información básica sobre ocupación de los recursos.

**CA-22.8 — Datos reales.** Los resultados deberán generarse a partir de las reservaciones y movimientos almacenados en TZISCA.

# 3. Criterios de aceptación — Recepción y cabinas

Estos criterios corresponden a la operación cotidiana del spa: creación de reservaciones manuales, agenda, disponibilidad, bloqueos, estados de cabina y cancelaciones.

## CU-23 — Crear reservación manual

CA-23.1 — Acceso a creación manual. Dado un usuario con rol Recepción y cabinas, cuando ingrese al módulo de reservaciones, entonces deberá poder iniciar una reservación manual.

CA-23.2 — Selección o registro de Cliente. Recepción deberá poder buscar un Cliente existente o registrarlo conforme a las reglas de cuentas antes de asociarlo.

CA-23.3 — Tratamientos independientes. La reservación manual deberá admitir uno o varios tratamientos, incluso repetidos, y cada instancia deberá conservar su propio número de personas, cabina, fecha y hora.

CA-23.4 — Cabina válida. Para cada tratamiento, TZISCA deberá impedir la selección de una cabina incompatible o sin capacidad suficiente.

CA-23.5 — Disponibilidad. Antes de crear la Reservacion, TZISCA deberá comprobar la disponibilidad de la cabina durante toda la duración del tratamiento e impedir continuar con los elementos en conflicto.

CA-23.6 — Estado previo al pago. Dado que la reservación manual requiere pago y sus datos son válidos, cuando Recepción continúe, entonces TZISCA deberá crear la Reservacion en estado EN_PROCESO con id_reservacion y cada ReservacionTratamiento en estado PENDIENTE.

CA-23.7 — Importe. Para una Reservacion manual EN_PROCESO, TZISCA deberá calcular el importe en backend conforme a la política aprobada antes de ejecutar CU-42.

CA-23.8 — Pago pendiente o en procesamiento. Si el Pago está Pendiente o Procesando, la Reservacion deberá permanecer EN_PROCESO, sus tratamientos deberán permanecer PENDIENTE y no deberá mostrarse como confirmada.

CA-23.9 — Pago fallido o cancelado. Si el Pago queda Fallido o Cancelado, TZISCA deberá conservar su trazabilidad y no deberá confirmar la Reservacion ni sus tratamientos.

CA-23.10 — Pago aprobado. Dado un Pago Pagado, cuando CU-42 devuelva el control a CU-23, entonces TZISCA deberá revalidar la disponibilidad antes de confirmar.

CA-23.11 — Confirmación manual. Solo si el Pago está Pagado y la revalidación resulta satisfactoria, la Reservacion deberá confirmarse y sus ReservacionTratamiento deberán pasar de PENDIENTE a CONFIRMADO.

CA-23.12 — Permanencia en proceso. Mientras no se cumplan las condiciones económicas y de disponibilidad, la Reservacion manual podrá permanecer EN_PROCESO y sus tratamientos en PENDIENTE.

CA-23.13 — Asignación inicial del proveedor. La creación manual no deberá asignar proveedor; después de la confirmación, la asignación inicial deberá corresponder exclusivamente al Administrador general.

CA-23.14 — Registro de responsable. TZISCA deberá guardar el usuario de Recepción que creó la Reservacion manual y la fecha y hora de la operación.

## CU-24 — Consultar agenda diaria

**CA-24.1 — Seleccionar fecha.** Recepción deberá poder seleccionar el día que desea consultar.

**CA-24.2 — Mostrar tratamientos programados.** TZISCA deberá mostrar los tratamientos programados para la fecha seleccionada.

**CA-24.3 — Información de agenda.** Por cada tratamiento deberá mostrarse, como mínimo, hora de inicio, hora de finalización, tratamiento, cabina, cliente, número de personas, proveedor asignado cuando exista y estado.

**CA-24.4 — Orden cronológico.** Los tratamientos deberán poder visualizarse ordenados por horario.

**CA-24.5 — Varias cabinas.** La agenda deberá permitir identificar qué cabina corresponde a cada servicio.

**CA-24.6 — Cambios actualizados.** Si un tratamiento es cancelado, reasignado o cambia de estado, la agenda deberá reflejar la información actualizada.

**CA-24.7 — Día sin reservaciones.** Si no existen tratamientos programados para la fecha consultada, el sistema deberá indicarlo claramente.

## CU-25 — Consultar disponibilidad

**CA-25.1 — Consulta por fecha.** Recepción deberá poder consultar la disponibilidad correspondiente a una fecha específica.

**CA-25.2 — Consulta por cabina.** Deberá poder revisar los espacios libres y ocupados de una cabina.

**CA-25.3 — Consulta relacionada con tratamiento.** Cuando se seleccione un tratamiento, el sistema deberá considerar únicamente las cabinas compatibles.

**CA-25.4 — Reservaciones confirmadas.** Los periodos correspondientes a tratamientos confirmados deberán mostrarse como no disponibles.

**CA-25.5 — Bloqueos temporales.** Los horarios con un bloqueo temporal vigente deberán considerarse no disponibles para evitar conflictos.

**CA-25.6 — Bloqueos operativos.** Una cabina bloqueada por mantenimiento, limpieza u otra causa deberá mostrarse como no disponible durante ese periodo.

**CA-25.7 — Estado operativo.** Una cabina en mantenimiento, fuera de servicio o desactivada no deberá ofrecerse como disponible.

**CA-25.8 — Duración completa.** La disponibilidad deberá calcularse tomando en cuenta todo el intervalo del tratamiento y no únicamente la hora de inicio.

## CU-26 — Bloquear horario o día de una cabina

**CA-26.1 — Seleccionar cabina.** Recepción deberá poder seleccionar la cabina que desea bloquear.

**CA-26.2 — Bloqueo por horas.** El sistema deberá permitir establecer una fecha, hora inicial y hora final.

**CA-26.3 — Bloqueo de día completo.** Recepción deberá poder marcar una cabina como no disponible durante todo un día.

**CA-26.4 — Motivo obligatorio.** Todo bloqueo deberá registrar un motivo, por ejemplo mantenimiento, limpieza, incidencia, uso interno u otro.

**CA-26.5 — Aplicación inmediata.** Una vez confirmado el bloqueo, el intervalo correspondiente no deberá aparecer disponible para nuevas reservaciones.

**CA-26.6 — Conflicto con reservación existente.** Si existen tratamientos confirmados dentro del periodo que se intenta bloquear, TZISCA deberá advertir a Recepción antes de completar la acción.

**CA-26.7 — No cancelar silenciosamente.** Crear un bloqueo no deberá cancelar automáticamente una reservación existente sin que el usuario gestione primero el conflicto.

**CA-26.8 — Historial.** El sistema deberá registrar quién creó el bloqueo, cuándo se creó y cuál fue el motivo.

**CA-26.9 — Liberar bloqueo.** Cuando un bloqueo deje de ser necesario, deberá poder retirarse para que los horarios vuelvan a calcularse como disponibles cuando corresponda.

## CU-27 — Gestionar estado operativo de cabina

**CA-27.1 — Consultar estado.** Recepción deberá poder consultar el estado actual de una cabina.

**CA-27.2 — Estados operativos.** El sistema deberá manejar los estados Disponible, Ocupada, En mantenimiento, Fuera de servicio y Desactivada.

**CA-27.3 — Mantenimiento.** Cuando una cabina cambie a En mantenimiento, deberá dejar de estar disponible para nuevas reservaciones.

**CA-27.4 — Fuera de servicio.** Una cabina marcada como Fuera de servicio tampoco deberá ofrecerse para nuevas reservaciones.

**CA-27.5 — Regreso a disponible.** Cuando termine la incidencia o mantenimiento y la cabina vuelva a Disponible, TZISCA deberá permitir utilizarla nuevamente, siempre que no exista otro bloqueo.

**CA-27.6 — Observación.** Al cambiar una cabina a mantenimiento o fuera de servicio, el sistema deberá permitir registrar el motivo u observación.

**CA-27.7 — Historial de cambios.** TZISCA deberá conservar quién realizó el cambio, fecha, hora, estado anterior y estado nuevo.

**CA-27.8 — Desactivación.** La desactivación administrativa de una cabina deberá quedar principalmente bajo control del Administrador general.

## CU-28 — Cancelar tratamiento

CA-28.1 — Selección. Dado un usuario de Recepción y cabinas autorizado, cuando abra una Reservacion, entonces deberá poder seleccionar un tratamiento cancelable.

CA-28.2 — Motivo y confirmación. Antes de cancelar, TZISCA deberá registrar el motivo cuando corresponda y solicitar una confirmación explícita.

CA-28.3 — Cambio de estado. Después de confirmar la acción, el tratamiento seleccionado deberá quedar CANCELADO y los demás no deberán modificarse.

CA-28.4 — Liberación de recursos. TZISCA deberá liberar la cabina, el horario y el proveedor asignado al tratamiento cancelado, si existe.

CA-28.5 — Historial operativo. El sistema deberá conservar el usuario de Recepción, la fecha, la hora y el motivo de la cancelación.

CA-28.6 — Detección del pago. Después de cancelar, TZISCA deberá consultar el Pago relacionado y verificar si está Pagado.

CA-28.7 — Importe afectado. Si existe un Pago Pagado, el sistema deberá calcular y registrar el importe del tratamiento afectado conforme a la política aprobada.

CA-28.8 — Posible devolución parcial. Solo si la política aprobada autoriza una devolución y existe importe reembolsable, Recepción deberá continuar con CU-43 usando tipo PARCIAL.

CA-28.9 — Sin devolución aplicable. Si no existe Pago Pagado, la política no autoriza devolución o no existe importe reembolsable, TZISCA deberá conservar la cancelación sin iniciar CU-43.

CA-28.10 — Trazabilidad económica. El Pago original, el importe afectado, la evaluación de la política y cualquier Devolucion deberán permanecer relacionados con la cancelación.

## CU-29 — Cancelar reservación completa

CA-29.1 — Selección y alcance. Dado un usuario de Recepción y cabinas autorizado, cuando seleccione Cancelar reservación, entonces TZISCA deberá mostrar todos los tratamientos activos afectados.

CA-29.2 — Advertencia y confirmación. Antes de ejecutar la cancelación, TZISCA deberá mostrar la advertencia, permitir registrar el motivo y exigir confirmación explícita.

CA-29.3 — Estados. Después de confirmar, todos los tratamientos activos que puedan cancelarse deberán quedar CANCELADO y los tratamientos completados deberán conservar su estado histórico.

CA-29.4 — Liberación de recursos. TZISCA deberá liberar las cabinas, horarios y proveedores de los tratamientos cancelados.

CA-29.5 — Conservación histórica. La Reservacion y sus tratamientos no deberán eliminarse físicamente y el sistema deberá registrar el usuario, la fecha, la hora y el motivo.

CA-29.6 — Detección del pago. Después de cancelar, TZISCA deberá consultar el Pago relacionado y verificar si está Pagado.

CA-29.7 — Importe afectado. Si existe un Pago Pagado, TZISCA deberá calcular y registrar el importe de los elementos cancelados conforme a la política aprobada.

CA-29.8 — Posible devolución total o parcial. Solo si la política aprobada autoriza una devolución y existe importe reembolsable, Recepción deberá continuar con CU-43 usando tipo TOTAL o PARCIAL según el alcance económico.

CA-29.9 — Sin devolución aplicable. Si no existe Pago Pagado, la política no autoriza devolución o no existe importe reembolsable, TZISCA deberá conservar la cancelación sin iniciar CU-43.

CA-29.10 — Trazabilidad económica. La cancelación deberá conservar la relación con el Pago original, el importe afectado, la evaluación de la política y cualquier Devolucion iniciada.

# 4. Criterios de aceptación — Proveedor de tratamiento y procesos automáticos

Este bloque valida las funciones del Proveedor y los procesos automáticos de TZISCA relacionados con atención, indisponibilidad y reasignación de proveedores.

## CU-30 — Consultar mi agenda

**CA-30.1 — Acceso a agenda propia.** Dado que el usuario tiene rol Proveedor de tratamiento, cuando ingrese a su agenda, entonces TZISCA deberá mostrar únicamente los tratamientos que tenga asignados.

**CA-30.2 — Organización por fecha y hora.** Los tratamientos deberán visualizarse ordenados por fecha y horario.

**CA-30.3 — Información mínima visible.** Cada elemento de agenda deberá mostrar tratamiento, fecha, hora de inicio, hora de finalización, cabina, número de personas y estado.

**CA-30.4 — Agenda vacía.** Si el proveedor no tiene tratamientos asignados para la fecha seleccionada, el sistema deberá indicarlo claramente.

**CA-30.5 — Tratamiento cancelado.** Si un tratamiento fue cancelado, no deberá mostrarse como pendiente de atención.

**CA-30.6 — Privacidad entre proveedores.** Un proveedor no podrá consultar la agenda de otro proveedor.

## CU-31 — Consultar detalle de tratamiento asignado

**CA-31.1 — Acceso restringido.** El proveedor solo podrá abrir el detalle de tratamientos que estén asignados a su cuenta.

**CA-31.2 — Información del tratamiento.** El detalle deberá mostrar nombre del tratamiento, descripción básica, duración, fecha, hora, cabina, número de personas y estado.

**CA-31.3 — Datos básicos del cliente.** El proveedor podrá consultar únicamente los datos del cliente necesarios para prestar el servicio.

**CA-31.4 — Tratamiento reasignado.** Si el tratamiento ya fue asignado a otro proveedor, el proveedor anterior no deberá poder gestionarlo.

**CA-31.5 — Tratamiento cancelado.** Si el tratamiento fue cancelado, TZISCA deberá mostrar su estado y bloquear acciones de atención.

## CU-32 — Marcar tratamiento En atención

**CA-32.1 — Acción disponible.** La opción “Iniciar atención” deberá mostrarse únicamente al proveedor asignado.

**CA-32.2 — Estado previo válido.** Solo un tratamiento en estado Confirmado podrá pasar a En atención.

**CA-32.3 — Confirmación de inicio.** El sistema deberá pedir confirmación antes de cambiar el estado.

**CA-32.4 — Registro de hora.** Al iniciar la atención, TZISCA deberá registrar la fecha y hora real de inicio.

**CA-32.5 — Cambio de estado.** El tratamiento deberá cambiar de Confirmado a En atención.

**CA-32.6 — Restricción de cancelados.** Un tratamiento Cancelado no podrá iniciar atención.

**CA-32.7 — Historial.** TZISCA deberá registrar qué proveedor inició la atención y cuándo.

## CU-33 — Marcar tratamiento Completado

**CA-33.1 — Estado previo.** Solo un tratamiento en estado En atención podrá marcarse como Completado.

**CA-33.2 — Acción exclusiva.** Solo el proveedor asignado deberá poder finalizar su atención.

**CA-33.3 — Confirmación.** Antes de completar el servicio, el sistema deberá solicitar confirmación.

**CA-33.4 — Hora de finalización.** TZISCA deberá registrar la fecha y hora real en que se finalizó el tratamiento.

**CA-33.5 — Cambio de estado.** El tratamiento deberá cambiar de En atención a Completado.

**CA-33.6 — Conservación histórica.** El tratamiento completado deberá conservarse en el historial.

**CA-33.7 — No reabrir automáticamente.** Un tratamiento Completado no deberá volver automáticamente a Confirmado o En atención.

## CU-34 — Registrar indisponibilidad

**CA-34.1 — Registrar periodo.** El proveedor deberá poder indicar un periodo durante el cual no podrá trabajar.

**CA-34.2 — Fecha obligatoria.** La indisponibilidad deberá incluir fecha de inicio y fecha de finalización, o una única fecha cuando corresponda.

**CA-34.3 — Horario parcial.** El sistema deberá permitir registrar indisponibilidad solo durante determinadas horas.

**CA-34.4 — Motivo.** El proveedor deberá poder indicar un motivo, por ejemplo incapacidad, permiso, ausencia u otro.

**CA-34.5 — Aplicación para nuevas asignaciones.** Una vez registrada la indisponibilidad, el proveedor no deberá aparecer disponible para nuevas asignaciones durante ese intervalo.

**CA-34.6 — Revisar tratamientos existentes.** TZISCA deberá comprobar automáticamente si el proveedor tiene tratamientos ya asignados dentro del periodo.

**CA-34.7 — Sin tratamientos afectados.** Si no existen tratamientos asignados, la indisponibilidad deberá registrarse sin iniciar un proceso de reasignación.

**CA-34.8 — Con tratamientos afectados.** Si existen tratamientos afectados, TZISCA deberá iniciar automáticamente la búsqueda de sustitutos.

**CA-34.9 — Historial.** La indisponibilidad deberá conservarse en el historial del proveedor.

## CU-35 — Buscar proveedor sustituto automáticamente

**CA-35.1 — Activación automática.** Dado que un proveedor registra indisponibilidad y tiene un tratamiento asignado dentro del periodo, TZISCA deberá iniciar automáticamente la búsqueda de un sustituto.

**CA-35.2 — Compatibilidad.** Solo deberán considerarse proveedores autorizados para realizar el mismo tratamiento.

**CA-35.3 — Estado activo.** Los proveedores desactivados no deberán participar en la búsqueda.

**CA-35.4 — Disponibilidad completa.** El proveedor candidato deberá estar disponible durante todo el intervalo del tratamiento.

**CA-35.5 — Sin traslapes.** Un proveedor que tenga otra atención que se traslape con el horario deberá ser descartado.

**CA-35.6 — Indisponibilidad registrada.** Un proveedor con indisponibilidad durante ese periodo también deberá descartarse.

**CA-35.7 — Encontrar sustituto.** Si existe al menos un proveedor válido, TZISCA deberá seleccionar o proponer uno para mantener el servicio.

**CA-35.8 — Mantener fecha y hora.** La propuesta deberá intentar conservar el mismo tratamiento, la misma fecha, la misma hora y la misma cabina.

**CA-35.9 — Informar al cliente.** El cambio no deberá realizarse de forma silenciosa. El cliente deberá ser informado de que el proveedor original no estará disponible.

**CA-35.10 — Opciones del cliente.** El cliente deberá poder aceptar al proveedor sustituto, cancelar solo el tratamiento afectado o cancelar toda la reservación.

**CA-35.11 — Aceptación del sustituto.** Si el cliente acepta, el nuevo proveedor deberá quedar asignado al tratamiento.

**CA-35.12 — Actualización de agenda.** El tratamiento deberá aparecer en la agenda del nuevo proveedor y dejar de aparecer como pendiente en la agenda del proveedor anterior.

**CA-35.13 — Historial de reasignación.** TZISCA deberá conservar registro del proveedor anterior, proveedor sustituto, fecha y motivo de la reasignación.

## CU-36 — Gestionar caso sin proveedor sustituto

**CA-36.1 — Sin opciones disponibles.** Si TZISCA no encuentra ningún proveedor compatible y disponible, deberá identificar el tratamiento como afectado.

**CA-36.2 — Informar al cliente.** El cliente deberá ser informado de que no se encontró un proveedor sustituto para conservar el servicio en ese horario.

**CA-36.3 — No reasignar proveedor inválido.** El sistema nunca deberá asignar automáticamente a un proveedor incompatible o con conflicto de horario solo para mantener la reservación.

**CA-36.4 — Cancelación individual.** El cliente deberá poder cancelar únicamente el tratamiento afectado.

**CA-36.5 — Cancelación total.** También deberá poder cancelar toda la reservación.

**CA-36.6 — Liberar recursos.** Si se cancela el tratamiento, deberán liberarse la cabina, horario y cualquier asignación pendiente.

**CA-36.7 — Mantener tratamientos restantes.** Si el cliente cancela solo el tratamiento afectado, los demás deberán conservarse.

## CU-37 — Consultar historial de indisponibilidades

**CA-37.1 — Historial propio.** El proveedor deberá poder consultar sus periodos de indisponibilidad registrados.

**CA-37.2 — Información del registro.** Cada elemento deberá mostrar al menos fecha de inicio, fecha de finalización, horario cuando aplique, motivo y fecha en que fue registrado.

**CA-37.3 — Acceso administrativo.** El Administrador general también podrá consultar esta información para seguimiento operativo.

**CA-37.4 — No eliminación histórica.** Una indisponibilidad pasada no deberá desaparecer automáticamente del historial.

## CU-38 — Consultar historial de atención

**CA-38.1 — Consultar servicios anteriores.** El proveedor deberá poder acceder a un historial de tratamientos atendidos.

**CA-38.2 — Información mostrada.** Cada registro podrá mostrar tratamiento, fecha, cabina, hora de inicio, hora de finalización y estado.

**CA-38.3 — Filtrar por fecha.** El proveedor deberá poder consultar su historial por periodo o fecha.

**CA-38.4 — Solo información propia.** Un proveedor solo podrá consultar su propio historial de atención.

**CA-38.5 — Tratamientos completados.** Los tratamientos Completados deberán conservarse en este historial.

**CA-38.6 — Diferenciar agenda e historial.** Los tratamientos pendientes o futuros deberán aparecer en la agenda; los ya atendidos deberán quedar disponibles en el historial.

# 5. Criterios de aceptación — Pagos y devoluciones

Los siguientes criterios validan el proceso de pago asociado a Reservaciones EN_PROCESO, la consulta y gestión de operaciones, los pagos de reservaciones manuales y las devoluciones. Los estados de Pago permitidos son Pendiente, Procesando, Pagado, Fallido, Cancelado, Reembolsado y Reembolsado parcialmente.

## CU-39 — Realizar pago de reservación

CA-39.1 — Reservación identificada. Dada una solicitud de pago, cuando TZISCA la valide, entonces deberá existir una Reservacion en estado EN_PROCESO con id_reservacion y al menos un ReservacionTratamiento en estado PENDIENTE.

CA-39.2 — Importe calculado por backend. Antes de crear el Pago, TZISCA deberá obtener del backend el importe calculado conforme a la política aprobada, mostrarlo al Cliente e impedir que el valor enviado por el Cliente lo sustituya.

CA-39.3 — Creación del pago. Cuando el Cliente inicie el cobro, TZISCA deberá crear un Pago en estado Pendiente relacionado con id_reservacion y después cambiarlo a Procesando al enviar la operación.

CA-39.4 — Estados permitidos. Todo Pago deberá usar exclusivamente Pendiente, Procesando, Pagado, Fallido, Cancelado, Reembolsado o Reembolsado parcialmente; un rechazo externo deberá registrarse como causa de Fallido.

CA-39.5 — Pago aprobado. Dado que el mecanismo de pago apruebe la operación, cuando TZISCA procese la respuesta, entonces deberá cambiar el Pago a Pagado, conservar monto, fecha, hora, método y referencia cuando exista, y enviar la Reservacion a CU-11 para revalidación.

CA-39.6 — Pago fallido. Dado un rechazo externo o error, cuando TZISCA procese la respuesta, entonces deberá cambiar el Pago a Fallido, informar al Cliente, conservar la trazabilidad del intento y no confirmar la Reservacion.

CA-39.7 — Reintento. Dado un Pago Fallido y bloqueos temporales vigentes, cuando el Cliente solicite reintentar, entonces TZISCA deberá permitir un nuevo intento relacionado con la misma Reservacion sin borrar el intento anterior.

CA-39.8 — Pago cancelado. Dado que el Cliente cancele la operación, cuando TZISCA registre el resultado, entonces el Pago deberá quedar Cancelado y la Reservacion deberá permanecer EN_PROCESO sin confirmarse.

CA-39.9 — Bloqueo expirado. Dado que un bloqueo temporal expire antes de la confirmación, cuando TZISCA continúe el flujo, entonces deberá liberarlo, revalidar la disponibilidad y no confirmar automáticamente.

CA-39.10 — Disponibilidad perdida antes del pago aprobado. Si la revalidación detecta que un recurso ya no está disponible y el Pago no está Pagado, TZISCA deberá impedir la confirmación e informar al Cliente para que modifique o elimine el tratamiento afectado.

CA-39.11 — Pago aprobado con disponibilidad perdida. Dado un Pago Pagado y una revalidación fallida, cuando TZISCA identifique el tratamiento afectado, entonces no deberá confirmarlo, deberá mantener la Reservacion en EN_PROCESO y ofrecer al Cliente, conforme a DP-EC-02 (RN-105), las opciones de seleccionar otra cabina u horario disponible, conservar los tratamientos válidos con devolución parcial del afectado, o cancelar la operación con devolución total, conservando la trazabilidad y sin crear ni presumir una devolución automática.

CA-39.12 — Prevención de cobro duplicado. Dada una respuesta repetida para la misma operación externa, cuando TZISCA la reciba, entonces no deberá crear un segundo cobro y deberá conservar una sola referencia económica con su trazabilidad técnica.

CA-39.13 — Prevención de confirmación duplicada. Dado un Pago Pagado ya procesado para una Reservacion confirmada, cuando se repita la respuesta o solicitud, entonces TZISCA no deberá confirmar otra vez ni duplicar ocupaciones.

CA-39.14 — Datos sensibles. Durante cualquier intento, TZISCA no deberá almacenar números completos de tarjeta, CVV, contraseñas bancarias ni otros datos que correspondan al mecanismo de pago.

## CU-40 — Consultar estado y detalle del pago

CA-40.1 — Consultar estado. Dado un Cliente autenticado y una Reservacion propia con Pago, cuando consulte el detalle, entonces TZISCA deberá mostrar el estado actual del Pago.

CA-40.2 — Estados normalizados. El estado mostrado deberá ser uno de los siguientes: Pendiente, Procesando, Pagado, Fallido, Cancelado, Reembolsado o Reembolsado parcialmente.

CA-40.3 — Información del pago. Cuando exista una operación registrada, el detalle deberá mostrar importe, fecha, método, estado y referencia cuando esté disponible.

CA-40.4 — Relación con reservación. El detalle del Pago deberá identificar la Reservacion a la que corresponde mediante su relación registrada.

CA-40.5 — Privacidad. Cuando un Cliente solicite el Pago de una Reservacion ajena, TZISCA deberá denegar la consulta y no exponer información sensible completa del medio de pago.

CA-40.6 — Estado no aprobado. Si el Pago está Pendiente, Procesando, Fallido o Cancelado, TZISCA deberá mostrar ese estado real y no presentarlo como Pagado.

CA-40.7 — Devoluciones relacionadas. Cuando exista una Devolucion, el detalle deberá mostrar por separado su tipo PARCIAL o TOTAL y su estado PENDIENTE, PROCESANDO, COMPLETADA, FALLIDA o CANCELADA, sin eliminar el Pago original.

## CU-41 — Consultar y gestionar pagos

CA-41.1 — Acceso administrativo. Dado un usuario con rol Administrador general, cuando ingrese al módulo de pagos, entonces deberá poder consultar las operaciones registradas.

CA-41.2 — Información mínima. Cada Pago deberá mostrar Reservacion, Cliente, importe, fecha, método, estado, referencia e intentos cuando existan.

CA-41.3 — Estados normalizados. Los filtros y resultados deberán usar únicamente Pendiente, Procesando, Pagado, Fallido, Cancelado, Reembolsado o Reembolsado parcialmente.

CA-41.4 — Rechazo externo. Cuando una operación externa sea rechazada, TZISCA deberá mostrar Fallido como estado del Pago y conservar el rechazo como causa o detalle de la operación.

CA-41.5 — Filtros. El Administrador deberá poder filtrar por estado y fecha o periodo, y buscar por id_reservacion o referencia cuando exista.

CA-41.6 — Detalle. Al seleccionar un Pago, TZISCA deberá mostrar su historial de intentos y las Devoluciones relacionadas, si existen.

CA-41.7 — Conservación histórica. Los registros de Pago en estado Fallido, Cancelado, Reembolsado o Reembolsado parcialmente no deberán eliminarse físicamente.

CA-41.8 — Consistencia. La gestión administrativa no deberá permitir marcar una Reservacion como pagada o confirmada sin un Pago Pagado registrado y la revalidación de disponibilidad correspondiente.

CA-41.9 — Auditoría. Toda observación, validación o acción administrativa deberá conservar el usuario responsable, la fecha, la hora y el motivo u observación.

## CU-42 — Registrar o validar pago en reservación manual

CA-42.1 — Acceso de Recepción. Dado un usuario con rol Recepción y cabinas, cuando consulte una reservación manual, entonces deberá poder registrar o validar el Pago según la modalidad operativa aprobada.

CA-42.2 — Precondición. Antes de registrar o validar el Pago, deberá existir una Reservacion manual EN_PROCESO con id_reservacion, al menos un ReservacionTratamiento PENDIENTE y un importe calculado por backend.

CA-42.3 — Relación obligatoria. Todo Pago registrado o validado deberá quedar relacionado con id_reservacion.

CA-42.4 — Estados normalizados. El Pago deberá usar exclusivamente Pendiente, Procesando, Pagado, Fallido, Cancelado, Reembolsado o Reembolsado parcialmente; un rechazo externo deberá registrarse como causa de Fallido.

CA-42.5 — Datos del registro. Cuando Recepción registre o valide una operación, TZISCA deberá conservar importe, fecha, hora, método, estado, referencia cuando exista y usuario responsable.

CA-42.6 — Pago no aprobado. Si el Pago está Pendiente, Procesando, Fallido o Cancelado, TZISCA deberá mantener la Reservacion EN_PROCESO y sus tratamientos PENDIENTE, y no deberá confirmarlos.

CA-42.7 — Pago aprobado. Si el Pago queda Pagado, TZISCA deberá regresar a CU-23 para revalidar disponibilidad; el estado Pagado no deberá confirmar por sí mismo la Reservacion.

CA-42.8 — Historial. Todo cambio de estado deberá conservar los movimientos anteriores necesarios para seguimiento y no deberá duplicar un Pago previamente registrado para la misma operación.

## CU-43 — Procesar devolución por cancelación

CA-43.1 — Condiciones de inicio. Dada una cancelación con un Pago Pagado, cuando una política aprobada autorice devolución y determine un importe reembolsable mayor que cero, entonces TZISCA deberá permitir iniciar CU-43.

CA-43.2 — Tipo de devolución. Al registrar la Devolucion, TZISCA deberá guardar el tipo PARCIAL o TOTAL según el alcance económico determinado y deberá mantenerlo separado del estado de procesamiento.

CA-43.3 — Estado inicial. Al crear la Devolucion, su estado deberá ser PENDIENTE y deberá quedar relacionada con el Pago y la Reservacion; para tipo PARCIAL también deberá relacionarse con el ReservacionTratamiento afectado cuando corresponda.

CA-43.4 — Inicio del procesamiento. Cuando comience el reembolso, TZISCA deberá cambiar la Devolucion de PENDIENTE a PROCESANDO y registrar la fecha, referencia y responsable cuando existan.

CA-43.5 — Devolución completada. Cuando el reembolso concluya correctamente, TZISCA deberá cambiar la Devolucion a COMPLETADA y el Pago a Reembolsado, si el tipo es TOTAL, o a Reembolsado parcialmente, si el tipo es PARCIAL.

CA-43.6 — Devolución fallida. Cuando el reembolso no pueda completarse, TZISCA deberá cambiar la Devolucion a FALLIDA, conservar el Pago original y registrar el motivo o resultado disponible para seguimiento.

CA-43.7 — Devolución cancelada. Cuando una devolución se cancele antes de completarse, TZISCA deberá cambiarla a CANCELADA y conservar su historial.

CA-43.8 — Sin devolución permitida. Si no existe Pago Pagado, la política no autoriza devolución o el importe reembolsable es cero, TZISCA no deberá crear una Devolucion y deberá registrar el resultado de la evaluación.

CA-43.9 — Estados oficiales. El estado de Devolucion deberá ser exclusivamente PENDIENTE, PROCESANDO, COMPLETADA, FALLIDA o CANCELADA.

CA-43.10 — Trazabilidad. La Devolucion deberá conservar importe, fecha de solicitud, fecha de procesamiento cuando exista, estado, referencia, responsable y relación con el Pago original sin eliminarlo.

CA-43.11 — Límite reembolsable. Antes de completar una devolución, TZISCA deberá verificar que la suma de los importes de las Devoluciones en estado COMPLETADA no supere el importe efectivamente Pagado y disponible para reembolso.

CA-43.12 — Prevención de duplicados. Dada una solicitud repetida para la misma devolución, cuando TZISCA la procese, entonces no deberá registrar dos reembolsos por el mismo importe y concepto.

# 6. Criterios de aceptación — Decisiones aprobadas (RN-91 a RN-108)

Estos criterios verifican los valores y condiciones operativas, económicas y técnicas aprobadas mediante el documento *Decisiones Aprobadas TZISCA*, que sustituyen el estado pendiente de DP-OP-01 a DP-OP-13, DP-EC-01 a DP-EC-02 y DP-TEC-01 a DP-TEC-03.

CA-91.1 — Apertura general a las 09:00. Dado un horario anterior a las 09:00, cuando el Cliente o Recepción consulten disponibilidad, entonces TZISCA no deberá ofrecerlo como horario reservable.

CA-92.1 — Cierre general a las 20:00 y finalización máxima. Dado un tratamiento cuya hora de inicio más su duración excedería las 20:00, cuando se calcule su disponibilidad, entonces TZISCA no deberá ofrecer ese horario como válido.

CA-93.1 — Días laborales de lunes a sábado. Dado un día comprendido entre lunes y sábado, cuando se consulte disponibilidad, entonces TZISCA deberá tratarlo como día laborable regular.

CA-93.2 — Domingo no laboral. Dada una fecha en domingo, cuando el Cliente o Recepción intenten reservar, entonces TZISCA no deberá ofrecer horarios disponibles, salvo que exista una excepción operativa registrada para esa fecha.

CA-94.1 — Excepciones operativas. Dado un festivo, cierre extraordinario u horario especial registrado como excepción operativa, cuando se consulte disponibilidad para esa fecha, entonces TZISCA deberá aplicar la excepción en lugar del calendario regular.

CA-95.1 — Intervalos de agenda de 30 minutos. Dado que TZISCA genera los horarios de inicio ofrecidos al Cliente, cuando construya la agenda disponible, entonces deberá hacerlo en intervalos de 30 minutos sin alterar la duración real del tratamiento.

CA-96.1 — Anticipación mínima de 2 horas. Dado que un Cliente intenta reservar con menos de 2 horas de anticipación respecto al inicio del servicio, cuando envíe la solicitud, entonces TZISCA deberá rechazarla.

CA-97.1 — Anticipación máxima de 60 días. Dado que un Cliente intenta reservar con más de 60 días de anticipación, cuando envíe la solicitud, entonces TZISCA deberá rechazarla.

CA-98.1 — Bloqueo temporal de 15 minutos. Dado que TZISCA crea un bloqueo temporal, cuando calcule su expiración, entonces deberá fijarla en 15 minutos desde la creación o renovación válida del bloqueo (DP-OP-08 / RN-98).

CA-99.1 — Tolerancia de 15 minutos. Dado un Cliente que llega hasta 15 minutos después del inicio programado, cuando Recepción evalúe la situación, entonces deberá verificar si el servicio todavía puede realizarse sin afectar reservaciones posteriores antes de considerarlo inasistencia para efectos de devolución.

CA-101.1 — Devolución del 100%. Dado que una cancelación se realiza con 24 horas o más de anticipación respecto al inicio del servicio y existe un Pago Pagado, cuando TZISCA calcule el importe reembolsable, entonces deberá determinar una devolución del 100%.

CA-101.2 — Devolución del 50%. Dado que una cancelación se realiza con al menos 6 horas y menos de 24 horas de anticipación y existe un Pago Pagado, cuando TZISCA calcule el importe reembolsable, entonces deberá determinar una devolución del 50%.

CA-101.3 — Sin devolución por anticipación insuficiente. Dado que una cancelación se realiza con menos de 6 horas de anticipación, cuando TZISCA evalúe la devolución, entonces no deberá determinar ningún importe reembolsable.

CA-102.1 — Cancelación por causa atribuible al spa. Dado que Administrador general o Recepción y cabinas cancelan por causa operativa atribuible al spa, cuando TZISCA calcule la devolución, entonces deberá determinar el 100% del importe afectado, independientemente de la anticipación.

CA-103.1 — Sin devolución por inasistencia o servicio iniciado/completado. Dado que la cancelación corresponde a una inasistencia, o el servicio ya inició o se completó, cuando TZISCA evalúe la devolución, entonces no deberá crear ninguna Devolucion.

CA-104.1 — Precio por persona. Dado un Tratamiento con precio_base definido, cuando se agregue a un ReservacionTratamiento, entonces TZISCA deberá fijar precio_unitario igual al precio_base vigente al momento de reservar.

CA-104.2 — Cálculo del importe y del total. Dado un ReservacionTratamiento con precio_unitario y numero_personas, cuando TZISCA calcule su importe, entonces deberá aplicar importe = precio_unitario × numero_personas, y el total de la Reservacion deberá ser la suma de los importes de todos sus tratamientos, sin cargos adicionales en el MVP.

CA-105.1 — Pago aprobado con disponibilidad perdida. Dado un Pago en estado Pagado cuya revalidación detecta pérdida de disponibilidad, cuando TZISCA procese el conflicto, entonces no deberá confirmar la Reservacion, deberá mantenerla en EN_PROCESO e informar al Cliente las tres opciones aprobadas: seleccionar otra cabina u horario disponible, conservar los tratamientos válidos con devolución parcial del afectado, o cancelar la operación con devolución total.

CA-106.1 — Autenticación con Identity y JWT. Dado un usuario con credenciales válidas, cuando inicie sesión, entonces TZISCA deberá emitir un access token JWT y permitir su renovación mediante un refresh token seguro, aplicando autorización por rol y permisos.

CA-106.2 — Rechazo por rol o permiso insuficiente. Dado un usuario autenticado sin el rol o permiso requerido, cuando intente acceder a una función restringida, entonces TZISCA deberá denegar el acceso.

CA-107.1 — Pasarela Stripe desacoplada mediante PaymentService. Dado que TZISCA procesa un pago, cuando se comunique con el proveedor externo, entonces deberá hacerlo a través de PaymentService, sin acoplar la lógica de negocio directamente a Stripe.

CA-108.1 — Recomendación determinista y reproducible. Dado el mismo conjunto de datos de cabinas, tratamiento y preferencias, cuando TZISCA genere la recomendación en distintos momentos, entonces deberá producir siempre el mismo resultado, aplicando en orden compatibilidad, capacidad, estado operativo, disponibilidad, preferencias, especialización, prioridad configurada e id_cabina ascendente como desempate final.

# 7. Nota para revisión

Estos criterios son la base verificable para las pruebas funcionales de CU-01 a CU-43. Las decisiones operativas, económicas y técnicas antes pendientes (DP-OP-01 a DP-OP-13, DP-EC-01 a DP-EC-02, DP-TEC-01 a DP-TEC-03) ya fueron aprobadas y sus valores se encuentran verificados en la sección 6; cualquier nueva decisión pendiente que surja en el futuro deberá aprobarse antes de asignar valores o automatismos que no estén definidos en las reglas de negocio oficiales.
