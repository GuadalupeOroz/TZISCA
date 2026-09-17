# Flujo principal de citas

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
    K --> L["Cita PENDIENTE"]
    L --> M["Pago"]
    M --> N["Revalidación"]
    N --> O["Confirmación"]
    O --> P["Mis citas"]
```

Representa el camino exitoso, no las alternativas de error. El Carrito puede originar una o varias Citas; cada Cita nace en `PENDIENTE` y conserva su propio bloqueo temporal. La confirmación requiere validar el pago exigible, su importe, los recursos y la disponibilidad; entonces la Cita pasa a `CONFIRMADA`. Si un Pago queda `PAGADO` pero se pierde la disponibilidad, la Cita no se confirma y se inicia la devolución aplicable, conforme a RN-81.

Fuente: [Diseño de API REST](../api/diseno-api-rest.md).
