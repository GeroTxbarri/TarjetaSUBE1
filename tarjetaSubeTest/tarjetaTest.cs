using NUnit.Framework;

[TestFixture]
public class TarjetaTest
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

    [Test]
    public void Pagar_y_Descontar()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var boleto = colectivo.pagarCon(tarjeta);

        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 1580));
        Assert.That(boleto.Tarifa, Is.EqualTo(1580));
    }



}