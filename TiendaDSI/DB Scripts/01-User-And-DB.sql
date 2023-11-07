use master
go

create login TiendaDSIUser with password = 'TiendaDSIUser'
create database TiendaDSI
use TiendaDSI
go

sp_grantdbaccess 'TiendaDSIUser'
go

grant create table to TiendaDSIUser
grant create procedure to TiendaDSIUser
grant create view to TiendaDSIUser
go