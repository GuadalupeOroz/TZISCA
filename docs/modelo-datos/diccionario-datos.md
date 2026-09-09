*Documento corregido y cerrado — Versión cerrada, 08/09/2026. Fuente oficial para el Diagrama Entidad–Relación, el modelo físico en SQL Server, el Diseño de API REST y la Matriz de Trazabilidad.*

> ***Nota de versión — documento cerrado. Esta es la versión corregida y cerrada del Diccionario de Datos y Modelo de Datos Depurado de TZISCA, actualizada el 08/09/2026 conforme a las instrucciones de corrección y cierre vigentes. El trabajo se realizó sobre la versión anterior del documento: no se reconstruyó desde cero, no se eliminaron entidades válidas y no se inventaron valores operativos o económicos pendientes de aprobación. Los cambios principales respecto a la versión previa son: (1) actualización de la referencia CU-01 a CU-38 → CU-01 a CU-43, y de RN-01 a RN-72 → RN-01 a RN-90; (2) incorporación de Reservacion.estado_reservacion (EN_PROCESO, CONFIRMADA, CANCELADA, EXPIRADA) y del ciclo formal de dos niveles con ReservacionTratamiento.estado; (3) cambio del default de ReservacionTratamiento.estado a PENDIENTE y normalización de su catálogo a PENDIENTE, CONFIRMADO, EN_ATENCION, COMPLETADO y CANCELADO, incluyendo la transición PENDIENTE→CANCELADO; (4) eliminación de la duración fija de 10 minutos en BloqueoTemporal.fecha_expiracion, trasladada a un parámetro operativo configurable pendiente de aprobación; (5) confirmación de los catálogos técnicos únicos de Pago.estado_pago y Devolucion.estado_devolucion, sin agregar RECHAZADO como estado adicional; (6) incorporación de Devolucion.tipo_devolucion (PARCIAL/TOTAL), separado del estado técnico de la devolución; (7) TransaccionPago se mantiene como entidad opcional; (8) DP-EC-01 documenta la fórmula del importe como decisión pendiente, sin inventarla; (9) nueva sección de configuración operativa pendiente de modelado físico (horario, días laborales, excepciones, intervalos de agenda, anticipación mínima y máxima, duración del bloqueo); (10) corrección de las cardinalidades Reservacion→Pago, ReservacionTratamiento→Devolucion y Pago→TransaccionPago a 1:0..N; (11) eliminación de la decisión pendiente sobre el uso de PENDIENTE, ya resuelta. El documento queda cerrado como fuente oficial para el Diagrama Entidad–Relación, el modelo físico en SQL Server, el Diseño de API REST y la Matriz de Trazabilidad.***

# 1. Propósito del documento

Este documento consolida la versión de trabajo del diccionario de datos de TZISCA y las decisiones de depuración realizadas antes de elaborar el Diagrama Entidad–Relación. El modelo se deriva del alcance funcional definido en los Casos de Uso CU-01 a CU-43, las Reglas de Negocio RN-01 a RN-90, los Criterios de Aceptación CU-01 a CU-43, la Propuesta General TZISCA actualizada, el Catálogo de Cabinas y Servicios TZISCA actualizado y el Diseño de API REST TZISCA.

La versión actual se declara derivada de:

\- Casos de Uso CU-01 a CU-43.

\- Reglas de Negocio RN-01 a RN-90.

\- Criterios de Aceptación CU-01 a CU-43.

\- Propuesta General TZISCA actualizada.

\- Catálogo de Cabinas y Servicios TZISCA actualizado.

\- Diseño de API REST TZISCA.

La finalidad es establecer qué entidades necesita el sistema, qué información almacenará cada una, cómo se relacionan y qué redundancias se evitarán antes de convertir el modelo en un esquema relacional definitivo para SQL Server.

# 2. Convención de tipos de datos acordada

Por decisión del proyecto, los campos de cadena o contenido alfanumérico se documentan utilizando el tipo TEXT. No se utilizará VARCHAR en este diccionario. TEXT se emplea aquí como convención conceptual del modelo; la implementación física en SQL Server definirá la equivalencia técnica adecuada para cada campo sin modificar la nomenclatura de este diccionario.

| **Tipo** | **Uso** |
|----|----|
| INT | Identificadores y cantidades enteras. |
| BIGINT | Identificadores de tablas transaccionales o históricas con mayor crecimiento. |
| BIT | Valores lógicos: sí/no, activo/inactivo. |
| DATE | Fechas sin hora cuando se requiera. |
| TIME | Horas sin fecha cuando se requiera. |
| DATETIME2 | Fecha y hora para eventos, intervalos, auditoría y programación. |
| TEXT | Cualquier cadena de caracteres o contenido textual. |
| DECIMAL(10,2) | Importes monetarios y valores económicos con dos posiciones decimales. |

# 3. Principios del modelo depurado

| **Decisión** | **Aplicación** |
|----|----|
| Reservación con detalle independiente | Una reservación funciona como encabezado y puede contener uno o varios tratamientos. Cada tratamiento conserva de forma independiente cabina, personas, horario y estado. |
| Usuarios centralizados | Cliente, Administrador general, Recepción y cabinas y Proveedor comparten la tabla Usuario; no se crean tablas separadas para cada rol. |
| Proveedor como extensión | Proveedor amplía la cuenta Usuario únicamente con la información específica necesaria para asignaciones e indisponibilidades. |
| Compatibilidades N:M | Tratamiento–Cabina y Proveedor–Tratamiento se resuelven mediante tablas intermedias. |
| Carrito separado de reservación | Agregar un servicio al carrito no equivale a reservarlo; por eso Carrito y CarritoTratamiento permanecen separados de Reservacion. |
| Dos tipos de bloqueo | BloqueoTemporal protege el horario durante el carrito; BloqueoCabina representa causas operativas como mantenimiento o limpieza. |
| Asignación de proveedor con historial | La tabla AsignacionProveedor conserva la asignación vigente y las sustituciones, evitando duplicar id_proveedor dentro de ReservacionTratamiento. |
| Ciclo formal de dos niveles (Reservacion / ReservacionTratamiento) | Toda reservación nace en EN_PROCESO con sus tratamientos en PENDIENTE. Solo cuando el pago es aprobado y la disponibilidad se revalida, Reservacion pasa a CONFIRMADA y cada ReservacionTratamiento válido pasa de PENDIENTE a CONFIRMADO (RN-38 a RN-41, RN-73, RN-75; ver sección 4 de este documento). Los tratamientos siguen manejando estados independientes entre sí y respecto al estado general de la reservación. |
| Estado actual + historial | Cabina, Reservacion y ReservacionTratamiento conservan el estado actual, mientras los historiales registran las transiciones. |
| Sin tabla Reporte | Los reportes básicos se obtienen consultando y agregando datos de reservaciones, tratamientos, cabinas y cancelaciones. |
| Sin Bitácora general en esta versión | La trazabilidad funcional queda cubierta por tablas específicas de historial, bloqueos, asignaciones y cancelaciones; una auditoría técnica global podrá añadirse si el proyecto la requiere posteriormente. |
| Pagos separados de la reservación | La información financiera no se almacena directamente dentro de Reservacion. Cada reservación puede relacionarse con uno o más registros de Pago para conservar intentos, estados y trazabilidad. Las devoluciones se registran mediante Devolucion, permitiendo movimientos totales o parciales sin eliminar el historial original del pago. Pago permanece separado de Reservacion y Devolucion permanece separada de Pago; esta versión no modifica esa decisión. |
| Importes históricos independientes del catálogo | El precio vigente de un tratamiento puede modificarse posteriormente. ReservacionTratamiento conserva el precio aplicado y el importe de la instancia al momento de reservar para no alterar información histórica. La fórmula exacta de cálculo del importe (por persona o por tratamiento completo) queda pendiente de aprobación conforme a DP-EC-01 (sección 11). |

# 4. Ciclo de vida de estados: Reservacion y ReservacionTratamiento

Este documento resuelve el uso ambiguo del estado Pendiente señalado en la versión anterior. TZISCA maneja dos niveles de estado independientes pero coordinados: el estado general de la reservación (Reservacion.estado_reservacion) y el estado de cada tratamiento que la compone (ReservacionTratamiento.estado). El flujo adoptado es consistente con RN-38 a RN-45 y RN-73 a RN-81 de Reglas de Negocio Horario y Políticas TZISCA (documento canónico).

## 4.1 Estados de Reservacion.estado_reservacion

| **Estado** | **Significado** |
|----|----|
| EN_PROCESO | Reservación creada antes de completarse el pago o la confirmación definitiva. Es el estado inicial (default). |
| CONFIRMADA | Los recursos quedaron confirmados y la condición económica se cumplió (pago aprobado y disponibilidad revalidada). |
| CANCELADA | La reservación fue cancelada en su totalidad. |
| EXPIRADA | El proceso no llegó a confirmarse y el bloqueo temporal asociado expiró; no puede continuar sin una nueva validación de disponibilidad. |

**Transiciones formales de Reservacion:**

| **Origen** | **Destino** | **Condición** |
|----|----|----|
| EN_PROCESO | CONFIRMADA | Pago aprobado y disponibilidad válida durante todo el intervalo (RN-39, RN-40, RN-75). |
| EN_PROCESO | CANCELADA | Cancelación solicitada antes de completarse el pago o la confirmación. |
| EN_PROCESO | EXPIRADA | El bloqueo temporal vigente expira sin que el pago o la confirmación se completen (RN-34, RN-81). |
| CONFIRMADA | CANCELADA | Cancelación posterior a la confirmación, sujeta a las reglas de Cancelacion y Devolucion. |

