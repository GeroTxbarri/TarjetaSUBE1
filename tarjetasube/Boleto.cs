public class Boleto
{
    public int Tarifa { get; set; }
    public int LineaColectivo { get; set; }

    public Boleto(int tarifa, int lineaColectivo)
    {
        Tarifa = tarifa;
        LineaColectivo = lineaColectivo;
    }
}