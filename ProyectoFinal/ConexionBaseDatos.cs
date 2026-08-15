static class ConexionBaseDatos
{
    public static string Ja_servidor = @"xJors";

    public static string ObtenerCadena()
    {
        return
            "Server=" + Ja_servidor +
            ";Database=SistemaSegurosDB;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;";
    }
}