**Nota de consistencia (DP-EC-02):** el caso en que el pago ya fue aprobado pero el bloqueo temporal expiró y la revalidación de disponibilidad resultó negativa está identificado formalmente como DP-EC-02 en Reglas de Negocio Horario y Políticas TZISCA (sección 17.2) y permanece pendiente de aprobación: la Reservacion no debe confirmarse mientras no exista disponibilidad, pero el tratamiento económico exacto (conciliación, devolución u otro mecanismo) no se define en este diccionario. El catálogo de ReservacionTratamiento.estado tampoco define un valor EXPIRADO equivalente (ver 4.2); el tratamiento de los registros PENDIENTE de una reservación EXPIRADA queda sujeto a la resolución de DP-EC-02.

## 4.2 Estados de ReservacionTratamiento.estado

| **Estado** | **Significado** |
|----|----|
| PENDIENTE | Estado inicial (default) de todo tratamiento de una reservación generada antes del pago (RN-38, RN-42). |
| CONFIRMADO | El pago fue aprobado y la disponibilidad revalidada resultó satisfactoria para este tratamiento (RN-40). |
| EN_ATENCION | El proveedor inició el servicio (RN-44). |
| COMPLETADO | El proveedor finalizó el servicio (RN-45). |
| CANCELADO | El tratamiento fue cancelado de forma individual; no puede pasar posteriormente a EN_ATENCION o COMPLETADO (RN-43). |

**Transiciones formales de ReservacionTratamiento (explicadas formalmente):**

| **Origen** | **Destino** | **Condición** |
|----|----|----|
| PENDIENTE | CONFIRMADO | Pago aprobado y disponibilidad revalidada satisfactoriamente para el tratamiento (RN-40). Un pago fallido, cancelado o rechazado no produce esta transición (RN-79). |
| CONFIRMADO | EN_ATENCION | El proveedor inicia el servicio (RN-44). |
| EN_ATENCION | COMPLETADO | El proveedor finaliza el servicio (RN-45). |
| CONFIRMADO | CANCELADO | El tratamiento se cancela de forma individual antes de iniciar la atención (RN-43, RN-61). |
| PENDIENTE | CANCELADO | El tratamiento se cancela o el proceso se abandona antes de confirmarse (por ejemplo, cancelación de la reservación en EN_PROCESO o expiración sin pago aprobado), conforme al diseño final del flujo de cancelación. |

**Coordinación entre ambos niveles:** la confirmación del pago aplica al conjunto de tratamientos vigentes de la reservación. Al aprobarse el pago y revalidarse la disponibilidad de cada tratamiento incluido, Reservacion pasa de EN_PROCESO a CONFIRMADA y cada ReservacionTratamiento válido pasa de PENDIENTE a CONFIRMADO en la misma operación (RN-39, RN-40, RN-75). Si uno de varios tratamientos presenta un conflicto, los demás no se cancelan automáticamente (RN-41).

# 5. Estructura general de entidades

| **\#** | **Entidad** | **Finalidad** |
|----|----|----|
| 1 | Rol | Define los roles y permisos generales reconocidos por TZISCA. |
| 2 | Usuario | Almacena las cuentas de acceso de todos los roles del sistema. |
| 3 | PreferenciaCliente | Guarda las preferencias opcionales utilizadas para personalizar recomendaciones de cabina. |
| 4 | TipoCabina | Clasifica las cabinas físicas por tipo funcional. |
| 5 | Cabina | Representa cada cabina física administrada por el spa. |
| 6 | Tratamiento | Almacena los servicios o tratamientos ofrecidos por el spa. |
| 7 | TratamientoCabina | Resuelve la relación muchos a muchos entre tratamientos y cabinas compatibles. |
| 8 | Proveedor | Extiende la cuenta Usuario cuando el usuario presta tratamientos. |
| 9 | ProveedorTratamiento | Indica qué tratamientos está autorizado a realizar cada proveedor. |
| 10 | Carrito | Representa la selección temporal del cliente antes de confirmar una reservación. |
| 11 | CarritoTratamiento | Almacena cada instancia de tratamiento agregada al carrito y su configuración provisional. |
| 12 | BloqueoTemporal | Protege provisionalmente un intervalo de cabina durante el proceso de carrito. |
| 13 | Reservacion | Encabezado general de la reservación; conserva su propio estado operativo (EN_PROCESO, CONFIRMADA, CANCELADA o EXPIRADA), independiente del estado de cada tratamiento. |
| 14 | ReservacionTratamiento | Tabla transaccional central: representa cada tratamiento independiente contenido en una reservación. |
| 15 | AsignacionProveedor | Registra asignaciones iniciales y sustituciones de proveedores, conservando historial. |
| 16 | BloqueoCabina | Registra bloqueos operativos de una cabina por mantenimiento, limpieza, incidencia, uso interno u otra causa. |
| 17 | HistorialEstadoCabina | Conserva los cambios del estado operativo de cada cabina. |
| 18 | IndisponibilidadProveedor | Conserva periodos en que un proveedor no puede recibir nuevas asignaciones. |
| 19 | HistorialEstadoTratamiento | Registra las transiciones de estado de cada tratamiento reservado. |
| 20 | Cancelacion | Conserva la información propia de cancelaciones individuales o de reservación completa. |
| 21 | Pago | Registra las operaciones económicas relacionadas con una reservación, incluyendo importe, método, estado y referencia. |
| 22 | Devolucion | Registra devoluciones totales o parciales relacionadas con pagos y cancelaciones, clasificadas mediante tipo_devolucion. |
| 23 | TransaccionPago (opcional) | Conserva historial técnico de intentos y respuestas de una pasarela externa cuando exista integración. |

Total confirmado: 22 entidades obligatorias más 1 entidad opcional (TransaccionPago) = **23 entidades**. La sección 7 propone, adicionalmente, entidades estructurales para el modelo operativo de agenda (ParametroOperativo, DiaLaborable, ExcepcionOperativa); no se incorporan todavía a este total porque sus valores están pendientes de aprobación del negocio (ver secciones 7 y 11, y el recálculo detallado en la sección 13).

# 6. Diccionario de datos

Las siguientes tablas constituyen la versión depurada previa al Diagrama Entidad–Relación. En la columna "Llave" se indica PK o la referencia FK. "Único" corresponde a restricciones propuestas para evitar duplicidades lógicas en los campos indicados.

## 6.1. Rol

Define los roles y permisos generales reconocidos por TZISCA.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_rol | INT | PK | No | Sí | IDENTITY | Identificador único del rol. |
| nombre | TEXT | — | No | Sí | — | Nombre del rol: Cliente, Administrador general, Recepción y cabinas o Proveedor de tratamiento. |
| descripcion | TEXT | — | Sí | No | NULL | Descripción funcional del rol. |
| activo | BIT | — | No | No | 1 | Indica si el rol está habilitado. |

**Relaciones:** Rol 1:N Usuario.

## 6.2. Usuario

Almacena las cuentas de acceso de todos los roles del sistema.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_usuario | INT | PK | No | Sí | IDENTITY | Identificador único del usuario. |
| id_rol | INT | FK → Rol.id_rol | No | No | — | Rol actual del usuario. |
| nombre | TEXT | — | No | No | — | Nombre del usuario. |
| correo | TEXT | — | No | Sí | — | Correo de acceso; no debe duplicarse. |
| telefono | TEXT | — | No | No | — | Teléfono de contacto. |
| password_hash | TEXT | — | No | No | — | Representación segura de la contraseña. |
| activo | BIT | — | No | No | 1 | Indica si la cuenta puede iniciar sesión. |
| fecha_registro | DATETIME2 | — | No | No | Fecha/hora actual | Fecha y hora de creación de la cuenta. |
| ultimo_acceso | DATETIME2 | — | Sí | No | NULL | Último acceso registrado. |

**Relaciones:** Rol 1:N Usuario; Usuario 1:0..1 PreferenciaCliente; Usuario 1:0..1 Proveedor; Usuario 1:N Carrito; Usuario 1:N Reservacion (como cliente); Usuario participa como responsable en movimientos operativos.

## 6.3. PreferenciaCliente

Guarda las preferencias opcionales utilizadas para personalizar recomendaciones de cabina.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_preferencia | INT | PK | No | Sí | IDENTITY | Identificador del registro de preferencias. |
| id_usuario | INT | FK → Usuario.id_usuario | No | Sí | — | Cliente propietario de las preferencias. |
| objetivo_visita | TEXT | — | Sí | No | NULL | Objetivo o intención general de la visita. |
| modalidad_preferida | TEXT | — | Sí | No | NULL | Preferencia de modalidad, por ejemplo individual o pareja. |
| privacidad_preferida | TEXT | — | Sí | No | NULL | Preferencia de privacidad. |
| ambiente_preferido | TEXT | — | Sí | No | NULL | Tipo de ambiente preferido. |
| requiere_accesibilidad | BIT | — | Sí | No | NULL | Preferencia relacionada con accesibilidad. |
| observaciones | TEXT | — | Sí | No | NULL | Información adicional de preferencias. |
| fecha_actualizacion | DATETIME2 | — | No | No | Fecha/hora actual | Última actualización de las preferencias. |

**Relaciones:** Usuario 1:0..1 PreferenciaCliente.

**Notas de diseño:** El perfil de preferencias es opcional; su ausencia no impide reservar.

## 6.4. TipoCabina

Clasifica las cabinas físicas por tipo funcional.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_tipo_cabina | INT | PK | No | Sí | IDENTITY | Identificador del tipo de cabina. |
| nombre | TEXT | — | No | Sí | — | Nombre del tipo de cabina. |
| descripcion | TEXT | — | Sí | No | NULL | Descripción general del tipo. |
| activo | BIT | — | No | No | 1 | Indica si el tipo permanece disponible para configuración. |

**Relaciones:** TipoCabina 1:N Cabina.

**Notas de diseño:** Tipos contemplados: masaje, facial, hidroterapia, sauna, integral/multifuncional y sal/haloterapia.

## 6.5. Cabina

