public class Boleto
{
    public DateTime Fecha { get; set; }
    public string TarjetaTipo { get; set; }
    public int TotalAbonado { get; set; }
    public int Tarifa => TotalAbonado;
    public int LineaColectivo { get; set; }
    public int SaldoRestante { get; set; }
    public int IdTarjeta { get; set; }

    public Boleto(int totalAbonado, int lineaColectivo, Tarjeta tarjeta)
    {
        Fecha = DateTime.Now;
        TarjetaTipo = tarjeta.Tipo.Nombre;
        TotalAbonado = totalAbonado;
        LineaColectivo = lineaColectivo;
        SaldoRestante = tarjeta.Saldo;
        IdTarjeta = tarjeta.Id;
    }
}