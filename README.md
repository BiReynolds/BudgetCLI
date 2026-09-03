# Budget CLI
## Current Behavior
- Add One-Time Bill
    - Syntax: `add bill (name) (amount) (dueDate)`
- Delete One-Time Bill by Name or Id
    - Syntax: `delete bill (id | name)`
- Pull specific bill info
    - Syntax: `show bill (id)`
- Pull specific bill info by name
    - Syntax: `show bill (name)`
- Pull List of One-Time Bills
    - Syntax: `show bills` (by default, filters to show only unpaid bills)
    - Allows `[ unpaid | paid ]` modifier
    - Allows `duedate [ = | < | > | <= | >= ] (date)` modifier
    - Allows `name contains [queryString]` and `name = [queryString]` modifier
    - Allows `amount [ = | < | > | <= | >= ] (amount)` modifier
    - Allows multiple query clauses in same query
- Mark bill paid by name or id
    - Syntax: `paid (name)`
    - Syntax: `paid (id)`
- Mark bill unpaid by name or id
    - Syntax: `unpaid (name)`
    - Syntax: `unpaid (id)`
- Edit existing bill by name or id
    - Syntax: `edit bill (name | id) (field) (newValue)`
- Add Query logic for `show` command

## In Progress
### Basic Functionality
- [x] Pull List of Recurring Bills ("show recurring")

- [x] Edit RecurringBillModel to add a "ReferenceDate" field.  This will be what the system uses to determine future date eligibility (ex: a weekly bill will compare the ReferenceDate's day of week to a future date's day of week to see if the future date needs an instance).  By default, this will match the StartDate
    - The idea in keeping this separate is to make sure that the StartDate never changes for a bill, even though we may want to change what day of the week a bill comes due on, for example
- [x] Edit RecurringBillModel to allow detection of changes (like the OneTimeBillModel )

- [x] Startup job which will add new occurrences of recurring bills on app open, if needed
    - [x] Add jobs table (might be overkill, but will be useful if we have other jobs we want to run later)
    - [x] Add column `RecurringBills.LastOneTimeDueDateAdded` which will keep track of the date of the last instance of each recurring bill which was added to the db
    - [x] On startup, check each recurring bill's LastOneTimeDueDateAdded to see if another instance should exist between LastOneTimeDueDateAdded and today + 1 year.  If so, add it (or them, if multiple are needed)

- [x] Add Recurring Bill (and corresponding one-time instances)
    - [x] Syntax: add bill (name) (amount) (firstDue) (recurringType) (optional: endDate)
    - [x] Adds record to RecurringBills table
    - [x] Adds a record to OneTimeBills table for each occurrence 
        - [x] Will add records up to 1 year in advance, or less if endDate demands it

- [ ] Delete Recurring Bill (and all one-time instances)
    - [ ] Syntax: delete recurring (name)
    - [ ] Deletes record from RecurringBills table
    - [ ] Deletes all **unpaid** instances of this bill from OneTimeBills table (will keep the paid instances since we will likely implement a bill history in a future phase)

- [ ] Edit Recurring Bill (and all one-time instances)
    - [ ] Syntax: edit recurring (name) (field) (newValue)
    - [ ] Edits record in RecurringBills table
    - [ ] Will need to edit / add / remove instances of this bill from the OneTimeBills table as needed.  
        - [ ] If EndDate changes, just need to check if there are any instances after the new EndDate and remove them 
        - [ ] If Name / Amount changes, just need to 
        - [ ] If ReferenceDate changes, need to check if instances need to be "scooted" (this allows you to move a bill from every monday to every tuesday, for example)
        - [ ] If Recurring Type changes... may just be best to delete / re-add the instances tbh

## Upcoming
### Goal Functionality
- Add Projection functionality
- Add Projection Summary functionality
- Add "Startup Wizard."  On startup, program will...
    - Show bills which have come due since last open and allow user to mark paid or keep unpaid
    - Show a "Dashboard" featuring upcoming Projection (next week, for example) and Projection Summary


### Future 
- Open to suggestions!