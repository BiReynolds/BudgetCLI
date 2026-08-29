DELETE FROM OneTimeBills
WHERE Name LIKE '%test%';

INSERT INTO OneTimeBills (Name, Amount, DueDate)
VALUES
('test Rent', 1000.00, '2026-09-01'),
('test Utilities', 300.00, '2026-09-05'),
('test Groceries', 100.00, '2026-09-03')
;