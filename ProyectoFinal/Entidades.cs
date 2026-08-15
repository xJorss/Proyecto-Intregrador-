using System;

class Cliente
{
    private int Ja_idCliente;
    private string Ja_cedula = "";
    private string Ja_nombre = "";
    private int[] Ja_alertasUAF = Array.Empty<int>();

    public int IdCliente { get { return Ja_idCliente; } set { if (value >= 0) Ja_idCliente = value; } }
    public string Cedula { get { return Ja_cedula; } set { if (CedulaValida(value)) Ja_cedula = value; } }
    public string Nombre { get { return Ja_nombre; } set { if (!string.IsNullOrWhiteSpace(value)) Ja_nombre = value; } }
    public int[] AlertasUAF { get { return Ja_alertasUAF; } set { if (value != null) Ja_alertasUAF = value; } }

    public Cliente() { }
    public Cliente(int Ja_id, string Ja_cedulaNueva, string Ja_nombreNuevo, int[] Ja_alertas) : this()
    {
        IdCliente = Ja_id;
        Cedula = Ja_cedulaNueva;
        Nombre = Ja_nombreNuevo;
        AlertasUAF = Ja_alertas;
    }

    private bool CedulaValida(string Ja_valor)
    {
        if (Ja_valor == null || Ja_valor.Length != 10) return false;
        for (int Ja_i = 0; Ja_i < Ja_valor.Length; Ja_i++)
            if (Ja_valor[Ja_i] < '0' || Ja_valor[Ja_i] > '9') return false;
        return true;
    }
}

class Ramo
{
    private int Ja_idRamo;
    private string Ja_nombre = "";
    private string Ja_descripcion = "";
    private bool Ja_activo;

    public int IdRamo { get { return Ja_idRamo; } set { if (value >= 0) Ja_idRamo = value; } }
    public string Nombre { get { return Ja_nombre; } set { if (!string.IsNullOrWhiteSpace(value)) Ja_nombre = value; } }
    public string Descripcion { get { return Ja_descripcion; } set { if (value != null) Ja_descripcion = value; } }
    public bool Activo { get { return Ja_activo; } set { Ja_activo = value; } }

    public Ramo() { }
    public Ramo(int Ja_id, string Ja_nombreNuevo, string Ja_descripcionNueva, bool Ja_activoNuevo) : this()
    {
        IdRamo = Ja_id;
        Nombre = Ja_nombreNuevo;
        Descripcion = Ja_descripcionNueva;
        Activo = Ja_activoNuevo;
    }
}

class Poliza
{
    private int Ja_idPoliza, Ja_idCliente, Ja_idRamo;
    private double Ja_capitalAsegurado, Ja_capitalRemanente, Ja_tasaRiesgo;
    private double Ja_primaBase, Ja_superBancos, Ja_seguroCampesino, Ja_derechoEmision;
    private double Ja_subtotal, Ja_iva, Ja_primaTotal;
    private DateTime Ja_fechaEmision = DateTime.Now;
    private string Ja_estado = "Activa";

