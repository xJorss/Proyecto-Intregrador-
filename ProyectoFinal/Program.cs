using System;
using System.Collections.Generic;

class SistemaSeguros
{
    static List<Cliente> Ja_ListaClientes = new List<Cliente>();
    static List<Ramo> Ja_ListaRamos = new List<Ramo>();
    static List<Poliza> Ja_ListaPolizas = new List<Poliza>();
    static List<Siniestro> Ja_ListaSiniestros = new List<Siniestro>();
    static List<Reaseguro> Ja_ListaReaseguros = new List<Reaseguro>();
    static List<AsientoContable> Ja_ListaAsientos = new List<AsientoContable>();
    static List<LogSistema> Ja_ListaLogs = new List<LogSistema>();

    static PolizaRepositorio Ja_polizaRam = new PolizaRepositorio();
    static SiniestroRepositorio Ja_siniestroRam = new SiniestroRepositorio();
    static ReaseguroRepositorio Ja_reaseguroRam = new ReaseguroRepositorio();
    static AsientoContableRepositorio Ja_asientoRam = new AsientoContableRepositorio();
    static LogSistemaRepositorio Ja_logRam = new LogSistemaRepositorio();
    static PolizaSqlRepositorio Ja_polizaSql = new PolizaSqlRepositorio();
    static SiniestroSqlRepositorio Ja_siniestroSql = new SiniestroSqlRepositorio();
    static ReaseguroSqlRepositorio Ja_reaseguroSql = new ReaseguroSqlRepositorio();
    static AsientoContableSqlRepositorio Ja_asientoSql = new AsientoContableSqlRepositorio();
    static LogSistemaSqlRepositorio Ja_logSql = new LogSistemaSqlRepositorio();

