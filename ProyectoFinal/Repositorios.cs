using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

interface IRepositorioGenerico<T>
{
    bool Agregar(List<T> Ja_lista, T Ja_entidad);
    T? BuscarPorId(List<T> Ja_lista, int Ja_id);
    bool Modificar(List<T> Ja_lista, T Ja_entidad);
    bool Eliminar(List<T> Ja_lista, int Ja_id);
    List<T> ObtenerTodos(List<T> Ja_lista);
}

abstract class RepositorioMemoria<T> : IRepositorioGenerico<T> where T : class
{
    protected abstract int ObtenerId(T Ja_entidad);

    public bool Agregar(List<T> Ja_lista, T Ja_entidad)
    {
        if (Ja_entidad == null || BuscarPorId(Ja_lista, ObtenerId(Ja_entidad)) != null) return false;
        Ja_lista.Add(Ja_entidad);
        return true;
    }

    public T? BuscarPorId(List<T> Ja_lista, int Ja_id)
    {
        foreach (T Ja_entidad in Ja_lista)
            if (ObtenerId(Ja_entidad) == Ja_id) return Ja_entidad;
        return null;
    }

    public bool Modificar(List<T> Ja_lista, T Ja_entidad)
    {
        for (int Ja_i = 0; Ja_i < Ja_lista.Count; Ja_i++)
        {
            if (ObtenerId(Ja_lista[Ja_i]) == ObtenerId(Ja_entidad))
            {
                Ja_lista[Ja_i] = Ja_entidad;
                return true;
            }
        }
        return false;
    }

    public bool Eliminar(List<T> Ja_lista, int Ja_id)
    {
        T? Ja_entidad = BuscarPorId(Ja_lista, Ja_id);
        return Ja_entidad != null && Ja_lista.Remove(Ja_entidad);
    }

    public List<T> ObtenerTodos(List<T> Ja_lista) { return new List<T>(Ja_lista); }
}

class PolizaRepositorio : RepositorioMemoria<Poliza>
{
    protected override int ObtenerId(Poliza Ja_entidad) { return Ja_entidad.IdPoliza; }
}

class SiniestroRepositorio : RepositorioMemoria<Siniestro>
{
    protected override int ObtenerId(Siniestro Ja_entidad) { return Ja_entidad.IdSiniestro; }
}

class ReaseguroRepositorio : RepositorioMemoria<Reaseguro>
{
    protected override int ObtenerId(Reaseguro Ja_entidad) { return Ja_entidad.IdReaseguro; }

    public Reaseguro? BuscarPorPoliza(List<Reaseguro> Ja_lista, int Ja_idPoliza)
    {
        foreach (Reaseguro Ja_reaseguro in Ja_lista)
            if (Ja_reaseguro.IdPoliza == Ja_idPoliza) return Ja_reaseguro;
        return null;
    }
}

class AsientoContableRepositorio : RepositorioMemoria<AsientoContable>
{
    protected override int ObtenerId(AsientoContable Ja_entidad) { return Ja_entidad.IdAsiento; }
}

class LogSistemaRepositorio : RepositorioMemoria<LogSistema>
{
    protected override int ObtenerId(LogSistema Ja_entidad) { return Ja_entidad.IdLog; }
}

static class SqlUtilidad
{
    public static SqlParameter Parametro(string Ja_nombre, SqlDbType Ja_tipo, object Ja_valor, int Ja_tamano = 0)
    {
        SqlParameter Ja_parametro = Ja_tamano > 0
            ? new SqlParameter(Ja_nombre, Ja_tipo, Ja_tamano)
            : new SqlParameter(Ja_nombre, Ja_tipo);
        Ja_parametro.Value = Ja_valor;
        return Ja_parametro;
    }

    public static DataTable Consultar(string Ja_sql, List<SqlParameter>? Ja_parametros = null)
    {
        DataTable Ja_tabla = new DataTable();
        try
        {
            using (SqlConnection Ja_conexion = new SqlConnection(ConexionBaseDatos.ObtenerCadena()))
            using (SqlCommand Ja_comando = new SqlCommand(Ja_sql, Ja_conexion))
            using (SqlDataAdapter Ja_adaptador = new SqlDataAdapter(Ja_comando))
            {
                AgregarParametros(Ja_comando, Ja_parametros);
                Ja_adaptador.Fill(Ja_tabla);
            }
        }
        catch (Exception Ja_ex)
        {
            MostrarError("ERROR SQL: ", Ja_ex);
        }
        return Ja_tabla;
    }

    public static int Ejecutar(string Ja_sql, List<SqlParameter>? Ja_parametros = null)
    {
        try
        {
            using (SqlConnection Ja_conexion = new SqlConnection(ConexionBaseDatos.ObtenerCadena()))
            using (SqlCommand Ja_comando = new SqlCommand(Ja_sql, Ja_conexion))
            {
                AgregarParametros(Ja_comando, Ja_parametros);
                Ja_conexion.Open();
                return Ja_comando.ExecuteNonQuery();
            }
        }
        catch (Exception Ja_ex)
        {
            MostrarError("ERROR SQL: ", Ja_ex);
            return -1;
        }
    }

