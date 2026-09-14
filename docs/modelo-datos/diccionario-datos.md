# TZISCA

**DICCIONARIO DE DATOS Y MODELO DE DATOS DEPURADO**

Versión alineada al modelo vigente de 19 entidades y a RN-01 a RN-108. Fecha de actualización: 13/09/2026.

## 1. Propósito del documento

Este documento define el modelo lógico y físico de referencia para SQL Server. La estructura se organiza en seguridad, catalogo, reservas, operacion y pagos. No declara como entidades oficiales conceptos del modelo anterior.

## 2. Convenciones de SQL Server

Los tipos indicados son tipos físicos de SQL Server. PK identifica llave primaria, FK llave foránea, UNIQUE unicidad y DEFAULT el valor aplicado por la base. Las fechas operativas deben almacenarse de forma consistente y convertirse a la zona horaria de la aplicación.

## 3. Inventario oficial de entidades

| Núm. | Entidad | Schema | Finalidad |
| --- | --- | --- | --- |
| 1 | Rol | seguridad | Define los roles funcionales y de autorización. |
| 2 | Usuario | seguridad | Representa la cuenta de acceso vinculada con ASP.NET Core Identity. |
| 3 | Cliente | seguridad | Distingue el perfil de cliente de la cuenta Usuario. |
| 4 | PreferenciaCliente | seguridad | Guarda preferencias opcionales para recomendaciones. |
| 5 | Tratamiento | catalogo | Define cada servicio ofrecido por el spa. |
| 6 | Carrito | reservas | Agrupa selecciones temporales del Cliente antes de confirmar Citas. |
| 7 | Proveedor | operacion | Representa al usuario que puede atender tratamientos. |
| 8 | TratamientoProveedor | operacion | Resuelve la relación N:M entre Tratamiento y Proveedor. |
| 9 | DisponibilidadProveedor | operacion | Registra disponibilidad o indisponibilidad operativa por intervalo. |
| 10 | Paquete | catalogo | Representa un conjunto comercial o funcional de tratamientos. |
| 11 | PaqueteTratamiento | catalogo | Resuelve la relación N:M entre Paquete y Tratamiento. |
| 12 | Cabina | operacion | Representa cada cabina física y separa habilitación de estado operativo. |
| 13 | EstadoCabina | operacion | Conserva el historial de cambios de Cabina.estado. |
| 14 | Cita | reservas | Unidad principal de agenda; representa un tratamiento programado para un Cliente. |
| 15 | CitaCabina | reservas | Resuelve la relación N:M entre Cita y Cabina. |
| 16 | Pago | pagos | Registra cobros asociados a una Cita. |
| 17 | Cancelacion | pagos | Conserva el motivo y momento de cancelar una Cita. |
| 18 | Devolucion | pagos | Registra reembolsos totales, parciales o de monto cero derivados de la política. |
| 19 | Transaccion | pagos | Conserva intentos y respuestas técnicas de PaymentService y Stripe. |

Total oficial: 19 entidades.

## 4. Diccionario de datos

### 4.1. Rol

Finalidad: Define los roles funcionales y de autorización.

Schema: seguridad. Nombre físico: seguridad.Rol.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_rol | INT IDENTITY | PK | No | Sí | — | > 0 |
| nombre | NVARCHAR(50) | — | No | Sí | — | No vacío |
| descripcion | NVARCHAR(250) | — | Sí | No | NULL | — |
| activo | BIT | — | No | No | 1 | 0 o 1 |

Relaciones y cardinalidades: Rol 1:N Usuario.

### 4.2. Usuario

Finalidad: Representa la cuenta de acceso vinculada con ASP.NET Core Identity.

Schema: seguridad. Nombre físico: seguridad.Usuario.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_usuario | INT IDENTITY | PK | No | Sí | — | > 0 |
| id_rol | INT | FK → seguridad.Rol | No | No | — | Rol existente |
| identity_user_id | NVARCHAR(450) | — | No | Sí | — | Identificador de Identity |
| nombre | NVARCHAR(100) | — | No | No | — | No vacío |
| correo | NVARCHAR(256) | — | No | Sí | — | Formato válido |
| activo | BIT | — | No | No | 1 | 0 o 1 |
| fecha_registro | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |

