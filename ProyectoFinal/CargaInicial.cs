using System;
using System.Collections.Generic;

class CargaInicial
{
    // Carga los clientes desde el archivo y devuelve una lista.
    public static List<Cliente> CargarClientes()
    {
        try
        {
            return PersistenciaArchivos.LeerClientes();
        }
        catch (ArchivoDatosException Ja_ex)
        {
            Console.WriteLine("ERROR al cargar clientes: " + Ja_ex.Message);
            return new List<Cliente>();
        }
    }

    // Carga los ramos desde el archivo y devuelve una lista.
    public static List<Ramo> CargarRamos()
    {
        try
        {
            return PersistenciaArchivos.LeerRamos();
        }
        catch (ArchivoDatosException Ja_ex)
        {
            Console.WriteLine("ERROR al cargar ramos: " + Ja_ex.Message);
            return new List<Ramo>();
        }
    }
}
