BEGIN TRANSACTION;

CREATE TEMP TABLE RecurringBillInstancesToFix AS
SELECT OTB.Id as OneTimeBillId, OTB.DueDate as OneTimeBillDueDate, RB.Name as RecurringBillName
FROM OneTimeBills OTB
JOIN RecurringBills RB on RB.Id = OTB.ParentId
WHERE OTB.ParentId is not null;

UPDATE OneTimeBills
SET Name = (
    SELECT concat(RecurringBillName, ' ', strftime('%Y/%m/%d', OneTimeBillDueDate))
    FROM RecurringBillInstancesToFix
    WHERE OneTimeBillId = OneTimeBills.Id
)
WHERE EXISTS (
    SELECT OneTimeBillId
    FROM RecurringBillInstancesToFix
    WHERE OneTimeBillId = OneTimeBills.Id
);

UPDATE AppInfo
SET InfoValue = '0.3.1'
WHERE InfoKey = 'DatabaseVersion';

COMMIT TRANSACTION;