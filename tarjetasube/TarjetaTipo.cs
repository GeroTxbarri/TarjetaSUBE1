public class TarjetaTipo
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int PorcentajeDescuento { get; set; }

    public TarjetaTipo(int id, string nombre, int porcentajeDescuento)
    {
        Id = id;
        Nombre = nombre;
        PorcentajeDescuento = porcentajeDescuento;
    }
}
