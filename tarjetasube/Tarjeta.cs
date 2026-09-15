public class Tarjeta {

    public int Saldo { get; set; }

    int[] montoValidos = {2000,3000,4000,5000,8000,10000,15000,20000,25000,30000};

    public void cargarTarjeta (int monto){
        
        if (montoValidos.Contains(monto) && Saldo+monto <=40000 && Saldo >= 0){
            Saldo += monto;
        }
        
    }
    public void descontarSaldo (int monto){
        if (Saldo >= monto){
            Saldo -= monto;
            return;
        }

         
    }

    
}