use TiendaDSI
go

create procedure ListarFacturas
as begin
  select * from Factura
end
go

create procedure ObtenerProductosDeFactura
@idFactura int
as begin
  select * from ProductosDeFactura where IdFactura = @idFactura
end
go

create procedure CrearFactura 
@noFactura int
as begin
  set nocount on;
  insert into Factura (noFactura) values (@noFactura);
end;
go

create procedure AgregarProductoAFactura
@idProducto int, @idFactura int, @cantidad int
as begin
  set nocount on;

  declare @precioProducto decimal(12,2) = (select precio from Producto where id = @idProducto);

  insert into Factura_Producto (cantidad, precio, subtotal, idFactura, idProducto) 
    values (@cantidad, @precioProducto, @cantidad * @precioProducto, @idFactura, @idProducto)
end
go