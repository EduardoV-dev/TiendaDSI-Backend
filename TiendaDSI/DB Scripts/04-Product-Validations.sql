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

create procedure validarSiProductoExiste
@id int
as begin
  if (not exists (select * from Producto where id = @id))
    begin
	  throw 60000, 'El producto no existe', 16;
	end
end
go