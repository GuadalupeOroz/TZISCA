**TZISCA**

**Reglas de Negocio**

*Sistema de Reservas, Recomendación y Gestión de Cabinas para Spa*

Este documento reúne las reglas de negocio definidas para TZISCA a partir del alcance funcional actualizado y de los casos de uso CU-01 a CU-43. Las reglas establecen las condiciones que debe cumplir el sistema para administrar usuarios, tratamientos, cabinas, carrito, disponibilidad, reservaciones, pagos, devoluciones, proveedores, cancelaciones, historial y reportes de forma consistente.

# 1. Usuarios y roles

**RN-01.** Todo usuario deberá iniciar sesión para acceder a funciones privadas del sistema.

**RN-02.** Cada usuario tendrá permisos asociados a su rol.

**RN-03.** Los roles definidos serán Cliente, Administrador general, Recepción y cabinas y Proveedor de tratamiento.

**RN-04.** El Cliente solo podrá consultar y gestionar sus propias reservaciones.

**RN-05.** El Administrador general tendrá acceso a la configuración general, usuarios, tratamientos, cabinas, proveedores, asignaciones y reportes.

**RN-06.** Recepción y cabinas tendrá acceso a funciones operativas de agenda, disponibilidad, reservaciones manuales, cancelaciones y estados de cabina.

**RN-07.** El Proveedor solo podrá consultar los tratamientos que tenga asignados.

# 2. Tratamientos

**RN-08.** Todo tratamiento deberá tener nombre, descripción, duración y estado.

**RN-09.** Solo los tratamientos activos podrán mostrarse al cliente.

**RN-10.** Un tratamiento podrá relacionarse con una o varias cabinas compatibles.

**RN-11.** El mismo tratamiento podrá agregarse varias veces a una misma reservación; cada elemento será gestionado de forma independiente.

# 3. Cabinas

**RN-12.** Toda cabina tendrá una capacidad máxima definida.

**RN-13.** Una cabina solo podrá utilizarse para tratamientos con los que sea compatible.

**RN-14.** La capacidad de la cabina deberá ser igual o superior al número de personas del tratamiento.

**RN-15.** Una cabina En mantenimiento, Fuera de servicio o Desactivada no podrá ofrecerse para nuevas reservaciones.

**RN-16.** Una cabina no podrá atender dos tratamientos cuyos intervalos de tiempo se traslapen.

# 4. Recomendación de cabinas

**RN-17.** TZISCA solo recomendará cabinas que cumplan primero compatibilidad con el tratamiento, capacidad suficiente, estado operativo válido y disponibilidad.

**RN-18.** Una vez cumplidos los criterios obligatorios, el sistema podrá utilizar las preferencias del cliente para ordenar las recomendaciones.

**RN-19.** Si el cliente no proporciona preferencias, la recomendación funcionará mediante compatibilidad, capacidad, disponibilidad y especialización.

**RN-20.** La cabina recomendada será orientativa; el cliente podrá seleccionar otra cabina compatible.

**RN-21.** Si existen dos cabinas con condiciones equivalentes, podrá darse prioridad a una cabina especializada sobre una multifuncional.

# 5. Carrito de reservación

**RN-22.** Una reservación podrá contener uno o varios tratamientos.

RN-23. Cada tratamiento del carrito será administrado como un elemento independiente con número de personas, cabina, fecha y hora.

**RN-24.** Los tratamientos de una misma reservación no estarán obligados a realizarse de manera consecutiva.

**RN-25.** Antes de confirmar, el cliente podrá agregar, eliminar o modificar tratamientos.

**RN-26.** Agregar un tratamiento al carrito no significa que esté reservado definitivamente.

# 6. Disponibilidad

**RN-27.** La disponibilidad deberá calcularse considerando toda la duración del tratamiento y no únicamente la hora de inicio.

**RN-28.** Un horario se considerará disponible únicamente si no existe ningún traslape con reservaciones confirmadas, bloqueos temporales, mantenimiento, bloqueos operativos o periodos fuera de servicio.

**RN-29.** Si un horario no está disponible, TZISCA deberá permitir seleccionar otro horario disponible.

# 7. Bloqueo temporal

**RN-30.** Cuando un cliente seleccione una cabina, fecha y hora válidas, TZISCA realizará un bloqueo temporal.

**RN-31.** La duración del bloqueo temporal deberá manejarse como un parámetro operativo configurable (ParametroOperativo.duracion_bloqueo_minutos). El valor aprobado es de 15 minutos, conforme a DP-OP-08 (ver RN-98).

