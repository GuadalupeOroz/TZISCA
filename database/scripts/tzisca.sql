IF DB_ID(N'TZISCA') IS NULL
BEGIN
    CREATE DATABASE TZISCA;
END;
GO

USE TZISCA;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'seguridad')
    EXEC(N'CREATE SCHEMA seguridad');
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'catalogo')
    EXEC(N'CREATE SCHEMA catalogo');
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'reservas')
    EXEC(N'CREATE SCHEMA reservas');
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'operacion')
    EXEC(N'CREATE SCHEMA operacion');
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'pagos')
    EXEC(N'CREATE SCHEMA pagos');
GO


CREATE TABLE seguridad.Rol (
    id_rol INT IDENTITY(1,1) NOT NULL,
    nombre NVARCHAR(50) NOT NULL,
    descripcion NVARCHAR(250) NULL,
    activo BIT NOT NULL
        CONSTRAINT DF_Rol_Activo DEFAULT (1),

    CONSTRAINT PK_Rol PRIMARY KEY (id_rol),
    CONSTRAINT UQ_Rol_Nombre UNIQUE (nombre),
    CONSTRAINT CK_Rol_Nombre_NoVacio
        CHECK (LEN(LTRIM(RTRIM(nombre))) > 0)
);
GO


CREATE TABLE seguridad.Usuario (
    id_usuario INT IDENTITY(1,1) NOT NULL,
    id_rol INT NOT NULL,
    identity_user_id NVARCHAR(450) NOT NULL,
    nombre NVARCHAR(100) NOT NULL,
    correo NVARCHAR(256) NOT NULL,
    activo BIT NOT NULL
        CONSTRAINT DF_Usuario_Activo DEFAULT (1),
    fecha_registro DATETIME2 NOT NULL
        CONSTRAINT DF_Usuario_FechaRegistro DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_Usuario PRIMARY KEY (id_usuario),

    CONSTRAINT FK_Usuario_Rol
        FOREIGN KEY (id_rol)
        REFERENCES seguridad.Rol(id_rol),

    CONSTRAINT UQ_Usuario_IdentityUserId UNIQUE (identity_user_id),
    CONSTRAINT UQ_Usuario_Correo UNIQUE (correo),

    CONSTRAINT CK_Usuario_Nombre_NoVacio
        CHECK (LEN(LTRIM(RTRIM(nombre))) > 0),

    CONSTRAINT CK_Usuario_Correo_NoVacio
        CHECK (LEN(LTRIM(RTRIM(correo))) > 0)
);
GO

CREATE INDEX IX_Usuario_Rol
    ON seguridad.Usuario(id_rol);
GO


CREATE TABLE seguridad.Cliente (
    id_cliente INT IDENTITY(1,1) NOT NULL,
    id_usuario INT NOT NULL,
    telefono NVARCHAR(25) NULL,
    activo BIT NOT NULL
        CONSTRAINT DF_Cliente_Activo DEFAULT (1),
    fecha_registro DATETIME2 NOT NULL
        CONSTRAINT DF_Cliente_FechaRegistro DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_Cliente PRIMARY KEY (id_cliente),

    CONSTRAINT FK_Cliente_Usuario
        FOREIGN KEY (id_usuario)
        REFERENCES seguridad.Usuario(id_usuario),

    CONSTRAINT UQ_Cliente_Usuario UNIQUE (id_usuario)
);
GO


CREATE TABLE seguridad.PreferenciaCliente (
    id_preferencia INT IDENTITY(1,1) NOT NULL,
    id_cliente INT NOT NULL,
    tipo_experiencia NVARCHAR(100) NULL,
    caracteristicas NVARCHAR(500) NULL,
    observaciones NVARCHAR(500) NULL,
    fecha_actualizacion DATETIME2 NOT NULL
        CONSTRAINT DF_PreferenciaCliente_FechaActualizacion
        DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_PreferenciaCliente PRIMARY KEY (id_preferencia),

    CONSTRAINT FK_PreferenciaCliente_Cliente
        FOREIGN KEY (id_cliente)
        REFERENCES seguridad.Cliente(id_cliente),

    CONSTRAINT UQ_PreferenciaCliente_Cliente UNIQUE (id_cliente)
);
GO


CREATE TABLE catalogo.Tratamiento (
    id_tratamiento INT IDENTITY(1,1) NOT NULL,
    nombre NVARCHAR(120) NOT NULL,
    descripcion NVARCHAR(MAX) NOT NULL,
    duracion_minutos INT NOT NULL,
    precio_base DECIMAL(10,2) NOT NULL,
    requisitos_cabina NVARCHAR(500) NULL,
    activo BIT NOT NULL
        CONSTRAINT DF_Tratamiento_Activo DEFAULT (1),

    CONSTRAINT PK_Tratamiento PRIMARY KEY (id_tratamiento),
    CONSTRAINT UQ_Tratamiento_Nombre UNIQUE (nombre),

    CONSTRAINT CK_Tratamiento_Nombre_NoVacio
        CHECK (LEN(LTRIM(RTRIM(nombre))) > 0),

    CONSTRAINT CK_Tratamiento_Duracion
        CHECK (duracion_minutos > 0),

    CONSTRAINT CK_Tratamiento_PrecioBase
        CHECK (precio_base >= 0)
);
GO