    public static object? Escalar(string Ja_sql, List<SqlParameter>? Ja_parametros = null)
    {
        try
        {
            using (SqlConnection Ja_conexion = new SqlConnection(ConexionBaseDatos.ObtenerCadena()))
            using (SqlCommand Ja_comando = new SqlCommand(Ja_sql, Ja_conexion))
            {
                AgregarParametros(Ja_comando, Ja_parametros);
                Ja_conexion.Open();
                return Ja_comando.ExecuteScalar();
            }
        }
        catch (Exception Ja_ex)
        {
            MostrarError("ERROR SQL: ", Ja_ex);
            return null;
        }
    }

    public static int SiguienteId(string Ja_tabla, string Ja_columna)
    {
        object? Ja_resultado = Escalar($"SELECT ISNULL(MAX({Ja_columna}), 0) + 1 FROM {Ja_tabla}");
        return Ja_resultado == null || Ja_resultado == DBNull.Value ? 0 : Convert.ToInt32(Ja_resultado);
    }

    public static void MostrarError(string Ja_prefijo, Exception Ja_ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(Ja_prefijo + Ja_ex.Message);
        Console.ResetColor();
    }

    private static void AgregarParametros(SqlCommand Ja_comando, List<SqlParameter>? Ja_parametros)
    {
        if (Ja_parametros == null) return;
        foreach (SqlParameter Ja_parametro in Ja_parametros) Ja_comando.Parameters.Add(Ja_parametro);
    }
}

class ClienteSqlRepositorio
{
    public bool GuardarClientes(List<Cliente> Ja_clientes)
    {
        string Ja_sql =
            "IF EXISTS (SELECT 1 FROM Clientes WHERE IdCliente=@IdCliente) " +
            "UPDATE Clientes SET Cedula=@Cedula,Nombre=@Nombre,AlertasUAF=@Alertas WHERE IdCliente=@IdCliente " +
            "ELSE INSERT INTO Clientes (IdCliente,Cedula,Nombre,AlertasUAF) VALUES (@IdCliente,@Cedula,@Nombre,@Alertas)";

        foreach (Cliente Ja_cliente in Ja_clientes)
        {
            List<SqlParameter> Ja_parametros = new List<SqlParameter>
            {
                SqlUtilidad.Parametro("@IdCliente", SqlDbType.Int, Ja_cliente.IdCliente),
                SqlUtilidad.Parametro("@Cedula", SqlDbType.VarChar, Ja_cliente.Cedula, 10),
                SqlUtilidad.Parametro("@Nombre", SqlDbType.VarChar, Ja_cliente.Nombre, 100),
                SqlUtilidad.Parametro("@Alertas", SqlDbType.VarChar, string.Join(",", Ja_cliente.AlertasUAF), 100)
            };
            if (SqlUtilidad.Ejecutar(Ja_sql, Ja_parametros) != 1) return false;
        }
        return true;
    }
}

class RamoSqlRepositorio
{
    public bool GuardarRamos(List<Ramo> Ja_ramos)
    {
        string Ja_sql =
            "IF EXISTS (SELECT 1 FROM Ramos WHERE IdRamo=@IdRamo) " +
            "UPDATE Ramos SET Nombre=@Nombre,Descripcion=@Descripcion,Activo=@Activo WHERE IdRamo=@IdRamo " +
            "ELSE INSERT INTO Ramos (IdRamo,Nombre,Descripcion,Activo) VALUES (@IdRamo,@Nombre,@Descripcion,@Activo)";

        foreach (Ramo Ja_ramo in Ja_ramos)
        {
            List<SqlParameter> Ja_parametros = new List<SqlParameter>
            {
                SqlUtilidad.Parametro("@IdRamo", SqlDbType.Int, Ja_ramo.IdRamo),
                SqlUtilidad.Parametro("@Nombre", SqlDbType.VarChar, Ja_ramo.Nombre, 100),
                SqlUtilidad.Parametro("@Descripcion", SqlDbType.VarChar, Ja_ramo.Descripcion, 200),
                SqlUtilidad.Parametro("@Activo", SqlDbType.Bit, Ja_ramo.Activo)
            };
            if (SqlUtilidad.Ejecutar(Ja_sql, Ja_parametros) != 1) return false;
        }
        return true;
    }
}

class PolizaSqlRepositorio
{
    private const string Ja_columnas = "IdPoliza,IdCliente,IdRamo,CapitalAsegurado,CapitalRemanente,TasaRiesgo,PrimaBase,SuperBancos,SeguroCampesino,DerechoEmision,Subtotal,IVA,PrimaTotal,FechaEmision,Estado";

    public List<Poliza> ObtenerTodas()
    {
        DataTable Ja_tabla = SqlUtilidad.Consultar("SELECT " + Ja_columnas + " FROM Polizas ORDER BY IdPoliza");
        List<Poliza> Ja_lista = new List<Poliza>();
        foreach (DataRow Ja_fila in Ja_tabla.Rows) Ja_lista.Add(Crear(Ja_fila));
        return Ja_lista;
    }

