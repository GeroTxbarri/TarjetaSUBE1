public class Boleto
{
    public int Tarifa { get; }
    public int LineaColectivo { get; }

    public Boleto(int tarifa, int lineaColectivo)
    {
        Tarifa = tarifa;
        LineaColectivo = lineaColectivo;
    }
}