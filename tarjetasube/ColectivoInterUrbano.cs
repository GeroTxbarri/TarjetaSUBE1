public class ColectivoInterUrbano : Colectivo {
    public ColectivoInterUrbano() {
        Tarifa = 3000;
    }

    public ColectivoInterUrbano(IProveedorDeFecha reloj) : base(reloj) {
        Tarifa = 3000;
    }
}