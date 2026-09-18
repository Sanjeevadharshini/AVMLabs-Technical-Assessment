-- AVMLabs Technical Assessment
-- Module 1 - SQL
-- Database Schema + Queries

IF DB_ID('AVMLabsDB') IS NULL
BEGIN
    CREATE DATABASE AVMLabsDB;
END
GO

USE AVMLabsDB;
GO

-- 1. Clients table
CREATE TABLE Clients
(
    ClientId INT IDENTITY(1,1) NOT NULL,
    ClientName NVARCHAR(150) NOT NULL,
    ContactPerson NVARCHAR(150) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    Country NVARCHAR(100) NOT NULL,
    CreditLimit DECIMAL(18,2) NOT NULL,
    IsActive BIT NOT NULL,
    CreatedOn DATETIME2(0) NOT NULL,
    UpdatedOn DATETIME2(0) NULL,

    CONSTRAINT PK_Clients PRIMARY KEY (ClientId),
    CONSTRAINT UQ_Clients_Email UNIQUE (Email),
    CONSTRAINT CK_Clients_CreditLimit CHECK (CreditLimit > 0),
    CONSTRAINT DF_Clients_IsActive DEFAULT (1) FOR IsActive,
    CONSTRAINT DF_Clients_CreatedOn DEFAULT (SYSDATETIME()) FOR CreatedOn
);
GO

-- 2. Tests table
CREATE TABLE Tests
(
    TestId INT IDENTITY(1,1) NOT NULL,
    TestCode VARCHAR(50) NOT NULL,
    TestName NVARCHAR(150) NOT NULL,
    SampleType NVARCHAR(100) NOT NULL,
    TATHours INT NOT NULL,
    Rate DECIMAL(18,2) NOT NULL,
    IsOST BIT NOT NULL,
    IsActive BIT NOT NULL,
    CreatedOn DATETIME2(0) NOT NULL,

    CONSTRAINT PK_Tests PRIMARY KEY (TestId),
    CONSTRAINT UQ_Tests_TestCode UNIQUE (TestCode),
    CONSTRAINT CK_Tests_TATHours CHECK (TATHours > 0),
    CONSTRAINT CK_Tests_Rate CHECK (Rate >= 0),
    CONSTRAINT DF_Tests_IsOST DEFAULT (0) FOR IsOST,
    CONSTRAINT DF_Tests_IsActive DEFAULT (1) FOR IsActive,
    CONSTRAINT DF_Tests_CreatedOn DEFAULT (SYSDATETIME()) FOR CreatedOn
);
GO

-- 3. Work Orders table
CREATE TABLE WorkOrders
(
    WOId INT IDENTITY(1,1) NOT NULL,
    ClientId INT NOT NULL,
    WODate DATE NOT NULL,
    Status VARCHAR(20) NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    CreatedBy NVARCHAR(100) NOT NULL,
    CreatedOn DATETIME2(0) NOT NULL,

    CONSTRAINT PK_WorkOrders PRIMARY KEY (WOId),
    CONSTRAINT FK_WorkOrders_Clients FOREIGN KEY (ClientId) REFERENCES Clients(ClientId),
    CONSTRAINT CK_WorkOrders_Status CHECK (Status IN ('Pending', 'Processing', 'Reported', 'Billed')),
    CONSTRAINT CK_WorkOrders_TotalAmount CHECK (TotalAmount >= 0),
    CONSTRAINT DF_WorkOrders_Status DEFAULT ('Pending') FOR Status,
    CONSTRAINT DF_WorkOrders_CreatedOn DEFAULT (SYSDATETIME()) FOR CreatedOn
);
GO

