using NUnit.Framework;

[TestFixture]
public class BoletoTests
{
    [Test]
    public void Boleto_TarjetaNormal_MuestraDatosCorrectos()
    {
        var tarjeta = new Tarjeta { Id = 1 };
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 1 };
        var boleto = colectivo.pagarCon(tarjeta);

        Assert.That(boleto.TotalAbonado, Is.EqualTo(1580));
        Assert.That(boleto.LineaColectivo, Is.EqualTo(1));
        Assert.That(boleto.TarjetaTipo, Is.EqualTo("Normal"));
        Assert.That(boleto.IdTarjeta, Is.EqualTo(1));
        Assert.That(boleto.SaldoRestante, Is.EqualTo(5000 - 1580));
        Assert.That(boleto.Fecha.Date, Is.EqualTo(DateTime.Today));
    }

    [Test]
    public void Boleto_TarjetaMedioBoleto_MuestraDatosCorrectos()
    {
        var tipo = new TarjetaTipo(2, "Medio boleto estudiantil", 50);
        var tarjeta = new Tarjeta(tipo) { Id = 2 };
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 2 };
        var hora = new DateTime(2026, 10, 22, 10, 0, 0);
        var boleto = colectivo.pagarCon(tarjeta, hora);

        Assert.That(boleto.TotalAbonado, Is.EqualTo(790));
        Assert.That(boleto.LineaColectivo, Is.EqualTo(2));
        Assert.That(boleto.TarjetaTipo, Is.EqualTo("Medio boleto estudiantil"));
        Assert.That(boleto.IdTarjeta, Is.EqualTo(2));
        Assert.That(boleto.SaldoRestante, Is.EqualTo(5000 - 790));
        Assert.That(boleto.Fecha.Date, Is.EqualTo(hora.Date));
    }

    [Test]
    public void Boleto_TarjetaBoletoGratuito_MuestraDatosCorrectos()
    {
        var tipo = new TarjetaTipo(3, "Boleto gratuito estudiantil", 100);
        var tarjeta = new Tarjeta(tipo) { Id = 3 };
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 3 };
        var hora = new DateTime(2026, 10, 22, 10, 0, 0);
        var boleto = colectivo.pagarCon(tarjeta, hora);

        Assert.That(boleto.TotalAbonado, Is.EqualTo(0));
        Assert.That(boleto.LineaColectivo, Is.EqualTo(3));
        Assert.That(boleto.TarjetaTipo, Is.EqualTo("Boleto gratuito estudiantil"));
        Assert.That(boleto.IdTarjeta, Is.EqualTo(3));
        Assert.That(boleto.SaldoRestante, Is.EqualTo(5000));
        Assert.That(boleto.Fecha.Date, Is.EqualTo(hora.Date));
    }

    [Test]
    public void Boleto_TarjetaFranquiciaCompleta_MuestraDatosCorrectos()
    {
        var tipo = new TarjetaTipo(4, "Franquicia completa", 100);
        var tarjeta = new Tarjeta(tipo) { Id = 4 };
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 4 };
        var hora = new DateTime(2026, 10, 22, 10, 0, 0);
        var boleto = colectivo.pagarCon(tarjeta, hora);

        Assert.That(boleto.TotalAbonado, Is.EqualTo(0));
        Assert.That(boleto.LineaColectivo, Is.EqualTo(4));
        Assert.That(boleto.TarjetaTipo, Is.EqualTo("Franquicia completa"));
        Assert.That(boleto.IdTarjeta, Is.EqualTo(4));
        Assert.That(boleto.SaldoRestante, Is.EqualTo(5000));
        Assert.That(boleto.Fecha.Date, Is.EqualTo(hora.Date));
    }

    [Test]
    public void Boleto_ViajeConSaldoNegativo_MuestraSaldoRestanteNegativo()
    {
        var tarjeta = new Tarjeta { Id = 5 };

        var colectivo = new Colectivo { Linea_numero = 5 };
        var boleto = colectivo.pagarCon(tarjeta);

        Assert.That(boleto.TotalAbonado, Is.EqualTo(1580));
        Assert.That(boleto.LineaColectivo, Is.EqualTo(5));
        Assert.That(boleto.TarjetaTipo, Is.EqualTo("Normal"));
        Assert.That(boleto.IdTarjeta, Is.EqualTo(5));
        Assert.That(boleto.SaldoRestante, Is.EqualTo(-1580));
        Assert.That(boleto.Fecha.Date, Is.EqualTo(DateTime.Today));
    }
}
