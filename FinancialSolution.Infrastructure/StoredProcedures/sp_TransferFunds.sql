ALTER PROCEDURE sp_TransferFunds
(
    @SenderAccountNumber NVARCHAR(50),
    @ReceiverAccountNumber NVARCHAR(50),
    @Amount DECIMAL(18,2),
    @Reference NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY

        DECLARE @SenderWalletId UNIQUEIDENTIFIER;
        DECLARE @ReceiverWalletId UNIQUEIDENTIFIER;

        DECLARE @SenderBalance DECIMAL(18,2);

        SELECT
            @SenderWalletId = Id,
            @SenderBalance = Balance
        FROM Wallets
        WHERE AccountNumber = @SenderAccountNumber;

        SELECT
            @ReceiverWalletId = Id
        FROM Wallets
        WHERE AccountNumber = @ReceiverAccountNumber;

        IF @SenderWalletId IS NULL
        BEGIN
            RAISERROR('Sender wallet not found',16,1);
        END

        IF @ReceiverWalletId IS NULL
        BEGIN
            RAISERROR('Receiver wallet not found',16,1);
        END

        IF @SenderBalance < @Amount
        BEGIN
            RAISERROR('Insufficient balance',16,1);
        END

        -- Debit Sender
        UPDATE Wallets
        SET Balance = Balance - @Amount
        WHERE Id = @SenderWalletId;

        -- Credit Receiver
        UPDATE Wallets
        SET Balance = Balance + @Amount
        WHERE Id = @ReceiverWalletId;

       -- Sender Transaction Record
INSERT INTO Transactions
(
    Id,
    WalletId,
    Amount,
    TransactionType,
    Status,
    Reference,
    CreatedAt
)
VALUES
(
    NEWID(),
    @SenderWalletId,
    @Amount,
    1,
    2,
    @Reference,
    GETUTCDATE()
);

-- Receiver Transaction Record
INSERT INTO Transactions
(
    Id,
    WalletId,
    Amount,
    TransactionType,
    Status,
    Reference,
    CreatedAt
)
VALUES
(
    NEWID(),
    @ReceiverWalletId,
    @Amount,
    2,
    2,
    @Reference,
    GETUTCDATE()
);

        COMMIT TRANSACTION;

    END TRY

    BEGIN CATCH

        ROLLBACK TRANSACTION;

        THROW;

    END CATCH
END