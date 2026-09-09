**TZISCA**

**DOCUMENTO DE CASOS DE USO**

**CU-01 a CU-43**

Sistema Web de Reservas, Recomendación y Gestión de Cabinas para Spa

*Alcance funcional detallado*

# 1. Propósito del documento

Este documento reúne los 43 casos de uso definidos para TZISCA a partir del alcance funcional acordado. Los casos de uso CU-01 a CU-38 conservan su numeración original para mantener consistencia con la documentación ya creada, y se incorporan los casos CU-39 a CU-43 para cubrir pagos, consulta y gestión de pagos y devoluciones. Los casos se organizan por los cuatro roles principales del sistema: Cliente, Administrador general, Recepción y cabinas y Proveedor de tratamiento. También se incluyen casos automáticos ejecutados por TZISCA cuando una acción del usuario activa reglas de negocio, como el bloqueo temporal de horarios, la búsqueda de un proveedor sustituto o el procesamiento de una devolución.

Cada caso de uso contiene actor principal, objetivo, precondiciones, flujo principal, flujos alternos, postcondiciones y reglas relacionadas. El documento servirá como base para la definición posterior de reglas de negocio, criterios de aceptación, modelo de datos, API e interfaces.

# 2. Roles del sistema

| **Rol** | **Responsabilidad principal** |
|----|----|
| **Cliente** | Consulta tratamientos, arma y confirma reservaciones, administra su carrito, consulta disponibilidad, realiza el pago de su reservación, consulta el estado y detalle del pago y puede cancelar tratamientos o una reservación completa. |
| **Administrador general** | Gestiona usuarios, roles, tratamientos, cabinas y proveedores; consulta reservaciones y reportes, realiza la asignación inicial de proveedores y consulta o gestiona pagos, incidencias y devoluciones conforme a sus permisos. |
| **Recepción y cabinas** | Opera la agenda diaria, crea reservaciones manuales, consulta clientes y disponibilidad, administra bloqueos y estados operativos de cabinas, realiza cancelaciones y registra o valida pagos asociados a reservaciones manuales. |
| **Proveedor de tratamiento** | Consulta únicamente los servicios que tiene asignados, registra su indisponibilidad, inicia y completa atenciones y consulta su historial. |

# 3. Consideraciones funcionales transversales

- Una reservación puede contener uno o varios tratamientos. Cada tratamiento maneja de forma independiente su cabina, fecha, hora, número de personas, proveedor y estado.

- El mismo tratamiento puede agregarse varias veces al carrito para configurarlo en distintos horarios u otras condiciones.

- Los estados definidos para cada tratamiento son: Pendiente, Confirmado, Cancelado, En atención y Completado.

- Los horarios seleccionados en el carrito pueden bloquearse temporalmente durante la duración configurable del bloqueo temporal conforme al parámetro operativo aprobado. Si el Cliente abandona el proceso o vence esa duración, los bloqueos se liberan automáticamente.

- La disponibilidad se valida durante todo el intervalo del tratamiento y debe impedir traslapes de una misma cabina.

- La recomendación de cabina utiliza primero criterios obligatorios como compatibilidad, capacidad y estado operativo; después puede personalizarse con preferencias opcionales del cliente.

- Las recomendaciones de salud y bienestar son informativas; TZISCA no realiza diagnóstico ni prescripción médica.

- La asignación inicial del proveedor la realiza únicamente el Administrador general. Si posteriormente el proveedor queda indisponible, TZISCA intenta encontrar automáticamente un sustituto.

- El cliente debe ser informado cuando exista un cambio de proveedor y puede aceptar al sustituto, cancelar solo el tratamiento afectado o cancelar toda la reservación.

- Las acciones relevantes deben conservar historial básico: usuario que realizó la acción, fecha y hora y, cuando corresponda, motivo u observación.

- Los bloqueos y cambios operativos de cabinas deben conservar un historial para consulta posterior.

- Recepción y cabinas puede consultar los datos básicos del cliente necesarios para la atención: nombre, teléfono, correo, reservaciones actuales e historial. El Proveedor solo verá la información mínima necesaria para prestar el servicio.

- Toda reservación deberá mantener una relación trazable con su información de pago cuando el cobro sea requerido para confirmarla.

- La confirmación definitiva de una reservación dependerá de que el pago requerido haya sido aprobado. Si el mecanismo externo rechaza la operación, el Pago quedará en estado Fallido; un Pago Fallido o Cancelado no producirá la confirmación definitiva.

- Durante el proceso de pago, los bloqueos temporales de cabina y horario se conservarán únicamente mientras continúen vigentes; si expiran, TZISCA deberá volver a validar disponibilidad antes de confirmar.

- El pago deberá conservar al menos importe, estado, fecha y hora, referencia de transacción cuando exista, medio de pago y relación con la reservación correspondiente.

- Los estados permitidos para Pago son: Pendiente, Procesando, Pagado, Fallido, Cancelado, Reembolsado y Reembolsado parcialmente. Un rechazo del mecanismo externo es una causa del estado Fallido y no constituye un estado adicional.

- Las cancelaciones deberán evaluar el impacto económico. Cuando proceda una devolución conforme a las políticas del spa, TZISCA deberá registrar y procesar una devolución parcial o total y mantener su trazabilidad.

# 4. Índice de casos de uso

| **Código** | **Caso de uso** | **Actor principal** |
|----|----|----|
| **CU-01** | Registrarse | Cliente |
| **CU-02** | Iniciar sesión | Cliente |
| **CU-03** | Consultar catálogo de tratamientos | Cliente |
| **CU-04** | Consultar detalle de tratamiento | Cliente |
| **CU-05** | Agregar tratamiento al carrito | Cliente |
| **CU-06** | Gestionar carrito de tratamientos | Cliente |
| **CU-07** | Obtener recomendación de cabina | Cliente |
| **CU-08** | Seleccionar cabina | Cliente |
| **CU-09** | Consultar disponibilidad y seleccionar fecha/hora | Cliente |
| **CU-10** | Bloquear temporalmente horario | Cliente / Sistema TZISCA |
| **CU-11** | Confirmar reservación | Cliente |
| **CU-12** | Consultar mis reservaciones | Cliente |
| **CU-13** | Cancelar tratamiento | Cliente |
| **CU-14** | Cancelar reservación completa | Cliente |
| **CU-15** | Gestionar tratamientos | Administrador general |
| **CU-16** | Gestionar cabinas | Administrador general |
| **CU-17** | Gestionar usuarios y roles | Administrador general |
| **CU-18** | Gestionar proveedores de tratamiento | Administrador general |
| **CU-19** | Asignar proveedor a tratamiento | Administrador general |
| **CU-20** | Consultar todas las reservaciones | Administrador general |
| **CU-21** | Cancelar tratamiento o reservación | Administrador general |
| **CU-22** | Consultar reportes básicos | Administrador general |
| **CU-23** | Crear reservación manual | Recepción y cabinas |
| **CU-24** | Consultar agenda diaria | Recepción y cabinas |
| **CU-25** | Consultar disponibilidad | Recepción y cabinas |
| **CU-26** | Bloquear horario o día de una cabina | Recepción y cabinas |
| **CU-27** | Gestionar estado operativo de cabina | Recepción y cabinas |
| **CU-28** | Cancelar tratamiento | Recepción y cabinas |
| **CU-29** | Cancelar reservación completa | Recepción y cabinas |
| **CU-30** | Consultar mi agenda | Proveedor de tratamiento |
| **CU-31** | Consultar detalle de tratamiento asignado | Proveedor de tratamiento |
| **CU-32** | Marcar tratamiento En atención | Proveedor de tratamiento |
| **CU-33** | Marcar tratamiento Completado | Proveedor de tratamiento |
| **CU-34** | Registrar indisponibilidad | Proveedor de tratamiento |
| **CU-35** | Buscar proveedor sustituto automáticamente | Sistema TZISCA; actores relacionados: Cliente, Proveedor afectado y Administrador general |
| **CU-36** | Gestionar caso sin proveedor sustituto | Sistema TZISCA; actor relacionado: Cliente |
| **CU-37** | Consultar historial de indisponibilidades | Proveedor de tratamiento; actor secundario: Administrador general |
| **CU-38** | Consultar historial de atención | Proveedor de tratamiento |
| CU-39 | Realizar pago de reservación | Cliente |
| CU-40 | Consultar estado y detalle del pago | Cliente |
| CU-41 | Consultar y gestionar pagos | Administrador general |
| CU-42 | Registrar o validar pago en reservación manual | Recepción y cabinas |
| CU-43 | Procesar devolución por cancelación | Sistema TZISCA / Administrador general / Recepción y cabinas |

# 5. Casos de uso del Cliente

## CU-01 — Registrarse

**Actor principal:** Cliente

**Objetivo:** Permitir que una persona cree una cuenta de cliente para acceder a las funciones de reservación de TZISCA.

Precondiciones

- La persona no ha iniciado sesión.

- El correo que se utilizará no debe pertenecer a otra cuenta activa.

Flujo principal

> 1\. El usuario abre la opción de registro.
>
> 2\. Captura nombre, correo electrónico, teléfono y contraseña.
>
> 3\. TZISCA valida que los datos obligatorios tengan un formato válido.
>
> 4\. El sistema verifica que el correo no se encuentre registrado.
>
> 5\. El usuario envía el formulario.
>
> 6\. TZISCA crea la cuenta con rol Cliente.
>
> 7\. El sistema confirma que el registro fue realizado correctamente.
>
> 8\. Después del registro, el sistema puede ofrecer al cliente completar un perfil opcional de preferencias para personalizar futuras recomendaciones.

Flujos alternos / excepciones

- Si el correo ya está registrado, TZISCA informa la situación y no crea una cuenta duplicada.

- Si falta un dato obligatorio o tiene un formato inválido, el sistema solicita corregirlo.

- Si el cliente decide no completar sus preferencias, podrá usar el sistema normalmente; las recomendaciones se harán con los criterios obligatorios disponibles.

Postcondiciones

- La cuenta del cliente queda registrada y disponible para iniciar sesión.

Reglas relacionadas

- El perfil de preferencias es opcional.

