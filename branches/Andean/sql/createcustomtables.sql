create table dbo.CLS_SINGULARITY
(
     ID T_GUID 
)
go
create table dbo.ASSN_SINGULARITY_CHILDREN
(
    LEFTID T_GUID,
	RIGHTID T_GUID
)
go
CREATE TRIGGER dbo.CASDEL_ASSN_SINGULARITY_CHILDREN_LEFT 
   ON  dbo.CLS_SINGULARITY 
   AFTER DELETE 
AS 
BEGIN
	SET NOCOUNT ON;

	DECLARE @id uniqueidentifier
	DECLARE @del_cursor cursor  
 
	SET @del_cursor = CURSOR FOR  
		SELECT ID FROM DELETED
 
	OPEN @del_cursor  
 
	FETCH NEXT FROM @del_cursor INTO @id 
 
	WHILE (@@FETCH_STATUS = 0)  
	BEGIN  
		DELETE FROM dbo.ASSN_SINGULARITY_CHILDREN WHERE LEFTID = @id
		FETCH NEXT FROM @del_cursor INTO @id 
	END  
 
	CLOSE @del_cursor 

END
go

CREATE TRIGGER dbo.CASDEL_ASSN_SINGULARITY_CHILDREN_RIGHT 
   ON  dbo.BUSINESSOBJECT 
   AFTER DELETE 
AS 
BEGIN
	SET NOCOUNT ON;

	DECLARE @id uniqueidentifier
	DECLARE @del_cursor cursor  
 
	SET @del_cursor = CURSOR FOR  
		SELECT ID FROM DELETED
 
	OPEN @del_cursor  
 
	FETCH NEXT FROM @del_cursor INTO @id 
 
	WHILE (@@FETCH_STATUS = 0)  
	BEGIN  
		DELETE FROM dbo.ASSN_SINGULARITY_CHILDREN WHERE RIGHTID = @id
		FETCH NEXT FROM @del_cursor INTO @id 
	END  
 
	CLOSE @del_cursor 

END
GO