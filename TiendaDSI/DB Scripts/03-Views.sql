use TiendaDSI
go

create View ProductoHabilitado
as
  select id, codigo, nombre, precio, costo, existencias, 
    cantidadMinima, cantidadMaxima from Producto where estado = 1;
go

create view ProductoDeshabilitado
as 
  select id, codigo, nombre, precio, costo, existencias, 
    cantidadMinima, cantidadMaxima from Producto where estado = 0;
go

create View DetalleFactura
as 
   select f.id, f.noFactura, p.codigo as codigo_producto, p.nombre as producto, fp.precio, 
    fp.cantidad, fp.subtotal, f.fecha from Factura_Producto as fp 
	join Factura as f on f.id = fp.id 
	join Producto as p on p.id = fp.idProducto
go

create view ProductosDeFactura
as
  select f.id as IdFactura, f.noFactura as NoFactura, p.id as IdProducto, p.codigo as CodigoProducto, 
  p.nombre as NombreProducto, fp.precio as PrecioProducto, fp.cantidad as CantidadProducto, fp.Subtotal,
  f.fecha as FechaFactura from Factura as f
  inner join Factura_Producto as fp on f.id = fp.idFactura 
  inner join Producto as p on p.id = fp.idProducto
go