CREATE TABLE reservas.Carrito (
    id_carrito BIGINT IDENTITY(1,1) NOT NULL,
    id_cliente INT NOT NULL,
    estado NVARCHAR(20) NOT NULL
        CONSTRAINT DF_Carrito_Estado DEFAULT (N'ACTIVO'),
    fecha_creacion DATETIME2 NOT NULL
        CONSTRAINT DF_Carrito_FechaCreacion DEFAULT (SYSUTCDATETIME()),
    fecha_actualizacion DATETIME2 NOT NULL
        CONSTRAINT DF_Carrito_FechaActualizacion DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_Carrito PRIMARY KEY (id_carrito),

    CONSTRAINT FK_Carrito_Cliente
        FOREIGN KEY (id_cliente)
        REFERENCES seguridad.Cliente(id_cliente),

    CONSTRAINT CK_Carrito_Estado
        CHECK (estado IN (
            N'ACTIVO',
            N'CONVERTIDO',
            N'ABANDONADO',
            N'EXPIRADO'
        ))
);
GO

CREATE INDEX IX_Carrito_Cliente
    ON reservas.Carrito(id_cliente);
GO


CREATE TABLE operacion.Proveedor (
    id_proveedor INT IDENTITY(1,1) NOT NULL,
    id_usuario INT NOT NULL,
    activo BIT NOT NULL
        CONSTRAINT DF_Proveedor_Activo DEFAULT (1),
    fecha_alta DATETIME2 NOT NULL
        CONSTRAINT DF_Proveedor_FechaAlta DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_Proveedor PRIMARY KEY (id_proveedor),

    CONSTRAINT FK_Proveedor_Usuario
        FOREIGN KEY (id_usuario)
        REFERENCES seguridad.Usuario(id_usuario),

    CONSTRAINT UQ_Proveedor_Usuario UNIQUE (id_usuario)
);
GO


CREATE TABLE operacion.TratamientoProveedor (
    id_tratamiento INT NOT NULL,
    id_proveedor INT NOT NULL,
    activo BIT NOT NULL
        CONSTRAINT DF_TratamientoProveedor_Activo DEFAULT (1),

    CONSTRAINT PK_TratamientoProveedor
        PRIMARY KEY (id_tratamiento, id_proveedor),

    CONSTRAINT FK_TratamientoProveedor_Tratamiento
        FOREIGN KEY (id_tratamiento)
        REFERENCES catalogo.Tratamiento(id_tratamiento),

    CONSTRAINT FK_TratamientoProveedor_Proveedor
        FOREIGN KEY (id_proveedor)
        REFERENCES operacion.Proveedor(id_proveedor)
);
GO

CREATE INDEX IX_TratamientoProveedor_Proveedor
    ON operacion.TratamientoProveedor(id_proveedor);
GO


CREATE TABLE operacion.DisponibilidadProveedor (
    id_disponibilidad BIGINT IDENTITY(1,1) NOT NULL,
    id_proveedor INT NOT NULL,
    fecha_hora_inicio DATETIME2 NOT NULL,
    fecha_hora_fin DATETIME2 NOT NULL,
    tipo NVARCHAR(20) NOT NULL,
    motivo NVARCHAR(250) NULL,
    activo BIT NOT NULL
        CONSTRAINT DF_DisponibilidadProveedor_Activo DEFAULT (1),

    CONSTRAINT PK_DisponibilidadProveedor PRIMARY KEY (id_disponibilidad),

    CONSTRAINT FK_DisponibilidadProveedor_Proveedor
        FOREIGN KEY (id_proveedor)
        REFERENCES operacion.Proveedor(id_proveedor),

    CONSTRAINT CK_DisponibilidadProveedor_Horario
        CHECK (fecha_hora_fin > fecha_hora_inicio),

    CONSTRAINT CK_DisponibilidadProveedor_Tipo
        CHECK (tipo IN (N'DISPONIBLE', N'NO_DISPONIBLE'))
);
GO

CREATE INDEX IX_DisponibilidadProveedor_Proveedor_Inicio
    ON operacion.DisponibilidadProveedor(id_proveedor, fecha_hora_inicio);
GO