**RN-32.** Mientras exista el bloqueo temporal, el recurso no podrá ofrecerse a otro cliente si genera conflicto.

RN-33. Si la reservación requiere pago, el bloqueo temporal deberá mantenerse vigente durante el proceso de pago y solo se convertirá en ocupación real cuando el pago sea aprobado y la reservación quede confirmada.

RN-34. Si el bloqueo temporal expira antes de completar la confirmación o el pago, el recurso deberá liberarse automáticamente y TZISCA deberá volver a validar la disponibilidad antes de continuar.

**RN-35.** Si el cliente abandona el carrito, los bloqueos temporales deberán liberarse.

**RN-36.** Si el cliente cambia fecha, hora o cabina, TZISCA liberará inmediatamente el bloqueo anterior antes de intentar crear el nuevo.

**RN-37.** Cuando existan varios tratamientos en el carrito, cada uno podrá tener su propio recurso bloqueado temporalmente.

# 8. Confirmación de reservaciones

RN-38. Al generar una reservación previa al pago, Reservacion deberá quedar en estado EN_PROCESO y cada ReservacionTratamiento deberá quedar en estado PENDIENTE.

RN-39. Una Reservacion en estado EN_PROCESO podrá confirmarse sin aprobación manual de Recepción y cabinas cuando todos sus recursos sean válidos, se haya revalidado su disponibilidad y, cuando corresponda, el pago haya sido aprobado.

RN-40. Después de un pago aprobado y de la revalidación satisfactoria de disponibilidad, cada ReservacionTratamiento válido pasará de PENDIENTE a CONFIRMADO. Un pago fallido, cancelado o rechazado por el mecanismo externo no producirá esta transición.

**RN-41.** Si uno de varios tratamientos presenta un conflicto antes de confirmar, los demás no deberán cancelarse automáticamente. El cliente podrá conservar los válidos, modificar o eliminar el afectado, o cancelar todo el proceso.

# 9. Estados de tratamientos

RN-42. Cada ReservacionTratamiento deberá manejar uno de los siguientes estados: PENDIENTE, CONFIRMADO, CANCELADO, EN_ATENCION o COMPLETADO. PENDIENTE será el estado inicial de los tratamientos de una Reservacion generada antes del pago.

RN-43. Un ReservacionTratamiento en estado CANCELADO no podrá pasar posteriormente a EN_ATENCION o COMPLETADO.

RN-44. Un ReservacionTratamiento en estado CONFIRMADO podrá pasar a EN_ATENCION cuando el proveedor inicie el servicio.

RN-45. Un ReservacionTratamiento en estado EN_ATENCION podrá pasar a COMPLETADO cuando el proveedor finalice el servicio.

# 10. Proveedores

**RN-46.** Un proveedor deberá estar relacionado con los tratamientos que está autorizado para realizar.

**RN-47.** La asignación inicial del proveedor será realizada exclusivamente por el Administrador general.

**RN-48.** Un proveedor no podrá ser asignado a dos tratamientos cuyos intervalos se traslapen.

**RN-49.** El proveedor solo podrá consultar los tratamientos que tenga asignados.

**RN-50.** El proveedor podrá registrar periodos de indisponibilidad.

**RN-51.** Durante un periodo de indisponibilidad, el proveedor no deberá aparecer como opción para nuevas asignaciones.

# 11. Reasignación automática

**RN-52.** Si un proveedor registra indisponibilidad y tiene tratamientos asignados, TZISCA deberá buscar automáticamente un sustituto.

**RN-53.** El proveedor sustituto deberá estar activo, poder realizar el tratamiento y estar disponible durante toda la fecha y horario requerido.

**RN-54.** Si existe un proveedor sustituto, el cliente deberá ser informado antes de realizar el cambio definitivo.

**RN-55.** El cliente podrá aceptar al nuevo proveedor, cancelar solamente el tratamiento afectado o cancelar toda la reservación.

**RN-56.** Si no existe proveedor sustituto, TZISCA deberá informar al cliente.

# 12. Cancelaciones

RN-57. El Cliente podrá cancelar uno de sus tratamientos o toda su reservación conforme a las políticas del spa; antes de finalizar la cancelación, TZISCA deberá verificar si existe un pago relacionado y si corresponde una devolución.

**RN-58.** Recepción y cabinas será el rol principalmente responsable de realizar cancelaciones operativas.

**RN-59.** El Administrador general también tendrá permisos de cancelación.

