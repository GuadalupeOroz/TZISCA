# Módulos de la API

[Índice de diagramas](README.md) · [Documentación](../README.md)

```mermaid
flowchart LR
    API["API REST /api/v1"]
    API --- M0["/auth"]
    API --- M1["/users"]
    API --- M2["/treatments"]
    API --- M3["/cabins"]
    API --- M4["/recommendations"]
    API --- M5["/availability"]
    API --- M6["/cart"]
    API --- M7["/temporary-blocks"]
    API --- M8["/reservations"]
    API --- M9["/providers"]
    API --- M10["/provider-assignments"]
    API --- M11["/payments"]
    API --- M12["/refunds"]
    API --- M13["/reports"]
```

Las líneas indican pertenencia al contrato API, no dependencias ni orden de ejecución. Los 14 módulos corresponden al [Diseño de API REST](../api/diseno-api-rest.md); los endpoints, permisos y DTO se consultan en ese documento.
