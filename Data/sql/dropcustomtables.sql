use fantasy
go

declare @tablename varchar(64)

declare tc cursor for select tableschema + '.' + tablename from BUSINESSASSOCIATION union select tableschema + '.' + tablename from businessclass where PARENTCLASSID is not null

open tc
fetch next from tc
into @tablename

while @@fetch_status = 0
begin
    if exists (select 1 from  sysobjects where  id = object_id(@tablename) and   type = 'U')
           exec ('drop table ' + @tablename)
	

	fetch next from tc
	into @tablename
end

close tc
deallocate tc

