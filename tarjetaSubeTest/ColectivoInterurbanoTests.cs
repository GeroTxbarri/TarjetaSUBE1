using NUnit.Framework;

[TestFixture]
public class ColectivoInterurbanoTests
{
    
    [Test]
    public void PagarNormal()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var colectivo = new ColectivoInterUrbano { Linea_numero = 101 };
        var boleto = colectivo.pagarCon(tarjeta);

        Assert.That(boleto.TotalAbonado, Is.EqualTo(3000));
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 3000));
        Assert.That(boleto.LineaColectivo, Is.EqualTo(101));
    }

    
    [Test]
    public void PagarConMedioBoleto()
    {
        var tipo = new TarjetaTipo(2, "Medio boleto estudiantil", 50) { ViajesConDescuentoPorDia = 2, MinutosEntreViajes = 5 };
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo = new ColectivoInterUrbano { Linea_numero = 101 };
        var fecha = new DateTime(2026, 10, 22, 10, 0, 0); // Jueves 10:00 (en franja horaria)

        var boleto = colectivo.pagarCon(tarjeta, fecha);

        Assert.That(boleto.TotalAbonado, Is.EqualTo(1500));
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 1500));
    }

    
    [Test]
    public void PagarConFranquiciaCompleta()
    {
        var tipo = new TarjetaTipo(4, "Franquicia completa", 100);
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo = new ColectivoInterUrbano { Linea_numero = 101 };
        var fecha = new DateTime(2026, 10, 22, 10, 0, 0);

        var boleto = colectivo.pagarCon(tarjeta, fecha);

        Assert.That(boleto.TotalAbonado, Is.EqualTo(0));
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000));
    }

    [Test]
    public void PagarConSaldoInsuficiente()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(2000);

        var colectivo = new ColectivoInterUrbano { Linea_numero = 101 };
        var boleto = colectivo.pagarCon(tarjeta);

        Assert.That(boleto.TotalAbonado, Is.EqualTo(3000));
        Assert.That(tarjeta.Saldo, Is.EqualTo(2000 - 3000)); // -1000
    }

    
    [Test]
    public void Franquicia_NoPuedeViajar()
    {
        var tipo = new TarjetaTipo(2, "Medio boleto estudiantil", 50);
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo = new ColectivoInterUrbano { Linea_numero = 101 };
        var sabado = new DateTime(2026, 10, 24, 10, 0, 0);

        var resultado = colectivo.pagarCon(tarjeta, sabado);

        Assert.That(resultado, Is.EqualTo(false));
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000));
    }
}
