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
        tarjeta.descontarSaldo(2150);
        Assert.That(tarjeta.Saldo, Is.EqualTo(-150));
        
    }

    [Test]
    public void descontarSaldo_novalido() {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(2000);
        tarjeta.descontarSaldo(2150);
        tarjeta.descontarSaldo(2150);
        tarjeta.descontarSaldo(2150);

        Assert.That(tarjeta.Saldo, Is.GreaterThanOrEqualTo(-2000));

    }

    [Test]
    public void descontarSaldo_viajeplus() {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(2000);
        tarjeta.descontarSaldo(2150);
        tarjeta.cargarTarjeta(2000);

        Assert.That(tarjeta.Saldo, Is.EqualTo(1850));

    }

    [Test]
    public void cargaExcedentePendiente(){
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(30000);
        tarjeta.cargarTarjeta(30000);
        Assert.That(tarjeta.Saldo, Is.EqualTo(56000));
        Assert.That(tarjeta.SaldoPendiente, Is.EqualTo(4000));

    }

    [Test]
    public void cargaExcedenteAcredita(){
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(30000);
        tarjeta.cargarTarjeta(30000);
        tarjeta.descontarSaldo(2150);
        Assert.That(tarjeta.Saldo, Is.EqualTo(56000));
        Assert.That(tarjeta.SaldoPendiente, Is.EqualTo(4000-2150));

    }

    [Test]
    public void usoFrecuente_viajes1al29TarifaNormal(){
        var tarjeta = new Tarjeta();
        for (int i = 0; i < 5; i++){
            tarjeta.cargarTarjeta(30000);
        }
        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 1, 10, 0, 0);

        for (int viaje = 1; viaje <= 29; viaje++){
            var boleto = colectivo.pagarCon(tarjeta, hora.AddMinutes(viaje));
            Assert.That(boleto.TotalAbonado, Is.EqualTo(2150));
        }
    }

    [Test]
    public void usoFrecuente_viajes30al79Descuento20(){
        var tarjeta = new Tarjeta();
        for (int i = 0; i < 5; i++){
            tarjeta.cargarTarjeta(30000);
        }
        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 1, 10, 0, 0);

        for (int viaje = 1; viaje <= 29; viaje++){
            colectivo.pagarCon(tarjeta, hora.AddMinutes(viaje));
        }
        for (int viaje = 30; viaje <= 79; viaje++){
            var boleto = colectivo.pagarCon(tarjeta, hora.AddMinutes(viaje));
            Assert.That(boleto.TotalAbonado, Is.EqualTo(2150 - 2150 * 20 / 100));
        }
    }

    [Test]
    public void usoFrecuente_desdeViaje80Descuento25(){
        var tarjeta = new Tarjeta();
        for (int i = 0; i < 5; i++){
            tarjeta.cargarTarjeta(30000);
        }
        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 1, 10, 0, 0);

        for (int viaje = 1; viaje <= 79; viaje++){
            colectivo.pagarCon(tarjeta, hora.AddMinutes(viaje));
        }
        var boleto80 = colectivo.pagarCon(tarjeta, hora.AddMinutes(80));
        var boleto81 = colectivo.pagarCon(tarjeta, hora.AddMinutes(81));

        Assert.That(boleto80.TotalAbonado, Is.EqualTo(1612.5m));
        Assert.That(boleto81.TotalAbonado, Is.EqualTo(1612.5m));
        Assert.That(tarjeta.Saldo, Is.EqualTo(150000 - 29 * 2150 - 50 * 1720 - 1612.5m * 2));
    }

    [Test]
    public void usoFrecuente_seReiniciaCadaMes(){
        var tarjeta = new Tarjeta();
        for (int i = 0; i < 5; i++){
            tarjeta.cargarTarjeta(30000);
        }
        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 1, 10, 0, 0);

        for (int viaje = 1; viaje <= 35; viaje++){
            colectivo.pagarCon(tarjeta, hora.AddMinutes(viaje));
        }
        var boletoOtroMes = colectivo.pagarCon(tarjeta, new DateTime(2026, 11, 2, 10, 0, 0));

        Assert.That(boletoOtroMes.TotalAbonado, Is.EqualTo(2150));
    }

    [Test]
    public void usoFrecuente_soloParaTarjetaNormal(){
        var tipo = new TarjetaTipo(2, "Medio boleto estudiantil", 50) { ViajesConDescuentoPorDia = 2, MinutosEntreViajes = 5 };
        var tarjeta = new Tarjeta(tipo);
        for (int i = 0; i < 5; i++){
            tarjeta.cargarTarjeta(30000);
        }
        var colectivo = new Colectivo { Linea_numero = 67 };
        var hora = new DateTime(2026, 10, 1, 10, 0, 0);

        for (int viaje = 1; viaje <= 29; viaje++){
            colectivo.pagarCon(tarjeta, hora.AddMinutes(viaje * 10));
        }
        var boleto30 = colectivo.pagarCon(tarjeta, hora.AddMinutes(300));

        Assert.That(boleto30.TotalAbonado, Is.EqualTo(2150));
    }
}
