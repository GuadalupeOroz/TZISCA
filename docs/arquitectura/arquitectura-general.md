# Arquitectura General — TZISCA

TZISCA utiliza una arquitectura cliente-servidor con separación de responsabilidades entre frontend, backend y base de datos.

La solución está diseñada para mantener separadas la interfaz de usuario, la lógica de negocio, la API y la persistencia de información.

---

## Vista general

```text
Usuarios
   │
   ▼
Frontend
Angular
   │
   ▼
API REST
ASP.NET Core
   │
   ▼
Lógica de negocio
   │
   ▼
Persistencia
SQL Server
```

---

## Componentes principales

### Frontend

El frontend de TZISCA será desarrollado con Angular y será responsable de las interfaces utilizadas por:

- Cliente.
- Administrador general.
- Recepción y cabinas.
- Proveedor de tratamiento.

Desde el frontend se realizarán operaciones como consulta de tratamientos, selección de cabinas, carrito, disponibilidad, reservaciones, pagos, agendas y funciones administrativas.

La lógica crítica del sistema no deberá depender únicamente del frontend.

### Backend

El backend será desarrollado con ASP.NET Core y concentrará la lógica principal de TZISCA.

Será responsable de:

- Exponer la API REST.
- Aplicar reglas de negocio.
- Gestionar autenticación y autorización mediante ASP.NET Core Identity, emitiendo JWT como access token y un refresh token seguro para renovación de sesión, con autorización por rol y permisos (DP-TEC-01, RN-106).
- Validar disponibilidad mediante AvailabilityService, con los parámetros operativos aprobados (horario, días laborables, intervalos, anticipación y bloqueo temporal).
- Administrar bloqueos temporales.
- Gestionar reservaciones.
- Gestionar proveedores y asignaciones, incluyendo la recomendación determinista de cabinas mediante RecommendationService (DP-TEC-03, RN-108).
- Gestionar pagos mediante PaymentService, la abstracción interna que integra Stripe como pasarela inicial aprobada sin acoplar la lógica de negocio al proveedor externo (DP-TEC-02, RN-107).
- Gestionar devoluciones mediante RefundService, aplicando las políticas de cancelación y devolución aprobadas (DP-OP-09 a DP-OP-13, RN-99 a RN-103).
- Mantener trazabilidad.
- Acceder a la base de datos.

### Servicios internos del backend (decisiones aprobadas)

Conforme a *Decisiones Aprobadas TZISCA*, el backend organiza la lógica crítica en los siguientes servicios internos. Esta sección es documental: no convierte estos servicios en código todavía.

- **ASP.NET Core Identity** — gestión de usuarios y credenciales (DP-TEC-01).
- **JWT** — access token emitido tras la autenticación, con refresh token seguro para renovación de sesión (DP-TEC-01, RN-106).
- **AvailabilityService** — cálculo de disponibilidad con los parámetros operativos aprobados: apertura 09:00, cierre 20:00, días laborables lunes a sábado, domingo no laboral con excepciones operativas, intervalos de 30 minutos, anticipación mínima de 2 horas y máxima de 60 días, y bloqueo temporal de 15 minutos (DP-OP-01 a DP-OP-08, RN-91 a RN-98).
- **RefundService** — determinación de devoluciones conforme a la política aprobada de tolerancia, cancelación y devolución (DP-OP-09 a DP-OP-13, RN-99 a RN-103).
- **PaymentService** — abstracción interna de pagos que integra **Stripe** como pasarela inicial aprobada, sin acoplar la lógica de negocio al proveedor externo (DP-TEC-02, RN-107).
- **RecommendationService** — algoritmo determinista de recomendación de cabinas (DP-TEC-03, RN-108).

---

## API REST

La comunicación entre Angular y el backend se realizará mediante una API REST utilizando JSON como formato principal de intercambio.

El prefijo propuesto para la API es:

```text
/api/v1
```

Los módulos funcionales definidos para TZISCA son:

- `/auth`
- `/users`
- `/treatments`
- `/cabins`
- `/recommendations`
- `/availability`
- `/cart`
- `/temporary-blocks`
- `/reservations`
- `/providers`
- `/provider-assignments`
- `/payments`
- `/refunds`
- `/reports`

La API deberá aplicar autenticación, autorización por rol, validaciones de negocio y respuestas HTTP consistentes.


---

## Base de datos

TZISCA utilizará SQL Server como sistema de persistencia.

La base de datos almacenará información relacionada con:

