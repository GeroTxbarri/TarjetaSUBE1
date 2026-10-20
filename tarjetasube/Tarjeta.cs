public class Tarjeta {

    public int Saldo { get; set; }
    public int Id { get; set; }
    public int TarjetaTipoId { get; set; }
    public TarjetaTipo Tipo { get; set; }

    int[] montoValidos = {2000,3000,4000,5000,8000,10000,15000,20000,25000,30000};

    public Tarjeta (){
        Tipo = new TarjetaTipo(1, "Normal", 0);
        TarjetaTipoId = Tipo.Id;
    }

    public Tarjeta (TarjetaTipo tipo){
        Tipo = tipo;
        TarjetaTipoId = tipo.Id;
    }

    public void cargarTarjeta (int monto){
        
        if (montoValidos.Contains(monto) && Saldo+monto <=40000 ){
            Saldo += monto;
        }
        
    }
    public bool descontarSaldo (int monto){
        if ((Saldo - monto) >= -2000){
            Saldo -= monto;
            return true;
        }

        return false;

         
    }

    public int calcularPrecio (int tarifa){
        int descuento = tarifa * Tipo.PorcentajeDescuento / 100;
        return tarifa - descuento;
    }

    
}