RN-60. Cuando se cancele un tratamiento, TZISCA deberá liberar automáticamente cabina, horario y proveedor asignado, sin eliminar el registro de pago o devolución que pudiera estar relacionado.

**RN-61.** Cancelar un tratamiento no deberá cancelar automáticamente los demás tratamientos de la reservación.

**RN-62.** Si el cliente desea modificar una reservación ya confirmada, deberá cancelar el tratamiento correspondiente o la reservación y realizar una nueva.

# 13. Operación de cabinas

**RN-63.** Recepción y cabinas podrá bloquear una cabina durante un intervalo específico o un día completo.

**RN-64.** Todo bloqueo operativo deberá tener una fecha y un motivo.

**RN-65.** Si se intenta bloquear una cabina que ya tiene reservaciones confirmadas durante ese periodo, el sistema deberá advertirlo.

**RN-66.** Recepción y cabinas podrá cambiar el estado operativo de las cabinas.

**RN-67.** La desactivación permanente de una cabina será responsabilidad principalmente del Administrador general.

# 14. Historial y trazabilidad

**RN-68.** TZISCA deberá conservar el historial de acciones relevantes, incluyendo cancelaciones, cambios de estado, bloqueos, mantenimiento, asignación y reasignación de proveedores, e inicio y finalización de atención.

**RN-69.** En cada movimiento relevante se deberá registrar al menos el usuario que realizó la acción, la fecha, la hora y la acción realizada.

**RN-70.** Cuando corresponda, también deberá registrarse un motivo u observación.

# 15. Reportes

**RN-71.** Los reportes deberán generarse utilizando información registrada por el sistema e inicialmente podrán incluir reservaciones por día, tratamientos más solicitados, cabinas más utilizadas, cancelaciones y ocupación general de cabinas.

**RN-72.** El Administrador general será el rol principal autorizado para consultar reportes.

# 16. Pagos y devoluciones

RN-73. Todo Pago deberá estar relacionado con una Reservacion. Cuando el pago sea previo a la confirmación definitiva, la Reservacion relacionada deberá permanecer en estado EN_PROCESO y sus ReservacionTratamiento en estado PENDIENTE.

RN-74. El sistema deberá calcular y mostrar el importe correspondiente antes de iniciar el proceso de pago, conforme a la fórmula aprobada en DP-EC-01 (ver RN-104).

RN-75. Una Reservacion que requiera pago no deberá considerarse definitivamente confirmada hasta que el pago haya sido aprobado y la revalidación de disponibilidad resulte satisfactoria. Solo entonces sus ReservacionTratamiento podrán pasar de PENDIENTE a CONFIRMADO.

RN-76. Todo pago deberá manejar un estado controlado.

RN-77. Los estados permitidos para Pago serán exclusivamente PENDIENTE, PROCESANDO, PAGADO, FALLIDO, CANCELADO, REEMBOLSADO y REEMBOLSADO_PARCIALMENTE. Un resultado externo rechazado deberá registrarse funcionalmente como FALLIDO y no constituirá un estado adicional.

RN-78. Un Pago en estado PAGADO deberá registrar monto, fecha, hora, método de pago y referencia de la operación.

RN-79. Cuando una operación de pago falle o sea rechazada por el mecanismo externo, el Pago deberá quedar en estado FALLIDO y no deberá marcar la Reservacion como pagada ni producir la transición de PENDIENTE a CONFIRMADO.

RN-80. El Cliente podrá reintentar un Pago en estado FALLIDO mientras la operación y el bloqueo temporal continúen vigentes.

RN-81. El sistema deberá volver a validar la disponibilidad si el bloqueo temporal expira antes de completar la confirmación. Si el pago ya fue aprobado y la disponibilidad se perdió, la Reservacion no deberá confirmarse y permanecerá EN_PROCESO conforme al tratamiento económico aprobado en DP-EC-02 (ver RN-105).

RN-82. TZISCA no deberá almacenar datos bancarios sensibles completos como número completo de tarjeta o CVV.

RN-83. Una cancelación deberá verificar si existe un pago relacionado.

RN-84. La cancelación de un tratamiento podrá generar una devolución parcial según las políticas definidas.

RN-85. La cancelación completa podrá generar una devolución total según las políticas definidas.

RN-86. Toda devolución deberá conservar trazabilidad.

RN-87. Una devolución deberá registrar monto, fecha, motivo, estado y usuario responsable cuando corresponda.

RN-88. Los pagos no deberán eliminarse físicamente aunque una reservación sea cancelada.

