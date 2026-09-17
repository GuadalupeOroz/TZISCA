# Estados de cita

[Índice de diagramas](README.md) · [Documentación](../README.md)

## Cita

```mermaid
stateDiagram-v2
    PENDIENTE --> CONFIRMADA
    PENDIENTE --> CANCELADA
    PENDIENTE --> EXPIRADA
    CONFIRMADA --> CANCELADA
    CONFIRMADA --> EN_ATENCION
    EN_ATENCION --> CANCELADA
    EN_ATENCION --> COMPLETADO
```

Se muestran las transiciones principales documentadas para la entidad Cita. Una Cita representa un tratamiento programado para un Cliente; no existe una entidad independiente de detalle de reservación.

Referencia: [Diccionario de datos](../modelo-datos/diccionario-datos.md) y [reglas de negocio](../reglas-negocio/reglas-negocio.md).
