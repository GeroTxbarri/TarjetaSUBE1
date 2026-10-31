public class EstacionBici
{
    private decimal Tarifa = 2418.75m;
    private decimal Multa = 1250;
    private int MinutosMaximos = 60;

    public bool retirarBici(Tarjeta tarjeta)
    {
        return retirarBici(tarjeta, DateTime.Now);
    }

    public bool retirarBici(Tarjeta tarjeta, DateTime fecha)
    {
        if (tarjeta.InicioUsoBici != null)
        {
            return false;
        }

        if (tarjeta.UltimoPagoBici.Date != fecha.Date)
        {
            decimal total = Tarifa + Multa * tarjeta.MultasBici;
            if (!tarjeta.descontarSaldo(total))
            {
                return false;
            }
            tarjeta.MultasBici = 0;
            tarjeta.UltimoPagoBici = fecha;
        }

        tarjeta.InicioUsoBici = fecha;
        return true;
    }

    public void devolverBici(Tarjeta tarjeta)
    {
        devolverBici(tarjeta, DateTime.Now);
    }

    public void devolverBici(Tarjeta tarjeta, DateTime fecha)
    {
        if (tarjeta.InicioUsoBici == null)
        {
            return;
        }

        if (fecha > tarjeta.InicioUsoBici.Value.AddMinutes(MinutosMaximos))
        {
            tarjeta.MultasBici++;
        }

        tarjeta.InicioUsoBici = null;
    }
}