RN-89. El Administrador general podrá consultar información de pagos según sus permisos.

RN-90. Recepción y cabinas podrá consultar el estado del pago de las reservaciones que gestione.

# 17. Reglas derivadas de las decisiones aprobadas (RN-91 a RN-108)

Estas reglas incorporan al cuerpo normativo de TZISCA las decisiones operativas, económicas y técnicas aprobadas mediante el documento *Decisiones Aprobadas TZISCA*. Sustituyen el estado de pendiente que tenían DP-OP-01 a DP-OP-13, DP-EC-01 a DP-EC-02 y DP-TEC-01 a DP-TEC-03 (sección 18) y ya pueden utilizarse para implementación.

**RN-91.** El horario general de apertura de TZISCA será a las 09:00.

**RN-92.** El horario general de cierre será a las 20:00 y ningún tratamiento deberá finalizar después de esa hora.

**RN-93.** Los días laborales regulares serán de lunes a sábado; el domingo será no laboral.

**RN-94.** Los festivos, cierres extraordinarios y horarios especiales deberán registrarse como excepciones operativas y prevalecerán sobre el calendario regular.

**RN-95.** Los horarios de inicio ofrecidos por la agenda se generarán en intervalos de 30 minutos, sin modificar la duración real de cada tratamiento.

**RN-96.** Una reservación deberá realizarse con al menos 2 horas de anticipación respecto del inicio del servicio.

**RN-97.** El Cliente podrá reservar como máximo con 60 días de anticipación.

**RN-98.** Todo bloqueo temporal tendrá una vigencia de 15 minutos; al expirar deberá liberarse o revalidarse según el flujo vigente.

**RN-99.** La tolerancia por llegada tardía será de 15 minutos; superado ese margen, Recepción determinará si el servicio puede realizarse sin afectar la agenda y, de no ser posible, se aplicará la política de inasistencia.

**RN-100.** Las cancelaciones podrán realizarse antes del inicio por el Cliente; Administrador general y Recepción y cabinas podrán cancelar por causas operativas registrando motivo y responsable.

**RN-101.** Una cancelación con 24 horas o más de anticipación dará derecho a devolución del 100%; entre 6 y menos de 24 horas, al 50%; con menos de 6 horas, no habrá devolución.

**RN-102.** Las cancelaciones por causa operativa atribuible al spa darán derecho a devolución del 100% del importe afectado.

**RN-103.** No habrá devolución por inasistencia, cancelación con menos de 6 horas, servicio ya iniciado o servicio completado.

**RN-104.** El precio_base de Tratamiento se interpretará como precio por persona; el importe de cada ReservacionTratamiento será precio_unitario × numero_personas y el total será la suma de sus importes. El MVP no aplicará cargos adicionales.

**RN-105.** Si existe Pago PAGADO y la disponibilidad se pierde antes de confirmar, la Reservacion no deberá confirmarse y permanecerá EN_PROCESO hasta resolver el conflicto mediante alternativa, devolución parcial o cancelación con devolución total.

**RN-106.** La autenticación se implementará con ASP.NET Core Identity y JWT, utilizando autorización por rol y renovación segura de sesión.

**RN-107.** Stripe será la pasarela inicial de pago y deberá integrarse mediante PaymentService desacoplado de la lógica de negocio.

**RN-108.** La recomendación de cabinas deberá ser determinista y reproducible, aplicando compatibilidad, capacidad, estado, disponibilidad, preferencias, especialización, prioridad configurada e id_cabina como desempate final.

# 18. Decisiones aprobadas

Esta sección documenta las decisiones operativas, económicas y técnicas que se encontraban pendientes de validación y que quedaron formalmente aprobadas mediante el documento *Decisiones Aprobadas TZISCA*. Los identificadores DP-OP, DP-EC y DP-TEC se conservan como trazabilidad histórica del proceso de aprobación; ya no representan pendientes y las reglas resultantes RN-91 a RN-108 (sección 17) son la referencia normativa vigente. Estas decisiones ya pueden utilizarse para implementación, incluyendo la configuración de AvailabilityService y RefundService.

## 18.1. Decisiones operativas aprobadas

**DP-OP-01 — Hora de apertura.** 09:00. *Impacto principal: AvailabilityService. Estado: Aprobada.*

**DP-OP-02 — Hora de cierre.** 20:00. Todo tratamiento deberá finalizar a más tardar a las 20:00. *Impacto principal: AvailabilityService. Estado: Aprobada.*

**DP-OP-03 — Días laborales.** Lunes a sábado. *Impacto principal: AvailabilityService. Estado: Aprobada.*

