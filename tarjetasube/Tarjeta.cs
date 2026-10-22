public class Tarjeta {

    public int Saldo { get; set; }
    public int Id { get; set; }
    public int TarjetaTipoId { get; set; }
    public TarjetaTipo Tipo { get; set; }

    public const int LimiteSaldo = 56000;
    public int SaldoPendiente { get; set; } = 0;

    public DateTime UltimoViaje { get; set; } = DateTime.MinValue;
    public int ViajesHoy { get; set; } = 0;

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
        
    
        if (montoValidos.Contains(monto) ){
            SaldoPendiente += monto;
            AcreditarCarga();
        }
        
        
    }
    public bool descontarSaldo (int monto){
        if ((Saldo - monto) >= -2000){
            Saldo -= monto;
            AcreditarCarga();
            return true;
        }

        return false;

         
    }

    public int calcularPrecio (int tarifa, DateTime fecha){
        if (Tipo.ViajesConDescuentoPorDia > 0 && viajesDelDia(fecha) >= Tipo.ViajesConDescuentoPorDia){
            return tarifa;
        }
        int descuento = tarifa * Tipo.PorcentajeDescuento / 100;
        return tarifa - descuento;
    }

    public bool puedeViajar (DateTime fecha){
        if (fecha < UltimoViaje.AddMinutes(Tipo.MinutosEntreViajes)){
            return false;
        }
        return true;
    }

    public int viajesDelDia (DateTime fecha){
        if (fecha.Date == UltimoViaje.Date){
            return ViajesHoy;
        }
        return 0;
    }

    public void registrarViaje (DateTime fecha){
        ViajesHoy = viajesDelDia(fecha) + 1;
        UltimoViaje = fecha;
    }

    public void AcreditarCarga (){
        if (SaldoPendiente > 0  && Saldo <= LimiteSaldo){

            int saldolibre = LimiteSaldo-Saldo;
            int saldoacreditar = Math.Min(saldolibre, SaldoPendiente);
            Saldo += saldoacreditar;
            SaldoPendiente -= saldoacreditar;

        }
    }

    
}
