CREATE PROCEDURE sp_CreateWallet 
(
  @CustomerId UNIQUEIDENTIFIER,
  @CurrencyId UNIQUEIDENTIFIER,
  @AccountNumber NVARCHAR(50)


)
AS
BEGIN 
  SET NOCOUNT ON;

  INSERT INTO Wallets(
  Id, 
  CustomerId, 
  CurrencyId, 
  AccountNumber, 
  Balance, 
  IsActive, 
  CreatedAt
  )
  VALUES
  (
	NEWID(), 
	@CustomerId, 
	@CurrencyId, 
	@AccountNumber, 
	0.00, 
	1, 
	GETUTCDATE()
  )
  END


 