-- 4. Work Order Items table
CREATE TABLE WorkOrderItems
(
    WOItemId INT IDENTITY(1,1) NOT NULL,
    WOId INT NOT NULL,
    TestId INT NOT NULL,
    Quantity INT NOT NULL,
    Rate DECIMAL(18,2) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    SampleStatus VARCHAR(20) NOT NULL,
    CreatedOn DATETIME2(0) NOT NULL,

    CONSTRAINT PK_WorkOrderItems PRIMARY KEY (WOItemId),
    CONSTRAINT FK_WorkOrderItems_WorkOrders FOREIGN KEY (WOId) REFERENCES WorkOrders(WOId),
    CONSTRAINT FK_WorkOrderItems_Tests FOREIGN KEY (TestId) REFERENCES Tests(TestId),
    CONSTRAINT CK_WorkOrderItems_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_WorkOrderItems_Rate CHECK (Rate >= 0),
    CONSTRAINT CK_WorkOrderItems_Amount CHECK (Amount = Quantity * Rate),
    CONSTRAINT CK_WorkOrderItems_SampleStatus CHECK (SampleStatus IN ('Received', 'InTransit')),
    CONSTRAINT DF_WorkOrderItems_CreatedOn DEFAULT (SYSDATETIME())
);
GO

-- 5. Invoices table
CREATE TABLE Invoices
(
    InvoiceId INT IDENTITY(1,1) NOT NULL,
    ClientId INT NOT NULL,
    WOId INT NOT NULL,
    InvoiceDate DATE NOT NULL,
    DueDate DATE NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status VARCHAR(20) NOT NULL,
    CreatedOn DATETIME2(0) NOT NULL,

    CONSTRAINT PK_Invoices PRIMARY KEY (InvoiceId),
    CONSTRAINT UQ_Invoices_WOId UNIQUE (WOId),
    CONSTRAINT FK_Invoices_Clients FOREIGN KEY (ClientId) REFERENCES Clients(ClientId),
    CONSTRAINT FK_Invoices_WorkOrders FOREIGN KEY (WOId) REFERENCES WorkOrders(WOId),
    CONSTRAINT CK_Invoices_Status CHECK (Status IN ('Pending', 'Paid', 'Overdue')),
    CONSTRAINT CK_Invoices_TotalAmount CHECK (TotalAmount >= 0),
    CONSTRAINT CK_Invoices_DueDate CHECK (DueDate >= InvoiceDate),
    CONSTRAINT DF_Invoices_Status DEFAULT ('Pending') FOR Status,
    CONSTRAINT DF_Invoices_CreatedOn DEFAULT (SYSDATETIME())
);
GO

-- 6. Payments table
CREATE TABLE Payments
(
    PaymentId INT IDENTITY(1,1) NOT NULL,
    InvoiceId INT NOT NULL,
    PaymentDate DATE NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Mode VARCHAR(20) NOT NULL,
    GatewayFee DECIMAL(18,2) NOT NULL,
    NetAmount DECIMAL(18,2) NOT NULL,
    CreatedOn DATETIME2(0) NOT NULL,

    CONSTRAINT PK_Payments PRIMARY KEY (PaymentId),
    CONSTRAINT FK_Payments_Invoices FOREIGN KEY (InvoiceId) REFERENCES Invoices(InvoiceId),
    CONSTRAINT CK_Payments_Amount CHECK (Amount > 0),
    CONSTRAINT CK_Payments_Mode CHECK (Mode IN ('Cash', 'Cheque', 'Online')),
    CONSTRAINT CK_Payments_GatewayFee CHECK (GatewayFee >= 0),
    CONSTRAINT CK_Payments_NetAmount CHECK (NetAmount >= 0),
    CONSTRAINT DF_Payments_GatewayFee DEFAULT (0) FOR GatewayFee,
    CONSTRAINT DF_Payments_CreatedOn DEFAULT (SYSDATETIME())
);
GO

-- Indexes

CREATE INDEX IX_Clients_Country ON Clients(Country);
CREATE INDEX IX_Clients_ClientName ON Clients(ClientName);

CREATE INDEX IX_WorkOrders_ClientId ON WorkOrders(ClientId);
CREATE INDEX IX_WorkOrders_WODate ON WorkOrders(WODate);