- No se requerirá información médica detallada para crear la cuenta.

- Las preferencias podrán utilizarse para personalizar recomendaciones, pero no para emitir diagnósticos ni prescripciones.

————————————————————————————

## CU-02 — Iniciar sesión

**Actor principal:** Cliente

**Objetivo:** Permitir que el cliente acceda de forma autenticada a su cuenta y a las funciones correspondientes a su rol.

Precondiciones

- La cuenta del cliente existe.

- La cuenta se encuentra activa.

Flujo principal

> 1\. El cliente abre la pantalla de inicio de sesión.
>
> 2\. Captura sus credenciales.
>
> 3\. TZISCA valida la información proporcionada.
>
> 4\. Si las credenciales son correctas, el sistema inicia la sesión.
>
> 5\. TZISCA identifica el rol del usuario y habilita las funciones correspondientes.
>
> 6\. El cliente accede a su área principal.

Flujos alternos / excepciones

- Si las credenciales son incorrectas, el sistema informa que no fue posible iniciar sesión.

- Si la cuenta está desactivada, TZISCA impide el acceso.

Postcondiciones

- Existe una sesión autenticada asociada al cliente.

Reglas relacionadas

- El mismo mecanismo de autenticación podrá ser utilizado por los demás roles del sistema, respetando sus permisos.

————————————————————————————

## CU-03 — Consultar catálogo de tratamientos

**Actor principal:** Cliente

**Objetivo:** Permitir que el cliente consulte los tratamientos activos ofrecidos por el spa.

Precondiciones

- El cliente inició sesión.

- Existen tratamientos activos en el catálogo.

Flujo principal

> 1\. El cliente entra al catálogo de tratamientos.
>
> 2\. TZISCA consulta los tratamientos activos.
>
> 3\. El sistema muestra la lista de servicios disponibles.
>
> 4\. Para cada tratamiento se presenta información resumida suficiente para identificarlo.
>
> 5\. El cliente puede seleccionar un tratamiento para consultar su detalle.

Flujos alternos / excepciones

- Si no existen tratamientos activos, el sistema informa que no hay servicios disponibles en ese momento.

Postcondiciones

- El cliente conoce las opciones disponibles y puede continuar al detalle de un tratamiento.

Reglas relacionadas

- Los tratamientos desactivados no deberán mostrarse como opciones reservables al cliente.

————————————————————————————

## CU-04 — Consultar detalle de tratamiento

**Actor principal:** Cliente

**Objetivo:** Permitir que el cliente conozca la información de un tratamiento antes de agregarlo al carrito.

Precondiciones

- El tratamiento existe.

- El tratamiento se encuentra activo.

Flujo principal

> 1\. El cliente selecciona un tratamiento del catálogo.
>
> 2\. TZISCA muestra su nombre, descripción, duración, beneficios informativos y consideraciones generales de uso.
>
> 3\. El sistema puede mostrar los tipos de cabina compatibles y otra información de referencia disponible.
>
> 4\. El cliente revisa la información.
>
> 5\. Si desea continuar, puede agregar el tratamiento al carrito.

Flujos alternos / excepciones

- Si el tratamiento fue desactivado mientras el cliente consultaba el catálogo, el sistema informa que ya no está disponible para reservar.

Postcondiciones

- El cliente dispone de información suficiente para decidir si agrega el tratamiento al carrito.

Reglas relacionadas

- La información de salud tendrá carácter general e informativo.

- TZISCA no presentará las recomendaciones como diagnóstico, prescripción ni valoración médica.

————————————————————————————

## CU-05 — Agregar tratamiento al carrito

**Actor principal:** Cliente

**Objetivo:** Permitir que el cliente agregue uno o varios tratamientos al carrito de reservación antes de configurar cabina, fecha y hora.

Precondiciones

- El cliente inició sesión.

- El tratamiento existe, está activo y se encuentra disponible en el catálogo.

Flujo principal

> 1\. El cliente entra al catálogo de tratamientos.
>
> 2\. Selecciona un tratamiento.
>
> 3\. Consulta su información.
>
> 4\. Presiona la opción Agregar al carrito.
>
> 5\. El sistema agrega el tratamiento al carrito como un elemento independiente.
>
> 6\. TZISCA actualiza la cantidad de tratamientos agregados.
>
> 7\. El cliente puede continuar agregando más tratamientos o ir al carrito.

Flujos alternos / excepciones

- Si el tratamiento fue desactivado antes de agregarse, el sistema informa que ya no está disponible.

- Si ocurre un error al agregarlo, TZISCA muestra un mensaje y no modifica el carrito.

- El mismo tratamiento puede agregarse más de una vez y cada instancia se administrará por separado.

Postcondiciones

- El tratamiento queda almacenado temporalmente en el carrito.

- Todavía no se reserva definitivamente cabina, horario ni proveedor.

Reglas relacionadas

- Un carrito puede contener uno o varios tratamientos.

- Agregar al carrito no equivale a confirmar una reservación.

- El cliente podrá quitar o modificar el elemento antes de confirmar.

————————————————————————————

## CU-06 — Gestionar carrito de tratamientos

**Actor principal:** Cliente

**Objetivo:** Permitir que el cliente administre los tratamientos agregados al carrito antes de confirmar la reservación.

Precondiciones

- El cliente inició sesión.

- Existe al menos un tratamiento agregado al carrito.

Flujo principal

> 1\. El cliente abre el carrito.
>
> 2\. TZISCA muestra todos los tratamientos agregados.
>
> 3\. El cliente selecciona uno de los elementos.
>
> 4\. Puede modificar número de personas, cabina, fecha u hora.
>
> 5\. Puede eliminar el tratamiento del carrito.
>
> 6\. Puede agregar más tratamientos.
>
> 7\. Puede tener el mismo tratamiento varias veces.
>
> 8\. El sistema mantiene cada elemento de manera independiente.
>
> 9\. El cliente continúa al proceso de cabina, fecha y hora.

Flujos alternos / excepciones

- Si el carrito queda vacío, el sistema informa que no existen tratamientos pendientes.

- Si elimina un tratamiento que ya tenía un horario bloqueado temporalmente, TZISCA libera inmediatamente ese horario.

- Si modifica cabina, fecha u hora, el sistema libera el bloqueo anterior y vuelve a validar la nueva opción.

- Si la nueva opción no está disponible, TZISCA no confirma el cambio y muestra alternativas cuando corresponda.

Postcondiciones

- El carrito queda actualizado con los tratamientos que el cliente desea conservar.

- Los cambios aún no representan una reservación definitiva.

Reglas relacionadas

- El mismo tratamiento puede aparecer varias veces en el carrito.

- Cada elemento se gestiona de forma independiente.

- La vigencia del bloqueo corresponde a la duración configurable del bloqueo temporal conforme al parámetro operativo aprobado.

- Al eliminar o cambiar un horario, el bloqueo anterior se libera inmediatamente.

————————————————————————————

## CU-07 — Obtener recomendación de cabina

**Actor principal:** Cliente

**Objetivo:** Permitir que TZISCA analice el tratamiento y muestre una cabina recomendada, además de otras alternativas compatibles.

Precondiciones

- El cliente inició sesión.

- El tratamiento está agregado al carrito.

- El tratamiento está activo.

- El cliente indicó el número de personas.

- Existen cabinas registradas y activas.

Flujo principal

> 1\. El cliente selecciona un tratamiento dentro del carrito.
>
> 2\. TZISCA consulta las cabinas compatibles con ese tratamiento.
>
> 3\. El sistema descarta cabinas con capacidad menor al número de personas.
>
> 4\. Descarta cabinas desactivadas, fuera de servicio o en mantenimiento.
>
> 5\. Si ya existe una fecha y hora seleccionadas, el sistema puede considerar también la disponibilidad del intervalo; la validación definitiva se realiza en CU-09.
>
> 6\. TZISCA identifica las cabinas que cumplen las condiciones obligatorias.
>
> 7\. Entre las opciones válidas, el sistema considera las preferencias del cliente cuando estén disponibles.
>
> 8\. Puede priorizar una cabina especializada frente a una multifuncional cuando ambas satisfacen los mismos criterios.
>
> 9\. TZISCA presenta una cabina como recomendada y muestra otras alternativas compatibles.
>
> 10\. El cliente consulta la información de las opciones.

Flujos alternos / excepciones

- Si no existe ninguna cabina compatible con el tratamiento y el número de personas, el sistema informa la situación.

- Si el cliente no completó preferencias, TZISCA realiza la recomendación usando compatibilidad, capacidad, estado y demás criterios obligatorios.

Postcondiciones

- El cliente dispone de una recomendación y de alternativas compatibles para elegir.

Reglas relacionadas

- La recomendación debe respetar primero compatibilidad, capacidad y estado operativo.

- Las preferencias del cliente solo personalizan entre opciones válidas.

- La recomendación no tendrá carácter clínico.

- Preferencias posibles: objetivo de la visita, modalidad individual/pareja, privacidad, ambiente y accesibilidad.

————————————————————————————

## CU-08 — Seleccionar cabina

**Actor principal:** Cliente

**Objetivo:** Permitir que el cliente elija la cabina que utilizará para un tratamiento, aceptando la recomendada o seleccionando otra alternativa compatible.

Precondiciones

- El cliente inició sesión.

- El tratamiento está en el carrito.

- El cliente indicó el número de personas.

- TZISCA ya identificó las cabinas compatibles.

- Existe al menos una cabina válida.

Flujo principal

> 1\. TZISCA muestra la cabina recomendada.
>
> 2\. También muestra otras cabinas compatibles como alternativas.
>
> 3\. El cliente revisa nombre, tipo, capacidad, características, equipamiento, beneficios, accesibilidad e imagen de referencia cuando exista.
>
> 4\. Puede aceptar la cabina recomendada.
>
> 5\. Si no la desea, puede seleccionar otra alternativa compatible.
>
> 6\. El sistema guarda temporalmente la cabina elegida para ese tratamiento.
>
> 7\. El cliente continúa con fecha y hora.

Flujos alternos / excepciones

- Si la cabina seleccionada deja de ser operativa o no puede utilizarse para el horario solicitado, TZISCA informa la situación y muestra otras opciones válidas.

