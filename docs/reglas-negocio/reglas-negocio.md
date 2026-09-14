# TZISCA

Reglas de Negocio

Sistema de Citas, Recomendación y Gestión de Cabinas para Spa

Este documento conserva RN-01 a RN-90, actualizadas para el modelo vigente, e incorpora RN-91 a RN-108 con las decisiones aprobadas. La referencia oficial es el modelo de 19 entidades distribuido en los schemas seguridad, catalogo, reservas, operacion y pagos.

## 1. Usuarios, clientes y roles

RN-01. Todo usuario deberá autenticarse para acceder a funciones privadas del sistema.

RN-02. Cada Usuario tendrá permisos asociados a un Rol y la autorización deberá aplicarse por rol o permiso.

RN-03. Los roles funcionales son Cliente, Administrador general, Recepción y cabinas y Proveedor de tratamiento.

RN-04. El Cliente solo podrá consultar y gestionar sus propias Citas, pagos, cancelaciones y devoluciones.

RN-05. El Administrador general tendrá acceso a configuración, usuarios, tratamientos, paquetes, cabinas, proveedores y reportes según sus permisos.

RN-06. Recepción y cabinas tendrá acceso a agenda, disponibilidad, Citas manuales, cancelaciones, pagos operativos y estados de cabina.

RN-07. El Proveedor solo podrá consultar y actualizar las Citas que tenga asignadas.

## 2. Tratamientos y paquetes

RN-08. Todo Tratamiento deberá tener nombre, descripción, duración, precio base por persona y estado de activación.

RN-09. Solo los tratamientos activos podrán mostrarse al cliente y agregarse a un Carrito.

RN-10. La compatibilidad de una cabina se determinará con los requisitos del Tratamiento y las características registradas de la Cabina.

RN-11. El mismo Tratamiento podrá originar varias Citas; cada Cita será gestionada de forma independiente.

## 3. Cabinas

RN-12. Toda Cabina tendrá una capacidad máxima definida.

RN-13. Una Cabina solo podrá asignarse a una Cita cuando sea compatible con el Tratamiento.

RN-14. La capacidad de la Cabina deberá ser igual o superior al número de personas de la Cita.

RN-15. Cabina.activo y Cabina.estado son conceptos independientes: activo habilita el registro y estado representa la condición operativa.

RN-16. Una Cabina no podrá asignarse a Citas cuyos intervalos se traslapen.

## 4. Recomendación de cabinas

RN-17. TZISCA solo recomendará cabinas que cumplan compatibilidad, capacidad, estado operativo y disponibilidad.

RN-18. Después de los criterios obligatorios, el sistema podrá usar las PreferenciaCliente para ordenar recomendaciones.

RN-19. La ausencia de preferencias no impedirá la recomendación.

RN-20. La recomendación será orientativa y el Cliente podrá elegir otra Cabina elegible.

RN-21. La especialización y la prioridad configurada solo se aplicarán después de cumplir los criterios obligatorios.

## 5. Carrito y selección

RN-22. Un Carrito podrá agrupar uno o varios tratamientos seleccionados antes de generar sus Citas.

RN-23. Cada selección conservará tratamiento, número de personas, horario y cabina propuesta sin crear una Cita confirmada.

RN-24. Las Citas generadas desde un mismo Carrito no estarán obligadas a ser consecutivas.

RN-25. Antes de confirmar, el Cliente podrá agregar, quitar o modificar selecciones.

RN-26. Agregar un Tratamiento al Carrito no confirma una Cita.

## 6. Disponibilidad

RN-27. La disponibilidad deberá calcularse durante toda la duración del Tratamiento.

RN-28. Un horario estará disponible solo si no se traslapa con Citas confirmadas, bloqueos vigentes, estados operativos incompatibles o indisponibilidad del Proveedor.

RN-29. Si el horario no está disponible, TZISCA deberá ofrecer otros horarios que sí cumplan las reglas.

## 7. Bloqueo temporal

RN-30. Al seleccionar Cabina, fecha y hora válidas, TZISCA deberá crear un bloqueo temporal asociado al Carrito o a la Cita pendiente.

RN-31. El bloqueo temporal tendrá una vigencia exacta de 15 minutos.

RN-32. Mientras el bloqueo esté vigente, el recurso no podrá ofrecerse a otra solicitud que produzca conflicto.

RN-33. Si la Cita requiere pago, el bloqueo se mantendrá solo durante su vigencia y no equivaldrá a una Cita confirmada.

RN-34. Al vencer el bloqueo, TZISCA liberará el recurso y revalidará la disponibilidad antes de continuar.

RN-35. Si el Cliente abandona el Carrito, los bloqueos vigentes deberán liberarse.

RN-36. Si cambia fecha, hora o Cabina, TZISCA liberará el bloqueo anterior antes de crear otro.