Relaciones y cardinalidades: Rol 1:N Usuario; Usuario 1:0..1 Cliente; Usuario 1:0..1 Proveedor.

### 4.3. Cliente

Finalidad: Distingue el perfil de cliente de la cuenta Usuario.

Schema: seguridad. Nombre físico: seguridad.Cliente.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_cliente | INT IDENTITY | PK | No | Sí | — | > 0 |
| id_usuario | INT | FK → seguridad.Usuario | No | Sí | — | Usuario existente |
| telefono | NVARCHAR(25) | — | Sí | No | NULL | — |
| activo | BIT | — | No | No | 1 | 0 o 1 |
| fecha_registro | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |

Relaciones y cardinalidades: Usuario 1:0..1 Cliente; Cliente 1:0..1 PreferenciaCliente; Cliente 1:N Carrito; Cliente 1:N Cita.

### 4.4. PreferenciaCliente

Finalidad: Guarda preferencias opcionales para recomendaciones.

Schema: seguridad. Nombre físico: seguridad.PreferenciaCliente.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_preferencia | INT IDENTITY | PK | No | Sí | — | > 0 |
| id_cliente | INT | FK → seguridad.Cliente | No | Sí | — | Cliente existente |
| tipo_experiencia | NVARCHAR(100) | — | Sí | No | NULL | — |
| caracteristicas | NVARCHAR(500) | — | Sí | No | NULL | — |
| observaciones | NVARCHAR(500) | — | Sí | No | NULL | — |
| fecha_actualizacion | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |

Relaciones y cardinalidades: Cliente 1:0..1 PreferenciaCliente.

### 4.5. Tratamiento

Finalidad: Define cada servicio ofrecido por el spa.

Schema: catalogo. Nombre físico: catalogo.Tratamiento.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_tratamiento | INT IDENTITY | PK | No | Sí | — | > 0 |
| nombre | NVARCHAR(120) | — | No | Sí | — | No vacío |
| descripcion | NVARCHAR(MAX) | — | No | No | — | — |
| duracion_minutos | INT | — | No | No | — | > 0 |
| precio_base | DECIMAL(10,2) | — | No | No | — | >= 0; precio por persona |
| requisitos_cabina | NVARCHAR(500) | — | Sí | No | NULL | — |
| activo | BIT | — | No | No | 1 | 0 o 1 |

Relaciones y cardinalidades: Tratamiento N:M Proveedor mediante TratamientoProveedor; Paquete N:M Tratamiento mediante PaqueteTratamiento; Tratamiento 1:N Cita.

### 4.6. Carrito

Finalidad: Agrupa selecciones temporales del Cliente antes de confirmar Citas.

Schema: reservas. Nombre físico: reservas.Carrito.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_carrito | BIGINT IDENTITY | PK | No | Sí | — | > 0 |
| id_cliente | INT | FK → seguridad.Cliente | No | No | — | Cliente existente |
| estado | NVARCHAR(20) | — | No | No | 'ACTIVO' | ACTIVO, CONVERTIDO, ABANDONADO, EXPIRADO |
| fecha_creacion | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |
| fecha_actualizacion | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |

Relaciones y cardinalidades: Cliente 1:N Carrito; Carrito 1:N Cita. El bloqueo de 15 minutos se conserva en Cita.fecha_expiracion_bloqueo.

### 4.7. Proveedor

Finalidad: Representa al usuario que puede atender tratamientos.

Schema: operacion. Nombre físico: operacion.Proveedor.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_proveedor | INT IDENTITY | PK | No | Sí | — | > 0 |
| id_usuario | INT | FK → seguridad.Usuario | No | Sí | — | Usuario existente |
| activo | BIT | — | No | No | 1 | 0 o 1 |
| fecha_alta | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |

Relaciones y cardinalidades: Usuario 1:0..1 Proveedor; Proveedor 1:N Cita; Proveedor N:M Tratamiento mediante TratamientoProveedor.

### 4.8. TratamientoProveedor

Finalidad: Resuelve la relación N:M entre Tratamiento y Proveedor.

