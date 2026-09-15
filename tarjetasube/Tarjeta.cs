public class Tarjeta {

    public int Saldo;

    public void cargarTarjeta (int monto){
        int[] montoValidos = {2000,3000,4000,5000,8000,10000,15000,20000,30000};
        if (montoValidos.Contains(monto) && Saldo+monto <40000){
            Saldo += monto;
        }
        
    }
    public void descontarSaldo (int monto){
        Saldo -= monto;
    }

    
}