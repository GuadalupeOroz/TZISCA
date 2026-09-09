**TZISCA**

**DISEÑO DE API REST**

Sistema Web de Reservas, Recomendación y Gestión de Cabinas para Spa

| **Documento** | Diseño de API REST — TZISCA |
|----|----|
| **Versión** | 1.0 — Diseño funcional/técnico inicial |
| **Fecha** | 8 de septiembre de 2026 |
| **Arquitectura base** | Frontend Angular → API REST .NET Core → SQL Server |
| **Formato de intercambio** | JSON |
| **Prefijo propuesto** | /api/v1 |
| **Autenticación** | Sesión autenticada/token; mecanismo concreto pendiente de aprobación (DP-TEC-01). No se impone JWT como decisión oficial. |

Nota de versión — documento cerrado. Esta es la versión corregida y cerrada del Diseño de API REST TZISCA v1, actualizada el 08/09/2026 sobre la versión vigente del documento (sin crear una API nueva desde cero). Se mantienen /api/v1, REST, JSON, Angular como frontend, ASP.NET Core como backend, SQL Server como base de datos, y las referencias CU-01 a CU-43 y RN-01 a RN-90. Cambios principales: (1) eliminación de toda referencia a una duración fija de 10 minutos en bloqueos temporales, sustituida por la duración configurable del parámetro operativo aprobado; (2) flujo REST principal corregido para incluir explícitamente POST /reservations (Reservacion EN_PROCESO / ReservacionTratamiento PENDIENTE) antes de POST /payments, y POST /reservations/{id}/confirm (Reservacion CONFIRMADA / ReservacionTratamiento CONFIRMADO) después de la revalidación; (3) POST /reservations ya no confirma ni devuelve CONFIRMADA/PAGADO; (4) POST /payments ya no confía en un monto enviado por Angular, calculado por el backend, con PAYMENT_AMOUNT_MISMATCH como error posible; (5) checklist explícito de POST /reservations/{id}/confirm y manejo de pago FALLIDO, bloqueo expirado y pago PAGADO con disponibilidad perdida (DP-EC-02, sin generar devolución automática); (6) catálogos de estado de Pago, Devolucion, Reservacion y ReservacionTratamiento normalizados en mayúsculas conforme al Diccionario de Datos, y tipo_devolucion como campo propio; (7) eliminación de la definición duplicada de GET /reservations/{id}/payments, conservada una sola vez como PAY-03; (8) catálogo de códigos de error de negocio agregado/normalizado; (9) autenticación y pasarela de pago mantenidas como decisiones pendientes (DP-TEC-01, DP-TEC-02); (10) DTO revisados para coincidir con los estados y campos del Diccionario actualizado; (11) matriz de cobertura CU-01 a CU-43 verificada y corregida; (12) conteo de endpoints recalculado a 66. El documento queda listo como contrato de implementación de ASP.NET Core y Angular.

*Documento derivado del alcance funcional, Casos de Uso CU-01 a CU-43, Reglas de Negocio RN-01 a RN-90 y el Diccionario de Datos TZISCA (versión corregida y cerrada).*

# 1. Propósito y alcance

Este documento define la interfaz REST propuesta para TZISCA. La API separa las responsabilidades de autenticación, usuarios, catálogo, recomendaciones, disponibilidad, carrito, bloqueos temporales, reservaciones, proveedores, asignaciones, pagos, devoluciones y reportes.

La propuesta respeta la separación establecida en el modelo de datos: el carrito es distinto de la reservación; Reservacion funciona como encabezado y ReservacionTratamiento contiene cada servicio de manera independiente; Pago se relaciona con Reservacion y Devolucion con Pago y, cuando corresponde, con un tratamiento específico.

Las rutas y DTO descritos aquí son un diseño de API. Cuando un detalle de implementación no aparece explícitamente en los documentos funcionales, se presenta como convención técnica propuesta y no como una regla de negocio ya aprobada.

# 2. Principios de diseño

- Versionado: /api/v1.

- REST y JSON: recursos representados como JSON; métodos HTTP con semántica estándar.

- Autorización por rol: Cliente, Administrador general, Recepción y cabinas y Proveedor de tratamiento.

- No exponer password_hash ni datos bancarios sensibles.

- No eliminar físicamente reservaciones, pagos, devoluciones ni historiales cuando la documentación exige trazabilidad.

- La disponibilidad se calcula sobre el intervalo completo del tratamiento.

- Los bloqueos temporales duran conforme a la duración configurable del bloqueo temporal definida por el parámetro operativo aprobado, y protegen la selección del cliente.

- Los reintentos de pago generan trazabilidad nueva; no sobrescriben operaciones anteriores.

- Los reportes se calculan desde los datos transaccionales; no se define una entidad Reporte.

# 3. Convenciones HTTP y errores

| **200 OK** | Consulta o actualización procesada correctamente. |
|----|----|
| **201 Created** | Recurso creado correctamente. |
| **204 No Content** | Operación correcta sin cuerpo de respuesta. |
| **400 Bad Request** | Solicitud mal formada o parámetros inválidos. |
| **401 Unauthorized** | Falta autenticación o la sesión/token no es válido. |
| **403 Forbidden** | El usuario está autenticado pero no tiene permisos. |
| **404 Not Found** | Recurso no encontrado o no pertenece al actor. |
| **409 Conflict** | Conflicto de disponibilidad, estado, duplicidad o condición de negocio. |
| **422 Unprocessable Entity** | Datos válidos estructuralmente pero no cumplen una regla de negocio. |
| **500 Internal Server Error** | Error interno no controlado. |

Formato de error propuesto:

> {"code":"RESOURCE_NOT_FOUND","message":"No se encontró el recurso.","details":{}}

Catálogo de códigos de error de negocio (campo "code" del cuerpo de error), normalizado en esta versión:

| BLOCK_EXPIRED | El bloqueo temporal referenciado ya expiró. 409 Conflict. |
|----|----|
| AVAILABILITY_CONFLICT | El intervalo solicitado ya no está disponible al revalidar. 409 Conflict. |
| RESERVATION_NOT_IN_PROCESS | La Reservacion no está en EN_PROCESO (ya fue confirmada, cancelada o expiró). 409 Conflict. |
| PAYMENT_REQUIRED | No existe un pago en PAGADO asociado a la Reservacion. 422 Unprocessable Entity. |
| PAYMENT_NOT_APPROVED | El pago referenciado existe pero no está en PAGADO. 422 Unprocessable Entity. |
| PAYMENT_ALREADY_APPROVED | Ya existe una confirmación previa para esta Reservacion/Pago. 409 Conflict. |
| PAYMENT_AMOUNT_MISMATCH | El monto recibido no coincide con el total calculado por el backend. 422 Unprocessable Entity. |
| PAYMENT_APPROVED_AVAILABILITY_LOST | El pago está PAGADO pero la disponibilidad revalidada ya no existe (DP-EC-02). 409 Conflict. |
| PAYMENT_NOT_FOUND | El pago referenciado no existe o no pertenece al actor. 404 Not Found. |
| REFUND_EXCEEDS_PAYMENT | El monto de la devolución supera el monto efectivamente pagado. 422 Unprocessable Entity. |
| REFUND_ALREADY_COMPLETED | La devolución referenciada ya está COMPLETADA y no admite otra operación. 409 Conflict. |
| PROVIDER_NOT_AVAILABLE | El proveedor no está autorizado, no está activo o tiene un traslape de agenda. 409 Conflict. |

# 4. Roles y abreviaturas

| **CL**      | Cliente                                               |
|-------------|-------------------------------------------------------|
| **AG**      | Administrador general                                 |
| **RC**      | Recepción y cabinas                                   |
| **PR**      | Proveedor de tratamiento                              |
| **SYS**     | Proceso interno de TZISCA                             |
| **Público** | Endpoint accesible sin autenticación, cuando aplique. |

Los casos de uso y reglas son la fuente funcional. Los endpoints automáticos no sustituyen los casos de uso: representan operaciones de backend necesarias para materializar sus flujos.

# 5. Catálogo de endpoints

Se documentan 66 endpoints agrupados en los 14 módulos solicitados. La nomenclatura de DTO es consistente con los recursos del modelo y puede implementarse posteriormente en .NET.

| **Módulo** | **Endpoints** | **Cobertura principal** |
|----|----|----|
| **/auth** | 4 | Registro, inicio/cierre de sesión y usuario autenticado. |
| **/users** | 5 | Usuarios, roles, estados y preferencias. |
| **/treatments** | 5 | Catálogo y compatibilidades tratamiento-cabina. |
| **/cabins** | 6 | Catálogo, ficha, compatibilidades y estado operativo. |
| **/recommendations** | 2 | Recomendación y explicación de cabinas. |
| **/availability** | 2 | Consulta y validación de intervalos. |
| **/cart** | 5 | Selección provisional de tratamientos. |
| **/temporary-blocks** | 3 | Protección temporal de cabinas durante el proceso. |
| **/reservations** | 7 | Reservaciones web/manuales, consulta, confirmación y cancelaciones. |
| **/providers** | 7 | Proveedores, agenda e indisponibilidades. |
| **/provider-assignments** | 5 | Asignaciones iniciales, candidatos y sustituciones. |
| **/payments** | 6 | Pagos, intentos, reintentos y registro manual. |
| **/refunds** | 4 | Devoluciones totales/parciales y consulta. |
| **/reports** | 5 | Indicadores básicos derivados de datos registrados. |

# /auth

## AUTH-01 — POST /auth/register

| **ID** | AUTH-01 |
|----|----|
| **Módulo** | /auth |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/auth/register |
| **Actor autorizado** | Público |
| **Descripción** | Registrar una cuenta de Cliente. |
| **Parámetros** | Sin parámetros de ruta; body con nombre, correo, teléfono y contraseña. |
| **Request** | {"nombre":"Ana","correo":"ana@correo.com","telefono":"9620000000","password":"\*\*\*\*\*\*\*\*"} |
| **Response** | {"id_usuario":101,"nombre":"Ana","correo":"ana@correo.com","rol":"Cliente","activo":true} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400, 409, 422 |
| **Caso de uso relacionado** | CU-01 |
| **Regla de negocio relacionada** | RN-01 a RN-03 |
| **DTO de entrada** | RegisterRequest |
| **DTO de salida** | UserResponse |

## AUTH-02 — POST /auth/login