Schema: operacion. Nombre físico: operacion.TratamientoProveedor.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_tratamiento | INT | PK/FK → catalogo.Tratamiento | No | Sí compuesta | — | Tratamiento existente |
| id_proveedor | INT | PK/FK → operacion.Proveedor | No | Sí compuesta | — | Proveedor existente |
| activo | BIT | — | No | No | 1 | 0 o 1 |

Relaciones y cardinalidades: Tratamiento N:M Proveedor mediante TratamientoProveedor; UNIQUE(id_tratamiento, id_proveedor).

### 4.9. DisponibilidadProveedor

Finalidad: Registra disponibilidad o indisponibilidad operativa por intervalo.

Schema: operacion. Nombre físico: operacion.DisponibilidadProveedor.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_disponibilidad | BIGINT IDENTITY | PK | No | Sí | — | > 0 |
| id_proveedor | INT | FK → operacion.Proveedor | No | No | — | Proveedor existente |
| fecha_hora_inicio | DATETIME2 | — | No | No | — | Menor que fecha_hora_fin |
| fecha_hora_fin | DATETIME2 | — | No | No | — | Mayor que fecha_hora_inicio |
| tipo | NVARCHAR(20) | — | No | No | — | DISPONIBLE o NO_DISPONIBLE |
| motivo | NVARCHAR(250) | — | Sí | No | NULL | — |
| activo | BIT | — | No | No | 1 | 0 o 1 |

Relaciones y cardinalidades: Proveedor 1:N DisponibilidadProveedor.

### 4.10. Paquete

Finalidad: Representa un conjunto comercial o funcional de tratamientos.

Schema: catalogo. Nombre físico: catalogo.Paquete.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_paquete | INT IDENTITY | PK | No | Sí | — | > 0 |
| nombre | NVARCHAR(120) | — | No | Sí | — | No vacío |
| descripcion | NVARCHAR(MAX) | — | Sí | No | NULL | — |
| activo | BIT | — | No | No | 1 | 0 o 1 |

Relaciones y cardinalidades: Paquete N:M Tratamiento mediante PaqueteTratamiento.

### 4.11. PaqueteTratamiento

Finalidad: Resuelve la relación N:M entre Paquete y Tratamiento.

Schema: catalogo. Nombre físico: catalogo.PaqueteTratamiento.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_paquete | INT | PK/FK → catalogo.Paquete | No | Sí compuesta | — | Paquete existente |
| id_tratamiento | INT | PK/FK → catalogo.Tratamiento | No | Sí compuesta | — | Tratamiento existente |

Relaciones y cardinalidades: Paquete N:M Tratamiento mediante PaqueteTratamiento; UNIQUE(id_paquete, id_tratamiento).

### 4.12. Cabina

Finalidad: Representa cada cabina física y separa habilitación de estado operativo.

Schema: operacion. Nombre físico: operacion.Cabina.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_cabina | INT IDENTITY | PK | No | Sí | — | > 0 |
| nombre | NVARCHAR(120) | — | No | Sí | — | No vacío |
| tipo | NVARCHAR(60) | — | No | No | — | Tipo funcional vigente |
| descripcion | NVARCHAR(MAX) | — | Sí | No | NULL | — |
| capacidad_maxima | INT | — | No | No | — | > 0 |
| caracteristicas | NVARCHAR(MAX) | — | Sí | No | NULL | — |
| beneficios | NVARCHAR(MAX) | — | Sí | No | NULL | — |
| prioridad | INT | — | No | No | 0 | >= 0 |
| activo | BIT | — | No | No | 1 | 0 o 1 |
| estado | NVARCHAR(20) | — | No | No | 'DISPONIBLE' | DISPONIBLE, OCUPADA, LIMPIEZA, MANTENIMIENTO |

Relaciones y cardinalidades: Cabina N:M Cita mediante CitaCabina; Cabina 1:N EstadoCabina.

### 4.13. EstadoCabina

Finalidad: Conserva el historial de cambios de Cabina.estado.

