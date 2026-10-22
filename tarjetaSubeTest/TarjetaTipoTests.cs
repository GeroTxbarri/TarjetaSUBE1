using NUnit.Framework;

[TestFixture]
public class TarjetaTipoTests
{
    [Test]
    public void Tarjeta_Normal_PagaTarifaCompleta()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var boleto = colectivo.pagarCon(tarjeta);

        Assert.That(tarjeta.TarjetaTipoId, Is.EqualTo(1));
        Assert.That(boleto.TotalAbonado, Is.EqualTo(1580));
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 1580));
    }

    [Test]
    public void Tarjeta_GuardaElIdDelTipo()
    {
        var tipo = new TarjetaTipo(2, "Medio boleto estudiantil", 50);
        var tarjeta = new Tarjeta(tipo);

        Assert.That(tarjeta.TarjetaTipoId, Is.EqualTo(2));
        Assert.That(tarjeta.Tipo.Nombre, Is.EqualTo("Medio boleto estudiantil"));
    }

    [Test]
    public void MedioBoleto_PagaLaMitad()
    {
        var tipo = new TarjetaTipo(2, "Medio boleto estudiantil", 50) { ViajesConDescuentoPorDia = 2, MinutosEntreViajes = 5 };
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 22, 10, 0, 0);

        var boleto1 = colectivo.pagarCon(tarjeta, hora);
        var boleto2 = colectivo.pagarCon(tarjeta, hora.AddMinutes(10));

        Assert.That(boleto1.TotalAbonado, Is.EqualTo(1580 / 2));
        Assert.That(boleto2.TotalAbonado, Is.EqualTo(1580 / 2));
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 790 * 2));
    }

    [Test]
    public void FranquiciaCompleta_SiemprePuedePagar()
    {
        var tipo = new TarjetaTipo(4, "Franquicia completa", 100);
        var tarjeta = new Tarjeta(tipo);

        var colectivo = new Colectivo { Linea_numero = 67 };

        for (int i = 0; i < 10; i++)
        {
            var boleto = colectivo.pagarCon(tarjeta);

            Assert.That(boleto.TotalAbonado, Is.EqualTo(0));
            Assert.That(tarjeta.Saldo, Is.EqualTo(0));
        }
    }

    [Test]
    public void BoletoGratuitoEstudiantil_NoDescuentaSaldo()
    {
        var tipo = new TarjetaTipo(3, "Boleto gratuito estudiantil", 100);
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(2000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var boleto = colectivo.pagarCon(tarjeta);

        Assert.That(boleto.TotalAbonado, Is.EqualTo(0));
        Assert.That(tarjeta.Saldo, Is.EqualTo(2000));
    }

    [Test]
    public void MedioBoleto_NoPuedeViajarAntesDe5Minutos()
    {
        var tipo = new TarjetaTipo(2, "Medio boleto estudiantil", 50) { ViajesConDescuentoPorDia = 2, MinutosEntreViajes = 5 };
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 22, 10, 0, 0);

        var boleto = colectivo.pagarCon(tarjeta, hora);
        var segundoViaje = colectivo.pagarCon(tarjeta, hora.AddMinutes(3));

        Assert.That(boleto.TotalAbonado, Is.EqualTo(790));
        Assert.That(segundoViaje, Is.EqualTo(false));
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 790));
    }

    [Test]
    public void MedioBoleto_PuedeViajarDespuesDe5Minutos()
    {
        var tipo = new TarjetaTipo(2, "Medio boleto estudiantil", 50) { ViajesConDescuentoPorDia = 2, MinutosEntreViajes = 5 };
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 22, 10, 0, 0);

        colectivo.pagarCon(tarjeta, hora);
        var segundoViaje = colectivo.pagarCon(tarjeta, hora.AddMinutes(5));

        Assert.That(segundoViaje.TotalAbonado, Is.EqualTo(790));
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 790 * 2));
    }

    [Test]
    public void MedioBoleto_MaximoDosViajesPorDia()
    {
        var tipo = new TarjetaTipo(2, "Medio boleto estudiantil", 50) { ViajesConDescuentoPorDia = 2, MinutosEntreViajes = 5 };
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 22, 10, 0, 0);

        var boleto1 = colectivo.pagarCon(tarjeta, hora);
        var boleto2 = colectivo.pagarCon(tarjeta, hora.AddMinutes(10));
        var boleto3 = colectivo.pagarCon(tarjeta, hora.AddMinutes(20));

        Assert.That(boleto1.TotalAbonado, Is.EqualTo(790));
        Assert.That(boleto2.TotalAbonado, Is.EqualTo(790));
        Assert.That(boleto3.TotalAbonado, Is.EqualTo(1580));
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 790 * 2 - 1580));
    }

    [Test]
    public void MedioBoleto_AlOtroDiaVuelveAPagarLaMitad()
    {
        var tipo = new TarjetaTipo(2, "Medio boleto estudiantil", 50) { ViajesConDescuentoPorDia = 2, MinutosEntreViajes = 5 };
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 22, 10, 0, 0);

        colectivo.pagarCon(tarjeta, hora);
        colectivo.pagarCon(tarjeta, hora.AddMinutes(10));
        colectivo.pagarCon(tarjeta, hora.AddMinutes(20));
        var boletoOtroDia = colectivo.pagarCon(tarjeta, hora.AddDays(1));

        Assert.That(boletoOtroDia.TotalAbonado, Is.EqualTo(790));
    }

    [Test]
    public void BoletoGratuito_MaximoDosViajesGratisPorDia()
    {
        var tipo = new TarjetaTipo(3, "Boleto gratuito estudiantil", 100) { ViajesConDescuentoPorDia = 2 };
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 22, 10, 0, 0);

        var boleto1 = colectivo.pagarCon(tarjeta, hora);
        var boleto2 = colectivo.pagarCon(tarjeta, hora.AddMinutes(10));
        var boleto3 = colectivo.pagarCon(tarjeta, hora.AddMinutes(20));

        Assert.That(boleto1.TotalAbonado, Is.EqualTo(0));
        Assert.That(boleto2.TotalAbonado, Is.EqualTo(0));
        Assert.That(boleto3.TotalAbonado, Is.Not.EqualTo(0));
    }

    [Test]
    public void BoletoGratuito_ViajesDespuesDelSegundoPaganCompleto()
    {
        var tipo = new TarjetaTipo(3, "Boleto gratuito estudiantil", 100) { ViajesConDescuentoPorDia = 2 };
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 22, 10, 0, 0);

        colectivo.pagarCon(tarjeta, hora);
        colectivo.pagarCon(tarjeta, hora.AddMinutes(10));
        var boleto3 = colectivo.pagarCon(tarjeta, hora.AddMinutes(20));
        var boleto4 = colectivo.pagarCon(tarjeta, hora.AddMinutes(30));

        Assert.That(boleto3.TotalAbonado, Is.EqualTo(1580));
        Assert.That(boleto4.TotalAbonado, Is.EqualTo(1580));
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 1580 * 2));
    }

    [Test]
    public void BoletoGratuito_AlOtroDiaVuelveASerGratis()
    {
        var tipo = new TarjetaTipo(3, "Boleto gratuito estudiantil", 100) { ViajesConDescuentoPorDia = 2 };
        var tarjeta = new Tarjeta(tipo);
        tarjeta.cargarTarjeta(5000);

        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 22, 10, 0, 0);

        colectivo.pagarCon(tarjeta, hora);
        colectivo.pagarCon(tarjeta, hora.AddMinutes(10));
        colectivo.pagarCon(tarjeta, hora.AddMinutes(20));
        var boletoOtroDia = colectivo.pagarCon(tarjeta, hora.AddDays(1));

        Assert.That(boletoOtroDia.TotalAbonado, Is.EqualTo(0));
    }
}
