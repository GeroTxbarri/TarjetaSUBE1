public class Boleto
{
    public DateTime Fecha { get; set; }
    public string TarjetaTipo { get; set; }
    public decimal TotalAbonado { get; set; }
    public int LineaColectivo { get; set; }
    public decimal SaldoRestante { get; set; }
    public int IdTarjeta { get; set; }
    public bool EsTrasbordo {get; set;} = false;

    public Boleto(decimal totalAbonado, int lineaColectivo, Tarjeta tarjeta, DateTime fecha,bool esTrasbordo = false)
    {
        Fecha = fecha;
        TarjetaTipo = tarjeta.Tipo.Nombre;
        TotalAbonado = totalAbonado;
        LineaColectivo = lineaColectivo;
        SaldoRestante = tarjeta.Saldo;
        IdTarjeta = tarjeta.Id;
        EsTrasbordo = esTrasbordo;
    }
}