# TZISCA

![TZISCA — reservas, agenda y cabinas para spa](docs/assets/tzisca-banner.png)

**Proyecto académico · Diseño y documentación · Implementación pendiente**

[Documentación](docs/README.md) · [Diagramas](docs/diagramas/README.md) · [Vistas por rol](docs/vistas/README.md) · [Sprint 1](docs/planificacion/sprint-1.md)

Sistema Web de Reservas, Recomendación y Gestión de Cabinas para Spa.

TZISCA centraliza el catálogo de tratamientos, cabinas, disponibilidad, reservaciones, proveedores, pagos, devoluciones y operación general del spa en una sola plataforma.

---

## Arquitectura

Frontend: Angular  
Backend: ASP.NET Core REST API  
Base de datos: SQL Server

```mermaid
flowchart LR
    U["Usuario"] --> F["Angular"]
    F -->|HTTP / JSON| A["ASP.NET Core REST API"]
    A --> D[(SQL Server)]
```

[Ver diagrama de arquitectura](docs/diagramas/arquitectura.md).

---

## Funcionalidades principales

Alcance documentado, pendiente de implementación.

- Registro e inicio de sesión.
- Control de acceso por roles.
- Catálogo de tratamientos.
- Catálogo de cabinas.
- Compatibilidad tratamiento-cabina.
- Recomendación de cabinas.
- Carrito de reservación.
- Consulta de disponibilidad.
- Bloqueos temporales.
- Reservaciones.
- Gestión de proveedores.
- Asignación y sustitución de proveedores.
- Pagos.
- Devoluciones.
- Cancelaciones.
- Historial y trazabilidad.
- Reportes básicos.

---

## Roles del sistema

| Rol | Función principal |
|---|---|
| Cliente | Consulta servicios, gestiona carrito, reserva, paga y consulta sus reservaciones |
| Administrador general | Administra usuarios, tratamientos, cabinas, proveedores, pagos, devoluciones y reportes |
| Recepción y cabinas | Gestiona agenda, disponibilidad, reservaciones manuales, bloqueos y cancelaciones |
| Proveedor de tratamiento | Consulta su agenda, registra indisponibilidad e inicia o completa atenciones |

---

## Flujo principal de reservación

Login → Catálogo → Detalle → Carrito → Número de personas → Recomendación de cabina → Selección de cabina → Fecha y hora → Validación de disponibilidad → Bloqueo temporal → Resumen → Reservación `EN_PROCESO` → Pago → Revalidación → Confirmación → Mis reservaciones

[Ver flujo en Mermaid](docs/diagramas/flujo-reservacion.md). La creación deja los tratamientos en `PENDIENTE`; la confirmación requiere el pago válido cuando corresponda y la disponibilidad revalidada.

---

## Tecnologías

| Área | Tecnología |
|---|---|
| Frontend | Angular |
| Backend | ASP.NET Core |
| API | REST / JSON |
| Base de datos | SQL Server |
| Control de versiones | Git / GitHub |

---

## Documentación

[Índice general de documentación](docs/README.md).

### General

- [Propuesta del proyecto](docs/propuesta/propuesta-proyecto.md)
- [Arquitectura general](docs/arquitectura/arquitectura-general.md)
- [Catálogo de cabinas](docs/catalogos/catalogo-cabinas.md)

### Funcional

- [Casos de uso](docs/casos-de-uso/casos-de-uso.md)
- [Criterios de aceptación](docs/criterios-aceptacion/criterios-aceptacion.md)
- [Reglas de negocio](docs/reglas-negocio/reglas-negocio.md)

### Técnica

- [Diseño de API REST](docs/api/diseno-api-rest.md)
- [Diccionario de datos](docs/modelo-datos/diccionario-datos.md)
- [Diagramas](docs/diagramas/README.md)
- [Vistas](docs/vistas/README.md)

### Planificación

- [Sprint 1](docs/planificacion/sprint-1.md)

---

## Estructura del repositorio