- Roles.
- Usuarios.
- Preferencias del cliente.
- Tipos de cabina.
- Cabinas.
- Tratamientos.
- Compatibilidades tratamiento-cabina.
- Proveedores.
- Compatibilidades proveedor-tratamiento.
- Carritos.
- Elementos del carrito.
- Bloqueos temporales.
- Reservaciones.
- Tratamientos reservados.
- Asignaciones de proveedor.
- Bloqueos operativos de cabina.
- Historiales de estado.
- Indisponibilidades de proveedor.
- Cancelaciones.
- Pagos.
- Devoluciones.


---

## Flujo general de una solicitud

```text
Usuario
   │
   ▼
Vista Angular
   │
   ▼
Servicio del frontend
   │
   ▼
HTTP Request
   │
   ▼
API REST
   │
   ▼
Lógica de negocio
   │
   ▼
Acceso a datos
   │
   ▼
SQL Server
   │
   ▼
Response JSON
   │
   ▼
Frontend
   │
   ▼
Usuario
```

---

## Disponibilidad

La disponibilidad deberá calcularse considerando todo el intervalo del tratamiento y no solamente la hora de inicio.

El backend deberá considerar:

- Duración completa del tratamiento.
- Cabina seleccionada.
- Compatibilidad tratamiento-cabina.
- Capacidad requerida.
- Estado operativo de la cabina.
- Reservaciones existentes.
- Bloqueos temporales.
- Bloqueos operativos.
- Periodos fuera de servicio.
- Parámetros operativos aprobados.

AvailabilityService deberá usar los valores aprobados de horario de apertura (09:00), cierre (20:00), días laborables (lunes a sábado, domingo no laboral con excepciones operativas), intervalos de agenda (30 minutos), anticipación mínima (2 horas), anticipación máxima (60 días) y duración del bloqueo temporal (15 minutos) como configuración (DP-OP-01 a DP-OP-08, RN-91 a RN-98), y no como valores fijos asumidos directamente en el código.

---

## Pagos

Los pagos se gestionarán de forma separada de la reservación, a través de PaymentService.

Una reservación podrá relacionarse con uno o más registros de pago para conservar intentos, estados, referencias, fechas, métodos de pago y trazabilidad.

La confirmación definitiva de una reservación que requiera pago dependerá de que el pago correspondiente haya sido aprobado. El importe se calcula conforme a la fórmula aprobada (DP-EC-01, RN-104): precio_base por persona, importe = precio_unitario × numero_personas.

Stripe es la pasarela inicial aprobada (DP-TEC-02, RN-107) y se integrará detrás de PaymentService, sin acoplar la lógica de negocio al proveedor externo. Pago conserva el estado financiero interno de TZISCA; TransaccionPago registra la interacción técnica con Stripe cuando se implemente. Esta arquitectura es documental y no implementa la integración con Stripe todavía.

TZISCA no deberá almacenar datos bancarios sensibles completos.

Si un pago queda `PAGADO` pero la revalidación de disponibilidad detecta que se perdió, la Reservacion no se confirma y permanece `EN_PROCESO` conforme a DP-EC-02 (RN-105): el Cliente puede seleccionar otra cabina u horario, conservar los tratamientos válidos con devolución parcial del afectado, o cancelar la operación con devolución total.

---

## Devoluciones

Las devoluciones se manejarán como operaciones independientes relacionadas con un pago, a través de RefundService.

La lógica de devolución deberá consultar la política de devolución aprobada por tramos de anticipación antes de determinar si corresponde un reembolso y cuál será el importe (DP-OP-11/DP-OP-12, RN-101 a RN-103): 100% con 24 horas o más de anticipación, 50% entre 6 y menos de 24 horas, sin devolución con menos de 6 horas, y 100% cuando la cancelación es atribuible al spa. No hay devolución por inasistencia, servicio iniciado o servicio completado.

---

## Principios técnicos

- Separación de responsabilidades.
- API REST versionada.
- Comunicación mediante JSON.
- Autenticación y autorización por rol.
- Validación de reglas críticas en backend.
- No exposición de datos sensibles.
- Conservación de información histórica.
- Trazabilidad de operaciones relevantes.
- Prevención de traslapes.
- Evitar dependencias innecesarias entre módulos.

---

## Documentación relacionada