CREATE TABLE catalogo.Paquete (
    id_paquete INT IDENTITY(1,1) NOT NULL,
    nombre NVARCHAR(120) NOT NULL,
    descripcion NVARCHAR(MAX) NULL,
    activo BIT NOT NULL
        CONSTRAINT DF_Paquete_Activo DEFAULT (1),

    CONSTRAINT PK_Paquete PRIMARY KEY (id_paquete),
    CONSTRAINT UQ_Paquete_Nombre UNIQUE (nombre),

    CONSTRAINT CK_Paquete_Nombre_NoVacio
        CHECK (LEN(LTRIM(RTRIM(nombre))) > 0)
);
GO


CREATE TABLE catalogo.PaqueteTratamiento (
    id_paquete INT NOT NULL,
    id_tratamiento INT NOT NULL,

    CONSTRAINT PK_PaqueteTratamiento
        PRIMARY KEY (id_paquete, id_tratamiento),

    CONSTRAINT FK_PaqueteTratamiento_Paquete
        FOREIGN KEY (id_paquete)
        REFERENCES catalogo.Paquete(id_paquete),

    CONSTRAINT FK_PaqueteTratamiento_Tratamiento
        FOREIGN KEY (id_tratamiento)
        REFERENCES catalogo.Tratamiento(id_tratamiento)
);
GO

CREATE INDEX IX_PaqueteTratamiento_Tratamiento
    ON catalogo.PaqueteTratamiento(id_tratamiento);
GO


CREATE TABLE operacion.Cabina (
    id_cabina INT IDENTITY(1,1) NOT NULL,
    nombre NVARCHAR(120) NOT NULL,
    tipo NVARCHAR(60) NOT NULL,
    descripcion NVARCHAR(MAX) NULL,
    capacidad_maxima INT NOT NULL,
    caracteristicas NVARCHAR(MAX) NULL,
    beneficios NVARCHAR(MAX) NULL,
    prioridad INT NOT NULL
        CONSTRAINT DF_Cabina_Prioridad DEFAULT (0),
    activo BIT NOT NULL
        CONSTRAINT DF_Cabina_Activo DEFAULT (1),
    estado NVARCHAR(20) NOT NULL
        CONSTRAINT DF_Cabina_Estado DEFAULT (N'DISPONIBLE'),

    CONSTRAINT PK_Cabina PRIMARY KEY (id_cabina),
    CONSTRAINT UQ_Cabina_Nombre UNIQUE (nombre),

    CONSTRAINT CK_Cabina_Nombre_NoVacio
        CHECK (LEN(LTRIM(RTRIM(nombre))) > 0),

    CONSTRAINT CK_Cabina_Capacidad
        CHECK (capacidad_maxima > 0),

    CONSTRAINT CK_Cabina_Prioridad
        CHECK (prioridad >= 0),

    CONSTRAINT CK_Cabina_Estado
        CHECK (estado IN (
            N'DISPONIBLE',
            N'OCUPADA',
            N'LIMPIEZA',
            N'MANTENIMIENTO'
        ))
);
GO


CREATE TABLE catalogo.TratamientoCabina (
    id_tratamiento INT NOT NULL,
    id_cabina INT NOT NULL,
    activo BIT NOT NULL
        CONSTRAINT DF_TratamientoCabina_Activo DEFAULT (1),
    CONSTRAINT PK_TratamientoCabina PRIMARY KEY (id_tratamiento, id_cabina),
    CONSTRAINT FK_TratamientoCabina_Tratamiento FOREIGN KEY (id_tratamiento)
        REFERENCES catalogo.Tratamiento(id_tratamiento),
    CONSTRAINT FK_TratamientoCabina_Cabina FOREIGN KEY (id_cabina)
        REFERENCES operacion.Cabina(id_cabina)
);
GO
CREATE INDEX IX_TratamientoCabina_Cabina ON catalogo.TratamientoCabina(id_cabina);
GO