**DP-OP-04 — Días no laborales y excepciones.** Domingo no laboral. Festivos, cierres extraordinarios y horarios especiales se registrarán como excepciones operativas. *Impacto principal: AvailabilityService / ExcepcionOperativa. Estado: Aprobada.*

**DP-OP-05 — Duración de intervalos de agenda.** 30 minutos para los horarios de inicio ofrecidos al cliente. *Impacto principal: AvailabilityService. Estado: Aprobada.*

**DP-OP-06 — Anticipación mínima para reservar.** 2 horas antes del inicio del servicio. *Impacto principal: AvailabilityService. Estado: Aprobada.*

**DP-OP-07 — Anticipación máxima para reservar.** 60 días hacia el futuro. *Impacto principal: AvailabilityService. Estado: Aprobada.*

**DP-OP-08 — Duración del bloqueo temporal.** 15 minutos. La expiración deberá calcularse desde la creación o renovación válida del bloqueo. *Impacto principal: AvailabilityService / flujo de pago. Estado: Aprobada.*

**DP-OP-09 — Política de tolerancia.** 15 minutos de tolerancia por llegada tardía. Después de ese margen, Recepción evaluará si el servicio aún puede realizarse sin afectar reservaciones posteriores; de no ser posible, se considerará inasistencia para efectos de devolución. *Impacto principal: Reservaciones / operación. Estado: Aprobada.*

**DP-OP-10 — Política de cancelación.** El Cliente puede cancelar antes del inicio del servicio. Administrador general y Recepción y cabinas pueden cancelar por causas operativas, registrando motivo y responsable. *Impacto principal: RefundService / Reservaciones. Estado: Aprobada.*

**DP-OP-11 — Condiciones de devolución.** Cancelación con 24 horas o más: 100%. Entre 6 y menos de 24 horas: 50%. Menos de 6 horas: sin devolución. Si el spa cancela por causa operativa: 100%. *Impacto principal: RefundService. Estado: Aprobada.*

**DP-OP-12 — Tiempo límite para cancelar con devolución.** Se conserva derecho a devolución conforme a los tramos aprobados: 100% con 24 h o más; 50% entre 6 h y menos de 24 h; sin devolución con menos de 6 h. *Impacto principal: RefundService. Estado: Aprobada.*

**DP-OP-13 — Casos sin derecho a devolución.** Inasistencia, cancelación con menos de 6 horas, servicio ya iniciado o servicio completado. *Impacto principal: RefundService. Estado: Aprobada.*

## 18.2. Decisiones económicas aprobadas

**DP-EC-01 — Fórmula del importe.** Tratamiento.precio_base se interpreta como precio por persona. Para cada ReservacionTratamiento: precio_unitario = precio_base vigente al reservar; importe = precio_unitario × numero_personas. El total de la Reservacion es la suma de los importes de sus tratamientos. No se aplican cargos adicionales en el MVP. *Impacto principal: Tratamiento, ReservacionTratamiento, Pago. Estado: Aprobada.* Ver RN-104.

**DP-EC-02 — Pago aprobado con disponibilidad perdida.** Si el Pago está PAGADO pero la revalidación confirma pérdida de disponibilidad, la Reservacion NO se confirma. Permanece en EN_PROCESO mientras se resuelve el conflicto. El Cliente podrá: (a) seleccionar otra cabina/horario disponible; (b) conservar tratamientos válidos y solicitar devolución parcial del tratamiento afectado; o (c) cancelar la operación y recibir devolución total. Toda acción deberá conservar trazabilidad. No se genera una confirmación automática ni se asume una devolución automática sin decisión del Cliente. *Impacto principal: PaymentService / RefundService / Reservaciones. Estado: Aprobada.* Ver RN-105.

## 18.3. Decisiones técnicas aprobadas

**DP-TEC-01 — Mecanismo de autenticación.** ASP.NET Core Identity para gestión de usuarios y credenciales. La API utilizará JWT como access token y un mecanismo de refresh token seguro para renovación de sesión. La autorización se aplicará por rol y permisos. *Impacto principal: /auth, /users, seguridad API. Estado: Aprobada.* Ver RN-106.

**DP-TEC-02 — Proveedor o pasarela de pago.** Stripe será la pasarela inicial. La integración se realizará detrás de una abstracción PaymentService para no acoplar la lógica de negocio al proveedor y permitir sustitución futura. No se almacenarán datos bancarios sensibles completos. *Impacto principal: /payments, /refunds, TransaccionPago. Estado: Aprobada.* Ver RN-107.

