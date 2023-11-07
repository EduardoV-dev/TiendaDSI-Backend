use TiendaDSI
go

create procedure ListarProductos 
as begin
  select * from ProductoHabilitado;
end
go

create procedure ListarProductosDeshabilitados
as begin
  select * from ProductoDeshabilitado;
end
go

create procedure ObtenerProducto 
@id int
as begin
  select * from ProductoHabilitado where id = @id
end
go

-- Procedimiento utilitario
create procedure validacionesProducto 
@cantidad int, @minimo int, @maximo int
as begin
   if (@cantidad < @minimo)
	  begin
	     throw 60000, 'Las existencias no pueden ser menor que la cantidad mínima', 16;
	  end 

   if (@cantidad > @maximo)
	  begin
	    throw 60001, 'Las existencias no pueder ser mayor que la cantidad maxima', 16;
	  end
end
go

create procedure CrearProducto 
@codigo varchar(20), @nombre varchar(100), @precio decimal(12,2), @costo decimal(12,2), 
@existencias int, @cantidadMinima int, @cantidadMaxima int, @estado bit = 1
as begin
  set nocount on;
  begin transaction

  begin try
    exec validacionesProducto @existencias, @cantidadMinima, @cantidadMaxima;

    insert into Producto (codigo, nombre, precio, costo, existencias, cantidadMinima, cantidadMaxima, estado) 
    values (@codigo, @nombre, @precio, @costo, @cantidadMaxima, @existencias, @cantidadMinima, @estado);

	commit transaction;
  end try
  begin catch
    throw;
	rollback transaction;
  end catch
end
go

create procedure EditarProducto
@id int, @codigo varchar(20), @nombre varchar(100), @precio decimal(12,2), 
@costo decimal(12,2), @existencias int, @cantidadMinima int, @cantidadMaxima int
as begin
  set nocount on;
  begin transaction;
   
  begin try
    if (not exists(select * from Producto where id = @id))
	  begin 
	    throw 60000, 'El producto que desea editar, no existe', 16;
	  end

    exec validacionesProducto @existencias, @cantidadMinima, @cantidadMaxima;

	update Producto set codigo = @codigo, nombre = @nombre, precio = @precio, costo = @costo,
	  existencias = @existencias, cantidadMinima = @cantidadMinima, cantidadMaxima = @cantidadMaxima 
	  where id = @id

	commit transaction;
  end try
  begin catch
    throw;
	rollback transaction;
  end catch
end
go

create procedure ActualizarEstadoProducto
@id int, @estado bit
as begin
  set nocount on;
  begin transaction;

  begin try
  end try
     
  begin catch
  end catch

  update Producto set estado = @estado where id = @id;
end
go