    public bool ExistePorCliente(int Ja_idCliente)
    {
        List<SqlParameter> Ja_p = new List<SqlParameter> { SqlUtilidad.Parametro("@IdCliente", SqlDbType.Int, Ja_idCliente) };
        object? Ja_resultado = SqlUtilidad.Escalar("SELECT COUNT(*) FROM Polizas WHERE IdCliente=@IdCliente AND Estado='Activa'", Ja_p);
        return Ja_resultado != null && Convert.ToInt32(Ja_resultado) > 0;
    }

    public int ObtenerSiguienteId() { return SqlUtilidad.SiguienteId("Polizas", "IdPoliza"); }

    public bool Insertar(Poliza Ja_poliza)
    {
        string Ja_sql = "INSERT INTO Polizas (" + Ja_columnas + ") VALUES " +
            "(@IdPoliza,@IdCliente,@IdRamo,@CapitalAsegurado,@CapitalRemanente,@TasaRiesgo,@PrimaBase,@SuperBancos,@SeguroCampesino,@DerechoEmision,@Subtotal,@IVA,@PrimaTotal,@FechaEmision,@Estado)";
        return SqlUtilidad.Ejecutar(Ja_sql, Parametros(Ja_poliza)) == 1;
    }

    public bool Actualizar(Poliza Ja_poliza)
    {
        string Ja_sql = "UPDATE Polizas SET IdCliente=@IdCliente,IdRamo=@IdRamo,CapitalAsegurado=@CapitalAsegurado," +
            "CapitalRemanente=@CapitalRemanente,TasaRiesgo=@TasaRiesgo,PrimaBase=@PrimaBase,SuperBancos=@SuperBancos," +
            "SeguroCampesino=@SeguroCampesino,DerechoEmision=@DerechoEmision,Subtotal=@Subtotal,IVA=@IVA," +
            "PrimaTotal=@PrimaTotal,FechaEmision=@FechaEmision,Estado=@Estado WHERE IdPoliza=@IdPoliza";
        return SqlUtilidad.Ejecutar(Ja_sql, Parametros(Ja_poliza)) == 1;
    }

    public bool TieneSiniestros(int Ja_idPoliza)
    {
        List<SqlParameter> Ja_p = new List<SqlParameter> { SqlUtilidad.Parametro("@IdPoliza", SqlDbType.Int, Ja_idPoliza) };
        object? Ja_resultado = SqlUtilidad.Escalar("SELECT COUNT(*) FROM Siniestros WHERE IdPoliza=@IdPoliza", Ja_p);
        return Ja_resultado != null && Convert.ToInt32(Ja_resultado) > 0;
    }

    public bool EliminarCompletaSinSiniestros(int Ja_idPoliza)
    {
        try
        {
            using (SqlConnection Ja_conexion = new SqlConnection(ConexionBaseDatos.ObtenerCadena()))
            {
                Ja_conexion.Open();
                using (SqlTransaction Ja_transaccion = Ja_conexion.BeginTransaction())
                {
                    if (ContarSiniestros(Ja_conexion, Ja_transaccion, Ja_idPoliza) > 0) return false;
                    EjecutarTransaccion(Ja_conexion, Ja_transaccion, "DELETE FROM AsientosContables WHERE IdPoliza=@IdPoliza", Ja_idPoliza);
                    EjecutarTransaccion(Ja_conexion, Ja_transaccion, "DELETE FROM Reaseguros WHERE IdPoliza=@IdPoliza", Ja_idPoliza);
                    int Ja_filas = EjecutarTransaccion(Ja_conexion, Ja_transaccion, "DELETE FROM Polizas WHERE IdPoliza=@IdPoliza", Ja_idPoliza);
                    if (Ja_filas != 1) return false;
                    Ja_transaccion.Commit();
                    return true;
                }
            }
        }
        catch (Exception Ja_ex)
        {
            SqlUtilidad.MostrarError("ERROR al eliminar póliza: ", Ja_ex);
            return false;
        }
    }

    private int ContarSiniestros(SqlConnection Ja_conexion, SqlTransaction Ja_transaccion, int Ja_id)
    {
        using (SqlCommand Ja_comando = new SqlCommand("SELECT COUNT(*) FROM Siniestros WHERE IdPoliza=@IdPoliza", Ja_conexion, Ja_transaccion))
        {
            Ja_comando.Parameters.Add("@IdPoliza", SqlDbType.Int).Value = Ja_id;
            return Convert.ToInt32(Ja_comando.ExecuteScalar());
        }
    }

    private int EjecutarTransaccion(SqlConnection Ja_conexion, SqlTransaction Ja_transaccion, string Ja_sql, int Ja_id)
    {
        using (SqlCommand Ja_comando = new SqlCommand(Ja_sql, Ja_conexion, Ja_transaccion))
        {
            Ja_comando.Parameters.Add("@IdPoliza", SqlDbType.Int).Value = Ja_id;
            return Ja_comando.ExecuteNonQuery();
        }
    }

