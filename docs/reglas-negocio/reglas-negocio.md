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

**RN-31.** La duración del bloqueo temporal deberá manejarse como un parámetro operativo configurable. El valor inicial queda pendiente de validación y aprobación por el responsable del proyecto antes de su implementación.

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

RN-74. El sistema deberá calcular y mostrar el importe correspondiente antes de iniciar el proceso de pago. La fórmula de cálculo queda pendiente de aprobación conforme a DP-EC-01.

RN-75. Una Reservacion que requiera pago no deberá considerarse definitivamente confirmada hasta que el pago haya sido aprobado y la revalidación de disponibilidad resulte satisfactoria. Solo entonces sus ReservacionTratamiento podrán pasar de PENDIENTE a CONFIRMADO.

RN-76. Todo pago deberá manejar un estado controlado.

RN-77. Los estados permitidos para Pago serán exclusivamente PENDIENTE, PROCESANDO, PAGADO, FALLIDO, CANCELADO, REEMBOLSADO y REEMBOLSADO_PARCIALMENTE. Un resultado externo rechazado deberá registrarse funcionalmente como FALLIDO y no constituirá un estado adicional.

RN-78. Un Pago en estado PAGADO deberá registrar monto, fecha, hora, método de pago y referencia de la operación.

RN-79. Cuando una operación de pago falle o sea rechazada por el mecanismo externo, el Pago deberá quedar en estado FALLIDO y no deberá marcar la Reservacion como pagada ni producir la transición de PENDIENTE a CONFIRMADO.

RN-80. El Cliente podrá reintentar un Pago en estado FALLIDO mientras la operación y el bloqueo temporal continúen vigentes.

RN-81. El sistema deberá volver a validar la disponibilidad si el bloqueo temporal expira antes de completar la confirmación. Si el pago ya fue aprobado y la disponibilidad se perdió, la Reservacion no deberá confirmarse y el tratamiento económico quedará sujeto a DP-EC-02.

RN-82. TZISCA no deberá almacenar datos bancarios sensibles completos como número completo de tarjeta o CVV.

RN-83. Una cancelación deberá verificar si existe un pago relacionado.

RN-84. La cancelación de un tratamiento podrá generar una devolución parcial según las políticas definidas.

RN-85. La cancelación completa podrá generar una devolución total según las políticas definidas.

RN-86. Toda devolución deberá conservar trazabilidad.

RN-87. Una devolución deberá registrar monto, fecha, motivo, estado y usuario responsable cuando corresponda.

RN-88. Los pagos no deberán eliminarse físicamente aunque una reservación sea cancelada.

RN-89. El Administrador general podrá consultar información de pagos según sus permisos.

RN-90. Recepción y cabinas podrá consultar el estado del pago de las reservaciones que gestione.

# 17. Horario operativo y políticas de reservación

Esta sección consolida los parámetros operativos y las políticas que todavía deben ser validados por el responsable del proyecto o por la empresa antes de su implementación definitiva. Mientras no exista aprobación formal, TZISCA no deberá asumir valores por defecto ni incorporar constantes en código para estos puntos. Las decisiones deberán quedar definidas antes de cerrar la lógica de AvailabilityService y RefundService.

Estas decisiones no se numeran todavía como nuevas reglas de negocio RN-91 en adelante, porque aún no contienen valores ni condiciones aprobadas. Una vez validadas, deberán convertirse en reglas verificables y reflejarse de forma consistente en casos de uso, criterios de aceptación, modelo de datos, API y pruebas.

## 17.1. Decisiones pendientes de validación

**DP-OP-01 — Hora de apertura.** Definir la hora oficial a partir de la cual podrán ofrecerse horarios reservables. *Impacto principal: AvailabilityService. Estado: Pendiente de aprobación.*

**DP-OP-02 — Hora de cierre.** Definir la hora límite de operación y establecer si un tratamiento debe finalizar antes o exactamente al cierre. *Impacto principal: AvailabilityService. Estado: Pendiente de aprobación.*