CREATE TABLE reservas.CarritoItem (
    id_carrito_item BIGINT IDENTITY(1,1) NOT NULL,
    id_carrito BIGINT NOT NULL,
    id_tratamiento INT NOT NULL,
    numero_personas INT NOT NULL CONSTRAINT DF_CarritoItem_NumeroPersonas DEFAULT (1),
    id_cabina INT NULL,
    fecha_hora_inicio DATETIME2 NULL,
    fecha_hora_fin DATETIME2 NULL,
    fecha_expiracion_bloqueo DATETIME2 NULL,
    fecha_creacion DATETIME2 NOT NULL CONSTRAINT DF_CarritoItem_FechaCreacion DEFAULT (SYSUTCDATETIME()),
    fecha_actualizacion DATETIME2 NOT NULL CONSTRAINT DF_CarritoItem_FechaActualizacion DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_CarritoItem PRIMARY KEY (id_carrito_item),
    CONSTRAINT FK_CarritoItem_Carrito FOREIGN KEY (id_carrito) REFERENCES reservas.Carrito(id_carrito),
    CONSTRAINT FK_CarritoItem_Tratamiento FOREIGN KEY (id_tratamiento) REFERENCES catalogo.Tratamiento(id_tratamiento),
    CONSTRAINT FK_CarritoItem_Cabina FOREIGN KEY (id_cabina) REFERENCES operacion.Cabina(id_cabina),
    CONSTRAINT CK_CarritoItem_NumeroPersonas CHECK (numero_personas > 0),
    CONSTRAINT CK_CarritoItem_HorarioCompleto CHECK ((fecha_hora_inicio IS NULL AND fecha_hora_fin IS NULL) OR (fecha_hora_inicio IS NOT NULL AND fecha_hora_fin IS NOT NULL)),
    CONSTRAINT CK_CarritoItem_RangoHorario CHECK (fecha_hora_inicio IS NULL OR fecha_hora_fin > fecha_hora_inicio)
);
GO
CREATE INDEX IX_CarritoItem_Carrito ON reservas.CarritoItem(id_carrito);
GO
CREATE INDEX IX_CarritoItem_Cabina_Bloqueo ON reservas.CarritoItem(id_cabina, fecha_hora_inicio, fecha_hora_fin, fecha_expiracion_bloqueo) WHERE id_cabina IS NOT NULL AND fecha_expiracion_bloqueo IS NOT NULL;
GO

CREATE TABLE operacion.BloqueoCabina (
    id_bloqueo_cabina BIGINT IDENTITY(1,1) NOT NULL,
    id_cabina INT NOT NULL,
    fecha_hora_inicio DATETIME2 NOT NULL,
    fecha_hora_fin DATETIME2 NOT NULL,
    motivo NVARCHAR(500) NOT NULL,
    activo BIT NOT NULL CONSTRAINT DF_BloqueoCabina_Activo DEFAULT (1),
    fecha_creacion DATETIME2 NOT NULL CONSTRAINT DF_BloqueoCabina_FechaCreacion DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_BloqueoCabina PRIMARY KEY (id_bloqueo_cabina),
    CONSTRAINT FK_BloqueoCabina_Cabina FOREIGN KEY (id_cabina) REFERENCES operacion.Cabina(id_cabina),
    CONSTRAINT CK_BloqueoCabina_Rango CHECK (fecha_hora_fin > fecha_hora_inicio),
    CONSTRAINT CK_BloqueoCabina_Motivo_NoVacio CHECK (LEN(LTRIM(RTRIM(motivo))) > 0)
);
GO
CREATE INDEX IX_BloqueoCabina_Cabina_Rango ON operacion.BloqueoCabina(id_cabina, fecha_hora_inicio, fecha_hora_fin) WHERE activo = 1;
GO

CREATE TABLE operacion.EstadoCabina (
    id_estado_cabina BIGINT IDENTITY(1,1) NOT NULL,
    id_cabina INT NOT NULL,
    estado_anterior NVARCHAR(20) NULL,
    estado_nuevo NVARCHAR(20) NOT NULL,
    fecha_cambio DATETIME2 NOT NULL
        CONSTRAINT DF_EstadoCabina_FechaCambio DEFAULT (SYSUTCDATETIME()),
    id_usuario INT NULL,
    motivo NVARCHAR(250) NULL,

    CONSTRAINT PK_EstadoCabina PRIMARY KEY (id_estado_cabina),

    CONSTRAINT FK_EstadoCabina_Cabina
        FOREIGN KEY (id_cabina)
        REFERENCES operacion.Cabina(id_cabina),

    CONSTRAINT FK_EstadoCabina_Usuario
        FOREIGN KEY (id_usuario)
        REFERENCES seguridad.Usuario(id_usuario),

    CONSTRAINT CK_EstadoCabina_Anterior
        CHECK (
            estado_anterior IS NULL
            OR estado_anterior IN (
                N'DISPONIBLE',
                N'OCUPADA',
                N'LIMPIEZA',
                N'MANTENIMIENTO'
            )
        ),

    CONSTRAINT CK_EstadoCabina_Nuevo
        CHECK (
            estado_nuevo IN (
                N'DISPONIBLE',
                N'OCUPADA',
                N'LIMPIEZA',
                N'MANTENIMIENTO'
            )
        )
);
GO

CREATE INDEX IX_EstadoCabina_Cabina_Fecha
    ON operacion.EstadoCabina(id_cabina, fecha_cambio);
GO


