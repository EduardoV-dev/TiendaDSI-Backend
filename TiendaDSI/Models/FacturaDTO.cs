namespace TiendaDSI.Models
{
    public class FacturaDTO
    {
        public int NoFactura { get; set; }
        public List<FacturaProductoDTO> Productos { get; set; }
    }
}