CREATE INDEX IX_WorkOrderItems_WOId ON WorkOrderItems(WOId);
CREATE INDEX IX_WorkOrderItems_TestId ON WorkOrderItems(TestId);
CREATE INDEX IX_WorkOrderItems_SampleStatus ON WorkOrderItems(SampleStatus);

CREATE INDEX IX_Invoices_ClientId ON Invoices(ClientId);
CREATE INDEX IX_Invoices_WOId ON Invoices(WOId);

CREATE INDEX IX_Payments_InvoiceId ON Payments(InvoiceId);
CREATE INDEX IX_Payments_PaymentDate ON Payments(PaymentDate);
GO

-- Seed data

INSERT INTO Clients (ClientName, ContactPerson, Phone, Email, City, Country, CreditLimit, IsActive, CreatedOn)
VALUES
('ABC Hospital', 'Ramesh Kumar', '9876543210', 'ramesh@abchospital.com', 'Coimbatore', 'India', 50000.00, 1, SYSDATETIME()),
('City Care Diagnostics', 'Priya Sharma', '9876543211', 'priya@citycare.com', 'Chennai', 'India', 30000.00, 1, SYSDATETIME()),
('Apollo Medical Center', 'Arun Kumar', '9876543212', 'arun@apollomedical.com', 'Bangalore', 'India', 75000.00, 1, SYSDATETIME()),
('Green Life Hospital', 'Meena Raj', '9876543213', 'meena@greenlife.com', 'Coimbatore', 'India', 40000.00, 1, SYSDATETIME()),
('Sri Ram Diagnostics', 'Karthik Raj', '9876543214', 'karthik@sriramdiagnostics.com', 'Dubai', 'UAE', 45000.00, 1, SYSDATETIME()),
('MedCare Hospital', 'Anitha Devi', '9876543215', 'anitha@medcare.com', 'Abu Dhabi', 'UAE', 60000.00, 1, SYSDATETIME()),
('Health First Lab', 'Suresh Kumar', '9876543216', 'suresh@healthfirst.com', 'Manama', 'Bahrain', 35000.00, 1, SYSDATETIME()),
('Prime Care Hospital', 'Divya Raj', '9876543217', 'divya@primecare.com', 'Riyadh', 'Saudi Arabia', 55000.00, 1, SYSDATETIME()),
('Wellness Diagnostics', 'Vijay Anand', '9876543218', 'vijay@wellnessdiagnostics.com', 'Muscat', 'Oman', 25000.00, 1, SYSDATETIME()),
('Care Plus Medical Center', 'Lakshmi Priya', '9876543219', 'lakshmi@careplus.com', 'Trichy', 'India', 70000.00, 1, SYSDATETIME()),
('Global Health Lab', 'Mohan Raj', '9876543220', 'mohan@globalhealth.com', 'Coimbatore', 'India', 50000.00, 1, SYSDATETIME()),
('Sunrise Hospital', 'Deepa Kumar', '9876543221', 'deepa@sunrisehospital.com', 'Chennai', 'India', 65000.00, 1, SYSDATETIME()),
('City Medical Lab', 'Ravi Shankar', '9876543222', 'ravi@citymedical.com', 'Bangalore', 'India', 30000.00, 1, SYSDATETIME()),
('LifeCare Diagnostics', 'Nandhini Raj', '9876543223', 'nandhini@lifecare.com', 'Doha', 'Qatar', 45000.00, 1, SYSDATETIME()),
('Metro Health Center', 'Prakash Kumar', '9876543224', 'prakash@metrohealth.com', 'Salalah', 'Oman', 55000.00, 1, SYSDATETIME());
GO

