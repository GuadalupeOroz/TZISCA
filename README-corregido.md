# TZISCA

**Proyecto académico · Diseño y documentación · Implementación pendiente**

Sistema web propuesto para citas, recomendación y gestión de cabinas para spa. La documentación describe catálogo, clientes, tratamientos, cabinas, disponibilidad, citas, proveedores, pagos, cancelaciones y devoluciones.

## Estado actual

El repositorio se encuentra en preparación documental y estructuración técnica previa al desarrollo. La documentación funcional y técnica está disponible en Markdown; no se afirma implementación de backend, frontend, endpoints, integración SQL ni pruebas.

## Arquitectura propuesta

- Frontend propuesto: Angular.
- Backend propuesto: ASP.NET Core REST API.
- Base de datos propuesta: SQL Server.
- Contrato API propuesto: `/api/v1`, 14 módulos y 68 endpoints.

## Modelo y diagramas

El modelo vigente contiene 19 entidades de dominio, organizadas en los schemas `seguridad`, `catalogo`, `reservas`, `operacion` y `pagos`. `Cita` es la unidad principal de agenda y `Cliente` es distinto de `Usuario`.

El inventario actual incluye 20 diagramas: 4 documentos Mermaid y 16 exportaciones PNG de FigJam. Entre estas últimas existen dos representaciones ER: `modelo-entidad-relacion-figma.png` y `modelo-entidad-relacion-chen-figjam.png`. Ambas exportaciones son referencias visuales y deben contrastarse contra `docs/modelo-datos/diccionario-datos.md` antes de considerarlas canónicas.

## Pendientes

- Desarrollo de backend y frontend.
- Integración y verificación SQL.
- Implementación de endpoints.
- Pruebas.
- Validación funcional de las propuestas visuales y actualización controlada del FigJam, si se aprueba.