| **ID** | AUTH-02 |
|----|----|
| **Módulo** | /auth |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/auth/login |
| **Actor autorizado** | Público |
| **Descripción** | Autenticar al usuario y establecer su sesión/token. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"correo":"ana@correo.com","password":"\*\*\*\*\*\*\*\*"} |
| **Response** | {"accessToken":"\<token\>","usuario":{"id_usuario":101,"nombre":"Ana","rol":"Cliente"}} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400, 401 |
| **Caso de uso relacionado** | CU-02 |
| **Regla de negocio relacionada** | RN-01 a RN-03 |
| **DTO de entrada** | LoginRequest |
| **DTO de salida** | LoginResponse |

## AUTH-03 — POST /auth/logout

| **ID** | AUTH-03 |
|----|----|
| **Módulo** | /auth |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/auth/logout |
| **Actor autorizado** | CL/AG/RC/PR |
| **Descripción** | Cerrar la sesión autenticada. |
| **Parámetros** | Header de autenticación. |
| **Request** | {} |
| **Response** | {"message":"Sesión cerrada correctamente."} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401 |
| **Caso de uso relacionado** | CU-02 |
| **Regla de negocio relacionada** | RN-01 a RN-02 |
| **DTO de entrada** | LogoutRequest |
| **DTO de salida** | MessageResponse |

## AUTH-04 — GET /auth/me

| **ID** | AUTH-04 |
|----|----|
| **Módulo** | /auth |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/auth/me |
| **Actor autorizado** | CL/AG/RC/PR |
| **Descripción** | Consultar el usuario autenticado y su rol. |
| **Parámetros** | Header de autenticación. |
| **Request** | No aplica. |
| **Response** | {"id_usuario":101,"nombre":"Ana","correo":"ana@correo.com","telefono":"9620000000","rol":"Cliente","activo":true} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401 |
| **Caso de uso relacionado** | CU-02 |
| **Regla de negocio relacionada** | RN-02 a RN-04 |
| **DTO de entrada** | — |
| **DTO de salida** | CurrentUserResponse |

# /users

## USR-01 — GET /users

| **ID** | USR-01 |
|----|----|
| **Módulo** | /users |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/users |
| **Actor autorizado** | AG |
| **Descripción** | Consultar usuarios registrados. |
| **Parámetros** | Query opcional: rol, activo, page, pageSize. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"id_usuario":101,"nombre":"Ana","correo":"ana@correo.com","rol":"Cliente","activo":true}\],"page":1,"pageSize":20,"total":1} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403 |
| **Caso de uso relacionado** | CU-17 |
| **Regla de negocio relacionada** | RN-02, RN-05 |
| **DTO de entrada** | — |
| **DTO de salida** | UserListResponse |

## USR-02 — GET /users/{id}

| **ID** | USR-02 |
|----|----|
| **Módulo** | /users |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/users/{id} |
| **Actor autorizado** | AG/RC |
| **Descripción** | Consultar datos de un usuario. RC solo para datos necesarios de clientes. |
| **Parámetros** | Path: id; query opcional según permiso. |
| **Request** | No aplica. |
| **Response** | {"id_usuario":101,"nombre":"Ana","correo":"ana@correo.com","telefono":"9620000000","rol":"Cliente","activo":true} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,404 |
| **Caso de uso relacionado** | CU-17 |
| **Regla de negocio relacionada** | RN-04 a RN-06 |
| **DTO de entrada** | — |
| **DTO de salida** | UserResponse |

## USR-03 — PATCH /users/{id}/role

| **ID** | USR-03 |
|----|----|
| **Módulo** | /users |
| **Método HTTP** | PATCH |
| **Ruta** | /api/v1/users/{id}/role |
| **Actor autorizado** | AG |
| **Descripción** | Cambiar el rol de una cuenta. |
| **Parámetros** | Path: id. |
| **Request** | {"id_rol":2} |
| **Response** | {"id_usuario":101,"rol":"Recepción y cabinas","activo":true} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,409 |
| **Caso de uso relacionado** | CU-17 |
| **Regla de negocio relacionada** | RN-02, RN-03, RN-05 |
| **DTO de entrada** | ChangeUserRoleRequest |
| **DTO de salida** | UserResponse |

## USR-04 — PATCH /users/{id}/status

| **ID** | USR-04 |
|----|----|
| **Módulo** | /users |
| **Método HTTP** | PATCH |
| **Ruta** | /api/v1/users/{id}/status |
| **Actor autorizado** | AG |
| **Descripción** | Activar o desactivar una cuenta sin eliminar su historial. |
| **Parámetros** | Path: id. |
| **Request** | {"activo":false} |
| **Response** | {"id_usuario":101,"activo":false} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404 |
| **Caso de uso relacionado** | CU-17 |
| **Regla de negocio relacionada** | RN-02, RN-05 |
| **DTO de entrada** | ChangeUserStatusRequest |
| **DTO de salida** | UserStatusResponse |

## USR-05 — GET /users/me/preferences

| **ID** | USR-05 |
|----|----|
| **Módulo** | /users |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/users/me/preferences |
| **Actor autorizado** | CL |
| **Descripción** | Consultar preferencias opcionales del cliente. |
| **Parámetros** | Header de autenticación. |
| **Request** | No aplica. |
| **Response** | {"objetivo_visita":"relajacion","modalidad_preferida":"pareja","privacidad_preferida":"media","ambiente_preferido":"tranquilo","requiere_accesibilidad":false} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,404 |
| **Caso de uso relacionado** | CU-01/CU-07 |
| **Regla de negocio relacionada** | RN-18 a RN-20 |
| **DTO de entrada** | — |
| **DTO de salida** | ClientPreferencesResponse |

# /treatments

## TRT-01 — GET /treatments

| **ID** | TRT-01 |
|----|----|
| **Módulo** | /treatments |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/treatments |
| **Actor autorizado** | CL/AG |
| **Descripción** | Consultar catálogo de tratamientos; para Cliente solo activos. |
| **Parámetros** | Query: activo, page, pageSize; Cliente no puede solicitar desactivados. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"id_tratamiento":1,"nombre":"Masaje relajante","descripcion":"...","duracion_minutos":60,"precio_base":800,"moneda":"MXN","activo":true}\],"total":1} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403 |
| **Caso de uso relacionado** | CU-03/CU-15 |
| **Regla de negocio relacionada** | RN-08, RN-09 |
| **DTO de entrada** | — |
| **DTO de salida** | TreatmentListResponse |

## TRT-02 — GET /treatments/{id}

| **ID** | TRT-02 |
|----|----|
| **Módulo** | /treatments |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/treatments/{id} |
| **Actor autorizado** | CL/AG |
| **Descripción** | Consultar el detalle de un tratamiento. |
| **Parámetros** | Path: id. |
| **Request** | No aplica. |
| **Response** | {"id_tratamiento":1,"nombre":"Masaje relajante","descripcion":"...","duracion_minutos":60,"beneficios":"...","recomendaciones_generales":"...","restricciones":"...","precio_base":800,"moneda":"MXN","activo":true,"cabinas_compatibles":\[1,5\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,404 |
| **Caso de uso relacionado** | CU-04 |
| **Regla de negocio relacionada** | RN-08 a RN-10 |
| **DTO de entrada** | — |
| **DTO de salida** | TreatmentDetailResponse |

## TRT-03 — POST /treatments

| **ID** | TRT-03 |
|----|----|
| **Módulo** | /treatments |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/treatments |
| **Actor autorizado** | AG |
| **Descripción** | Crear un tratamiento. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"nombre":"Masaje relajante","descripcion":"...","duracion_minutos":60,"beneficios":"...","recomendaciones_generales":"...","restricciones":"...","requiere_proveedor":true,"precio_base":800,"moneda":"MXN","cabinas_compatibles":\[1,5\]} |
| **Response** | {"id_tratamiento":10,"nombre":"Masaje relajante","duracion_minutos":60,"precio_base":800,"activo":true} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,403,409,422 |
| **Caso de uso relacionado** | CU-15 |
| **Regla de negocio relacionada** | RN-08, RN-10 |
| **DTO de entrada** | CreateTreatmentRequest |
| **DTO de salida** | TreatmentResponse |

## TRT-04 — PATCH /treatments/{id}

| **ID** | TRT-04 |
|----|----|
| **Módulo** | /treatments |
| **Método HTTP** | PATCH |
| **Ruta** | /api/v1/treatments/{id} |
| **Actor autorizado** | AG |
| **Descripción** | Editar datos de catálogo sin modificar el historial de reservas. |
| **Parámetros** | Path: id. |
| **Request** | {"descripcion":"Nueva descripción","precio_base":850,"activo":true} |
| **Response** | {"id_tratamiento":10,"descripcion":"Nueva descripción","precio_base":850,"activo":true} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,422 |
| **Caso de uso relacionado** | CU-15 |
| **Regla de negocio relacionada** | RN-08, RN-09 |
| **DTO de entrada** | UpdateTreatmentRequest |
| **DTO de salida** | TreatmentResponse |

## TRT-05 — PUT /treatments/{id}/cabins

| **ID** | TRT-05 |
|----|----|
| **Módulo** | /treatments |
| **Método HTTP** | PUT |
| **Ruta** | /api/v1/treatments/{id}/cabins |
| **Actor autorizado** | AG |
| **Descripción** | Administrar cabinas compatibles y su prioridad/especialización. |
| **Parámetros** | Path: id. |
| **Request** | {"cabinas":\[{"id_cabina":1,"es_especializada":true,"prioridad":1},{"id_cabina":5,"es_especializada":false,"prioridad":2}\]} |
| **Response** | {"id_tratamiento":10,"cabinas_compatibles":\[{"id_cabina":1,"es_especializada":true,"prioridad":1},{"id_cabina":5,"es_especializada":false,"prioridad":2}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,409 |
| **Caso de uso relacionado** | CU-15 |
| **Regla de negocio relacionada** | RN-10, RN-17, RN-21 |
| **DTO de entrada** | SetTreatmentCabinsRequest |
| **DTO de salida** | TreatmentCabinsResponse |

# /cabins

## CAB-01 — GET /cabins

| **ID** | CAB-01 |
|----|----|
| **Módulo** | /cabins |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/cabins |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Consultar cabinas; Cliente recibe opciones reservables. |
| **Parámetros** | Query: tipo, activo, estado_operativo, page, pageSize. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"id_cabina":1,"nombre":"Masaje 01","tipo":"Masaje","capacidad_maxima":2,"estado_operativo":"Disponible","activo":true}\],"total":1} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403 |
| **Caso de uso relacionado** | CU-16 |
| **Regla de negocio relacionada** | RN-12, RN-15 |
| **DTO de entrada** | — |
| **DTO de salida** | CabinListResponse |