INSERT INTO Tests (TestCode, TestName, SampleType, TATHours, Rate, IsOST, IsActive, CreatedOn)
VALUES
('CBC001', 'Complete Blood Count', 'Blood', 24, 500.00, 0, 1, SYSDATETIME()),
('LFT001', 'Liver Function Test', 'Blood', 24, 800.00, 0, 1, SYSDATETIME()),
('KFT001', 'Kidney Function Test', 'Blood', 24, 750.00, 0, 1, SYSDATETIME()),
('THY001', 'Thyroid Profile', 'Blood', 48, 1000.00, 0, 1, SYSDATETIME()),
('LIP001', 'Lipid Profile', 'Blood', 24, 650.00, 0, 1, SYSDATETIME()),
('HBA001', 'HbA1c', 'Blood', 12, 450.00, 0, 1, SYSDATETIME()),
('URI001', 'Urine Routine', 'Urine', 12, 300.00, 0, 1, SYSDATETIME()),
('CUL001', 'Urine Culture', 'Urine', 72, 1200.00, 1, 1, SYSDATETIME()),
('PCR001', 'PCR Test', 'Swab', 48, 1500.00, 1, 1, SYSDATETIME()),
('VIT001', 'Vitamin D', 'Blood', 48, 900.00, 1, 1, SYSDATETIME());
GO

INSERT INTO WorkOrders (ClientId, WODate, Status, TotalAmount, CreatedBy, CreatedOn)
VALUES
(1, DATEADD(DAY, -2, CAST(GETDATE() AS DATE)), 'Pending', 2200.00, 'Admin', SYSDATETIME()),
(1, DATEADD(DAY, -5, CAST(GETDATE() AS DATE)), 'Processing', 2900.00, 'Admin', SYSDATETIME()),
(2, DATEADD(DAY, -1, CAST(GETDATE() AS DATE)), 'Reported', 1800.00, 'Admin', SYSDATETIME()),
(3, DATEADD(DAY, -20, CAST(GETDATE() AS DATE)), 'Billed', 3350.00, 'Admin', SYSDATETIME()),
(3, DATEADD(DAY, -15, CAST(GETDATE() AS DATE)), 'Billed', 2500.00, 'Admin', SYSDATETIME()),
(2, DATEADD(DAY, -7, CAST(GETDATE() AS DATE)), 'Processing', 2850.00, 'Admin', SYSDATETIME()),
(5, DATEADD(DAY, -10, CAST(GETDATE() AS DATE)), 'Processing', 2400.00, 'Admin', SYSDATETIME());
GO

INSERT INTO WorkOrderItems (WOId, TestId, Quantity, Rate, Amount, SampleStatus, CreatedOn)
VALUES
(1, 1, 2, 500.00, 1000.00, 'Received', SYSDATETIME()),
(1, 6, 2, 450.00, 900.00, 'Received', SYSDATETIME()),
(1, 7, 1, 300.00, 300.00, 'Received', SYSDATETIME()),

(2, 2, 1, 800.00, 800.00, 'Received', SYSDATETIME()),
(2, 4, 1, 1000.00, 1000.00, 'InTransit', SYSDATETIME()),
(2, 5, 1, 650.00, 650.00, 'InTransit', SYSDATETIME()),
(2, 6, 1, 450.00, 450.00, 'Received', SYSDATETIME()),

(3, 9, 1, 1500.00, 1500.00, 'Received', SYSDATETIME()),
(3, 7, 1, 300.00, 300.00, 'Received', SYSDATETIME()),

(4, 4, 1, 1000.00, 1000.00, 'Received', SYSDATETIME()),
(4, 5, 2, 650.00, 1300.00, 'Received', SYSDATETIME()),
(4, 6, 1, 450.00, 450.00, 'Received', SYSDATETIME()),
(4, 7, 2, 300.00, 600.00, 'Received', SYSDATETIME()),

(5, 5, 2, 650.00, 1300.00, 'Received', SYSDATETIME()),
(5, 6, 2, 450.00, 900.00, 'Received', SYSDATETIME()),
(5, 7, 1, 300.00, 300.00, 'Received', SYSDATETIME()),

(6, 9, 1, 1500.00, 1500.00, 'InTransit', SYSDATETIME()),
(6, 10, 1, 900.00, 900.00, 'Received', SYSDATETIME()),
(6, 6, 1, 450.00, 450.00, 'Received', SYSDATETIME()),

