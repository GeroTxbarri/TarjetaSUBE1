using NUnit.Framework;

[TestFixture]
public class ProveedorDeFechaSistemaTests
{
    [Test]
    public void Ahora_DevuelveLaHoraDeLaComputadora()
    {
        // Unico test que mira la hora real: es justamente lo que hace esta clase
        var reloj = new ProveedorDeFechaSistema();

        Assert.That(reloj.Ahora, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
    }
}
