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
- Gestionar autenticación y autorización.
- Validar disponibilidad.
- Administrar bloqueos temporales.
- Gestionar reservaciones.
- Gestionar proveedores y asignaciones.
- Gestionar pagos y devoluciones.
- Mantener trazabilidad.
- Acceder a la base de datos.


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

Los valores de horario de apertura, cierre, días laborables, anticipación mínima y máxima y duración del bloqueo temporal deberán manejarse como configuración y no como valores asumidos en código.

---

## Pagos

Los pagos se gestionarán de forma separada de la reservación.

Una reservación podrá relacionarse con uno o más registros de pago para conservar intentos, estados, referencias, fechas, métodos de pago y trazabilidad.

La confirmación definitiva de una reservación que requiera pago dependerá de que el pago correspondiente haya sido aprobado.

TZISCA no deberá almacenar datos bancarios sensibles completos.

---

## Devoluciones

Las devoluciones se manejarán como operaciones independientes relacionadas con un pago.

La lógica de devolución deberá consultar las políticas aprobadas antes de determinar si corresponde un reembolso y cuál será el importe.

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

## Decisiones pendientes antes de cerrar la implementación

Existen parámetros funcionales, económicos y técnicos que todavía no deben fijarse como constantes en el código.

### Parámetros operativos pendientes

- Hora de apertura.
- Hora de cierre.
- Días laborales.
- Días no laborales.
- Duración de intervalos de agenda.
- Anticipación mínima para reservar.
- Anticipación máxima para reservar.
- Duración del bloqueo temporal.
- Política de tolerancia.
- Política de cancelación.
- Condiciones de devolución.
- Tiempo límite para cancelar con devolución.
- Casos sin derecho a devolución.

### Decisiones económicas pendientes

- Fórmula del importe de un tratamiento.
- Definir si `precio_base` corresponde por servicio o por persona.
- Definir si el importe depende del número de personas.
- Definir posibles cargos adicionales.
- Definir el tratamiento económico cuando exista pago aprobado pero se pierda la disponibilidad.

### Decisiones técnicas pendientes

- Mecanismo concreto de autenticación.
- Pasarela o proveedor de pago.
- Algoritmo determinista de recomendación de cabinas.

Estas decisiones deberán permanecer como configuración o definición pendiente hasta contar con aprobación formal.