Postcondiciones

- El tratamiento queda relacionado temporalmente con la cabina seleccionada.

Reglas relacionadas

- El cliente no está obligado a aceptar la recomendación.

- Solo podrá elegir cabinas compatibles, con capacidad suficiente y operativas.

- Un tratamiento utiliza una sola cabina a la vez; una reservación con varios tratamientos puede utilizar diferentes cabinas.

————————————————————————————

## CU-09 — Consultar disponibilidad y seleccionar fecha/hora

**Actor principal:** Cliente

**Objetivo:** Permitir que el cliente seleccione una fecha y hora para cada tratamiento y que TZISCA valide la disponibilidad durante toda la duración del servicio.

Precondiciones

- El cliente inició sesión.

- El tratamiento está en el carrito.

- El cliente indicó número de personas.

- Existe una cabina seleccionada.

- El tratamiento tiene una duración definida.

Flujo principal

> 1\. El cliente selecciona una fecha.
>
> 2\. TZISCA consulta la disponibilidad de la cabina.
>
> 3\. El cliente selecciona una hora.
>
> 4\. El sistema obtiene la duración del tratamiento.
>
> 5\. Calcula la hora estimada de finalización.
>
> 6\. Verifica que la cabina permanezca libre durante todo el intervalo.
>
> 7\. Si no existe conflicto, muestra el horario como disponible.
>
> 8\. El cliente continúa con el bloqueo temporal del horario.

Flujos alternos / excepciones

- Si la cabina está ocupada durante cualquier parte del intervalo, el horario se considera no disponible.

- Si la cabina está en mantenimiento o fuera de servicio, no se ofrece como disponible.

- Si el horario seleccionado está ocupado, TZISCA muestra horarios alternativos.

- El cliente puede elegir otra hora sin eliminar los demás tratamientos del carrito.

Postcondiciones

- El tratamiento queda con una fecha y hora válidas listas para ser bloqueadas temporalmente.

Reglas relacionadas

- La disponibilidad se valida durante todo el intervalo del tratamiento, no únicamente en la hora de inicio.

- No deben existir traslapes de uso de la misma cabina.

————————————————————————————

## CU-10 — Bloquear temporalmente horario

**Actor principal:** Cliente / Sistema TZISCA

**Objetivo:** Evitar que dos clientes intenten confirmar simultáneamente la misma cabina en horarios incompatibles.

Precondiciones

- El cliente seleccionó cabina, fecha y hora.

- TZISCA validó que el intervalo se encuentra disponible.

Flujo principal

> 1\. El cliente selecciona el horario.
>
> 2\. TZISCA crea un bloqueo temporal de la cabina y el intervalo.
>
> 3\. El bloqueo tendrá la duración configurable del bloqueo temporal conforme al parámetro operativo aprobado.
>
> 4\. Durante ese periodo el mismo recurso no se ofrecerá a otros clientes para intervalos que entren en conflicto.
>
> 5\. Si existen varios tratamientos en el carrito, TZISCA puede mantener bloqueados temporalmente los horarios de todos ellos.
>
> 6\. El cliente continúa preparando la reservación.

Flujos alternos / excepciones

- Si el cliente cambia de hora, TZISCA libera inmediatamente el bloqueo anterior y trata de bloquear el nuevo horario.

- Si elimina el tratamiento del carrito, el bloqueo correspondiente se elimina.

- Si abandona el proceso, TZISCA libera los bloqueos temporales.

- Si vence la duración configurable del bloqueo temporal conforme al parámetro operativo aprobado sin que la reservación se confirme, TZISCA libera automáticamente todos los horarios temporales.

Postcondiciones

- El horario queda protegido temporalmente, pero todavía no constituye una reservación definitiva.

Reglas relacionadas

- Si el Cliente continúa al pago, el bloqueo temporal podrá mantenerse durante el intento de pago únicamente mientras no haya expirado su vigencia.

- Si el bloqueo expira durante el pago, TZISCA libera el recurso, vuelve a validar la disponibilidad y no confirma automáticamente la reservación.

- El bloqueo temporal no debe mantenerse indefinidamente.

- La vigencia aplicable será la duración configurable del bloqueo temporal conforme al parámetro operativo aprobado; el caso de uso no establece un valor fijo.

————————————————————————————

## CU-11 — Confirmar reservación

**Actor principal:** Cliente

**Objetivo:** Permitir que el Cliente cree una reservación en proceso desde el carrito y la confirme definitivamente solo después del pago aprobado y de la revalidación satisfactoria de disponibilidad.

Precondiciones

- El cliente inició sesión.

- El carrito contiene al menos un tratamiento configurado.

- Cada tratamiento tiene número de personas, cabina, fecha y hora.

- La disponibilidad de los recursos es válida.

- Los bloqueos temporales se encuentran vigentes.

- TZISCA dispone de los datos necesarios para calcular el importe, cuya fórmula se regirá por la política aprobada.

Flujo principal

> 1\. El Cliente abre el resumen del carrito configurado.
>
> 2\. TZISCA verifica que todos los tratamientos tengan número de personas, cabina, fecha y hora.
>
> 3\. El sistema valida la disponibilidad y la vigencia de los bloqueos temporales.
>
> 4\. TZISCA crea la Reservacion en estado EN_PROCESO y asigna id_reservacion. Esta creación no equivale a una confirmación definitiva.
>
> 5\. El sistema crea cada ReservacionTratamiento en estado PENDIENTE.
>
> 6\. TZISCA calcula y muestra el importe de los tratamientos y el total de la Reservacion conforme a la política de cálculo aprobada.
>
> 7\. El Cliente continúa a CU-39 — Realizar pago de reservación.
>
> 8\. CU-39 genera y procesa el Pago relacionado con id_reservacion.
>
> 9\. Si el Pago queda Pagado, TZISCA regresa a CU-11 y revalida la disponibilidad de todos los recursos.

10\. Si la disponibilidad continúa válida, TZISCA confirma definitivamente la Reservacion.

11\. Cada ReservacionTratamiento confirmado pasa de PENDIENTE a CONFIRMADO.

12\. Los bloqueos temporales correspondientes se convierten en ocupaciones reales.

13\. TZISCA muestra la confirmación al Cliente y conserva la relación trazable con el Pago.

Flujos alternos / excepciones

- Si la disponibilidad deja de ser válida antes de crear la Reservacion EN_PROCESO, TZISCA identifica los tratamientos afectados y no crea los elementos en conflicto.

- El Cliente puede conservar los tratamientos que continúan disponibles.

- El Cliente puede modificar o eliminar el tratamiento afectado, o cancelar todo el proceso.

- Si el mecanismo externo rechaza la operación o el pago falla, el Pago queda en estado Fallido, se conserva la trazabilidad del intento y la Reservacion no se confirma.

- Mientras los bloqueos temporales continúen vigentes, el Cliente puede reintentar un Pago Fallido sin perder automáticamente los recursos seleccionados.

- Si un bloqueo temporal expira, TZISCA lo libera, revalida la disponibilidad y no confirma automáticamente la Reservacion.

- Si el Pago ya está Pagado y al revalidar se perdió disponibilidad, TZISCA no confirma el ReservacionTratamiento afectado, informa al Cliente, conserva el Pago y su trazabilidad y envía el caso al tratamiento económico definido por las políticas o por el proceso de conciliación. Mientras esa política permanezca pendiente, no se presume una devolución automática.

Postcondiciones

- La creación de una Reservacion EN_PROCESO y de sus ReservacionTratamiento en PENDIENTE no equivale a una confirmación definitiva.

- La Reservacion queda confirmada únicamente después del Pago Pagado y de la revalidación satisfactoria de disponibilidad.

- Los intervalos confirmados dejan de estar disponibles para otros clientes y el Pago conserva su trazabilidad.

Reglas relacionadas

- Crear la Reservacion EN_PROCESO no la confirma; la confirmación requiere disponibilidad válida, bloqueos vigentes o revalidados y Pago Pagado cuando corresponda.

- CU-11 utiliza CU-39 para procesar el pago y CU-40 para consultar posteriormente su estado.

- La asignación inicial del proveedor se realiza después de la confirmación y exclusivamente por el Administrador general.

————————————————————————————

## CU-12 — Consultar mis reservaciones

**Actor principal:** Cliente

**Objetivo:** Permitir que el cliente consulte las reservaciones realizadas y el estado de sus tratamientos.

Precondiciones

- El cliente inició sesión.

Flujo principal

> 1\. El cliente entra a Mis reservaciones.
>
> 2\. TZISCA consulta las reservaciones asociadas a su cuenta.
>
> 3\. Muestra la lista correspondiente.
>
> 4\. El cliente selecciona una reservación.
>
> 5\. El sistema muestra los tratamientos incluidos.
>
> 6\. Para cada tratamiento presenta nombre, cabina, fecha, hora, número de personas, estado y proveedor asignado cuando corresponda.

7\. TZISCA muestra el estado general del pago asociado a la reservación y permite acceder a su detalle mediante CU-40.

Flujos alternos / excepciones

- Si el cliente aún no tiene reservaciones, TZISCA muestra un estado vacío informativo.

Postcondiciones

- El cliente conoce el estado actual e histórico de sus servicios.

Reglas relacionadas

- El cliente solo podrá consultar la información de pago correspondiente a sus propias reservaciones.

- El cliente solo puede consultar sus propias reservaciones.

————————————————————————————

## CU-13 — Cancelar tratamiento

**Actor principal:** Cliente

**Objetivo:** Permitir que el cliente cancele únicamente uno de los tratamientos de una reservación sin cancelar los demás.

Precondiciones

- El cliente inició sesión.

- La reservación le pertenece.

- El tratamiento puede ser cancelado conforme a las políticas vigentes del spa.

- Si la reservación tiene un pago asociado, TZISCA puede identificar el importe correspondiente al tratamiento y su estado de pago.

Flujo principal

> 1\. El cliente consulta una reservación.
>
> 2\. Selecciona uno de sus tratamientos.
>
> 3\. Elige Cancelar tratamiento.
>
> 4\. TZISCA solicita confirmación.
>
> 5\. El cliente confirma.
>
> 6\. El tratamiento cambia al estado Cancelado.
>
> 7\. TZISCA libera cabina, horario y proveedor asignado cuando exista.
>
> 8\. Los demás tratamientos permanecen sin cambios.

