public class EstacionBici
{
    private decimal Tarifa = 2418.75m;
    private decimal Multa = 1250;
    private int MinutosMaximos = 60;
    private IProveedorDeFecha Reloj = new ProveedorDeFechaSistema();

    public EstacionBici() { }

    public EstacionBici(IProveedorDeFecha reloj)
    {
        Reloj = reloj;
    }

    public bool retirarBici(Tarjeta tarjeta)
    {
        return retirarBici(tarjeta, Reloj.Ahora);
    }

        public bool retirarBici(Tarjeta tarjeta, DateTime fecha)
    {
        if (tarjeta.InicioUsoBici != null)
        {
            return false;
        }

        decimal aCobrar = 0;
        bool esNuevoDia = tarjeta.UltimoPagoBici.Date != fecha.Date;

        if (esNuevoDia)
        {
            aCobrar += Tarifa;
        }
        if (tarjeta.MultasBici > 0)
        {
            aCobrar += Multa * tarjeta.MultasBici;
        }

        if (aCobrar > 0)
        {
            if (tarjeta.Saldo < aCobrar)
            {
                return false;
            }
            tarjeta.Saldo -= aCobrar;
            tarjeta.AcreditarCarga();
            tarjeta.MultasBici = 0;
            if (esNuevoDia)
            {
                tarjeta.UltimoPagoBici = fecha;
            }
        }

        tarjeta.InicioUsoBici = fecha;
        return true;
    }

    public void devolverBici(Tarjeta tarjeta)
    {
        devolverBici(tarjeta, Reloj.Ahora);
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
