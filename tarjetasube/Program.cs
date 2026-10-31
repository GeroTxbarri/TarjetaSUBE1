// Simulación básica del sistema de Tarjeta SUBE

var tarjeta = new Tarjeta();

Console.WriteLine("=== Tarjeta SUBE - Simulación ===\n");

// Cargar saldo
Console.WriteLine("Cargando $5000...");
tarjeta.cargarTarjeta(5000);
Console.WriteLine($"Saldo: ${tarjeta.Saldo}");

// Intento de carga inválida
Console.WriteLine("\nIntentando cargar $1234 (monto inválido)...");
tarjeta.cargarTarjeta(1234);
Console.WriteLine($"Saldo (sin cambios): ${tarjeta.Saldo}");

// Viajar en colectivo (fecha explícita para no depender del horario real)
var colectivo = new Colectivo { Linea_numero = 67 };
Console.WriteLine($"\nViajando en colectivo línea {colectivo.Linea_numero}...");
var fecha = new DateTime(2026, 10, 22, 10, 0, 0);
var resultado = colectivo.pagarCon(tarjeta, fecha);
if (resultado is Boleto boleto)
{
    Console.WriteLine($"Boleto emitido — Total abonado: ${boleto.TotalAbonado} | Línea: {boleto.LineaColectivo}");
    Console.WriteLine($"Saldo restante: ${tarjeta.Saldo}");
}

// Intento de viaje sin saldo suficiente
Console.WriteLine("\nVaciando tarjeta y probando viajar sin saldo...");
tarjeta.Saldo = 0;
Console.WriteLine($"Saldo actual: ${tarjeta.Saldo}");
var resultadoDenegado = colectivo.pagarCon(tarjeta, fecha.AddMinutes(5));
if (resultadoDenegado is bool denegado && !denegado)
{
    Console.WriteLine("Pago denegado: saldo insuficiente.");
}
else
{
    Console.WriteLine($"Boleto emitido — Total abonado: ${((Boleto)resultadoDenegado).TotalAbonado}");
}

Console.WriteLine("\n=== Fin de la simulación ===");