**DP-TEC-03 — Algoritmo determinista de recomendación.** Orden de evaluación: 1) compatibilidad TratamientoCabina; 2) capacidad suficiente; 3) estado operativo permitido; 4) disponibilidad del intervalo completo; 5) preferencias del cliente cuando existan; 6) prioridad a cabina especializada frente a multifuncional cuando ambas cumplan; 7) desempate por prioridad configurada y, finalmente, id_cabina ascendente para garantizar reproducibilidad. *Impacto principal: RecommendationService. Estado: Aprobada.* Ver RN-108.

## 18.4. Condiciones para AvailabilityService

AvailabilityService deberá calcular disponibilidad utilizando los parámetros operativos aprobados en esta sección: horario de apertura (09:00) y cierre (20:00), días laborables (lunes a sábado) y no laborables (domingo, más excepciones operativas), duración de intervalos (30 minutos), anticipación mínima (2 horas) y máxima (60 días), vigencia de bloqueos temporales (15 minutos) y los bloqueos operativos ya definidos para cabinas.

Estos parámetros deberán implementarse como configuración (ParametroOperativo, DiaLaborable, ExcepcionOperativa) y no como constantes dispersas en el código.

La disponibilidad continuará respetando las reglas ya vigentes sobre duración completa del tratamiento, prevención de traslapes, compatibilidad, capacidad, estado operativo de la cabina y bloqueos existentes.

## 18.5. Condiciones para RefundService

RefundService deberá aplicar las políticas ya aprobadas en esta sección al determinar si corresponde una devolución: la política de cancelación (DP-OP-10), las condiciones de devolución por tramos de anticipación (DP-OP-11 y DP-OP-12) y los casos sin derecho a devolución (DP-OP-13), además del tratamiento económico de pago aprobado con disponibilidad perdida (DP-EC-02).

Antes de generar una devolución, RefundService deberá consultar el momento de la cancelación respecto al inicio del servicio, el pago original, el importe pagado, las devoluciones previas y el tratamiento o reservación afectados.

RefundService deberá permitir determinar de forma reproducible si corresponde devolución total, parcial o ninguna devolución y registrar el resultado conforme a las reglas de pagos y devoluciones ya definidas.

## 18.6. Criterio de cierre de estas decisiones

Esta sección queda cerrada. DP-OP-01 a DP-OP-13, DP-EC-01 a DP-EC-02 y DP-TEC-01 a DP-TEC-03 cuentan con un valor, fórmula, mecanismo o condición formalmente aprobada, están representadas en las reglas de negocio RN-91 a RN-108 (sección 17) y ya pueden convertirse en criterios de aceptación, casos de uso, modelo de datos, API y pruebas.

Nota de consistencia: el valor oficial de la duración del bloqueo temporal es de 15 minutos (DP-OP-08, RN-98). Cualquier referencia previa a un valor distinto, incluida la mención histórica de 10 minutos en versiones anteriores del documento, queda sustituida por este valor aprobado.

# 19. Resumen de reglas por área — Total: 108 reglas de negocio

| **Área**                                              | **Rango**      | **Cantidad** |
|--------------------------------------------------------|----------------|--------------|
| Usuarios y roles                                       | RN-01 a RN-07  | 7            |
| Tratamientos                                            | RN-08 a RN-11  | 4            |
| Cabinas                                                 | RN-12 a RN-16  | 5            |
| Recomendación de cabinas                                | RN-17 a RN-21  | 5            |
| Carrito de reservación                                  | RN-22 a RN-26  | 5            |
| Disponibilidad                                          | RN-27 a RN-29  | 3            |
| Bloqueo temporal                                        | RN-30 a RN-37  | 8            |
| Confirmación                                            | RN-38 a RN-41  | 4            |
| Estados                                                 | RN-42 a RN-45  | 4            |
| Proveedores                                             | RN-46 a RN-51  | 6            |
| Reasignación automática                                 | RN-52 a RN-56  | 5            |
| Cancelaciones                                           | RN-57 a RN-62  | 6            |
| Operación de cabinas                                    | RN-63 a RN-67  | 5            |
| Historial y trazabilidad                                | RN-68 a RN-70  | 3            |
| Reportes                                                | RN-71 a RN-72  | 2            |
| Pagos y devoluciones                                    | RN-73 a RN-90  | 18           |
| Decisiones aprobadas (horario, económicas y técnicas)   | RN-91 a RN-108 | 18           |
