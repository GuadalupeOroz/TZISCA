# TZISCA

**DISEÑO DE API REST**

Sistema Web de Citas, Recomendación y Gestión de Cabinas para Spa

| Documento | Diseño de API REST — TZISCA |
| --- | --- |
| Versión | 2.1 — Modelo vigente de 22 entidades |
| Fecha | 18 de septiembre de 2026 |
| Arquitectura base | Frontend Angular → API REST .NET Core → SQL Server |
| Formato de intercambio | JSON |
| Prefijo propuesto | /api/v1 |
| Autenticación | ASP.NET Core Identity, JWT como access token y refresh token seguro. |

Versión 2.1 — 18/09/2026. Contrato alineado con 22 entidades, CU-01 a CU-43 y RN-01 a RN-117.

Base URL: /api/v1

## 1. Propósito y alcance

La API REST expone JSON para Angular y se implementa en ASP.NET Core sobre SQL Server. Cita es el recurso principal de agenda; Cliente es distinto de Usuario; Stripe se integra detrás de PaymentService.

## 2. Arquitectura y seguridad

- Angular consume exclusivamente /api/v1.

- ASP.NET Core Identity administra credenciales; la API usa JWT como access token y refresh token seguro con rotación y revocación.

- La autorización se aplica por rol o permiso y por propiedad del recurso.

- PaymentService encapsula Stripe y evita acoplar los controladores a la pasarela.

- No se exponen password hashes, refresh tokens en texto plano, números completos de tarjeta ni CVV.

## 3. Convenciones HTTP

La API usa GET para consulta, POST para creación o acciones, PUT para reemplazo de relaciones y PATCH para cambios parciales. Los errores usan {code, message, details}; las fechas se expresan en ISO 8601.

| Código | Uso |
| --- | --- |
| 200 | Consulta o acción exitosa |
| 201 | Creación exitosa |
| 204 | Acción sin cuerpo |
| 400 | Solicitud inválida |
| 401 | No autenticado |
| 403 | No autorizado |
| 404 | Recurso inexistente |
| 409 | Conflicto de disponibilidad o estado |
| 422 | Regla de negocio incumplida |

## 4. Recursos y modelo

Los DTO se basan en las 22 entidades vigentes. CarritoItem es un recurso persistente del carrito y conserva el bloqueo temporal de 15 minutos; BloqueoCabina representa un bloqueo operativo programado.

## 5. Catálogo de endpoints

### /auth

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| AUTH-01 | POST | /api/v1/auth/register | Público | Crea Usuario y Cliente mediante Identity. |
| AUTH-02 | POST | /api/v1/auth/login | Público | Devuelve JWT de acceso y establece refresh token seguro. |
| AUTH-03 | POST | /api/v1/auth/refresh | Público con refresh token | Rota el refresh token y emite un nuevo JWT. |
| AUTH-04 | POST | /api/v1/auth/logout | Autenticado | Revoca el refresh token vigente. |
| AUTH-05 | GET | /api/v1/auth/me | Autenticado | Devuelve identidad, Rol y permisos efectivos. |

### /users

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| USR-01 | GET | /api/v1/users | Administrador | Lista usuarios. |
| USR-02 | GET | /api/v1/users/{id} | Administrador | Consulta un Usuario. |
| USR-03 | PATCH | /api/v1/users/{id}/role | Administrador | Cambia el Rol. |
| USR-04 | PATCH | /api/v1/users/{id}/status | Administrador | Cambia Usuario.activo. |

### /clients

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| CLI-01 | GET | /api/v1/clients/me | Cliente | Consulta el perfil Cliente separado de Usuario. |
| CLI-02 | GET | /api/v1/clients/me/preferences | Cliente | Consulta PreferenciaCliente. |
| CLI-03 | PUT | /api/v1/clients/me/preferences | Cliente | Crea o actualiza PreferenciaCliente. |

### /treatments

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| TRT-01 | GET | /api/v1/treatments | Público | Lista tratamientos activos. |
| TRT-02 | GET | /api/v1/treatments/{id} | Público | Consulta detalle y precio por persona. |
| TRT-03 | POST | /api/v1/treatments | Administrador | Crea Tratamiento. |
| TRT-04 | PATCH | /api/v1/treatments/{id} | Administrador | Actualiza Tratamiento. |
| TRT-05 | GET | /api/v1/treatments/{id}/providers | Administrador o recepción | Consulta TratamientoProveedor. |
| TRT-06 | GET | /api/v1/treatments/{id}/cabins | Público | Consulta cabinas compatibles mediante TratamientoCabina. |
| TRT-07 | PUT | /api/v1/treatments/{id}/cabins | Administrador | Sustituye compatibilidades TratamientoCabina. |

### /packages

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| PKG-01 | GET | /api/v1/packages | Público | Lista Paquetes activos. |
| PKG-02 | GET | /api/v1/packages/{id} | Público | Consulta Paquete y sus tratamientos. |
| PKG-03 | POST | /api/v1/packages | Administrador | Crea Paquete. |
| PKG-04 | PATCH | /api/v1/packages/{id} | Administrador | Actualiza Paquete. |
| PKG-05 | PUT | /api/v1/packages/{id}/treatments | Administrador | Sustituye las relaciones PaqueteTratamiento. |

