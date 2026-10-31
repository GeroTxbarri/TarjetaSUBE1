using NUnit.Framework;

[TestFixture]
public class EstacionBiciTests
{
    // =====================================================================
    // 1. RETIRO EXITOSO - tarifa correcta, sin demora, saldo suficiente
    // =====================================================================

    [Test]
    public void RetirarBici_ConSaldoSuficiente_CobraLaTarifaYPermiteRetiro()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var estacion = new EstacionBici();
        var inicio = new DateTime(2026, 11, 1, 9, 0, 0);

        var resultado = estacion.retirarBici(tarjeta, inicio);

        Assert.That(resultado, Is.True);
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 2418.75m));
        Assert.That(tarjeta.InicioUsoBici, Is.EqualTo(inicio));
    }

    [Test]
    public void RetirarYDevolver_SinDemora_NoGeneraMulta()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var estacion = new EstacionBici();
        var inicio = new DateTime(2026, 11, 1, 9, 0, 0);
        var devolucion = inicio.AddMinutes(59); // Dentro del límite de 60 min

        estacion.retirarBici(tarjeta, inicio);
        estacion.devolverBici(tarjeta, devolucion);

        Assert.That(tarjeta.MultasBici, Is.EqualTo(0));
        Assert.That(tarjeta.InicioUsoBici, Is.Null);
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 2418.75m));
    }

    [Test]
    public void RetirarYDevolver_JustoEnElLimite_NoGeneraMulta()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var estacion = new EstacionBici();
        var inicio = new DateTime(2026, 11, 1, 9, 0, 0);
        var devolucion = inicio.AddMinutes(60); // Exactamente 60 min: no es mayor a 60, no genera multa

        estacion.retirarBici(tarjeta, inicio);
        estacion.devolverBici(tarjeta, devolucion);

        Assert.That(tarjeta.MultasBici, Is.EqualTo(0));
    }

    // =====================================================================
    // 2. NO SE PUEDE RETIRAR - saldo insuficiente (con y sin multa pendiente)
    // =====================================================================

    [Test]
    public void RetirarBici_SinSaldo_NoPermiteRetiro()
    {
        var tarjeta = new Tarjeta(); // Saldo = 0

        var estacion = new EstacionBici();
        var fecha = new DateTime(2026, 11, 1, 9, 0, 0);

        var resultado = estacion.retirarBici(tarjeta, fecha);

        Assert.That(resultado, Is.False);
        Assert.That(tarjeta.InicioUsoBici, Is.Null);
        Assert.That(tarjeta.Saldo, Is.EqualTo(0));
    }

    [Test]
    public void RetirarBici_SaldoInsuficienteSinMulta_NoPermiteRetiro()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(2000); // Saldo 2000, tarifa es 2418.75

        var estacion = new EstacionBici();
        var fecha = new DateTime(2026, 11, 1, 9, 0, 0);

        var resultado = estacion.retirarBici(tarjeta, fecha);

        Assert.That(resultado, Is.False);
        Assert.That(tarjeta.InicioUsoBici, Is.Null);
        Assert.That(tarjeta.Saldo, Is.EqualTo(2000)); // No se descontó nada
    }

    [Test]
    public void RetirarBici_ConMultaPendienteYSaldoInsuficiente_NoPermiteRetiro()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(3000); // Alcanza para la tarifa ($2418.75) pero no para tarifa + 1 multa ($3668.75)

        var estacion = new EstacionBici();
        var dia1 = new DateTime(2026, 11, 1, 9, 0, 0);
        var dia1DevolucionTardia = dia1.AddMinutes(90); // Devuelve tarde: genera 1 multa
        var dia2 = new DateTime(2026, 11, 2, 9, 0, 0);  // Nuevo día con multa pendiente

        // Día 1: retira y devuelve tarde
        estacion.retirarBici(tarjeta, dia1);
        estacion.devolverBici(tarjeta, dia1DevolucionTardia);

        Assert.That(tarjeta.MultasBici, Is.EqualTo(1));
        // Saldo restante después del día 1: 3000 - 2418.75 = 581.25

        // Día 2: intenta retirar pero no le alcanza para tarifa ($2418.75) + multa ($1250) = $3668.75
        var resultado = estacion.retirarBici(tarjeta, dia2);

        Assert.That(resultado, Is.False);
        Assert.That(tarjeta.InicioUsoBici, Is.Null);
        Assert.That(tarjeta.Saldo, Is.EqualTo(3000 - 2418.75m)); // Saldo no cambió en día 2
        Assert.That(tarjeta.MultasBici, Is.EqualTo(1)); // La multa sigue pendiente
    }

    [Test]
    public void RetirarBici_MismoDia_ConMultaYSaldoInsuficiente_NoPermiteRetiro()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var estacion = new EstacionBici();
        var inicio = new DateTime(2026, 11, 1, 9, 0, 0);
        var devolucionTardia = inicio.AddMinutes(90); // Genera multa
        var segundoRetiro = inicio.AddMinutes(100);

        // Retira y devuelve tarde: genera 1 multa
        estacion.retirarBici(tarjeta, inicio);
        // Saldo: 5000 - 2418.75 = 1581.25
        estacion.devolverBici(tarjeta, devolucionTardia); // MultasBici = 1

        // Forzamos saldo insuficiente para pagar la multa ($1250)
        tarjeta.Saldo = 0m;

        var resultado = estacion.retirarBici(tarjeta, segundoRetiro);

        Assert.That(resultado, Is.False);
        Assert.That(tarjeta.InicioUsoBici, Is.Null);
        Assert.That(tarjeta.MultasBici, Is.EqualTo(1)); // La multa sigue pendiente
    }

    // =====================================================================
    // 3. RETIRO CON UNA O VARIAS MULTAS ACUMULADAS
    // =====================================================================

    [Test]
    public void RetirarBici_ConUnaMultaAcumulada_CobraLaTarifaMasLaMulta()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000); // 5000 para el día 1
        tarjeta.cargarTarjeta(5000); // 5000 para el día 2

        var estacion = new EstacionBici();
        var dia1 = new DateTime(2026, 11, 1, 9, 0, 0);
        var dia1DevolucionTardia = dia1.AddMinutes(90); // Tarde: genera 1 multa
        var dia2 = new DateTime(2026, 11, 2, 9, 0, 0);

        estacion.retirarBici(tarjeta, dia1);
        estacion.devolverBici(tarjeta, dia1DevolucionTardia);

        decimal saldoAntesDia2 = tarjeta.Saldo;
        Assert.That(tarjeta.MultasBici, Is.EqualTo(1));

        var resultado = estacion.retirarBici(tarjeta, dia2);

        Assert.That(resultado, Is.True);
        Assert.That(tarjeta.MultasBici, Is.EqualTo(0)); // Multas cobradas y reseteadas
        // Cobró tarifa diaria ($2418.75) + 1 multa ($1250) = $3668.75
        Assert.That(tarjeta.Saldo, Is.EqualTo(saldoAntesDia2 - 2418.75m - 1250m));
    }

    [Test]
    public void RetirarBici_ConVariasMultasAcumuladas_CobraTodasLasMultasJuntas()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(10000);

        var estacion = new EstacionBici();

        // Día 1: retira y devuelve tarde (multa 1)
        var dia1 = new DateTime(2026, 11, 1, 9, 0, 0);
        estacion.retirarBici(tarjeta, dia1);
        estacion.devolverBici(tarjeta, dia1.AddMinutes(90));
        Assert.That(tarjeta.MultasBici, Is.EqualTo(1));

        // Forzamos una segunda multa directamente para simular multas acumuladas
        tarjeta.MultasBici = 2;

        // Día 2: retira pagando tarifa + 2 multas acumuladas
        var dia2 = new DateTime(2026, 11, 2, 9, 0, 0);
        decimal saldoAntesDia2 = tarjeta.Saldo;

        var resultado = estacion.retirarBici(tarjeta, dia2);

        Assert.That(resultado, Is.True);
        Assert.That(tarjeta.MultasBici, Is.EqualTo(0));
        // Cobró: tarifa ($2418.75) + 2 multas ($1250 * 2 = $2500) = $4918.75
        Assert.That(tarjeta.Saldo, Is.EqualTo(saldoAntesDia2 - 2418.75m - 1250m * 2));
    }

    [Test]
    public void Devolver_Tarde_GeneraMulta_Y_SiguienteRetiroMismoDiaCobraMulta()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(10000);

        var estacion = new EstacionBici();
        var inicio = new DateTime(2026, 11, 1, 9, 0, 0);
        var devolucionTardia = inicio.AddMinutes(90); // Genera multa
        var segundoRetiro = devolucionTardia.AddMinutes(5);

        estacion.retirarBici(tarjeta, inicio);
        estacion.devolverBici(tarjeta, devolucionTardia);

        Assert.That(tarjeta.MultasBici, Is.EqualTo(1));

        decimal saldoAntesSegundoRetiro = tarjeta.Saldo;
        var resultado = estacion.retirarBici(tarjeta, segundoRetiro);

        Assert.That(resultado, Is.True);
        Assert.That(tarjeta.MultasBici, Is.EqualTo(0));
        // En el mismo día: no cobra tarifa diaria, solo cobra la multa ($1250)
        Assert.That(tarjeta.Saldo, Is.EqualTo(saldoAntesSegundoRetiro - 1250m));
    }

    // =====================================================================
    // 4. CASOS ADICIONALES
    // =====================================================================

    [Test]
    public void RetirarBici_MismoDia_SinMultas_NoCobra()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var estacion = new EstacionBici();
        var inicio = new DateTime(2026, 11, 1, 9, 0, 0);
        var devolucion = inicio.AddMinutes(30);
        var segundoRetiro = inicio.AddMinutes(45);

        estacion.retirarBici(tarjeta, inicio);
        estacion.devolverBici(tarjeta, devolucion);

        decimal saldoAntes = tarjeta.Saldo;
        var resultado = estacion.retirarBici(tarjeta, segundoRetiro);

        Assert.That(resultado, Is.True);
        Assert.That(tarjeta.Saldo, Is.EqualTo(saldoAntes)); // No se cobró nada el mismo día
    }

    [Test]
    public void RetirarBici_ConBiciEnUso_NoPermiteSegundoRetiro()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(5000);

        var estacion = new EstacionBici();
        var inicio = new DateTime(2026, 11, 1, 9, 0, 0);

        estacion.retirarBici(tarjeta, inicio);
        var resultado = estacion.retirarBici(tarjeta, inicio.AddMinutes(10));

        Assert.That(resultado, Is.False);
    }

    [Test]
    public void RetirarBici_ConFranquicia_CobraLaTarifaCompleta_SinDescuento()
    {
        var tipoMedioBoleto = new TarjetaTipo(2, "Medio boleto estudiantil", 50) { ViajesConDescuentoPorDia = 2, MinutosEntreViajes = 5 };
        var tarjeta = new Tarjeta(tipoMedioBoleto);
        tarjeta.cargarTarjeta(5000);

        var estacion = new EstacionBici();
        var fecha = new DateTime(2026, 11, 1, 10, 0, 0);

        var resultado = estacion.retirarBici(tarjeta, fecha);

        Assert.That(resultado, Is.True);
        // Cobra la tarifa completa ($2418.75), sin descuento del 50% del medio boleto
        Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - 2418.75m));
    }

    [Test]
    public void RetirarBici_ConSaldoPendiente_AcreditaHastaElMaximo()
    {
        var tarjeta = new Tarjeta();
        tarjeta.cargarTarjeta(30000);
        tarjeta.cargarTarjeta(30000); // Saldo 56000, quedan 4000 pendientes

        var estacion = new EstacionBici();
        var fecha = new DateTime(2026, 11, 1, 9, 0, 0);

        var resultado = estacion.retirarBici(tarjeta, fecha);

        Assert.That(resultado, Is.True);
        Assert.That(tarjeta.Saldo, Is.EqualTo(56000));
        Assert.That(tarjeta.SaldoPendiente, Is.EqualTo(4000 - 2418.75m));
    }
}
