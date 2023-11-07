namespace TiendaDSI.Models
{
    public class Factura
    {
        public int IdFactura { get; set; }
        public int NoFactura { get; set; }
        public DateTime Fecha { get; set; }
        public List<ProductoFacturado>? Productos { get; set; }
    }
}
