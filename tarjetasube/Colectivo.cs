public class Colectivo
{
    private int Tarifa = 1580;

    public int Linea_numero { get; set; }

    public dynamic pagarCon(Tarjeta tarjeta)
    {
        return pagarCon(tarjeta, DateTime.Now);
    }

    public dynamic pagarCon(Tarjeta tarjeta, DateTime fecha)
    {
        if (!tarjeta.puedeViajar(fecha))
        {
            return false;
        }
        int precio = tarjeta.calcularPrecio(Tarifa, fecha);
        if (!tarjeta.descontarSaldo(precio))
        {
            return false;
        }
        tarjeta.registrarViaje(fecha);
        return new Boleto(precio, Linea_numero, tarjeta, fecha);
    }
}