    private List<SqlParameter> Parametros(Poliza Ja_p)
    {
        return new List<SqlParameter>
        {
            SqlUtilidad.Parametro("@IdPoliza",SqlDbType.Int,Ja_p.IdPoliza),
            SqlUtilidad.Parametro("@IdCliente",SqlDbType.Int,Ja_p.IdCliente),
            SqlUtilidad.Parametro("@IdRamo",SqlDbType.Int,Ja_p.IdRamo),
            SqlUtilidad.Parametro("@CapitalAsegurado",SqlDbType.Decimal,Convert.ToDecimal(Ja_p.CapitalAsegurado)),
            SqlUtilidad.Parametro("@CapitalRemanente",SqlDbType.Decimal,Convert.ToDecimal(Ja_p.CapitalRemanente)),
            SqlUtilidad.Parametro("@TasaRiesgo",SqlDbType.Decimal,Convert.ToDecimal(Ja_p.TasaRiesgo)),
            SqlUtilidad.Parametro("@PrimaBase",SqlDbType.Decimal,Convert.ToDecimal(Ja_p.PrimaBase)),
            SqlUtilidad.Parametro("@SuperBancos",SqlDbType.Decimal,Convert.ToDecimal(Ja_p.SuperBancos)),
            SqlUtilidad.Parametro("@SeguroCampesino",SqlDbType.Decimal,Convert.ToDecimal(Ja_p.SeguroCampesino)),
            SqlUtilidad.Parametro("@DerechoEmision",SqlDbType.Decimal,Convert.ToDecimal(Ja_p.DerechoEmision)),
            SqlUtilidad.Parametro("@Subtotal",SqlDbType.Decimal,Convert.ToDecimal(Ja_p.Subtotal)),
            SqlUtilidad.Parametro("@IVA",SqlDbType.Decimal,Convert.ToDecimal(Ja_p.IVA)),
            SqlUtilidad.Parametro("@PrimaTotal",SqlDbType.Decimal,Convert.ToDecimal(Ja_p.PrimaTotal)),
            SqlUtilidad.Parametro("@FechaEmision",SqlDbType.DateTime2,Ja_p.FechaEmision),
            SqlUtilidad.Parametro("@Estado",SqlDbType.VarChar,Ja_p.Estado,20)
        };
    }

    private Poliza Crear(DataRow Ja_f)
    {
        return new Poliza(Convert.ToInt32(Ja_f[0]), Convert.ToInt32(Ja_f[1]), Convert.ToInt32(Ja_f[2]),
            Convert.ToDouble(Ja_f[3]), Convert.ToDouble(Ja_f[4]), Convert.ToDouble(Ja_f[5]),
            Convert.ToDouble(Ja_f[6]), Convert.ToDouble(Ja_f[7]), Convert.ToDouble(Ja_f[8]),
            Convert.ToDouble(Ja_f[9]), Convert.ToDouble(Ja_f[10]), Convert.ToDouble(Ja_f[11]),
            Convert.ToDouble(Ja_f[12]), Convert.ToDateTime(Ja_f[13]), Convert.ToString(Ja_f[14]) ?? "Activa");
    }
}

class SiniestroSqlRepositorio
{
    private const string Ja_columnas = "IdSiniestro,IdPoliza,MontoReclamo,PorcentajeDeducible,ValorDeducible,PagoNeto,CapitalConsumido,FechaSiniestro,Estado,Observacion";

    public List<Siniestro> ObtenerTodos()
    {
        DataTable Ja_tabla = SqlUtilidad.Consultar("SELECT " + Ja_columnas + " FROM Siniestros ORDER BY IdSiniestro");
        List<Siniestro> Ja_lista = new List<Siniestro>();
        foreach (DataRow Ja_f in Ja_tabla.Rows)
            Ja_lista.Add(new Siniestro(Convert.ToInt32(Ja_f[0]), Convert.ToInt32(Ja_f[1]),
                Convert.ToDouble(Ja_f[2]), Convert.ToDouble(Ja_f[3]), Convert.ToDouble(Ja_f[4]),
                Convert.ToDouble(Ja_f[5]), Convert.ToDouble(Ja_f[6]), Convert.ToDateTime(Ja_f[7]),
                Convert.ToString(Ja_f[8]) ?? "Rechazado", Convert.ToString(Ja_f[9]) ?? ""));
        return Ja_lista;
    }

    public int ObtenerSiguienteId() { return SqlUtilidad.SiguienteId("Siniestros", "IdSiniestro"); }

    public bool RegistrarConActualizacionCapital(Siniestro Ja_siniestro, double Ja_nuevoCapitalRemanente)
    {
        return GuardarConCapital(Ja_siniestro, Ja_nuevoCapitalRemanente, true);
    }

    public bool ActualizarConCapital(Siniestro Ja_siniestro, double Ja_nuevoCapitalRemanente)
    {
        return GuardarConCapital(Ja_siniestro, Ja_nuevoCapitalRemanente, false);
    }