    static void Main(string[] args)
    {
        CargarDatos();
        bool Ja_continuar = true;
        while (Ja_continuar)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("SISTEMA INTEGRAL DE SEGUROS (SIS)");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Póliza");
            Console.WriteLine("2. Siniestro");
            Console.WriteLine("3. Reaseguro");
            Console.WriteLine("4. Contabilidad");
            Console.WriteLine("5. Logs");
            Console.WriteLine("6. Salir");

            switch (LeerOpcionMenu("", 1, 6))
            {
                case 1: MenuPolizas(); break;
                case 2: MenuSiniestros(); break;
                case 3: MenuReaseguro(); break;
                case 4: MenuContabilidad(); break;
                case 5: MenuLogs(); break;
                case 6:
                    GuardarArchivos();
                    Ja_continuar = false;
                    break;
            }
        }
    }

    static void CargarDatos()
    {
        Ja_ListaClientes = CargaInicial.CargarClientes();
        Ja_ListaRamos = CargaInicial.CargarRamos();
        bool Ja_clientesOk = new ClienteSqlRepositorio().GuardarClientes(Ja_ListaClientes);
        bool Ja_ramosOk = new RamoSqlRepositorio().GuardarRamos(Ja_ListaRamos);
        if (!Ja_clientesOk || !Ja_ramosOk)
            MostrarAdvertencia("ADVERTENCIA: No fue posible sincronizar los datos con SQL Server.");

        Ja_ListaPolizas = Ja_polizaSql.ObtenerTodas();
        Ja_ListaSiniestros = Ja_siniestroSql.ObtenerTodos();
        Ja_ListaReaseguros = Ja_reaseguroSql.ObtenerTodos();
        Ja_ListaAsientos = Ja_asientoSql.ObtenerTodos();
        Ja_ListaLogs = Ja_logSql.ObtenerTodos();

        Console.WriteLine("========================================");
        Console.WriteLine("        CARGA INICIAL DEL SISTEMA");
        Console.WriteLine("========================================");
        Console.WriteLine("Clientes cargados: " + Ja_ListaClientes.Count);
        Console.WriteLine("Ramos cargados: " + Ja_ListaRamos.Count);
        if (Ja_ListaClientes.Count == 10 && Ja_ListaRamos.Count == 5)
            MostrarExito("Datos iniciales cargados correctamente.");
        else
            MostrarAdvertencia("ADVERTENCIA: La carga inicial está incompleta.");
    }

    static void GuardarArchivos()
    {
        bool Ja_clientesOk = PersistenciaArchivos.GuardarClientes(Ja_ListaClientes);
        bool Ja_ramosOk = PersistenciaArchivos.GuardarRamos(Ja_ListaRamos);
        if (Ja_clientesOk && Ja_ramosOk) MostrarExito("Datos guardados correctamente.");
        else MostrarAdvertencia("ADVERTENCIA: Algunos datos no pudieron guardarse.");
    }

    static void MenuPolizas()
    {
        bool Ja_volver = false;
        while (!Ja_volver)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("=============MENU POLIZAS===============");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Emitir Póliza");
            Console.WriteLine("2. Consultar Póliza");
            Console.WriteLine("3. Modificar Póliza");
            Console.WriteLine("4. Eliminar Póliza");
            Console.WriteLine("5. Volver al menú principal");
            switch (LeerOpcionMenu("", 1, 5))
            {
                case 1: EmitirPoliza(); break;
                case 2: ConsultarPoliza(); break;
                case 3: ModificarPoliza(); break;
                case 4: EliminarPoliza(); break;
                case 5: Ja_volver = true; break;
            }
        }
    }

    static void MenuSiniestros()
    {
        bool Ja_volver = false;
        while (!Ja_volver)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("=============MENU SINIESTROS============");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Registrar Siniestro");
            Console.WriteLine("2. Consultar Siniestro");
            Console.WriteLine("3. Modificar Siniestro");
            Console.WriteLine("4. Eliminar Siniestro");
            Console.WriteLine("5. Volver al menú principal");
            switch (LeerOpcionMenu("", 1, 5))
            {
                case 1: RegistarSiniestro(); break;
                case 2: ConsultarSiniestro(); break;
                case 3: ModificarSiniestro(); break;
                case 4: EliminarSiniestro(); break;
                case 5: Ja_volver = true; break;
            }
        }
    }

    static void MenuReaseguro()
    {
        bool Ja_volver = false;
        while (!Ja_volver)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("=========== MENU REASEGUROS ============");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Consultar Reaseguro");
            Console.WriteLine("2. Volver");
            int Ja_opcion = LeerOpcionMenu("", 1, 2);
            if (Ja_opcion == 1) ConsultarReaseguro();
            else Ja_volver = true;
        }
    }

    static int LeerOpcionMenu(string Ja_mensaje, int Ja_minimo, int Ja_maximo)
    {
        int Ja_valor;
        do
        {
            if (Ja_mensaje != "") Console.WriteLine(Ja_mensaje);
            if (int.TryParse(Console.ReadLine(), out Ja_valor) && Ja_valor >= Ja_minimo && Ja_valor <= Ja_maximo)
                return Ja_valor;
            MostrarError("Opción incorrecta, intente nuevamente.");
        } while (true);
    }

    static double LeerDouble(string Ja_mensaje, double Ja_minimo, double Ja_maximo)
    {
        double Ja_valor;
        do
        {
            Console.WriteLine(Ja_mensaje);
            if (double.TryParse(Console.ReadLine(), out Ja_valor) && Ja_valor >= Ja_minimo && Ja_valor <= Ja_maximo)
                return Ja_valor;
            MostrarError("Valor incorrecto, intente nuevamente.");
        } while (true);
    }

    static bool Confirmar(string Ja_mensaje)
    {
        while (true)
        {
            Console.WriteLine(Ja_mensaje);
            string? Ja_respuesta = Console.ReadLine();
            if (Ja_respuesta == "S" || Ja_respuesta == "s") return true;
            if (Ja_respuesta == "N" || Ja_respuesta == "n") return false;
            MostrarError("Respuesta inválida.");
        }
    }

    static void MostrarError(string Ja_mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(Ja_mensaje);
        Console.ResetColor();
    }

    static void MostrarAdvertencia(string Ja_mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(Ja_mensaje);
        Console.ResetColor();
    }

    static void MostrarExito(string Ja_mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(Ja_mensaje);
        Console.ResetColor();
    }

    static bool CedulaValida(string? Ja_cedula)
    {
        if (Ja_cedula == null || Ja_cedula.Length != 10) return false;
        for (int Ja_i = 0; Ja_i < Ja_cedula.Length; Ja_i++)
            if (Ja_cedula[Ja_i] < '0' || Ja_cedula[Ja_i] > '9') return false;
        return true;
    }

    static Cliente? SolicitarClientePorCedula(string Ja_mensaje)
    {
        Console.WriteLine(Ja_mensaje);
        string? Ja_cedula = Console.ReadLine();
        if (!CedulaValida(Ja_cedula))
        {
            MostrarError("Cédula inválida.");
            return null;
        }
        foreach (Cliente Ja_cliente in Ja_ListaClientes)
            if (Ja_cliente.Cedula == Ja_cedula) return Ja_cliente;
        MostrarError("Cliente no encontrado.");
        return null;
    }

    static Cliente? BuscarCliente(int Ja_id)
    {
        foreach (Cliente Ja_cliente in Ja_ListaClientes)
            if (Ja_cliente.IdCliente == Ja_id) return Ja_cliente;
        return null;
    }

    static Ramo? BuscarRamo(int Ja_id)
    {
        foreach (Ramo Ja_ramo in Ja_ListaRamos)
            if (Ja_ramo.IdRamo == Ja_id) return Ja_ramo;
        return null;
    }

    static Poliza? BuscarPoliza(int Ja_id) { return Ja_polizaRam.BuscarPorId(Ja_ListaPolizas, Ja_id); }

    static bool TieneAlerta(Cliente Ja_cliente, int Ja_codigo)
    {
        foreach (int Ja_alerta in Ja_cliente.AlertasUAF)
            if (Ja_alerta == Ja_codigo) return true;
        return false;
    }

    static List<Poliza> PolizasDelCliente(Cliente Ja_cliente, bool Ja_soloActivas)
    {
        List<Poliza> Ja_lista = new List<Poliza>();
        foreach (Poliza Ja_poliza in Ja_ListaPolizas)
            if (Ja_poliza.IdCliente == Ja_cliente.IdCliente && (!Ja_soloActivas || Ja_poliza.Estado == "Activa"))
                Ja_lista.Add(Ja_poliza);
        return Ja_lista;
    }

    static Poliza? SeleccionarPoliza(Cliente Ja_cliente, bool Ja_soloActivas)
    {
        List<Poliza> Ja_lista = PolizasDelCliente(Ja_cliente, Ja_soloActivas);
        if (Ja_lista.Count == 0)
        {
            MostrarAdvertencia("El cliente no tiene pólizas registradas.");
            return null;
        }
        if (Ja_lista.Count == 1) return Ja_lista[0];

        Console.WriteLine("PÓLIZAS DEL CLIENTE");
        for (int Ja_i = 0; Ja_i < Ja_lista.Count; Ja_i++)
            Console.WriteLine($"{Ja_i + 1}. Id: {Ja_lista[Ja_i].IdPoliza} | Capital: ${Ja_lista[Ja_i].CapitalAsegurado:F2} | Estado: {Ja_lista[Ja_i].Estado}");
        int Ja_opcion = LeerOpcionMenu("Seleccione una póliza:", 1, Ja_lista.Count);
        return Ja_lista[Ja_opcion - 1];
    }

    static void EmitirPoliza()
    {
        Cliente? Ja_cliente = SolicitarClientePorCedula("Ingrese la cédula del cliente:");
        if (Ja_cliente == null) return;
        if (TieneAlerta(Ja_cliente, 999))
        {
            MostrarError("Cliente bloqueado por alerta UAF 999.");
            RegistrarLog("Poliza", "Advertencia", "Emisión bloqueada para " + Ja_cliente.Cedula);
            return;
        }
        if (TieneAlerta(Ja_cliente, 404)) MostrarAdvertencia("ADVERTENCIA UAF 404: revisar documentación del cliente.");
        if (Ja_polizaSql.ExistePorCliente(Ja_cliente.IdCliente))
        {
            MostrarAdvertencia("El cliente ya posee una póliza activa.");
            return;
        }

        Console.WriteLine("RAMOS ACTIVOS");
        foreach (Ramo Ja_ramo in Ja_ListaRamos)
            if (Ja_ramo.Activo) Console.WriteLine($"{Ja_ramo.IdRamo}. {Ja_ramo.Nombre} - {Ja_ramo.Descripcion}");
        int Ja_idRamo = LeerOpcionMenu("Seleccione el ramo:", 1, MayorIdRamo());
        Ramo? Ja_ramoSeleccionado = BuscarRamo(Ja_idRamo);
        if (Ja_ramoSeleccionado == null || !Ja_ramoSeleccionado.Activo)
        {
            MostrarError("Ramo inválido o inactivo.");
            return;
        }

        double Ja_capital = LeerDouble("Ingrese el capital asegurado:", 0.01, double.MaxValue);
        double Ja_tasa = LeerDouble("Ingrese la tasa de riesgo (%):", 0.01, 100);
        Poliza Ja_poliza = CrearPoliza(Ja_polizaSql.ObtenerSiguienteId(), Ja_cliente.IdCliente,
            Ja_idRamo, Ja_capital, Ja_capital, Ja_tasa, DateTime.Now, "Activa");
        if (Ja_poliza.IdPoliza <= 0 || !Ja_polizaSql.Insertar(Ja_poliza))
        {
            MostrarError("No fue posible registrar la póliza.");
            return;
        }

        Ja_polizaRam.Agregar(Ja_ListaPolizas, Ja_poliza);
        GuardarReaseguro(Ja_poliza);
        GestionarAsientoEmision(Ja_poliza);
        RegistrarLog("Poliza", "Informacion", "Póliza emitida: " + Ja_poliza.IdPoliza);
        MostrarExito("Póliza emitida correctamente.");
        MostrarPoliza(Ja_poliza);
    }

    static int MayorIdRamo()
    {
        int Ja_mayor = 1;
        foreach (Ramo Ja_ramo in Ja_ListaRamos)
            if (Ja_ramo.IdRamo > Ja_mayor) Ja_mayor = Ja_ramo.IdRamo;
        return Ja_mayor;
    }

    static Poliza CrearPoliza(int Ja_id, int Ja_cliente, int Ja_ramo, double Ja_capital,
        double Ja_remanente, double Ja_tasa, DateTime Ja_fecha, string Ja_estado)
    {
        double Ja_base = 0, Ja_super = 0, Ja_campesino = 0, Ja_derecho = 0;
        double Ja_subtotal = 0, Ja_iva = 0, Ja_total = 0;
        CalculosSeguros.CalcularPrima(Ja_capital, Ja_tasa, ref Ja_base, ref Ja_super,
            ref Ja_campesino, ref Ja_derecho, ref Ja_subtotal, ref Ja_iva, ref Ja_total);
        return new Poliza(Ja_id, Ja_cliente, Ja_ramo, Ja_capital, Ja_remanente, Ja_tasa,
            Ja_base, Ja_super, Ja_campesino, Ja_derecho, Ja_subtotal, Ja_iva, Ja_total, Ja_fecha, Ja_estado);
    }

    static void ConsultarPoliza()
    {
        Cliente? Ja_cliente = SolicitarClientePorCedula("Ingrese la cédula del cliente:");
        if (Ja_cliente == null) return;
        Poliza? Ja_poliza = SeleccionarPoliza(Ja_cliente, false);
        if (Ja_poliza != null) MostrarPoliza(Ja_poliza);
    }

    static void ModificarPoliza()
    {
        Cliente? Ja_cliente = SolicitarClientePorCedula("Ingrese la cédula del cliente:");
        if (Ja_cliente == null) return;
        Poliza? Ja_anterior = SeleccionarPoliza(Ja_cliente, false);
        if (Ja_anterior == null) return;

        Console.WriteLine($"Capital actual: ${Ja_anterior.CapitalAsegurado:F2}");
        Console.WriteLine($"Capital remanente: ${Ja_anterior.CapitalRemanente:F2}");
        double Ja_nuevoCapital = LeerDouble("Ingrese el nuevo capital:", 0.01, double.MaxValue);
        double Ja_nuevaTasa = LeerDouble("Ingrese la nueva tasa (%):", 0.01, 100);
        double Ja_consumido = Ja_anterior.CapitalAsegurado - Ja_anterior.CapitalRemanente;
        if (Ja_nuevoCapital < Ja_consumido)
        {
            MostrarError("El nuevo capital no puede ser menor al capital ya consumido por siniestros.");
            Console.WriteLine($"Capital consumido actualmente: ${Ja_consumido:F2}");
            return;
        }

        Poliza Ja_nueva = CrearPoliza(Ja_anterior.IdPoliza, Ja_anterior.IdCliente, Ja_anterior.IdRamo,
            Ja_nuevoCapital, Ja_nuevoCapital - Ja_consumido, Ja_nuevaTasa,
            Ja_anterior.FechaEmision, Ja_anterior.Estado);
        if (!Ja_polizaSql.Actualizar(Ja_nueva))
        {
            MostrarError("No fue posible modificar la póliza.");
            return;
        }
        Ja_polizaRam.Modificar(Ja_ListaPolizas, Ja_nueva);
        GuardarReaseguro(Ja_nueva);
        GestionarAsientoEmision(Ja_nueva);
        RegistrarLog("Poliza", "Informacion", "Póliza modificada: " + Ja_nueva.IdPoliza);
        MostrarExito("Póliza modificada correctamente.");
        MostrarPoliza(Ja_nueva);
    }

    static void EliminarPoliza()
    {
        Cliente? Ja_cliente = SolicitarClientePorCedula("Ingrese la cédula del cliente:");
        if (Ja_cliente == null) return;
        Poliza? Ja_poliza = SeleccionarPoliza(Ja_cliente, false);
        if (Ja_poliza == null) return;
        if (Ja_polizaSql.TieneSiniestros(Ja_poliza.IdPoliza))
        {
            MostrarError("No se puede eliminar una póliza que tiene siniestros.");
            return;
        }
        if (!Confirmar("¿Desea eliminar la póliza? (S/N)")) return;
        if (!Ja_polizaSql.EliminarCompletaSinSiniestros(Ja_poliza.IdPoliza))
        {
            MostrarError("No fue posible eliminar la póliza.");
            return;
        }

        Ja_polizaRam.Eliminar(Ja_ListaPolizas, Ja_poliza.IdPoliza);
        EliminarReaseguroRam(Ja_poliza.IdPoliza);
        EliminarAsientosPolizaRam(Ja_poliza.IdPoliza);
        RegistrarLog("Poliza", "Informacion", "Póliza eliminada: " + Ja_poliza.IdPoliza);
        MostrarExito("Póliza eliminada correctamente.");
    }

    static void MostrarPoliza(Poliza Ja_p)
    {
        Cliente? Ja_cliente = BuscarCliente(Ja_p.IdCliente);
        Ramo? Ja_ramo = BuscarRamo(Ja_p.IdRamo);
        Console.WriteLine("========================================");
        Console.WriteLine("PÓLIZA / FACTURA");
        Console.WriteLine("Id: " + Ja_p.IdPoliza);
        Console.WriteLine("Cliente: " + (Ja_cliente == null ? "No disponible" : Ja_cliente.Nombre));
        Console.WriteLine("Ramo: " + (Ja_ramo == null ? "No disponible" : Ja_ramo.Nombre));
        Console.WriteLine($"Capital asegurado: ${Ja_p.CapitalAsegurado:F2}");
        Console.WriteLine($"Capital remanente: ${Ja_p.CapitalRemanente:F2}");
        Console.WriteLine($"Tasa: {Ja_p.TasaRiesgo:F2}%");
        Console.WriteLine($"Prima base: ${Ja_p.PrimaBase:F2}");
        Console.WriteLine($"Superintendencia de Bancos: ${Ja_p.SuperBancos:F2}");
        Console.WriteLine($"Seguro Campesino: ${Ja_p.SeguroCampesino:F2}");
        Console.WriteLine($"Derecho de emisión: ${Ja_p.DerechoEmision:F2}");
        Console.WriteLine($"Subtotal: ${Ja_p.Subtotal:F2}");
        Console.WriteLine($"IVA: ${Ja_p.IVA:F2}");
        Console.WriteLine($"Prima total: ${Ja_p.PrimaTotal:F2}");
        Console.WriteLine("Estado: " + Ja_p.Estado);
    }

    static List<Siniestro> SiniestrosDelCliente(Cliente Ja_cliente)
    {
        List<Siniestro> Ja_lista = new List<Siniestro>();
        foreach (Siniestro Ja_siniestro in Ja_ListaSiniestros)
        {
            Poliza? Ja_poliza = BuscarPoliza(Ja_siniestro.IdPoliza);
            if (Ja_poliza != null && Ja_poliza.IdCliente == Ja_cliente.IdCliente) Ja_lista.Add(Ja_siniestro);
        }
        return Ja_lista;
    }

    static Siniestro? SeleccionarSiniestro(Cliente Ja_cliente)
    {
        List<Siniestro> Ja_lista = SiniestrosDelCliente(Ja_cliente);
        if (Ja_lista.Count == 0)
        {
            MostrarAdvertencia("El cliente no tiene siniestros registrados.");
            return null;
        }
        if (Ja_lista.Count == 1) return Ja_lista[0];

        Console.WriteLine("SINIESTROS DEL CLIENTE");
        foreach (Siniestro Ja_s in Ja_lista)
            Console.WriteLine($"IdSiniestro: {Ja_s.IdSiniestro} | MontoReclamo: ${Ja_s.MontoReclamo:F2} | Estado: {Ja_s.Estado} | Fecha: {Ja_s.FechaSiniestro:dd/MM/yyyy}");

        while (true)
        {
            Console.WriteLine("Ingrese el IdSiniestro:");
            if (int.TryParse(Console.ReadLine(), out int Ja_id))
                foreach (Siniestro Ja_s in Ja_lista)
                    if (Ja_s.IdSiniestro == Ja_id) return Ja_s;
            MostrarError("Selección inválida.");
        }
    }

    static void RegistarSiniestro()
    {
        Cliente? Ja_cliente = SolicitarClientePorCedula("Ingrese la cédula del cliente:");
        if (Ja_cliente == null) return;
        Poliza? Ja_poliza = SeleccionarPoliza(Ja_cliente, true);
        if (Ja_poliza == null) return;
        Console.WriteLine($"Capital remanente: ${Ja_poliza.CapitalRemanente:F2}");
        double Ja_monto = LeerDouble("Ingrese el monto del reclamo:", 0.01, double.MaxValue);
        double Ja_porcentaje = LeerDouble("Ingrese el porcentaje de deducible:", 0, 100);
        Siniestro Ja_siniestro = CrearSiniestro(Ja_siniestroSql.ObtenerSiguienteId(), Ja_poliza,
            Ja_monto, Ja_porcentaje, DateTime.Now);
        double Ja_nuevoRemanente = Ja_poliza.CapitalRemanente - Ja_siniestro.CapitalConsumido;
        if (Ja_siniestro.IdSiniestro <= 0 || !Ja_siniestroSql.RegistrarConActualizacionCapital(Ja_siniestro, Ja_nuevoRemanente))
        {
            MostrarError("No fue posible registrar el siniestro.");
            return;
        }

        Ja_siniestroRam.Agregar(Ja_ListaSiniestros, Ja_siniestro);
        Ja_poliza.CapitalRemanente = Ja_nuevoRemanente;
        if (Ja_siniestro.Estado == "Aprobado") GestionarAsientoSiniestro(Ja_siniestro);
        RegistrarLog("Siniestro", "Informacion", "Siniestro registrado: " + Ja_siniestro.IdSiniestro);
        MostrarExito("Siniestro registrado correctamente.");
        MostrarSiniestro(Ja_siniestro);
    }

    static Siniestro CrearSiniestro(int Ja_id, Poliza Ja_poliza, double Ja_monto,
        double Ja_porcentaje, DateTime Ja_fecha)
    {
        double Ja_deducible = 0, Ja_pago = 0, Ja_consumido = 0;
        string Ja_estado = "Rechazado";
        CalculosSeguros.CalcularSiniestro(Ja_monto, Ja_porcentaje, Ja_poliza.CapitalRemanente,
            ref Ja_deducible, ref Ja_pago, ref Ja_consumido, ref Ja_estado);
        string Ja_observacion = Ja_estado == "Aprobado"
            ? "Reclamo aprobado." : "Reclamo superior al capital remanente.";
        return new Siniestro(Ja_id, Ja_poliza.IdPoliza, Ja_monto, Ja_porcentaje,
            Ja_deducible, Ja_pago, Ja_consumido, Ja_fecha, Ja_estado, Ja_observacion);
    }

    static void ConsultarSiniestro()
    {
        Cliente? Ja_cliente = SolicitarClientePorCedula("Ingrese la cédula del cliente:");
        if (Ja_cliente == null) return;
        Siniestro? Ja_siniestro = SeleccionarSiniestro(Ja_cliente);
        if (Ja_siniestro != null) MostrarSiniestro(Ja_siniestro);
    }

    static void ModificarSiniestro()
    {
        Cliente? Ja_cliente = SolicitarClientePorCedula("Ingrese la cédula del cliente:");
        if (Ja_cliente == null) return;
        Siniestro? Ja_anterior = SeleccionarSiniestro(Ja_cliente);
        if (Ja_anterior == null) return;
        Poliza? Ja_poliza = BuscarPoliza(Ja_anterior.IdPoliza);
        if (Ja_poliza == null) return;

        double Ja_disponible = Ja_poliza.CapitalRemanente + Ja_anterior.CapitalConsumido;
        Console.WriteLine($"Capital disponible: ${Ja_disponible:F2}");
        double Ja_monto = LeerDouble("Ingrese el nuevo monto:", 0.01, double.MaxValue);
        double Ja_porcentaje = LeerDouble("Ingrese el nuevo deducible (%):", 0, 100);
        Poliza Ja_temporal = CrearPoliza(Ja_poliza.IdPoliza, Ja_poliza.IdCliente, Ja_poliza.IdRamo,
            Ja_poliza.CapitalAsegurado, Ja_disponible, Ja_poliza.TasaRiesgo,
            Ja_poliza.FechaEmision, Ja_poliza.Estado);
        Siniestro Ja_nuevo = CrearSiniestro(Ja_anterior.IdSiniestro, Ja_temporal,
            Ja_monto, Ja_porcentaje, Ja_anterior.FechaSiniestro);
        double Ja_nuevoRemanente = Ja_disponible - Ja_nuevo.CapitalConsumido;
        if (!Ja_siniestroSql.ActualizarConCapital(Ja_nuevo, Ja_nuevoRemanente))
        {
            MostrarError("No fue posible modificar el siniestro.");
            return;
        }

        Ja_siniestroRam.Modificar(Ja_ListaSiniestros, Ja_nuevo);
        Ja_poliza.CapitalRemanente = Ja_nuevoRemanente;
        GestionarAsientoSiniestro(Ja_nuevo);
        RegistrarLog("Siniestro", "Informacion", "Siniestro modificado: " + Ja_nuevo.IdSiniestro);
        MostrarExito("Siniestro modificado correctamente.");
        MostrarSiniestro(Ja_nuevo);
    }

    static void EliminarSiniestro()
    {
        Cliente? Ja_cliente = SolicitarClientePorCedula("Ingrese la cédula del cliente:");
        if (Ja_cliente == null) return;
        Siniestro? Ja_siniestro = SeleccionarSiniestro(Ja_cliente);
        if (Ja_siniestro == null || !Confirmar("¿Desea eliminar el siniestro? (S/N)")) return;
        Poliza? Ja_poliza = BuscarPoliza(Ja_siniestro.IdPoliza);
        if (!Ja_siniestroSql.EliminarConDevolucionCapital(Ja_siniestro.IdSiniestro))
        {
            MostrarError("No fue posible eliminar el siniestro.");
            return;
        }

        if (Ja_poliza != null) Ja_poliza.CapitalRemanente += Ja_siniestro.CapitalConsumido;
        Ja_siniestroRam.Eliminar(Ja_ListaSiniestros, Ja_siniestro.IdSiniestro);
        EliminarAsientoSiniestroRam(Ja_siniestro.IdSiniestro);
        RegistrarLog("Siniestro", "Informacion", "Siniestro eliminado: " + Ja_siniestro.IdSiniestro);
        MostrarExito("Siniestro eliminado correctamente.");
    }

    static void MostrarSiniestro(Siniestro Ja_s)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("DETALLE DEL SINIESTRO");
        Console.WriteLine("Id: " + Ja_s.IdSiniestro);
        Console.WriteLine("Id póliza: " + Ja_s.IdPoliza);
        Console.WriteLine($"Daño reclamado: ${Ja_s.MontoReclamo:F2}");
        Console.WriteLine($"A cargo del cliente: ${Ja_s.ValorDeducible:F2}");
        Console.WriteLine($"Cubierto por aseguradora: ${Ja_s.PagoNeto:F2}");
        Console.WriteLine($"Capital consumido: ${Ja_s.CapitalConsumido:F2}");
        Console.WriteLine("Estado: " + Ja_s.Estado);
        Console.WriteLine("Observación: " + Ja_s.Observacion);
    }

    static void GuardarReaseguro(Poliza Ja_poliza)
    {
        double Ja_retencion = 0, Ja_contrato = 0, Ja_facultativo = 0;
        CalculosSeguros.CalcularReaseguro(Ja_poliza.CapitalAsegurado,
            ref Ja_retencion, ref Ja_contrato, ref Ja_facultativo);
        Reaseguro? Ja_reaseguro = Ja_reaseguroRam.BuscarPorPoliza(Ja_ListaReaseguros, Ja_poliza.IdPoliza);
        bool Ja_nuevo = Ja_reaseguro == null;
        if (Ja_reaseguro == null)
        {
            int Ja_id = Ja_reaseguroSql.ObtenerSiguienteId();
            if (Ja_id <= 0) return;
            Ja_reaseguro = new Reaseguro();
            Ja_reaseguro.IdReaseguro = Ja_id;
            Ja_reaseguro.IdPoliza = Ja_poliza.IdPoliza;
        }

        Ja_reaseguro.MontoRetencion = Ja_retencion;
        Ja_reaseguro.MontoContrato = Ja_contrato;
        Ja_reaseguro.MontoFacultativo = Ja_facultativo;
        Ja_reaseguro.TotalRepartido = Ja_poliza.CapitalAsegurado;
        Ja_reaseguro.IndiceRetencion = Ja_retencion > 0 ? 6 : -1;
        Ja_reaseguro.IndiceContrato = Ja_contrato > 0 ? 3 : -1;
        Ja_reaseguro.IndiceFacultativo = Ja_facultativo > 0 ? 1 : -1;
        Ja_reaseguro.FechaGeneracion = DateTime.Now;
        Ja_reaseguro.Generado = true;

        bool Ja_guardado = Ja_nuevo ? Ja_reaseguroSql.Insertar(Ja_reaseguro) : Ja_reaseguroSql.Actualizar(Ja_reaseguro);
        if (!Ja_guardado)
        {
            MostrarAdvertencia("No fue posible guardar el reaseguro.");
            return;
        }
        if (Ja_nuevo) Ja_reaseguroRam.Agregar(Ja_ListaReaseguros, Ja_reaseguro);
        else Ja_reaseguroRam.Modificar(Ja_ListaReaseguros, Ja_reaseguro);
        GestionarAsientoReaseguro(Ja_reaseguro);
        RegistrarLog("Reaseguro", "Informacion", "Reaseguro actualizado para póliza " + Ja_poliza.IdPoliza);
    }

    static void ConsultarReaseguro()
    {
        Cliente? Ja_cliente = SolicitarClientePorCedula("Ingrese la cédula del cliente:");
        if (Ja_cliente == null) return;
        Poliza? Ja_poliza = SeleccionarPoliza(Ja_cliente, false);
        if (Ja_poliza == null) return;
        Reaseguro? Ja_reaseguro = Ja_reaseguroRam.BuscarPorPoliza(Ja_ListaReaseguros, Ja_poliza.IdPoliza);
        if (Ja_reaseguro == null)
        {
            MostrarAdvertencia("No existe reaseguro para la póliza.");
            return;
        }
        Console.WriteLine("========================================");
        Console.WriteLine("REPARTO DE REASEGURO");
        Console.WriteLine("Id póliza: " + Ja_poliza.IdPoliza);
        Console.WriteLine($"Capital: ${Ja_poliza.CapitalAsegurado:F2}");
        Console.WriteLine($"Retención: ${Ja_reaseguro.MontoRetencion:F2}");
        Console.WriteLine($"Contrato (Hispana Re): ${Ja_reaseguro.MontoContrato:F2}");
        Console.WriteLine($"Facultativo (Equinoccial Seguros): ${Ja_reaseguro.MontoFacultativo:F2}");
        Console.WriteLine($"Total repartido: ${Ja_reaseguro.TotalRepartido:F2}");
    }

    static void GestionarAsientoEmision(Poliza Ja_poliza)
    {
        GuardarAsiento("Emision", "Cuentas por Cobrar", "Ingresos por primas",
            Ja_poliza.PrimaTotal, Ja_poliza.IdPoliza, -1, "Emisión de póliza " + Ja_poliza.IdPoliza);
    }

    static void GestionarAsientoSiniestro(Siniestro Ja_siniestro)
    {
        if (Ja_siniestro.Estado == "Aprobado")
        {
            GuardarAsiento("Siniestro", "Gastos por siniestros", "Caja/Bancos", Ja_siniestro.PagoNeto,
                Ja_siniestro.IdPoliza, Ja_siniestro.IdSiniestro, "Pago de siniestro " + Ja_siniestro.IdSiniestro);
        }
        else
        {
            AsientoContable? Ja_existente = Ja_asientoSql.BuscarPagoPorSiniestro(Ja_siniestro.IdSiniestro);
            if (Ja_existente != null)
            {
                Ja_existente.Valor = 0;
                Ja_existente.Estado = "Anulado";
                if (Ja_asientoSql.Actualizar(Ja_existente)) Ja_asientoRam.Modificar(Ja_ListaAsientos, Ja_existente);
            }
        }
    }

    static void GestionarAsientoReaseguro(Reaseguro Ja_reaseguro)
    {
        double Ja_valor = Ja_reaseguro.MontoContrato + Ja_reaseguro.MontoFacultativo;
        AsientoContable? Ja_existente = Ja_asientoSql.BuscarPorPoliza(Ja_reaseguro.IdPoliza, "Reaseguro");
        if (Ja_valor > 0)
        {
            GuardarAsiento("Reaseguro", "Gastos por reaseguro", "Reaseguradores por pagar",
                Ja_valor, Ja_reaseguro.IdPoliza, -1, "Cesión de reaseguro");
        }
        else if (Ja_existente != null)
        {
            Ja_existente.Valor = 0;
            Ja_existente.Estado = "Anulado";
            if (Ja_asientoSql.Actualizar(Ja_existente)) Ja_asientoRam.Modificar(Ja_ListaAsientos, Ja_existente);
        }
    }

    static void GuardarAsiento(string Ja_tipo, string Ja_debe, string Ja_haber,
        double Ja_valor, int Ja_idPoliza, int Ja_idSiniestro, string Ja_descripcion)
    {
        AsientoContable? Ja_asiento = Ja_idSiniestro > 0
            ? Ja_asientoSql.BuscarPagoPorSiniestro(Ja_idSiniestro)
            : Ja_asientoSql.BuscarPorPoliza(Ja_idPoliza, Ja_tipo);
        bool Ja_nuevo = Ja_asiento == null;
        if (Ja_asiento == null)
        {
            int Ja_id = Ja_asientoSql.ObtenerSiguienteId();
            if (Ja_id <= 0) return;
            Ja_asiento = new AsientoContable();
            Ja_asiento.IdAsiento = Ja_id;
        }
        Ja_asiento.TipoOperacion = Ja_tipo;
        Ja_asiento.CuentaDebe = Ja_debe;
        Ja_asiento.CuentaHaber = Ja_haber;
        Ja_asiento.Valor = Ja_valor;
        Ja_asiento.Fecha = DateTime.Now;
        Ja_asiento.IdPoliza = Ja_idPoliza;
        Ja_asiento.IdSiniestro = Ja_idSiniestro;
        Ja_asiento.Descripcion = Ja_descripcion;
        Ja_asiento.Estado = "Registrado";
        bool Ja_ok = Ja_nuevo ? Ja_asientoSql.Insertar(Ja_asiento) : Ja_asientoSql.Actualizar(Ja_asiento);
        if (Ja_ok)
        {
            if (Ja_nuevo) Ja_asientoRam.Agregar(Ja_ListaAsientos, Ja_asiento);
            else Ja_asientoRam.Modificar(Ja_ListaAsientos, Ja_asiento);
        }
    }

    static void MenuContabilidad()
    {
        bool Ja_volver = false;
        while (!Ja_volver)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("=========== MENU CONTABILIDAD ==========");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Listar todos los asientos");
            Console.WriteLine("2. Mostrar total Debe y Haber");
            Console.WriteLine("3. Mostrar total de primas recaudadas");
            Console.WriteLine("4. Volver");
            switch (LeerOpcionMenu("", 1, 4))
            {
                case 1: ListarAsientos(); break;
                case 2: MostrarTotales(); break;
                case 3: MostrarPrimas(); break;
                case 4: Ja_volver = true; break;
            }
        }
    }

    static void ListarAsientos()
    {
        if (Ja_ListaAsientos.Count == 0)
        {
            MostrarAdvertencia("No existen asientos contables.");
            return;
        }
        foreach (AsientoContable Ja_a in Ja_ListaAsientos)
            Console.WriteLine($"{Ja_a.IdAsiento} | {Ja_a.Fecha:dd/MM/yyyy HH:mm} | {Ja_a.TipoOperacion} | Debe: {Ja_a.CuentaDebe} | Haber: {Ja_a.CuentaHaber} | ${Ja_a.Valor:F2} | {Ja_a.Estado}");
    }

    static void MostrarTotales()
    {
        double Ja_total = 0;
        foreach (AsientoContable Ja_a in Ja_ListaAsientos)
            if (Ja_a.Estado == "Registrado") Ja_total += Ja_a.Valor;
        Console.WriteLine($"Total Debe: ${Ja_total:F2}");
        Console.WriteLine($"Total Haber: ${Ja_total:F2}");
    }

    static void MostrarPrimas()
    {
        double Ja_total = 0;
        foreach (AsientoContable Ja_a in Ja_ListaAsientos)
            if (Ja_a.TipoOperacion == "Emision" && Ja_a.Estado == "Registrado") Ja_total += Ja_a.Valor;
        Console.WriteLine($"Total de primas recaudadas: ${Ja_total:F2}");
    }

    static void MenuLogs()
    {
        bool Ja_volver = false;
        while (!Ja_volver)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("=============== MENU LOGS ==============");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Mostrar todos");
            Console.WriteLine("2. Volver");
            int Ja_opcion = LeerOpcionMenu("", 1, 2);
            if (Ja_opcion == 1) ConsultarLogs();
            else Ja_volver = true;
        }
    }

    static void RegistrarLog(string Ja_modulo, string Ja_tipo, string Ja_mensaje)
    {
        int Ja_id = Ja_logSql.ObtenerSiguienteId();
        LogSistema Ja_log = new LogSistema(Ja_id, DateTime.Now, Ja_modulo, Ja_tipo, Ja_mensaje, "Sistema");
        if (Ja_id > 0 && Ja_logSql.Insertar(Ja_log)) Ja_logRam.Agregar(Ja_ListaLogs, Ja_log);
        PersistenciaArchivos.GuardarLogAuditoria(Ja_log);
    }

    static void ConsultarLogs()
    {
        if (Ja_ListaLogs.Count == 0)
        {
            MostrarAdvertencia("No existen logs registrados.");
            return;
        }
        foreach (LogSistema Ja_log in Ja_ListaLogs)
        {
            string Ja_modulo = Ja_log.Modulo == "Poliza" ? "Póliza" : Ja_log.Modulo;
            string Ja_tipo = Ja_log.Tipo == "Informacion" ? "Información" : Ja_log.Tipo;
            Console.WriteLine($"{Ja_log.IdLog} | {Ja_log.Fecha:dd/MM/yyyy HH:mm:ss} | {Ja_modulo} | {Ja_tipo} | {Ja_log.Mensaje} | {Ja_log.Usuario}");
        }
    }

    static void EliminarReaseguroRam(int Ja_idPoliza)
    {
        for (int Ja_i = Ja_ListaReaseguros.Count - 1; Ja_i >= 0; Ja_i--)
            if (Ja_ListaReaseguros[Ja_i].IdPoliza == Ja_idPoliza) Ja_ListaReaseguros.RemoveAt(Ja_i);
    }

    static void EliminarAsientosPolizaRam(int Ja_idPoliza)
    {
        for (int Ja_i = Ja_ListaAsientos.Count - 1; Ja_i >= 0; Ja_i--)
            if (Ja_ListaAsientos[Ja_i].IdPoliza == Ja_idPoliza) Ja_ListaAsientos.RemoveAt(Ja_i);
    }

    static void EliminarAsientoSiniestroRam(int Ja_idSiniestro)
    {
        for (int Ja_i = Ja_ListaAsientos.Count - 1; Ja_i >= 0; Ja_i--)
            if (Ja_ListaAsientos[Ja_i].IdSiniestro == Ja_idSiniestro) Ja_ListaAsientos.RemoveAt(Ja_i);
    }
}
