// Reloj falso para los tests: devuelve siempre la fecha que se le indique
public class ProveedorDeFechaFijo : IProveedorDeFecha
{
    public DateTime Ahora { get; set; }
}