9\. TZISCA revisa el Pago relacionado y verifica si se encuentra en estado Pagado.

10\. Si existe un Pago Pagado, el sistema consulta la política aprobada para determinar si corresponde una devolución parcial y cuál es el importe reembolsable.

11\. Solo cuando la política aplicable autoriza una devolución y existe un importe reembolsable, TZISCA inicia CU-43 — Procesar devolución por cancelación.

> 12\. El sistema registra la cancelación y su impacto económico.

Flujos alternos / excepciones

- Si el tratamiento ya está completado o no puede cancelarse conforme a las reglas vigentes, TZISCA impide la operación.

- Si no existe un Pago Pagado, no hay una política aprobada aplicable o no existe importe reembolsable, TZISCA conserva la cancelación y su trazabilidad sin iniciar CU-43.

- Si una devolución iniciada no puede completarse inmediatamente, conserva el estado de devolución que corresponda para seguimiento.

Postcondiciones

- Solo el tratamiento seleccionado queda cancelado y sus recursos quedan liberados.

- El Pago conserva su trazabilidad y, cuando se haya iniciado CU-43, el estado de la Devolucion queda actualizado.

Reglas relacionadas

- La cancelación de un tratamiento solo podrá originar una devolución de tipo PARCIAL cuando la política aprobada así lo determine; no genera automáticamente una devolución total.

- Una reservación confirmada no se edita directamente; si el cliente desea cambiar un servicio, debe cancelarlo y generar una nueva selección/reservación.

————————————————————————————

## CU-14 — Cancelar reservación completa

**Actor principal:** Cliente

**Objetivo:** Permitir que el cliente cancele todos los tratamientos activos de una reservación.

Precondiciones

- El cliente inició sesión.

- La reservación le pertenece.

- La cancelación está permitida conforme a las políticas aplicables.

- Si existe un pago asociado, TZISCA puede consultar su importe, estado y devoluciones previas.

Flujo principal

> 1\. El cliente abre una reservación.
>
> 2\. Selecciona Cancelar reservación.
>
> 3\. TZISCA informa que se cancelarán todos los tratamientos activos.
>
> 4\. El cliente confirma.
>
> 5\. El sistema cambia los tratamientos activos al estado Cancelado.
>
> 6\. Libera cabinas, horarios y proveedores asignados.

7\. TZISCA revisa el Pago relacionado y verifica si se encuentra en estado Pagado.

8\. Si existe un Pago Pagado, el sistema consulta la política aprobada para determinar si corresponde una devolución de tipo TOTAL o, según los elementos afectados, PARCIAL, y cuál es el importe reembolsable.

9\. Solo cuando la política aplicable autoriza una devolución y existe un importe reembolsable, TZISCA inicia CU-43 — Procesar devolución por cancelación.

> 10\. Registra el movimiento, el impacto económico y muestra la confirmación.

Flujos alternos / excepciones

- Si algún tratamiento ya se encuentra completado, no se modifica su estado histórico; la cancelación aplica a los tratamientos que todavía puedan cancelarse.

- Si no existe un Pago Pagado, no hay una política aprobada aplicable o no existe importe reembolsable, TZISCA registra la condición económica sin iniciar CU-43.

- Si una devolución iniciada queda PENDIENTE o FALLIDA, la cancelación se conserva y la Devolucion queda disponible para seguimiento.

Postcondiciones

- La reservación deja de tener tratamientos activos reservados.

- El Pago conserva su trazabilidad y, cuando corresponda, la Devolucion conserva su tipo y estado.

Reglas relacionadas

- La cancelación completa evalúa una posible devolución de tipo TOTAL o PARCIAL únicamente conforme a la política aprobada y al importe reembolsable.

- El cliente debe poder distinguir entre cancelar un tratamiento y cancelar toda la reservación.

————————————————————————————

# 6. Casos de uso del Administrador general

## CU-15 — Gestionar tratamientos

**Actor principal:** Administrador general

**Objetivo:** Permitir al Administrador general crear, consultar, editar y desactivar tratamientos disponibles en TZISCA.

Precondiciones

- El Administrador general inició sesión.

- Tiene permisos de administración.

Flujo principal

> 1\. El Administrador entra al módulo de tratamientos.
>
> 2\. TZISCA muestra el catálogo registrado.
>
> 3\. Puede crear un nuevo tratamiento.
>
> 4\. Captura su información.
>
> 5\. El sistema valida los datos.
>
> 6\. El tratamiento queda registrado.
>
> 7\. También puede seleccionar uno existente para editarlo.
>
> 8\. Puede modificar su información.
>
> 9\. Puede desactivarlo si ya no debe mostrarse a los clientes.

Flujos alternos / excepciones

- Si falta información obligatoria, el sistema solicita completarla.

- Si el tratamiento está asociado a reservaciones existentes, no se elimina físicamente; se desactiva para conservar el historial.

- Si existen datos inválidos, no se guardan los cambios.

Postcondiciones

- El catálogo de tratamientos queda actualizado.

Reglas relacionadas

- La información administrada incluye nombre, descripción, duración, beneficios informativos, recomendaciones generales, restricciones, cabinas compatibles y estado activo/inactivo.

————————————————————————————

## CU-16 — Gestionar cabinas

**Actor principal:** Administrador general

**Objetivo:** Permitir crear, editar, consultar y desactivar cabinas del spa.

Precondiciones

- El Administrador general inició sesión.

- Tiene permisos administrativos.

Flujo principal

> 1\. El Administrador entra al módulo de cabinas.
>
> 2\. Consulta las cabinas existentes.
>
> 3\. Selecciona crear una nueva cabina.
>
> 4\. Captura nombre o identificador, tipo, capacidad, descripción, características, equipamiento, accesibilidad y demás datos definidos.
>
> 5\. Relaciona la cabina con los tratamientos compatibles.
>
> 6\. Guarda la información.
>
> 7\. Puede editar posteriormente la cabina y modificar sus relaciones o características.
>
> 8\. Puede desactivarla cuando deje de formar parte del catálogo activo.

Flujos alternos / excepciones

- Si faltan datos obligatorios, TZISCA solicita completarlos.

- Si la cabina tiene historial de reservaciones, se conserva el registro y se desactiva en lugar de eliminarse físicamente.

Postcondiciones

- La información de las cabinas queda actualizada.

Reglas relacionadas

- Estados contemplados: Disponible, Ocupada, En mantenimiento, Fuera de servicio y Desactivada.

- La desactivación administrativa corresponde principalmente al Administrador general.

————————————————————————————

## CU-17 — Gestionar usuarios y roles

**Actor principal:** Administrador general

**Objetivo:** Permitir al Administrador consultar usuarios y controlar los roles y permisos del sistema.

Precondiciones

- El Administrador general inició sesión.

- Tiene permisos para administrar usuarios.

Flujo principal

> 1\. El Administrador entra al módulo de usuarios.
>
> 2\. TZISCA muestra las cuentas registradas.
>
> 3\. Selecciona un usuario.
>
> 4\. Consulta sus datos básicos.
>
> 5\. Puede asignar o modificar su rol.
>
> 6\. El sistema actualiza los permisos correspondientes.
>
> 7\. Puede desactivar una cuenta cuando deba dejar de utilizarse.

Flujos alternos / excepciones

- Si intenta realizar un cambio no permitido, el sistema rechaza la acción.

- Una cuenta desactivada conserva su historial.

Postcondiciones

- El usuario queda asociado al rol y permisos definidos.

Reglas relacionadas

- Roles definidos: Cliente, Administrador general, Recepción y cabinas y Proveedor de tratamiento.

————————————————————————————

## CU-18 — Gestionar proveedores de tratamiento

**Actor principal:** Administrador general

**Objetivo:** Administrar a las personas que prestan los servicios del spa y relacionarlas con los tratamientos que pueden realizar.

Precondiciones

- El Administrador general inició sesión.

- Existen tratamientos registrados.

Flujo principal

> 1\. El Administrador entra al módulo de proveedores.
>
> 2\. Consulta los proveedores existentes.
>
> 3\. Puede registrar uno nuevo.
>
> 4\. Captura sus datos.
>
> 5\. Selecciona los tratamientos que puede realizar.
>
> 6\. Guarda la información.
>
> 7\. Puede editar posteriormente los datos o tratamientos asociados.
>
> 8\. Puede desactivar al proveedor cuando deje de prestar servicios.

Flujos alternos / excepciones

- Si el proveedor tiene historial de servicios, TZISCA conserva dicho historial aun cuando la cuenta sea desactivada.

Postcondiciones

- El proveedor queda registrado y relacionado con los tratamientos correspondientes.

Reglas relacionadas

- Solo los proveedores asociados a un tratamiento podrán considerarse para su asignación o sustitución.

————————————————————————————

## CU-19 — Asignar proveedor a tratamiento

**Actor principal:** Administrador general

**Objetivo:** Permitir que el Administrador general seleccione qué proveedor atenderá cada tratamiento confirmado.

Precondiciones

- Existe una reservación confirmada.

- El tratamiento requiere un proveedor.

- Existen proveedores capaces de realizarlo.

Flujo principal

> 1\. El Administrador consulta una reservación.
>
> 2\. Selecciona uno de sus tratamientos.
>
> 3\. TZISCA muestra los proveedores compatibles con ese tratamiento.
>
> 4\. El sistema descarta o identifica a quienes no estén disponibles para la fecha y hora.
>
> 5\. El Administrador selecciona un proveedor disponible.
>
> 6\. Confirma la asignación.
>
> 7\. TZISCA relaciona al proveedor con el tratamiento.
>
> 8\. El tratamiento aparece en la agenda del proveedor.
>
> 9\. El sistema registra quién realizó la asignación y cuándo.

Flujos alternos / excepciones

- Si ningún proveedor compatible está disponible, TZISCA informa al Administrador.

- Si el proveedor seleccionado presenta un conflicto de horario o indisponibilidad, el sistema no permite la asignación y solicita elegir otro.