**DP-OP-03 — Días laborales.** Definir qué días de la semana forman parte de la operación regular del spa. *Impacto principal: AvailabilityService. Estado: Pendiente de aprobación.*

**DP-OP-04 — Días no laborales.** Definir los días en los que no se deberán ofrecer reservaciones y el mecanismo para registrar cierres o excepciones operativas. *Impacto principal: AvailabilityService. Estado: Pendiente de aprobación.*

**DP-OP-05 — Duración de intervalos de agenda.** Definir la granularidad con la que se generarán o mostrarán los horarios disponibles para iniciar un tratamiento. *Impacto principal: AvailabilityService. Estado: Pendiente de aprobación.*

**DP-OP-06 — Anticipación mínima para reservar.** Definir cuánto tiempo antes del inicio del servicio debe realizarse una reservación. *Impacto principal: AvailabilityService. Estado: Pendiente de aprobación.*

**DP-OP-07 — Anticipación máxima para reservar.** Definir hasta qué fecha futura podrá reservar un cliente. *Impacto principal: AvailabilityService. Estado: Pendiente de aprobación.*

**DP-OP-08 — Tiempo de duración del bloqueo temporal.** Definir la vigencia exacta de un bloqueo temporal durante selección, confirmación y pago. *Impacto principal: AvailabilityService / flujo de pago. Estado: Pendiente de aprobación.*

**DP-OP-09 — Política de tolerancia.** Definir el margen permitido ante llegada tardía y las consecuencias operativas aplicables al servicio y a la reservación. *Impacto principal: Reservaciones / operación. Estado: Pendiente de aprobación.*

**DP-OP-10 — Política de cancelación.** Definir las condiciones bajo las cuales un cliente, Recepción y cabinas o el Administrador general pueden cancelar un tratamiento o una reservación. *Impacto principal: RefundService / Reservaciones. Estado: Pendiente de aprobación.*

**DP-OP-11 — Condiciones de devolución.** Definir cuándo una cancelación genera devolución total, parcial o ninguna devolución, así como los criterios que determinan el importe elegible. *Impacto principal: RefundService. Estado: Pendiente de aprobación.*

**DP-OP-12 — Tiempo límite para cancelar con devolución.** Definir la anticipación requerida respecto al inicio del servicio para que una cancelación conserve derecho a devolución. *Impacto principal: RefundService. Estado: Pendiente de aprobación.*

**DP-OP-13 — Casos sin derecho a devolución.** Definir las situaciones en las que una cancelación no genera devolución y cómo deberán registrarse para trazabilidad. *Impacto principal: RefundService. Estado: Pendiente de aprobación.*

## 17.2. Decisiones económicas pendientes

DP-EC-01 — Fórmula del importe de tratamiento. Definir la fórmula antes de implementar el cálculo o confirmar importes. Estado: Pendiente de aprobación.

- Determinar si precio_base corresponde al tratamiento completo.

- Determinar si precio_base corresponde por persona.

- Determinar si importe = precio_unitario × numero_personas.

- Determinar si existen cargos adicionales y, en su caso, cuáles son.

DP-EC-02 — Pago aprobado con bloqueo expirado y disponibilidad perdida. Definir el tratamiento económico cuando el pago haya sido aprobado, el bloqueo temporal haya expirado y la revalidación confirme que la disponibilidad se perdió. La Reservacion no deberá confirmarse mientras no exista disponibilidad. Estado: Pendiente de aprobación.

- Determinar si corresponde un proceso de conciliación.

- Determinar si corresponde una devolución.

- Determinar si corresponde cancelar la operación.

- Determinar si corresponde otro mecanismo formalmente aprobado.

## 17.3. Decisiones técnicas pendientes

DP-TEC-01 — Mecanismo de autenticación. Definir el mecanismo concreto de autenticación y gestión de sesión o token. Estado: Pendiente de aprobación.