## CAB-02 — GET /cabins/{id}

| **ID** | CAB-02 |
|----|----|
| **Módulo** | /cabins |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/cabins/{id} |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Consultar ficha de una cabina. |
| **Parámetros** | Path: id. |
| **Request** | No aplica. |
| **Response** | {"id_cabina":1,"nombre":"Masaje 01","tipo":"Masaje","capacidad_maxima":2,"descripcion":"...","caracteristicas":"...","equipamiento":"...","beneficios":"...","accesibilidad":"...","imagen_url":"...","estado_operativo":"Disponible","tratamientos_compatibles":\[1,2\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,404 |
| **Caso de uso relacionado** | CU-04/CU-08/CU-16 |
| **Regla de negocio relacionada** | RN-12, RN-13 |
| **DTO de entrada** | — |
| **DTO de salida** | CabinDetailResponse |

## CAB-03 — POST /cabins

| **ID** | CAB-03 |
|----|----|
| **Módulo** | /cabins |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/cabins |
| **Actor autorizado** | AG |
| **Descripción** | Registrar una cabina física. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"id_tipo_cabina":1,"nombre":"Masaje 01","capacidad_maxima":2,"descripcion":"...","caracteristicas":"...","equipamiento":"...","accesibilidad":"...","observaciones":"...","tratamientos_compatibles":\[1,2\]} |
| **Response** | {"id_cabina":1,"nombre":"Masaje 01","capacidad_maxima":2,"estado_operativo":"Disponible"} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,403,409,422 |
| **Caso de uso relacionado** | CU-16 |
| **Regla de negocio relacionada** | RN-12, RN-13 |
| **DTO de entrada** | CreateCabinRequest |
| **DTO de salida** | CabinResponse |

## CAB-04 — PATCH /cabins/{id}

| **ID** | CAB-04 |
|----|----|
| **Módulo** | /cabins |
| **Método HTTP** | PATCH |
| **Ruta** | /api/v1/cabins/{id} |
| **Actor autorizado** | AG |
| **Descripción** | Editar la información de una cabina. |
| **Parámetros** | Path: id. |
| **Request** | {"descripcion":"Actualizada","capacidad_maxima":2,"observaciones":"..."} |
| **Response** | {"id_cabina":1,"descripcion":"Actualizada","capacidad_maxima":2} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,422 |
| **Caso de uso relacionado** | CU-16 |
| **Regla de negocio relacionada** | RN-12, RN-15 |
| **DTO de entrada** | UpdateCabinRequest |
| **DTO de salida** | CabinResponse |

## CAB-05 — PATCH /cabins/{id}/status

| **ID** | CAB-05 |
|----|----|
| **Módulo** | /cabins |
| **Método HTTP** | PATCH |
| **Ruta** | /api/v1/cabins/{id}/status |
| **Actor autorizado** | AG/RC |
| **Descripción** | Cambiar estado operativo de la cabina; Desactivada corresponde principalmente a AG. |
| **Parámetros** | Path: id. |
| **Request** | {"estado_operativo":"En mantenimiento","observacion":"Limpieza profunda"} |
| **Response** | {"id_cabina":1,"estado_operativo":"En mantenimiento","fecha_cambio":"2026-09-08T09:00:00"} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,409 |
| **Caso de uso relacionado** | CU-16/CU-27 |
| **Regla de negocio relacionada** | RN-15, RN-66, RN-67 |
| **DTO de entrada** | ChangeCabinStatusRequest |
| **DTO de salida** | CabinStatusResponse |

## CAB-06 — GET /cabins/{id}/treatments

| **ID** | CAB-06 |
|----|----|
| **Módulo** | /cabins |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/cabins/{id}/treatments |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Consultar tratamientos compatibles con una cabina. |
| **Parámetros** | Path: id. |
| **Request** | No aplica. |
| **Response** | {"cabina":{"id_cabina":1,"nombre":"Masaje 01"},"tratamientos":\[{"id_tratamiento":1,"nombre":"Masaje relajante","es_especializada":true,"prioridad":1}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,404 |
| **Caso de uso relacionado** | CU-08/CU-16 |
| **Regla de negocio relacionada** | RN-10, RN-13 |
| **DTO de entrada** | — |
| **DTO de salida** | CabinTreatmentsResponse |

# /recommendations

## REC-01 — GET /recommendations/cabins

| **ID** | REC-01 |
|----|----|
| **Módulo** | /recommendations |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/recommendations/cabins |
| **Actor autorizado** | CL |
| **Descripción** | Obtener cabinas compatibles y disponibles, destacando la recomendada. |
| **Parámetros** | Query: id_tratamiento, numero_personas, fecha_hora_inicio, opcional fecha_hora_fin. |
| **Request** | No aplica. |
| **Response** | {"id_tratamiento":1,"numero_personas":2,"recomendada":{"id_cabina":1,"score":0.92,"razones":\["Compatible","Capacidad suficiente","Disponible","Especializada"\]},"alternativas":\[{"id_cabina":5,"score":0.81,"razones":\["Compatible","Disponible"\]}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,404,409,422 |
| **Caso de uso relacionado** | CU-07 |
| **Regla de negocio relacionada** | RN-17 a RN-21 |
| **DTO de entrada** | CabinRecommendationQuery |
| **DTO de salida** | CabinRecommendationResponse |

## REC-02 — GET /recommendations/cabins/{id}/explanation

| **ID** | REC-02 |
|----|----|
| **Módulo** | /recommendations |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/recommendations/cabins/{id}/explanation |
| **Actor autorizado** | CL |
| **Descripción** | Consultar la explicación de por qué una cabina es adecuada. |
| **Parámetros** | Path: id; Query: id_tratamiento, numero_personas y opcional fecha_hora_inicio. |
| **Request** | No aplica. |
| **Response** | {"id_cabina":1,"id_tratamiento":1,"es_compatible":true,"capacidad_suficiente":true,"disponible":true,"es_especializada":true,"razones":\["Cabina especializada para el tratamiento"\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,404 |
| **Caso de uso relacionado** | CU-07/CU-08 |
| **Regla de negocio relacionada** | RN-17 a RN-21 |
| **DTO de entrada** | CabinRecommendationExplanationQuery |
| **DTO de salida** | CabinRecommendationExplanationResponse |

# /availability

## AVL-01 — GET /availability

| **ID** | AVL-01 |
|----|----|
| **Módulo** | /availability |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/availability |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Consultar horarios disponibles considerando duración completa y bloqueos. |
| **Parámetros** | Query: id_tratamiento, id_cabina opcional, numero_personas, fecha, hora_inicio opcional. |
| **Request** | No aplica. |
| **Response** | {"fecha":"2026-09-10","id_tratamiento":1,"slots":\[{"inicio":"10:00","fin":"11:00","disponible":true},{"inicio":"11:00","fin":"12:00","disponible":false}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,409,422 |
| **Caso de uso relacionado** | CU-09/CU-25 |
| **Regla de negocio relacionada** | RN-27 a RN-29 |
| **DTO de entrada** | AvailabilityQuery |
| **DTO de salida** | AvailabilityResponse |

## AVL-02 — POST /availability/validate

| **ID** | AVL-02 |
|----|----|
| **Módulo** | /availability |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/availability/validate |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Validar un intervalo concreto antes de seleccionarlo o bloquearlo. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"id_tratamiento":1,"id_cabina":1,"numero_personas":2,"fecha_hora_inicio":"2026-09-10T10:00:00"} |
| **Response** | {"disponible":true,"fecha_hora_inicio":"2026-09-10T10:00:00","fecha_hora_fin":"2026-09-10T11:00:00","motivos_conflicto":\[\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,409,422 |
| **Caso de uso relacionado** | CU-09/CU-25 |
| **Regla de negocio relacionada** | RN-13, RN-14, RN-27, RN-28 |
| **DTO de entrada** | ValidateAvailabilityRequest |
| **DTO de salida** | AvailabilityValidationResponse |

# /cart

## CRT-01 — GET /cart

| **ID** | CRT-01 |
|----|----|
| **Módulo** | /cart |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/cart |
| **Actor autorizado** | CL |
| **Descripción** | Consultar el carrito activo del cliente. |
| **Parámetros** | Header de autenticación. |
| **Request** | No aplica. |
| **Response** | {"id_carrito":50,"estado":"ACTIVO","items":\[{"id_carrito_tratamiento":501,"id_tratamiento":1,"id_cabina":1,"numero_personas":2,"fecha_hora_inicio":"2026-09-10T10:00:00","fecha_hora_fin":"2026-09-10T11:00:00"}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401 |
| **Caso de uso relacionado** | CU-06 |
| **Regla de negocio relacionada** | RN-22 a RN-26 |
| **DTO de entrada** | — |
| **DTO de salida** | CartResponse |

## CRT-02 — POST /cart/items

| **ID** | CRT-02 |
|----|----|
| **Módulo** | /cart |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/cart/items |
| **Actor autorizado** | CL |
| **Descripción** | Agregar una instancia de tratamiento al carrito. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"id_tratamiento":1} |
| **Response** | {"id_carrito_tratamiento":501,"id_carrito":50,"id_tratamiento":1,"numero_personas":null,"id_cabina":null,"fecha_hora_inicio":null,"fecha_hora_fin":null} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,404,409 |
| **Caso de uso relacionado** | CU-05 |
| **Regla de negocio relacionada** | RN-11, RN-22, RN-26 |
| **DTO de entrada** | AddCartItemRequest |
| **DTO de salida** | CartItemResponse |

## CRT-03 — PATCH /cart/items/{id}

| **ID** | CRT-03 |
|----|----|
| **Módulo** | /cart |
| **Método HTTP** | PATCH |
| **Ruta** | /api/v1/cart/items/{id} |
| **Actor autorizado** | CL |
| **Descripción** | Modificar cabina, personas, fecha y hora de una instancia. |
| **Parámetros** | Path: id del elemento. |
| **Request** | {"id_cabina":1,"numero_personas":2,"fecha_hora_inicio":"2026-09-10T10:00:00"} |
| **Response** | {"id_carrito_tratamiento":501,"id_cabina":1,"numero_personas":2,"fecha_hora_inicio":"2026-09-10T10:00:00","fecha_hora_fin":"2026-09-10T11:00:00","bloqueo_temporal_id":700} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,404,409,422 |
| **Caso de uso relacionado** | CU-06/CU-08/CU-09/CU-10 |
| **Regla de negocio relacionada** | RN-23, RN-25, RN-27, RN-30, RN-36 |
| **DTO de entrada** | UpdateCartItemRequest |
| **DTO de salida** | CartItemResponse |

## CRT-04 — DELETE /cart/items/{id}

| **ID** | CRT-04 |
|----|----|
| **Módulo** | /cart |
| **Método HTTP** | DELETE |
| **Ruta** | /api/v1/cart/items/{id} |
| **Actor autorizado** | CL |
| **Descripción** | Eliminar una instancia del carrito y liberar su bloqueo si existe. |
| **Parámetros** | Path: id del elemento. |
| **Request** | No aplica. |
| **Response** | Sin contenido. |
| **Código HTTP exitoso** | 204 No Content |
| **Códigos de error** | 401,404,409 |
| **Caso de uso relacionado** | CU-06 |
| **Regla de negocio relacionada** | RN-25, RN-35, RN-36 |
| **DTO de entrada** | — |
| **DTO de salida** | — |

## CRT-05 — DELETE /cart

| **ID** | CRT-05 |
|----|----|
| **Módulo** | /cart |
| **Método HTTP** | DELETE |
| **Ruta** | /api/v1/cart |
| **Actor autorizado** | CL |
| **Descripción** | Abandonar el carrito activo y liberar sus bloqueos temporales. |
| **Parámetros** | Header de autenticación. |
| **Request** | No aplica. |
| **Response** | {"estado":"ABANDONADO","bloqueos_liberados":2} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,409 |
| **Caso de uso relacionado** | CU-06/CU-10 |
| **Regla de negocio relacionada** | RN-26, RN-35 |
| **DTO de entrada** | — |
| **DTO de salida** | AbandonCartResponse |

# /temporary-blocks

## TMP-01 — POST /temporary-blocks

| **ID** | TMP-01 |
|----|----|
| **Módulo** | /temporary-blocks |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/temporary-blocks |
| **Actor autorizado** | CL |
| **Descripción** | Crear un bloqueo temporal para una selección válida. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"id_carrito_tratamiento":501,"id_cabina":1,"fecha_hora_inicio":"2026-09-10T10:00:00","fecha_hora_fin":"2026-09-10T11:00:00"} |
| **Response** | {"id_bloqueo_temporal":700,"id_cabina":1,"fecha_hora_inicio":"2026-09-10T10:00:00","fecha_hora_fin":"2026-09-10T11:00:00","fecha_creacion":"2026-09-08T09:00:00","fecha_expiracion":"\<calculada: fecha_creacion + duración configurable del bloqueo temporal (parámetro operativo aprobado)\>","segundos_restantes":"\<calculado dinámicamente\>","estado":"ACTIVO"} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,404,409,422 |
| Errores de negocio | AVAILABILITY_CONFLICT. |
| **Caso de uso relacionado** | CU-10 |
| **Regla de negocio relacionada** | RN-30 a RN-33 |
| **DTO de entrada** | CreateTemporaryBlockRequest |
| **DTO de salida** | TemporaryBlockResponse |

## TMP-02 — GET /temporary-blocks/{id}

| **ID** | TMP-02 |
|----|----|
| **Módulo** | /temporary-blocks |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/temporary-blocks/{id} |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Consultar el estado de un bloqueo temporal. |
| **Parámetros** | Path: id. |
| **Request** | No aplica. |
| **Response** | {"id_bloqueo_temporal":700,"estado":"ACTIVO","fecha_creacion":"2026-09-08T09:00:00","fecha_expiracion":"\<calculada: fecha_creacion + duración configurable del bloqueo temporal (parámetro operativo aprobado)\>","segundos_restantes":"\<calculado dinámicamente\>"} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,404 |
| Errores de negocio | BLOCK_EXPIRED. |
| **Caso de uso relacionado** | CU-10 |
| **Regla de negocio relacionada** | RN-31 a RN-34 |
| **DTO de entrada** | — |
| **DTO de salida** | TemporaryBlockResponse |

## TMP-03 — DELETE /temporary-blocks/{id}

| **ID** | TMP-03 |
|----|----|
| **Módulo** | /temporary-blocks |
| **Método HTTP** | DELETE |
| **Ruta** | /api/v1/temporary-blocks/{id} |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Liberar un bloqueo vigente por cambio o abandono. |
| **Parámetros** | Path: id. |
| **Request** | No aplica. |
| **Response** | {"id_bloqueo_temporal":700,"estado":"LIBERADO"} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,404,409 |
| Errores de negocio | BLOCK_EXPIRED. |
| **Caso de uso relacionado** | CU-06/CU-10 |
| **Regla de negocio relacionada** | RN-34 a RN-36 |
| **DTO de entrada** | — |
| **DTO de salida** | TemporaryBlockReleaseResponse |

# /reservations

## RSV-01 — POST /reservations

| **ID** | RSV-01 |
|----|----|
| **Módulo** | /reservations |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/reservations |
| **Actor autorizado** | CL |
| **Descripción** | Crear la reservación desde el carrito: valida que los elementos estén configurados y que los bloqueos temporales estén vigentes, crea la Reservacion en EN_PROCESO, crea cada ReservacionTratamiento en PENDIENTE con su precio histórico (precio_unitario/importe) y calcula el total conforme a la política aprobada. No confirma la reservación ni requiere pago aprobado en esta operación. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"id_carrito":50} |
| **Response** | {"id_reservacion":9001,"estado_reservacion":"EN_PROCESO","tratamientos":\[{"id_reservacion_tratamiento":9101,"estado":"PENDIENTE","precio_unitario":800,"importe":800}\],"total":800,"moneda":"MXN"} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,404,409,422 |
| **Caso de uso relacionado** | CU-11 |
| **Regla de negocio relacionada** | RN-38 a RN-41 |
| **DTO de entrada** | CreateReservationRequest |
| **DTO de salida** | ReservationResponse |

## RSV-02 — GET /reservations

| **ID** | RSV-02 |
|----|----|
| **Módulo** | /reservations |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/reservations |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Listar reservaciones; Cliente solo las propias. |
| **Parámetros** | Query: fecha_desde, fecha_hasta, estado, cliente, page, pageSize según rol. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"id_reservacion":9001,"id_cliente":101,"origen":"WEB","fecha_creacion":"2026-09-08T09:00:00","estado_pago":"PAGADO"}\],"total":1} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403 |
| **Caso de uso relacionado** | CU-12/CU-20 |
| **Regla de negocio relacionada** | RN-04, RN-05 |
| **DTO de entrada** | — |
| **DTO de salida** | ReservationListResponse |

## RSV-03 — GET /reservations/{id}

| **ID** | RSV-03 |
|----|----|
| **Módulo** | /reservations |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/reservations/{id} |
| **Actor autorizado** | CL/AG/RC/PR |
| **Descripción** | Consultar detalle; PR solo si el servicio le pertenece. |
| **Parámetros** | Path: id. |
| **Request** | No aplica. |
| **Response** | {"id_reservacion":9001,"cliente":{"id":101,"nombre":"Ana"},"tratamientos":\[{"id":9101,"tratamiento":"Masaje relajante","cabina":"Masaje 01","fecha_hora_inicio":"2026-09-10T10:00:00","fecha_hora_fin":"2026-09-10T11:00:00","numero_personas":2,"estado":"CONFIRMADO","proveedor":"..."}\],"pagos":\[{"id_pago":3001,"estado_pago":"PAGADO","monto":800}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,404 |
| **Caso de uso relacionado** | CU-12/CU-20/CU-31 |
| **Regla de negocio relacionada** | RN-04, RN-49, RN-73 |
| **DTO de entrada** | — |
| **DTO de salida** | ReservationDetailResponse |

## RSV-04 — POST /reservations/manual

| **ID** | RSV-04 |
|----|----|
| **Módulo** | /reservations |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/reservations/manual |
| **Actor autorizado** | RC |
| **Descripción** | Crear una reservación manual para un cliente. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"id_cliente":101,"tratamientos":\[{"id_tratamiento":1,"id_cabina":1,"numero_personas":2,"fecha_hora_inicio":"2026-09-10T10:00:00"}\],"observaciones":"Llamada telefónica"} |
| **Response** | {"id_reservacion":9002,"estado_reservacion":"EN_PROCESO","origen":"RECEPCION","id_cliente":101,"tratamientos":\[{"id_reservacion_tratamiento":9102,"estado":"PENDIENTE","importe":800}\],"total":800} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,403,404,409,422 |
| **Caso de uso relacionado** | CU-23 |
| **Regla de negocio relacionada** | RN-22, RN-23, RN-27, RN-39 |
| **DTO de entrada** | CreateManualReservationRequest |
| **DTO de salida** | ManualReservationResponse |

## RSV-05 — POST /reservations/{id}/confirm

| **ID** | RSV-05 |
|----|----|
| **Módulo** | /reservations |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/reservations/{id}/confirm |
| **Actor autorizado** | CL/RC |
| **Descripción** | Confirmar definitivamente una reservación EN_PROCESO. Debe comprobar: que la Reservacion exista; que estado_reservacion = EN_PROCESO; que exista el pago requerido en PAGADO; que ese pago pertenezca a la Reservacion; que el importe sea válido; los bloqueos temporales correspondientes; la disponibilidad revalidada; y que no exista una confirmación previa. Si todo es válido: Reservacion pasa de EN_PROCESO a CONFIRMADA, cada ReservacionTratamiento válido pasa de PENDIENTE a CONFIRMADO, y el BloqueoTemporal correspondiente pasa a CONFIRMADO (convertido funcionalmente en ocupación real). Si el pago está PAGADO pero AvailabilityService indica que la disponibilidad ya no existe, no se confirma: se responde 409 Conflict con PAYMENT_APPROVED_AVAILABILITY_LOST, conservando el Pago sin eliminarlo y sin generar automáticamente una devolución mientras DP-EC-02 siga pendiente. |
| **Parámetros** | Path: id. |
| **Request** | {"id_pago":3001} |
| **Response** | {"id_reservacion":9002,"estado_reservacion":"CONFIRMADA","tratamientos_confirmados":1,"estado_pago":"PAGADO"} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,409,422 |
| Errores de negocio | RESERVATION_NOT_IN_PROCESS, PAYMENT_REQUIRED, PAYMENT_NOT_APPROVED, PAYMENT_ALREADY_APPROVED, PAYMENT_APPROVED_AVAILABILITY_LOST, BLOCK_EXPIRED, AVAILABILITY_CONFLICT. |
| **Caso de uso relacionado** | CU-11/CU-23 |
| **Regla de negocio relacionada** | RN-38 a RN-40, RN-75 |
| **DTO de entrada** | ConfirmReservationRequest |
| **DTO de salida** | ReservationConfirmationResponse |

## RSV-06 — POST /reservations/{id}/cancel

| **ID** | RSV-06 |
|----|----|
| **Módulo** | /reservations |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/reservations/{id}/cancel |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Cancelar la reservación completa, liberar recursos y evaluar devolución. |
| **Parámetros** | Path: id. |
| **Request** | {"motivo":"Cambio de planes","observaciones":"..."} |
| **Response** | {"id_reservacion":9001,"cancelada":true,"tratamientos_cancelados":1,"devolucion":{"generada":true,"id_devolucion":5001,"monto":800,"estado_devolucion":"PENDIENTE"}} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,409,422 |
| **Caso de uso relacionado** | CU-14/CU-21/CU-29/CU-43 |
| **Regla de negocio relacionada** | RN-57 a RN-61, RN-83 a RN-87 |
| **DTO de entrada** | CancelReservationRequest |
| **DTO de salida** | CancellationResultResponse |

## RSV-07 — POST /reservations/{id}/treatments/{treatmentId}/cancel

| **ID** | RSV-07 |
|----|----|
| **Módulo** | /reservations |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/reservations/{id}/treatments/{treatmentId}/cancel |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Cancelar solo un tratamiento dentro de una reservación. |
| **Parámetros** | Path: id de reservación y treatmentId. |
| **Request** | {"motivo":"No podré asistir"} |
| **Response** | {"id_reservacion":9001,"id_reservacion_tratamiento":9101,"estado":"CANCELADO","recursos_liberados":true,"devolucion":{"generada":true,"monto":400,"estado_devolucion":"PENDIENTE"}} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,409,422 |
| **Caso de uso relacionado** | CU-13/CU-21/CU-28/CU-43 |
| **Regla de negocio relacionada** | RN-57, RN-60, RN-61, RN-84 |
| **DTO de entrada** | CancelTreatmentRequest |
| **DTO de salida** | TreatmentCancellationResponse |

## Nota: la consulta de pagos de una reservación se documenta una sola vez, como PAY-03 — GET /reservations/{id}/payments, dentro del módulo /payments (ver sección correspondiente). No se define un identificador ni una tabla adicional en este módulo para evitar una definición duplicada del mismo endpoint.

# /providers

## PRV-01 — GET /providers

| **ID** | PRV-01 |
|----|----|
| **Módulo** | /providers |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/providers |
| **Actor autorizado** | AG/RC |
| **Descripción** | Consultar proveedores registrados y su estado. |
| **Parámetros** | Query: activo, id_tratamiento, page, pageSize. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"id_proveedor":20,"id_usuario":110,"nombre":"Proveedor 1","activo":true,"tratamientos":\[1,2\]}\],"total":1} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403 |
| **Caso de uso relacionado** | CU-18/CU-19 |
| **Regla de negocio relacionada** | RN-46, RN-47 |
| **DTO de entrada** | — |
| **DTO de salida** | ProviderListResponse |

## PRV-02 — GET /providers/{id}

| **ID** | PRV-02 |
|----|----|
| **Módulo** | /providers |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/providers/{id} |
| **Actor autorizado** | AG/RC |
| **Descripción** | Consultar información de un proveedor. |
| **Parámetros** | Path: id. |
| **Request** | No aplica. |
| **Response** | {"id_proveedor":20,"id_usuario":110,"nombre":"Proveedor 1","descripcion":"...","activo":true,"tratamientos":\[1,2\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,404 |
| **Caso de uso relacionado** | CU-18 |
| **Regla de negocio relacionada** | RN-46, RN-49 |
| **DTO de entrada** | — |
| **DTO de salida** | ProviderResponse |

## PRV-03 — POST /providers

| **ID** | PRV-03 |
|----|----|
| **Módulo** | /providers |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/providers |
| **Actor autorizado** | AG |
| **Descripción** | Registrar proveedor y sus tratamientos autorizados. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"id_usuario":110,"descripcion":"...","observaciones":"...","tratamientos":\[1,2\]} |
| **Response** | {"id_proveedor":20,"id_usuario":110,"activo":true,"tratamientos":\[1,2\]} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,403,404,409 |
| **Caso de uso relacionado** | CU-18 |
| **Regla de negocio relacionada** | RN-46, RN-47 |
| **DTO de entrada** | CreateProviderRequest |
| **DTO de salida** | ProviderResponse |

## PRV-04 — PATCH /providers/{id}

| **ID** | PRV-04 |
|----|----|
| **Módulo** | /providers |
| **Método HTTP** | PATCH |
| **Ruta** | /api/v1/providers/{id} |
| **Actor autorizado** | AG |
| **Descripción** | Editar o desactivar el perfil operativo del proveedor. |
| **Parámetros** | Path: id. |
| **Request** | {"descripcion":"Actualizada","activo":true,"tratamientos":\[1,3\]} |
| **Response** | {"id_proveedor":20,"activo":true,"tratamientos":\[1,3\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,409 |
| **Caso de uso relacionado** | CU-18 |
| **Regla de negocio relacionada** | RN-46, RN-51 |
| **DTO de entrada** | UpdateProviderRequest |
| **DTO de salida** | ProviderResponse |

## PRV-05 — GET /providers/me/schedule

| **ID** | PRV-05 |
|----|----|
| **Módulo** | /providers |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/providers/me/schedule |
| **Actor autorizado** | PR |
| **Descripción** | Consultar la agenda propia por fecha. |
| **Parámetros** | Query: fecha_desde, fecha_hasta. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"id_reservacion_tratamiento":9101,"tratamiento":"Masaje","fecha_hora_inicio":"2026-09-10T10:00:00","fecha_hora_fin":"2026-09-10T11:00:00","cabina":"Masaje 01","numero_personas":2,"estado":"CONFIRMADO"}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403 |
| **Caso de uso relacionado** | CU-30 |
| **Regla de negocio relacionada** | RN-49 |
| **DTO de entrada** | — |
| **DTO de salida** | ProviderScheduleResponse |

## PRV-06 — GET /providers/me/unavailability

| **ID** | PRV-06 |
|----|----|
| **Módulo** | /providers |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/providers/me/unavailability |
| **Actor autorizado** | PR/AG |
| **Descripción** | Consultar historial de indisponibilidades propias; AG puede consultar para seguimiento. |
| **Parámetros** | Query opcional: fecha_desde, fecha_hasta, activo. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"id_indisponibilidad":600,"fecha_hora_inicio":"2026-09-12T00:00:00","fecha_hora_fin":"2026-09-12T23:59:59","motivo":"Permiso","activo":true}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403 |
| **Caso de uso relacionado** | CU-37 |
| **Regla de negocio relacionada** | RN-50, RN-56 |
| **DTO de entrada** | — |
| **DTO de salida** | ProviderUnavailabilityListResponse |

## PRV-07 — POST /providers/me/unavailability

| **ID** | PRV-07 |
|----|----|
| **Módulo** | /providers |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/providers/me/unavailability |
| **Actor autorizado** | PR |
| **Descripción** | Registrar un periodo de indisponibilidad. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"fecha_hora_inicio":"2026-09-12T00:00:00","fecha_hora_fin":"2026-09-12T23:59:59","motivo":"Permiso","observaciones":"..."} |
| **Response** | {"id_indisponibilidad":600,"fecha_hora_inicio":"2026-09-12T00:00:00","fecha_hora_fin":"2026-09-12T23:59:59","activo":true,"reasignacion_iniciada":true} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,403,409,422 |
| **Caso de uso relacionado** | CU-34/CU-35 |
| **Regla de negocio relacionada** | RN-50 a RN-53 |
| **DTO de entrada** | CreateProviderUnavailabilityRequest |
| **DTO de salida** | ProviderUnavailabilityResponse |

# /provider-assignments

## ASN-01 — GET /provider-assignments/candidates

| **ID** | ASN-01 |
|----|----|
| **Módulo** | /provider-assignments |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/provider-assignments/candidates |
| **Actor autorizado** | AG |
| **Descripción** | Obtener proveedores candidatos para un tratamiento reservado. |
| **Parámetros** | Query: id_reservacion_tratamiento. |
| **Request** | No aplica. |
| **Response** | {"id_reservacion_tratamiento":9101,"candidatos":\[{"id_proveedor":20,"nombre":"Proveedor 1","compatible":true,"disponible":true}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,409 |
| **Caso de uso relacionado** | CU-19 |
| **Regla de negocio relacionada** | RN-46, RN-48, RN-51, RN-53 |
| **DTO de entrada** | AssignmentCandidatesQuery |
| **DTO de salida** | ProviderCandidatesResponse |

## ASN-02 — POST /provider-assignments

| **ID** | ASN-02 |
|----|----|
| **Módulo** | /provider-assignments |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/provider-assignments |
| **Actor autorizado** | AG |
| **Descripción** | Realizar la asignación inicial de proveedor. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"id_reservacion_tratamiento":9101,"id_proveedor":20} |
| **Response** | {"id_asignacion":7001,"id_reservacion_tratamiento":9101,"id_proveedor":20,"estado":"ACTUAL","fecha_asignacion":"2026-09-08T10:00:00"} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,403,404,409,422 |
| Errores de negocio | PROVIDER_NOT_AVAILABLE. |
| **Caso de uso relacionado** | CU-19 |
| **Regla de negocio relacionada** | RN-46 a RN-49 |
| **DTO de entrada** | CreateProviderAssignmentRequest |
| **DTO de salida** | ProviderAssignmentResponse |

## ASN-03 — GET /provider-assignments/{id}

| **ID** | ASN-03 |
|----|----|
| **Módulo** | /provider-assignments |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/provider-assignments/{id} |
| **Actor autorizado** | AG/RC/PR |
| **Descripción** | Consultar una asignación; PR solo cuando le corresponda. |
| **Parámetros** | Path: id. |
| **Request** | No aplica. |
| **Response** | {"id_asignacion":7001,"id_proveedor":20,"estado":"ACTUAL","fecha_asignacion":"2026-09-08T10:00:00","id_asignacion_anterior":null} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,404 |
| **Caso de uso relacionado** | CU-19/CU-30/CU-31 |
| **Regla de negocio relacionada** | RN-47, RN-49, RN-68 |
| **DTO de entrada** | — |
| **DTO de salida** | ProviderAssignmentResponse |

## ASN-04 — POST /provider-assignments/{id}/replacement-decision

| **ID** | ASN-04 |
|----|----|
| **Módulo** | /provider-assignments |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/provider-assignments/{id}/replacement-decision |
| **Actor autorizado** | CL |
| **Descripción** | Aceptar o rechazar la propuesta de proveedor sustituto. |
| **Parámetros** | Path: id de la propuesta/asignación. |
| **Request** | {"decision":"ACEPTAR"} |
| **Response** | {"id_asignacion":7002,"estado":"ACTUAL","decision_cliente":"ACEPTAR","id_asignacion_anterior":7001} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 400,401,403,404,409,422 |
| **Caso de uso relacionado** | CU-35/CU-36 |
| **Regla de negocio relacionada** | RN-52 a RN-56 |
| **DTO de entrada** | ReplacementDecisionRequest |
| **DTO de salida** | ReplacementDecisionResponse |

## ASN-05 — GET /provider-assignments/treatment/{treatmentId}/history

| **ID** | ASN-05 |
|----|----|
| **Módulo** | /provider-assignments |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/provider-assignments/treatment/{treatmentId}/history |
| **Actor autorizado** | AG/PR |
| **Descripción** | Consultar historial de asignaciones y sustituciones de un tratamiento. |
| **Parámetros** | Path: treatmentId. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"id_asignacion":7001,"id_proveedor":20,"estado":"REEMPLAZADA"},{"id_asignacion":7002,"id_proveedor":21,"estado":"ACTUAL","id_asignacion_anterior":7001}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,404 |
| **Caso de uso relacionado** | CU-19/CU-35/CU-37 |
| **Regla de negocio relacionada** | RN-52 a RN-55, RN-68 |
| **DTO de entrada** | — |
| **DTO de salida** | ProviderAssignmentHistoryResponse |

# /payments

## PAY-01 — POST /payments

| **ID** | PAY-01 |
|----|----|
| **Módulo** | /payments |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/payments |
| **Actor autorizado** | CL |
| **Descripción** | Crear un intento de pago asociado a una reservación en proceso (EN_PROCESO) y procesarlo mediante el mecanismo configurado. El monto se obtiene y calcula en el backend a partir de la Reservacion/ReservacionTratamiento vigentes; no se confía en un monto enviado desde Angular. Si el request conserva un campo monto por razones técnicas, debe coincidir con el total calculado por el backend o se rechaza con PAYMENT_AMOUNT_MISMATCH. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"id_reservacion":9001,"metodo_pago":"METODO_CONFIGURADO","id_bloqueos":\[700\]} |
| **Response** | {"id_pago":3001,"id_reservacion":9001,"monto":800,"moneda":"MXN","metodo_pago":"METODO_CONFIGURADO","estado_pago":"PROCESANDO","referencia":null,"fecha_creacion":"2026-09-08T09:05:00"} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,404,409,422 |
| Errores de negocio | PAYMENT_AMOUNT_MISMATCH, RESERVATION_NOT_IN_PROCESS, BLOCK_EXPIRED. |
| **Caso de uso relacionado** | CU-39 |
| **Regla de negocio relacionada** | RN-73 a RN-82 |
| **DTO de entrada** | CreatePaymentRequest |
| **DTO de salida** | PaymentResponse |

## PAY-02 — GET /payments/{id}

| **ID** | PAY-02 |
|----|----|
| **Módulo** | /payments |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/payments/{id} |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Consultar el estado y detalle de un pago, respetando permisos. |
| **Parámetros** | Path: id. |
| **Request** | No aplica. |
| **Response** | {"id_pago":3001,"id_reservacion":9001,"monto":800,"moneda":"MXN","metodo_pago":"METODO_CONFIGURADO","estado_pago":"PAGADO","referencia":"REF-123","fecha_creacion":"2026-09-08T09:05:00","fecha_pago":"2026-09-08T09:06:00","fecha_actualizacion":"2026-09-08T09:06:00"} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,404 |
| **Caso de uso relacionado** | CU-40/CU-41/CU-42 |
| **Regla de negocio relacionada** | RN-76 a RN-79, RN-89, RN-90 |
| **DTO de entrada** | — |
| **DTO de salida** | PaymentDetailResponse |

## PAY-03 — GET /reservations/{id}/payments

| **ID** | PAY-03 |
|----|----|
| **Módulo** | /payments |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/reservations/{id}/payments |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Listar todos los pagos e intentos de una reservación. |
| **Parámetros** | Path: id de reservación. |
| **Request** | No aplica. |
| **Response** | {"id_reservacion":9001,"pagos":\[{"id_pago":3001,"monto":800,"estado_pago":"PAGADO","referencia":"REF-123"}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,404 |
| **Caso de uso relacionado** | CU-40/CU-41/CU-42 |
| **Regla de negocio relacionada** | RN-73, RN-76, RN-88 |
| **DTO de entrada** | — |
| **DTO de salida** | ReservationPaymentListResponse |

## PAY-04 — POST /payments/{id}/retry

| **ID** | PAY-04 |
|----|----|
| **Módulo** | /payments |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/payments/{id}/retry |
| **Actor autorizado** | CL |
| **Descripción** | Crear un nuevo intento para un pago fallido, conservando trazabilidad. |
| **Parámetros** | Path: id del pago fallido. |
| **Request** | {"confirmar_reintento":true} |
| **Response** | {"id_pago_anterior":3001,"id_pago_nuevo":3002,"estado_pago":"PROCESANDO","bloqueo_vigente":true} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,404,409,422 |
| **Caso de uso relacionado** | CU-39 |
| **Regla de negocio relacionada** | RN-80, RN-81, RN-88 |
| **DTO de entrada** | RetryPaymentRequest |
| **DTO de salida** | PaymentRetryResponse |

## PAY-05 — GET /payments

| **ID** | PAY-05 |
|----|----|
| **Módulo** | /payments |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/payments |
| **Actor autorizado** | AG/RC |
| **Descripción** | Consultar y filtrar pagos; RC consulta los de reservaciones que gestiona. |
| **Parámetros** | Query: id_reservacion, cliente, fecha_desde, fecha_hasta, estado_pago, referencia, page, pageSize. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"id_pago":3001,"id_reservacion":9001,"monto":800,"estado_pago":"PAGADO","referencia":"REF-123"}\],"total":1} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403 |
| **Caso de uso relacionado** | CU-41/CU-42 |
| **Regla de negocio relacionada** | RN-89, RN-90 |
| **DTO de entrada** | — |
| **DTO de salida** | PaymentListResponse |

## PAY-06 — POST /payments/manual

| **ID** | PAY-06 |
|----|----|
| **Módulo** | /payments |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/payments/manual |
| **Actor autorizado** | RC |
| **Descripción** | Registrar o validar un pago de una reservación manual. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"id_reservacion":9002,"monto":800,"moneda":"MXN","metodo_pago":"EFECTIVO","estado_pago":"PAGADO","referencia":"REC-1002","observacion":"Pago en recepción"} |
| **Response** | {"id_pago":3003,"id_reservacion":9002,"monto":800,"estado_pago":"PAGADO","metodo_pago":"EFECTIVO","referencia":"REC-1002","id_usuario_responsable":115} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,403,404,409,422 |
| **Caso de uso relacionado** | CU-42 |
| **Regla de negocio relacionada** | RN-73, RN-78, RN-79, RN-82 |
| **DTO de entrada** | RegisterManualPaymentRequest |
| **DTO de salida** | PaymentResponse |

# /refunds

## REF-01 — POST /refunds

| **ID** | REF-01 |
|----|----|
| **Módulo** | /refunds |
| **Método HTTP** | POST |
| **Ruta** | /api/v1/refunds |
| **Actor autorizado** | AG/RC/SYS |
| **Descripción** | Crear/procesar una devolución total o parcial derivada de una cancelación. |
| **Parámetros** | Sin parámetros de ruta. |
| **Request** | {"id_pago":3001,"id_reservacion":9001,"id_reservacion_tratamiento":9101,"monto":400,"motivo":"Cancelación de tratamiento","tipo_devolucion":"PARCIAL"} |
| **Response** | {"id_devolucion":5001,"id_pago":3001,"id_reservacion":9001,"id_reservacion_tratamiento":9101,"tipo_devolucion":"PARCIAL","monto":400,"estado_devolucion":"PENDIENTE","fecha_solicitud":"2026-09-08T10:00:00"} |
| **Código HTTP exitoso** | 201 Created |
| **Códigos de error** | 400,401,403,404,409,422 |
| Errores de negocio | PAYMENT_NOT_FOUND, REFUND_EXCEEDS_PAYMENT, REFUND_ALREADY_COMPLETED. |
| **Caso de uso relacionado** | CU-43 |
| **Regla de negocio relacionada** | RN-83 a RN-87 |
| **DTO de entrada** | CreateRefundRequest |
| **DTO de salida** | RefundResponse |

## REF-02 — GET /refunds/{id}

| **ID** | REF-02 |
|----|----|
| **Módulo** | /refunds |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/refunds/{id} |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Consultar una devolución y su estado. |
| **Parámetros** | Path: id. |
| **Request** | No aplica. |
| **Response** | {"id_devolucion":5001,"id_pago":3001,"id_reservacion":9001,"monto":400,"motivo":"Cancelación de tratamiento","estado_devolucion":"COMPLETADA","fecha_solicitud":"2026-09-08T10:00:00","fecha_procesamiento":"2026-09-08T10:03:00"} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,404 |
| **Caso de uso relacionado** | CU-40/CU-41/CU-43 |
| **Regla de negocio relacionada** | RN-86, RN-87, RN-89, RN-90 |
| **DTO de entrada** | — |
| **DTO de salida** | RefundDetailResponse |

## REF-03 — GET /payments/{id}/refunds

| **ID** | REF-03 |
|----|----|
| **Módulo** | /refunds |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/payments/{id}/refunds |
| **Actor autorizado** | CL/AG/RC |
| **Descripción** | Consultar devoluciones asociadas a un pago. |
| **Parámetros** | Path: id del pago. |
| **Request** | No aplica. |
| **Response** | {"id_pago":3001,"monto_pagado":800,"monto_reembolsado":400,"devoluciones":\[{"id_devolucion":5001,"monto":400,"estado_devolucion":"COMPLETADA","tipo_devolucion":"PARCIAL"}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,404 |
| **Caso de uso relacionado** | CU-40/CU-41/CU-43 |
| **Regla de negocio relacionada** | RN-84 a RN-87 |
| **DTO de entrada** | — |
| **DTO de salida** | PaymentRefundListResponse |

## REF-04 — GET /refunds

| **ID** | REF-04 |
|----|----|
| **Módulo** | /refunds |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/refunds |
| **Actor autorizado** | AG/RC |
| **Descripción** | Consultar devoluciones para seguimiento administrativo/operativo. |
| **Parámetros** | Query: id_pago, id_reservacion, estado_devolucion, fecha_desde, fecha_hasta, page, pageSize. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"id_devolucion":5001,"id_pago":3001,"monto":400,"estado_devolucion":"COMPLETADA"}\],"total":1} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403 |
| **Caso de uso relacionado** | CU-41/CU-43 |
| **Regla de negocio relacionada** | RN-86 a RN-90 |
| **DTO de entrada** | — |
| **DTO de salida** | RefundListResponse |