CREATE TABLE reservas.Cita (
    id_cita BIGINT IDENTITY(1,1) NOT NULL,
    id_cliente INT NOT NULL,
    id_tratamiento INT NOT NULL,
    id_proveedor INT NULL,
    id_carrito BIGINT NULL,
    numero_personas INT NOT NULL
        CONSTRAINT DF_Cita_NumeroPersonas DEFAULT (1),
    fecha_hora_inicio DATETIME2 NOT NULL,
    fecha_hora_fin DATETIME2 NOT NULL,
    estado NVARCHAR(20) NOT NULL
        CONSTRAINT DF_Cita_Estado DEFAULT (N'PENDIENTE'),
    precio_unitario DECIMAL(10,2) NOT NULL,
    importe DECIMAL(10,2) NOT NULL,
    fecha_expiracion_bloqueo DATETIME2 NULL,
    fecha_creacion DATETIME2 NOT NULL
        CONSTRAINT DF_Cita_FechaCreacion DEFAULT (SYSUTCDATETIME()),
    fecha_confirmacion DATETIME2 NULL,
    observaciones NVARCHAR(500) NULL,

    CONSTRAINT PK_Cita PRIMARY KEY (id_cita),

    CONSTRAINT FK_Cita_Cliente
        FOREIGN KEY (id_cliente)
        REFERENCES seguridad.Cliente(id_cliente),

    CONSTRAINT FK_Cita_Tratamiento
        FOREIGN KEY (id_tratamiento)
        REFERENCES catalogo.Tratamiento(id_tratamiento),

    CONSTRAINT FK_Cita_Proveedor
        FOREIGN KEY (id_proveedor)
        REFERENCES operacion.Proveedor(id_proveedor),

    CONSTRAINT FK_Cita_Carrito
        FOREIGN KEY (id_carrito)
        REFERENCES reservas.Carrito(id_carrito),

    CONSTRAINT CK_Cita_NumeroPersonas
        CHECK (numero_personas > 0),

    CONSTRAINT CK_Cita_Horario
        CHECK (fecha_hora_fin > fecha_hora_inicio),

    CONSTRAINT CK_Cita_Inicio_Intervalo30
        CHECK (
            DATEPART(SECOND, fecha_hora_inicio) = 0
            AND DATEPART(MILLISECOND, fecha_hora_inicio) = 0
            AND DATEPART(MINUTE, fecha_hora_inicio) IN (0, 30)
        ),

    CONSTRAINT CK_Cita_Estado
        CHECK (estado IN (
            N'PENDIENTE',
            N'CONFIRMADA',
            N'EN_ATENCION',
            N'COMPLETADA',
            N'CANCELADA',
            N'EXPIRADA'
        )),

    CONSTRAINT CK_Cita_PrecioUnitario
        CHECK (precio_unitario >= 0),

    CONSTRAINT CK_Cita_Importe
        CHECK (importe >= 0),

    CONSTRAINT CK_Cita_Importe_Calculo
        CHECK (importe = precio_unitario * numero_personas),

    CONSTRAINT CK_Cita_FechaConfirmacion
        CHECK (
            fecha_confirmacion IS NULL
            OR fecha_confirmacion >= fecha_creacion
        )
);
GO

CREATE INDEX IX_Cita_Cliente
    ON reservas.Cita(id_cliente);
GO

CREATE INDEX IX_Cita_Tratamiento
    ON reservas.Cita(id_tratamiento);
GO

CREATE INDEX IX_Cita_Proveedor_Inicio
    ON reservas.Cita(id_proveedor, fecha_hora_inicio);
GO

CREATE INDEX IX_Cita_Carrito
    ON reservas.Cita(id_carrito);
GO

CREATE INDEX IX_Cita_Estado_Inicio
    ON reservas.Cita(estado, fecha_hora_inicio);
GO


CREATE TABLE reservas.CitaCabina (
    id_cita BIGINT NOT NULL,
    id_cabina INT NOT NULL,
    fecha_asignacion DATETIME2 NOT NULL
        CONSTRAINT DF_CitaCabina_FechaAsignacion DEFAULT (SYSUTCDATETIME()),
    activo BIT NOT NULL
        CONSTRAINT DF_CitaCabina_Activo DEFAULT (1),

    CONSTRAINT PK_CitaCabina
        PRIMARY KEY (id_cita, id_cabina),

    CONSTRAINT FK_CitaCabina_Cita
        FOREIGN KEY (id_cita)
        REFERENCES reservas.Cita(id_cita),

    CONSTRAINT FK_CitaCabina_Cabina
        FOREIGN KEY (id_cabina)
        REFERENCES operacion.Cabina(id_cabina)
);
GO

CREATE INDEX IX_CitaCabina_Cabina
    ON reservas.CitaCabina(id_cabina);
GO


