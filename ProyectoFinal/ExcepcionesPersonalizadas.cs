using System;

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

class ClienteInvalidoException : Exception
{
    public ClienteInvalidoException(string Ja_mensaje)
        : base(Ja_mensaje)
    {
    }

}

class RamoInvalidoException : Exception
{
    public RamoInvalidoException(string Ja_mensaje)
        : base(Ja_mensaje)
    {
    }

}
