CREATE TABLE OneTimeBills (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    DueDate TEXT NOT NULL,
    IsPaid INTEGER NOT NULL DEFAULT 0
);

UPDATE AppInfo
SET InfoValue = '0.1'
WHERE InfoKey = 'DatabaseVersion';

INSERT INTO AppInfo (InfoKey, InfoValue) VALUES
('LastUpdated', CURRENT_DATE),
('LastOpened', CURRENT_DATE);