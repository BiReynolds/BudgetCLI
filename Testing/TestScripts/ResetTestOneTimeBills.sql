DELETE FROM OneTimeBills
WHERE Name LIKE '%test%';

INSERT INTO OneTimeBills (Name, Amount, DueDate, IsPaid)
VALUES
('test Rent', 1000.00, '2026-09-01', false),
('test Utilities', 300.00, '2026-09-05', true),
('test Groceries', 100.00, '2026-09-03', false),
('test subscription', 50.00, '2026-09-04', true)
;