static class ConexionBaseDatos
{
    // Guarda el nombre del servidor SQL que utiliza el sistema.
    public static string Ja_servidor = @"xJors";

    // Devuelve la cadena de conexión para acceder a la base de datos.
    public static string ObtenerCadena()
    {
        return
            "Server=" + Ja_servidor +
            ";Database=SistemaSegurosDB;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;";
    }
}