### /cabins

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| CAB-01 | GET | /api/v1/cabins | Público | Lista cabinas; expone activo y estado por separado. |
| CAB-02 | GET | /api/v1/cabins/{id} | Público | Consulta una Cabina. |
| CAB-03 | POST | /api/v1/cabins | Administrador | Crea Cabina. |
| CAB-04 | PATCH | /api/v1/cabins/{id} | Administrador | Actualiza datos y activo. |
| CAB-05 | PATCH | /api/v1/cabins/{id}/status | Administrador o recepción | Cambia estado operativo. |
| CAB-06 | GET | /api/v1/cabins/{id}/status-history | Administrador o recepción | Consulta EstadoCabina. |
| CAB-07 | GET | /api/v1/cabins/{id}/blocks | Administrador o recepción | Lista BloqueoCabina, activo e histórico. |
| CAB-08 | POST | /api/v1/cabins/{id}/blocks | Administrador o recepción | Crea un bloqueo operativo con motivo e intervalo. |
| CAB-09 | PATCH | /api/v1/cabins/{id}/blocks/{blockId} | Administrador o recepción | Actualiza o desactiva un BloqueoCabina. |

### /recommendations

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| REC-01 | GET | /api/v1/recommendations/cabins | Cliente o recepción | Orden determinista de cabinas elegibles. |
| REC-02 | GET | /api/v1/recommendations/cabins/{id}/explanation | Cliente o recepción | Explica criterios sin alterar el orden. |

### /availability

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| AVL-01 | GET | /api/v1/availability | Cliente o personal | Genera horarios cada 30 minutos. |
| AVL-02 | POST | /api/v1/availability/validate | Cliente o personal | Valida horario, anticipación, Cabina y Proveedor. |

### /cart

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| CRT-01 | GET | /api/v1/cart | Cliente | Consulta el Carrito activo y sus CarritoItem. |
| CRT-02 | POST | /api/v1/cart/items | Cliente | Crea un CarritoItem temporal. |
| CRT-03 | PATCH | /api/v1/cart/items/{id} | Cliente | Modifica un CarritoItem y renueva su bloqueo cuando procede. |
| CRT-04 | DELETE | /api/v1/cart/items/{id} | Cliente | Elimina un CarritoItem y libera su bloqueo. |
| CRT-05 | DELETE | /api/v1/cart | Cliente | Abandona el Carrito y libera bloqueos. |

### /appointments

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| APT-01 | POST | /api/v1/appointments | Cliente | Genera Citas PENDIENTES desde el Carrito. |
| APT-02 | GET | /api/v1/appointments | Autenticado | Lista Citas según rol y propietario. |
| APT-03 | GET | /api/v1/appointments/{id} | Autenticado | Consulta una Cita. |
| APT-04 | POST | /api/v1/appointments/manual | Recepción | Crea una Cita manual. |
| APT-05 | POST | /api/v1/appointments/{id}/confirm | Cliente o recepción | Revalida y confirma la Cita. |
| APT-06 | POST | /api/v1/appointments/{id}/cancel | Cliente o personal | Crea Cancelacion y calcula Devolucion. |
| APT-07 | PUT | /api/v1/appointments/{id}/cabins | Cliente o personal | Actualiza CitaCabina antes de confirmar. |
| APT-08 | PUT | /api/v1/appointments/{id}/provider | Administrador | Asigna o sustituye Proveedor en Cita. |

### /providers

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| PRV-01 | GET | /api/v1/providers | Administrador o recepción | Lista proveedores. |
| PRV-02 | GET | /api/v1/providers/{id} | Administrador o recepción | Consulta Proveedor. |
| PRV-03 | POST | /api/v1/providers | Administrador | Crea Proveedor vinculado con Usuario. |
| PRV-04 | PATCH | /api/v1/providers/{id} | Administrador | Actualiza Proveedor. |
| PRV-05 | GET | /api/v1/providers/me/schedule | Proveedor | Consulta Citas asignadas. |
| PRV-06 | GET | /api/v1/providers/me/availability | Proveedor | Consulta DisponibilidadProveedor. |
| PRV-07 | POST | /api/v1/providers/me/availability | Proveedor | Registra disponibilidad o indisponibilidad. |

### /payments

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| PAY-01 | POST | /api/v1/payments | Cliente | Inicia Pago mediante PaymentService y Stripe. |
| PAY-02 | GET | /api/v1/payments/{id} | Autenticado | Consulta Pago según permisos. |
| PAY-03 | GET | /api/v1/appointments/{id}/payments | Autenticado | Lista pagos de una Cita. |
| PAY-04 | POST | /api/v1/payments/{id}/retry | Cliente | Reintenta un Pago FALLIDO. |
| PAY-05 | GET | /api/v1/payments | Administrador | Consulta pagos. |
| PAY-06 | POST | /api/v1/payments/manual | Recepción | Registra o valida pago manual. |
| PAY-07 | GET | /api/v1/payments/{id}/transactions | Administrador | Consulta Transaccion sin datos sensibles. |

