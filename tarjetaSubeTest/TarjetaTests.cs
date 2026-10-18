using NUnit.Framework;

[TestFixture]
public class TarjetaTests
{
    [Test]
    [TestCase(2000)]
    [TestCase(3000)]
    [TestCase(4000)]
    [TestCase(5000)]
    [TestCase(8000)]
    [TestCase(10000)]
    [TestCase(15000)]
    [TestCase(20000)]
    [TestCase(25000)]
    [TestCase(30000)]

    public void CargarTarjeta_Valido(int monto){

        var tarjeta = new Tarjeta();

        tarjeta.cargarTarjeta(monto);

        Assert.That(tarjeta.Saldo, Is.EqualTo(monto));
    }

    [Test]
    public void CargarTarjeta_invalido(){
        var tarjeta = new Tarjeta();

        tarjeta.cargarTarjeta(9999);

        Assert.That(tarjeta.Saldo, Is.EqualTo(0));
    }

    [Test]
    public void descontarSaldo_valido(){
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(2000);
        tarjeta.descontarSaldo(1580);
        Assert.That(tarjeta.Saldo, Is.EqualTo(420));
        
    }

    [Test]
    public void descontarSaldo_novalido() {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(2000);
        tarjeta.descontarSaldo(1580);
        tarjeta.descontarSaldo(1580);
        tarjeta.descontarSaldo(1580);

        Assert.That(tarjeta.Saldo, Is.GreaterThanOrEqualTo(-2000));

    }

    [Test]
    public void descontarSaldo_viajeplus() {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(2000);
        tarjeta.descontarSaldo(1580);
        tarjeta.descontarSaldo(1580);
        tarjeta.cargarTarjeta(2000)

        Assert.That(tarjeta.Saldo, Is.EqualTo(840));

    }



}
