use TiendaDSI
go

exec ListarProductos
exec ListarFacturas
exec ListarProductosDeshabilitados
exec ObtenerProducto 1
exec ActualizarEstadoProducto 1, 1
exec CrearProducto '1236', 'Computadora HP', 10500.25, 5500.60, 20, 2, 40
exec EditarProducto 1, '4321', 'Computadora ASUS', 12500.25, 7500.60, 9, 10, 100

exec CrearFactura  1, 1234, 49
exec ListarFacturas
