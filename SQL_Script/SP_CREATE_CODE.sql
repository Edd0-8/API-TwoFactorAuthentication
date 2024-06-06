CREATE PROCEDURE SP_CREATE_CODE 
(
	@email varchar(250),
	@etiqueta varchar(10),
	@code varchar(32)
)
AS
BEGIN 
	DECLARE @IdUsuario INT
	IF NOT exists (SELECT * FROM Usuario where Email = @email)
		BEGIN
			INSERT INTO Usuario VALUES (@email)		
		END
	SELECT @IdUsuario = Id FROM Usuario WHERE Email = @Email;
	IF exists (SELECT * FROM TwoFactorSecret WHERE Etiqueta = @etiqueta AND IdUsuario = @IdUsuario)
		BEGIN
			UPDATE TwoFactorSecret SET Token = @code WHERE Etiqueta = @etiqueta AND IdUsuario = @IdUsuario
		END
	ELSE
		BEGIN
			INSERT INTO TwoFactorSecret VALUES (@etiqueta, @IdUsuario, @code)
		END
	
END