Postcondiciones

- El tratamiento queda asociado a un proveedor.

Reglas relacionadas

- La asignación inicial es manual y exclusiva del Administrador general.

- El proveedor no selecciona libremente las reservaciones que desea atender.

————————————————————————————

## CU-20 — Consultar todas las reservaciones

**Actor principal:** Administrador general

**Objetivo:** Permitir al Administrador general consultar todas las reservaciones registradas en TZISCA.

Precondiciones

- El Administrador general inició sesión.

Flujo principal

> 1\. El Administrador entra al módulo de reservaciones.
>
> 2\. TZISCA muestra la lista de reservaciones.
>
> 3\. Selecciona una reservación.
>
> 4\. Consulta la información general del cliente.
>
> 5\. Consulta los tratamientos incluidos.
>
> 6\. Para cada tratamiento puede revisar cabina, fecha, hora, número de personas, proveedor y estado.

7\. TZISCA muestra el estado del pago relacionado con la reservación y, cuando existan, los importes o estados de devolución.

Flujos alternos / excepciones

- Si no existen reservaciones para el criterio consultado, TZISCA muestra un resultado vacío informativo.

Postcondiciones

- El Administrador dispone de información para supervisar la operación y dar seguimiento.

- La consulta administrativa de reservaciones deberá mantener visible la relación entre la reservación y su pago.

Reglas relacionadas

- La consulta debe respetar el historial incluso cuando existan tratamientos cancelados o completados.

————————————————————————————

## CU-21 — Cancelar tratamiento o reservación

**Actor principal:** Administrador general

**Objetivo:** Permitir al Administrador general cancelar un tratamiento específico o todos los tratamientos activos de una reservación.

Precondiciones

- Existe una reservación.

- El Administrador general inició sesión.

- Los elementos afectados todavía pueden cancelarse.

- TZISCA puede consultar el Pago asociado a la Reservacion y su estado.

Flujo principal

> 1\. El Administrador abre la reservación.
>
> 2\. Si desea cancelar un solo servicio, selecciona el tratamiento y elige Cancelar tratamiento.
>
> 3\. Registra el motivo y confirma.
>
> 4\. TZISCA cambia ese tratamiento a Cancelado y libera cabina, horario y proveedor.
>
> 5\. Si desea cancelar toda la reservación, selecciona Cancelar reservación.
>
> 6\. El sistema muestra los tratamientos que serán afectados.
>
> 7\. El Administrador registra el motivo y confirma.
>
> 8\. TZISCA cancela los tratamientos activos y libera sus recursos.

9\. TZISCA revisa si existe un Pago en estado Pagado relacionado con los elementos cancelados.

10\. Si existe, el Administrador consulta la política aprobada para determinar el tipo de devolución y el importe reembolsable.

11\. Solo cuando la política aplicable autoriza una devolución y existe un importe reembolsable, TZISCA ejecuta CU-43 — Procesar devolución por cancelación.

> 12\. El sistema registra quién realizó la acción, fecha, hora, motivo e impacto económico.

Flujos alternos / excepciones

- Si un tratamiento ya no puede cancelarse, TZISCA informa la restricción y no modifica su estado.

- Si no existe un Pago Pagado, no hay una política aprobada aplicable o no existe importe reembolsable, TZISCA registra la cancelación sin iniciar CU-43.

Postcondiciones

- Los tratamientos afectados quedan cancelados y sus recursos vuelven a estar disponibles.

- La cancelación conserva la relación económica de la Reservacion, el Pago original y, cuando se haya iniciado CU-43, el tipo, importe y estado de la Devolucion.

Reglas relacionadas

- El Administrador general tiene este permiso como respaldo o para casos especiales; la operación cotidiana corresponde principalmente a Recepción y cabinas.

————————————————————————————

## CU-22 — Consultar reportes básicos

**Actor principal:** Administrador general

**Objetivo:** Permitir consultar información operativa básica sobre el funcionamiento del spa.

Precondiciones

- El Administrador general inició sesión.

- Existen datos registrados para el periodo consultado.

Flujo principal

> 1\. El Administrador entra al módulo de reportes.
>
> 2\. Selecciona el periodo que desea consultar.
>
> 3\. TZISCA procesa la información disponible.
>
> 4\. Muestra indicadores básicos.

Flujos alternos / excepciones

- Si no existen datos para el periodo, el sistema informa que no hay información suficiente para generar resultados.

Postcondiciones

- El Administrador obtiene información resumida para seguimiento de la operación.

Reglas relacionadas

- Reportes básicos definidos: número de reservaciones por día, tratamientos más solicitados, cabinas más utilizadas, cancelaciones y ocupación general de cabinas.

- Los reportes son operativos; los reportes avanzados pueden quedar para una etapa posterior.

————————————————————————————

# 7. Casos de uso de Recepción y cabinas

## CU-23 — Crear reservación manual

**Actor principal:** Recepción y cabinas

**Objetivo:** Permitir que Recepción y cabinas cree una reservación manual para un Cliente y, cuando se requiera pago, la mantenga EN_PROCESO hasta cumplir las condiciones de confirmación.

Precondiciones

- Recepción y cabinas inició sesión.

- El cliente existe o puede ser registrado.

- Existen tratamientos activos.

Flujo principal

> 1\. Recepción entra al módulo de reservaciones.
>
> 2\. Selecciona Nueva reservación.
>
> 3\. Busca o registra al cliente.
>
> 4\. Selecciona uno o varios tratamientos.
>
> 5\. Indica número de personas para cada tratamiento.
>
> 6\. Selecciona cabina.
>
> 7\. Selecciona fecha y hora.
>
> 8\. TZISCA valida compatibilidad, capacidad y disponibilidad.
>
> 9\. Recepción revisa el resumen de la reservación manual.
>
> 10\. Cuando se requiere pago, TZISCA crea la Reservacion en estado EN_PROCESO con id_reservacion y crea sus ReservacionTratamiento en estado PENDIENTE.
>
> 11\. TZISCA calcula el importe conforme a la política aprobada y Recepción registra o valida el Pago mediante CU-42.
>
> 12\. Si el Pago queda Pagado, TZISCA revalida la disponibilidad; si continúa válida, confirma la Reservacion y cambia sus ReservacionTratamiento de PENDIENTE a CONFIRMADO.

13\. Después de la confirmación, el proveedor inicial será asignado exclusivamente por el Administrador general.

Flujos alternos / excepciones

- Si un horario no está disponible, TZISCA muestra alternativas.

- Si una cabina no es compatible, no puede seleccionarse.

- Si uno de varios tratamientos presenta conflicto, Recepción puede conservar los demás o quitar el afectado.

- Si el Pago queda Pendiente, Fallido o Cancelado, la Reservacion manual permanece EN_PROCESO, sus ReservacionTratamiento permanecen PENDIENTE y no se confirma definitivamente.

Postcondiciones

- La Reservacion manual puede quedar EN_PROCESO con sus ReservacionTratamiento en PENDIENTE mientras se resuelve el pago requerido.

- La Reservacion queda relacionada con su Pago cuando el cobro es requerido y solo se confirma después de un Pago Pagado y de la revalidación satisfactoria de disponibilidad.

Reglas relacionadas

- La forma de pago de una reservación manual puede diferir del flujo en línea del Cliente, pero debe usar los estados normalizados y conservar trazabilidad.

- Recepción y cabinas no realiza la asignación inicial de proveedores; esa asignación ocurre después de la confirmación y corresponde exclusivamente al Administrador general.

————————————————————————————

## CU-24 — Consultar agenda diaria

**Actor principal:** Recepción y cabinas

**Objetivo:** Permitir consultar la operación del spa organizada por fecha y horario.

Precondiciones

- Recepción y cabinas inició sesión.

Flujo principal

> 1\. Recepción entra a la agenda.
>
> 2\. Selecciona una fecha.
>
> 3\. TZISCA muestra los tratamientos programados.
>
> 4\. Para cada servicio muestra hora de inicio y finalización, tratamiento, cabina, cliente, número de personas, proveedor asignado, estado del tratamiento y estado de pago cuando corresponda.
>
> 5\. Recepción puede cambiar de día para revisar otras fechas.

Flujos alternos / excepciones

- Si no existen servicios programados para la fecha, el sistema muestra la agenda sin registros activos.

Postcondiciones

- Recepción conoce la ocupación y actividades programadas.

Reglas relacionadas

- La agenda debe reflejar los cambios de estado, cancelaciones y reasignaciones vigentes.

————————————————————————————

## CU-25 — Consultar disponibilidad

**Actor principal:** Recepción y cabinas

**Objetivo:** Permitir revisar qué cabinas y horarios están disponibles antes de crear o atender una reservación.

Precondiciones

- Recepción y cabinas inició sesión.

Flujo principal

> 1\. Recepción selecciona una fecha.
>
> 2\. Puede consultar por tratamiento o por cabina.
>
> 3\. TZISCA revisa reservaciones confirmadas, bloqueos temporales, mantenimiento, cabinas fuera de servicio y otros bloqueos operativos.
>
> 4\. El sistema muestra periodos disponibles y ocupados.

Flujos alternos / excepciones

- Si una cabina cambia de estado durante la consulta, TZISCA debe utilizar la información vigente al momento de confirmar una operación.

Postcondiciones

- Recepción puede identificar correctamente los recursos disponibles.

Reglas relacionadas

- La disponibilidad debe considerar la duración completa del servicio y evitar traslapes.

————————————————————————————

## CU-26 — Bloquear horario o día de una cabina

**Actor principal:** Recepción y cabinas

**Objetivo:** Impedir que una cabina sea reservada durante un periodo en el que no podrá utilizarse.

Precondiciones

- La cabina existe.

- Recepción y cabinas inició sesión.

Flujo principal

> 1\. Recepción selecciona una cabina.
>
> 2\. Elige Bloquear disponibilidad.
>
> 3\. Selecciona la fecha.
>
> 4\. Indica si el bloqueo corresponde a un intervalo de horas o a todo el día.
>
> 5\. Registra el motivo.
>
> 6\. Confirma.
>
> 7\. TZISCA crea el bloqueo.
>
> 8\. Ese periodo deja de ofrecerse como disponible.
>
> 9\. El sistema registra el bloqueo en el historial de la cabina.