(7, 8, 1, 1200.00, 1200.00, 'Received', SYSDATETIME()),
(7, 3, 1, 750.00, 750.00, 'Received', SYSDATETIME()),
(7, 6, 1, 450.00, 450.00, 'Received', SYSDATETIME());
GO

INSERT INTO Invoices (ClientId, WOId, InvoiceDate, DueDate, TotalAmount, Status, CreatedOn)
VALUES
(3, 4, DATEADD(DAY, -20, CAST(GETDATE() AS DATE)), DATEADD(DAY, 10, CAST(GETDATE() AS DATE)), 3350.00, 'Pending', SYSDATETIME()),
(3, 5, DATEADD(DAY, -15, CAST(GETDATE() AS DATE)), DATEADD(DAY, 15, CAST(GETDATE() AS DATE)), 2500.00, 'Pending', SYSDATETIME());
GO

INSERT INTO Payments (InvoiceId, PaymentDate, Amount, Mode, GatewayFee, NetAmount, CreatedOn)
VALUES
(1, DATEADD(DAY, -3, CAST(GETDATE() AS DATE)), 1000.00, 'Cash', 0.00, 1000.00, SYSDATETIME());
GO

-- Query 1
-- List all active clients along with their total outstanding amount.
-- Outstanding amount = Pending Invoice amount - Payments received.

SELECT C.ClientName, C.City, C.Country, ISNULL(SUM(Inv_Pay.TotalAmount - Inv_Pay.TotalPaid), 0) AS OutstandingAmount
FROM Clients C
LEFT JOIN
(
    SELECT I.ClientId, I.InvoiceId, I.TotalAmount, ISNULL(SUM(P.Amount), 0) AS TotalPaid
    FROM Invoices I
    LEFT JOIN Payments P ON P.InvoiceId = I.InvoiceId
    WHERE I.Status = 'Pending'
    GROUP BY I.ClientId, I.InvoiceId, I.TotalAmount
) Inv_Pay ON Inv_Pay.ClientId = C.ClientId
WHERE C.IsActive = 1
GROUP BY C.ClientId, C.ClientName, C.City, C.Country
ORDER BY OutstandingAmount DESC;


-- Query 2
-- Show a daily Work Order summary for the last 30 days.

;WITH DateCTE AS
(
    SELECT DATEADD(DAY, -29, CAST(GETDATE() AS DATE)) AS WODate

    UNION ALL

    SELECT DATEADD(DAY, 1, WODate)
    FROM DateCTE
    WHERE WODate < CAST(GETDATE() AS DATE)
),
WorkOrderSummary AS
(
    SELECT W.WODate, COUNT(W.WOId) AS TotalWOs,
           SUM(W.TotalAmount) AS TotalRevenue,
           SUM(ISNULL(WI.TotalTests, 0)) AS TotalTests
    FROM WorkOrders W
    LEFT JOIN
    (
        SELECT WOId, SUM(Quantity) AS TotalTests
        FROM WorkOrderItems
        GROUP BY WOId
    ) WI ON WI.WOId = W.WOId
    WHERE W.WODate >= DATEADD(DAY, -29, CAST(GETDATE() AS DATE))
          AND W.WODate <= CAST(GETDATE() AS DATE)
    GROUP BY W.WODate
)
SELECT D.WODate, ISNULL(W.TotalWOs, 0) AS TotalWOs,
       ISNULL(W.TotalTests, 0) AS TotalTests,
       ISNULL(W.TotalRevenue, 0) AS TotalRevenue
FROM DateCTE D
LEFT JOIN WorkOrderSummary W ON W.WODate = D.WODate
ORDER BY D.WODate
OPTION (MAXRECURSION 30);


-- Query 3
-- Find all Work Order Items where SampleStatus = 'InTransit'
-- for more than 48 hours from WODate.

SELECT W.WOId, C.ClientName, T.TestName, W.WODate,
       DATEDIFF(HOUR, W.WODate, GETDATE()) AS HoursElapsed