    private bool GuardarConCapital(Siniestro Ja_siniestro, double Ja_nuevoCapitalRemanente, bool Ja_insertar)
    {
        if (Ja_nuevoCapitalRemanente < 0) return false;
        try
        {
            using (SqlConnection Ja_conexion = new SqlConnection(ConexionBaseDatos.ObtenerCadena()))
            {
                Ja_conexion.Open();
                using (SqlTransaction Ja_transaccion = Ja_conexion.BeginTransaction())
                {
                    if (!CapitalValido(Ja_conexion, Ja_transaccion, Ja_siniestro.IdPoliza, Ja_nuevoCapitalRemanente)) return false;
                    string Ja_sql = Ja_insertar
                        ? "INSERT INTO Siniestros (" + Ja_columnas + ") VALUES (@IdSiniestro,@IdPoliza,@MontoReclamo,@PorcentajeDeducible,@ValorDeducible,@PagoNeto,@CapitalConsumido,@FechaSiniestro,@Estado,@Observacion)"
                        : "UPDATE Siniestros SET IdPoliza=@IdPoliza,MontoReclamo=@MontoReclamo,PorcentajeDeducible=@PorcentajeDeducible,ValorDeducible=@ValorDeducible,PagoNeto=@PagoNeto,CapitalConsumido=@CapitalConsumido,FechaSiniestro=@FechaSiniestro,Estado=@Estado,Observacion=@Observacion WHERE IdSiniestro=@IdSiniestro";
                    if (EjecutarSiniestro(Ja_conexion, Ja_transaccion, Ja_sql, Ja_siniestro) != 1) return false;
                    if (ActualizarCapital(Ja_conexion, Ja_transaccion, Ja_siniestro.IdPoliza, Ja_nuevoCapitalRemanente) != 1) return false;
                    Ja_transaccion.Commit();
                    return true;
                }
            }
        }
        catch (Exception Ja_ex)
        {
            SqlUtilidad.MostrarError("ERROR al guardar siniestro: ", Ja_ex);
            return false;
        }
    }

    public bool EliminarConDevolucionCapital(int Ja_idSiniestro)
    {
        try
        {
            using (SqlConnection Ja_conexion = new SqlConnection(ConexionBaseDatos.ObtenerCadena()))
            {
                Ja_conexion.Open();
                using (SqlTransaction Ja_transaccion = Ja_conexion.BeginTransaction())
                {
                    int Ja_idPoliza;
                    double Ja_consumido;
                    using (SqlCommand Ja_buscar = new SqlCommand("SELECT IdPoliza,CapitalConsumido FROM Siniestros WHERE IdSiniestro=@Id", Ja_conexion, Ja_transaccion))
                    {
                        Ja_buscar.Parameters.Add("@Id", SqlDbType.Int).Value = Ja_idSiniestro;
                        using (SqlDataReader Ja_lector = Ja_buscar.ExecuteReader())
                        {
                            if (!Ja_lector.Read()) return false;
                            Ja_idPoliza = Ja_lector.GetInt32(0);
                            Ja_consumido = Convert.ToDouble(Ja_lector.GetDecimal(1));
                        }
                    }
                    double Ja_remanente = CapitalActual(Ja_conexion, Ja_transaccion, Ja_idPoliza) + Ja_consumido;
                    if (!CapitalValido(Ja_conexion, Ja_transaccion, Ja_idPoliza, Ja_remanente)) return false;
                    EjecutarPorId(Ja_conexion, Ja_transaccion, "DELETE FROM AsientosContables WHERE IdSiniestro=@Id", Ja_idSiniestro);
                    if (EjecutarPorId(Ja_conexion, Ja_transaccion, "DELETE FROM Siniestros WHERE IdSiniestro=@Id", Ja_idSiniestro) != 1) return false;
                    if (ActualizarCapital(Ja_conexion, Ja_transaccion, Ja_idPoliza, Ja_remanente) != 1) return false;
                    Ja_transaccion.Commit();
                    return true;
                }
            }
        }
        catch (Exception Ja_ex)
        {
            SqlUtilidad.MostrarError("ERROR al eliminar siniestro: ", Ja_ex);
            return false;
        }
    }

    private bool CapitalValido(SqlConnection Ja_c, SqlTransaction Ja_t, int Ja_idPoliza, double Ja_remanente)
    {
        using (SqlCommand Ja_cmd = new SqlCommand("SELECT CapitalAsegurado FROM Polizas WHERE IdPoliza=@Id", Ja_c, Ja_t))
        {
            Ja_cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Ja_idPoliza;
            object? Ja_resultado = Ja_cmd.ExecuteScalar();
            if (Ja_resultado == null || Ja_resultado == DBNull.Value) return false;
            if (Ja_remanente > Convert.ToDouble(Ja_resultado))
            {
                Console.WriteLine("ADVERTENCIA: El capital remanente no puede superar el capital asegurado.");
                return false;
            }
            return Ja_remanente >= 0;
        }
    }

    private double CapitalActual(SqlConnection Ja_c, SqlTransaction Ja_t, int Ja_idPoliza)
    {
        using (SqlCommand Ja_cmd = new SqlCommand("SELECT CapitalRemanente FROM Polizas WHERE IdPoliza=@Id", Ja_c, Ja_t))
        {
            Ja_cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Ja_idPoliza;
            return Convert.ToDouble(Ja_cmd.ExecuteScalar());
        }
    }

    private int ActualizarCapital(SqlConnection Ja_c, SqlTransaction Ja_t, int Ja_idPoliza, double Ja_remanente)
    {
        using (SqlCommand Ja_cmd = new SqlCommand("UPDATE Polizas SET CapitalRemanente=@Capital WHERE IdPoliza=@Id", Ja_c, Ja_t))
        {
            Ja_cmd.Parameters.Add("@Capital", SqlDbType.Decimal).Value = Convert.ToDecimal(Ja_remanente);
            Ja_cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Ja_idPoliza;
            return Ja_cmd.ExecuteNonQuery();
        }
    }