CREATE TABLE pagos.Pago (
    id_pago BIGINT IDENTITY(1,1) NOT NULL,
    id_cita BIGINT NOT NULL,
    monto DECIMAL(10,2) NOT NULL,
    moneda CHAR(3) NOT NULL
        CONSTRAINT DF_Pago_Moneda DEFAULT ('MXN'),
    metodo_pago NVARCHAR(40) NOT NULL,
    estado NVARCHAR(30) NOT NULL
        CONSTRAINT DF_Pago_Estado DEFAULT (N'PENDIENTE'),
    referencia NVARCHAR(150) NULL,
    fecha_creacion DATETIME2 NOT NULL
        CONSTRAINT DF_Pago_FechaCreacion DEFAULT (SYSUTCDATETIME()),
    fecha_pago DATETIME2 NULL,

    CONSTRAINT PK_Pago PRIMARY KEY (id_pago),

    CONSTRAINT FK_Pago_Cita
        FOREIGN KEY (id_cita)
        REFERENCES reservas.Cita(id_cita),

    CONSTRAINT CK_Pago_Monto
        CHECK (monto > 0),

    CONSTRAINT CK_Pago_Moneda
        CHECK (LEN(moneda) = 3),

    CONSTRAINT CK_Pago_Estado
        CHECK (estado IN (
            N'PENDIENTE',
            N'PROCESANDO',
            N'PAGADO',
            N'FALLIDO',
            N'CANCELADO',
            N'REEMBOLSADO',
            N'REEMBOLSADO_PARCIALMENTE'
        )),

    CONSTRAINT CK_Pago_FechaPago
        CHECK (
            fecha_pago IS NULL
            OR fecha_pago >= fecha_creacion
        )
);
GO

CREATE INDEX IX_Pago_Cita
    ON pagos.Pago(id_cita);
GO

CREATE INDEX IX_Pago_Estado
    ON pagos.Pago(estado);
GO

CREATE INDEX IX_Pago_Referencia
    ON pagos.Pago(referencia)
    WHERE referencia IS NOT NULL;
GO


CREATE TABLE pagos.Cancelacion (
    id_cancelacion BIGINT IDENTITY(1,1) NOT NULL,
    id_cita BIGINT NOT NULL,
    id_usuario INT NOT NULL,
    motivo NVARCHAR(500) NOT NULL,
    atribuible_spa BIT NOT NULL
        CONSTRAINT DF_Cancelacion_AtribuibleSpa DEFAULT (0),
    fecha_cancelacion DATETIME2 NOT NULL
        CONSTRAINT DF_Cancelacion_FechaCancelacion DEFAULT (SYSUTCDATETIME()),
    observaciones NVARCHAR(500) NULL,

    CONSTRAINT PK_Cancelacion PRIMARY KEY (id_cancelacion),

    CONSTRAINT FK_Cancelacion_Cita
        FOREIGN KEY (id_cita)
        REFERENCES reservas.Cita(id_cita),

    CONSTRAINT FK_Cancelacion_Usuario
        FOREIGN KEY (id_usuario)
        REFERENCES seguridad.Usuario(id_usuario),

    CONSTRAINT CK_Cancelacion_Motivo_NoVacio
        CHECK (LEN(LTRIM(RTRIM(motivo))) > 0)
);
GO

CREATE INDEX IX_Cancelacion_Cita
    ON pagos.Cancelacion(id_cita);
GO

CREATE INDEX IX_Cancelacion_Usuario
    ON pagos.Cancelacion(id_usuario);
GO


CREATE TABLE pagos.Devolucion (
    id_devolucion BIGINT IDENTITY(1,1) NOT NULL,
    id_pago BIGINT NOT NULL,
    id_cancelacion BIGINT NOT NULL,
    tipo NVARCHAR(20) NOT NULL,
    motivo NVARCHAR(500) NULL,
    porcentaje DECIMAL(5,2) NOT NULL,
    monto DECIMAL(10,2) NOT NULL
        CONSTRAINT DF_Devolucion_Monto DEFAULT (0),
    estado NVARCHAR(20) NOT NULL
        CONSTRAINT DF_Devolucion_Estado DEFAULT (N'PENDIENTE'),
    fecha_solicitud DATETIME2 NOT NULL
        CONSTRAINT DF_Devolucion_FechaSolicitud DEFAULT (SYSUTCDATETIME()),
    fecha_procesamiento DATETIME2 NULL,
    id_usuario_responsable INT NULL,

    CONSTRAINT PK_Devolucion PRIMARY KEY (id_devolucion),

    CONSTRAINT FK_Devolucion_Pago
        FOREIGN KEY (id_pago)
        REFERENCES pagos.Pago(id_pago),

    CONSTRAINT FK_Devolucion_Cancelacion
        FOREIGN KEY (id_cancelacion)
        REFERENCES pagos.Cancelacion(id_cancelacion),

    CONSTRAINT FK_Devolucion_UsuarioResponsable
        FOREIGN KEY (id_usuario_responsable)
        REFERENCES seguridad.Usuario(id_usuario),

    CONSTRAINT CK_Devolucion_Tipo
        CHECK (tipo IN (
            N'TOTAL',
            N'PARCIAL',
            N'SIN_DEVOLUCION'
        )),

    CONSTRAINT CK_Devolucion_Porcentaje
        CHECK (porcentaje IN (0, 50, 100)),

    CONSTRAINT CK_Devolucion_Monto
        CHECK (monto >= 0),

    CONSTRAINT CK_Devolucion_Estado
        CHECK (estado IN (
            N'PENDIENTE',
            N'PROCESANDO',
            N'COMPLETADA',
            N'FALLIDA',
            N'CANCELADA'
        )),

    CONSTRAINT CK_Devolucion_Fechas
        CHECK (
            fecha_procesamiento IS NULL
            OR fecha_procesamiento >= fecha_solicitud
        )
);
GO

