public class Colectivo
{
    private int Tarifa = 1580;

    public int Linea_numero { get; set; }

    public Boleto pagarCon(Tarjeta tarjeta)
    {
        tarjeta.descontarSaldo(Tarifa);
        return new Boleto(Tarifa, Linea_numero);
    }
}