    private int EjecutarPorId(SqlConnection Ja_c, SqlTransaction Ja_t, string Ja_sql, int Ja_id)
    {
        using (SqlCommand Ja_cmd = new SqlCommand(Ja_sql, Ja_c, Ja_t))
        {
            Ja_cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Ja_id;
            return Ja_cmd.ExecuteNonQuery();
        }
    }

    private int EjecutarSiniestro(SqlConnection Ja_c, SqlTransaction Ja_t, string Ja_sql, Siniestro Ja_s)
    {
        using (SqlCommand Ja_cmd = new SqlCommand(Ja_sql, Ja_c, Ja_t))
        {
            foreach (SqlParameter Ja_p in Parametros(Ja_s)) Ja_cmd.Parameters.Add(Ja_p);
            return Ja_cmd.ExecuteNonQuery();
        }
    }

    private List<SqlParameter> Parametros(Siniestro Ja_s)
    {
        return new List<SqlParameter>
        {
            SqlUtilidad.Parametro("@IdSiniestro",SqlDbType.Int,Ja_s.IdSiniestro),
            SqlUtilidad.Parametro("@IdPoliza",SqlDbType.Int,Ja_s.IdPoliza),
            SqlUtilidad.Parametro("@MontoReclamo",SqlDbType.Decimal,Convert.ToDecimal(Ja_s.MontoReclamo)),
            SqlUtilidad.Parametro("@PorcentajeDeducible",SqlDbType.Decimal,Convert.ToDecimal(Ja_s.PorcentajeDeducible)),
            SqlUtilidad.Parametro("@ValorDeducible",SqlDbType.Decimal,Convert.ToDecimal(Ja_s.ValorDeducible)),
            SqlUtilidad.Parametro("@PagoNeto",SqlDbType.Decimal,Convert.ToDecimal(Ja_s.PagoNeto)),
            SqlUtilidad.Parametro("@CapitalConsumido",SqlDbType.Decimal,Convert.ToDecimal(Ja_s.CapitalConsumido)),
            SqlUtilidad.Parametro("@FechaSiniestro",SqlDbType.DateTime2,Ja_s.FechaSiniestro),
            SqlUtilidad.Parametro("@Estado",SqlDbType.VarChar,Ja_s.Estado,20),
            SqlUtilidad.Parametro("@Observacion",SqlDbType.VarChar,Ja_s.Observacion,300)
        };
    }
}

class ReaseguroSqlRepositorio
{
    private const string Ja_columnas = "IdReaseguro,IdPoliza,MontoRetencion,MontoContrato,MontoFacultativo,TotalRepartido,IndiceRetencion,IndiceContrato,IndiceFacultativo,FechaGeneracion,Generado";

    public List<Reaseguro> ObtenerTodos()
    {
        DataTable Ja_tabla = SqlUtilidad.Consultar("SELECT " + Ja_columnas + " FROM Reaseguros ORDER BY IdReaseguro");
        List<Reaseguro> Ja_lista = new List<Reaseguro>();
        foreach (DataRow Ja_f in Ja_tabla.Rows) Ja_lista.Add(Crear(Ja_f));
        return Ja_lista;
    }

    public Reaseguro? BuscarPorPoliza(int Ja_idPoliza)
    {
        List<SqlParameter> Ja_p = new List<SqlParameter> { SqlUtilidad.Parametro("@IdPoliza", SqlDbType.Int, Ja_idPoliza) };
        DataTable Ja_tabla = SqlUtilidad.Consultar("SELECT " + Ja_columnas + " FROM Reaseguros WHERE IdPoliza=@IdPoliza", Ja_p);
        return Ja_tabla.Rows.Count == 0 ? null : Crear(Ja_tabla.Rows[0]);
    }

    public int ObtenerSiguienteId() { return SqlUtilidad.SiguienteId("Reaseguros", "IdReaseguro"); }
    public bool Insertar(Reaseguro Ja_r)
    {
        string Ja_sql = "INSERT INTO Reaseguros (" + Ja_columnas + ") VALUES (@IdReaseguro,@IdPoliza,@MontoRetencion,@MontoContrato,@MontoFacultativo,@TotalRepartido,@IndiceRetencion,@IndiceContrato,@IndiceFacultativo,@FechaGeneracion,@Generado)";
        return SqlUtilidad.Ejecutar(Ja_sql, Parametros(Ja_r)) == 1;
    }
    public bool Actualizar(Reaseguro Ja_r)
    {
        string Ja_sql = "UPDATE Reaseguros SET IdPoliza=@IdPoliza,MontoRetencion=@MontoRetencion,MontoContrato=@MontoContrato,MontoFacultativo=@MontoFacultativo,TotalRepartido=@TotalRepartido,IndiceRetencion=@IndiceRetencion,IndiceContrato=@IndiceContrato,IndiceFacultativo=@IndiceFacultativo,FechaGeneracion=@FechaGeneracion,Generado=@Generado WHERE IdReaseguro=@IdReaseguro";
        return SqlUtilidad.Ejecutar(Ja_sql, Parametros(Ja_r)) == 1;
    }