# /reports

## RPT-01 — GET /reports/reservations-by-day

| **ID** | RPT-01 |
|----|----|
| **Módulo** | /reports |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/reports/reservations-by-day |
| **Actor autorizado** | AG |
| **Descripción** | Reporte de reservaciones por día. |
| **Parámetros** | Query: fecha_desde, fecha_hasta. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"fecha":"2026-09-08","reservaciones":12}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,422 |
| **Caso de uso relacionado** | CU-22 |
| **Regla de negocio relacionada** | RN-71, RN-72 |
| **DTO de entrada** | ReportDateRangeQuery |
| **DTO de salida** | ReservationsByDayReportResponse |

## RPT-02 — GET /reports/top-treatments

| **ID** | RPT-02 |
|----|----|
| **Módulo** | /reports |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/reports/top-treatments |
| **Actor autorizado** | AG |
| **Descripción** | Reporte de tratamientos más solicitados. |
| **Parámetros** | Query: fecha_desde, fecha_hasta, limit opcional. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"id_tratamiento":1,"nombre":"Masaje relajante","cantidad":35}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,422 |
| **Caso de uso relacionado** | CU-22 |
| **Regla de negocio relacionada** | RN-71, RN-72 |
| **DTO de entrada** | TopTreatmentsQuery |
| **DTO de salida** | TopTreatmentsReportResponse |