    public int IdPoliza { get { return Ja_idPoliza; } set { if (value >= 0) Ja_idPoliza = value; } }
    public int IdCliente { get { return Ja_idCliente; } set { if (value >= 0) Ja_idCliente = value; } }
    public int IdRamo { get { return Ja_idRamo; } set { if (value >= 0) Ja_idRamo = value; } }
    public double CapitalAsegurado { get { return Ja_capitalAsegurado; } set { if (value > 0) Ja_capitalAsegurado = value; } }
    public double CapitalRemanente
    {
        get { return Ja_capitalRemanente; }
        set
        {
            if (value >= 0 && value <= Ja_capitalAsegurado) Ja_capitalRemanente = value;
        }
    }
    public double TasaRiesgo { get { return Ja_tasaRiesgo; } set { if (value > 0 && value <= 100) Ja_tasaRiesgo = value; } }
    public double PrimaBase { get { return Ja_primaBase; } set { if (value >= 0) Ja_primaBase = value; } }
    public double SuperBancos { get { return Ja_superBancos; } set { if (value >= 0) Ja_superBancos = value; } }
    public double SeguroCampesino { get { return Ja_seguroCampesino; } set { if (value >= 0) Ja_seguroCampesino = value; } }
    public double DerechoEmision { get { return Ja_derechoEmision; } set { if (value >= 0) Ja_derechoEmision = value; } }
    public double Subtotal { get { return Ja_subtotal; } set { if (value >= 0) Ja_subtotal = value; } }
    public double IVA { get { return Ja_iva; } set { if (value >= 0) Ja_iva = value; } }
    public double PrimaTotal { get { return Ja_primaTotal; } set { if (value >= 0) Ja_primaTotal = value; } }
    public DateTime FechaEmision { get { return Ja_fechaEmision; } set { Ja_fechaEmision = value; } }
    public string Estado { get { return Ja_estado; } set { if (value == "Activa" || value == "Inactiva") Ja_estado = value; } }

    public Poliza() { }
    public Poliza(int Ja_idPolizaNuevo, int Ja_idClienteNuevo, int Ja_idRamoNuevo,
        double Ja_capital, double Ja_remanente, double Ja_tasa, double Ja_base,
        double Ja_super, double Ja_campesino, double Ja_derecho, double Ja_subtotalNuevo,
        double Ja_ivaNueva, double Ja_total, DateTime Ja_fecha, string Ja_estadoNuevo) : this()
    {
        IdPoliza = Ja_idPolizaNuevo;
        IdCliente = Ja_idClienteNuevo;
        IdRamo = Ja_idRamoNuevo;
        CapitalAsegurado = Ja_capital;
        CapitalRemanente = Ja_remanente;
        TasaRiesgo = Ja_tasa;
        PrimaBase = Ja_base;
        SuperBancos = Ja_super;
        SeguroCampesino = Ja_campesino;
        DerechoEmision = Ja_derecho;
        Subtotal = Ja_subtotalNuevo;
        IVA = Ja_ivaNueva;
        PrimaTotal = Ja_total;
        FechaEmision = Ja_fecha;
        Estado = Ja_estadoNuevo;
    }
}

class Siniestro
{
    private int Ja_idSiniestro, Ja_idPoliza;
    private double Ja_montoReclamo, Ja_porcentajeDeducible, Ja_valorDeducible;
    private double Ja_pagoNeto, Ja_capitalConsumido;
    private DateTime Ja_fechaSiniestro = DateTime.Now;
    private string Ja_estado = "Rechazado", Ja_observacion = "";

    public int IdSiniestro { get { return Ja_idSiniestro; } set { if (value >= 0) Ja_idSiniestro = value; } }
    public int IdPoliza { get { return Ja_idPoliza; } set { if (value >= 0) Ja_idPoliza = value; } }
    public double MontoReclamo { get { return Ja_montoReclamo; } set { if (value > 0) Ja_montoReclamo = value; } }
    public double PorcentajeDeducible { get { return Ja_porcentajeDeducible; } set { if (value >= 0 && value <= 100) Ja_porcentajeDeducible = value; } }
    public double ValorDeducible { get { return Ja_valorDeducible; } set { if (value >= 0) Ja_valorDeducible = value; } }
    public double PagoNeto { get { return Ja_pagoNeto; } set { if (value >= 0) Ja_pagoNeto = value; } }
    public double CapitalConsumido { get { return Ja_capitalConsumido; } set { if (value >= 0) Ja_capitalConsumido = value; } }
    public DateTime FechaSiniestro { get { return Ja_fechaSiniestro; } set { Ja_fechaSiniestro = value; } }
    public string Estado { get { return Ja_estado; } set { if (value == "Aprobado" || value == "Rechazado") Ja_estado = value; } }
    public string Observacion { get { return Ja_observacion; } set { if (value != null) Ja_observacion = value; } }