    private Reaseguro Crear(DataRow Ja_f)
    {
        return new Reaseguro(Convert.ToInt32(Ja_f[0]), Convert.ToInt32(Ja_f[1]), Convert.ToDouble(Ja_f[2]),
            Convert.ToDouble(Ja_f[3]), Convert.ToDouble(Ja_f[4]), Convert.ToDouble(Ja_f[5]),
            Convert.ToInt32(Ja_f[6]), Convert.ToInt32(Ja_f[7]), Convert.ToInt32(Ja_f[8]),
            Convert.ToDateTime(Ja_f[9]), Convert.ToBoolean(Ja_f[10]));
    }

    private List<SqlParameter> Parametros(Reaseguro Ja_r)
    {
        return new List<SqlParameter>
        {
            SqlUtilidad.Parametro("@IdReaseguro",SqlDbType.Int,Ja_r.IdReaseguro),
            SqlUtilidad.Parametro("@IdPoliza",SqlDbType.Int,Ja_r.IdPoliza),
            SqlUtilidad.Parametro("@MontoRetencion",SqlDbType.Decimal,Convert.ToDecimal(Ja_r.MontoRetencion)),
            SqlUtilidad.Parametro("@MontoContrato",SqlDbType.Decimal,Convert.ToDecimal(Ja_r.MontoContrato)),
            SqlUtilidad.Parametro("@MontoFacultativo",SqlDbType.Decimal,Convert.ToDecimal(Ja_r.MontoFacultativo)),
            SqlUtilidad.Parametro("@TotalRepartido",SqlDbType.Decimal,Convert.ToDecimal(Ja_r.TotalRepartido)),
            SqlUtilidad.Parametro("@IndiceRetencion",SqlDbType.Int,Ja_r.IndiceRetencion),
            SqlUtilidad.Parametro("@IndiceContrato",SqlDbType.Int,Ja_r.IndiceContrato),
            SqlUtilidad.Parametro("@IndiceFacultativo",SqlDbType.Int,Ja_r.IndiceFacultativo),
            SqlUtilidad.Parametro("@FechaGeneracion",SqlDbType.DateTime2,Ja_r.FechaGeneracion),
            SqlUtilidad.Parametro("@Generado",SqlDbType.Bit,Ja_r.Generado)
        };
    }
}

class AsientoContableSqlRepositorio
{
    private const string Ja_columnas = "IdAsiento,TipoOperacion,CuentaDebe,CuentaHaber,Valor,Fecha,IdPoliza,IdSiniestro,Descripcion,Estado";

    public List<AsientoContable> ObtenerTodos()
    {
        DataTable Ja_tabla = SqlUtilidad.Consultar("SELECT " + Ja_columnas + " FROM AsientosContables ORDER BY IdAsiento");
        return Convertir(Ja_tabla);
    }

    public AsientoContable? BuscarPagoPorSiniestro(int Ja_idSiniestro)
    {
        List<SqlParameter> Ja_p = new List<SqlParameter> { SqlUtilidad.Parametro("@Id", SqlDbType.Int, Ja_idSiniestro) };
        DataTable Ja_t = SqlUtilidad.Consultar("SELECT " + Ja_columnas + " FROM AsientosContables WHERE IdSiniestro=@Id AND TipoOperacion='Siniestro'", Ja_p);
        List<AsientoContable> Ja_lista = Convertir(Ja_t);
        return Ja_lista.Count == 0 ? null : Ja_lista[0];
    }

    public AsientoContable? BuscarPorPoliza(int Ja_idPoliza, string Ja_tipo)
    {
        List<SqlParameter> Ja_p = new List<SqlParameter>
        {
            SqlUtilidad.Parametro("@Id",SqlDbType.Int,Ja_idPoliza),
            SqlUtilidad.Parametro("@Tipo",SqlDbType.VarChar,Ja_tipo,30)
        };
        DataTable Ja_t = SqlUtilidad.Consultar("SELECT " + Ja_columnas + " FROM AsientosContables WHERE IdPoliza=@Id AND TipoOperacion=@Tipo", Ja_p);
        List<AsientoContable> Ja_lista = Convertir(Ja_t);
        return Ja_lista.Count == 0 ? null : Ja_lista[0];
    }

    public bool ExisteAsientoOperacionPoliza(int Ja_idPoliza, string Ja_tipo)
    {
        return BuscarPorPoliza(Ja_idPoliza, Ja_tipo) != null;
    }

    public int ObtenerSiguienteId() { return SqlUtilidad.SiguienteId("AsientosContables", "IdAsiento"); }
    public bool Insertar(AsientoContable Ja_a)
    {
        string Ja_sql = "INSERT INTO AsientosContables (" + Ja_columnas + ") VALUES (@IdAsiento,@TipoOperacion,@CuentaDebe,@CuentaHaber,@Valor,@Fecha,@IdPoliza,@IdSiniestro,@Descripcion,@Estado)";
        return SqlUtilidad.Ejecutar(Ja_sql, Parametros(Ja_a)) == 1;
    }
    public bool Actualizar(AsientoContable Ja_a)
    {
        string Ja_sql = "UPDATE AsientosContables SET TipoOperacion=@TipoOperacion,CuentaDebe=@CuentaDebe,CuentaHaber=@CuentaHaber,Valor=@Valor,Fecha=@Fecha,IdPoliza=@IdPoliza,IdSiniestro=@IdSiniestro,Descripcion=@Descripcion,Estado=@Estado WHERE IdAsiento=@IdAsiento";
        return SqlUtilidad.Ejecutar(Ja_sql, Parametros(Ja_a)) == 1;
    }