```text
TZISCA/
├── backend/
├── frontend/
├── database/
│   ├── migrations/
│   ├── scripts/
│   └── seed/
├── docs/
│   ├── propuesta/
│   ├── planificacion/
│   ├── catalogos/
│   ├── arquitectura/
│   ├── api/
│   ├── casos-de-uso/
│   ├── criterios-aceptacion/
│   ├── reglas-negocio/
│   ├── modelo-datos/
│   ├── diagramas/
│   ├── vistas/
│   │   ├── cliente/
│   │   ├── administrador/
│   │   ├── recepcion/
│   │   └── proveedor/
│   ├── screenshots/
│   │   └── thumbs/
│   └── assets/
├── tests/
├── README.md
└── .gitignore
```

`fuentes/` y `respaldo-md/` se conservan localmente y están ignoradas por Git.

## Galería visual

La portada es una ilustración conceptual creada localmente. La documentación incluye **73 pantallas y variantes de Figma V3** y **15 diagramas del FigJam**. Las interfaces son prototipos; las capturas de una aplicación implementada siguen pendientes.

- [Inventario de vistas por rol](docs/vistas/README.md).
- [Convención de capturas y miniaturas](docs/screenshots/README.md).
- [Origen y nodos de las interfaces](docs/vistas/origen-figma.md).
- [Origen y nodos de los diagramas](docs/diagramas/origen-figma.md).

### Cliente

<table>
<tr>
<td align="center"><a href="docs/vistas/cliente/cliente-login.png"><img src="docs/screenshots/thumbs/cliente-login.png" width="240" alt="Prototipo: Acceso común — Todos los roles"><br>Acceso común — Todos los roles</a></td>
<td align="center"><a href="docs/vistas/cliente/cliente-catalogo.png"><img src="docs/screenshots/thumbs/cliente-catalogo.png" width="240" alt="Prototipo: Inicio y catálogo — Cliente"><br>Inicio y catálogo — Cliente</a></td>
<td align="center"><a href="docs/vistas/cliente/cliente-carrito.png"><img src="docs/screenshots/thumbs/cliente-carrito.png" width="240" alt="Prototipo: Tu carrito"><br>Tu carrito</a></td>
</tr>
</table>

[Ver todas las pantallas de Cliente](docs/vistas/cliente/README.md).

### Administrador

<table>
<tr>
<td align="center"><a href="docs/vistas/administrador/admin-dashboard.png"><img src="docs/screenshots/thumbs/admin-dashboard.png" width="240" alt="Prototipo: Dashboard"><br>Dashboard</a></td>
<td align="center"><a href="docs/vistas/administrador/admin-reservaciones.png"><img src="docs/screenshots/thumbs/admin-reservaciones.png" width="240" alt="Prototipo: Reservaciones"><br>Reservaciones</a></td>
<td align="center"><a href="docs/vistas/administrador/admin-pagos.png"><img src="docs/screenshots/thumbs/admin-pagos.png" width="240" alt="Prototipo: Pagos"><br>Pagos</a></td>
</tr>
</table>

[Ver todas las pantallas de Administrador general](docs/vistas/administrador/README.md).

### Recepción

<table>
<tr>
<td align="center"><a href="docs/vistas/recepcion/recepcion-agenda-diaria.png"><img src="docs/screenshots/thumbs/recepcion-agenda-diaria.png" width="240" alt="Prototipo: Agenda diaria — Recepción"><br>Agenda diaria — Recepción</a></td>
<td align="center"><a href="docs/vistas/recepcion/recepcion-reservacion-manual.png"><img src="docs/screenshots/thumbs/recepcion-reservacion-manual.png" width="240" alt="Prototipo: Reserva · 1 Cliente"><br>Reserva · 1 Cliente</a></td>
<td align="center"><a href="docs/vistas/recepcion/recepcion-pago-manual.png"><img src="docs/screenshots/thumbs/recepcion-pago-manual.png" width="240" alt="Prototipo: Reserva · 5 Pago"><br>Reserva · 5 Pago</a></td>
</tr>
</table>

[Ver todas las pantallas de Recepción y cabinas](docs/vistas/recepcion/README.md).

### Proveedor

