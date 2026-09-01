# Budget CLI
## Current Behavior
- Add One-Time Bill
    - Syntax: `add bill '(name)' (amount) (dueDate)`
- Delete One-Time Bill by Name or Id
    - Syntax: `delete bill (id | name)`
- Pull specific bill info
    - Syntax: `show bill (id)`
- Pull specific bill info by name
    - Syntax: `show bill '(name)'`
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
- Add Recurring Bill (and corresponding one-time instances)
- Delete Recurring Bill (and all one-time instances)
- Edit Recurring Bill (and all one-time instances)
- Pull List of Recurring Bills ("Budget Summary")

## Upcoming
### Goal Functionality
- Add Projection functionality
- Add Projection Summary functionality
- Add "Startup Wizard."  On startup, program will...
    - Show bills which have come due since last open and allow user to mark paid or keep unpaid
    - Show a "Dashboard" featuring upcoming Projection (next week, for example) and Projection Summary


### Future 
- Open to suggestions!