    public Siniestro() { }
    public Siniestro(int Ja_id, int Ja_poliza, double Ja_monto, double Ja_porcentaje,
        double Ja_deducible, double Ja_pago, double Ja_consumido, DateTime Ja_fecha,
        string Ja_estadoNuevo, string Ja_observacionNueva) : this()
    {
        IdSiniestro = Ja_id;
        IdPoliza = Ja_poliza;
        MontoReclamo = Ja_monto;
        PorcentajeDeducible = Ja_porcentaje;
        ValorDeducible = Ja_deducible;
        PagoNeto = Ja_pago;
        CapitalConsumido = Ja_consumido;
        FechaSiniestro = Ja_fecha;
        Estado = Ja_estadoNuevo;
        Observacion = Ja_observacionNueva;
    }
}

class Reaseguro
{
    private int Ja_idReaseguro, Ja_idPoliza;
    private double Ja_montoRetencion, Ja_montoContrato, Ja_montoFacultativo, Ja_totalRepartido;
    private int Ja_indiceRetencion = -1, Ja_indiceContrato = -1, Ja_indiceFacultativo = -1;
    private DateTime Ja_fechaGeneracion = DateTime.Now;
    private bool Ja_generado;

    public int IdReaseguro { get { return Ja_idReaseguro; } set { if (value >= 0) Ja_idReaseguro = value; } }
    public int IdPoliza { get { return Ja_idPoliza; } set { if (value >= 0) Ja_idPoliza = value; } }
    public double MontoRetencion { get { return Ja_montoRetencion; } set { if (value >= 0) Ja_montoRetencion = value; } }
    public double MontoContrato { get { return Ja_montoContrato; } set { if (value >= 0) Ja_montoContrato = value; } }
    public double MontoFacultativo { get { return Ja_montoFacultativo; } set { if (value >= 0) Ja_montoFacultativo = value; } }
    public double TotalRepartido { get { return Ja_totalRepartido; } set { if (value >= 0) Ja_totalRepartido = value; } }
    public int IndiceRetencion { get { return Ja_indiceRetencion; } set { if (value >= -1) Ja_indiceRetencion = value; } }
    public int IndiceContrato { get { return Ja_indiceContrato; } set { if (value >= -1) Ja_indiceContrato = value; } }
    public int IndiceFacultativo { get { return Ja_indiceFacultativo; } set { if (value >= -1) Ja_indiceFacultativo = value; } }
    public DateTime FechaGeneracion { get { return Ja_fechaGeneracion; } set { Ja_fechaGeneracion = value; } }
    public bool Generado { get { return Ja_generado; } set { Ja_generado = value; } }

    public Reaseguro() { }
    public Reaseguro(int Ja_id, int Ja_poliza, double Ja_retencion, double Ja_contrato,
        double Ja_facultativo, double Ja_total, int Ja_indiceRet, int Ja_indiceCon,
        int Ja_indiceFac, DateTime Ja_fecha, bool Ja_generadoNuevo) : this()
    {
        IdReaseguro = Ja_id;
        IdPoliza = Ja_poliza;
        MontoRetencion = Ja_retencion;
        MontoContrato = Ja_contrato;
        MontoFacultativo = Ja_facultativo;
        TotalRepartido = Ja_total;
        IndiceRetencion = Ja_indiceRet;
        IndiceContrato = Ja_indiceCon;
        IndiceFacultativo = Ja_indiceFac;
        FechaGeneracion = Ja_fecha;
        Generado = Ja_generadoNuevo;
    }
}

class AsientoContable
{
    private int Ja_idAsiento, Ja_idPoliza = -1, Ja_idSiniestro = -1;
    private string Ja_tipoOperacion = "Emision", Ja_cuentaDebe = "", Ja_cuentaHaber = "";
    private double Ja_valor;
    private DateTime Ja_fecha = DateTime.Now;
    private string Ja_descripcion = "", Ja_estado = "Registrado";

