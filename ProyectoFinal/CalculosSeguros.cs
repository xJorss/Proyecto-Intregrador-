using System;

class CalculosSeguros
{
    // Calcula la prima base, cargos adicionales, IVA y prima total.
    public static void CalcularPrima(double Ja_capital, double Ja_tasa,
        ref double Ja_primaBase, ref double Ja_superBancos, ref double Ja_seguroCampesino,
        ref double Ja_derechoEmision, ref double Ja_subtotal, ref double Ja_iva, ref double Ja_total)
    {
        Ja_primaBase = Ja_capital * Ja_tasa / 100;
        Ja_superBancos = Ja_primaBase * 0.035;
        Ja_seguroCampesino = Ja_primaBase * 0.005;
        Ja_derechoEmision = Ja_capital <= 10000 ? 0.50 : Ja_capital <= 40000 ? 1.00 : 2.00;
        Ja_subtotal = Ja_primaBase + Ja_superBancos + Ja_seguroCampesino + Ja_derechoEmision;
        Ja_iva = Ja_subtotal * 0.12;
        Ja_total = Ja_subtotal + Ja_iva;
    }

    // Divide el capital entre retención, contrato y facultativo.
    public static void CalcularReaseguro(double Ja_capital, ref double Ja_retencion,
        ref double Ja_contrato, ref double Ja_facultativo)
    {
        Ja_retencion = Math.Min(Ja_capital, 500000);
        double Ja_excedente = Ja_capital - Ja_retencion;
        Ja_contrato = Math.Min(Ja_excedente, 50000);
        Ja_facultativo = Math.Max(0, Ja_excedente - Ja_contrato);
    }

    // Calcula el deducible, el pago neto y el estado del siniestro.
    public static void CalcularSiniestro(double Ja_monto, double Ja_porcentaje,
        double Ja_capitalRemanente, ref double Ja_deducible, ref double Ja_pago,
        ref double Ja_consumido, ref string Ja_estado)
    {
        if (Ja_monto <= Ja_capitalRemanente)
        {
            Ja_deducible = Ja_monto * Ja_porcentaje / 100;
            Ja_pago = Ja_monto - Ja_deducible;
            Ja_consumido = Ja_monto;
            Ja_estado = "Aprobado";
        }
        else
        {
            Ja_deducible = 0;
            Ja_pago = 0;
            Ja_consumido = 0;
            Ja_estado = "Rechazado";
        }
    }
}
