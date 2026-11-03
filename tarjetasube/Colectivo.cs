public class Colectivo
{
    protected int Tarifa = 2150;
    protected IProveedorDeFecha Reloj = new ProveedorDeFechaSistema();

    public int Id { get; set; }
    public int Linea_numero { get; set; }

    public Colectivo() { }

    public Colectivo(IProveedorDeFecha reloj)
    {
        Reloj = reloj;
    }

    public dynamic pagarCon(Tarjeta tarjeta)
    {
        return pagarCon(tarjeta, Reloj.Ahora);
    }

    public dynamic pagarCon(Tarjeta tarjeta, DateTime fecha)
    {
        if (!tarjeta.puedeViajar(fecha))
        {
            return false;
        }

        bool transbordo = tarjeta.esTrasbordo(Linea_numero, fecha);
        
        decimal precio = 0;
        if (!transbordo){
            precio = tarjeta.calcularPrecio(Tarifa, fecha);
        }

        if (!tarjeta.descontarSaldo(precio))
        {
            return false;
        }

        tarjeta.registrarViaje(fecha, Linea_numero, transbordo);
        return new Boleto(precio, Linea_numero, tarjeta, fecha, transbordo);
    }
}