Flujos alternos / excepciones

- Si ya existen reservaciones dentro del periodo solicitado, TZISCA advierte la situación antes de aplicar el bloqueo para que los casos afectados sean revisados.

Postcondiciones

- El intervalo queda marcado como no disponible.

Reglas relacionadas

- Motivos posibles: mantenimiento, limpieza, incidencia, uso interno u otro.

- El historial debe conservar fecha, intervalo, motivo y usuario que realizó el bloqueo.

————————————————————————————

## CU-27 — Gestionar estado operativo de cabina

**Actor principal:** Recepción y cabinas

**Objetivo:** Mantener actualizado el estado real de las cabinas durante la operación diaria.

Precondiciones

- Recepción y cabinas inició sesión.

- La cabina existe.

Flujo principal

> 1\. Recepción selecciona una cabina.
>
> 2\. Consulta su estado actual.
>
> 3\. Cambia el estado según la situación operativa.
>
> 4\. Registra una observación cuando corresponda.
>
> 5\. Guarda el cambio.
>
> 6\. TZISCA actualiza la disponibilidad de la cabina.
>
> 7\. El sistema registra el movimiento en el historial.

Flujos alternos / excepciones

- Si el cambio solicitado corresponde a Desactivada, la acción administrativa principal deberá realizarla el Administrador general.

Postcondiciones

- El estado operativo de la cabina queda actualizado y se refleja en las validaciones de disponibilidad.

Reglas relacionadas

- Una cabina En mantenimiento o Fuera de servicio no se ofrece para nuevas reservaciones.

- Recepción maneja principalmente los estados operativos diarios; Desactivada pertenece al control administrativo del catálogo.

————————————————————————————

## CU-28 — Cancelar tratamiento

**Actor principal:** Recepción y cabinas

**Objetivo:** Permitir que Recepción y cabinas cancele únicamente un tratamiento de una reservación.

Precondiciones

- Recepción y cabinas inició sesión.

- La reservación existe.

- El tratamiento puede cancelarse.

- Si existe un pago asociado, Recepción puede consultar el importe y estado correspondiente.

Flujo principal

> 1\. Recepción busca la reservación.
>
> 2\. Selecciona el tratamiento.
>
> 3\. Elige Cancelar tratamiento.
>
> 4\. Registra el motivo.
>
> 5\. Confirma la acción.
>
> 6\. El tratamiento cambia a Cancelado.
>
> 7\. TZISCA libera cabina, horario y proveedor asignado.
>
> 8\. Los demás tratamientos permanecen activos.

9\. TZISCA revisa el Pago relacionado y verifica si se encuentra en estado Pagado.

10\. Si existe un Pago Pagado, el sistema consulta la política aprobada para determinar si corresponde una devolución de tipo PARCIAL y cuál es el importe reembolsable.

11\. Solo cuando la política aplicable autoriza una devolución y existe un importe reembolsable, Recepción continúa con CU-43 — Procesar devolución por cancelación.

> 12\. El sistema registra usuario, fecha, hora, motivo e impacto económico.

Flujos alternos / excepciones

- Si el tratamiento no puede cancelarse, TZISCA informa la restricción.

- Si no existe un Pago Pagado, no hay una política aprobada aplicable o no existe importe reembolsable, TZISCA registra la condición económica sin iniciar CU-43.

Postcondiciones

- Solo el tratamiento seleccionado queda cancelado y sus recursos son liberados.

- El Pago conserva su trazabilidad y, cuando se haya iniciado CU-43, la Devolucion conserva su tipo y estado.

Reglas relacionadas

- Recepción y cabinas es el rol operativo principal para cancelaciones.

————————————————————————————

## CU-29 — Cancelar reservación completa

**Actor principal:** Recepción y cabinas

**Objetivo:** Permitir que Recepción y cabinas cancele todos los tratamientos activos de una reservación.

Precondiciones

- Recepción y cabinas inició sesión.

- La reservación existe.

- Si existe un pago asociado, Recepción puede consultar su importe, estado y devoluciones previas.

Flujo principal

> 1\. Recepción abre la reservación.
>
> 2\. Selecciona Cancelar reservación.
>
> 3\. TZISCA muestra los tratamientos afectados.
>
> 4\. Recepción registra el motivo.
>
> 5\. Confirma la cancelación.
>
> 6\. Los tratamientos activos pasan a Cancelado.
>
> 7\. TZISCA libera cabinas, horarios y proveedores.

8\. TZISCA revisa el Pago asociado y verifica si se encuentra en estado Pagado.

9\. Si existe un Pago Pagado, el sistema consulta la política aprobada para determinar si corresponde una devolución de tipo TOTAL o PARCIAL y cuál es el importe reembolsable.

10\. Solo cuando la política aplicable autoriza una devolución y existe un importe reembolsable, Recepción continúa con CU-43 — Procesar devolución por cancelación.

> 11\. El sistema registra usuario, fecha, hora, motivo e impacto económico.

Flujos alternos / excepciones

- Si existen tratamientos completados, se conserva su estado histórico y la cancelación aplica únicamente a los elementos que todavía puedan cancelarse.

- Si una devolución iniciada no puede completarse de inmediato, conserva el estado de devolución correspondiente para seguimiento sin revertir la cancelación.

Postcondiciones

- La reservación deja de tener tratamientos activos reservados.

- El Pago conserva su trazabilidad y, cuando corresponda, la Devolucion conserva su tipo y estado.

Reglas relacionadas

- Recepción puede consultar datos básicos del cliente necesarios para atender solicitudes: nombre, teléfono, correo, reservaciones actuales e historial de reservaciones.

————————————————————————————

# 8. Casos de uso del Proveedor de tratamiento y automatizaciones relacionadas

## CU-30 — Consultar mi agenda

**Actor principal:** Proveedor de tratamiento

**Objetivo:** Permitir que el proveedor consulte los tratamientos que el Administrador general le ha asignado, organizados por fecha y hora.

Precondiciones

- El proveedor inició sesión.

- Su cuenta está activa.

Flujo principal

> 1\. El proveedor entra a Mi agenda.
>
> 2\. TZISCA identifica al proveedor autenticado.
>
> 3\. Consulta únicamente los tratamientos asignados a ese proveedor.
>
> 4\. Los servicios se muestran organizados por fecha y hora.
>
> 5\. El proveedor puede seleccionar un día específico.
>
> 6\. TZISCA muestra los servicios programados para ese día.
>
> 7\. El proveedor puede abrir un tratamiento para consultar su detalle.

Flujos alternos / excepciones

- Si no tiene tratamientos asignados para el periodo seleccionado, el sistema informa que no existen servicios programados.

- Si un tratamiento fue cancelado, puede permanecer visible en el historial, claramente identificado como Cancelado.

Postcondiciones

- El proveedor conoce qué servicios debe atender y en qué horario.

Reglas relacionadas

- Un proveedor no puede consultar la agenda de otros proveedores.

- El proveedor no elige qué reservaciones desea atender.

————————————————————————————

## CU-31 — Consultar detalle de tratamiento asignado

**Actor principal:** Proveedor de tratamiento

**Objetivo:** Permitir al proveedor consultar la información necesaria para atender correctamente un tratamiento asignado.

Precondiciones

- El proveedor inició sesión.

- El tratamiento está asignado a ese proveedor.

- La reservación existe.

Flujo principal

> 1\. El proveedor abre su agenda.
>
> 2\. Selecciona un tratamiento.
>
> 3\. TZISCA valida que el tratamiento realmente le pertenezca.
>
> 4\. El sistema muestra nombre del tratamiento, descripción básica, fecha, hora de inicio, duración, hora estimada de finalización, cabina, número de personas, estado y datos básicos del cliente necesarios para la atención.
>
> 5\. El proveedor consulta la información antes de atender.

Flujos alternos / excepciones

- Si el tratamiento dejó de estar asignado al proveedor por una reasignación, TZISCA actualiza su agenda e impide continuar con ese servicio.

- Si el tratamiento fue cancelado, el sistema informa la situación y no permite iniciar atención.

Postcondiciones

- El proveedor dispone de la información necesaria para preparar el servicio.

Reglas relacionadas

- El proveedor no necesita acceso completo al perfil administrativo del cliente.

- Solo se muestran los datos necesarios para prestar el servicio.

————————————————————————————

## CU-32 — Marcar tratamiento En atención

**Actor principal:** Proveedor de tratamiento

**Objetivo:** Registrar que el proveedor comenzó a prestar el tratamiento al cliente.

Precondiciones

- El proveedor inició sesión.

- El tratamiento está asignado a ese proveedor.

- El tratamiento tiene estado Confirmado.

- El tratamiento no está cancelado.

Flujo principal

> 1\. El proveedor abre el tratamiento asignado.
>
> 2\. Selecciona Iniciar atención.
>
> 3\. TZISCA solicita confirmación.
>
> 4\. El proveedor confirma.
>
> 5\. El sistema registra fecha y hora de inicio.
>
> 6\. El estado cambia de Confirmado a En atención.
>
> 7\. TZISCA registra quién realizó el cambio.

Flujos alternos / excepciones

- Si el tratamiento está Cancelado o Completado, el sistema no permite iniciar atención.

- Si el proveedor ya no es el asignado, TZISCA impide la operación.

Postcondiciones

- El tratamiento queda identificado como un servicio actualmente en curso.

Reglas relacionadas

- Solo el proveedor asignado puede iniciar la atención.

————————————————————————————

## CU-33 — Marcar tratamiento Completado

**Actor principal:** Proveedor de tratamiento

**Objetivo:** Registrar que el servicio terminó y fue atendido.

Precondiciones

- El proveedor inició sesión.

- El tratamiento le pertenece.

- El tratamiento se encuentra En atención.

Flujo principal

> 1\. El proveedor abre el tratamiento.
>
> 2\. Selecciona Finalizar atención.
>
> 3\. TZISCA solicita confirmación.
>
> 4\. El proveedor confirma.
>
> 5\. El sistema registra la hora de finalización real.
>
> 6\. El tratamiento cambia de En atención a Completado.
>
> 7\. TZISCA registra el movimiento en el historial.