CREATE INDEX IX_Devolucion_Pago
    ON pagos.Devolucion(id_pago);
GO

CREATE INDEX IX_Devolucion_Cancelacion
    ON pagos.Devolucion(id_cancelacion);
GO


CREATE TABLE pagos.Transaccion (
    id_transaccion BIGINT IDENTITY(1,1) NOT NULL,
    id_pago BIGINT NOT NULL,
    id_devolucion BIGINT NULL,
    referencia_externa NVARCHAR(150) NULL,
    proveedor_pago NVARCHAR(40) NOT NULL
        CONSTRAINT DF_Transaccion_ProveedorPago DEFAULT (N'STRIPE'),
    tipo NVARCHAR(30) NOT NULL,
    estado NVARCHAR(30) NOT NULL,
    codigo_respuesta NVARCHAR(100) NULL,
    fecha DATETIME2 NOT NULL
        CONSTRAINT DF_Transaccion_Fecha DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_Transaccion PRIMARY KEY (id_transaccion),

    CONSTRAINT FK_Transaccion_Pago
        FOREIGN KEY (id_pago)
        REFERENCES pagos.Pago(id_pago),

    CONSTRAINT FK_Transaccion_Devolucion
        FOREIGN KEY (id_devolucion)
        REFERENCES pagos.Devolucion(id_devolucion),

    CONSTRAINT CK_Transaccion_Tipo
        CHECK (tipo IN (
            N'PAGO',
            N'REINTENTO',
            N'DEVOLUCION'
        ))
);
GO

CREATE INDEX IX_Transaccion_Pago
    ON pagos.Transaccion(id_pago);
GO

CREATE INDEX IX_Transaccion_Devolucion
    ON pagos.Transaccion(id_devolucion)
    WHERE id_devolucion IS NOT NULL;
GO

CREATE INDEX IX_Transaccion_ReferenciaExterna
    ON pagos.Transaccion(referencia_externa)
    WHERE referencia_externa IS NOT NULL;
GO


IF DATABASE_PRINCIPAL_ID(N'rol_cliente') IS NULL
    CREATE ROLE rol_cliente;
GO

IF DATABASE_PRINCIPAL_ID(N'rol_administrador') IS NULL
    CREATE ROLE rol_administrador;
GO

IF DATABASE_PRINCIPAL_ID(N'rol_recepcionista') IS NULL
    CREATE ROLE rol_recepcionista;
GO

IF DATABASE_PRINCIPAL_ID(N'rol_proveedor') IS NULL
    CREATE ROLE rol_proveedor;
GO


GRANT SELECT ON SCHEMA::catalogo TO rol_cliente;

GRANT SELECT ON seguridad.Cliente TO rol_cliente;
GRANT SELECT, INSERT, UPDATE ON seguridad.PreferenciaCliente TO rol_cliente;

GRANT SELECT ON operacion.Cabina TO rol_cliente;
GRANT SELECT ON catalogo.TratamientoCabina TO rol_cliente;
GRANT SELECT ON operacion.Proveedor TO rol_cliente;
GRANT SELECT ON operacion.DisponibilidadProveedor TO rol_cliente;

GRANT SELECT, INSERT, UPDATE ON reservas.Carrito TO rol_cliente;
GRANT SELECT, INSERT, UPDATE, DELETE ON reservas.CarritoItem TO rol_cliente;
GRANT SELECT, INSERT ON reservas.Cita TO rol_cliente;
GRANT SELECT, INSERT, UPDATE ON reservas.CitaCabina TO rol_cliente;

GRANT SELECT ON pagos.Pago TO rol_cliente;
GRANT SELECT ON pagos.Cancelacion TO rol_cliente;
GRANT SELECT ON pagos.Devolucion TO rol_cliente;
GO


GRANT SELECT, INSERT, UPDATE, DELETE
ON SCHEMA::seguridad
TO rol_administrador;

GRANT SELECT, INSERT, UPDATE, DELETE
ON SCHEMA::catalogo
TO rol_administrador;

GRANT SELECT, INSERT, UPDATE, DELETE
ON SCHEMA::reservas
TO rol_administrador;