Representa cada cabina física administrada por el spa.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_cabina | INT | PK | No | Sí | IDENTITY | Identificador único de la cabina. |
| id_tipo_cabina | INT | FK → TipoCabina.id_tipo_cabina | No | No | — | Tipo al que pertenece la cabina. |
| nombre | TEXT | — | No | Sí | — | Nombre o identificador visible de la cabina. |
| capacidad_maxima | INT | — | No | No | — | Cantidad máxima de personas permitidas. |
| descripcion | TEXT | — | Sí | No | NULL | Descripción general. |
| caracteristicas | TEXT | — | Sí | No | NULL | Características registradas para informar al cliente. |
| equipamiento | TEXT | — | Sí | No | NULL | Equipamiento disponible. |
| beneficios | TEXT | — | Sí | No | NULL | Beneficios informativos asociados al espacio. |
| accesibilidad | TEXT | — | Sí | No | NULL | Información de accesibilidad. |
| imagen_url | TEXT | — | Sí | No | NULL | Referencia o ubicación de imagen. |
| observaciones | TEXT | — | Sí | No | NULL | Notas operativas. |
| estado_operativo | TEXT | — | No | No | Disponible | Estado base: Disponible, En mantenimiento, Fuera de servicio o Desactivada. |

**Relaciones:** TipoCabina 1:N Cabina; Cabina 1:N TratamientoCabina; Cabina 1:N BloqueoTemporal; Cabina 1:N BloqueoCabina; Cabina 1:N HistorialEstadoCabina; Cabina 1:N ReservacionTratamiento.

**Notas de diseño:** Los estados persistentes de la cabina son Disponible, En mantenimiento, Fuera de servicio y Desactivada. "Ocupada" se interpreta como un estado *calculado* para un intervalo específico a partir de ReservacionTratamiento, BloqueoTemporal y BloqueoCabina; por ello no se guarda como estado permanente de catálogo. Esta versión revisó específicamente este punto y confirma que no debe agregarse "Ocupada" como valor persistente de Cabina.estado_operativo: una cabina puede estar libre a una hora y ocupada en otra sin generar contradicciones en el registro base.

## 6.6. Tratamiento

Almacena los servicios o tratamientos ofrecidos por el spa.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_tratamiento | INT | PK | No | Sí | IDENTITY | Identificador del tratamiento. |
| nombre | TEXT | — | No | No | — | Nombre del tratamiento. |
| descripcion | TEXT | — | No | No | — | Descripción del servicio. |
| duracion_minutos | INT | — | No | No | — | Duración programada en minutos. |
| caracteristicas | TEXT | — | Sí | No | NULL | Características generales. |
| beneficios | TEXT | — | Sí | No | NULL | Beneficios informativos. |
| recomendaciones_generales | TEXT | — | Sí | No | NULL | Recomendaciones generales de uso. |
| restricciones | TEXT | — | Sí | No | NULL | Consideraciones o restricciones operativas. |
| imagen_url | TEXT | — | Sí | No | NULL | Imagen de referencia. |
| requiere_proveedor | BIT | — | No | No | 1 | Indica si el servicio requiere proveedor asignado. |
| activo | BIT | — | No | No | 1 | Solo tratamientos activos se ofrecen para nuevas reservaciones. |
| fecha_registro | DATETIME2 | — | No | No | Fecha/hora actual | Fecha de alta en el catálogo. |
| precio_base | DECIMAL(10,2) | — | No | No | — | Precio base vigente del tratamiento para nuevas reservaciones. Debe ser mayor o igual que cero. La fórmula exacta de aplicación (por persona o por tratamiento completo) está pendiente de aprobación conforme a DP-EC-01 (sección 11); este diccionario no la anticipa. |
| moneda | TEXT | — | No | No | MXN | Moneda en la que se expresa el precio base. Para la primera versión se utilizará MXN. |

**Relaciones:** Tratamiento 1:N TratamientoCabina; Tratamiento 1:N ProveedorTratamiento; Tratamiento 1:N CarritoTratamiento; Tratamiento 1:N ReservacionTratamiento.

## 6.7. TratamientoCabina

Resuelve la relación muchos a muchos entre tratamientos y cabinas compatibles.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_tratamiento_cabina | INT | PK | No | Sí | IDENTITY | Identificador de la compatibilidad. |
| id_tratamiento | INT | FK → Tratamiento.id_tratamiento | No | No | — | Tratamiento compatible. |
| id_cabina | INT | FK → Cabina.id_cabina | No | No | — | Cabina compatible. |
| es_especializada | BIT | — | No | No | 0 | Indica si la cabina es especializada para el tratamiento. |
| prioridad | INT | — | Sí | No | NULL | Valor propuesto para ordenar opciones válidas. |
| activo | BIT | — | No | No | 1 | Estado de la relación de compatibilidad. |

**Relaciones:** Tratamiento N:M Cabina mediante TratamientoCabina.

**Notas de diseño:** Restricción propuesta: no repetir la misma combinación id_tratamiento + id_cabina.

## 6.8. Proveedor

Extiende la cuenta Usuario cuando el usuario presta tratamientos.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_proveedor | INT | PK | No | Sí | IDENTITY | Identificador interno del proveedor. |
| id_usuario | INT | FK → Usuario.id_usuario | No | Sí | — | Cuenta asociada al proveedor. |
| descripcion | TEXT | — | Sí | No | NULL | Información adicional del proveedor. |
| observaciones | TEXT | — | Sí | No | NULL | Notas administrativas. |
| activo | BIT | — | No | No | 1 | Indica si puede participar en nuevas asignaciones. |

**Relaciones:** Usuario 1:0..1 Proveedor; Proveedor 1:N ProveedorTratamiento; Proveedor 1:N IndisponibilidadProveedor; Proveedor 1:N AsignacionProveedor.

**Notas de diseño:** Usuario.activo controla si la cuenta puede acceder al sistema; Proveedor.activo controla si el proveedor puede participar en nuevas asignaciones. Ambos estados se mantienen separados porque representan decisiones distintas.

## 6.9. ProveedorTratamiento

Indica qué tratamientos está autorizado a realizar cada proveedor.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_proveedor_tratamiento | INT | PK | No | Sí | IDENTITY | Identificador de la autorización. |
| id_proveedor | INT | FK → Proveedor.id_proveedor | No | No | — | Proveedor. |
| id_tratamiento | INT | FK → Tratamiento.id_tratamiento | No | No | — | Tratamiento que puede realizar. |
| activo | BIT | — | No | No | 1 | Estado de la autorización. |

**Relaciones:** Proveedor N:M Tratamiento mediante ProveedorTratamiento.

**Notas de diseño:** Restricción propuesta: no repetir la misma combinación id_proveedor + id_tratamiento.

## 6.10. Carrito

Representa la selección temporal del cliente antes de confirmar una reservación.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_carrito | BIGINT | PK | No | Sí | IDENTITY | Identificador del carrito. |
| id_cliente | INT | FK → Usuario.id_usuario | No | No | — | Cliente propietario. |
| estado | TEXT | — | No | No | ACTIVO | Estado: ACTIVO, CONFIRMADO o ABANDONADO. |
| fecha_creacion | DATETIME2 | — | No | No | Fecha/hora actual | Fecha y hora de creación. |
| fecha_actualizacion | DATETIME2 | — | No | No | Fecha/hora actual | Última modificación. |

**Relaciones:** Usuario 1:N Carrito; Carrito 1:N CarritoTratamiento.

**Notas de diseño:** Agregar tratamientos al carrito no crea una reservación definitiva. Un cliente podrá conservar carritos históricos, pero no deberá tener más de un carrito en estado ACTIVO al mismo tiempo.

## 6.11. CarritoTratamiento

Almacena cada instancia de tratamiento agregada al carrito y su configuración provisional.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_carrito_tratamiento | BIGINT | PK | No | Sí | IDENTITY | Identificador de la instancia. |
| id_carrito | BIGINT | FK → Carrito.id_carrito | No | No | — | Carrito propietario. |
| id_tratamiento | INT | FK → Tratamiento.id_tratamiento | No | No | — | Tratamiento agregado. |
| id_cabina | INT | FK → Cabina.id_cabina | Sí | No | NULL | Cabina seleccionada provisionalmente. |
| numero_personas | INT | — | Sí | No | NULL | Número de personas para esta instancia. |
| fecha_hora_inicio | DATETIME2 | — | Sí | No | NULL | Inicio provisional seleccionado. |
| fecha_hora_fin | DATETIME2 | — | Sí | No | NULL | Fin calculado según duración. |
| fecha_agregado | DATETIME2 | — | No | No | Fecha/hora actual | Momento en que se agregó al carrito. |

**Relaciones:** Carrito 1:N CarritoTratamiento; Tratamiento 1:N CarritoTratamiento; Cabina 1:N CarritoTratamiento; CarritoTratamiento 1:N BloqueoTemporal.

**Notas de diseño:** El mismo tratamiento puede aparecer varias veces; cada instancia se configura de forma independiente.

## 6.12. BloqueoTemporal

Protege provisionalmente un intervalo de cabina durante el proceso de carrito.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_bloqueo_temporal | BIGINT | PK | No | Sí | IDENTITY | Identificador del bloqueo. |
| id_carrito_tratamiento | BIGINT | FK → CarritoTratamiento.id_carrito_tratamiento | No | No | — | Elemento del carrito relacionado. |
| id_cabina | INT | FK → Cabina.id_cabina | No | No | — | Cabina protegida. |
| fecha_hora_inicio | DATETIME2 | — | No | No | — | Inicio del intervalo. |
| fecha_hora_fin | DATETIME2 | — | No | No | — | Fin del intervalo. |
| fecha_creacion | DATETIME2 | — | No | No | Fecha/hora actual | Creación del bloqueo. |
| fecha_expiracion | DATETIME2 | — | No | No | Calculado: fecha_creacion + ParametroOperativo.duracion_bloqueo_minutos vigente | Vencimiento del bloqueo. No se fija un valor de minutos en el diccionario: se calcula a partir del parámetro operativo de duración de bloqueo vigente (sección 7, DP-OP-08), cuyo valor numérico está pendiente de aprobación. |
| estado | TEXT | — | No | No | ACTIVO | Estado: ACTIVO, CONFIRMADO, EXPIRADO o LIBERADO. |