RN-37. Cada selección del Carrito podrá tener un bloqueo independiente.

## 8. Confirmación de citas

RN-38. Toda Cita creada antes del pago deberá iniciar en estado PENDIENTE.

RN-39. Una Cita PENDIENTE podrá confirmarse cuando recursos, horario y proveedor sean válidos y el pago requerido esté PAGADO.

RN-40. La confirmación deberá revalidar disponibilidad y convertir el estado de la Cita a CONFIRMADA en una operación consistente.

RN-41. Si una Cita de un Carrito presenta conflicto, las demás no deberán cancelarse automáticamente.

## 9. Estados de cita

RN-42. Los estados de Cita serán PENDIENTE, CONFIRMADA, EN_ATENCION, COMPLETADA, CANCELADA y EXPIRADA.

RN-43. Una Cita CANCELADA o EXPIRADA no podrá pasar a EN_ATENCION ni COMPLETADA.

RN-44. Una Cita CONFIRMADA podrá pasar a EN_ATENCION cuando inicie el servicio.

RN-45. Una Cita EN_ATENCION podrá pasar a COMPLETADA cuando termine el servicio.

## 10. Proveedores

RN-46. La autorización de un Proveedor para realizar un Tratamiento se registrará mediante TratamientoProveedor.

RN-47. La asignación inicial de Proveedor a una Cita será realizada por el Administrador general.

RN-48. Un Proveedor no podrá asignarse a Citas con intervalos traslapados.

RN-49. El Proveedor solo podrá consultar las Citas que tenga asignadas.

RN-50. DisponibilidadProveedor registrará periodos DISPONIBLE o NO_DISPONIBLE.

RN-51. Un Proveedor NO_DISPONIBLE no deberá aparecer como candidato durante el intervalo afectado.

## 11. Sustitución de proveedor

RN-52. Si un Proveedor queda no disponible para una Cita asignada, TZISCA buscará un sustituto.

RN-53. El sustituto deberá estar activo, autorizado mediante TratamientoProveedor y disponible durante todo el intervalo.

RN-54. El Cliente deberá ser informado antes de aplicar una sustitución definitiva.

RN-55. El Cliente podrá aceptar al sustituto o cancelar la Cita afectada.

RN-56. Si no existe sustituto, TZISCA informará al Cliente y mantendrá la trazabilidad de la incidencia.

## 12. Cancelaciones

RN-57. El Cliente podrá cancelar una Cita conforme a la política aprobada y el sistema calculará la devolución aplicable.

RN-58. Recepción y cabinas será el rol principal para cancelaciones operativas.

RN-59. El Administrador general también podrá cancelar Citas según sus permisos.

RN-60. Al cancelar una Cita se liberarán su horario, cabinas y proveedor sin borrar Pago, Cancelacion, Devolucion ni Transaccion.

RN-61. Cancelar una Cita no cancelará automáticamente otras Citas del mismo Carrito.

RN-62. Para modificar una Cita confirmada, el Cliente deberá cancelarla y generar una nueva, salvo ajustes operativos autorizados.

## 13. Operación de cabinas

RN-63. Recepción y cabinas podrá cambiar Cabina.estado conforme a la operación.

RN-64. Todo cambio de estado deberá registrar fecha y motivo cuando corresponda.

RN-65. Antes de cambiar a MANTENIMIENTO o LIMPIEZA, TZISCA deberá advertir sobre Citas activas afectadas.

RN-66. Los estados operativos permitidos son DISPONIBLE, OCUPADA, LIMPIEZA y MANTENIMIENTO.

RN-67. Desactivar Cabina.activo será una acción administrativa distinta de cambiar Cabina.estado.

## 14. Historial y trazabilidad

RN-68. TZISCA conservará la trazabilidad de cancelaciones, pagos, devoluciones, transacciones, disponibilidad de proveedores y estados de cabina y cita.

RN-69. Cada movimiento relevante registrará fecha, hora y responsable cuando exista.

RN-70. Los cambios de Cabina.estado se registrarán en EstadoCabina mediante operacion.TR_Cabina_CambioEstado.

## 15. Reportes

RN-71. Los reportes podrán incluir Citas por día, tratamientos solicitados, cabinas utilizadas, cancelaciones y ocupación.

RN-72. El Administrador general será el rol principal autorizado para consultar reportes.

## 16. Pagos y devoluciones

RN-73. Todo Pago deberá relacionarse con una Cita.

RN-74. El backend calculará y mostrará el total antes de iniciar el Pago.

RN-75. Una Cita que requiera pago solo podrá confirmarse con Pago PAGADO y disponibilidad revalidada.

RN-76. Todo Pago deberá manejar un estado controlado.

RN-77. Los estados de Pago son PENDIENTE, PROCESANDO, PAGADO, FALLIDO, CANCELADO, REEMBOLSADO y REEMBOLSADO_PARCIALMENTE.

