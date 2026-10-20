using NUnit.Framework;

[TestFixture]
public class BoletoTests
{
    [Test]
    public void Boleto_TarjetaNormal_MuestraDatosCorrectos()
    {
        var tarjeta = new Tarjeta { Id = 1 };
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 102 };
        var boleto = colectivo.pagarCon(tarjeta);

        // Verificamos los datos requeridos por la consigna
        Assert.That(boleto.TotalAbonado, Is.EqualTo(1580));
        Assert.That(boleto.LineaColectivo, Is.EqualTo(102));
        Assert.That(boleto.TarjetaTipo, Is.EqualTo("Normal"));
        Assert.That(boleto.IdTarjeta, Is.EqualTo(1));
        Assert.That(boleto.SaldoRestante, Is.EqualTo(5000 - 1580));
        Assert.That(boleto.Fecha.Date, Is.EqualTo(DateTime.Today));
    }

    // TODO: Completar los tests para los otros tipos de tarjeta y saldo negativo
}
