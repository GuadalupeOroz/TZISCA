# Arquitectura

[Índice de diagramas](README.md) · [Documentación](../README.md)

```mermaid
flowchart LR
    U["Usuario"] --> F["Angular"]
    F -->|HTTP / JSON| A["ASP.NET Core REST API"]
    A --> D[(SQL Server)]
```

Arquitectura prevista; la lógica de negocio y la validación de disponibilidad residen en el backend.

Fuente: [Arquitectura general](../arquitectura/arquitectura-general.md).
