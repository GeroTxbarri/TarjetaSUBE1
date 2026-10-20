public class Colectivo
{
    private int Tarifa = 1580;

    public int Linea_numero { get; set; }

    public dynamic pagarCon(Tarjeta tarjeta)
    {
        if(!tarjeta.descontarSaldo(Tarifa)){
            return false;
        }
        return new Boleto(Tarifa, Linea_numero);
    }
}