- [Propuesta del proyecto](../propuesta/propuesta-proyecto.md)
- [Catálogo de cabinas](../catalogos/catalogo-cabinas.md)
- [Diseño de API REST](../api/diseno-api-rest.md)
- [Casos de uso](../casos-de-uso/)
- [Criterios de aceptación](../criterios-aceptacion/)
- [Reglas de negocio](../reglas-negocio/)
- [Modelo de datos](../modelo-datos/)
- [Diagramas](../diagramas/)
- [Vistas](../vistas/)


---

## Flujo de reservación y confirmación

El flujo de reservación de TZISCA separa la creación previa de la reservación de su confirmación definitiva.

1. El Cliente inicia sesión.
2. Consulta el catálogo.
3. Consulta el detalle del tratamiento.
4. Agrega uno o varios tratamientos al carrito.
5. Configura número de personas.
6. Obtiene recomendación de cabina.
7. Selecciona una cabina compatible.
8. Selecciona fecha y hora.
9. TZISCA valida disponibilidad.
10. El sistema crea un bloqueo temporal.
11. El Cliente revisa el resumen.
12. TZISCA crea la `Reservacion` en estado `EN_PROCESO`.
13. Cada `ReservacionTratamiento` queda inicialmente en estado `PENDIENTE`.
14. El sistema calcula el importe conforme a la política aprobada.
15. El Cliente realiza el pago.
16. TZISCA valida el resultado del pago.
17. Si el pago queda `PAGADO`, el sistema vuelve a validar la disponibilidad.
18. Si la disponibilidad continúa válida, la reservación se confirma.
19. Los `ReservacionTratamiento` válidos pasan de `PENDIENTE` a `CONFIRMADO`.
20. Los bloqueos temporales se convierten en ocupaciones reales.
21. El Cliente puede consultar la reservación y el estado del pago.

Un pago `FALLIDO`, `CANCELADO` o rechazado no deberá producir la confirmación definitiva.


---

## Decisiones aprobadas

Los parámetros funcionales, económicos y técnicos que antes estaban pendientes fueron aprobados mediante *Decisiones Aprobadas TZISCA* y se convirtieron en las reglas de negocio RN-91 a RN-108. Deben implementarse como configuración (AvailabilityService, RefundService) y no como constantes dispersas en el código.

### Parámetros operativos aprobados

- Hora de apertura: 09:00.
- Hora de cierre: 20:00; ningún tratamiento debe finalizar después de esa hora.
- Días laborales: lunes a sábado.
- Días no laborales: domingo; festivos, cierres extraordinarios y horarios especiales se gestionan mediante excepciones operativas.
- Duración de intervalos de agenda: 30 minutos.
- Anticipación mínima para reservar: 2 horas.
- Anticipación máxima para reservar: 60 días.
- Duración del bloqueo temporal: 15 minutos.
- Política de tolerancia: 15 minutos; superado ese margen, Recepción evalúa si el servicio aún puede realizarse.
- Política de cancelación: el Cliente cancela antes del inicio; Administrador general y Recepción y cabinas cancelan por causas operativas registrando motivo y responsable.
- Condiciones de devolución: 100% con 24 horas o más de anticipación, 50% entre 6 y menos de 24 horas, sin devolución con menos de 6 horas; 100% si la cancelación es atribuible al spa.
- Casos sin derecho a devolución: inasistencia, cancelación con menos de 6 horas, servicio iniciado o servicio completado.

### Decisiones económicas aprobadas

- Fórmula del importe de un tratamiento: `precio_base` es precio por persona.
- El importe depende del número de personas: `importe = precio_unitario × numero_personas`.
- El total de la reservación es la suma de los importes de sus tratamientos; el MVP no aplica cargos adicionales.
- Pago aprobado con disponibilidad perdida: la Reservacion no se confirma y permanece `EN_PROCESO`; el Cliente elige entre otra cabina/horario, devolución parcial del tratamiento afectado o cancelación con devolución total, sin que TZISCA presuma ninguna acción automáticamente.

### Decisiones técnicas aprobadas

- Mecanismo de autenticación: ASP.NET Core Identity + JWT, con refresh token seguro y autorización por rol y permisos.
- Pasarela de pago: Stripe, integrada detrás de PaymentService.
- Algoritmo determinista de recomendación de cabinas: compatibilidad, capacidad, estado operativo, disponibilidad, preferencias, especialización, prioridad configurada e id_cabina ascendente como desempate final.

Estas decisiones ya pueden utilizarse para la implementación de AvailabilityService, RefundService, PaymentService, la autenticación y RecommendationService.

