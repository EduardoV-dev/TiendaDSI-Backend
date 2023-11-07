namespace TiendaDSI.Models
{
    public class Producto
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public decimal precio { get; set; }
        public decimal costo { get; set; }
        public int existencias { get; set; }
        public int cantidadMinima { get; set; }
        public int cantidadMaxima { get; set; }
    }
}