DP-TEC-02 — Proveedor o pasarela de pago. Definir el proveedor, la pasarela o el mecanismo concreto para procesar pagos. Estado: Pendiente de aprobación.

DP-TEC-03 — Algoritmo determinista de recomendación. Definir el algoritmo, sus criterios, prioridades y reglas de desempate para producir resultados reproducibles. Estado: Pendiente de aprobación.

## 17.4. Condiciones para AvailabilityService

AvailabilityService deberá calcular disponibilidad únicamente con parámetros operativos previamente aprobados. Como mínimo, deberá considerar horario de apertura y cierre, días laborables y no laborables, duración de intervalos, anticipación mínima y máxima, vigencia de bloqueos temporales y los bloqueos operativos ya definidos para cabinas.

La ausencia de un valor aprobado no deberá resolverse mediante un valor supuesto en el código. Antes de habilitar el módulo en producción, estos parámetros deberán existir como configuración explícita y validada.

La disponibilidad continuará respetando las reglas ya vigentes sobre duración completa del tratamiento, prevención de traslapes, compatibilidad, capacidad, estado operativo de la cabina y bloqueos existentes.

## 17.5. Condiciones para RefundService

RefundService no deberá decidir devoluciones únicamente porque exista una cancelación. Antes de generar una devolución deberá consultar la política aprobada, el momento de la cancelación respecto al inicio del servicio, el pago original, el importe pagado, las devoluciones previas y el tratamiento o reservación afectados.

Mientras no estén aprobadas la política de cancelación, las condiciones de devolución, el tiempo límite y los casos sin derecho a devolución, el sistema podrá conservar la trazabilidad de la cancelación y del pago, pero no deberá aplicar porcentajes, importes, plazos o criterios de reembolso asumidos.

Una vez aprobadas las políticas, RefundService deberá permitir determinar de forma reproducible si corresponde devolución total, parcial o ninguna devolución y registrar el resultado conforme a las reglas de pagos y devoluciones ya definidas.

## 17.6. Criterio de cierre de estas decisiones

Esta sección se considerará cerrada cuando cada decisión pendiente tenga un valor, fórmula, mecanismo o condición formalmente aprobada, esté representada en las reglas de negocio correspondientes y pueda convertirse en criterios de aceptación y pruebas. Hasta entonces, los elementos de esta sección deberán conservar el estado Pendiente de aprobación.

Nota de consistencia: cualquier valor documentado para la duración del bloqueo temporal, incluido un valor fijo expresado en minutos, deberá considerarse no aprobado hasta su validación formal.

# 18. Resumen de reglas por área — Total: 90 reglas de negocio

| **Área**                 | **Rango**     | **Cantidad** |
|--------------------------|---------------|--------------|
| Usuarios y roles         | RN-01 a RN-07 | 7            |
| Tratamientos             | RN-08 a RN-11 | 4            |
| Cabinas                  | RN-12 a RN-16 | 5            |
| Recomendación de cabinas | RN-17 a RN-21 | 5            |
| Carrito de reservación   | RN-22 a RN-26 | 5            |
| Disponibilidad           | RN-27 a RN-29 | 3            |
| Bloqueo temporal         | RN-30 a RN-37 | 8            |
| Confirmación             | RN-38 a RN-41 | 4            |
| Estados                  | RN-42 a RN-45 | 4            |
| Proveedores              | RN-46 a RN-51 | 6            |
| Reasignación automática  | RN-52 a RN-56 | 5            |
| Cancelaciones            | RN-57 a RN-62 | 6            |
| Operación de cabinas     | RN-63 a RN-67 | 5            |
| Historial y trazabilidad | RN-68 a RN-70 | 3            |
| Reportes                 | RN-71 a RN-72 | 2            |
| Pagos y devoluciones     | RN-73 a RN-90 | 18           |
