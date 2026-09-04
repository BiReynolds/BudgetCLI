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
- Pull List of Recurring Bills ("show recurring")

- Add Recurring Bill (and corresponding one-time instances)
    - Syntax: `add bill (name) (amount) (firstDue) (recurringType) (optional: endDate)`

- Delete Recurring Bill (and all one-time instances)
    - Syntax: `delete recurring (name)`

## In Progress
- [ ] Add Projection functionality
    - [/] `projection (amount)` will pull the next month of bills and the projected balance on each day
    - [ ] `projection (amount) (number)` will display the next (number) months of bills and the projected balance on each day
- [ ] Add Projection Summary functionality
    - [ ] `projection summary` will display a few key figures from the next month of bills
        - [ ] Displays the lowest balance within the next 30, 60, 90 days
        - [ ] Displays the dates corresponding to the above lowest balances
        - [ ] Displays the bills which are coming out on the day of the lowest balances
- [ ] Add "Startup Wizard."  On startup, program will...
    - Show bills which have come due since last open and allow user to mark paid or keep unpaid
    - Show a "Dashboard" featuring upcoming Projection (next week, for example) and Projection Summary

## Upcoming
- [ ] Edit Recurring Bill (and all one-time instances)
    - [ ] Syntax: edit recurring (name) (field) (newValue)
    - [ ] Edits record in RecurringBills table
    - [ ] Will need to edit / add / remove instances of this bill from the OneTimeBills table as needed.  
        - [ ] If EndDate changes, just need to check if there are any instances after the new EndDate and remove them 
        - [ ] If Name / Amount changes, just need to update the Name / Amount for each instance
        - [ ] If ReferenceDate changes, need to check if instances need to be "scooted" (this allows you to move a bill from every monday to every tuesday, for example)
        - [ ] If Recurring Type changes... may just be best to delete / re-add the instances tbh


### Future 
- Open to suggestions!