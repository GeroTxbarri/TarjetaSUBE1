public class TarjetaTipo
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int PorcentajeDescuento { get; set; }
    public int ViajesConDescuentoPorDia { get; set; } = 0;
    public int MinutosEntreViajes { get; set; } = 0;

    public TarjetaTipo(int id, string nombre, int porcentajeDescuento)
    {
        Id = id;
        Nombre = nombre;
        PorcentajeDescuento = porcentajeDescuento;
    }
}
