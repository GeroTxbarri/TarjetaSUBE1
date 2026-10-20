public class Colectivo
{
    private int Tarifa = 1580;

    public int Linea_numero { get; set; }

    public dynamic pagarCon(Tarjeta tarjeta)
    {
        int precio = tarjeta.calcularPrecio(Tarifa);
        if (!tarjeta.descontarSaldo(precio))
        {
            return false;
        }
        return new Boleto(precio, Linea_numero);
    }
}