RN-78. Un Pago PAGADO deberá registrar monto, fecha, método y referencia.

RN-79. Un Pago FALLIDO no deberá confirmar la Cita.

RN-80. El Cliente podrá reintentar un Pago FALLIDO mientras la Cita y su bloqueo sean válidos.

RN-81. Si el bloqueo venció después de un Pago PAGADO y la disponibilidad se perdió, la Cita no se confirmará y se iniciará la devolución correspondiente.

RN-82. TZISCA no almacenará números completos de tarjeta, CVV ni credenciales bancarias.

RN-83. Una Cancelacion deberá comprobar si existe Pago relacionado.

RN-84. Una Cancelacion podrá generar Devolucion conforme al plazo y causa aprobados.

RN-85. La Devolucion deberá asociarse al Pago y a la Cancelacion que la originó.

RN-86. Toda Devolucion deberá conservar trazabilidad.

RN-87. Una Devolucion registrará monto, porcentaje aplicado, fecha, motivo, estado y responsable cuando corresponda.

RN-88. Pago, Cancelacion, Devolucion y Transaccion no se eliminarán físicamente por cancelar una Cita.

RN-89. El Administrador general podrá consultar y gestionar pagos según sus permisos.

RN-90. Recepción y cabinas podrá consultar el estado de pagos de las Citas que gestione.

## 17. Parámetros y decisiones aprobadas

RN-91. El horario regular de apertura será 09:00.

RN-92. El horario regular de cierre será 20:00 y todo Tratamiento deberá finalizar a más tardar a las 20:00.

RN-93. Los días laborales regulares serán de lunes a sábado.

RN-94. El domingo será no laboral.

RN-95. Festivos, cierres extraordinarios y horarios especiales se registrarán como excepciones operativas de configuración, sin crear una entidad de dominio adicional.

RN-96. Los horarios de inicio se generarán en intervalos de 30 minutos.

RN-97. La anticipación mínima para crear una Cita será de 2 horas.

RN-98. La anticipación máxima para crear una Cita será de 60 días.

RN-99. Todo bloqueo temporal vencerá 15 minutos después de su creación.

RN-100. La tolerancia de llegada será de 15 minutos.

RN-101. Una cancelación realizada 24 horas o más antes del inicio generará devolución del 100 %.

RN-102. Una cancelación realizada entre 6 horas y menos de 24 horas antes del inicio generará devolución del 50 %.

RN-103. Una cancelación realizada con menos de 6 horas de anticipación no generará devolución.

RN-104. Una cancelación atribuible al spa generará devolución del 100 %.

RN-105. No habrá devolución por inasistencia, cancelación con menos de 6 horas, servicio iniciado o servicio completado.

RN-106. Tratamiento.precio_base es por persona; Cita.precio_unitario conserva el precio vigente al reservar; Cita.importe = precio_unitario × numero_personas; el total es la suma de importes y el MVP no aplica cargos adicionales.

RN-107. La autenticación usará ASP.NET Core Identity, JWT como access token y refresh token seguro; Stripe será la pasarela inicial detrás de PaymentService.

RN-108. La recomendación será determinista y ordenará por compatibilidad, capacidad, estado operativo, disponibilidad, preferencias, cabina especializada antes que multifuncional, prioridad configurada e id_cabina ascendente.

## 18. Resumen de reglas por área

| Área | Rango | Cantidad |
| --- | --- | --- |
| Usuarios, clientes y roles | RN-01 a RN-007 | 7 |
| Tratamientos y paquetes | RN-08 a RN-011 | 4 |
| Cabinas | RN-12 a RN-016 | 5 |
| Recomendación de cabinas | RN-17 a RN-021 | 5 |
| Carrito y selección | RN-22 a RN-026 | 5 |
| Disponibilidad | RN-27 a RN-029 | 3 |
| Bloqueo temporal | RN-30 a RN-037 | 8 |
| Confirmación de citas | RN-38 a RN-041 | 4 |
| Estados de cita | RN-42 a RN-045 | 4 |
| Proveedores | RN-46 a RN-051 | 6 |
| Sustitución de proveedor | RN-52 a RN-056 | 5 |
| Cancelaciones | RN-57 a RN-062 | 6 |
| Operación de cabinas | RN-63 a RN-067 | 5 |
| Historial y trazabilidad | RN-68 a RN-070 | 3 |
| Reportes | RN-71 a RN-072 | 2 |
| Pagos y devoluciones | RN-73 a RN-090 | 18 |
| Parámetros y decisiones aprobadas | RN-91 a RN-108 | 18 |
| Total | RN-01 a RN-108 | 108 |

Las decisiones DP-OP-01 a DP-OP-13, DP-EC-01, DP-EC-02 y DP-TEC-01 a DP-TEC-03 quedaron resueltas por RN-91 a RN-108 y ya no están pendientes de aprobación.
