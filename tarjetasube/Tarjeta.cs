public class Tarjeta {

    public int Saldo { get; set; }
    public int Id { get; set; }
    public int TarjetaTipoId { get; set; }
    public TarjetaTipo Tipo { get; set; }

    public const int LimiteSaldo = 56000;
    public int SaldoPendiente { get; set; } = 0;

    public DateTime UltimoViaje { get; set; } = DateTime.MinValue;
    public int ViajesHoy { get; set; } = 0;
    public int ViajesMes { get; set; } = 0;

    public int UltimaLinea { get; set; } = -1;
    public DateTime? InicioVentanaTrasbordo { get; set; } = null;

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
        if (!esFranquicia()){
            int numeroDeViaje = viajesDelMes(fecha) + 1;
            if (numeroDeViaje >= 80){
                return tarifa - tarifa * 25 / 100;
            }
            if (numeroDeViaje >= 30){
                return tarifa - tarifa * 20 / 100;
            }
            return tarifa;
        }
        if (Tipo.ViajesConDescuentoPorDia > 0 && viajesDelDia(fecha) >= Tipo.ViajesConDescuentoPorDia){
            return tarifa;
        }
        int descuento = tarifa * Tipo.PorcentajeDescuento / 100;
        return tarifa - descuento;
    }

    public bool puedeViajar (DateTime fecha){
        if (esFranquicia() && !estaEnFranjaHoraria(fecha)){
            return false;
        }
        if (fecha < UltimoViaje.AddMinutes(Tipo.MinutosEntreViajes)){
            return false;
        }
        return true;
    }

    public bool esFranquicia (){
        // el tipo 1 es Normal, todos los demas son franquicias
        return Tipo.Id != 1;
    }

    public bool estaEnFranjaHoraria (DateTime fecha){
        if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday){
            return false;
        }
        if (fecha.Hour < 6 || fecha.Hour >= 22){
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

    public int viajesDelMes (DateTime fecha){
        if (fecha.Year == UltimoViaje.Year && fecha.Month == UltimoViaje.Month){
            return ViajesMes;
        }
        return 0;
    }

    public void registrarViaje (DateTime fecha, int linea = -1, bool esTrasbordo = false){
        UltimoViaje = fecha;
        UltimaLinea = linea;

        if (!esTrasbordo)
        {
            InicioVentanaTrasbordo = fecha;
            ViajesHoy = viajesDelDia(fecha) + 1;
            ViajesMes = viajesDelMes(fecha) + 1;
        }
    }

    public void AcreditarCarga (){
        if (SaldoPendiente > 0  && Saldo <= LimiteSaldo){

            int saldolibre = LimiteSaldo-Saldo;
            int saldoacreditar = Math.Min(saldolibre, SaldoPendiente);
            Saldo += saldoacreditar;
            SaldoPendiente -= saldoacreditar;

        }
    }

    public bool esTrasbordo(int linea, DateTime fecha) {
        if (InicioVentanaTrasbordo == null || UltimaLinea == -1) 
            return false;
        if (linea == UltimaLinea) 
            return false;
        if (!estaEnHorarioTrasbordo(fecha)) 
            return false;
        if (fecha > InicioVentanaTrasbordo.Value.AddHours(1) || fecha < InicioVentanaTrasbordo.Value) return false;
            return true;
}

public bool estaEnHorarioTrasbordo(DateTime fecha) {
    if (fecha.DayOfWeek == DayOfWeek.Sunday) 
        return false;
    if (fecha.Hour < 7 || fecha.Hour >= 22) 
        return false;
    return true;
}


    
}
