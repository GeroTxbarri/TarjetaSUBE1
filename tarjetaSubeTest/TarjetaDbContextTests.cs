using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

[TestFixture]
public class TarjetaDbContextTests
{
    private DbContextOptions<TarjetaDbContext> opciones = null!;

    [SetUp]
    public void Setup()
    {
        // Base en memoria nueva para cada test, no se usa la base real
        opciones = new DbContextOptionsBuilder<TarjetaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    [Test]
    public void GuardaTarjetaConSuTipo()
    {
        var tipo = new TarjetaTipo(2, "Medio boleto estudiantil", 50);
        var tarjeta = new Tarjeta(tipo) { Id = 1 };
        tarjeta.cargarTarjeta(5000);

        using (var db = new TarjetaDbContext(opciones))
        {
            db.Tarjetas.Add(tarjeta);
            db.SaveChanges();
        }

        using (var db = new TarjetaDbContext(opciones))
        {
            var guardada = db.Tarjetas.Include(t => t.Tipo).First();

            Assert.That(guardada.Id, Is.EqualTo(1));
            Assert.That(guardada.Saldo, Is.EqualTo(5000));
            Assert.That(guardada.TarjetaTipoId, Is.EqualTo(2));
            Assert.That(guardada.Tipo.Nombre, Is.EqualTo("Medio boleto estudiantil"));
            Assert.That(guardada.Tipo.PorcentajeDescuento, Is.EqualTo(50));
        }
    }

    [Test]
    public void DosTarjetasPuedenCompartirElMismoTipo()
    {
        var normal = new TarjetaTipo(1, "Normal", 0);

        using (var db = new TarjetaDbContext(opciones))
        {
            db.Tarjetas.Add(new Tarjeta(normal) { Id = 1 });
            db.Tarjetas.Add(new Tarjeta(normal) { Id = 2 });
            db.SaveChanges();
        }

        using (var db = new TarjetaDbContext(opciones))
        {
            Assert.That(db.Tarjetas.Count(), Is.EqualTo(2));
            Assert.That(db.TarjetaTipos.Count(), Is.EqualTo(1));
        }
    }

    [Test]
    public void GuardaColectivo()
    {
        using (var db = new TarjetaDbContext(opciones))
        {
            db.Colectivos.Add(new Colectivo { Linea_numero = 67 });
            db.SaveChanges();
        }

        using (var db = new TarjetaDbContext(opciones))
        {
            var guardado = db.Colectivos.First();

            Assert.That(guardado.Id, Is.Not.EqualTo(0));
            Assert.That(guardado.Linea_numero, Is.EqualTo(67));
        }
    }

    [Test]
    public void GuardaColectivoInterUrbano_SigueCobrandoSuTarifa()
    {
        using (var db = new TarjetaDbContext(opciones))
        {
            db.ColectivosInterUrbanos.Add(new ColectivoInterUrbano { Linea_numero = 101 });
            db.SaveChanges();
        }

        using (var db = new TarjetaDbContext(opciones))
        {
            var guardado = db.Colectivos.First();
            var tarjeta = new Tarjeta();
            tarjeta.cargarTarjeta(5000);

            Boleto boleto = guardado.pagarCon(tarjeta, new DateTime(2026, 10, 22, 10, 0, 0));

            Assert.That(guardado, Is.TypeOf<ColectivoInterUrbano>());
            Assert.That(boleto.TotalAbonado, Is.EqualTo(3000));
        }
    }

    [Test]
    public void GuardaBoletoConSusDatos()
    {
        var tarjeta = new Tarjeta { Id = 7 };
        tarjeta.cargarTarjeta(5000);
        var colectivo = new Colectivo { Linea_numero = 67 };
        var fecha = new DateTime(2026, 10, 22, 10, 0, 0);

        Boleto boleto = colectivo.pagarCon(tarjeta, fecha);

        using (var db = new TarjetaDbContext(opciones))
        {
            db.Boletos.Add(boleto);
            db.SaveChanges();
        }

        using (var db = new TarjetaDbContext(opciones))
        {
            var guardado = db.Boletos.First();

            Assert.That(guardado.Id, Is.Not.EqualTo(0));
            Assert.That(guardado.Fecha, Is.EqualTo(fecha));
            Assert.That(guardado.TarjetaTipo, Is.EqualTo("Normal"));
            Assert.That(guardado.TotalAbonado, Is.EqualTo(2150));
            Assert.That(guardado.LineaColectivo, Is.EqualTo(67));
            Assert.That(guardado.SaldoRestante, Is.EqualTo(5000 - 2150));
            Assert.That(guardado.IdTarjeta, Is.EqualTo(7));
            Assert.That(guardado.EsTrasbordo, Is.False);
        }
    }

    [Test]
    public void SinOpciones_UsaSqlite()
    {
        using var db = new TarjetaDbContext();

        Assert.That(db.Database.ProviderName, Does.Contain("Sqlite"));
    }
}