Flujos alternos / excepciones

- Si el tratamiento no se encuentra En atención, TZISCA no permite marcarlo como Completado.

Postcondiciones

- El tratamiento queda registrado como atendido y conserva su información histórica.

Reglas relacionadas

- Solo el proveedor asignado puede completar el tratamiento.

- Un tratamiento completado no se elimina.

————————————————————————————

## CU-34 — Registrar indisponibilidad

**Actor principal:** Proveedor de tratamiento

**Objetivo:** Permitir que un proveedor informe que no podrá trabajar durante determinadas fechas u horarios.

Precondiciones

- El proveedor inició sesión.

- Su cuenta está activa.

Flujo principal

> 1\. El proveedor entra a la sección de disponibilidad.
>
> 2\. Selecciona Registrar indisponibilidad.
>
> 3\. Indica fecha de inicio y fecha de finalización.
>
> 4\. Si corresponde, especifica un intervalo de horas.
>
> 5\. Selecciona o captura el motivo.
>
> 6\. Confirma la indisponibilidad.
>
> 7\. TZISCA registra el periodo.
>
> 8\. El proveedor deja de mostrarse como disponible para nuevas asignaciones durante ese intervalo.
>
> 9\. El sistema revisa si ya tiene tratamientos asignados en el periodo afectado.

Flujos alternos / excepciones

- Si no tiene tratamientos asignados en ese periodo, TZISCA únicamente registra la indisponibilidad.

- Si sí tiene tratamientos asignados, el sistema activa la búsqueda automática de proveedores sustitutos.

Postcondiciones

- El proveedor queda marcado como no disponible durante el periodo registrado.

Reglas relacionadas

- Motivos posibles: incapacidad, permiso, ausencia, situación personal u otro.

- El proveedor no cancela directamente las reservaciones por esta causa; registra su indisponibilidad.

- La indisponibilidad se conserva en un historial.

————————————————————————————

## CU-35 — Buscar proveedor sustituto automáticamente

**Actor principal:** Sistema TZISCA; actores relacionados: Cliente, Proveedor afectado y Administrador general

**Objetivo:** Intentar mantener un tratamiento reservado cuando el proveedor originalmente asignado deja de estar disponible.

Precondiciones

- Existe un tratamiento Confirmado.

- El proveedor asignado quedó indisponible.

- Existen otros proveedores registrados.

Flujo principal

> 1\. TZISCA identifica los tratamientos afectados por la indisponibilidad.
>
> 2\. Para cada tratamiento consulta qué proveedores están autorizados para realizarlo.
>
> 3\. Descarta al proveedor original.
>
> 4\. Descarta proveedores inactivos.
>
> 5\. Descarta proveedores con otra atención que se traslape con el horario.
>
> 6\. Descarta proveedores que tengan una indisponibilidad registrada.
>
> 7\. Obtiene los proveedores compatibles y disponibles.
>
> 8\. Selecciona un candidato sustituto disponible.
>
> 9\. Registra una propuesta de reasignación.
>
> 10\. Informa al cliente que el proveedor originalmente asignado no podrá atenderlo y que existe una alternativa para conservar el mismo tratamiento, fecha y hora.
>
> 11\. El cliente decide si acepta al nuevo proveedor, cancela únicamente el tratamiento afectado o cancela toda la reservación.

Flujos alternos / excepciones

- Si el cliente acepta, TZISCA elimina la asignación anterior, asigna el nuevo proveedor, actualiza su agenda y conserva cabina, fecha y hora.

- Si el cliente cancela solo el tratamiento, TZISCA cambia su estado a Cancelado y libera cabina, horario y asignaciones.

- Si el cliente cancela toda la reservación, TZISCA cancela los tratamientos activos y libera sus recursos.

- Si no existe proveedor sustituto, continúa CU-36.

Postcondiciones

- El tratamiento queda reasignado o se procesa la decisión de cancelación del cliente.

- Si la decisión del Cliente implica cancelar un tratamiento o toda la Reservacion, CU-43 se aplica únicamente cuando existe un Pago Pagado, una política aprobada aplicable y un importe reembolsable.

Reglas relacionadas

- La reasignación automática se activa por una indisponibilidad del proveedor.

- La sustitución final debe ser informada al cliente y queda sujeta a su decisión.

- El medio técnico exacto de notificación al cliente queda por definir.

————————————————————————————

## CU-36 — Gestionar caso sin proveedor sustituto

**Actor principal:** Sistema TZISCA; actor relacionado: Cliente

**Objetivo:** Gestionar la situación en la que ningún proveedor compatible puede cubrir un tratamiento afectado.

Precondiciones

- El proveedor original está indisponible.

- TZISCA ejecutó la búsqueda automática.

- No existe otro proveedor válido para la misma fecha y hora.

Flujo principal

> 1\. TZISCA determina que no existe sustituto disponible.
>
> 2\. Informa al cliente que el proveedor no podrá prestar el servicio.
>
> 3\. Indica que no se encontró una alternativa para conservar el mismo horario.
>
> 4\. El cliente decide si cancela solo el tratamiento afectado o cancela toda la reservación.
>
> 5\. TZISCA procesa la decisión y libera los recursos correspondientes cuando exista cancelación.

Flujos alternos / excepciones

- La búsqueda automática de otro horario no forma parte de este caso de uso definido actualmente.

Postcondiciones

- El cliente queda informado y el sistema registra la decisión adoptada.

- Las cancelaciones provocadas por falta de proveedor sustituto deben liberar los recursos y revisar el Pago. CU-43 se aplica únicamente cuando existe un Pago Pagado, una política aprobada aplicable y un importe reembolsable.

Reglas relacionadas

- No se debe reasignar a un proveedor incompatible o no disponible.

————————————————————————————

## CU-37 — Consultar historial de indisponibilidades

**Actor principal:** Proveedor de tratamiento; actor secundario: Administrador general

**Objetivo:** Permitir consultar los periodos de indisponibilidad registrados anteriormente.

Precondiciones

- El usuario correspondiente inició sesión.

Flujo principal

> 1\. El usuario entra al historial de disponibilidad.
>
> 2\. TZISCA muestra los registros existentes.
>
> 3\. Cada registro presenta fecha de inicio, fecha final, horario cuando aplique, motivo y fecha de registro.
>
> 4\. El proveedor consulta su propio historial.
>
> 5\. El Administrador general puede utilizar la información para seguimiento operativo.

Flujos alternos / excepciones

- Si no existen registros, TZISCA muestra que no hay indisponibilidades históricas.

Postcondiciones

- El usuario dispone del historial de periodos no disponibles.

Reglas relacionadas

- El historial se conserva aunque el periodo ya haya terminado.

————————————————————————————

## CU-38 — Consultar historial de atención

**Actor principal:** Proveedor de tratamiento

**Objetivo:** Permitir que el proveedor consulte los tratamientos que ya atendió.

Precondiciones

- El proveedor inició sesión.

Flujo principal

> 1\. El proveedor entra a Historial.
>
> 2\. TZISCA muestra tratamientos anteriores asociados al proveedor.
>
> 3\. Puede consultar la información por fecha.
>
> 4\. Para cada registro visualiza tratamiento, fecha, cabina, estado, hora de inicio y hora de finalización.

Flujos alternos / excepciones

- Si el proveedor aún no tiene servicios completados, el sistema muestra un historial vacío.

Postcondiciones

- El proveedor puede distinguir entre su agenda pendiente y los servicios que ya atendió.

Reglas relacionadas

- La información completada debe conservarse como historial operativo.

————————————————————————————

# 9. Casos de uso de pagos y devoluciones

## CU-39 — Realizar pago de reservación

Actor principal: Cliente

Objetivo: Permitir que el Cliente pague el importe requerido de una Reservacion EN_PROCESO identificada antes de su confirmación definitiva.

Precondiciones

- El Cliente inició sesión.

- Existe una Reservacion en estado EN_PROCESO con id_reservacion.

- La Reservacion contiene al menos un ReservacionTratamiento en estado PENDIENTE y tiene calculado el importe aplicable.

- Las cabinas y horarios requeridos se encuentran protegidos por bloqueos temporales vigentes.

Flujo principal

1\. El Cliente revisa la Reservacion EN_PROCESO identificada por id_reservacion y su importe total.

2\. Selecciona la opción Pagar o Continuar al pago.

3\. TZISCA crea un Pago en estado Pendiente relacionado con id_reservacion.

4\. El sistema muestra o utiliza los medios de pago configurados para el spa.

5\. El Cliente proporciona o confirma la información requerida por el medio de pago.

6\. TZISCA cambia el Pago a Procesando y envía la operación al mecanismo o proveedor de pago configurado.

7\. El sistema recibe y registra el resultado de la operación externa.

8\. Si el pago es aprobado, TZISCA cambia el Pago a Pagado y registra importe, fecha, hora, medio y referencia de transacción cuando exista.

9\. TZISCA conserva la relación del Pago con id_reservacion y la trazabilidad completa del intento.

10\. El sistema regresa a CU-11 para revalidar la disponibilidad y, únicamente si continúa válida, completar la confirmación definitiva.

Flujos alternos / excepciones

- Si el mecanismo externo rechaza la operación o se produce un error, TZISCA cambia el Pago a Fallido, conserva la trazabilidad del intento y no confirma la Reservacion. El Cliente puede reintentar mientras los bloqueos temporales continúen vigentes.

- Si el Cliente cancela el proceso de pago, el Pago queda Cancelado, la Reservacion permanece EN_PROCESO y no se confirma.

- Si un bloqueo temporal expira, TZISCA lo libera, revalida la disponibilidad y no confirma automáticamente. Si el Pago ya está Pagado y se perdió disponibilidad, no confirma el ReservacionTratamiento afectado, informa al Cliente, conserva el Pago y su trazabilidad y remite el caso al tratamiento económico definido por las políticas o por conciliación; mientras esa política esté pendiente, no se presume una devolución automática.

- Si el sistema recibe respuestas repetidas de una misma operación, evita registrar cobros duplicados y conserva la trazabilidad técnica.

Postcondiciones

