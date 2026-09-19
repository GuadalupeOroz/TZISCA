# Módulos de la API

[Índice de diagramas](README.md) · [Documentación](../README.md)

```mermaid
flowchart LR
    API["API REST /api/v1"]
    API --- M0["/auth"]
    API --- M1["/users"]
    API --- M2["/clients"]
    API --- M3["/treatments"]
    API --- M4["/packages"]
    API --- M5["/cabins"]
    API --- M6["/recommendations"]
    API --- M7["/availability"]
    API --- M8["/cart"]
    API --- M9["/appointments"]
    API --- M10["/providers"]
    API --- M11["/payments"]
    API --- M12["/refunds"]
    API --- M13["/reports"]
```

Las líneas indican pertenencia al contrato API, no dependencias ni orden de ejecución. Los 14 módulos corresponden al [Diseño de API REST](../api/diseno-api-rest.md). `CarritoItem` se gestiona en `/cart` y `BloqueoCabina` en `/cabins`; no constituyen módulos independientes.