FROM WorkOrderItems WI
INNER JOIN WorkOrders W ON W.WOId = WI.WOId
INNER JOIN Clients C ON C.ClientId = W.ClientId
INNER JOIN Tests T ON T.TestId = WI.TestId
WHERE WI.SampleStatus = 'InTransit'
      AND DATEDIFF(HOUR, W.WODate, GETDATE()) > 48
ORDER BY HoursElapsed DESC;
GO


-- Query 4
-- Stored procedure for client ledger.

CREATE OR ALTER PROCEDURE GetClientLedger
    @ClientId INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Ledger AS
    (
        SELECT
            I.InvoiceDate AS LedgerDate,
            CONCAT('Invoice #', I.InvoiceId) AS Description,
            I.TotalAmount AS Debit,
            CAST(0 AS DECIMAL(18,2)) AS Credit,
            1 AS SortOrder,
            I.InvoiceId AS TransactionId
        FROM Invoices I
        WHERE I.ClientId = @ClientId

        UNION ALL

        SELECT
            P.PaymentDate AS LedgerDate,
            CONCAT('Payment #', P.PaymentId) AS Description,
            CAST(0 AS DECIMAL(18,2)) AS Debit,
            P.Amount AS Credit,
            2 AS SortOrder,
            P.PaymentId AS TransactionId
        FROM Payments P
        INNER JOIN Invoices I ON I.InvoiceId = P.InvoiceId
        WHERE I.ClientId = @ClientId

        UNION ALL

        SELECT
            P.PaymentDate AS LedgerDate,
            CONCAT('Gateway Fee #', P.PaymentId) AS Description,
            P.GatewayFee AS Debit,
            CAST(0 AS DECIMAL(18,2)) AS Credit,
            3 AS SortOrder,
            P.PaymentId AS TransactionId
        FROM Payments P
        INNER JOIN Invoices I ON I.InvoiceId = P.InvoiceId
        WHERE I.ClientId = @ClientId
          AND P.GatewayFee > 0
    )
    SELECT
        LedgerDate AS [Date],
        Description,
        Debit,
        Credit,
        SUM(Debit - Credit) OVER
        (
            ORDER BY LedgerDate, SortOrder, TransactionId
            ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
        ) AS RunningBalance
    FROM Ledger
    ORDER BY LedgerDate, SortOrder, TransactionId;
END;
GO

-- Example:
-- EXEC GetClientLedger 1;


-- Query 5
-- Flag clients who have exceeded their CreditLimit.

;WITH PendingInvoices AS
(
    SELECT I.ClientId, SUM(I.TotalAmount - ISNULL(P.PaidAmount, 0)) AS PendingInvoiceAmount
    FROM Invoices I
    OUTER APPLY
    (
        SELECT SUM(Amount) AS PaidAmount
        FROM Payments
        WHERE InvoiceId = I.InvoiceId
    ) P
    WHERE I.Status = 'Pending'
    GROUP BY I.ClientId
),
InTransitOrders AS
(
    SELECT W.ClientId, SUM(WI.Amount) AS InTransitAmount
    FROM WorkOrders W
    INNER JOIN WorkOrderItems WI ON WI.WOId = W.WOId
    WHERE WI.SampleStatus = 'InTransit'
      AND NOT EXISTS
      (
          SELECT 1
          FROM Invoices I
          WHERE I.WOId = W.WOId
      )
    GROUP BY W.ClientId
)
SELECT C.ClientName, C.CreditLimit,
       ISNULL(P.PendingInvoiceAmount, 0) + ISNULL(T.InTransitAmount, 0) AS CurrentOutstanding,
       ISNULL(P.PendingInvoiceAmount, 0) + ISNULL(T.InTransitAmount, 0) - C.CreditLimit AS ExcessAmount
FROM Clients C
LEFT JOIN PendingInvoices P ON P.ClientId = C.ClientId
LEFT JOIN InTransitOrders T ON T.ClientId = C.ClientId
WHERE (ISNULL(P.PendingInvoiceAmount, 0) + ISNULL(T.InTransitAmount, 0)) > C.CreditLimit
ORDER BY ExcessAmount DESC;