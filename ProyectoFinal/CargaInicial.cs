using System;
using System.Collections.Generic;

class CargaInicial
{
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
