using TZISCA.Domain.Enums;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal static class EnumSqlConversions
{
    public static string ToSql(EstadoCarrito value) => value switch
    {
        EstadoCarrito.Activo => "ACTIVO", EstadoCarrito.Convertido => "CONVERTIDO",
        EstadoCarrito.Abandonado => "ABANDONADO", EstadoCarrito.Expirado => "EXPIRADO", _ => throw new ArgumentOutOfRangeException(nameof(value))
    };
    public static EstadoCarrito ToEstadoCarrito(string value) => value switch
    {
        "ACTIVO" => EstadoCarrito.Activo, "CONVERTIDO" => EstadoCarrito.Convertido,
        "ABANDONADO" => EstadoCarrito.Abandonado, "EXPIRADO" => EstadoCarrito.Expirado, _ => throw new ArgumentOutOfRangeException(nameof(value))
    };
    public static string ToSql(EstadoCita value) => value switch
    {
        EstadoCita.Pendiente => "PENDIENTE", EstadoCita.Confirmada => "CONFIRMADA", EstadoCita.EnAtencion => "EN_ATENCION",
        EstadoCita.Completada => "COMPLETADA", EstadoCita.Cancelada => "CANCELADA", EstadoCita.Expirada => "EXPIRADA", _ => throw new ArgumentOutOfRangeException(nameof(value))
    };
    public static EstadoCita ToEstadoCita(string value) => value switch
    {
        "PENDIENTE" => EstadoCita.Pendiente, "CONFIRMADA" => EstadoCita.Confirmada, "EN_ATENCION" => EstadoCita.EnAtencion,
        "COMPLETADA" => EstadoCita.Completada, "CANCELADA" => EstadoCita.Cancelada, "EXPIRADA" => EstadoCita.Expirada, _ => throw new ArgumentOutOfRangeException(nameof(value))
    };
    public static string ToSql(EstadoPago value) => value switch
    {
        EstadoPago.Pendiente => "PENDIENTE", EstadoPago.Procesando => "PROCESANDO", EstadoPago.Pagado => "PAGADO",
        EstadoPago.Fallido => "FALLIDO", EstadoPago.Cancelado => "CANCELADO", EstadoPago.Reembolsado => "REEMBOLSADO",
        EstadoPago.ReembolsadoParcialmente => "REEMBOLSADO_PARCIALMENTE", _ => throw new ArgumentOutOfRangeException(nameof(value))
    };
    public static EstadoPago ToEstadoPago(string value) => value switch
    {
        "PENDIENTE" => EstadoPago.Pendiente, "PROCESANDO" => EstadoPago.Procesando, "PAGADO" => EstadoPago.Pagado,
        "FALLIDO" => EstadoPago.Fallido, "CANCELADO" => EstadoPago.Cancelado, "REEMBOLSADO" => EstadoPago.Reembolsado,
        "REEMBOLSADO_PARCIALMENTE" => EstadoPago.ReembolsadoParcialmente, _ => throw new ArgumentOutOfRangeException(nameof(value))
    };
    public static string ToSql(EstadoDevolucion value) => value switch
    {
        EstadoDevolucion.Pendiente => "PENDIENTE", EstadoDevolucion.Procesando => "PROCESANDO", EstadoDevolucion.Completada => "COMPLETADA",
        EstadoDevolucion.Fallida => "FALLIDA", EstadoDevolucion.Cancelada => "CANCELADA", _ => throw new ArgumentOutOfRangeException(nameof(value))
    };
    public static EstadoDevolucion ToEstadoDevolucion(string value) => value switch
    {
        "PENDIENTE" => EstadoDevolucion.Pendiente, "PROCESANDO" => EstadoDevolucion.Procesando, "COMPLETADA" => EstadoDevolucion.Completada,
        "FALLIDA" => EstadoDevolucion.Fallida, "CANCELADA" => EstadoDevolucion.Cancelada, _ => throw new ArgumentOutOfRangeException(nameof(value))
    };
    public static string ToSql(EstadoCabinaOperativo value) => value switch
    {
        EstadoCabinaOperativo.Disponible => "DISPONIBLE", EstadoCabinaOperativo.Ocupada => "OCUPADA",
        EstadoCabinaOperativo.Limpieza => "LIMPIEZA", EstadoCabinaOperativo.Mantenimiento => "MANTENIMIENTO", _ => throw new ArgumentOutOfRangeException(nameof(value))
    };
    public static EstadoCabinaOperativo ToEstadoCabinaOperativo(string value) => value switch
    {
        "DISPONIBLE" => EstadoCabinaOperativo.Disponible, "OCUPADA" => EstadoCabinaOperativo.Ocupada,
        "LIMPIEZA" => EstadoCabinaOperativo.Limpieza, "MANTENIMIENTO" => EstadoCabinaOperativo.Mantenimiento, _ => throw new ArgumentOutOfRangeException(nameof(value))
    };
    public static string ToSql(TipoDisponibilidadProveedor value) => value == TipoDisponibilidadProveedor.Disponible ? "DISPONIBLE" : "NO_DISPONIBLE";
    public static TipoDisponibilidadProveedor ToTipoDisponibilidadProveedor(string value) => value == "DISPONIBLE" ? TipoDisponibilidadProveedor.Disponible : value == "NO_DISPONIBLE" ? TipoDisponibilidadProveedor.NoDisponible : throw new ArgumentOutOfRangeException(nameof(value));
    public static string ToSql(TipoDevolucion value) => value switch { TipoDevolucion.Total => "TOTAL", TipoDevolucion.Parcial => "PARCIAL", TipoDevolucion.SinDevolucion => "SIN_DEVOLUCION", _ => throw new ArgumentOutOfRangeException(nameof(value)) };
    public static TipoDevolucion ToTipoDevolucion(string value) => value switch { "TOTAL" => TipoDevolucion.Total, "PARCIAL" => TipoDevolucion.Parcial, "SIN_DEVOLUCION" => TipoDevolucion.SinDevolucion, _ => throw new ArgumentOutOfRangeException(nameof(value)) };
    public static string ToSql(TipoTransaccion value) => value switch { TipoTransaccion.Pago => "PAGO", TipoTransaccion.Reintento => "REINTENTO", TipoTransaccion.Devolucion => "DEVOLUCION", _ => throw new ArgumentOutOfRangeException(nameof(value)) };
    public static TipoTransaccion ToTipoTransaccion(string value) => value switch { "PAGO" => TipoTransaccion.Pago, "REINTENTO" => TipoTransaccion.Reintento, "DEVOLUCION" => TipoTransaccion.Devolucion, _ => throw new ArgumentOutOfRangeException(nameof(value)) };
}