    public int IdAsiento { get { return Ja_idAsiento; } set { if (value >= 0) Ja_idAsiento = value; } }
    public string TipoOperacion { get { return Ja_tipoOperacion; } set { if (value == "Emision" || value == "Siniestro" || value == "Reaseguro") Ja_tipoOperacion = value; } }
    public string CuentaDebe { get { return Ja_cuentaDebe; } set { if (!string.IsNullOrWhiteSpace(value)) Ja_cuentaDebe = value; } }
    public string CuentaHaber { get { return Ja_cuentaHaber; } set { if (!string.IsNullOrWhiteSpace(value)) Ja_cuentaHaber = value; } }
    public double Valor { get { return Ja_valor; } set { if (value >= 0) Ja_valor = value; } }
    public DateTime Fecha { get { return Ja_fecha; } set { Ja_fecha = value; } }
    public int IdPoliza { get { return Ja_idPoliza; } set { if (value >= -1) Ja_idPoliza = value; } }
    public int IdSiniestro { get { return Ja_idSiniestro; } set { if (value >= -1) Ja_idSiniestro = value; } }
    public string Descripcion { get { return Ja_descripcion; } set { if (value != null) Ja_descripcion = value; } }
    public string Estado { get { return Ja_estado; } set { if (value == "Registrado" || value == "Anulado") Ja_estado = value; } }

    public AsientoContable() { }
    public AsientoContable(int Ja_id, string Ja_tipo, string Ja_debe, string Ja_haber,
        double Ja_valorNuevo, DateTime Ja_fechaNueva, int Ja_poliza, int Ja_siniestro,
        string Ja_descripcionNueva, string Ja_estadoNuevo) : this()
    {
        IdAsiento = Ja_id;
        TipoOperacion = Ja_tipo;
        CuentaDebe = Ja_debe;
        CuentaHaber = Ja_haber;
        Valor = Ja_valorNuevo;
        Fecha = Ja_fechaNueva;
        IdPoliza = Ja_poliza;
        IdSiniestro = Ja_siniestro;
        Descripcion = Ja_descripcionNueva;
        Estado = Ja_estadoNuevo;
    }
}

class LogSistema
{
    private int Ja_idLog;
    private DateTime Ja_fecha = DateTime.Now;
    private string Ja_modulo = "Sistema", Ja_tipo = "Informacion", Ja_mensaje = "", Ja_usuario = "Sistema";

    public int IdLog { get { return Ja_idLog; } set { if (value >= 0) Ja_idLog = value; } }
    public DateTime Fecha { get { return Ja_fecha; } set { Ja_fecha = value; } }
    public string Modulo { get { return Ja_modulo; } set { if (!string.IsNullOrWhiteSpace(value)) Ja_modulo = value; } }
    public string Tipo { get { return Ja_tipo; } set { if (!string.IsNullOrWhiteSpace(value)) Ja_tipo = value; } }
    public string Mensaje { get { return Ja_mensaje; } set { if (!string.IsNullOrWhiteSpace(value)) Ja_mensaje = value; } }
    public string Usuario { get { return Ja_usuario; } set { if (!string.IsNullOrWhiteSpace(value)) Ja_usuario = value; } }

    public LogSistema() { }
    public LogSistema(int Ja_id, DateTime Ja_fechaNueva, string Ja_moduloNuevo,
        string Ja_tipoNuevo, string Ja_mensajeNuevo, string Ja_usuarioNuevo) : this()
    {
        IdLog = Ja_id;
        Fecha = Ja_fechaNueva;
        Modulo = Ja_moduloNuevo;
        Tipo = Ja_tipoNuevo;
        Mensaje = Ja_mensajeNuevo;
        Usuario = Ja_usuarioNuevo;
    }
}
