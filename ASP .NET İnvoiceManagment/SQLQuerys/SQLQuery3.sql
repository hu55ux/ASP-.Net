DECLARE @i INT = 1;

-- Sample service adları
DECLARE @Services TABLE (ServiceName NVARCHAR(100));
INSERT INTO @Services (ServiceName)
VALUES 
('Software Development'),
('Consulting'),
('Design Services'),
('Maintenance'),
('Training'),
('Support'),
('Hosting'),
('Licensing');

WHILE @i <= 150
BEGIN
    DECLARE @InvoiceId UNIQUEIDENTIFIER;
    DECLARE @Service NVARCHAR(100);
    DECLARE @Quantity DECIMAL(10,2);
    DECLARE @Amount DECIMAL(10,2);

    -- Random Invoice seçirik
    SELECT TOP 1 @InvoiceId = Id FROM Invoices ORDER BY NEWID();

    -- Random Service seçirik
    SELECT TOP 1 @Service = ServiceName FROM @Services ORDER BY NEWID();

    -- Random Quantity (1-100)
    SET @Quantity = CAST(RAND(CHECKSUM(NEWID())) * 99 + 1 AS DECIMAL(10,2));

    -- Random Amount (10-500)
    SET @Amount = CAST(RAND(CHECKSUM(NEWID())) * 490 + 10 AS DECIMAL(10,2));

    -- Insert
    INSERT INTO InvoiceRows (Id, InvoiceId, Service, Quantity, Amount)
    VALUES (
        NEWID(),
        @InvoiceId,
        @Service,
        @Quantity,
        @Amount
    );

    SET @i = @i + 1;
END
