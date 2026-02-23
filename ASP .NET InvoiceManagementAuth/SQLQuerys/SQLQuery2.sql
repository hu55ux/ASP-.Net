-- 150 invoice insert üçün döngü
DECLARE @i INT = 1;

WHILE @i <= 150
BEGIN
    -- Dəyişənlər
    DECLARE @CustomerId UNIQUEIDENTIFIER;
    DECLARE @StartDate DATETIMEOFFSET;
    DECLARE @EndDate DATETIMEOFFSET;
    DECLARE @TotalSum DECIMAL(18,2);
    DECLARE @Status INT;
    DECLARE @DeletedAt DATETIMEOFFSET = NULL;

    -- Random CustomerId seçirik (50 müştəri olduğunu fərz edirik)
    SELECT TOP 1 @CustomerId = Id 
    FROM Customers 
    ORDER BY NEWID();

    -- Random start date 2024-01-01 və 2025-01-01 arasında
    SET @StartDate = DATEADD(DAY, CAST(RAND(CHECKSUM(NEWID())) * 365 AS INT), '2024-01-01');

    -- Random end date 1-30 gün sonrası
    SET @EndDate = DATEADD(DAY, CAST(RAND(CHECKSUM(NEWID())) * 30 + 1 AS INT), @StartDate);

    -- Random total sum 50–5000 AZN
    SET @TotalSum = CAST(RAND(CHECKSUM(NEWID())) * 4950 + 50 AS DECIMAL(18,2));

    -- Random status 0–6 (InvoiceStatus enum)
    SET @Status = CAST(RAND(CHECKSUM(NEWID())) * 7 AS INT);

    -- Soft delete üçün 10% ehtimal
    IF RAND(CHECKSUM(NEWID())) < 0.1
        SET @DeletedAt = SYSDATETIMEOFFSET();

    -- Insert
    INSERT INTO Invoices 
        (Id, CustomerId, StartDate, EndDate, TotalSum, Comment, Status, CreatedAt, UpdatedAt, DeletedAt)
    VALUES 
        (NEWID(), @CustomerId, @StartDate, @EndDate, @TotalSum, 
         'Sample invoice generated automatically', @Status, SYSDATETIMEOFFSET(), NULL, @DeletedAt);

    -- Döngü sayğacını artırırıq
    SET @i = @i + 1;
END
