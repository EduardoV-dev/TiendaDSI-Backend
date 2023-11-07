use TiendaDSI
go

create trigger AI_Actualizar_Cantidad on Factura_Producto
after insert
as
  set nocount on;
  declare @idProducto int, @cantidadVendida int;
  select @idProducto = idProducto, @cantidadVendida = cantidad from inserted;

  declare @cantidadExistente int = (select existencias from Producto where id = @idProducto)
  declare @nuevaCantidad int = @cantidadExistente - @cantidadVendida;

  update Producto set existencias = @nuevaCantidad where id = @idProducto
go