Schema: operacion. Nombre físico: operacion.EstadoCabina.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_estado_cabina | BIGINT IDENTITY | PK | No | Sí | — | > 0 |
| id_cabina | INT | FK → operacion.Cabina | No | No | — | Cabina existente |
| estado_anterior | NVARCHAR(20) | — | Sí | No | NULL | Catálogo de Cabina.estado |
| estado_nuevo | NVARCHAR(20) | — | No | No | — | Catálogo de Cabina.estado |
| fecha_cambio | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |
| id_usuario | INT | FK → seguridad.Usuario | Sí | No | NULL | Responsable o proceso automático |
| motivo | NVARCHAR(250) | — | Sí | No | NULL | — |

Relaciones y cardinalidades: Cabina 1:N EstadoCabina. El trigger operacion.TR_Cabina_CambioEstado registra los cambios.

### 4.14. Cita

Finalidad: Unidad principal de agenda; representa un tratamiento programado para un Cliente.

Schema: reservas. Nombre físico: reservas.Cita.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_cita | BIGINT IDENTITY | PK | No | Sí | — | > 0 |
| id_cliente | INT | FK → seguridad.Cliente | No | No | — | Cliente existente |
| id_tratamiento | INT | FK → catalogo.Tratamiento | No | No | — | Tratamiento activo |
| id_proveedor | INT | FK → operacion.Proveedor | Sí | No | NULL | Proveedor autorizado y disponible |
| id_carrito | BIGINT | FK → reservas.Carrito | Sí | No | NULL | Carrito origen cuando exista |
| numero_personas | INT | — | No | No | 1 | > 0 |
| fecha_hora_inicio | DATETIME2 | — | No | No | — | Inicio en intervalo de 30 minutos |
| fecha_hora_fin | DATETIME2 | — | No | No | — | Posterior al inicio y a más tardar 20:00 |
| estado | NVARCHAR(20) | — | No | No | 'PENDIENTE' | PENDIENTE, CONFIRMADA, EN_ATENCION, COMPLETADA, CANCELADA, EXPIRADA |
| precio_unitario | DECIMAL(10,2) | — | No | No | — | >= 0; copia de precio_base al reservar |
| importe | DECIMAL(10,2) | — | No | No | — | precio_unitario × numero_personas |
| fecha_expiracion_bloqueo | DATETIME2 | — | Sí | No | NULL | Creación + 15 minutos mientras PENDIENTE |
| fecha_creacion | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |
| fecha_confirmacion | DATETIME2 | — | Sí | No | NULL | — |
| observaciones | NVARCHAR(500) | — | Sí | No | NULL | — |

Relaciones y cardinalidades: Cliente 1:N Cita; Tratamiento 1:N Cita; Proveedor 1:N Cita; Carrito 1:N Cita; Cita N:M Cabina mediante CitaCabina; Cita 1:0..N Pago y Cancelacion.

### 4.15. CitaCabina

Finalidad: Resuelve la relación N:M entre Cita y Cabina.

Schema: reservas. Nombre físico: reservas.CitaCabina.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_cita | BIGINT | PK/FK → reservas.Cita | No | Sí compuesta | — | Cita existente |
| id_cabina | INT | PK/FK → operacion.Cabina | No | Sí compuesta | — | Cabina activa, elegible y sin traslape |
| fecha_asignacion | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |
| activo | BIT | — | No | No | 1 | 0 o 1 |

Relaciones y cardinalidades: Cita N:M Cabina mediante CitaCabina; UNIQUE(id_cita, id_cabina).

### 4.16. Pago

Finalidad: Registra cobros asociados a una Cita.

Schema: pagos. Nombre físico: pagos.Pago.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_pago | BIGINT IDENTITY | PK | No | Sí | — | > 0 |
| id_cita | BIGINT | FK → reservas.Cita | No | No | — | Cita existente |
| monto | DECIMAL(10,2) | — | No | No | — | > 0; calculado por backend |
| moneda | CHAR(3) | — | No | No | 'MXN' | Código ISO 4217 |
| metodo_pago | NVARCHAR(40) | — | No | No | — | — |
| estado | NVARCHAR(30) | — | No | No | 'PENDIENTE' | Catálogo RN-77 |
| referencia | NVARCHAR(150) | — | Sí | No | NULL | — |
| fecha_creacion | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |
| fecha_pago | DATETIME2 | — | Sí | No | NULL | — |