**Relaciones:** CarritoTratamiento 1:N BloqueoTemporal; Cabina 1:N BloqueoTemporal.

**Notas de diseño:** Solo un bloqueo vigente debe proteger una misma selección; bloqueos anteriores pueden conservarse como registros expirados o liberados. Esta versión elimina la referencia fija a "Creación + 10 minutos" que traía el documento anterior: RN-31 y la nota de consistencia de la sección 17 de Reglas de Negocio Horario y Políticas TZISCA establecen explícitamente que cualquier valor fijo en minutos debe considerarse no aprobado hasta su validación formal (ver sección 7, ParametroOperativo.duracion_bloqueo_minutos).

## 6.13. Reservacion

Encabezado general de una reservación.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_reservacion | BIGINT | PK | No | Sí | IDENTITY | Identificador de la reservación. |
| id_cliente | INT | FK → Usuario.id_usuario | No | No | — | Cliente propietario. |
| creada_por | INT | FK → Usuario.id_usuario | No | No | — | Usuario que generó la reservación; puede ser cliente o recepción. |
| origen | TEXT | — | No | No | — | Origen, por ejemplo WEB o RECEPCION. |
| **estado_reservacion** | TEXT | — | No | No | EN_PROCESO | **Campo nuevo en esta versión.** Estado general de la reservación. Valores conceptuales: EN_PROCESO (creada antes del pago/confirmación), CONFIRMADA (recursos confirmados y condición económica cumplida), CANCELADA (cancelación completa) o EXPIRADA (proceso no confirmado cuyo bloqueo expiró y ya no puede continuar sin nueva validación). Ver ciclo formal completo en la sección 4. |
| fecha_creacion | DATETIME2 | — | No | No | Fecha/hora actual | Fecha y hora de registro. |
| observaciones | TEXT | — | Sí | No | NULL | Observaciones generales. |

**Relaciones: Usuario 1:N Reservacion; Reservacion 1:N ReservacionTratamiento; Reservacion 1:N Cancelacion; Reservacion 1:0..N Pago; Reservacion 1:N Devolucion.**

**Notas de diseño:** No almacena cabina, proveedor, fecha u hora de cada servicio; esos datos pertenecen a ReservacionTratamiento. estado_reservacion es independiente del estado de cada tratamiento (ReservacionTratamiento.estado): los tratamientos siguen manejando sus propios estados independientes (ver sección 4). No se crea en esta versión una entidad HistorialEstadoReservacion; los cambios relevantes de la reservación quedan trazables de forma indirecta mediante Cancelacion, Pago y HistorialEstadoTratamiento. Si el proyecto requiere trazabilidad explícita de estado_reservacion en el futuro, podrá evaluarse una tabla de historial dedicada, sin que esto sea necesario para el modelo físico actual.

## 6.14. ReservacionTratamiento

Tabla transaccional central: representa cada tratamiento independiente contenido en una reservación.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_reservacion_tratamiento | BIGINT | PK | No | Sí | IDENTITY | Identificador del servicio reservado. |
| id_reservacion | BIGINT | FK → Reservacion.id_reservacion | No | No | — | Reservación a la que pertenece. |
| id_tratamiento | INT | FK → Tratamiento.id_tratamiento | No | No | — | Tratamiento reservado. |
| id_cabina | INT | FK → Cabina.id_cabina | No | No | — | Cabina asignada. |
| numero_personas | INT | — | No | No | — | Cantidad de personas. |
| fecha_hora_inicio | DATETIME2 | — | No | No | — | Inicio programado. |
| fecha_hora_fin_programada | DATETIME2 | — | No | No | — | Fin programado. |
| fecha_hora_inicio_real | DATETIME2 | — | Sí | No | NULL | Inicio real de atención. |
| fecha_hora_fin_real | DATETIME2 | — | Sí | No | NULL | Fin real de atención. |
| estado | TEXT | — | No | No | PENDIENTE | Estado actual: PENDIENTE, CONFIRMADO, EN_ATENCION, COMPLETADO o CANCELADO. Cambio en esta versión: el default pasa de CONFIRMADO a PENDIENTE, consistente con RN-38 y RN-42 (todo tratamiento nace PENDIENTE en una reservación EN_PROCESO). Ciclo formal completo en la sección 4. |
| fecha_confirmacion | DATETIME2 | — | Sí | No | NULL | Momento de confirmación. |
| observaciones | TEXT | — | Sí | No | NULL | Notas del servicio reservado. |
| precio_unitario | DECIMAL(10,2) | — | No | No | — | Precio aplicado al tratamiento al momento de generar la reservación. Se conserva como valor histórico. |
| importe | DECIMAL(10,2) | — | No | No | — | Importe correspondiente a esta instancia del tratamiento reservado y base para el cálculo económico. La fórmula exacta (por persona o por tratamiento completo) está pendiente conforme a DP-EC-01 (sección 11). |

**Relaciones: Reservacion 1:N ReservacionTratamiento; Tratamiento 1:N ReservacionTratamiento; Cabina 1:N ReservacionTratamiento; ReservacionTratamiento 1:N AsignacionProveedor; ReservacionTratamiento 1:N HistorialEstadoTratamiento; ReservacionTratamiento 1:N Cancelacion; ReservacionTratamiento 1:0..N Devolucion.**

**Notas de diseño: No contiene id_proveedor; la asignación vigente e histórica se administra mediante AsignacionProveedor. Debe evitar traslapes de cabina durante todo el intervalo. El momento de uso del estado PENDIENTE queda resuelto en esta versión (sección 4): toda instancia nace PENDIENTE al crearse la reservación (Reservacion en EN_PROCESO); pasa a CONFIRMADO únicamente cuando el pago es aprobado y la disponibilidad se revalida. precio_unitario conserva el precio aplicado al momento de reservar y no se recalcula automáticamente si cambia Tratamiento.precio_base. importe conserva el valor económico de la instancia reservada.**

## 6.15. AsignacionProveedor

Registra asignaciones iniciales y sustituciones de proveedores, conservando historial.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_asignacion | BIGINT | PK | No | Sí | IDENTITY | Identificador de la asignación. |
| id_reservacion_tratamiento | BIGINT | FK → ReservacionTratamiento.id_reservacion_tratamiento | No | No | — | Tratamiento reservado. |
| id_asignacion_anterior | BIGINT | FK → AsignacionProveedor.id_asignacion | Sí | No | NULL | Asignación previa sustituida por este registro; enlaza el historial de sustituciones. |
| id_proveedor | INT | FK → Proveedor.id_proveedor | No | No | — | Proveedor asignado. |
| id_usuario_responsable | INT | FK → Usuario.id_usuario | Sí | No | NULL | Usuario que realizó o confirmó el movimiento; puede ser NULL si el proceso fue automático. |
| tipo_asignacion | TEXT | — | No | No | — | Tipo: INICIAL o SUSTITUCION. |
| motivo | TEXT | — | Sí | No | NULL | Motivo de la asignación o sustitución. |
| fecha_asignacion | DATETIME2 | — | No | No | Fecha/hora actual | Inicio de vigencia. |
| fecha_fin_asignacion | DATETIME2 | — | Sí | No | NULL | Fin de vigencia cuando deja de ser la asignación actual. |
| estado | TEXT | — | No | No | ACTUAL | Estado: PROPUESTA, ACTUAL, SUSTITUIDA, RECHAZADA o CANCELADA. |
| aceptado_cliente | BIT | — | Sí | No | NULL | Decisión del cliente ante una sustitución propuesta; NULL mientras no exista decisión. |
| fecha_decision | DATETIME2 | — | Sí | No | NULL | Fecha/hora de la decisión del cliente. |

**Relaciones:** ReservacionTratamiento 1:N AsignacionProveedor; Proveedor 1:N AsignacionProveedor; Usuario 1:N AsignacionProveedor (responsable); AsignacionProveedor 0..1:N AsignacionProveedor mediante id_asignacion_anterior.

**Notas de diseño:** La asignación inicial corresponde al Administrador general. El proveedor asignado debe estar autorizado y disponible durante todo el intervalo. No debe haber dos asignaciones ACTUAL simultáneas para el mismo ReservacionTratamiento. Cuando TZISCA encuentre un sustituto, la nueva asignación puede registrarse primero como PROPUESTA; si el cliente la acepta pasa a ACTUAL y la anterior a SUSTITUIDA; si la rechaza pasa a RECHAZADA.

## 6.16. BloqueoCabina

Registra bloqueos operativos de una cabina por mantenimiento, limpieza, incidencia, uso interno u otra causa.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_bloqueo_cabina | BIGINT | PK | No | Sí | IDENTITY | Identificador del bloqueo operativo. |
| id_cabina | INT | FK → Cabina.id_cabina | No | No | — | Cabina afectada. |
| id_usuario | INT | FK → Usuario.id_usuario | No | No | — | Usuario que creó el bloqueo. |
| fecha_hora_inicio | DATETIME2 | — | No | No | — | Inicio del bloqueo. |
| fecha_hora_fin | DATETIME2 | — | No | No | — | Fin del bloqueo. |
| dia_completo | BIT | — | No | No | 0 | Indica si corresponde al día completo. |
| motivo | TEXT | — | No | No | — | Motivo del bloqueo. |
| observaciones | TEXT | — | Sí | No | NULL | Detalles adicionales. |
| activo | BIT | — | No | No | 1 | Indica si el bloqueo sigue vigente. |
| fecha_registro | DATETIME2 | — | No | No | Fecha/hora actual | Momento de creación. |
| fecha_liberacion | DATETIME2 | — | Sí | No | NULL | Momento en que se retiró el bloqueo. |