- El Pago queda registrado y relacionado con la Reservacion EN_PROCESO mediante id_reservacion.

- Un Pago Pagado permite regresar a CU-11, pero no confirma por sí mismo la Reservacion ni sustituye la revalidación final de disponibilidad.

Reglas relacionadas

- Los únicos estados de Pago son Pendiente, Procesando, Pagado, Fallido, Cancelado, Reembolsado y Reembolsado parcialmente; un rechazo externo se registra como causa de Fallido.

- La información sensible del medio de pago deberá manejarse de acuerdo con el mecanismo de pago utilizado y no deberá exponerse innecesariamente en TZISCA.

- TZISCA conserva la trazabilidad de cada intento de Pago y evita duplicidades.

————————————————————————————

## CU-40 — Consultar estado y detalle del pago

Actor principal: Cliente

Objetivo: Permitir que el Cliente consulte el estado y la información trazable del pago asociado a una de sus reservaciones.

Precondiciones

- El Cliente inició sesión.

- Existe una reservación o intento de reservación asociado a su cuenta con información de pago registrada.

Flujo principal

1\. El Cliente entra a Mis reservaciones.

2\. Selecciona una reservación.

3\. TZISCA muestra el estado general del pago.

4\. El Cliente selecciona Ver detalle del pago.

5\. El sistema muestra importe, estado, fecha y hora, medio de pago identificado de forma segura y referencia de transacción cuando exista.

6\. Si existen devoluciones, TZISCA muestra su tipo PARCIAL o TOTAL, su importe, fecha y estado de devolución.

Flujos alternos / excepciones

- Si la reservación no tiene un pago registrado, TZISCA informa que no existe información de pago asociada.

- Si una Devolucion está PENDIENTE o FALLIDA, el sistema muestra ese estado sin presentarla como COMPLETADA.

Postcondiciones

- El Cliente conoce el estado económico de su reservación sin modificarlo.

Reglas relacionadas

- El Cliente solo podrá consultar pagos y devoluciones asociados a sus propias reservaciones.

- No se mostrarán datos sensibles completos del instrumento de pago.

————————————————————————————

## CU-41 — Consultar y gestionar pagos

Actor principal: Administrador general

Objetivo: Permitir al Administrador general consultar pagos, identificar incidencias y gestionar su seguimiento administrativo sin perder trazabilidad.

Precondiciones

- El Administrador general inició sesión.

- Tiene permisos para consultar y gestionar pagos.

Flujo principal

1\. El Administrador entra al módulo de pagos.

2\. TZISCA muestra los pagos registrados.

3\. El Administrador puede filtrar por reservación, cliente, fecha, estado o referencia.

4\. Selecciona un pago.

5\. El sistema muestra importe, estado, reservación relacionada, cliente, fecha, medio, referencia, intentos y devoluciones asociadas.

6\. El Administrador revisa Pagos en estado Pendiente, Procesando, Pagado, Fallido, Cancelado, Reembolsado o Reembolsado parcialmente, así como las devoluciones relacionadas. Un rechazo externo se consulta como causa de un Pago Fallido.

7\. Cuando la operación lo permita, registra una observación, validación administrativa o acción de seguimiento.

8\. TZISCA conserva quién realizó la acción, fecha, hora y motivo u observación.

Flujos alternos / excepciones

- Si no existe el pago buscado, TZISCA muestra un resultado vacío informativo.

- Si una operación depende del proveedor de pago y no puede resolverse desde TZISCA, el sistema conserva la incidencia para seguimiento sin alterar arbitrariamente el resultado original.

Postcondiciones

- La información de pago queda consultada o actualizada de acuerdo con los permisos administrativos y conserva trazabilidad.

Reglas relacionadas

- Cambios administrativos sobre pagos deberán quedar auditados.

- La gestión de pagos no deberá eliminar el historial de intentos, referencias ni devoluciones.

————————————————————————————

## CU-42 — Registrar o validar pago en reservación manual

Actor principal: Recepción y cabinas

Objetivo: Permitir que Recepción y cabinas registre o valide el Pago de una Reservacion manual EN_PROCESO antes de su confirmación definitiva.

Precondiciones

- Recepción y cabinas inició sesión.

- Existe una Reservacion manual EN_PROCESO con id_reservacion, ReservacionTratamiento en PENDIENTE e importe calculado cuando el pago es requerido.

Flujo principal

1\. Recepción revisa la Reservacion manual EN_PROCESO, su id_reservacion y el importe calculado.

2\. Selecciona la opción Registrar o validar pago.

3\. Indica el medio de pago utilizado o selecciona la operación de pago correspondiente.

4\. TZISCA solicita la información mínima necesaria para identificar y comprobar el pago.

5\. Recepción registra el pago recibido o valida el resultado disponible usando los estados normalizados de Pago.

6\. TZISCA guarda importe, fecha, hora, medio, referencia cuando exista, estado y usuario responsable.

7\. El Pago queda relacionado con id_reservacion y conserva su trazabilidad.

8\. Si el Pago queda Pagado, Recepción regresa a CU-23 para revalidar la disponibilidad y confirmar la Reservacion; el Pago Pagado no la confirma por sí mismo.

Flujos alternos / excepciones

- Si la operación externa es rechazada o falla, el Pago queda Fallido. TZISCA conserva la trazabilidad y no confirma la Reservacion.

- Si el Pago se realiza por un medio externo o presencial, Recepción registra la referencia u observación disponible y utiliza uno de los estados normalizados.

- Si se detecta un pago previamente registrado para la misma operación, TZISCA deberá advertirlo para evitar duplicidad.

Postcondiciones

- El Pago queda registrado o validado, asociado mediante id_reservacion y disponible para continuar el flujo de CU-23.

Reglas relacionadas

- Recepción no puede presentar como Pagado un Pago Pendiente, Procesando, Fallido, Cancelado o inexistente. Un rechazo externo se registra como causa de Fallido.

- Toda validación manual registra al usuario responsable y conserva la trazabilidad de los cambios de estado.

————————————————————————————

## CU-43 — Procesar devolución por cancelación

Actor principal: Sistema TZISCA / Administrador general / Recepción y cabinas

Objetivo: Gestionar una Devolucion de tipo PARCIAL o TOTAL cuando una cancelación tenga un Pago Pagado y la política aprobada establezca un importe reembolsable.

Precondiciones

- Existe una cancelación de tratamiento o reservación.

- Existe un Pago en estado Pagado relacionado con la Reservacion.

- Existe una política aprobada que permite determinar el importe reembolsable; si la política está pendiente, no se presume una devolución automática.

Flujo principal

1\. TZISCA identifica la cancelación y el Pago original relacionado.

2\. Determina si la cancelación afecta un ReservacionTratamiento específico o toda la Reservacion.

3\. Calcula el importe reembolsable conforme a la política aprobada.

4\. TZISCA asigna a la Devolucion el tipo PARCIAL o TOTAL, según el alcance económico de la cancelación.

5\. El sistema crea la Devolucion en estado PENDIENTE y, cuando se requiere intervención operativa, el Administrador general o Recepción valida la operación permitida.

6\. Al iniciar el reembolso, TZISCA cambia la Devolucion a PROCESANDO y solicita la operación al mecanismo de pago configurado o registra el trámite manual.

7\. El sistema registra importe, fecha, hora, referencia cuando exista y usuario responsable cuando aplique.

8\. Si el reembolso concluye correctamente, TZISCA cambia la Devolucion a COMPLETADA y actualiza el Pago a Reembolsado o Reembolsado parcialmente, según corresponda.

9\. TZISCA conserva la relación entre Pago, Devolucion, Reservacion y, para una devolución de tipo PARCIAL, el ReservacionTratamiento afectado.

Flujos alternos / excepciones

- Si la política aprobada indica que no existe importe reembolsable, TZISCA registra la evaluación y no crea una Devolucion.

- Si el reembolso no puede completarse, la Devolucion queda FALLIDA para seguimiento sin eliminar el registro de cancelación ni el Pago original.

- Si la devolución se cancela antes de completarse, la Devolucion queda CANCELADA y conserva su trazabilidad.

- Si ya existe una Devolucion COMPLETADA de tipo TOTAL, TZISCA impide otra devolución del mismo importe; si existen devoluciones parciales previas, considera el importe ya reembolsado.

Postcondiciones

- La Devolucion conserva por separado su tipo, PARCIAL o TOTAL, y su estado: PENDIENTE, PROCESANDO, COMPLETADA, FALLIDA o CANCELADA.

- El Pago, la Reservacion, la cancelación y la Devolucion conservan una relación trazable.

Reglas relacionadas

- La suma de los importes de las Devoluciones en estado COMPLETADA no puede superar el importe efectivamente Pagado y disponible para reembolso.

- Toda Devolucion conserva tipo, fecha, importe, estado, referencia y responsable cuando corresponda.

- Las devoluciones derivadas de CU-13, CU-14, CU-21, CU-28, CU-29, CU-35 o CU-36 siguen CU-43 únicamente cuando existe un Pago Pagado, una política aprobada aplicable y un importe reembolsable.

————————————————————————————

# 10. Resumen de cobertura funcional

| **Bloque**                       | **Rango**                    | **Cantidad** |
|----------------------------------|------------------------------|--------------|
| Cliente                          | CU-01 a CU-14, CU-39 a CU-40 | 16           |
| Administrador general            | CU-15 a CU-22, CU-41         | 9            |
| Recepción y cabinas              | CU-23 a CU-29, CU-42         | 8            |
| Proveedor y automatizaciones     | CU-30 a CU-38                | 9            |
| Pagos y devoluciones compartidas | CU-43                        | 1            |
| Total                            |                              | 43           |

Con estos 43 casos de uso queda documentado el alcance funcional principal acordado para TZISCA, incluyendo el módulo obligatorio de pagos y la relación entre reservaciones, pagos, cancelaciones y devoluciones. Los casos CU-01 a CU-38 conservan su numeración original y los nuevos casos se agregan a partir de CU-39 para evitar inconsistencias con la documentación existente. La existencia de un caso de uso no implica que todos deban implementarse en el mismo sprint; la priorización del MVP y de los siguientes incrementos deberá realizarse utilizando este documento como referencia funcional.
