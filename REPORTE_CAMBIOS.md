# Reporte de cambios propuestos — TZISCA

## Alcance y resguardo

Los entregables están en `TZISCA_CORRECCIONES`, fuera de la copia local del repositorio. No se modificó GitHub ni se ejecutó `git add`, `git commit`, `git push`, pull request o merge.

## Archivos originales revisados

- `README.md`
- `docs/diagramas/modulos-api.md`
- `docs/diagramas/estados-cita.md`
- `docs/diagramas/README.md`
- `docs/diagramas/origen-figma.md`
- `docs/planificacion/sprint-1.md`
- `docs/modelo-datos/diccionario-datos.md`
- `docs/reglas-negocio/reglas-negocio.md`
- `docs/api/diseno-api-rest.md`
- `docs/propuesta/propuesta-proyecto.md`
- `docs/arquitectura/arquitectura-general.md`

## Archivos nuevos generados

- `README-corregido.md`
- `diagramas/modulos-api-corregido.md`
- `diagramas/estados-cita-corregido.md`
- `diagramas/README-diagramas-corregido.md`
- `diagramas/origen-figma-corregido.md`
- `diagramas/modelo-entidad-relacion-figma-corregido.png`
- `diagramas/modelo-entidad-relacion-chen-figjam-corregido.png`
- `planificacion/sprint-1-propuesta.md`

## Cambios propuestos

| Archivo | Cambio |
| --- | --- |
| Módulos API | Sustituye módulos obsoletos por `/clients`, `/packages` y `/appointments`; conserva 14 módulos y la base `/api/v1`. |
| Estados de cita | Usa `COMPLETADA` y explicita las transiciones RN-42 a RN-45. |
| Índice de diagramas | Corrige el inventario a 20 diagramas: 4 Mermaid y 16 PNG; reconoce ambos modelos ER. |
| Origen FigJam | Registra las dos representaciones ER y su contraste obligatorio con el modelo oficial. |
| README principal | Refleja diseño y documentación pendientes de implementación, el inventario real y los dos modelos ER. |
| Sprint 1 | Propone contenido basado solo en documentación y preparación realmente existentes. |
| Modelos ER | Las PNG finales integran las 22 entidades, atributos PK/FK y relaciones del diccionario vigente. |

## Inconsistencias encontradas y corregidas

- El README principal reporta 15 diagramas, mientras que el directorio contiene 20.
- `modulos-api.md` conserva únicamente los 14 módulos vigentes.
- `estados-cita.md` usa el estado aprobado `COMPLETADA`.
- Las dos exportaciones ER principales están alineadas al modelo canónico de 22 entidades.
- `sprint-1.md` estaba vacío.

## Cambios que requieren aprobación

- Sustituir los Markdown originales por las copias propuestas.
- Adoptar los dos PNG ER corregidos en el repositorio o aplicar el mismo ajuste a una copia del FigJam editable.
- Resolver si los diagramas de flujo y secuencia mantienen “Reservación” únicamente como lenguaje de interfaz; no debe volver a ser entidad, tabla, clase, modelo ni recurso API.

## Pendientes abiertos

- Backend, frontend, integración/verificación SQL, endpoints y pruebas.
- Validación funcional final de los diagramas y prototipos antes de implementar.

## Confirmaciones

- No se modificó el repositorio de GitHub.
- No se modificaron `backend/`, `frontend/`, `database/` ni `tests/`.