**Relaciones:** Cabina 1:N BloqueoCabina; Usuario 1:N BloqueoCabina.

**Notas de diseño:** Si existen reservaciones confirmadas en el intervalo, el sistema debe advertir antes de aplicar el bloqueo.

## 6.17. HistorialEstadoCabina

Conserva los cambios del estado operativo de cada cabina.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_historial_cabina | BIGINT | PK | No | Sí | IDENTITY | Identificador del cambio. |
| id_cabina | INT | FK → Cabina.id_cabina | No | No | — | Cabina afectada. |
| id_usuario | INT | FK → Usuario.id_usuario | No | No | — | Usuario que realizó el cambio. |
| estado_anterior | TEXT | — | No | No | — | Estado previo. |
| estado_nuevo | TEXT | — | No | No | — | Estado posterior. |
| fecha_cambio | DATETIME2 | — | No | No | Fecha/hora actual | Momento del cambio. |
| observacion | TEXT | — | Sí | No | NULL | Motivo u observación. |

**Relaciones:** Cabina 1:N HistorialEstadoCabina; Usuario 1:N HistorialEstadoCabina.

## 6.18. IndisponibilidadProveedor

Conserva periodos en que un proveedor no puede recibir nuevas asignaciones.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_indisponibilidad | BIGINT | PK | No | Sí | IDENTITY | Identificador de la indisponibilidad. |
| id_proveedor | INT | FK → Proveedor.id_proveedor | No | No | — | Proveedor afectado. |
| fecha_hora_inicio | DATETIME2 | — | No | No | — | Inicio de la indisponibilidad. |
| fecha_hora_fin | DATETIME2 | — | No | No | — | Fin de la indisponibilidad. |
| motivo | TEXT | — | Sí | No | NULL | Motivo registrado. |
| observaciones | TEXT | — | Sí | No | NULL | Información adicional. |
| fecha_registro | DATETIME2 | — | No | No | Fecha/hora actual | Fecha de registro. |
| activo | BIT | — | No | No | 1 | Estado administrativo del registro. |

**Relaciones:** Proveedor 1:N IndisponibilidadProveedor.

**Notas de diseño:** La indisponibilidad histórica se conserva aunque el periodo ya haya concluido. Si afecta tratamientos asignados, activa el proceso de búsqueda de sustituto.

## 6.19. HistorialEstadoTratamiento

Registra las transiciones de estado de cada tratamiento reservado.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_historial_estado | BIGINT | PK | No | Sí | IDENTITY | Identificador del movimiento. |
| id_reservacion_tratamiento | BIGINT | FK → ReservacionTratamiento.id_reservacion_tratamiento | No | No | — | Servicio reservado afectado. |
| id_usuario | INT | FK → Usuario.id_usuario | Sí | No | NULL | Usuario que realizó el cambio; puede ser NULL en procesos automáticos. |
| estado_anterior | TEXT | — | Sí | No | NULL | Estado previo. |
| estado_nuevo | TEXT | — | No | No | — | Nuevo estado. |
| fecha_cambio | DATETIME2 | — | No | No | Fecha/hora actual | Momento de la transición. |
| motivo | TEXT | — | Sí | No | NULL | Motivo cuando corresponda. |
| observaciones | TEXT | — | Sí | No | NULL | Información adicional. |

**Relaciones:** ReservacionTratamiento 1:N HistorialEstadoTratamiento; Usuario 1:N HistorialEstadoTratamiento.

**Notas de diseño: Transiciones formales registradas: PENDIENTE→CONFIRMADO (pago aprobado y disponibilidad revalidada), CONFIRMADO→EN_ATENCION, EN_ATENCION→COMPLETADO, CONFIRMADO→CANCELADO y PENDIENTE→CANCELADO (proceso abandonado o cancelado antes de confirmarse). Esta versión resuelve la ambigüedad que traía el documento anterior sobre el uso de PENDIENTE; el ciclo completo se documenta en la sección 4.**

## 6.20. Cancelacion

Conserva la información propia de cancelaciones individuales o de reservación completa.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_cancelacion | BIGINT | PK | No | Sí | IDENTITY | Identificador de la cancelación. |
| id_reservacion | BIGINT | FK → Reservacion.id_reservacion | No | No | — | Reservación afectada. |
| id_reservacion_tratamiento | BIGINT | FK → ReservacionTratamiento.id_reservacion_tratamiento | Sí | No | NULL | Tratamiento específico si la cancelación es individual. |
| id_usuario | INT | FK → Usuario.id_usuario | No | No | — | Usuario que realizó la cancelación. |
| tipo_cancelacion | TEXT | — | No | No | — | TRATAMIENTO o RESERVACION_COMPLETA. |
| motivo | TEXT | — | Sí | No | NULL | Motivo registrado. |
| fecha_cancelacion | DATETIME2 | — | No | No | Fecha/hora actual | Momento de la cancelación. |
| observaciones | TEXT | — | Sí | No | NULL | Información adicional. |

**Relaciones:** Reservacion 1:N Cancelacion; ReservacionTratamiento 1:N Cancelacion; Usuario 1:N Cancelacion.

**Notas de diseño:** La cancelación modifica estados y libera recursos, pero no elimina físicamente la reservación ni su historial. Cuando tipo_cancelacion = TRATAMIENTO, id_reservacion_tratamiento debe ser obligatorio y pertenecer a la misma id_reservacion registrada. Cuando tipo_cancelacion = RESERVACION_COMPLETA, id_reservacion_tratamiento debe permanecer NULL.

## 6.21. Pago

Representa las operaciones de pago relacionadas con una reservación y permite controlar el importe, método, estado y trazabilidad de cada operación.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_pago | BIGINT | PK | No | Sí | IDENTITY | Identificador único del pago. |
| id_reservacion | BIGINT | FK → Reservacion.id_reservacion | No | No | — | Reservación relacionada con el pago. |
| monto | DECIMAL(10,2) | — | No | No | — | Importe asociado a la operación de pago. Debe ser mayor que cero. |
| moneda | TEXT | — | No | No | MXN | Moneda utilizada para el pago. |
| metodo_pago | TEXT | — | No | No | — | Método utilizado para realizar el pago. |
| estado_pago | TEXT | — | No | No | PENDIENTE | Estado actual: PENDIENTE, PROCESANDO, PAGADO, FALLIDO, CANCELADO, REEMBOLSADO o REEMBOLSADO_PARCIALMENTE. Catálogo revisado en esta versión (punto 10 de la corrección) y confirmado sin cambios: es el único catálogo técnico de estados de Pago en todo el modelo (RN-77). |
| referencia | TEXT | — | Sí | No | NULL | Referencia interna o externa relacionada con la operación. |
| fecha_creacion | DATETIME2 | — | No | No | Fecha/hora actual | Momento en que se generó el registro del pago. |
| fecha_pago | DATETIME2 | — | Sí | No | NULL | Fecha y hora en que el pago fue aprobado. |
| fecha_actualizacion | DATETIME2 | — | No | No | Fecha/hora actual | Última modificación del estado o información del pago. |

**Relaciones: Reservacion 1:0..N Pago; Pago 1:0..N Devolucion; Pago 1:0..N TransaccionPago cuando se implemente integración externa.**

**Notas de diseño: Una reservación puede generar más de un registro de pago cuando existan reintentos o nuevas operaciones. Un pago solo se considera completado cuando estado_pago = PAGADO. Un pago fallido no confirma económicamente la reservación (RN-79). Un rechazo reportado por una pasarela de pago externa se interpreta funcionalmente como estado_pago = FALLIDO; no se agrega RECHAZADO como valor adicional de este catálogo. TZISCA no almacenará números completos de tarjeta, CVV, contraseñas bancarias ni otros datos financieros sensibles. Pago permanece como entidad separada de Reservacion en esta versión, sin cambios.**

## 6.22. Devolucion

Permite registrar devoluciones totales o parciales relacionadas con cancelaciones de reservaciones o tratamientos individuales.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_devolucion | BIGINT | PK | No | Sí | IDENTITY | Identificador único de la devolución. |
| id_pago | BIGINT | FK → Pago.id_pago | No | No | — | Pago original sobre el que se realiza la devolución. |
| id_reservacion | BIGINT | FK → Reservacion.id_reservacion | No | No | — | Reservación relacionada. |
| id_reservacion_tratamiento | BIGINT | FK → ReservacionTratamiento.id_reservacion_tratamiento | Sí | No | NULL | Tratamiento específico cuando la devolución sea parcial. |
| **tipo_devolucion** | TEXT | — | No | No | — | **Campo nuevo en esta versión.** Valores: PARCIAL o TOTAL. Determina el alcance de la devolución: TOTAL implica id_reservacion_tratamiento en NULL (cancelación completa de la reservación); PARCIAL exige id_reservacion_tratamiento válido, perteneciente a la misma reservación. No se usa como valor de estado_devolucion. |
| monto | DECIMAL(10,2) | — | No | No | — | Importe a devolver. Debe ser mayor que cero. |
| motivo | TEXT | — | Sí | No | NULL | Motivo que originó la devolución. |
| estado_devolucion | TEXT | — | No | No | PENDIENTE | Estado: PENDIENTE, PROCESANDO, COMPLETADA, FALLIDA o CANCELADA. Catálogo técnico único revisado en esta versión (punto 11 de la corrección) y confirmado sin cambios; no se utilizan "PARCIAL" ni "TOTAL" como estado, ya que corresponden al tipo de devolución (tipo_devolucion). |
| fecha_solicitud | DATETIME2 | — | No | No | Fecha/hora actual | Momento en que se solicitó o registró la devolución. |
| fecha_procesamiento | DATETIME2 | — | Sí | No | NULL | Momento en que la devolución fue procesada. |
| id_usuario_responsable | INT | FK → Usuario.id_usuario | Sí | No | NULL | Usuario responsable; puede ser NULL cuando el proceso sea automático. |

