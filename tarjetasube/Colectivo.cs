public class Colectivo
{
    private int Tarifa = 1580;

    public int Linea_numero { get; set; }

    public Boleto pagarCon(Tarjeta tarjeta)
    {
        int precio = tarjeta.calcularPrecio(Tarifa);
        tarjeta.descontarSaldo(precio);
        return new Boleto(precio, Linea_numero);
    }
}
