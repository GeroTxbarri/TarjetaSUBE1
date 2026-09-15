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

// Viajar en colectivo
var colectivo = new Colectivo { Linea_numero = 67 };
Console.WriteLine($"\nViajando en colectivo línea {colectivo.Linea_numero}...");
var boleto = colectivo.pagarCon(tarjeta);
Console.WriteLine($"Boleto emitido — Tarifa: ${boleto.Tarifa} | Línea: {boleto.LineaColectivo}");
Console.WriteLine($"Saldo restante: ${tarjeta.Saldo}");

// Intento de viaje sin saldo suficiente
Console.WriteLine("\nVaciando tarjeta y probando viajar sin saldo...");
tarjeta.descontarSaldo(tarjeta.Saldo); // vaciamos
Console.WriteLine($"Saldo actual: ${tarjeta.Saldo}");
var boletoDenegado = colectivo.pagarCon(tarjeta);
Console.WriteLine($"Saldo después del intento: ${tarjeta.Saldo} (no descontó)");
Console.WriteLine($"Tarifa del boleto: ${boletoDenegado.Tarifa} (boleto generado igual — ver nota)");

Console.WriteLine("\n=== Fin de la simulación ===");
