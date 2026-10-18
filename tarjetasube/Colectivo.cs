public class Colectivo
{
    private int Tarifa = 1580;

    public int Linea_numero { get; set; }

    public bool pagarCon(Tarjeta tarjeta)
    {
        if(Tarjeta.Saldo < Tarifa){
            return false;
        }
        tarjeta.descontarSaldo(Tarifa);
        return true;
    }
}