Relaciones y cardinalidades: Cita 1:0..N Pago; Pago 1:0..N Devolucion y Transaccion.

### 4.17. Cancelacion

Finalidad: Conserva el motivo y momento de cancelar una Cita.

Schema: pagos. Nombre físico: pagos.Cancelacion.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_cancelacion | BIGINT IDENTITY | PK | No | Sí | — | > 0 |
| id_cita | BIGINT | FK → reservas.Cita | No | No | — | Cita existente |
| id_usuario | INT | FK → seguridad.Usuario | No | No | — | Responsable existente |
| motivo | NVARCHAR(500) | — | No | No | — | No vacío |
| atribuible_spa | BIT | — | No | No | 0 | 0 o 1 |
| fecha_cancelacion | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |
| observaciones | NVARCHAR(500) | — | Sí | No | NULL | — |

Relaciones y cardinalidades: Cita 1:0..N Cancelacion; Cancelacion 1:0..N Devolucion.

### 4.18. Devolucion

Finalidad: Registra reembolsos totales, parciales o de monto cero derivados de la política.

Schema: pagos. Nombre físico: pagos.Devolucion.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_devolucion | BIGINT IDENTITY | PK | No | Sí | — | > 0 |
| id_pago | BIGINT | FK → pagos.Pago | No | No | — | Pago existente |
| id_cancelacion | BIGINT | FK → pagos.Cancelacion | No | No | — | Cancelacion existente |
| tipo | NVARCHAR(20) | — | No | No | — | TOTAL, PARCIAL o SIN_DEVOLUCION |
| porcentaje | DECIMAL(5,2) | — | No | No | — | 0, 50 o 100 |
| monto | DECIMAL(10,2) | — | No | No | 0 | >= 0 y no mayor que lo pagado |
| estado | NVARCHAR(20) | — | No | No | 'PENDIENTE' | PENDIENTE, PROCESANDO, COMPLETADA, FALLIDA, CANCELADA |
| fecha_solicitud | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |
| fecha_procesamiento | DATETIME2 | — | Sí | No | NULL | — |
| id_usuario_responsable | INT | FK → seguridad.Usuario | Sí | No | NULL | — |

Relaciones y cardinalidades: Pago 1:0..N Devolucion; Cancelacion 1:0..N Devolucion.

### 4.19. Transaccion

Finalidad: Conserva intentos y respuestas técnicas de PaymentService y Stripe.

Schema: pagos. Nombre físico: pagos.Transaccion.

| Atributo | Tipo SQL Server | Llave | NULL | UNIQUE | DEFAULT | CHECK o restricción |
| --- | --- | --- | --- | --- | --- | --- |
| id_transaccion | BIGINT IDENTITY | PK | No | Sí | — | > 0 |
| id_pago | BIGINT | FK → pagos.Pago | No | No | — | Pago existente |
| referencia_externa | NVARCHAR(150) | — | Sí | No | NULL | — |
| proveedor_pago | NVARCHAR(40) | — | No | No | 'STRIPE' | Pasarela detrás de PaymentService |
| tipo | NVARCHAR(30) | — | No | No | — | PAGO, REINTENTO o DEVOLUCION |
| estado | NVARCHAR(30) | — | No | No | — | Estado técnico |
| codigo_respuesta | NVARCHAR(100) | — | Sí | No | NULL | Sin datos bancarios sensibles |
| fecha | DATETIME2 | — | No | No | SYSUTCDATETIME() | — |

Relaciones y cardinalidades: Pago 1:0..N Transaccion.

## 5. Relaciones y cardinalidades finales