**Relaciones:** Pago 1:0..N Devolucion (no todo pago genera devolución); Reservacion 1:N Devolucion; ReservacionTratamiento 1:N Devolucion de forma opcional; Usuario 1:N Devolucion como responsable.

**Notas de diseño:** tipo_devolucion determina el alcance (TOTAL/PARCIAL) y estado_devolucion determina el avance técnico del trámite (PENDIENTE/PROCESANDO/COMPLETADA/FALLIDA/CANCELADA); ambos catálogos son independientes entre sí. En una devolución TOTAL, id_reservacion_tratamiento permanece NULL. En una devolución PARCIAL debe identificar un tratamiento perteneciente a la misma reservación. La suma de devoluciones COMPLETADAS no debe superar el monto efectivamente pagado. Devolucion permanece como entidad separada de Pago en esta versión, sin cambios.

## 6.23. TransaccionPago (opcional)

Registra el historial técnico de las operaciones realizadas contra un proveedor o pasarela externa de pagos. Solo será necesaria cuando exista una integración que requiera conservar intentos y respuestas técnicas.

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_transaccion | BIGINT | PK | No | Sí | IDENTITY | Identificador interno de la transacción. |
| id_pago | BIGINT | FK → Pago.id_pago | No | No | — | Pago al que pertenece la transacción. |
| referencia_externa | TEXT | — | Sí | No | NULL | Identificador proporcionado por el proveedor externo. |
| proveedor_pago | TEXT | — | No | No | — | Proveedor o pasarela utilizada. |
| estado | TEXT | — | No | No | — | Resultado o estado técnico de la transacción. |
| codigo_respuesta | TEXT | — | Sí | No | NULL | Código técnico devuelto por la pasarela. |
| fecha | DATETIME2 | — | No | No | Fecha/hora actual | Momento en que ocurrió la transacción. |

**Relaciones: Pago 1:0..N TransaccionPago (únicamente si la entidad opcional se implementa).**

**Notas de diseño:** No se almacenarán números completos de tarjetas, CVV, contraseñas bancarias ni otra información sensible. TransaccionPago conserva únicamente referencias, estados y respuestas técnicas necesarias para trazabilidad. Se mantiene como entidad opcional en esta versión: solo se incorpora al esquema físico si el proyecto integra una pasarela externa.

# 7. Modelo operativo pendiente (parámetros de agenda)

Reglas de Negocio Horario y Políticas TZISCA (sección 17, DP-OP-01 a DP-OP-08) deja pendientes de aprobación los parámetros operativos de agenda del spa. Este documento no inventa esos valores. Se proponen a continuación las estructuras necesarias para SQL Server de modo que, una vez aprobados los valores, puedan incorporarse directamente al esquema físico sin requerir un nuevo rediseño. **Todos los valores marcados como PENDIENTE DE APROBACIÓN quedan fuera de esta versión del diccionario** y no deben asumirse por defecto ni codificarse como constantes (ver sección 17.4 de Reglas de Negocio Horario y Políticas TZISCA).

Estas tres entidades son una **propuesta estructural**: permiten construir el esquema físico correspondiente, pero no se incorporan todavía al conteo confirmado de 23 entidades (sección 5) porque sus valores operativos dependen de aprobación del negocio. No tienen llaves foráneas obligatorias hacia las entidades transaccionales; alimentan la validación de disponibilidad (BloqueoTemporal, ReservacionTratamiento) mediante lógica de servicio (AvailabilityService), no mediante integridad referencial física.

## 7.1. ParametroOperativo (propuesta — valores pendientes de aprobación)

Parámetros generales vigentes de operación del spa (horario general, intervalo de agenda, anticipación y duración de bloqueo).

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_parametro_operativo | INT | PK | No | Sí | IDENTITY | Identificador de la configuración. |
| hora_apertura_general | TIME | — | No | No | PENDIENTE DE APROBACIÓN | Hora oficial de apertura (DP-OP-01). Valor no definido en este documento. |
| hora_cierre_general | TIME | — | No | No | PENDIENTE DE APROBACIÓN | Hora límite de operación (DP-OP-02), incluyendo si un tratamiento debe finalizar antes o exactamente al cierre. |
| intervalo_agenda_minutos | INT | — | No | No | PENDIENTE DE APROBACIÓN | Granularidad con la que se generan los horarios disponibles para iniciar un tratamiento (DP-OP-05). |
| anticipacion_minima_minutos | INT | — | No | No | PENDIENTE DE APROBACIÓN | Tiempo mínimo previo al inicio del servicio para poder reservarlo (DP-OP-06). |
| anticipacion_maxima_dias | INT | — | No | No | PENDIENTE DE APROBACIÓN | Fecha futura máxima hasta la que un cliente puede reservar (DP-OP-07). |
| duracion_bloqueo_minutos | INT | — | No | No | PENDIENTE DE APROBACIÓN | Vigencia exacta del bloqueo temporal (DP-OP-08). Sustituye el valor fijo de 10 minutos eliminado de BloqueoTemporal en esta versión; alimenta BloqueoTemporal.fecha_expiracion (sección 6.12). |
| fecha_vigencia_desde | DATETIME2 | — | No | No | Fecha/hora actual | Inicio de vigencia de esta configuración. |
| activo | BIT | — | No | No | 1 | Indica si es la configuración vigente. Solo debe existir una fila activo = 1 a la vez. |

## 7.2. DiaLaborable (propuesta — valores pendientes de aprobación)

Días de la semana en que el spa opera y su horario particular cuando difiere del horario general (DP-OP-03).

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_dia_laborable | INT | PK | No | Sí | IDENTITY | Identificador del registro. |
| dia_semana | INT | — | No | Sí | — | Día de la semana (1 = lunes … 7 = domingo). |
| hora_apertura | TIME | — | Sí | No | NULL | Hora de apertura específica de ese día; si es NULL aplica ParametroOperativo.hora_apertura_general. |
| hora_cierre | TIME | — | Sí | No | NULL | Hora de cierre específica de ese día; si es NULL aplica ParametroOperativo.hora_cierre_general. |
| activo | BIT | — | No | No | PENDIENTE DE APROBACIÓN | Indica si el spa opera ese día de la semana (DP-OP-03). Qué días quedan activos no se define en este documento. |

## 7.3. ExcepcionOperativa (propuesta — valores pendientes de aprobación)

Fechas específicas no laborables o con horario especial, como excepción al calendario regular (DP-OP-04).

| **Campo** | **Tipo** | **Llave** | **Nulo** | **Único** | **Default** | **Descripción / restricción** |
|----|----|----|----|----|----|----|
| id_excepcion | INT | PK | No | Sí | IDENTITY | Identificador de la excepción. |
| fecha | DATE | — | No | Sí | — | Fecha específica de la excepción. |
| tipo_excepcion | TEXT | — | No | No | — | Valores: NO_LABORABLE o HORARIO_ESPECIAL. |
| hora_apertura_especial | TIME | — | Sí | No | NULL | Solo aplica si tipo_excepcion = HORARIO_ESPECIAL. |
| hora_cierre_especial | TIME | — | Sí | No | NULL | Solo aplica si tipo_excepcion = HORARIO_ESPECIAL. |
| motivo | TEXT | — | Sí | No | NULL | Motivo de la excepción (feriado, cierre general, mantenimiento, etc.). Mecanismo y catálogo de causas pendientes de aprobación (DP-OP-04). |
| fecha_registro | DATETIME2 | — | No | No | Fecha/hora actual | Momento de registro de la excepción. |

# 8. Relaciones y cardinalidades previstas

| **Entidad A** | **Entidad B** | **Cardinalidad** | **Justificación** |
|----|----|----|----|
| Rol | Usuario | 1:N | Un rol puede estar asignado a muchos usuarios; cada usuario tiene un rol actual. |
| Usuario | PreferenciaCliente | 1:0..1 | El cliente puede no tener preferencias o tener un único perfil de preferencias. |
| Usuario | Proveedor | 1:0..1 | Solo usuarios con función de proveedor requieren esta extensión. |
| TipoCabina | Cabina | 1:N | Un tipo clasifica muchas cabinas físicas. |
| Tratamiento | Cabina | N:M | Se resuelve mediante TratamientoCabina. |
| Proveedor | Tratamiento | N:M | Se resuelve mediante ProveedorTratamiento. |
| Usuario | Carrito | 1:N | Un cliente puede generar varios carritos a lo largo del tiempo. |
| Carrito | CarritoTratamiento | 1:N | Un carrito puede contener varias instancias de tratamientos. |
| CarritoTratamiento | BloqueoTemporal | 1:N | Una instancia puede generar varios bloqueos históricos, aunque solo uno debe permanecer vigente a la vez. |
| Usuario | Reservacion | 1:N | Un cliente puede tener muchas reservaciones. |
| Reservacion | ReservacionTratamiento | 1:N | Cada reservación contiene uno o varios servicios independientes. |
| Cabina | ReservacionTratamiento | 1:N | Una cabina participa en múltiples servicios en diferentes intervalos. |
| ReservacionTratamiento | AsignacionProveedor | 1:N | Permite conservar asignación inicial y sustituciones. |
| Proveedor | AsignacionProveedor | 1:N | Un proveedor puede atender muchas asignaciones en distintos intervalos. |
| Cabina | BloqueoCabina | 1:N | Una cabina puede tener muchos bloqueos operativos históricos. |
| Cabina | HistorialEstadoCabina | 1:N | Se conservan todos los cambios de estado operativo. |
| Proveedor | IndisponibilidadProveedor | 1:N | Un proveedor puede registrar múltiples periodos de indisponibilidad. |
| ReservacionTratamiento | HistorialEstadoTratamiento | 1:N | Cada servicio conserva sus transiciones de estado. |
| Reservacion | Cancelacion | 1:N | Una reservación puede tener movimientos de cancelación. |
| ReservacionTratamiento | Cancelacion | 1:N opcional | Las cancelaciones individuales se relacionan con el tratamiento específico. |
| AsignacionProveedor | AsignacionProveedor | 0..1:N | Una asignación de sustitución puede enlazar la asignación anterior para conservar trazabilidad explícita del cambio. |
| Reservacion | Pago | 1:0..N | Una reservación puede generar diferentes operaciones de pago o reintentos. |
| Pago | Devolucion | **1:0..N** | Un pago puede generar ninguna, una o varias devoluciones parciales o totales; no todo pago debe tener devolución. **Corrección de esta versión:** la cardinalidad se ajustó de 1:N a 1:0..N para reflejar explícitamente que la devolución es opcional. |
| Reservacion | Devolucion | 1:N | Las devoluciones permanecen relacionadas con la reservación que las originó. |
| ReservacionTratamiento | Devolucion | 1:0..N | Una devolución parcial puede corresponder a un tratamiento reservado específico. |
| Usuario | Devolucion | 1:N opcional | Permite identificar al usuario responsable de procesar una devolución. |
| Pago | TransaccionPago | 1:0..N | Un pago puede generar varios intentos o respuestas técnicas de una pasarela externa. |

