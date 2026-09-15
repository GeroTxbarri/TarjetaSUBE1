public class Colectivo{
    public int linea_numero;

    public Boleto pagarCon (Tarjeta tarjeta){
        tarjeta.descontarSaldo(1580);

        return new Boleto(1580, linea_numero);
    }

}


