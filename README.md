# Budget CLI
## Current Behavior

## In Progress
### Minimal Functionality
- Add One-Time Bill
    - [x] Syntax: `add bill '[name]' [amount] [dueDate]`
- Delete One-Time Bill by Name
    - [x] Syntax: `delete bill [id]`
- Pull specific bill info
    - [x] Syntax: `show bill [id]`
- Pull List of One-Time Bills
    - [ ] Syntax: `show bills`

## Upcoming
### Minimal Functionality
- Mark bill paid
- Edit existing bill
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