# 9. Reglas estructurales que debe respetar el modelo

- Una cabina solo puede seleccionarse para tratamientos con los que tenga una relación activa de compatibilidad.

- La capacidad máxima de la cabina debe ser igual o superior al número de personas del tratamiento reservado.

- La disponibilidad se valida durante todo el intervalo fecha_hora_inicio–fecha_hora_fin_programada.

- No deben existir traslapes de uso de una misma cabina entre tratamientos confirmados, bloqueos temporales vigentes y bloqueos operativos.

- **Los bloqueos temporales tienen una duración determinada por el parámetro operativo vigente (ParametroOperativo.duracion_bloqueo_minutos, sección 7.1; valor pendiente de aprobación conforme a DP-OP-08)** y deben liberarse al expirar, cambiar la selección o eliminar el tratamiento del carrito. Esta versión elimina la referencia fija a 10 minutos que traía el documento anterior.

- Un proveedor solo puede asignarse si está autorizado para realizar el tratamiento y no tiene otra atención o indisponibilidad que se traslape.

- La asignación inicial del proveedor corresponde al Administrador general; una sustitución debe conservar la asignación anterior, registrar la propuesta y considerar la decisión del cliente antes de quedar como asignación ACTUAL cuando aplique.

- **Reservacion.estado_reservacion debe aceptar únicamente EN_PROCESO, CONFIRMADA, CANCELADA o EXPIRADA, con valor por default EN_PROCESO.**

- Los estados de ReservacionTratamiento son PENDIENTE, CONFIRMADO, EN_ATENCION, COMPLETADO y CANCELADO, con valor por default PENDIENTE; su transición a CONFIRMADO depende de la aprobación del pago y de la revalidación de disponibilidad, no de la sola creación de la reservación (ver sección 4).

- Una cabina En mantenimiento, Fuera de servicio o Desactivada no se ofrece para nuevas reservaciones.

- Cancelar un tratamiento libera sus recursos pero no elimina físicamente el registro ni los demás tratamientos de la reservación.

- Una reservación completa puede cancelarse sin borrar su historial; los tratamientos completados conservan su estado histórico.

- Los reportes básicos se generan a partir de los datos existentes y no requieren una entidad Reporte en esta versión.

- Tratamiento.duracion_minutos, Cabina.capacidad_maxima y los campos numero_personas deben ser mayores que cero.

- En BloqueoTemporal, BloqueoCabina e IndisponibilidadProveedor, la fecha/hora final debe ser posterior a la fecha/hora inicial; en ReservacionTratamiento, fecha_hora_fin_programada debe ser posterior a fecha_hora_inicio.

- TratamientoCabina no debe repetir la combinación id_tratamiento + id_cabina; ProveedorTratamiento no debe repetir la combinación id_proveedor + id_tratamiento.

- Un cliente puede conservar varios carritos históricos, pero solo uno puede permanecer en estado ACTIVO simultáneamente.

- Los campos de estado, origen y tipo deben aceptar únicamente los valores definidos en este diccionario y en las reglas de negocio correspondientes.

- No debe existir más de una AsignacionProveedor en estado ACTUAL para el mismo ReservacionTratamiento.

- En una cancelación individual, el tratamiento indicado debe pertenecer a la reservación registrada; en una cancelación completa no se debe indicar un tratamiento específico.

- Todo Pago deberá pertenecer a una Reservacion existente.

- Pago.monto y Devolucion.monto deberán ser mayores que cero.

- Todo importe económico deberá manejarse mediante DECIMAL y no mediante TEXT o tipos enteros.

- Los estados de Pago y Devolucion deberán aceptar únicamente los valores definidos en este diccionario y las reglas de negocio.

- Un pago en estado FALLIDO no deberá considerarse aprobado ni confirmar definitivamente una reservación que requiera pago.

- Los reintentos de pago deberán conservar trazabilidad y no sobrescribir operaciones anteriores.

- Una Devolucion deberá estar relacionada con un Pago existente, pero no todo Pago debe generar una Devolucion.

- La suma de devoluciones COMPLETADAS asociadas a un pago no podrá superar el monto efectivamente pagado.

- **Devolucion.tipo_devolucion debe ser PARCIAL o TOTAL; TOTAL implica id_reservacion_tratamiento en NULL y PARCIAL exige id_reservacion_tratamiento válido, perteneciente a la misma Reservacion.**

- Los registros de Pago y Devolucion no deberán eliminarse físicamente después de procesarse.

- La modificación posterior de Tratamiento.precio_base no deberá alterar ReservacionTratamiento.precio_unitario ni su importe histórico.

- TZISCA no deberá almacenar información bancaria sensible completa.

- Si se utiliza una pasarela externa, las referencias y respuestas técnicas podrán conservarse mediante TransaccionPago.

- Los parámetros operativos de agenda (ParametroOperativo, DiaLaborable, ExcepcionOperativa, sección 7) no deberán poblarse con valores supuestos; AvailabilityService no deberá calcular disponibilidad hasta que existan valores aprobados y configurados explícitamente (Reglas de Negocio Horario y Políticas TZISCA, sección 17.4).

# 10. Decisiones de depuración y normalización

## 10.1 Reservacion y ReservacionTratamiento

Se separó el encabezado de la reservación de los datos específicos de cada tratamiento. Esta decisión evita repetir información general del cliente y permite que una misma reservación contenga servicios con cabinas, horarios, proveedores y estados diferentes.

## 10.2 Rol y Usuario

Los roles no se modelan como tablas de persona independientes. La autenticación y datos comunes permanecen en Usuario, mientras Rol determina permisos.

## 10.3 Cabina y ocupación

La ocupación no se guarda como un estado permanente. Se calcula para el intervalo consultado a partir de ReservacionTratamiento, BloqueoTemporal y BloqueoCabina. Por ello, "Ocupada" puede mostrarse en la interfaz como resultado de disponibilidad, pero no forma parte de los valores persistentes de Cabina.estado_operativo. De esta forma una cabina puede estar libre a una hora y ocupada en otra sin generar contradicciones. Este punto se revisó explícitamente en esta versión y se confirma sin cambios.

## 10.4 AsignacionProveedor

Se eliminó id_proveedor de ReservacionTratamiento y se trasladó la relación a AsignacionProveedor. Esto permite conocer la asignación actual, registrar propuestas de sustitución y conservar sustituciones sin duplicar datos. La referencia opcional id_asignacion_anterior permite enlazar explícitamente una sustitución con la asignación que reemplaza.

## 10.5 Carrito y bloqueos

El carrito se mantiene separado porque es un estado previo a la reservación definitiva. Los bloqueos temporales protegen recursos durante el proceso, pero no sustituyen una reservación confirmada.

## 10.6 Historiales específicos

Los historiales de cabina, tratamiento y asignaciones se conservan como entidades separadas para registrar quién realizó el cambio, cuándo ocurrió y cuál fue el movimiento.

## 10.7 Reportes y bitácora

No se crea tabla Reporte porque los indicadores se derivan de la información transaccional. La bitácora general tampoco se incluye en esta versión porque las acciones funcionales relevantes ya tienen entidades de trazabilidad específicas.

## 10.8 Pagos y reservaciones

La información de pago se separa de Reservacion para evitar mezclar el estado operativo con el estado financiero. Una reservación puede generar distintos intentos de pago, por lo que Pago conserva cada operación y su estado correspondiente. Esta versión confirma que Pago se mantiene separado de Reservacion: la incorporación de Reservacion.estado_reservacion no fusiona ambos conceptos, ya que el estado operativo de la reservación y el estado financiero de cada pago siguen siendo independientes y se coordinan únicamente mediante las transiciones descritas en la sección 4.

## 10.9 Devoluciones

Las devoluciones se modelan mediante una entidad independiente para conservar la trazabilidad de cancelaciones con impacto económico, y se mantienen separadas de Pago en esta versión. Se incorpora el campo tipo_devolucion (PARCIAL/TOTAL) para dejar explícito el alcance de cada devolución sin sobrecargar estado_devolucion, cuyo catálogo técnico se confirma como PENDIENTE, PROCESANDO, COMPLETADA, FALLIDA y CANCELADA (sin usar PARCIAL ni TOTAL como estado). Se corrige además la cardinalidad Pago→Devolucion de 1:N a 1:0..N, dejando explícito que no todo pago genera una devolución.

## 10.10 Precios históricos

