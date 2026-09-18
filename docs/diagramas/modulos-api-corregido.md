# Módulos de la API

Propuesta de corrección local de `docs/diagramas/modulos-api.md`.

```mermaid
flowchart LR
    API["API REST /api/v1"]
    API --- M1["/auth"]
    API --- M2["/users"]
    API --- M3["/clients"]
    API --- M4["/treatments"]
    API --- M5["/packages"]
    API --- M6["/cabins"]
    API --- M7["/recommendations"]
    API --- M8["/availability"]
    API --- M9["/cart"]
    API --- M10["/appointments"]
    API --- M11["/providers"]
    API --- M12["/payments"]
    API --- M13["/refunds"]
    API --- M14["/reports"]
```

Las líneas indican pertenencia al contrato API, no dependencias ni orden de ejecución. El contrato vigente usa la base `/api/v1`, contiene 14 módulos y 68 endpoints, conforme a `docs/api/diseno-api-rest.md`.

Cambios propuestos: se sustituyen `/temporary-blocks`, `/reservations` y `/provider-assignments` por `/clients`, `/packages` y `/appointments`. El bloqueo temporal y las asignaciones de proveedor son comportamientos de los recursos vigentes; no son módulos independientes.
