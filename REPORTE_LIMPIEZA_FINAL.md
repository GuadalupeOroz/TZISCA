# Reporte de limpieza final

**Fecha:** 19/09/2026  
**Alcance:** limpieza documental local de `TZISCA/`.

## 1. Archivos modificados

- `docs/criterios-aceptacion/criterios-aceptacion.md`
- `docs/casos-de-uso/casos-de-uso.md`
- `docs/propuesta/propuesta-proyecto.md`

También se verificaron las fuentes finales de modelo, reglas, API, diagramas, planificación, README y SQL sin alterar el modelo ni recursos visuales durante esta limpieza.

## 2. Modelo antiguo corregido

Se sustituyeron redacciones que hacían parecer que una Cita contiene varios tratamientos. La regla aplicada es: **una Cita representa exactamente un Tratamiento**. Cuando una acción incluye varios tratamientos, ahora se expresa como una operación o Carrito que genera varias Citas independientes.

## 3. Criterios de aceptación ajustados

Se revisaron especialmente CU-12, CU-13, CU-14, CU-23 y CU-42. Los detalles, cancelaciones, pagos y estados ya se describen por Cita; las diferencias de estado se aplican a Citas distintas originadas por la misma operación o Carrito.

## 4. Casos de uso revisados

Se alinearon los casos de consulta, cancelación, confirmación y atención para que los estados técnicos `PENDIENTE`, `CONFIRMADA`, `EN_ATENCION`, `COMPLETADA`, `CANCELADA` y `EXPIRADA` pertenezcan a Cita.

## 5. Errores tipográficos y técnicos

Al cierre de esta limpieza no quedaron coincidencias para `COMPLETADO`, `Citassss`, `el Cita`, `Cita, Cita, Pago`, `/temporary-blocks`, `/reservations`, `/provider-assignments` ni `ReservacionTratamiento`.

## 6. Archivos temporales eliminados

Tras compararlos con las fuentes finales, se enviaron a la Papelera local los duplicados:

- `README-corregido.md`
- `docs/diagramas/README-diagramas-corregido.md`
- `docs/diagramas/estados-cita-corregido.md`
- `docs/diagramas/modulos-api-corregido.md`
- `docs/diagramas/origen-figma-corregido.md`
- `docs/planificacion/sprint-1-propuesta.md`

No contenían información única; eran propuestas resumidas o desactualizadas frente a los archivos finales.

## 7. Archivos conservados

Se conservaron los README, documentos funcionales, reglas, API, modelo de datos, SQL y diagramas finales indicados en el alcance. No se modificaron los PNG, el FigJam/Figma ni las vistas durante esta limpieza.

## 8. Modelo y SQL

El 19/09/2026 se documentó temporalmente una ampliación a **22 entidades**, incluyendo `TratamientoCabina`, `CarritoItem`, `BloqueoCabina`, `Devolucion.motivo` y `Transaccion.id_devolucion`. Posteriormente, el 30/09/2026, SQL y código volvieron explícitamente al modelo vigente de **19 entidades**. Este reporte no modificó estructuralmente SQL.

## 9. Alcances no modificados

- FigJam/Figma: no modificado en esta limpieza.
- Backend, frontend y tests: no modificados.
- GitHub remoto: no modificado; no se ejecutó `git add`, `git commit`, `git push`, PR ni merge.

## 10. Estado

**LISTO PARA INICIAR BACKEND.** La ampliación temporal de 22 entidades quedó como antecedente histórico; el modelo vigente es de 19 entidades. La implementación, migraciones, autenticación, endpoints y pruebas siguen pendientes porque están fuera del alcance documental.
