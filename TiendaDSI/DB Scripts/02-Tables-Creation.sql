use TiendaDSI
go

create table Producto (
  id int identity(1,1) primary key,
  codigo varchar(20) not null,
  nombre varchar(100) not null,
  precio decimal(12,2) not null,
  costo decimal(12,2) not null,
  existencias int not null,
  cantidadMinima int not null,
  cantidadMaxima int not null,
  estado bit default 1,
)
go

create table Factura (
  id int identity(1,1) primary key,
  noFactura int not null,
  fecha datetime default getdate()
)
go

create table Factura_Producto (
  id int identity(1,1) primary key,
  precio decimal(12,2) not null,
  cantidad int not null,
  subtotal decimal(12,2) not null,
  idFactura int foreign key references Factura(id),
  idProducto int foreign key references Producto(id),
)
go