    private List<AsientoContable> Convertir(DataTable Ja_tabla)
    {
        List<AsientoContable> Ja_lista = new List<AsientoContable>();
        foreach (DataRow Ja_f in Ja_tabla.Rows)
        {
            int Ja_poliza = Ja_f.IsNull(6) ? -1 : Convert.ToInt32(Ja_f[6]);
            int Ja_siniestro = Ja_f.IsNull(7) ? -1 : Convert.ToInt32(Ja_f[7]);
            Ja_lista.Add(new AsientoContable(Convert.ToInt32(Ja_f[0]), Convert.ToString(Ja_f[1]) ?? "Emision",
                Convert.ToString(Ja_f[2]) ?? "", Convert.ToString(Ja_f[3]) ?? "", Convert.ToDouble(Ja_f[4]),
                Convert.ToDateTime(Ja_f[5]), Ja_poliza, Ja_siniestro, Convert.ToString(Ja_f[8]) ?? "",
                Convert.ToString(Ja_f[9]) ?? "Registrado"));
        }
        return Ja_lista;
    }

    private List<SqlParameter> Parametros(AsientoContable Ja_a)
    {
        return new List<SqlParameter>
        {
            SqlUtilidad.Parametro("@IdAsiento",SqlDbType.Int,Ja_a.IdAsiento),
            SqlUtilidad.Parametro("@TipoOperacion",SqlDbType.VarChar,Ja_a.TipoOperacion,30),
            SqlUtilidad.Parametro("@CuentaDebe",SqlDbType.VarChar,Ja_a.CuentaDebe,100),
            SqlUtilidad.Parametro("@CuentaHaber",SqlDbType.VarChar,Ja_a.CuentaHaber,100),
            SqlUtilidad.Parametro("@Valor",SqlDbType.Decimal,Convert.ToDecimal(Ja_a.Valor)),
            SqlUtilidad.Parametro("@Fecha",SqlDbType.DateTime2,Ja_a.Fecha),
            SqlUtilidad.Parametro("@IdPoliza",SqlDbType.Int,Ja_a.IdPoliza > 0 ? Ja_a.IdPoliza : DBNull.Value),
            SqlUtilidad.Parametro("@IdSiniestro",SqlDbType.Int,Ja_a.IdSiniestro > 0 ? Ja_a.IdSiniestro : DBNull.Value),
            SqlUtilidad.Parametro("@Descripcion",SqlDbType.VarChar,Ja_a.Descripcion,300),
            SqlUtilidad.Parametro("@Estado",SqlDbType.VarChar,Ja_a.Estado,20)
        };
    }
}

class LogSistemaSqlRepositorio
{
    public List<LogSistema> ObtenerTodos()
    {
        DataTable Ja_tabla = SqlUtilidad.Consultar("SELECT IdLog,Fecha,Modulo,Tipo,Mensaje,Usuario FROM Logs ORDER BY IdLog");
        List<LogSistema> Ja_lista = new List<LogSistema>();
        foreach (DataRow Ja_f in Ja_tabla.Rows)
            Ja_lista.Add(new LogSistema(Convert.ToInt32(Ja_f[0]), Convert.ToDateTime(Ja_f[1]),
                Convert.ToString(Ja_f[2]) ?? "Sistema", Convert.ToString(Ja_f[3]) ?? "Informacion",
                Convert.ToString(Ja_f[4]) ?? "", Convert.ToString(Ja_f[5]) ?? "Sistema"));
        return Ja_lista;
    }

    public int ObtenerSiguienteId() { return SqlUtilidad.SiguienteId("Logs", "IdLog"); }

    public bool Insertar(LogSistema Ja_log)
    {
        string Ja_sql = "INSERT INTO Logs (IdLog,Fecha,Modulo,Tipo,Mensaje,Usuario) VALUES (@Id,@Fecha,@Modulo,@Tipo,@Mensaje,@Usuario)";
        List<SqlParameter> Ja_p = new List<SqlParameter>
        {
            SqlUtilidad.Parametro("@Id",SqlDbType.Int,Ja_log.IdLog),
            SqlUtilidad.Parametro("@Fecha",SqlDbType.DateTime2,Ja_log.Fecha),
            SqlUtilidad.Parametro("@Modulo",SqlDbType.VarChar,Ja_log.Modulo,30),
            SqlUtilidad.Parametro("@Tipo",SqlDbType.VarChar,Ja_log.Tipo,30),
            SqlUtilidad.Parametro("@Mensaje",SqlDbType.VarChar,Ja_log.Mensaje,500),
            SqlUtilidad.Parametro("@Usuario",SqlDbType.VarChar,Ja_log.Usuario,100)
        };
        return SqlUtilidad.Ejecutar(Ja_sql, Ja_p) == 1;
    }
}
