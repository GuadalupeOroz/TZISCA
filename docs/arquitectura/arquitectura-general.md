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

Desde el frontend se realizarán operaciones como consulta de tratamientos, selección de cabinas, carrito, disponibilidad, citas, pagos, agendas y funciones administrativas.

La lógica crítica del sistema no deberá depender únicamente del frontend.

### Backend

El backend será desarrollado con ASP.NET Core y concentrará la lógica principal de TZISCA.

Será responsable de:

- Exponer la API REST.
- Aplicar reglas de negocio.
- Gestionar autenticación y autorización mediante ASP.NET Core Identity, emitiendo JWT como access token y un refresh token seguro para renovación de sesión, con autorización por rol y permisos (RN-107).
- Validar disponibilidad mediante AvailabilityService, con los parámetros operativos aprobados (horario, días laborables, intervalos, anticipación y bloqueo temporal).
- Administrar bloqueos temporales.
- Gestionar Citas y sus relaciones CitaCabina.
- Gestionar proveedores y su disponibilidad, incluyendo la recomendación determinista de cabinas mediante RecommendationService (RN-108).
- Gestionar pagos y devoluciones mediante PaymentService, la abstracción interna que integra Stripe como pasarela inicial aprobada sin acoplar la lógica de negocio al proveedor externo (RN-73 a RN-90, RN-107).
- Mantener trazabilidad.
- Acceder a la base de datos.

### Servicios internos del backend (decisiones aprobadas)

Conforme a *Decisiones Aprobadas TZISCA*, el backend organiza la lógica crítica en los siguientes servicios internos. Esta sección es documental: no convierte estos servicios en código todavía.

- **ASP.NET Core Identity** — gestión de usuarios y credenciales.
- **JWT** — access token emitido tras la autenticación, con refresh token seguro para renovación de sesión (RN-107).
- **AvailabilityService** — cálculo de disponibilidad con apertura 09:00, cierre 20:00, días laborables de lunes a sábado, intervalos de 30 minutos, anticipación mínima de 2 horas y máxima de 60 días, y bloqueo temporal de 15 minutos (RN-91 a RN-99).
- **PaymentService** — gestión de pagos y devoluciones; integra **Stripe** sin acoplar la lógica de negocio al proveedor externo (RN-73 a RN-90, RN-107).
- **RecommendationService** — algoritmo determinista de recomendación de cabinas (RN-108).

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
- `/clients`
- `/treatments`
- `/packages`
- `/cabins`
- `/recommendations`
- `/availability`
- `/cart`
- `/appointments`
- `/providers`
- `/payments`
- `/refunds`
- `/reports`

La API deberá aplicar autenticación, autorización por rol, validaciones de negocio y respuestas HTTP consistentes.


---

## Base de datos

TZISCA utilizará SQL Server como sistema de persistencia.

La base de datos almacenará información relacionada con:

- seguridad: Rol, Usuario, Cliente y PreferenciaCliente.
- catalogo: Tratamiento, Paquete y PaqueteTratamiento.
- reservas: Carrito, Cita y CitaCabina.
- operacion: Proveedor, TratamientoProveedor, DisponibilidadProveedor, Cabina y EstadoCabina.
- pagos: Pago, Cancelacion, Devolucion y Transaccion.


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
- Citas existentes.
- Bloqueos temporales.
- Bloqueos operativos.
- Periodos fuera de servicio.
- Parámetros operativos aprobados.

AvailabilityService deberá usar los valores aprobados de horario de apertura (09:00), cierre (20:00), días laborables (lunes a sábado, domingo no laboral con excepciones operativas), intervalos de agenda (30 minutos), anticipación mínima (2 horas), anticipación máxima (60 días) y duración del bloqueo temporal (15 minutos) como configuración (RN-91 a RN-99), y no como valores fijos asumidos directamente en el código.

---

## Pagos

Los pagos se gestionarán por medio de PaymentService y se asociarán a una Cita.

Una Cita podrá relacionarse con uno o más registros de Pago para conservar intentos, estados, referencias, fechas, métodos de pago y trazabilidad.

La confirmación definitiva de una Cita que requiera pago dependerá de que el pago correspondiente haya sido aprobado. El importe se calcula conforme a RN-106: precio_base por persona, importe = precio_unitario × numero_personas.

Stripe es la pasarela inicial aprobada y se integra detrás de PaymentService, sin acoplar la lógica de negocio al proveedor externo (RN-107). Pago conserva el estado financiero interno de TZISCA; Transaccion registra la interacción técnica con Stripe cuando se implemente. Esta arquitectura es documental y no implementa la integración con Stripe todavía.

TZISCA no deberá almacenar datos bancarios sensibles completos.

Si un Pago queda `PAGADO` pero la revalidación de disponibilidad detecta que se perdió, la Cita no se confirma y se inicia la devolución aplicable, conforme a RN-81.

---

## Devoluciones

Las devoluciones se manejarán como operaciones independientes relacionadas con un Pago y una Cancelacion, mediante PaymentService.

La lógica de devolución deberá consultar la política aprobada por tramos de anticipación antes de determinar si corresponde un reembolso y cuál será el importe (RN-101 a RN-105): 100 % con 24 horas o más de anticipación, 50 % entre 6 y menos de 24 horas, sin devolución con menos de 6 horas y 100 % cuando la cancelación es atribuible al spa. No hay devolución por inasistencia, servicio iniciado o servicio completado.

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

## Flujo de citas y confirmación

El flujo de TZISCA separa la creación de Citas desde el Carrito de su confirmación definitiva.

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
12. TZISCA crea una Cita por cada tratamiento seleccionado, en estado `PENDIENTE`.
13. Cada Cita conserva su relación CitaCabina y su fecha de expiración de bloqueo.
14. El sistema calcula el importe conforme a la política aprobada.
15. El Cliente realiza el pago.
16. TZISCA valida el resultado del pago.
17. Si el pago queda `PAGADO`, el sistema vuelve a validar la disponibilidad.
18. Si la disponibilidad continúa válida, cada Cita aplicable pasa a `CONFIRMADA`.
19. El bloqueo temporal deja de estar vigente al confirmarse o expirar la Cita.
20. El Cliente puede consultar sus Citas y el estado de cada Pago.

Un pago `FALLIDO`, `CANCELADO` o rechazado no deberá producir la confirmación definitiva.


---

## Decisiones aprobadas

Los parámetros funcionales, económicos y técnicos están establecidos en las reglas RN-91 a RN-108. Deben implementarse como configuración y no como constantes dispersas en el código.

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
- El total de un Carrito es la suma de los importes de las Citas que origine; el MVP no aplica cargos adicionales.
- Pago aprobado con disponibilidad perdida: la Cita no se confirma y se inicia la devolución aplicable, conforme a RN-81.

### Decisiones técnicas aprobadas

- Mecanismo de autenticación: ASP.NET Core Identity + JWT, con refresh token seguro y autorización por rol y permisos.
- Pasarela de pago: Stripe, integrada detrás de PaymentService.
- Algoritmo determinista de recomendación de cabinas: compatibilidad, capacidad, estado operativo, disponibilidad, preferencias, especialización, prioridad configurada e id_cabina ascendente como desempate final.

Estas decisiones ya pueden utilizarse para la implementación de AvailabilityService, PaymentService, la autenticación y RecommendationService.
