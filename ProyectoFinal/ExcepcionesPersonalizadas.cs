using System;

// Se usa cuando ocurre un problema al leer o guardar archivos.
class ArchivoDatosException : Exception
{
    public ArchivoDatosException(string Ja_mensaje)
        : base(Ja_mensaje)
    {
    }

    public ArchivoDatosException(string Ja_mensaje, Exception Ja_excepcionInterna)
        : base(Ja_mensaje, Ja_excepcionInterna)
    {
    }
}

// Se usa cuando los datos de un cliente no son válidos.
class ClienteInvalidoException : Exception
{
    public ClienteInvalidoException(string Ja_mensaje)
        : base(Ja_mensaje)
    {
    }

}

// Se usa cuando los datos de un ramo no son válidos.
class RamoInvalidoException : Exception
{
    public RamoInvalidoException(string Ja_mensaje)
        : base(Ja_mensaje)
    {
    }

}
