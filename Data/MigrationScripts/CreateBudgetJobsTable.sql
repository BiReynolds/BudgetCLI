BEGIN TRANSACTION;

CREATE TABLE BudgetJobs (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT,
    LastRunDate TEXT
);

INSERT INTO BudgetJobs (Name, Description)
VALUES (
    'AddNewRecurringBillInstances', 
    'Checks each active recurring bill and adds new instances if necessary'
);

UPDATE AppInfo
SET InfoValue = '0.3'
WHERE InfoKey = 'DatabaseVersion';

COMMIT;