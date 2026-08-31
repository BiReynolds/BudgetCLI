# Budget CLI
## Current Behavior
- Add One-Time Bill
    - Syntax: `add bill '[name]' [amount] [dueDate]`
- Delete One-Time Bill by Name
    - Syntax: `delete bill [id]`
- Pull specific bill info
    - Syntax: `show bill [id]`
- Pull List of One-Time Bills
    - Syntax: `show bills`
- Pull specific bill info by name
    - Syntax: `show bill '[name]'`
- Mark bill paid by name or id
    - Syntax: `paid '[name]'`
    - Syntax: `paid [id]`
- Mark bill unpaid by name or id
    - Syntax: `unpaid '[name]'`
    - Syntax: `unpaid [id]`
- Edit existing bill by name or id
    - [x] Syntax: `edit [name or id] [field] [newValue]`
        - [x] Add necessary field tokens as subcommands (Name, Amount, DueDate)

## In Progress
### Minimal Functionality
- Add Query logic for `show` command
    - [ ] Allow `unpaid` modifier
    - [ ] Allow `due < before | after |  > < today | [date] >`
    - [ ] Allow `name contains '[queryString]'`
    - [ ] Allow `amount < <= | >= > [amount]`
    - [ ] Allow multiple query clauses in same query
## Upcoming
### Technical / Backend changes
- Refactor BasicEvaluator so that any command which takes a bill as argument uses the GetShownBillFromArgs method (including those in ShowCommandHelper)
### Basic Functionality
- Add Recurring Bill (and corresponding one-time instances)
- Delete Recurring Bill (and all one-time instances)
- Edit Recurring Bill (and all one-time instances)
- Pull List of Recurring Bills ("Budget Summary")
### Goal Functionality
- Add Projection functionality
- Add Projection Summary functionality
- Add "Startup Wizard."  On startup, program will...
    - Show bills which have come due since last open and allow user to mark paid or keep unpaid
    - Show a "Dashboard" featuring upcoming Projection (next week, for example) and Projection Summary


### Future 
- Open to suggestions!