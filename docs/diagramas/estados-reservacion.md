# Estados de reservación

[Índice de diagramas](README.md) · [Documentación](../README.md)

## Reservacion

```mermaid
stateDiagram-v2
    EN_PROCESO --> CONFIRMADA
    EN_PROCESO --> CANCELADA
    EN_PROCESO --> EXPIRADA
    CONFIRMADA --> CANCELADA
```

## ReservacionTratamiento

```mermaid
stateDiagram-v2
    PENDIENTE --> CONFIRMADO
    PENDIENTE --> CANCELADO
    CONFIRMADO --> EN_ATENCION
    CONFIRMADO --> CANCELADO
    EN_ATENCION --> COMPLETADO
```

Se muestran únicamente las transiciones principales solicitadas, sin añadir estados iniciales o finales. Los ciclos del encabezado y de cada tratamiento se presentan por separado.

Referencia: [Diccionario de datos](../modelo-datos/diccionario-datos.md) y [reglas de negocio](../reglas-negocio/reglas-negocio.md).
