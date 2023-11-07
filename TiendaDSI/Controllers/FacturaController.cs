using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using TiendaDSI.Models;

namespace TiendaDSI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        public readonly string db;

        public FacturaController(IConfiguration configuration)
        {
            db = configuration.GetConnectionString("Database");
        }

        [HttpGet]
        public IActionResult obtenerFacturas()
        {
            List<Factura> facturas = new();

            try
            {
                using (SqlConnection cn = new(db))
                {
                    cn.Open();

                    using (SqlCommand cmd = new("ListarFacturas", cn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                int idFactura = Convert.ToInt32(rd["id"]);
                                List<ProductoFacturado> productos = new();

                                using (SqlConnection cn2 = new(db)) {
                                    cn2.Open();

                                    using (SqlCommand cmd2 = new("ObtenerProductosDeFactura", cn2))
                                    {
                                        cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                                        cmd2.Parameters.AddWithValue("@idFactura", idFactura);

                                        using (SqlDataReader rd2 = cmd2.ExecuteReader())
                                        {
                                            while (rd2.Read()) {
                                                productos.Add(new ProductoFacturado()
                                                {
                                                    Cantidad = Convert.ToInt32(rd2["CantidadProducto"]),
                                                    Codigo = rd2["CodigoProducto"].ToString(),
                                                    Id = Convert.ToInt32(rd2["IdProducto"]),
                                                    Nombre = rd2["NombreProducto"].ToString(),
                                                    Precio = Convert.ToDecimal(rd2["PrecioProducto"]),
                                                    Subtotal = Convert.ToDecimal(rd2["Subtotal"])
                                                });
                                            }
                                        }
                                    }
                                }

                                facturas.Add(new Factura
                                {
                                    IdFactura = idFactura,
                                    Fecha = Convert.ToDateTime(rd["fecha"]),
                                    NoFactura = Convert.ToInt32(rd["noFactura"]),
                                    Productos = productos,
                                });
                            }
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = facturas });
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al obtener facturas" });
            }
        }

        [HttpPost]
        public IActionResult crearFactura([FromBody] FacturaDTO factura)
        {
            try
            { 
                using (SqlConnection cn = new(db))
                {
                    cn.Open();

                    using (SqlCommand cmd = new("CrearFactura", cn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@noFactura", factura.NoFactura);

                        cmd.ExecuteNonQuery();

                        // Obtener el ID de la factura recién insertada
                        /*cmd.CommandText = "SELECT SCOPE_IDENTITY()";
                        int facturaId = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine(facturaId)*/

                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "SELECT @@IDENTITY";
                        int facturaId = Convert.ToInt32(cmd.ExecuteScalar());

                        // Insertar productos relacionados a la factura
                        foreach (var producto in factura.Productos)
                        {
                            cmd.CommandText = "AgregarProductoAFactura";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Clear();

                            cmd.Parameters.AddWithValue("@idFactura", facturaId);
                            cmd.Parameters.AddWithValue("@idProducto", producto.IdProducto);
                            cmd.Parameters.AddWithValue("@cantidad", producto.CantidadProducto);

                            cmd.ExecuteNonQuery();
                        }
                    }

                        return StatusCode(StatusCodes.Status201Created, new { message = "OK" });
                    
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Hubo un error al intentar crear la factura" });
            }
        }
    }
}