<table>
<tr>
<td align="center"><a href="docs/vistas/proveedor/proveedor-agenda.png"><img src="docs/screenshots/thumbs/proveedor-agenda.png" width="240" alt="Prototipo: Mi agenda"><br>Mi agenda</a></td>
<td align="center"><a href="docs/vistas/proveedor/proveedor-detalle-tratamiento.png"><img src="docs/screenshots/thumbs/proveedor-detalle-tratamiento.png" width="240" alt="Prototipo: Detalle del tratamiento"><br>Detalle del tratamiento</a></td>
<td align="center"><a href="docs/vistas/proveedor/proveedor-iniciar-atencion.png"><img src="docs/screenshots/thumbs/proveedor-iniciar-atencion.png" width="240" alt="Prototipo: Confirmar inicio de atención"><br>Confirmar inicio de atención</a></td>
</tr>
</table>

[Ver todas las pantallas de Proveedor de tratamiento](docs/vistas/proveedor/README.md).

## Diagramas de casos de uso

Exportaciones del FigJam correspondientes a las cinco referencias compartidas. Abre cada miniatura para leer la imagen completa.

<table>
<tr>
<td align="center"><a href="docs/diagramas/casos-uso-general.png"><img src="docs/screenshots/thumbs/casos-uso-general.png" height="160" alt="Casos de uso general"><br>Casos de uso general</a></td>
<td align="center"><a href="docs/diagramas/casos-uso-cliente.png"><img src="docs/screenshots/thumbs/casos-uso-cliente.png" height="160" alt="Casos de uso — Cliente"><br>Casos de uso — Cliente</a></td>
<td align="center"><a href="docs/diagramas/casos-uso-administrador.png"><img src="docs/screenshots/thumbs/casos-uso-administrador.png" height="160" alt="Casos de uso — Administrador general"><br>Casos de uso — Administrador general</a></td>
</tr>
<tr>
<td align="center"><a href="docs/diagramas/casos-uso-recepcion.png"><img src="docs/screenshots/thumbs/casos-uso-recepcion.png" height="160" alt="Casos de uso — Recepción y cabinas"><br>Casos de uso — Recepción y cabinas</a></td>
<td align="center"><a href="docs/diagramas/casos-uso-proveedor.png"><img src="docs/screenshots/thumbs/casos-uso-proveedor.png" height="160" alt="Casos de uso — Proveedor y automatizaciones"><br>Casos de uso — Proveedor y automatizaciones</a></td>
</tr>
</table>

[Ver los 15 diagramas exportados y los cuatro Mermaid](docs/diagramas/README.md).

## Diagramas

- [Arquitectura](docs/diagramas/arquitectura.md)
- [Flujo de reservación](docs/diagramas/flujo-reservacion.md)
- [Módulos de la API](docs/diagramas/modulos-api.md)
- [Estados de reservación y tratamientos](docs/diagramas/estados-reservacion.md)

---

## Estado actual

El repositorio se encuentra en etapa de preparación documental y estructuración técnica previa al desarrollo.

La documentación funcional y técnica se encuentra organizada en Markdown para facilitar su consulta directamente desde GitHub.

---

El Sprint 1 existe como archivo, pero está vacío y pendiente de planificación. Las interfaces V3 ya están documentadas; quedan pendientes su validación funcional y las capturas de la aplicación implementada.

## Decisiones pendientes

Antes de cerrar ciertos módulos todavía deben aprobarse algunos parámetros:

- Horario de apertura y cierre.
- Días laborables y no laborables.
- Duración de intervalos de agenda.
- Anticipación mínima y máxima de reservación.
- Duración del bloqueo temporal.
- Política de tolerancia.
- Política de cancelación.
- Condiciones de devolución.
- Fórmula del importe.
- Tratamiento económico del pago aprobado con disponibilidad perdida (DP-EC-02).
- Pasarela de pago.
- Mecanismo concreto de autenticación.
- Algoritmo determinista de recomendación.

---

## Proyecto académico

Proyecto desarrollado como parte de prácticas profesionales.

TZISCA busca servir como base funcional y técnica para una plataforma real de gestión de servicios de spa.
