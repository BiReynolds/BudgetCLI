BEGIN TRANSACTION;

CREATE TABLE RecurringBills (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Amount REAL NOT NULL,
    StartDate TEXT NOT NULL,
    EndDate TEXT,
    RecurringType INTEGER NOT NULL,
    ReferenceDate TEXT NOT NULL
);

-- worth noting that we could add the ParentId column as a Foreign Key, but sqlite does not support addition of foreign keys to existing tables
-- also, not all one time bills will have a parent id, which makes the foreign key constraint a bit harder to maintain

ALTER TABLE OneTimeBills ADD ParentId INTEGER;

UPDATE AppInfo
SET InfoValue = '0.2'
WHERE InfoKey = 'DatabaseVersion';

COMMIT;