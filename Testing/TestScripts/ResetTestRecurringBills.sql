DELETE FROM RecurringBills
WHERE Name like '%test%';

INSERT INTO RecurringBills (Name, Amount, StartDate, EndDate, RecurringType, ReferenceDate)
VALUES
('test recurring groceries', 200.00, '2026/09/07', null, 0, '2026/09/07'),
('test recurring pay', 2000.00, '2026/09/12', '2027/09/07', 1, '2026/09/12'),
('test recurring rent', 1000.00, '2026/09/01', null, 2, '2026/09/01');