### /refunds

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| REF-01 | POST | /api/v1/refunds | Sistema o personal autorizado | Procesa Devolucion según política. |
| REF-02 | GET | /api/v1/refunds/{id} | Autenticado | Consulta Devolucion según permisos. |
| REF-03 | GET | /api/v1/payments/{id}/refunds | Autenticado | Lista devoluciones de un Pago. |
| REF-04 | GET | /api/v1/refunds | Administrador | Consulta devoluciones. |

### /reports

| ID | Método | Ruta | Roles | Propósito |
| --- | --- | --- | --- | --- |
| RPT-01 | GET | /api/v1/reports/appointments-by-day | Administrador | Citas por día. |
| RPT-02 | GET | /api/v1/reports/top-treatments | Administrador | Tratamientos solicitados. |
| RPT-03 | GET | /api/v1/reports/top-cabins | Administrador | Cabinas utilizadas. |
| RPT-04 | GET | /api/v1/reports/cancellations | Administrador | Cancelaciones y devoluciones. |
| RPT-05 | GET | /api/v1/reports/cabin-occupancy | Administrador | Ocupación por CitaCabina y estado. |

Total recalculado: 73 endpoints en 14 módulos.

## 6. DTO principales

| DTO | Campos mínimos |
| --- | --- |
| AuthResponse | accessToken, expiresAt, user, role; el refresh token seguro no se devuelve en texto plano cuando se usa cookie HttpOnly. |
| ClientResponse | idCliente, idUsuario, nombre, correo, telefono, activo, preferences. |
| AppointmentResponse | idCita, cliente, tratamiento, proveedor, cabinas, inicio, fin, numeroPersonas, estado, precioUnitario, importe, bloqueoExpiraEn. |
| CabinResponse | idCabina, nombre, tipo, capacidadMaxima, prioridad, activo, estado. |
| PackageResponse | idPaquete, nombre, descripcion, activo, tratamientos. |
| PaymentResponse | idPago, idCita, monto, moneda, metodoPago, estado, referencia, fechas. |
| RefundResponse | idDevolucion, idPago, idCancelacion, tipo, porcentaje, monto, estado, fechas. |
| AvailabilityResponse | slots de 30 minutos, elegibilidad, motivo de rechazo, bloqueoExpiraEn. |
| CabinRecommendationResponse | cabina, orden, criteriosAplicados y explicación reproducible. |

## 7. Validaciones obligatorias

- Rechazar inicios antes de 09:00 y finales posteriores a 20:00.

- Aceptar operación regular solo de lunes a sábado y aplicar excepciones configuradas.

- Rechazar Citas con menos de 2 horas o más de 60 días de anticipación.

- Generar inicios cada 30 minutos y expirar el bloqueo exactamente a los 15 minutos.

- Calcular precio_unitario desde Tratamiento.precio_base vigente, importe por personas y total como suma sin cargos adicionales en el MVP.

- Aplicar devolución de 100 %, 50 % o 0 % según RN-101 a RN-105; una causa atribuible al spa aplica 100 %.

- Si el Pago queda PAGADO pero se pierde disponibilidad, no confirmar la Cita e iniciar la devolución aplicable.

- Ordenar recomendaciones exactamente conforme a RN-108 y desempatar por id_cabina ascendente.

## 8. Flujos REST

Flujo cliente: consultar catálogo o Paquete → gestionar Carrito → validar disponibilidad → POST /appointments → POST /payments cuando corresponda → POST /appointments/{id}/confirm. La confirmación revalida recursos y nunca confía en un monto calculado por Angular.

Flujo de cancelación: POST /appointments/{id}/cancel → registrar Cancelacion → calcular porcentaje → crear Devolucion cuando corresponda → procesar mediante PaymentService → conservar Transaccion.

## 9. Cobertura de casos de uso

| Casos | Módulos principales |
| --- | --- |
| CU-01 a CU-02 | /auth, /clients |
| CU-03 a CU-06 | /treatments, /packages, /cart |
| CU-07 a CU-10 | /recommendations, /availability, /cart |
| CU-11 a CU-14 | /appointments, /payments, /refunds |
| CU-15 a CU-22 | /users, /treatments, /packages, /cabins, /providers, /reports |
| CU-23 a CU-29 | /appointments, /availability, /cabins, /payments |
| CU-30 a CU-38 | /providers, /appointments |
| CU-39 a CU-43 | /payments, /appointments, /refunds |

## 10. Estado de decisiones

No quedan como pendientes DP-OP-01 a DP-OP-13, DP-EC-01, DP-EC-02 ni DP-TEC-01 a DP-TEC-03. Sus valores están incorporados en RN-91 a RN-108 y en este contrato.