El precio actual pertenece al catálogo Tratamiento, mientras que el precio efectivamente aplicado se conserva en ReservacionTratamiento. Esta separación evita que una modificación futura en los precios del catálogo altere la información histórica de reservaciones previamente realizadas. La fórmula exacta con la que se calcula el importe (por persona o por tratamiento completo) permanece como decisión pendiente DP-EC-01 (sección 11); este diccionario no la anticipa.

## 10.11 Transacciones externas

TransaccionPago se incorporará únicamente cuando TZISCA utilice una pasarela externa que requiera conservar diferentes intentos, referencias o respuestas técnicas. Pago representa el estado financiero dentro de TZISCA y TransaccionPago representa la interacción técnica con el proveedor externo. Se mantiene como entidad opcional en esta versión, sin cambios.

## 10.12 Resolución del uso del estado Pendiente

La versión anterior de este diccionario dejaba como decisión pendiente "el momento exacto de uso del estado Pendiente en ReservacionTratamiento". Esta versión lo resuelve adoptando el ciclo formal descrito en la sección 4: toda Reservacion nace en EN_PROCESO con sus ReservacionTratamiento en PENDIENTE; cuando el pago es aprobado y la disponibilidad se revalida satisfactoriamente, Reservacion pasa a CONFIRMADA y cada ReservacionTratamiento válido pasa a CONFIRMADO. Esta resolución es consistente con RN-38 a RN-45 y RN-73 a RN-81 de Reglas de Negocio Horario y Políticas TZISCA (documento canónico) y no requiere validación adicional antes de la implementación física.

## 10.13 Duración del bloqueo temporal

La versión anterior fijaba la duración del bloqueo temporal en "Creación + 10 minutos", tanto en BloqueoTemporal.fecha_expiracion como en las reglas estructurales. Esta versión elimina toda referencia fija en minutos y traslada la duración a un parámetro operativo configurable (ParametroOperativo.duracion_bloqueo_minutos, sección 7.1), consistente con RN-31 y con la nota de consistencia de la sección 17 de Reglas de Negocio Horario y Políticas TZISCA, que establece que cualquier valor fijo en minutos debe considerarse no aprobado hasta su validación formal (DP-OP-08).

# 11. Decisiones pendientes antes del modelo físico

Se elimina de esta lista el pendiente sobre el momento de uso del estado Pendiente en ReservacionTratamiento, dado que su ciclo completo queda resuelto en la sección 4 de este documento (ver también 10.12). Quedan vigentes las siguientes decisiones, tomadas directamente del catálogo de decisiones pendientes de Reglas de Negocio Horario y Políticas TZISCA (documento canónico) para no duplicar identificadores:

| **Código** | **Decisión** | **Estado** | **Impacto en este diccionario** |
|----|----|----|----|
| **DP-EC-01** | Fórmula del importe de tratamiento. No se determina todavía si Tratamiento.precio_base corresponde al tratamiento completo o es por persona, si importe = precio_unitario × numero_personas, ni si existen cargos adicionales. | Pendiente de aprobación | Afecta Tratamiento.precio_base, ReservacionTratamiento.precio_unitario/importe y la validación de Pago.monto (secciones 6.6, 6.14, 6.21). |
| DP-EC-02 | Tratamiento económico cuando el pago fue aprobado, el bloqueo temporal expiró y la revalidación confirma pérdida de disponibilidad. No se determina si corresponde conciliación, devolución, cancelación u otro mecanismo aprobado. | Pendiente de aprobación | Afecta la transición Reservacion.estado_reservacion → EXPIRADA cuando ya existe un Pago aprobado, y el tratamiento de los ReservacionTratamiento en PENDIENTE asociados (sección 4.1). |
| DP-OP-01 a DP-OP-08 | Hora de apertura, hora de cierre, días laborales, días no laborales/excepciones, duración de intervalos de agenda, anticipación mínima, anticipación máxima y duración del bloqueo temporal. | Pendiente de aprobación | Estructuras propuestas en la sección 7 (ParametroOperativo, DiaLaborable, ExcepcionOperativa); valores no definidos en este diccionario. |

DP-OP-09 a DP-OP-13 (tolerancia y políticas de cancelación/devolución) y DP-TEC-01 a DP-TEC-03 (autenticación, pasarela de pago, algoritmo de recomendación) permanecen igualmente pendientes en Reglas de Negocio Horario y Políticas TZISCA, pero no tienen impacto directo en la estructura del diccionario de datos y no se repiten aquí.

# 12. Modelo lógico resumido previo al ER

- Núcleo de usuarios: Rol → Usuario → PreferenciaCliente / Proveedor.

- Núcleo de catálogo: TipoCabina → Cabina; Tratamiento ↔ Cabina mediante TratamientoCabina; Proveedor ↔ Tratamiento mediante ProveedorTratamiento.

- Núcleo de carrito: Usuario → Carrito → CarritoTratamiento → BloqueoTemporal.

- Núcleo de reservación: Usuario → Reservacion (estado_reservacion) → ReservacionTratamiento (estado) → AsignacionProveedor / HistorialEstadoTratamiento / Cancelacion.

- Núcleo operativo: Cabina → BloqueoCabina / HistorialEstadoCabina; Proveedor → IndisponibilidadProveedor.

- Núcleo financiero: Reservacion → Pago → Devolucion (tipo_devolucion); ReservacionTratamiento → Devolucion para devoluciones parciales; Pago → TransaccionPago cuando exista integración con una pasarela externa.

- Núcleo operativo pendiente (propuesta, sección 7): ParametroOperativo / DiaLaborable / ExcepcionOperativa — estructura definida, valores pendientes de aprobación (DP-OP-01 a DP-OP-08).

# 13. Resultado de esta etapa

El modelo corregido y confirmado queda compuesto por 22 entidades obligatorias más 1 entidad opcional (TransaccionPago, aplicable únicamente si se integra una pasarela de pago externa), para un total de **23 entidades**. Este número no cambia respecto a la versión anterior porque las correcciones de esta etapa se resolvieron mediante campos nuevos en entidades ya existentes (Reservacion.estado_reservacion, Devolucion.tipo_devolucion) y no mediante entidades adicionales obligatorias.

Respecto a la versión anterior del diccionario, esta corrección: (1) actualizó la referencia de casos de uso a CU-01 a CU-43; (2) resolvió el uso ambiguo del estado PENDIENTE mediante el ciclo formal de dos niveles Reservacion.estado_reservacion / ReservacionTratamiento.estado (sección 4); (3) incorporó Reservacion.estado_reservacion; (4) cambió el default de ReservacionTratamiento.estado a PENDIENTE y normalizó su catálogo a PENDIENTE, CONFIRMADO, EN_ATENCION, COMPLETADO y CANCELADO, incluyendo la transición PENDIENTE→CANCELADO; (6)-(7) eliminó la duración fija de 10 minutos de BloqueoTemporal y de las reglas estructurales, trasladándola a un parámetro operativo configurable; (10)-(11) confirmó, sin cambios, los catálogos técnicos únicos de Pago.estado_pago y Devolucion.estado_devolucion; (12) incorporó Devolucion.tipo_devolucion; (13) corrigió las cardinalidades Reservacion→Pago, Pago→Devolucion, ReservacionTratamiento→Devolucion y Pago→TransaccionPago a 1:0..N.

Adicionalmente, la sección 7 documenta tres entidades estructurales propuestas para el modelo operativo de agenda (ParametroOperativo, DiaLaborable, ExcepcionOperativa); no se incorporan todavía al conteo confirmado de 23 entidades porque sus valores dependen de aprobación del negocio (sección 11). Quedan como decisiones pendientes antes de construir el esquema físico definitivo: DP-EC-01 (fórmula exacta de importe), DP-EC-02 (tratamiento económico de pago aprobado con disponibilidad perdida) y DP-OP-01 a DP-OP-08 (parámetros operativos de agenda). El Diagrama Entidad–Relación deberá representar esta versión actualizada del modelo, diferenciando de forma explícita las 23 entidades confirmadas de las entidades operativas propuestas y pendientes de aprobación.

# 14. Fuentes documentales del proyecto utilizadas

- Propuesta de Proyecto de Prácticas Profesionales – Sistema Web de Reservas, Recomendación y Gestión de Cabinas para Spa.

- TZISCA – Catálogo de cabinas y servicios.

- TZISCA – Documento de Casos de Uso CU-01 a CU-43.

- TZISCA – Reglas de Negocio Horario y Políticas, RN-01 a RN-90 (documento canónico vigente; sustituye a la referencia previa "Reglas de Negocio RN-01 a RN-72", correspondiente al documento excluido "Reglas de Negocio TZISCA Actualizadas", conforme al Control documental TZISCA).

- TZISCA – Diseño de API REST.

- TZISCA – Criterios de Aceptación CU-01 a CU-43.

# 15. Cierre del documento

Estado del documento: CERRADO. Esta versión corregida cierra el Diccionario de Datos y Modelo de Datos Depurado de TZISCA conforme a las 12 instrucciones de corrección solicitadas el 08/09/2026 (referencias actualizadas, Reservacion.estado_reservacion, ReservacionTratamiento.estado, BloqueoTemporal.fecha_expiracion, catálogos de Pago y Devolucion, TransaccionPago opcional, DP-EC-01 sin fórmula inventada, configuración operativa pendiente de modelado físico, relaciones y decisiones pendientes).

El trabajo se realizó sobre la versión vigente del documento: no se reconstruyó desde cero, no se eliminó ninguna entidad válida y no se inventó ningún valor operativo o económico pendiente de aprobación de negocio. Las decisiones que siguen pendientes de aprobación (DP-EC-01, DP-EC-02, DP-OP-01 a DP-OP-13, DP-TEC-01 a DP-TEC-03) permanecen documentadas como tales y no fueron resueltas ni asumidas por defecto.

Con este cierre, el documento queda listo para utilizarse como fuente oficial del Diagrama Entidad–Relación, del modelo físico en SQL Server, del Diseño de API REST y de la Matriz de Trazabilidad de TZISCA.