## RPT-03 — GET /reports/top-cabins

| **ID** | RPT-03 |
|----|----|
| **Módulo** | /reports |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/reports/top-cabins |
| **Actor autorizado** | AG |
| **Descripción** | Reporte de cabinas más utilizadas. |
| **Parámetros** | Query: fecha_desde, fecha_hasta, limit opcional. |
| **Request** | No aplica. |
| **Response** | {"items":\[{"id_cabina":1,"nombre":"Masaje 01","servicios":42,"ocupacion_porcentual":68.4}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,422 |
| **Caso de uso relacionado** | CU-22 |
| **Regla de negocio relacionada** | RN-71, RN-72 |
| **DTO de entrada** | TopCabinsQuery |
| **DTO de salida** | TopCabinsReportResponse |

## RPT-04 — GET /reports/cancellations

| **ID** | RPT-04 |
|----|----|
| **Módulo** | /reports |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/reports/cancellations |
| **Actor autorizado** | AG |
| **Descripción** | Reporte de cancelaciones. |
| **Parámetros** | Query: fecha_desde, fecha_hasta, tipo opcional. |
| **Request** | No aplica. |
| **Response** | {"total":8,"por_tipo":{"TRATAMIENTO":5,"RESERVACION_COMPLETA":3}} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,422 |
| **Caso de uso relacionado** | CU-22 |
| **Regla de negocio relacionada** | RN-57, RN-71, RN-72 |
| **DTO de entrada** | CancellationReportQuery |
| **DTO de salida** | CancellationReportResponse |

## RPT-05 — GET /reports/cabin-occupancy

| **ID** | RPT-05 |
|----|----|
| **Módulo** | /reports |
| **Método HTTP** | GET |
| **Ruta** | /api/v1/reports/cabin-occupancy |
| **Actor autorizado** | AG |
| **Descripción** | Reporte de ocupación general de cabinas. |
| **Parámetros** | Query: fecha_desde, fecha_hasta, id_cabina opcional. |
| **Request** | No aplica. |
| **Response** | {"periodo":{"desde":"2026-09-01","hasta":"2026-09-07"},"items":\[{"id_cabina":1,"horas_ocupadas":34,"horas_disponibles":50,"ocupacion_porcentual":68}\]} |
| **Código HTTP exitoso** | 200 OK |
| **Códigos de error** | 401,403,422 |
| **Caso de uso relacionado** | CU-22 |
| **Regla de negocio relacionada** | RN-71, RN-72 |
| **DTO de entrada** | CabinOccupancyQuery |
| **DTO de salida** | CabinOccupancyReportResponse |

# 6. Convención de DTO

Los DTO separan el contrato HTTP de las entidades de persistencia. No se recomienda devolver directamente las entidades de SQL Server. Los campos se nombran siguiendo el diccionario de datos, mientras que el DTO puede usar la convención elegida por el equipo (.NET puede mapear posteriormente a PascalCase).

| **DTO de entrada/salida** | **Estructura y propósito** |
|----|----|
| **RegisterRequest** | nombre: TEXT; correo: TEXT; telefono: TEXT; password: TEXT — Entrada de registro. La contraseña solo se procesa para generar un hash. |
| **LoginRequest** | correo: TEXT; password: TEXT — Credenciales de acceso. |
| **LoginResponse** | accessToken: TEXT; usuario: UserResponse — Resultado de autenticación. |
| **UserResponse** | id_usuario: INT; nombre: TEXT; correo: TEXT; telefono: TEXT; rol: TEXT; activo: BIT — Representación pública del usuario. |
| **ChangeUserRoleRequest** | id_rol: INT — Cambio de rol por AG. |
| **ChangeUserStatusRequest** | activo: BIT — Activación/desactivación. |
| **ClientPreferencesResponse** | objetivo_visita: TEXT; modalidad_preferida: TEXT; privacidad_preferida: TEXT; ambiente_preferido: TEXT; requiere_accesibilidad: BIT — Preferencias opcionales. |
| **CreateTreatmentRequest** | nombre: TEXT; descripcion: TEXT; duracion_minutos: INT; beneficios: TEXT; recomendaciones_generales: TEXT; restricciones: TEXT; requiere_proveedor: BIT; precio_base: DECIMAL(10,2); moneda: TEXT; cabinas_compatibles: INT\[\] — Alta de tratamiento. |
| **UpdateTreatmentRequest** | Campos parciales del tratamiento que se desean modificar. — Actualización sin alterar precios históricos de reservaciones. |
| **CreateCabinRequest** | id_tipo_cabina: INT; nombre: TEXT; capacidad_maxima: INT; descripcion: TEXT; caracteristicas: TEXT; equipamiento: TEXT; accesibilidad: TEXT; observaciones: TEXT; tratamientos_compatibles: INT\[\] — Alta de cabina. |
| **ChangeCabinStatusRequest** | estado_operativo: TEXT; observacion: TEXT — Cambio operativo con trazabilidad. |
| **CabinRecommendationQuery** | id_tratamiento: INT; numero_personas: INT; fecha_hora_inicio: DATETIME2 opcional; fecha_hora_fin: DATETIME2 opcional — Criterios para recomendación. |
| **AvailabilityQuery** | id_tratamiento: INT; id_cabina: INT opcional; numero_personas: INT; fecha: DATE; hora_inicio: TIME opcional — Consulta de disponibilidad. |
| **AddCartItemRequest** | id_tratamiento: INT — Agrega una instancia independiente. |
| **UpdateCartItemRequest** | id_cabina: INT; numero_personas: INT; fecha_hora_inicio: DATETIME2 — Configuración provisional; la hora final se calcula. |
| **CreateTemporaryBlockRequest** | id_carrito_tratamiento: BIGINT; id_cabina: INT; fecha_hora_inicio: DATETIME2; fecha_hora_fin: DATETIME2 — Bloqueo temporal con duración configurable conforme al parámetro operativo aprobado. |
| **CreateReservationRequest** | id_carrito: BIGINT — Crea la reservación en EN_PROCESO a partir del carrito; no confirma ni requiere pago en esta operación. |
| **CreateManualReservationRequest** | id_cliente: INT; tratamientos\[\]; observaciones: TEXT — Reservación creada por Recepción. |
| **ConfirmReservationRequest** | id_pago: BIGINT opcional — Confirmación después de validar que el pago requerido esté PAGADO, que pertenezca a la Reservacion, que el importe sea válido, los bloqueos correspondientes y la disponibilidad revalidada. |
| **CancelTreatmentRequest** | motivo: TEXT; observaciones: TEXT — Cancelación individual. |
| **CancelReservationRequest** | motivo: TEXT; observaciones: TEXT — Cancelación completa. |
| **CreateProviderRequest** | id_usuario: INT; descripcion: TEXT; observaciones: TEXT; tratamientos: INT\[\] — Alta/extensión de proveedor. |
| **UpdateProviderRequest** | descripcion: TEXT; observaciones: TEXT; activo: BIT; tratamientos: INT\[\] — Actualización de proveedor. |
| **CreateProviderUnavailabilityRequest** | fecha_hora_inicio: DATETIME2; fecha_hora_fin: DATETIME2; motivo: TEXT; observaciones: TEXT — Indisponibilidad del proveedor. |
| **CreateProviderAssignmentRequest** | id_reservacion_tratamiento: BIGINT; id_proveedor: INT — Asignación inicial por AG. |
| **ReplacementDecisionRequest** | decision: ACEPTAR\|CANCELAR_TRATAMIENTO\|CANCELAR_RESERVACION — Decisión del cliente sobre sustitución. |
| **CreatePaymentRequest** | id_reservacion: BIGINT; metodo_pago: TEXT; id_bloqueos: BIGINT\[\] — Inicio de intento de pago. El monto se calcula en el backend a partir de la Reservacion y no se recibe como dato de confianza del cliente; si se envía por razones técnicas se valida contra el total del backend (PAYMENT_AMOUNT_MISMATCH). No contiene número completo de tarjeta ni CVV. |
| **RetryPaymentRequest** | confirmar_reintento: BIT — Solicita un nuevo intento sin sobrescribir el anterior. |
| **RegisterManualPaymentRequest** | id_reservacion: BIGINT; monto: DECIMAL(10,2); moneda: TEXT; metodo_pago: TEXT; estado_pago: TEXT; referencia: TEXT; observacion: TEXT — Registro/validación de pago manual. |
| **CreateRefundRequest** | id_pago: BIGINT; id_reservacion: BIGINT; id_reservacion_tratamiento: BIGINT opcional; monto: DECIMAL(10,2); motivo: TEXT; tipo_devolucion: PARCIAL\|TOTAL — Devolución conforme a cancelación y política aplicable. |

# 7. DTO de salida y reglas de exposición

## PaymentDetailResponse

id_pago, id_reservacion, monto, moneda, metodo_pago, estado_pago, referencia, fecha_creacion, fecha_pago, fecha_actualizacion. No debe contener datos bancarios sensibles.

## RefundResponse

id_devolucion, id_pago, id_reservacion, id_reservacion_tratamiento opcional, monto, motivo, estado_devolucion, fecha_solicitud, fecha_procesamiento, id_usuario_responsable opcional.

## ReservationDetailResponse

id_reservacion, cliente, origen, fecha_creacion, observaciones, tratamientos\[\] y pagos\[\]. Cada tratamiento incluye cabina, horario, personas, estado, importe y proveedor vigente cuando exista.

## AvailabilityResponse

fecha, tratamiento solicitado, slots con inicio, fin y disponibilidad. Cuando no esté disponible puede incluir motivos de conflicto.

## CabinRecommendationResponse

Cabina recomendada y alternativas. Cada opción puede incluir razones de compatibilidad, capacidad, disponibilidad y especialización.

# 8. Reglas de seguridad y consistencia

- El backend debe aplicar autorización por rol aunque el frontend oculte botones.

- El Cliente no puede consultar pagos, devoluciones o reservaciones de otra cuenta.

- El Proveedor solo puede consultar y operar tratamientos asignados a su cuenta.

- La disponibilidad debe validarse nuevamente al momento de confirmar; una consulta previa no reserva el recurso.

- Los bloqueos temporales deben expirar/liberarse automáticamente y no deben convertirse en reservación por sí mismos.

- Si Pago = FALLIDO, el registro se conserva, la reservación no se confirma y sus ReservacionTratamiento no cambian a CONFIRMADO; se permite POST /payments/{id}/retry cuando proceda. Los reintentos generan trazabilidad nueva y no sobrescriben intentos anteriores.

- La suma de devoluciones completadas de un pago no puede superar el importe efectivamente pagado.

- Las cancelaciones liberan recursos pero conservan el registro histórico.

- No se almacenarán números completos de tarjeta, CVV, contraseñas bancarias ni otra información financiera sensible.

# 9. Flujo REST de una reservación con pago

1.  1\. GET /treatments → el Cliente consulta tratamientos activos (Carrito).

2.  2\. POST /cart/items → agrega uno o varios tratamientos (Carrito).

3.  3\. GET /recommendations/cabins → obtiene recomendación y alternativas (Carrito).

4.  4\. PATCH /cart/items/{id} → selecciona personas, cabina y fecha/hora (Carrito).

5.  5\. GET/POST /availability → se consulta/valida el intervalo completo (Disponibilidad).

6.  6\. POST /temporary-blocks → TZISCA protege la cabina durante la duración configurable del bloqueo temporal definida por el parámetro operativo aprobado (Bloqueo temporal).

7.  7\. El Cliente revisa el resumen del carrito: tratamientos, cabina, horario y total (Resumen).

8.  8\. POST /reservations → crea la Reservacion en EN_PROCESO y cada ReservacionTratamiento en PENDIENTE; no confirma ni requiere pago en esta operación.

9.  9\. POST /payments → se registra el intento de pago asociado a la Reservacion EN_PROCESO (Pago); el monto se calcula en el backend, no se confía en el enviado por el cliente.

10. 10\. POST /payments/{id}/retry → solo si el intento falla y el bloqueo sigue vigente.

11. 11\. POST /reservations/{id}/confirm → TZISCA revalida el pago (PAGADO) y la disponibilidad (Revalidación); si es válido, Reservacion pasa de EN_PROCESO a CONFIRMADA y cada ReservacionTratamiento válido pasa de PENDIENTE a CONFIRMADO.

12. 12\. POST /reservations/{id}/cancel o /reservations/{id}/treatments/{treatmentId}/cancel → si hay cancelación, se evalúa devolución.

13. 13\. POST /refunds → se registra/procesa la devolución total o parcial.

# 10. Flujo de reservación manual

- POST /reservations/manual → Recepción crea la reservación; Reservacion queda en EN_PROCESO y cada ReservacionTratamiento en PENDIENTE.

- POST /payments/manual → Recepción registra o valida el pago (PAGADO).

- POST /reservations/{id}/confirm → se confirma cuando la condición económica (pago PAGADO) y los recursos (disponibilidad revalidada) sean válidos: Reservacion pasa a CONFIRMADA y cada ReservacionTratamiento válido pasa a CONFIRMADO.

- La asignación inicial del proveedor no corresponde a Recepción; posteriormente se utiliza /provider-assignments.

# 11. Matriz de cobertura por caso de uso

| **Caso de uso** | **Endpoint(s) principal(es)** | **Cobertura** |
|----|----|----|
| **CU-01** | /auth/register | Registro |
| **CU-02** | /auth/login, /auth/logout, /auth/me | Autenticación |
| **CU-03/CU-04** | /treatments | Catálogo y detalle |
| **CU-05/CU-06** | /cart | Carrito |
| **CU-07/CU-08** | /recommendations, /cabins | Recomendación y selección |
| **CU-09** | /availability | Disponibilidad |
| **CU-10** | /temporary-blocks | Bloqueo temporal |
| CU-11 | POST /reservations, POST /reservations/{id}/confirm | Creación en proceso (EN_PROCESO) y confirmación (CONFIRMADA) |
| CU-12 | GET /reservations, GET /reservations/{id} | Consulta y supervisión de reservaciones |
| **CU-13/CU-14** | /reservations/{id}/.../cancel | Cancelaciones |
| **CU-15** | /treatments | Gestión de tratamientos |
| **CU-16** | /cabins | Gestión de cabinas |
| **CU-17** | /users | Usuarios y roles |
| **CU-18** | /providers | Gestión de proveedores |
| **CU-19** | /provider-assignments | Asignación inicial |
| **CU-20/CU-21** | /reservations | Supervisión y cancelación administrativa |
| **CU-22** | /reports | Reportes |
| **CU-23/CU-24/CU-25** | /reservations/manual, /availability, /reservations | Operación de Recepción |
| **CU-26/CU-27** | /temporary-blocks, /cabins/{id}/status | Bloqueos y estado de cabina |
| **CU-28/CU-29** | /reservations/{id}/.../cancel | Cancelaciones operativas |
| **CU-30/CU-31/CU-32/CU-33** | /providers/me/schedule, /reservations, acciones de estado | Agenda y atención |
| **CU-34/CU-35/CU-36/CU-37/CU-38** | /providers/me/unavailability, /provider-assignments | Indisponibilidad, sustitución e historial |
| **CU-39** | POST /payments, POST /payments/{id}/retry | Registro de pago y reintento |
| **CU-40** | GET /payments/{id}, GET /reservations/{id}/payments | Consulta económica del pago |
| **CU-41** | /payments, /refunds | Gestión administrativa |
| **CU-42** | /payments/manual | Pago manual |
| **CU-43** | POST /refunds, GET /refunds/{id}, GET /payments/{id}/refunds | Registro y consulta de devolución |

# 12. Decisiones pendientes antes de implementación física

El uso del estado PENDIENTE en ReservacionTratamiento ya no es una decisión pendiente: el Diccionario de Datos TZISCA (versión corregida y cerrada) resuelve su ciclo formal (EN_PROCESO/PENDIENTE → CONFIRMADA/CONFIRMADO) y esta API lo implementa mediante POST /reservations y POST /reservations/{id}/confirm. Permanecen pendientes de aprobación, sin que esta API invente sus valores: DP-EC-01 (fórmula del importe), DP-EC-02 (pago aprobado con disponibilidad perdida, ver PAYMENT_APPROVED_AVAILABILITY_LOST), DP-OP-01 a DP-OP-13 (horario de apertura y cierre, días laborables/no laborables, excepciones, intervalos de agenda, anticipación mínima y máxima, duración del bloqueo temporal, y tolerancias/políticas de cancelación y devolución) y DP-TEC-01 (mecanismo de autenticación). Esta API no fija ninguno de esos valores ni fuerza una transición adicional de estado; deberán validarse antes del esquema físico definitivo.

Asimismo, la pasarela de pago permanece abstracta (DP-TEC-02, pendiente de aprobación). /payments representa el contrato interno de TZISCA mediante un PaymentService abstracto; la implementación futura podrá conectar un mecanismo externo, pero no se fija Stripe, Mercado Pago, PayPal ni otro proveedor en este documento hasta que DP-TEC-02 se cierre.

# 13. Fuentes documentales utilizadas

- Propuesta de Proyecto de Prácticas Profesionales — Sistema Web de Reservas, Recomendación y Gestión de Cabinas para Spa.

- TZISCA — Catálogo de cabinas y servicios.

- TZISCA — Casos de Uso CU-01 a CU-43.

- TZISCA — Reglas de Negocio actualizadas RN-01 a RN-90.

- TZISCA — Criterios de Aceptación CU-01 a CU-43.

- TZISCA — Diccionario de Datos TZISCA Pagos.

# 14. Cierre del documento

Estado del documento: CERRADO. Esta versión corregida cierra el Diseño de API REST TZISCA v1 conforme a las 17 instrucciones de corrección solicitadas el 08/09/2026, trabajando sobre la versión vigente del documento: no se creó una API nueva desde cero, y se mantuvieron /api/v1, REST, JSON, Angular, ASP.NET Core, SQL Server, CU-01 a CU-43 y RN-01 a RN-90.

Las decisiones que siguen pendientes de aprobación (DP-EC-01, DP-EC-02, DP-OP-01 a DP-OP-13, DP-TEC-01, DP-TEC-02, DP-TEC-03) permanecen documentadas como tales: esta API no fija valores operativos, económicos, de autenticación ni de pasarela de pago que aún no hayan sido aprobados.

Con este cierre, el documento queda listo para utilizarse como contrato de implementación de ASP.NET Core y Angular.