GRANT SELECT, INSERT, UPDATE, DELETE
ON SCHEMA::operacion
TO rol_administrador;

GRANT SELECT, INSERT, UPDATE, DELETE
ON SCHEMA::pagos
TO rol_administrador;
GO


GRANT SELECT ON seguridad.Usuario TO rol_recepcionista;
GRANT SELECT ON seguridad.Cliente TO rol_recepcionista;
GRANT SELECT ON seguridad.PreferenciaCliente TO rol_recepcionista;

GRANT SELECT ON SCHEMA::catalogo TO rol_recepcionista;

GRANT SELECT ON operacion.Proveedor TO rol_recepcionista;
GRANT SELECT ON operacion.TratamientoProveedor TO rol_recepcionista;
GRANT SELECT ON operacion.DisponibilidadProveedor TO rol_recepcionista;
GRANT SELECT, UPDATE ON operacion.Cabina TO rol_recepcionista;
GRANT SELECT ON operacion.EstadoCabina TO rol_recepcionista;
GRANT SELECT, INSERT, UPDATE ON operacion.BloqueoCabina TO rol_recepcionista;

GRANT SELECT, INSERT, UPDATE ON reservas.Cita TO rol_recepcionista;
GRANT SELECT, INSERT, UPDATE ON reservas.CitaCabina TO rol_recepcionista;

GRANT SELECT, INSERT, UPDATE ON pagos.Pago TO rol_recepcionista;
GRANT SELECT, INSERT ON pagos.Cancelacion TO rol_recepcionista;
GRANT SELECT, INSERT, UPDATE ON pagos.Devolucion TO rol_recepcionista;
GO


GRANT SELECT ON catalogo.Tratamiento TO rol_proveedor;

GRANT SELECT ON operacion.TratamientoProveedor TO rol_proveedor;
GRANT SELECT, INSERT, UPDATE ON operacion.DisponibilidadProveedor TO rol_proveedor;

GRANT SELECT, UPDATE ON reservas.Cita TO rol_proveedor;
GRANT SELECT ON reservas.CitaCabina TO rol_proveedor;

GRANT SELECT ON operacion.Cabina TO rol_proveedor;
GO


CREATE OR ALTER TRIGGER operacion.TR_Cabina_CambioEstado
ON operacion.Cabina
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO operacion.EstadoCabina
    (
        id_cabina,
        estado_anterior,
        estado_nuevo,
        fecha_cambio,
        id_usuario,
        motivo
    )
    SELECT
        i.id_cabina,
        d.estado,
        i.estado,
        SYSUTCDATETIME(),
        NULL,
        CASE
            WHEN d.id_cabina IS NULL THEN N'Estado inicial'
            ELSE N'Cambio de estado de cabina'
        END
    FROM inserted i
    LEFT JOIN deleted d
        ON d.id_cabina = i.id_cabina
    WHERE d.id_cabina IS NULL
       OR d.estado <> i.estado;
END;
GO


CREATE OR ALTER TRIGGER pagos.TR_Cancelacion_ActualizarCita
ON pagos.Cancelacion
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE c
    SET c.estado = N'CANCELADA'
    FROM reservas.Cita c
    INNER JOIN inserted i
        ON i.id_cita = c.id_cita
    WHERE c.estado NOT IN (
        N'CANCELADA',
        N'COMPLETADA',
        N'EXPIRADA'
    );
END;
GO


IF NOT EXISTS (SELECT 1 FROM seguridad.Rol WHERE nombre = N'Cliente')
    INSERT INTO seguridad.Rol(nombre, descripcion)
    VALUES (N'Cliente', N'Usuario cliente del spa');

IF NOT EXISTS (SELECT 1 FROM seguridad.Rol WHERE nombre = N'Administrador general')
    INSERT INTO seguridad.Rol(nombre, descripcion)
    VALUES (N'Administrador general', N'Administración general del sistema');

IF NOT EXISTS (SELECT 1 FROM seguridad.Rol WHERE nombre = N'Recepción y cabinas')
    INSERT INTO seguridad.Rol(nombre, descripcion)
    VALUES (N'Recepción y cabinas', N'Operación de agenda, citas y cabinas');

IF NOT EXISTS (SELECT 1 FROM seguridad.Rol WHERE nombre = N'Proveedor de tratamiento')
    INSERT INTO seguridad.Rol(nombre, descripcion)
    VALUES (N'Proveedor de tratamiento', N'Proveedor autorizado para atender tratamientos');
GO


SELECT
    s.name AS [schema],
    t.name AS tabla
FROM sys.tables t
INNER JOIN sys.schemas s
    ON s.schema_id = t.schema_id
WHERE s.name IN (
    N'seguridad',
    N'catalogo',
    N'reservas',
    N'operacion',
    N'pagos'
)
ORDER BY s.name, t.name;
GO
