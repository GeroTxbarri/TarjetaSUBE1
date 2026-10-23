using NUnit.Framework;

[TestFixture]
public class TrasbordoTests
{
    [Test]
    public void TrasbordoValido_EntreLineasDistintas_EsGratisYMarcaBoleto()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var colectivo1 = new Colectivo { Linea_numero = 101 };
        var colectivo2 = new Colectivo { Linea_numero = 102 };

        var fechaPrimerViaje = new DateTime(2026, 10, 22, 10, 0, 0); // Jueves 10:00
        var fechaTrasbordo = new DateTime(2026, 10, 22, 10, 30, 0);   // Jueves 10:30 (dentro de 1 hora)

        var boleto1 = colectivo1.pagarCon(tarjeta, fechaPrimerViaje);
        var boleto2 = colectivo2.pagarCon(tarjeta, fechaTrasbordo);

        Assert.That(boleto1.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto1.EsTrasbordo, Is.False);

        Assert.That(boleto2.TotalAbonado, Is.EqualTo(0));
        Assert.That(boleto2.EsTrasbordo, Is.True);
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 2150)); // No se descontó nada en el trasbordo
    }

    [Test]
    public void MismaLinea_NoEsTrasbordo_CobraPasajeNormal()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var colectivo1 = new Colectivo { Linea_numero = 101 };
        var colectivo2 = new Colectivo { Linea_numero = 101 }; // Misma línea

        var fecha1 = new DateTime(2026, 10, 22, 10, 0, 0);
        var fecha2 = new DateTime(2026, 10, 22, 10, 20, 0);

        var boleto1 = colectivo1.pagarCon(tarjeta, fecha1);
        var boleto2 = colectivo2.pagarCon(tarjeta, fecha2);

        Assert.That(boleto1.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto2.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto2.EsTrasbordo, Is.False);
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 2150 * 2));
    }

    [Test]
    public void PasadaUnaHora_NoEsTrasbordo_CobraPasajeNormal()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var colectivo1 = new Colectivo { Linea_numero = 101 };
        var colectivo2 = new Colectivo { Linea_numero = 102 };

        var fecha1 = new DateTime(2026, 10, 22, 10, 0, 0);
        var fecha2 = new DateTime(2026, 10, 22, 11, 5, 0); // 65 minutos después

        var boleto1 = colectivo1.pagarCon(tarjeta, fecha1);
        var boleto2 = colectivo2.pagarCon(tarjeta, fecha2);

        Assert.That(boleto1.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto2.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto2.EsTrasbordo, Is.False);
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 2150 * 2));
    }

    [Test]
    public void MultiplesTrasbordos_DentroDeLaHora_SinLimite()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var colectivo1 = new Colectivo { Linea_numero = 101 };
        var colectivo2 = new Colectivo { Linea_numero = 102 };
        var colectivo3 = new Colectivo { Linea_numero = 103 };

        var fecha1 = new DateTime(2026, 10, 22, 10, 0, 0);
        var fecha2 = new DateTime(2026, 10, 22, 10, 20, 0);
        var fecha3 = new DateTime(2026, 10, 22, 10, 45, 0);

        var boleto1 = colectivo1.pagarCon(tarjeta, fecha1);
        var boleto2 = colectivo2.pagarCon(tarjeta, fecha2);
        var boleto3 = colectivo3.pagarCon(tarjeta, fecha3);

        Assert.That(boleto1.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto1.EsTrasbordo, Is.False);

        Assert.That(boleto2.TotalAbonado, Is.EqualTo(0));
        Assert.That(boleto2.EsTrasbordo, Is.True);

        Assert.That(boleto3.TotalAbonado, Is.EqualTo(0));
        Assert.That(boleto3.EsTrasbordo, Is.True);

        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 2150));
    }

    [Test]
    public void Domingo_NoPermiteTrasbordo()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var colectivo1 = new Colectivo { Linea_numero = 101 };
        var colectivo2 = new Colectivo { Linea_numero = 102 };

        var domingo1 = new DateTime(2026, 10, 25, 10, 0, 0); // Domingo
        var domingo2 = new DateTime(2026, 10, 25, 10, 20, 0);

        var boleto1 = colectivo1.pagarCon(tarjeta, domingo1);
        var boleto2 = colectivo2.pagarCon(tarjeta, domingo2);

        Assert.That(boleto1.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto2.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto2.EsTrasbordo, Is.False);
    }

    [Test]
    public void FueraDeHorario_AntesDeLas7_NoEsTrasbordo()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var colectivo1 = new Colectivo { Linea_numero = 101 };
        var colectivo2 = new Colectivo { Linea_numero = 102 };

        var hora1 = new DateTime(2026, 10, 22, 6, 20, 0);
        var hora2 = new DateTime(2026, 10, 22, 6, 45, 0); // Antes de las 7:00

        var boleto1 = colectivo1.pagarCon(tarjeta, hora1);
        var boleto2 = colectivo2.pagarCon(tarjeta, hora2);

        Assert.That(boleto1.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto2.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto2.EsTrasbordo, Is.False);
    }

    [Test]
    public void FueraDeHorario_DesdeLas22_NoEsTrasbordo()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var colectivo1 = new Colectivo { Linea_numero = 101 };
        var colectivo2 = new Colectivo { Linea_numero = 102 };

        var hora1 = new DateTime(2026, 10, 22, 21, 40, 0);
        var hora2 = new DateTime(2026, 10, 22, 22, 10, 0); // A partir de las 22:00

        var boleto1 = colectivo1.pagarCon(tarjeta, hora1);
        var boleto2 = colectivo2.pagarCon(tarjeta, hora2);

        Assert.That(boleto1.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto2.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto2.EsTrasbordo, Is.False);
    }

    [Test]
    public void Trasbordo_UrbanoAInterurbano_EsGratis()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var colectivoUrbano = new Colectivo { Linea_numero = 101 };
        var colectivoInterurbano = new ColectivoInterUrbano { Linea_numero = 201 };

        var hora1 = new DateTime(2026, 10, 22, 10, 0, 0);
        var hora2 = new DateTime(2026, 10, 22, 10, 25, 0);

        var boleto1 = colectivoUrbano.pagarCon(tarjeta, hora1);
        var boleto2 = colectivoInterurbano.pagarCon(tarjeta, hora2);

        Assert.That(boleto1.TotalAbonado, Is.EqualTo(2150));
        Assert.That(boleto2.TotalAbonado, Is.EqualTo(0));
        Assert.That(boleto2.EsTrasbordo, Is.True);
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 2150));
    }

    [Test]
    public void MedioBoleto_PuedeRealizarTrasbordoSinCosto()
    {
        var tipo = new TarjetaTipo(2, "Medio boleto estudiantil", 50) { ViajesConDescuentoPorDia = 2, MinutosEntreViajes = 5 };
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo1 = new Colectivo { Linea_numero = 101 };
        var colectivo2 = new Colectivo { Linea_numero = 102 };

        var hora1 = new DateTime(2026, 10, 22, 10, 0, 0);
        var hora2 = new DateTime(2026, 10, 22, 10, 15, 0); // Pasaron > 5 min y < 60 min

        var boleto1 = colectivo1.pagarCon(tarjeta, hora1);
        var boleto2 = colectivo2.pagarCon(tarjeta, hora2);

        Assert.That(boleto1.TotalAbonado, Is.EqualTo(1075));
        Assert.That(boleto2.TotalAbonado, Is.EqualTo(0));
        Assert.That(boleto2.EsTrasbordo, Is.True);
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 1075));
    }
}
