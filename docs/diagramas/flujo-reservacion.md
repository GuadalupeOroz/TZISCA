# Flujo principal de reservación

[Índice de diagramas](README.md) · [Documentación](../README.md)

```mermaid
flowchart TD
    A["Login"] --> B["Catálogo"]
    B --> C["Detalle"]
    C --> D["Carrito"]
    D --> E["Personas"]
    E --> F["Recomendación"]
    F --> G["Cabina"]
    G --> H["Fecha/hora"]
    H --> I["Disponibilidad"]
    I --> J["Bloqueo"]
    J --> K["Resumen"]
    K --> L["Reservacion EN_PROCESO"]
    L --> M["Pago"]
    M --> N["Revalidación"]
    N --> O["Confirmación"]
    O --> P["Mis reservaciones"]
```

Representa el camino exitoso, no las alternativas de error. Al crear la reservación, sus tratamientos quedan en `PENDIENTE`. La confirmación requiere validar el pago requerido, su pertenencia e importe, los bloqueos y la disponibilidad; entonces la reservación pasa a `CONFIRMADA` y sus tratamientos válidos a `CONFIRMADO`. Un pago aprobado con disponibilidad perdida no confirma ni genera una devolución automática mientras siga pendiente DP-EC-02.

Fuente: [Diseño de API REST](../api/diseno-api-rest.md).
