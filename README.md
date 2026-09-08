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

- Pull List of Recurring Bills ("show recurring")
- Add Recurring Bill (and corresponding one-time instances)
    - Syntax: `add bill (name) (amount) (firstDue) (recurringType) (optional: endDate)`
- Delete Recurring Bill (and all one-time instances)
    - Syntax: `delete recurring (name)`
- Edit Recurring Bill (and all **unpaid** one-time instances)
    - Syntax: `edit recurring (name) (field) (newValue)`
    - Currently supports (field) = 'name', 'amount', 'duedate', 'enddate'
        - Changing the name of a recurring bill will also change the name of the corresponding unpaid one-time bills 
        - Changing the amount of a recurring bill also changes the amount of the corresponding unpaid one-time bills
        - Changing the next due date of a recurring bill will find the earliest unpaid instance of the recurring bill, the amount it needs to move to get to the proposed next due date, and shifts all the corresponding unpaid one-time bills by that amound.  This method also updates the name of the instances, since the due date is part of the name
        - Changing the end date of a recurring bill will also delete any instances which exist after the new end date, add new instances which need to be added (within the 12 month lookahead period), and update the LastOneTimeDueDateAdded field for the recurring bill accordingly
- Projection functionality
    - Syntax: `projection (optional: numMonths)`
        - Shows a table of the next numMonths months of dates, the projected balance on each of those days, and bills coming out on each of those days
        - If numMonths is not provided, will default to 1 month
    - Syntax: `projection summary`
        - Shows a small table with 3 rows: "Next Month", "1 - 2 Months", and "2 - 3 Months"
        - Each row will display the date of the lowest balance in the respective range, the lowest balance value, and the bills which are coming out on the day of the lowest balance

- "Startup Wizard."  
    - On startup, program will
    - Show a "Dashboard" featuring upcoming Projection (next week, for example) and Projection Summary based on user input amount

## In Progress
- Other Wizards
    - [ ] 'Add Bill' Wizard (will cover both one time bills and recurring bills)
    - [ ] 'Delete Bill' confirmation
    - [ ] 'Delete Recurring' confirmation
    - [ ] 'Edit Bill' Wizard
    - [ ] 'Edit Recurring' Wizard

## Upcoming
- [ ] Help functionality
    - [ ] `help` command
    - [ ] `help (reserved word)` commands

### Future 
- Open to suggestions!