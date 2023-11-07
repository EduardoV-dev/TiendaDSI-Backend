using Microsoft.AspNetCore.Mvc;
using TiendaDSI.Models;
using System.Data.SqlClient;
using System.Data;

namespace TiendaDSI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly string dbString;

        public ProductoController(IConfiguration configuration)
        {
            dbString = configuration.GetConnectionString("Database");
        }

        [HttpGet]
        public IActionResult obtenerProductos()
        {
            try
            {
                List<Producto> productos = new();

                using (SqlConnection connection = new(dbString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new("ListarProductos", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(new Producto
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    codigo = reader["codigo"].ToString(),
                                    nombre = reader["nombre"].ToString(),
                                    precio = Convert.ToDecimal(reader["precio"]),
                                    costo = Convert.ToDecimal(reader["costo"]),
                                    existencias = Convert.ToInt32(reader["existencias"]),
                                    cantidadMinima = Convert.ToInt32(reader["existencias"]),
                                    cantidadMaxima = Convert.ToInt32(reader["cantidadMaxima"])
                                });
                            }
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = productos });
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Hubo un error al intentar obtener los productos" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult obtenerProductoPorId(int id)
        {
            try
            {
                using (SqlConnection connection = new(dbString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new("ObtenerProducto", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read()) return StatusCode(StatusCodes.Status404NotFound, new { message = "El producto no existe" });

                            Producto producto = new() {
                                id = Convert.ToInt32(reader["id"]),
                                codigo = reader["codigo"].ToString(),
                                nombre = reader["nombre"].ToString(),
                                precio = Convert.ToDecimal(reader["precio"]),
                                costo = Convert.ToDecimal(reader["costo"]),
                                existencias = Convert.ToInt32(reader["existencias"]),
                                cantidadMinima = Convert.ToInt32(reader["existencias"]),
                                cantidadMaxima = Convert.ToInt32(reader["cantidadMaxima"])
                            };

                            return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = producto });
                        }
                    }
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "No se pudo obtener el producto" });
            }
        }

        [HttpPost]
        public IActionResult crearProducto([FromBody] Producto producto)
        {
            try
            { 

                using (SqlConnection connection = new(dbString))
                {
                    connection.Open();

                    using (SqlCommand command = new("CrearProducto", connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@codigo", producto.codigo);
                        command.Parameters.AddWithValue("@nombre", producto.nombre);
                        command.Parameters.AddWithValue("@precio", producto.precio);
                        command.Parameters.AddWithValue("@costo", producto.costo);
                        command.Parameters.AddWithValue("@existencias", producto.existencias);
                        command.Parameters.AddWithValue("@cantidadMinima", producto.cantidadMinima);
                        command.Parameters.AddWithValue("@cantidadMaxima", producto.cantidadMaxima);

                        command.ExecuteNonQuery();

                        return StatusCode(StatusCodes.Status201Created, new { message = "OK" });
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Number == 60000 || ex.Number == 60001) // Custom errors from SQL Server 
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Hubo un error al intentar crear el producto" });
            }
        }

        [HttpPut("{id}")]
        public IActionResult editarProducto([FromBody] Producto producto, int id)
        {
            try { 
                using (SqlConnection connection = new(dbString))
                {
                    connection.Open();

                    using (SqlCommand command = new("EditarProducto", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@codigo", producto.codigo);
                        command.Parameters.AddWithValue("@nombre", producto.nombre);
                        command.Parameters.AddWithValue("@precio", producto.precio);
                        command.Parameters.AddWithValue("@costo", producto.costo);
                        command.Parameters.AddWithValue("@existencias", producto.existencias);
                        command.Parameters.AddWithValue("@cantidadMinima", producto.cantidadMinima);
                        command.Parameters.AddWithValue("@cantidadMaxima", producto.cantidadMaxima);

                        command.ExecuteNonQuery();

                        return StatusCode(StatusCodes.Status200OK, new { message = "OK" });
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Number >= 60000) // Custom errors from SQL Server 
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Hubo un error al intentar editar el producto" });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult deshabilitarProducto (int id) 
        {
            try
            {
                using (SqlConnection connection = new(dbString))
                {
                    connection.Open();

                    using (SqlCommand command = new("ActualizarEstadoProducto", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@estado", 0);

                        command.ExecuteNonQuery();

                        return StatusCode(StatusCodes.Status200OK, new { message = "OK" });
                    }
                }
            }
            catch (SqlException ex)
            { 
                if (ex.Number >= 60000)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = ex.Message });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Hubo un error al intentar eliminar el producto" });
            }
        }
    }
}