| Entidad A | Entidad B | Cardinalidad | Implementación |
| --- | --- | --- | --- |
| Rol | Usuario | 1:N | Cada Usuario pertenece a un Rol. |
| Usuario | Cliente | 1:0..1 | Cliente extiende la cuenta cuando el usuario es cliente. |
| Usuario | Proveedor | 1:0..1 | Proveedor extiende la cuenta cuando presta servicios. |
| Cliente | PreferenciaCliente | 1:0..1 | Las preferencias son opcionales. |
| Cliente | Carrito | 1:N | Un cliente puede conservar carritos históricos. |
| Cliente | Cita | 1:N | Una persona cliente puede tener muchas citas. |
| Tratamiento | Proveedor | N:M | Resuelta por TratamientoProveedor. |
| Paquete | Tratamiento | N:M | Resuelta por PaqueteTratamiento. |
| Carrito | Cita | 1:N | Un carrito puede originar varias citas independientes. |
| Tratamiento | Cita | 1:N | Cada cita programa un tratamiento. |
| Proveedor | Cita | 1:N | Una cita puede tener un proveedor asignado. |
| Proveedor | DisponibilidadProveedor | 1:N | Historial por intervalos. |
| Cita | Cabina | N:M | Resuelta por CitaCabina. |
| Cabina | EstadoCabina | 1:N | Historial de cambios de estado. |
| Cita | Pago | 1:0..N | Permite reintentos de cobro. |
| Cita | Cancelacion | 1:0..N | Conserva intentos o movimientos de cancelación. |
| Pago | Devolucion | 1:0..N | Un pago puede no tener o tener varias devoluciones. |
| Cancelacion | Devolucion | 1:0..N | La devolución identifica su causa. |
| Pago | Transaccion | 1:0..N | Historial técnico de Stripe/PaymentService. |

## 6. Reglas físicas y de integridad

- Las relaciones N:M se implementan únicamente mediante TratamientoProveedor, PaqueteTratamiento y CitaCabina.

- Cabina.activo indica si el registro está habilitado; Cabina.estado indica DISPONIBLE, OCUPADA, LIMPIEZA o MANTENIMIENTO.

- operacion.TR_Cabina_CambioEstado debe insertar el cambio en operacion.EstadoCabina cuando Cabina.estado cambie.

- La lógica existente de cancelaciones y pagos debe verificarse contra los scripts SQL cuando estén disponibles; su nombre de trigger no se inventa en este documento.

- Cita.fecha_expiracion_bloqueo materializa el bloqueo de 15 minutos sin crear una entidad adicional.

- Tratamiento.precio_base es por persona; Cita.precio_unitario congela ese precio y Cita.importe multiplica por numero_personas.

- Los horarios deben iniciar cada 30 minutos, respetar 2 horas de anticipación mínima, 60 días máxima y terminar a más tardar a las 20:00 de lunes a sábado.

- Las tablas de ASP.NET Core Identity son infraestructura de autenticación y no se cuentan como entidades de dominio dentro de las 19.

## 7. Schemas y roles SQL

| Schema | Entidades |
| --- | --- |
| seguridad | Rol, Usuario, Cliente, PreferenciaCliente |
| catalogo | Tratamiento, Paquete, PaqueteTratamiento |
| reservas | Carrito, Cita, CitaCabina |
| operacion | Proveedor, TratamientoProveedor, DisponibilidadProveedor, Cabina, EstadoCabina |
| pagos | Pago, Cancelacion, Devolucion, Transaccion |

| Rol SQL | Alcance previsto |
| --- | --- |
| rol_cliente | Operaciones propias de cliente a través de la API; sin acceso directo amplio a tablas. |
| rol_administrador | Administración autorizada de los cinco schemas y consulta de reportes. |
| rol_recepcionista | Operación de reservas, cabinas y pagos según procedimientos o permisos mínimos. |
| rol_proveedor | Consulta y actualización limitada a su agenda y disponibilidad. |

## 8. Decisiones vigentes

RN-91 a RN-108 reemplazan el bloque de decisiones pendientes. Las políticas de horario, agenda, tolerancia, cancelación, precio, autenticación, Stripe y recomendación determinista están aprobadas.

## 9. Punto pendiente de verificación

No existen scripts SQL en la carpeta local. Antes de implementar el modelo físico se debe contrastar este diccionario con los scripts reales, confirmar el trigger de cancelaciones y pagos que ya exista y validar los GRANT efectivos de los cuatro roles SQL. Esta verificación no cambia el inventario oficial de 19 entidades.
