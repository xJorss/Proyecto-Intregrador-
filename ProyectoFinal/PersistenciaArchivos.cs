using System;
using System.Collections.Generic;
using System.IO;

class PersistenciaArchivos
{
    private static string Ruta(string Ja_archivo)
    {
        return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Datos", Ja_archivo));
    }

    public static List<Cliente> LeerClientes()
    {
        List<Cliente> Ja_clientes = new List<Cliente>();
        string Ja_ruta = Ruta("clientes.csv");

        try
        {
            using (StreamReader Ja_lector = new StreamReader(Ja_ruta))
            {
                string? Ja_linea;
                while ((Ja_linea = Ja_lector.ReadLine()) != null)
                {
                    if (Ja_linea.Trim() == "") continue;
                    string[] Ja_partes = Ja_linea.Split(';');
                    if (Ja_partes.Length != 4 || !int.TryParse(Ja_partes[0], out int Ja_id))
                        throw new ClienteInvalidoException("Registro de cliente inválido: " + Ja_linea);

                    string[] Ja_alertasTexto = Ja_partes[3].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    int[] Ja_alertas = new int[Ja_alertasTexto.Length];
                    for (int Ja_i = 0; Ja_i < Ja_alertasTexto.Length; Ja_i++)
                    {
                        if (!int.TryParse(Ja_alertasTexto[Ja_i], out Ja_alertas[Ja_i]))
                            throw new ClienteInvalidoException("Alerta UAF inválida: " + Ja_linea);
                    }
                    Cliente Ja_cliente = new Cliente(Ja_id, Ja_partes[1].Trim(), Ja_partes[2].Trim(), Ja_alertas);
                    if (Ja_cliente.Cedula == "" || Ja_cliente.Nombre == "")
                        throw new ClienteInvalidoException("Datos de cliente inválidos: " + Ja_linea);
                    Ja_clientes.Add(Ja_cliente);
                }
            }
            return Ja_clientes;
        }
        catch (Exception Ja_ex) when (Ja_ex is IOException || Ja_ex is ClienteInvalidoException)
        {
            throw new ArchivoDatosException("No se pudo leer clientes.csv.", Ja_ex);
        }
    }

    public static List<Ramo> LeerRamos()
    {
        List<Ramo> Ja_ramos = new List<Ramo>();
        string Ja_ruta = Ruta("ramos.csv");

        try
        {
            using (StreamReader Ja_lector = new StreamReader(Ja_ruta))
            {
                string? Ja_linea;
                while ((Ja_linea = Ja_lector.ReadLine()) != null)
                {
                    if (Ja_linea.Trim() == "") continue;
                    string[] Ja_partes = Ja_linea.Split(';');
                    if (Ja_partes.Length != 4 || !int.TryParse(Ja_partes[0], out int Ja_id) ||
                        !bool.TryParse(Ja_partes[3], out bool Ja_activo))
                        throw new RamoInvalidoException("Registro de ramo inválido: " + Ja_linea);

                    Ramo Ja_ramo = new Ramo(Ja_id, Ja_partes[1].Trim(), Ja_partes[2].Trim(), Ja_activo);
                    if (Ja_ramo.Nombre == "")
                        throw new RamoInvalidoException("Datos de ramo inválidos: " + Ja_linea);
                    Ja_ramos.Add(Ja_ramo);
                }
            }
            return Ja_ramos;
        }
        catch (Exception Ja_ex) when (Ja_ex is IOException || Ja_ex is RamoInvalidoException)
        {
            throw new ArchivoDatosException("No se pudo leer ramos.csv.", Ja_ex);
        }
    }

    public static bool GuardarClientes(List<Cliente> Ja_clientes)
    {
        try
        {
            using (StreamWriter Ja_escritor = new StreamWriter(Ruta("clientes.csv"), false))
            {
                foreach (Cliente Ja_cliente in Ja_clientes)
                {
                    string Ja_alertas = string.Join(",", Ja_cliente.AlertasUAF);
                    Ja_escritor.WriteLine($"{Ja_cliente.IdCliente};{Ja_cliente.Cedula};{Ja_cliente.Nombre};{Ja_alertas}");
                }
            }
            return true;
        }
        catch (IOException Ja_ex)
        {
            Console.WriteLine("ERROR al guardar clientes: " + Ja_ex.Message);
            return false;
        }
    }

    public static bool GuardarRamos(List<Ramo> Ja_ramos)
    {
        try
        {
            using (StreamWriter Ja_escritor = new StreamWriter(Ruta("ramos.csv"), false))
            {
                foreach (Ramo Ja_ramo in Ja_ramos)
                    Ja_escritor.WriteLine($"{Ja_ramo.IdRamo};{Ja_ramo.Nombre};{Ja_ramo.Descripcion};{Ja_ramo.Activo.ToString().ToLower()}");
            }
            return true;
        }
        catch (IOException Ja_ex)
        {
            Console.WriteLine("ERROR al guardar ramos: " + Ja_ex.Message);
            return false;
        }
    }

    public static bool GuardarLogAuditoria(LogSistema Ja_log)
    {
        try
        {
            using (StreamWriter Ja_escritor = new StreamWriter(Ruta("auditoria.txt"), true))
            {
                Ja_escritor.WriteLine($"{Ja_log.Fecha:yyyy-MM-dd HH:mm:ss};{Ja_log.Modulo};{Ja_log.Tipo};{Ja_log.Mensaje};{Ja_log.Usuario}");
            }
            return true;
        }
        catch (IOException)
        {
            return false;
        }
    }
}
