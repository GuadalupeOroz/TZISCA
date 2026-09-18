# Diagramas técnicos

Propuesta de corrección local de `docs/diagramas/README.md`.

Los diagramas documentan el diseño previsto de TZISCA. No representan una aplicación implementada.

## Inventario verificado

El directorio `docs/diagramas/` contiene 20 diagramas: 4 documentos Mermaid y 16 exportaciones PNG del FigJam.

| Grupo | Cantidad | Elementos |
| --- | ---: | --- |
| Mermaid | 4 | Arquitectura, flujo de citas, módulos API y estados de cita |
| Casos de uso | 5 | General, Cliente, Administrador, Recepción y Proveedor |
| Flujos | 3 | Cita del cliente, cancelación y sustitución de proveedor |
| Secuencias | 5 | Cita, cancelación, sustitución, pago y devolución |
| Arquitectura técnica | 1 | Arquitectura técnica de TZISCA |
| Modelos ER | 2 | Modelo relacional y representación Chen |
| **Total** | **20** | **4 Mermaid + 16 PNG** |

## Modelos entidad–relación

- `modelo-entidad-relacion-figma.png`: exportación visual de referencia; contiene elementos del modelo anterior y no sustituye al diccionario de datos vigente.
- `modelo-entidad-relacion-chen-figjam.png`: segunda representación ER en notación Chen; también requiere corrección contra el modelo oficial.
- La fuente canónica para entidades, atributos, PK, FK y cardinalidades es `docs/modelo-datos/diccionario-datos.md`, con 19 entidades distribuidas en los schemas `seguridad`, `catalogo`, `reservas`, `operacion` y `pagos`.

Las copias propuestas `modelo-entidad-relacion-figma-corregido.png` y `modelo-entidad-relacion-chen-figjam-corregido.png` actualizan el modelo a esas 19 entidades. No reemplazan las exportaciones originales ni modifican el FigJam.

## Correcciones asociadas

- Usar los 14 módulos vigentes en `modulos-api-corregido.md`.
- Usar `COMPLETADA`, no `COMPLETADO`, en el diagrama de estados.
- Mantener “Reservación” como texto de interfaz cuando corresponda; no tratarlo como entidad, tabla, clase o recurso API vigente.
