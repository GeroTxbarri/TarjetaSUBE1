using NUnit.Framework;

[TestFixture]
public class ColectivoTests
{
    private static readonly DateTime FechaValida = new DateTime(2026, 10, 22, 10, 0, 0); // Jueves 10:00

    [Test]
    public void PagarCon_SaldoSuficiente_DescuentaYDevuelveBoleto()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var boleto = colectivo.pagarCon(tarjeta, FechaValida);

        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 2150));
        Assert.That(boleto.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto.LineaColectivo, Is.EqualTo(67));
        Assert.That(boleto.EsTrasbordo, Is.False);
    }

    [Test]
    public void PagarCon_SaldoCeroYNoAlcanzaMargenNegativo_DevuelveFalse()
    {
        var tarjeta = new Tarjeta(); // Saldo = 0

        var colectivo = new Colectivo { Linea_numero = 67 };
        var resultado = colectivo.pagarCon(tarjeta, FechaValida);

        Assert.That(resultado, Is.EqualTo(false));
        Assert.That(tarjeta.Saldo, Is.EqualTo(0)); // No se descontó nada
    }

    [Test]
    public void PagarCon_SaldoInsuficientePeroEnMargenNegativo_DevuelveBoleto()
    {
        // $500 - $2150 = -$1650, dentro del margen de -$2000
        var tarjeta = new Tarjeta { Saldo = 500 };

        var colectivo = new Colectivo { Linea_numero = 67 };
        var boleto = colectivo.pagarCon(tarjeta, FechaValida);

        Assert.That(boleto.TotalAbonado, Is.EqualTo(2150));
        Assert.That(tarjeta.Saldo, Is.EqualTo(500 - 2150));
    }

    [Test]
    public void PagarCon_SegundoViaje_SaldoYaEnNegativoFueraDelMargen_DevuelveFalse()
    {
        // Primer viaje deja saldo negativo; el segundo lo sacaría por debajo de -$2000
        var tarjeta = new Tarjeta { Saldo = 500 };

        var colectivo = new Colectivo { Linea_numero = 67 };
        colectivo.pagarCon(tarjeta, FechaValida);                               // Saldo: -1650
        var resultado = colectivo.pagarCon(tarjeta, FechaValida.AddMinutes(1)); // -1650 - 2150 < -2000 → rechazado

        Assert.That(resultado, Is.EqualTo(false));
        Assert.That(tarjeta.Saldo, Is.EqualTo(500 - 2150)); // Saldo no cambió en el segundo intento
    }

    [Test]
    public void PagarCon_DevuelveBoletoConTodosLosDatosCorrectos()
    {
        var tarjeta = new Tarjeta { Id = 99 };
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 42 };
        var boleto = colectivo.pagarCon(tarjeta, FechaValida);

        Assert.That(boleto.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto.LineaColectivo, Is.EqualTo(42));
        Assert.That(boleto.IdTarjeta, Is.EqualTo(99));
        Assert.That(boleto.TarjetaTipo, Is.EqualTo("Normal"));
        Assert.That(boleto.SaldoRestante, Is.EqualTo(5000 - 2150));
        Assert.That(boleto.Fecha, Is.EqualTo(FechaValida));
    }

    [Test]
    public void PagarCon_TarjetaFranquicia_FueraDeFranja_DevuelveFalse()
    {
        var tipo = new TarjetaTipo(4, "Franquicia completa", 100);
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var domingo = new DateTime(2026, 10, 25, 10, 0, 0); // Domingo

        var resultado = colectivo.pagarCon(tarjeta, domingo);

        Assert.That(resultado, Is.EqualTo(false));
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000)); // No se descontó
    }
}
