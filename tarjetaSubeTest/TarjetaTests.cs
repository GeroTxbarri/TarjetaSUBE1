using NUnit.Framework;

[TestFixture]
public class TarjetaTests
{
    [Test]
    public void CargarTarjeta_Valido(){

        var tarjeta = new Tarjeta();

        tarjeta.cargarTarjeta(2000);

        Assert.That(tarjeta.Saldo, Is.EqualTo(2000));
    }

    [Test]
    public void CargarTarjeta_invalido(){
        var tarjeta = new Tarjeta();

        tarjeta.cargarTarjeta(9999);

        Assert.That(tarjeta.Saldo, Is.EqualTo(0));
    }
}
